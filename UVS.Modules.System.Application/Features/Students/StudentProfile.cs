using UVS.Domain.Students;
using UVS.Modules.System.Application.Profiles;

namespace UVS.Modules.System.Application.Features.Students;

public class StudentProfile:AppProfile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentResponse>();
    }
}