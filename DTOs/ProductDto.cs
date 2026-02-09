namespace InventoryApi.DTOs;

public record ProductDto(int Id, string SKU, string Name, string Description, decimal Price);
