namespace ECommerce.Application.DTOs.Orders;

public record AddOrderItemRequest(
    Guid ProductId,
    int Quantity
);
