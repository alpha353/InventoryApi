using InventoryApi.Models;

namespace InventoryApi.DTOs;

public record StockAdjustmentDto(int ProductId, int QuantityChange, TransactionType Type);
