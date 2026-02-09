using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApi.Models;

public enum TransactionType
{
    Adjustment,
    Sale,
    Restock,
    Return
}

public class StockTransaction
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public int QuantityChange { get; set; }

    public TransactionType Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
