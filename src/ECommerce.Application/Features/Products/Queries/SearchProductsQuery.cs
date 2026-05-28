using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record SearchProductsQuery(string Term) : IRequest<List<ProductResponse>>;

public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, List<ProductResponse>>
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public SearchProductsHandler(IProductRepository products, ICategoryRepository categories)
    {
        _products   = products;
        _categories = categories;
    }

    public async Task<List<ProductResponse>> Handle(SearchProductsQuery query, CancellationToken ct)
    {
        var products = await _products.SearchByNameAsync(query.Term, ct);
        var cats     = (await _categories.GetAllAsync(ct)).ToDictionary(c => c.Id, c => c.Name);
        return products
            .Select(p => p.ToResponse(cats.GetValueOrDefault(p.CategoryId, "Sin categoría")))
            .ToList();
    }
}
