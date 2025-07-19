
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Instructors.GetInstructor;

public sealed record GetInstructorQuery(Guid InstructorId):IQuery<InstructorResponse>
{ 
    
}