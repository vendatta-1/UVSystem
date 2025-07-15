using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.System.Application.Features.Students.CreateStudent;
using UVS.Modules.System.Application.Features.Students.EnrollCourse;
using UVS.Modules.System.Application.Features.Students.GetStudent;
using UVS.Modules.System.Application.Features.Students.GetStudents;

namespace UVS.Modules.System.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(ISender sender) : UVSController
{
    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var query = new GetStudentsQuery();
        var result = await sender.Send(query);
        return result.Match(Results.Ok , ApiResults.Problem);
    }

    [HttpPost]
    public async Task<IResult> CreateStudent(CreateStudentCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(Results.Ok , ApiResults.Problem);
    }

    [HttpPost("[action]")]
    public async Task<IResult> EnrollCourse(EnrollCourseCommand command)
    {
        var result = await sender.Send(command);
        return result.Match(Results.Created , ApiResults.Problem);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetStudent(Guid id)
    {
        var query = new GetStudentQuery(id);
        var result = await sender.Send(query);
        return result.Match(Results.Ok , ApiResults.Problem);
    }
}