using InventoryApi.Models;

namespace InventoryApi.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(string? search, int page, int pageSize);
    Task<Product?> GetProductByIdAsync(int id);
    Task<int> GetTotalCountAsync(string? search);
    Task AddProductAsync(Product product);
    Task SaveChangesAsync();
}
