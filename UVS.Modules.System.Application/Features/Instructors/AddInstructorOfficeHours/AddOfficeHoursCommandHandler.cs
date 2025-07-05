using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Instructors;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Instructors.AddInstructorOfficeHours;

public sealed class AddOfficeHoursCommandHandler(IInstructorRepository repository, IUnitOfWork unitOfWork) : 
    ICommandHandler<AddOfficeHourCommand>
{
    public async Task<Result> Handle(AddOfficeHourCommand request, CancellationToken cancellationToken)
    {
        var officeHour = new OfficeHour
        (
            request.Day,
            request.StartTime,
            request.EndTime
        );

        var result = await repository.GetByIdAsync(request.InstructorId);
        if (result.IsFailure)
        {
            return Result.Failure(Error.NotFound("Instructor.NotFound",$"there is non instructor has this id {request.InstructorId}"));
            
        }

        var instructor = result.Value;
        
        instructor.AddOfficeHour(officeHour);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();

    }
}