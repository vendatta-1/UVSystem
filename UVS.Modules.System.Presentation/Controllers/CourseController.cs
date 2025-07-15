using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.System.Application.Features.Courses.AddCourseSchedule;
using UVS.Modules.System.Application.Features.Courses.CreateCourse;
using UVS.Modules.System.Application.Features.Courses.GetCourse;

namespace UVS.Modules.System.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(ISender sender):UVSController
{
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetCourse(Guid id)
    {
        var query = new GetCourseQuery(id);
        var result =await sender.Send(query);
        return result.Match(Results.Ok, ApiResults.Problem);
    }

    [HttpPost]
    public async Task<IResult> CreateCourse(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);
        
        return result.Match(Results.Ok, ApiResults.Problem);
    }

    [HttpPut]
    public async Task<IResult> AddSchedule(AddScheduleCommand request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);
        
        return result.Match(Results.NoContent, ApiResults.Problem);
        
    }
    
}