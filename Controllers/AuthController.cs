using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.UserCommands;
using Transferciniz.API.Entities;
using Transferciniz.API.Services;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IS3Service _s3Service;

    public AuthController(IMediator mediator, IS3Service s3Service)
    {
        _mediator = mediator;
        _s3Service = s3Service;
    }


    [HttpPost]
    public async Task<User> RegisterCustomer(RegisterCustomerCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<User> RegisterDriver(RegisterDriverCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<LoginCommandResponse> Login(LoginCommand request) => await _mediator.Send(request);

    [HttpPost]
    public async Task<string> Upload(IFormFile request) => await _s3Service.UploadFileToSpacesAsync(request);
}