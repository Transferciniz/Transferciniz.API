using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferciniz.API.Commands.ExtraServiceCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class ExtraServiceController: ControllerBase
{
    private readonly IMediator _mediator;

    public ExtraServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ExtraService> Create(CreateExtraServiceCommand request) => await _mediator.Send(request);
}