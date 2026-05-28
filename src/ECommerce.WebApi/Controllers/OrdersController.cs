using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Application.Features.Orders.Queries;
using MediatR;

namespace ECommerce.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _mediator.Send(new GetOrdersByUserQuery(userId), ct));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetOrdersByUserQuery(userId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateOrderCommand(request.UserId, request.Items), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
