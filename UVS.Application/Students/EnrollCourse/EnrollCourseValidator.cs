using FluentValidation;

namespace UVS.Application.Students.EnrollCourse;

public class EnrollCourseValidator:AbstractValidator<EnrollCourseCommand>
{
    public EnrollCourseValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
    }
}