using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public GetProductByIdHandler(IProductRepository products, ICategoryRepository categories)
    {
        _products   = products;
        _categories = categories;
    }

    public async Task<ProductResponse?> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(query.Id, ct);
        if (product is null) return null;
        var category = await _categories.GetByIdAsync(product.CategoryId, ct);
        return product.ToResponse(category?.Name ?? "Sin categoría");
    }
}
