using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands;

public record CreateOrderCommand(Guid UserId, List<AddOrderItemRequest> Items) : IRequest<OrderResponse>;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository   _orders;
    private readonly IProductRepository _products;
    private readonly IUserRepository    _users;
    private readonly IUnitOfWork        _uow;

    public CreateOrderHandler(IOrderRepository orders, IProductRepository products, IUserRepository users, IUnitOfWork uow)
    {
        _orders   = orders;
        _products = products;
        _users    = users;
        _uow      = uow;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand cmd, CancellationToken ct)
    {
        if (await _users.GetByIdAsync(cmd.UserId, ct) is null)
            throw new DomainException("Usuario no encontrado.");

        var productMap = new Dictionary<Guid, Product>();
        foreach (var item in cmd.Items)
        {
            if (productMap.ContainsKey(item.ProductId)) continue;
            var product = await _products.GetByIdAsync(item.ProductId, ct);
            if (product is null)
                throw new DomainException($"Producto '{item.ProductId}' no encontrado.");
            productMap[item.ProductId] = product;
        }

        var order = new Order(cmd.UserId);
        foreach (var item in cmd.Items)
            order.AddItem(productMap[item.ProductId], item.Quantity);

        await _orders.AddAsync(order, ct);
        await _uow.SaveChangesAsync(ct);

        var names = productMap.ToDictionary(kv => kv.Key, kv => kv.Value.Name);
        return ToResponse(order, names);
    }

    internal static OrderResponse ToResponse(Order o, Dictionary<Guid, string> names) =>
        new(o.Id, o.UserId, o.CreatedAt, o.Status, o.Total,
            o.Items.Select(i => new OrderItemResponse(
                i.Id, i.ProductId,
                names.GetValueOrDefault(i.ProductId, ""),
                i.UnitPrice, i.Quantity, i.Subtotal)).ToList());
}
