using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserResponse?>;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly IUserRepository _users;

    public GetUserByIdHandler(IUserRepository users) => _users = users;

    public async Task<UserResponse?> Handle(GetUserByIdQuery query, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(query.Id, ct);
        return user is null ? null : new UserResponse(user.Id, user.Email, user.Name, user.Role, user.CreatedAt);
    }
}
