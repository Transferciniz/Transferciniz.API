using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.AuthCommands;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize]
public class UserController: ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    [HttpPost]
    public async Task<RegisterUserCommandResponse> Register(RegisterUserCommand request) => await _mediator.Send(request);
}