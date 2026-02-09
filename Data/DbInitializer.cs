using InventoryApi.Models;

namespace InventoryApi.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<AppDbContext>>();

        for (int i = 0; i < 5; i++)
        {
            try
            {
                // EnsureCreated is enough for this demo, in prod use Migrations
                await context.Database.EnsureCreatedAsync();

                if (await context.Products.AnyAsync())
                {
                    return;   // DB has been seeded
                }

                await SeedDataAsync(context);
                return;
            }
            catch (Exception ex)
            {
                if (i == 4)
                {
                    logger.LogError(ex, "Failed to seed DB after 5 attempts.");
                    throw;
                }
                logger.LogWarning($"DB Initialization failed ({ex.Message}). Retrying in 3 seconds...");
                await Task.Delay(3000);
            }
        }
    }

    private static async Task SeedDataAsync(AppDbContext context)
    {
        var products = new Product[]
        {
            new Product { Name = "Laptop", SKU = "TECH-001", Description = "High performance laptop", Price = 1200.00m },
            new Product { Name = "Mouse", SKU = "TECH-002", Description = "Wireless mouse", Price = 25.00m },
            new Product { Name = "Keyboard", SKU = "TECH-003", Description = "Mechanical keyboard", Price = 80.00m },
            new Product { Name = "Monitor", SKU = "TECH-004", Description = "27 inch 4K monitor", Price = 350.00m },
            new Product { Name = "Desk Chair", SKU = "FURN-001", Description = "Ergonomic desk chair", Price = 150.00m }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        var transactions = new List<StockTransaction>();
        foreach (var p in products)
        {
            transactions.Add(new StockTransaction 
            { 
                ProductId = p.Id, 
                QuantityChange = 10, 
                Type = TransactionType.Restock, 
                CreatedAt = DateTime.UtcNow 
            });
        }

        await context.StockTransactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }
}
