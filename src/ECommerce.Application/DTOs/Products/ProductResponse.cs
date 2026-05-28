namespace ECommerce.Application.DTOs.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId,
    string CategoryName,
    DateTime CreatedAt
);
