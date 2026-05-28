using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.Orders;

public record OrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    OrderStatus Status,
    decimal Total,
    List<OrderItemResponse> Items
);
