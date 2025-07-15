using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.System.Application.Features.Departments.CreateDepartment;
using UVS.Modules.System.Application.Features.Departments.GetDepartment;
using UVS.Modules.System.Application.Features.Departments.SetDepartmentHead;

namespace UVS.Modules.System.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController(ISender sender): UVSController
{
    [HttpGet("{id:guid}")]
    public async Task<IResult> Get(Guid id)
    {
        var getQuery = new GetDepartmentQuery(id);
        var result = await sender.Send(getQuery);
        return result.Match(Results.Ok , ApiResults.Problem);
    }

    [HttpPost]
    public async Task<IResult> Create(CreateDepartmentCommand command)
    {
        var result = await sender.Send(command);
        return result.Match(Results.Ok , ApiResults.Problem);
    }

    [HttpPut]
    public async Task<IResult> SetHead(SetDepartmentHeadCommand command)
    {
        var result = await sender.Send(command);
        return result.Match(Results.NoContent, ApiResults.Problem);
    }
    
    
}