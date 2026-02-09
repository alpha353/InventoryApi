using InventoryApi.Models;

namespace InventoryApi.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        // Look for any products.
        if (context.Products.Any())
        {
            return;   // DB has been seeded
        }

        var products = new Product[]
        {
            new Product { Name = "Laptop", SKU = "TECH-001", Description = "High performance laptop", Price = 1200.00m },
            new Product { Name = "Mouse", SKU = "TECH-002", Description = "Wireless mouse", Price = 25.00m },
            new Product { Name = "Keyboard", SKU = "TECH-003", Description = "Mechanical keyboard", Price = 80.00m },
            new Product { Name = "Monitor", SKU = "TECH-004", Description = "27 inch 4K monitor", Price = 350.00m },
            new Product { Name = "Desk Chair", SKU = "FURN-001", Description = "Ergonomic desk chair", Price = 150.00m }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
        
        // Initial stock transactions for seeded products
        var transactions = new List<StockTransaction>();
        foreach(var p in products)
        {
             transactions.Add(new StockTransaction 
             { 
                 ProductId = p.Id, 
                 QuantityChange = 10, 
                 Type = TransactionType.Restock, 
                 CreatedAt = DateTime.UtcNow 
             });
        }
        
        context.StockTransactions.AddRange(transactions);
        context.SaveChanges();
    }
}
