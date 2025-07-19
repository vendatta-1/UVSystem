using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.System.Application.Features.Semesters.CreateSemester;
using UVS.Modules.System.Application.Features.Semesters.GetSemester;

namespace UVS.Modules.System.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class SemesterController(ISender sender) : UVSController
{

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateSemesterCommand command)
    {
        var result = await sender.Send(command);
        return result.Match(Results.Ok, ApiResults.Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IResult> Get(Guid id)
    {
        var result = await sender.Send(new GetSemesterQuery(id));
        
        return result.Match(Results.Ok, ApiResults.Problem);
    }
}