using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public record GetAllCategoriesQuery : IRequest<List<CategoryResponse>>;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categories;

    public GetAllCategoriesHandler(ICategoryRepository categories) => _categories = categories;

    public async Task<List<CategoryResponse>> Handle(GetAllCategoriesQuery _, CancellationToken ct)
    {
        var categories = await _categories.GetAllAsync(ct);
        return categories.Select(c => new CategoryResponse(c.Id, c.Name)).ToList();
    }
}
