using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.Authentication.Application.Identity.CreateRole;
using UVS.Modules.Authentication.Application.Users.Login;
using UVS.Modules.Authentication.Application.Users.Register;

namespace UVS.Modules.Authentication.Presentation.Users;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "manage-users")]
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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);
        
        return result.Match(Results.Ok, ApiResults.Problem);
    }
    
}