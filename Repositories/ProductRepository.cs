using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsAsync(string? search, int page, int pageSize)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.SKU.Contains(search));
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<int> GetTotalCountAsync(string? search)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.SKU.Contains(search));
        }

        return await query.CountAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
