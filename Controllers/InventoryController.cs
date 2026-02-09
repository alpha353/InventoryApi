using FluentValidation;
using InventoryApi.DTOs;
using InventoryApi.Models;
using InventoryApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(
    IProductRepository productRepository, 
    IInventoryRepository inventoryRepository,
    IValidator<StockAdjustmentDto> validator) : ControllerBase
{
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var products = await productRepository.GetProductsAsync(search, page, pageSize);
        var totalCount = await productRepository.GetTotalCountAsync(search);

        var response = new
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = products.Select(p => new ProductDto(p.Id, p.SKU, p.Name, p.Description, p.Price))
        };

        return Ok(response);
    }

    [HttpPost("adjust")]
    public async Task<IActionResult> AdjustStock([FromBody] StockAdjustmentDto adjustment)
    {
        var validationResult = await validator.ValidateAsync(adjustment);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var product = await productRepository.GetProductByIdAsync(adjustment.ProductId);
        if (product == null)
        {
            return NotFound($"Product with ID {adjustment.ProductId} not found.");
        }

        // Business Logic: Check Insufficient Stock for Sales
        if (adjustment.QuantityChange < 0)
        {
            var currentStock = await inventoryRepository.GetStockLevelAsync(adjustment.ProductId);
            if (currentStock + adjustment.QuantityChange < 0)
            {
                return BadRequest("Insufficient stock for this transaction.");
            }
        }

        var transaction = new StockTransaction
        {
            ProductId = adjustment.ProductId,
            QuantityChange = adjustment.QuantityChange,
            Type = adjustment.Type,
            CreatedAt = DateTime.UtcNow
        };

        await inventoryRepository.AddTransactionAsync(transaction);
        await inventoryRepository.SaveChangesAsync();

        return Ok(new { Message = "Stock adjusted successfully.", NewStockLevel = await inventoryRepository.GetStockLevelAsync(adjustment.ProductId) });
    }

    [HttpGet("{id}/level")]
    public async Task<IActionResult> GetStockLevel(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound($"Product with ID {id} not found.");
        }

        var stockLevel = await inventoryRepository.GetStockLevelAsync(id);
        return Ok(new { ProductId = id, StockLevel = stockLevel });
    }
}
