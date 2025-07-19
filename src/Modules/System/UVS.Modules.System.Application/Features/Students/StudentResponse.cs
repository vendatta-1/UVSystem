namespace UVS.Modules.System.Application.Features.Students;

public record StudentResponse
{ 
    public string FirstName { get; init; } 
    
    public string LastName { get; init; }
    
    public string NationalId { get; init; }
    
    public string Email { get; init; } 
    
    public string Phone { get; init; }
    
    public DateTime DateOfBirth { get; init; } 
    
    public string Gender { get; init; }
    
    public Guid DepartmentId { get; init; } 
}