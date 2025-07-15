namespace UVS.Authentication.Domain.Users;

public sealed class Role
{
    public static readonly Role Admin = new Role("Admin");
    public static readonly Role Student = new Role("Student");
    public static readonly Role Instructor = new Role("Instructor");
    public static readonly Role Head = new Role("Head");

    public Role(){}
    private Role(string name)
    {
        Name = name;
    }
    public string Name { get;  set; }
}