using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Users.Commands;

public record CreateUserCommand(string Email, string Name, string Password) : IRequest<UserResponse>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IUnitOfWork     _uow;

    public CreateUserHandler(IUserRepository users, IPasswordHasher hasher, IUnitOfWork uow)
    {
        _users  = users;
        _hasher = hasher;
        _uow    = uow;
    }

    public async Task<UserResponse> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        if (await _users.GetByEmailAsync(cmd.Email, ct) is not null)
            throw new DomainException("Ya existe un usuario con ese email.");

        var user = new User(cmd.Email, cmd.Name, _hasher.Hash(cmd.Password));
        await _users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return new UserResponse(user.Id, user.Email, user.Name, user.Role, user.CreatedAt);
    }
}
