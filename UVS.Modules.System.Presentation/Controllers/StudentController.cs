using MediatR;
using Microsoft.AspNetCore.Mvc;
using UVS.Modules.System.Application.Features.Students.CreateStudent;
using UVS.Modules.System.Application.Features.Students.GetStudents;
using UVS.Presentation.Abstractions;

namespace UVS.Modules.System.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(ISender sender) : SystemController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetStudentsQuery();
        var result = await sender.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentCommand command)
    {
        var result  = await sender.Send(command);
        return Ok(result);
    }
}