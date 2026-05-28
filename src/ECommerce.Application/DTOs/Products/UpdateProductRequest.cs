namespace ECommerce.Application.DTOs.Products;

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId
);
