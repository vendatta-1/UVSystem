using Microsoft.Extensions.Logging;
using UVS.Domain.Instructors;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class InstructorRepository(UVSDbContext context, ILogger<InstructorRepository> logger)  :
    Repository<Instructor>(context, logger),IInstructorRepository
{
    
}