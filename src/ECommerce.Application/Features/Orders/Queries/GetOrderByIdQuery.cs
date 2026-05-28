using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse?>;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    private readonly IOrderRepository   _orders;
    private readonly IProductRepository _products;

    public GetOrderByIdHandler(IOrderRepository orders, IProductRepository products)
    {
        _orders   = orders;
        _products = products;
    }

    public async Task<OrderResponse?> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await _orders.GetByIdWithItemsAsync(query.Id, ct);
        if (order is null) return null;

        var names = new Dictionary<Guid, string>();
        foreach (var item in order.Items)
        {
            if (!names.ContainsKey(item.ProductId))
            {
                var p = await _products.GetByIdAsync(item.ProductId, ct);
                names[item.ProductId] = p?.Name ?? "";
            }
        }

        return CreateOrderHandler.ToResponse(order, names);
    }
}
