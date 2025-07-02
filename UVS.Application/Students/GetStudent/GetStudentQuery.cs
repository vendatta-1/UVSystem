using UVS.Application.Messaging;

namespace UVS.Application.Students.GetStudent;

public record GetStudentQuery(Guid StudentId):IQuery<StudentResponse>;