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
         var exist = await repository.ExistsAsync(se=>string.Equals(se.Name, request.Name, StringComparison.InvariantCultureIgnoreCase));
         if (exist.IsSuccess)
         {
             throw new ApplicationException($"Semester with name {request.Name} already exists");
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
        var result = await repository.CreateAsync(semester);

        if (result.IsSuccess)
        {
           await unitOfWork.SaveChangesAsync(cancellationToken);
           return result.Value;
        }
        throw new ApplicationException($"Failed to create semester");
    }
}