using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands;

public record CreateCategoryCommand(string Name) : IRequest<CategoryResponse>;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categories;
    private readonly IUnitOfWork         _uow;

    public CreateCategoryHandler(ICategoryRepository categories, IUnitOfWork uow)
    {
        _categories = categories;
        _uow        = uow;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand cmd, CancellationToken ct)
    {
        var category = new Category(cmd.Name);
        await _categories.AddAsync(category, ct);
        await _uow.SaveChangesAsync(ct);
        return new CategoryResponse(category.Id, category.Name);
    }
}
