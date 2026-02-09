using InventoryApi.Models;

namespace InventoryApi.Repositories;

public interface IInventoryRepository
{
    Task<int> GetStockLevelAsync(int productId);
    Task AddTransactionAsync(StockTransaction transaction);
    Task SaveChangesAsync();
}
