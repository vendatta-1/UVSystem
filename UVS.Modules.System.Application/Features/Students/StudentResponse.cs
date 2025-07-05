namespace UVS.Modules.System.Application.Features.Students;

public record StudentResponse
{ 
    public string FullName { get; set; } 
    public string NationalId { get; set; }
    
    public string Email { get; set; } 
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Gender { get; set; }

    public Guid DepartmentId { get; set; } 
}