using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.Authentication.Application.Users.CreateRole;
using UVS.Modules.Authentication.Application.Users.Register;

namespace UVS.Modules.Authentication.Presentation.Users;

[ApiController]
[Route("api/[controller]")]
public class UserController(ISender sender) : UVSController
{

    [HttpPost]
    public async Task<IResult> CreateUser([FromBody] RegisterUserCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(Results.Ok, ApiResults.Problem);
    }

    [HttpPost("role")]
    public async Task<IResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        var result = await sender.Send(command);
        
        return result.Match(Results.Ok, ApiResults.Problem);
    }
}