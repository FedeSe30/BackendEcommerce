namespace ECommerce.Application.DTOs.Users;

public record UpdateUserRequest(
    string Name,
    string? Role
);
