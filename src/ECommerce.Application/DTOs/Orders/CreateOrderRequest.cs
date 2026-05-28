namespace ECommerce.Application.DTOs.Orders;

public record CreateOrderRequest(
    Guid UserId,
    List<AddOrderItemRequest> Items
);
