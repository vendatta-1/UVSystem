
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Students.GetStudents;

public sealed record GetStudentsQuery:IQuery<IReadOnlyList<StudentResponse>> ;