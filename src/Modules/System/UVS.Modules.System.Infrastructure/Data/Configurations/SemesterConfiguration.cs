using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UVS.Domain.Semesters;

namespace UVS.Modules.System.Infrastructure.Data.Configurations;

public class SemesterConfiguration : IEntityTypeConfiguration<SemesterCourse>
{
    public void Configure(EntityTypeBuilder<SemesterCourse> builder)
    {
        builder.HasKey(x =>new {x.CourseId, x.SemesterId});
        
        builder.HasOne(x=>x.Semester).WithMany(x=>x.SemesterCourses).HasForeignKey(x=>x.SemesterId);
        
        builder.HasOne(x=>x.Course)
            .WithMany(x=>x.SemesterCourses).HasForeignKey(x=>x.CourseId);
        
    }
}