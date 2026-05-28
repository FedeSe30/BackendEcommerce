using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappers;

internal static class ProductMapper
{
    internal static ProductResponse ToResponse(this Product p, string categoryName) =>
        new(p.Id, p.Name, p.Description, p.Price, p.Stock, p.CategoryId, categoryName, p.CreatedAt);
}
