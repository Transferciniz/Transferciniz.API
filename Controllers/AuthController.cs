using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<LoginCommandResponse> Login(LoginCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<UploadProfilePictureCommandResponse> ChangeProfilePicture(UploadProfilePictureCommand request) => await _mediator.Send(request);
}