using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record GetAllProductsQuery : IRequest<List<ProductResponse>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public GetAllProductsHandler(IProductRepository products, ICategoryRepository categories)
    {
        _products   = products;
        _categories = categories;
    }

    public async Task<List<ProductResponse>> Handle(GetAllProductsQuery _, CancellationToken ct)
    {
        var products = await _products.GetAllAsync(ct);
        var cats     = (await _categories.GetAllAsync(ct)).ToDictionary(c => c.Id, c => c.Name);
        return products
            .Select(p => p.ToResponse(cats.GetValueOrDefault(p.CategoryId, "Sin categoría")))
            .ToList();
    }
}
