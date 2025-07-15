using System.Net;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Identity;

namespace UVS.Authentication.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger) : IIdentityProviderService
{
    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [new CredentialRepresentation("Password",user.Password, false)]);

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
        }
    }
    public async Task<Result<string>> GetRoleIdAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await keyCloakClient.GetRoleIdAsync(roleName, cancellationToken);
            return id;
        }
        catch (Exception e)
        { 
            logger.LogError(e, "GetRoleIdAsync exception");
            return Result.Failure<string>(new Error("Role.NotFound", e.Message, ErrorType.NotFound));
        }   
    }
    public async Task<Result<string>> CreateRoleAsync(RoleModel role, CancellationToken cancellationToken = default)
    
    {
        var roleRepresentation = new CreateRoleRepresentation(role.Name, role.Description);
        try
        {
           var httpResponse = await keyCloakClient.CreateRoleAsync(new CreateRoleRepresentation(role.Name, role.Description), cancellationToken);
           return httpResponse;
        }   
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure<string>(new Error("Role.Create",e.Message, ErrorType.Failure));
        }
         
    }
    public Task<Result> ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> LogoutAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string[]>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


    
}

