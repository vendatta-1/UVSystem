using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UVS.Domain.Common;
using UVS.Domain.Students;
using UVS.Infrastructure.Data;

namespace UVS.Infrastructure.Repositories;

internal sealed class StudentRepository(UVSDbContext context,ILogger<StudentRepository> logger):
    Repository<Student>(context, logger),
    IStudentRepository
{
    private readonly UVSDbContext _context = context;
    
}