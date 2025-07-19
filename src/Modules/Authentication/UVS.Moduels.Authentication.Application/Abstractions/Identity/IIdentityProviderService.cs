using UVS.Common.Domain;

namespace UVS.Modules.Authentication.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
    
    Task<Result> ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default);
    
    Task<Result<string>> LoginAsync(LoginModel model, CancellationToken cancellationToken = default); 
    
    Task<Result<string[]>> GetRolesAsync(Guid userId ,CancellationToken cancellationToken = default);
    
    Task<Result<string>> CreateRoleAsync(RoleModel role, CancellationToken cancellationToken = default);
    
    Task<Result<string>> GetRoleIdAsync(string roleName, CancellationToken cancellationToken = default);
    
    Task<Result> AssignRoleAsync(string roleName,string identityId , CancellationToken cancellationToken = default);

}
