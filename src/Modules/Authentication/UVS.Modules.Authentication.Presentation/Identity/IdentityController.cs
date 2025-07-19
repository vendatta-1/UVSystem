using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using UVS.Common.Presentation.Endpoints;
using UVS.Common.Presentation.Results;
using UVS.Modules.Authentication.Application.Identity.GetRoleId;

namespace UVS.Modules.Authentication.Presentation.Identity;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "manage-users")]
public sealed class IdentityController(ISender sender):UVSController
{
    [HttpGet]
    [Route("[controller]/[action]/{roleName:required}")]
    [Authorize(Roles = "manage-users")]
    public async Task<IResult> GetRoleId(string roleName)
    {
        var result = await sender.Send(new GetRoleIdCommand(roleName));
        return result.Match(Results.Ok, ApiResults.Problem);
    }
}