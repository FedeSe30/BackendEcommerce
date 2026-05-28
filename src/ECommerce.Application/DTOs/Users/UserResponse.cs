namespace ECommerce.Application.DTOs.Users;

public record UserResponse(
    Guid Id,
    string Email,
    string Name,
    string Role,
    DateTime CreatedAt
);
