using FluentValidation;
using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Validators;

public class StockAdjustmentValidator : AbstractValidator<StockAdjustmentDto>
{
    public StockAdjustmentValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.QuantityChange).NotEqual(0).WithMessage("Quantity change cannot be zero.");
        RuleFor(x => x.Type).IsInEnum();
        
        RuleFor(x => x).Must(x => 
            (x.Type == TransactionType.Sale && x.QuantityChange < 0) ||
            (x.Type == TransactionType.Restock && x.QuantityChange > 0) ||
            (x.Type == TransactionType.Return && x.QuantityChange > 0) ||
            (x.Type == TransactionType.Adjustment))
            .WithMessage("Quantity change sign must match transaction type logic (e.g. Sale must be negative).");
    }
}
