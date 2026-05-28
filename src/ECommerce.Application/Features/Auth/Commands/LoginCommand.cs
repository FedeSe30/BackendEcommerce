using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<string?>;

public class LoginHandler : IRequestHandler<LoginCommand, string?>
{
    private readonly IUserRepository _users;
    private readonly ITokenService   _tokenService;
    private readonly IPasswordHasher _hasher;

    public LoginHandler(IUserRepository users, ITokenService tokenService, IPasswordHasher hasher)
    {
        _users        = users;
        _tokenService = tokenService;
        _hasher       = hasher;
    }

    public async Task<string?> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(cmd.Email, ct);
        if (user is null) return null;

        if (!_hasher.Verify(cmd.Password, user.PasswordHash)) return null;

        return _tokenService.GenerateToken(user);
    }
}
