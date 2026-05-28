using MediatR;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Features.Auth.Commands;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var token = await _mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        if (token is null)
            return Unauthorized(new { message = "Credenciales incorrectas." });

        return Ok(new { token });
    }
}
