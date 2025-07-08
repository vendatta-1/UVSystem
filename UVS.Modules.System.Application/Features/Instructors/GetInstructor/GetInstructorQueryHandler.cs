 
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Instructors;

namespace UVS.Modules.System.Application.Features.Instructors.GetInstructor;

public sealed class GetInstructorQueryHandler(IInstructorRepository repository) : IQueryHandler<GetInstructorQuery,InstructorResponse>
{
    public async Task<Result<InstructorResponse>> Handle(GetInstructorQuery request, CancellationToken cancellationToken)
    {
        var instructor = await repository.GetAsync(x => x.Id == request.InstructorId);

        if (instructor == null)
        {
            return Result.Failure<InstructorResponse>(Error.NotFound("Instructor.NotFound",
                $"there is non instructor has this id {request.InstructorId}"));
        }

        return Result.Success(new InstructorResponse(instructor.FullName, instructor.Email,
            instructor.DepartmentId));
        
    }
}