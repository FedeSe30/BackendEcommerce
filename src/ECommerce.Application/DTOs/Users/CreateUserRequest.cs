namespace ECommerce.Application.DTOs.Users;

public record CreateUserRequest(
    string Email,
    string Name,
    string Password
);
