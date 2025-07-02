using UVS.Application.Profiles;
using UVS.Domain.Students;

namespace UVS.Application.Students;

public class StudentProfile:AppProfile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentResponse>();
    }
}