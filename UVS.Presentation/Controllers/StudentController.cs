using MediatR;
using Microsoft.AspNetCore.Mvc;
using UVS.Application.Students.GetStudents;

namespace UVS.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(ISender sender):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetStudentsQuery();
        var result = await sender.Send(query);
        return Ok(result);
    }
}