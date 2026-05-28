using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries;

public record GetOrdersByUserQuery(Guid UserId) : IRequest<List<OrderResponse>>;

public class GetOrdersByUserHandler : IRequestHandler<GetOrdersByUserQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orders;

    public GetOrdersByUserHandler(IOrderRepository orders) => _orders = orders;

    public async Task<List<OrderResponse>> Handle(GetOrdersByUserQuery query, CancellationToken ct)
    {
        var orders = await _orders.GetByUserIdAsync(query.UserId, ct);
        return orders
            .Select(o => CreateOrderHandler.ToResponse(o, new Dictionary<Guid, string>()))
            .ToList();
    }
}
