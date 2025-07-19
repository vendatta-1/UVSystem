using UVS.Common.Application.Caching;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Semesters;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Semesters.CreateSemester;

public sealed class CreateSemesterCommandHandler(ISemesterRepository repository,ICacheService cacheService,IUnitOfWork unitOfWork)
:ICommandHandler<CreateSemesterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSemesterCommand request, CancellationToken cancellationToken)
    {
         var exist = await repository.ExistsAsync(se=>se.Name.ToLower() == request.Name.ToLower() );
         if (exist)
         {
             return Result.Failure<Guid>(Error.Conflict("Semester.Conflict",$"the semester with the name {request.Name} already exists."));
         }

         var semester = Semester.Create
         (
             request.Name,
             request.StartDate,
             request.EndDate,
             request.DepartmentId,
             request.AcademicYear,
             request.IsCurrent
         );
        var semesterId = await repository.CreateAsync(semester);

      
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return semesterId;
    }
}