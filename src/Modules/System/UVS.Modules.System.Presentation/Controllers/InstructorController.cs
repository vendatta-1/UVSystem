using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.System.Application.Features.Instructors.AddInstructorOfficeHours;
using UVS.Modules.System.Application.Features.Instructors.CreateInstructor;
using UVS.Modules.System.Application.Features.Instructors.GetInstructor;

namespace UVS.Modules.System.Presentation.Controllers;


[ApiController]
[Route("api/instructor")]
[Authorize]
public class InstructorController(ISender sender) : UVSController
{
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Head,Instructor")]
    public async Task<IResult> GetInstructors(Guid id, CancellationToken cancellationToken)
    {
        
        var query = new GetInstructorQuery(id);
        
        var result =await sender.Send(query, cancellationToken);
        
        return result.Match(Results.Ok, ApiResults.Problem);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Head")]
    public async Task<IResult> CreateInstructor([FromBody] CreateInstructorCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        
        return result.Match(Results.Ok, ApiResults.Problem);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Head,Instructor")]
    public async Task<IResult> AddOfficeHours([FromBody] AddOfficeHourCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        
        return result.Match(Results.NoContent, ApiResults.Problem);
    }
    
}