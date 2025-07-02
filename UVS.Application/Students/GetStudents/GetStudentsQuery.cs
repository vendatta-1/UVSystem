using UVS.Application.Messaging;

namespace UVS.Application.Students.GetStudents;

public sealed record GetStudentsQuery:IQuery<IReadOnlyList<StudentResponse>> ;