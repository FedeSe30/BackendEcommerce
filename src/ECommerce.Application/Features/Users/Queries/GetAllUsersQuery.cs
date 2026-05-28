using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<List<UserResponse>>;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
{
    private readonly IUserRepository _users;

    public GetAllUsersHandler(IUserRepository users) => _users = users;

    public async Task<List<UserResponse>> Handle(GetAllUsersQuery _, CancellationToken ct)
    {
        var users = await _users.GetAllAsync(ct);
        return users.Select(u => new UserResponse(u.Id, u.Email, u.Name, u.Role, u.CreatedAt)).ToList();
    }
}
