using UVS.Common.Domain;

namespace UVS.Modules.Authentication.Domain.Users;

public sealed class User : AuditEntity
{
    private readonly List<Role> _roles= [];
    private User(){}

    
    public  new Guid Id { get; init; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
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
        user.Raise(new UserRegisteredDomainEvent(user.Id,Guid.Parse(user.IdentityId)));
        return user;
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

}