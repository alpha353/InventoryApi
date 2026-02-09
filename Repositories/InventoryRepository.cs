using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repositories;

public class InventoryRepository(AppDbContext context) : IInventoryRepository
{
    public async Task<int> GetStockLevelAsync(int productId)
    {
        return await context.StockTransactions
            .Where(t => t.ProductId == productId)
            .SumAsync(t => t.QuantityChange);
    }

    public async Task AddTransactionAsync(StockTransaction transaction)
    {
        await context.StockTransactions.AddAsync(transaction);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
