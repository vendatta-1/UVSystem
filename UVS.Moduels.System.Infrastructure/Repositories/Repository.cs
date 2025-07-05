using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Repository;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal class Repository<T>(UVSDbContext dbContext, ILogger<Repository<T>> logger):IRepository<T>
where T:AuditEntity
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();
    public async Task<Result<Guid>> CreateAsync(T entity)
    { 
        var result =  await  _dbSet.AddAsync(entity);
        return result.State == EntityState.Added
            ? result.Entity.Id
            : throw new ApplicationException($"internal error while append {nameof(T)}");
    }

    public async Task<Result> UpdateAsync(T entity)
    {
        var result = _dbSet.Update(entity);
        
        return await Task.FromResult(result.State == EntityState.Modified?Result.Success() :Result.Failure(error:Error.Failure("Update failed","Internal server error")) );
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        var result = _dbSet.Remove(entity.Value);
        return  result.State ==EntityState.Deleted?Result.Success() :Result.Failure(Error.Problem("Delete failed","Internal server error") ); 
    }

    public async Task<Result> DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var entity  = await GetAsync(predicate);
        entity.Value.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(entity.Value);
        return Result.Failure(Error.Failure("Delete failed","Check the expression may be hit no entity"));
    }

    public async Task<Result<IReadOnlyCollection<T>>> GetAllAsync()
    {
        return  await Task.FromResult(_dbSet.AsNoTracking().ToList());
    }

    public async Task<Result<IReadOnlyCollection<T>>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<Result<T?>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).FirstOrDefaultAsync();
        
    }

    public async Task<Result<T?>> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }
    

    public async Task<Result> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).AnyAsync()?Result.Success() :Result.Failure(Error.NotFound("Entity.NotFound","Entity not found"));
    }

    public async Task<Result<int>> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).CountAsync();
    }

    public async Task<PagedResult<T>> GetPagedAsync(int page = 1, int pageSize = 20)
    { 
        var entities = _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
        var count = await _dbSet.CountAsync();
        var pageResult = PagedResult<T>.Success(
            entities,
            count,
            pageSize,
            page);
        return entities.Any() ? pageResult : PagedResult<T>.Failure(Error.None);

    }

    public async Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int page =1, int pageSize=20)
    {
       var entites = await _dbSet.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
       var count = await _dbSet.CountAsync();
       return PagedResult<T>.Success(
           entites,
           count,
           pageSize, page);
    }

    public async Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>,IOrderedQueryable<T>> orderedBy, int page, int pageSize)
    {
        var query = _dbSet.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize);
        orderedBy(query);
        var count = await _dbSet.CountAsync();
        return PagedResult<T>.Success(
            query.ToArray(),
            count,
            pageSize,
            page);

    }
}