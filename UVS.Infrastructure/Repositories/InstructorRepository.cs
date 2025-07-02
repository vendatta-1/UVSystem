using Microsoft.Extensions.Logging;
using UVS.Domain.Instructors;
using UVS.Infrastructure.Data;

namespace UVS.Infrastructure.Repositories;

internal sealed class InstructorRepository(UVSDbContext context, ILogger<InstructorRepository> logger)  :
    Repository<Instructor>(context, logger),IInstructorRepository
{
    
}