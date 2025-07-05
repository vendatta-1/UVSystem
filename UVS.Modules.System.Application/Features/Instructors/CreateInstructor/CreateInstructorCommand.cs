 
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Instructors.CreateInstructor;

public sealed record CreateInstructorCommand(string FullName, string Email, Guid DepartmentId) : ICommand<Guid>
{ 

}