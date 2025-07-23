using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UVS.Common.Application.Clock;
using UVS.Domain.Common;
using UVS.Domain.Students;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class StudentRepository(UVSDbContext context,ILogger<StudentRepository> logger, IDateTimeProvider dateTimeProvider) :
    Repository<Student>(context, logger, dateTimeProvider),
    IStudentRepository
{
    private readonly UVSDbContext _context = context;
    
}