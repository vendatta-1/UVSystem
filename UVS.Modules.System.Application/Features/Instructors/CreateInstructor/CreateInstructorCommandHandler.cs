using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Instructors;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Instructors.CreateInstructor;

public sealed class CreateInstructorCommandHandler(IInstructorRepository instructorRepository, IUnitOfWork unitOfWork)
    :ICommandHandler<CreateInstructorCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = Instructor.Create(request.FullName, request.Email, request.DepartmentId);

        var result = await instructorRepository.CreateAsync(instructor);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(
                Error.Failure("Instructor.FaildCreat", "internal error while create instructor"));
        }

        return Result.Success<Guid>(result.Value);
    }
}