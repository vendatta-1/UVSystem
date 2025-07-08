using Dapper;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Courses;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Courses.GetCourse;

public sealed class GetCourseQueryHandler(IDbConnectionFactory dbConnectionFactory,ICourseRepository courseRepository):IQueryHandler<GetCourseQuery, CourseResponse>
{
    public async Task<Result<CourseResponse>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    { 
        var exists = await courseRepository.ExistsAsync(x=>x.Id == request.CourseId);

        if (!exists)
        {
            return Result.Failure<CourseResponse>(Error.NotFound("Course.NotFound",$"there is no course with id {request.CourseId}"));
        }

        string sql = $"""
                     
                     SELECT c.code AS {nameof(CourseResponse.Code)},
                            c.name AS {nameof(CourseResponse.Name)},
                            c.description AS {nameof(CourseResponse.Description)},
                            c.credit_hours AS {nameof(CourseResponse.CreditHours)},
                            c.department_id AS {nameof(CourseResponse.DepartmentId)},
                            c.instructor_id AS {nameof(CourseResponse.InstructorId)},
                            sc.semester_id AS {nameof(CourseResponse.SemesterId)},
                            FROM course as c
                            join semester_course as sc on sc.course_id = c.course_id
                            WHERE c.course_id = @courseId
                     """;

       await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
       
       var courseResponse = await dbConnection.QueryFirstOrDefaultAsync<CourseResponse>(sql, new {courseId = request.CourseId});


       return courseResponse ?? Result.Failure<CourseResponse>(Error.Problem("General.InternalServerError",
           $"while try to get course with id {request.CourseId} may be there is non semester is associated with this course"));

    }
}