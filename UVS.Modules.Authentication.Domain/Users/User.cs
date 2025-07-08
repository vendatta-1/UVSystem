using UVS.Common.Domain;

namespace UVS.Authentication.Domain.Users;

public sealed class User : Entity
{
    private readonly List<Role> _roles= [];
    private User(){}

    
    public  new Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string IdentityId { get; init; }
    public IReadOnlyCollection<Role> Roles => _roles;
    
    public static User Create(string firstName, string lastName, string email, string identityId,Role role)
    {
        var user = new User()
        {
            Id =  Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            IdentityId = identityId,
        };
        user._roles.Add(role);
        user.Raise(new UserRegisteredDomainEvent(user.Id));
        return user;
    }
    
}