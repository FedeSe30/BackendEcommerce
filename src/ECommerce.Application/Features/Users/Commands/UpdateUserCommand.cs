using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Users.Commands;

public record UpdateUserCommand(Guid Id, string Name, string? Role) : IRequest<UserResponse?>;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponse?>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork     _uow;

    public UpdateUserHandler(IUserRepository users, IUnitOfWork uow)
    {
        _users = users;
        _uow   = uow;
    }

    public async Task<UserResponse?> Handle(UpdateUserCommand cmd, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(cmd.Id, ct);
        if (user is null) return null;

        user.UpdateName(cmd.Name);
        if (cmd.Role is not null) user.UpdateRole(cmd.Role);
        await _users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return new UserResponse(user.Id, user.Email, user.Name, user.Role, user.CreatedAt);
    }
}
