using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest<bool>;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork     _uow;

    public DeleteUserHandler(IUserRepository users, IUnitOfWork uow)
    {
        _users = users;
        _uow   = uow;
    }

    public async Task<bool> Handle(DeleteUserCommand cmd, CancellationToken ct)
    {
        if (await _users.GetByIdAsync(cmd.Id, ct) is null) return false;
        await _users.DeleteAsync(cmd.Id, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
