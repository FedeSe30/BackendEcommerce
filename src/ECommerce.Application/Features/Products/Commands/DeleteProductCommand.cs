using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _products;
    private readonly IUnitOfWork        _uow;

    public DeleteProductHandler(IProductRepository products, IUnitOfWork uow)
    {
        _products = products;
        _uow      = uow;
    }

    public async Task<bool> Handle(DeleteProductCommand cmd, CancellationToken ct)
    {
        if (!await _products.ExistsAsync(cmd.Id, ct)) return false;
        await _products.DeleteAsync(cmd.Id, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
