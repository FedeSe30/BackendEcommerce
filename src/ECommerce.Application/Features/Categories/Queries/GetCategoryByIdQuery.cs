using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryResponse?>;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse?>
{
    private readonly ICategoryRepository _categories;

    public GetCategoryByIdHandler(ICategoryRepository categories) => _categories = categories;

    public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery query, CancellationToken ct)
    {
        var category = await _categories.GetByIdAsync(query.Id, ct);
        return category is null ? null : new CategoryResponse(category.Id, category.Name);
    }
}
