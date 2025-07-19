 
using UVS.Common.Application.Caching;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Instructors;

namespace UVS.Modules.System.Application.Features.Instructors.GetInstructor;

public sealed class GetInstructorQueryHandler(IInstructorRepository repository, ICacheService cacheService) : IQueryHandler<GetInstructorQuery,InstructorResponse>
{
    public async Task<Result<InstructorResponse>> Handle(GetInstructorQuery request, CancellationToken cancellationToken)
    {
        var key = $"instructor-{request.InstructorId}";
        var cachedInstructor = await cacheService.GetAsync<InstructorResponse>(key, cancellationToken);
        if (cachedInstructor is not null)
        {
            return cachedInstructor;
        }
        var instructor = await repository.GetAsync(x => x.Id == request.InstructorId && x.DeletedAt == null);

        if (instructor == null)
        {
            return Result.Failure<InstructorResponse>(Error.NotFound("Instructor.NotFound",
                $"there is non instructor has this id {request.InstructorId}"));
        }

        await cacheService.SetAsync(key, instructor, cancellationToken: cancellationToken);

        return Result.Success(new InstructorResponse(instructor.FullName, instructor.Email,
            instructor.DepartmentId));
        
    }
}