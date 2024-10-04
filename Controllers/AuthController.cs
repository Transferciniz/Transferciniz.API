using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skybrud.Social.Google;
using Transferciniz.API.Commands.AccountCommands;
using Transferciniz.API.Commands.AuthCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }



    [HttpPost]
    [AllowAnonymous]
    public async Task<LoginCommandResponse> Login(LoginCommand request) => await _mediator.Send(request);

    [HttpPost]
    [AllowAnonymous]
    public async Task<RegisterCommandResponse> Register(RegisterCommand request) => await _mediator.Send(request);

   


}