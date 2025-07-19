 
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Students.GetStudent;

public sealed record GetStudentQuery(Guid StudentId):IQuery<StudentResponse>;