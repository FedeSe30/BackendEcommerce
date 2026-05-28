using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record UpdateProductCommand(
    Guid Id, string Name, string Description, decimal Price, int Stock, Guid CategoryId
) : IRequest<ProductResponse?>;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductResponse?>
{
    private readonly IProductRepository  _products;
    private readonly ICategoryRepository _categories;
    private readonly IUnitOfWork         _uow;

    public UpdateProductHandler(IProductRepository products, ICategoryRepository categories, IUnitOfWork uow)
    {
        _products   = products;
        _categories = categories;
        _uow        = uow;
    }

    public async Task<ProductResponse?> Handle(UpdateProductCommand cmd, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(cmd.Id, ct);
        if (product is null) return null;

        product.Update(cmd.Name, cmd.Description, cmd.Price, cmd.Stock, cmd.CategoryId);
        await _products.UpdateAsync(product, ct);
        await _uow.SaveChangesAsync(ct);

        var category = await _categories.GetByIdAsync(cmd.CategoryId, ct);
        return product.ToResponse(category?.Name ?? "Sin categoría");
    }
}
