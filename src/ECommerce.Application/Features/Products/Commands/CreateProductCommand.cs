using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record CreateProductCommand(
    string Name, string Description, decimal Price, int Stock, Guid CategoryId
) : IRequest<ProductResponse>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository  _products;
    private readonly ICategoryRepository _categories;
    private readonly IUnitOfWork         _uow;

    public CreateProductHandler(IProductRepository products, ICategoryRepository categories, IUnitOfWork uow)
    {
        _products   = products;
        _categories = categories;
        _uow        = uow;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = new Product(cmd.Name, cmd.Description, cmd.Price, cmd.Stock, cmd.CategoryId);
        await _products.AddAsync(product, ct);
        await _uow.SaveChangesAsync(ct);

        var category = await _categories.GetByIdAsync(cmd.CategoryId, ct);
        return product.ToResponse(category?.Name ?? "Sin categoría");
    }
}
