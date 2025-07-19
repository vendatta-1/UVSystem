 
using UVS.Common.Application.Messaging;
using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Students.CreateStudent;

public record CreateStudentCommand:ICommand<Guid>
{
    
    public string FullName { get; set; }
  
    public string NationalId { get; set; }
   
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string Gender { get; set; }
  
    public string DepartmentName { get; set; }
    
    public AcademicLevel Level { get; set; }

    public string City { get; set; } = null!;
    public string? Town { get; set; } 
    public string ZipCode { get; set; } = null!;
}