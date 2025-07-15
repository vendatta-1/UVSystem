using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UVS.Authentication.Infrastructure.Data;
using UVS.Common.Application.Clock;
using UVS.Common.Domain;

namespace UVS.Authentication.Infrastructure.Repositories;

internal class Repository<T>(AuthDbContext dbContext, ILogger<Repository<T>> logger, IDateTimeProvider dateTimeProvider)
    :IRepository<T> where T:AuditEntity
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();
    public virtual async Task<Guid> CreateAsync(T entity)
    { 
        var result =  await  _dbSet.AddAsync(entity);
        return result.State == EntityState.Added
            ? result.Entity.Id
            : throw new ApplicationException($"internal error while append {nameof(T)}");
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var result = _dbSet.Update(entity);
        return await Task.FromResult(result.State == EntityState.Modified);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new ApplicationException($"internal error while delete {nameof(T)} there is non entity has this id: {id}");
        }
        entity.DeletedAt = dateTimeProvider.UtcNow;

        var result = await UpdateAsync(entity);
        return  result ?true:
            throw new ApplicationException($"internal error while delete {nameof(T)}"); 
    }

    public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var entity  = await GetAsync(predicate);
        if (entity == null)
        {
            throw new ApplicationException($"internal error while delete {nameof(T)} there is non entity match predict");
        }
        entity.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(entity);
        return true;
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return  await Task.FromResult(_dbSet.AsNoTracking().ToList());
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await GetAsync(x => x.Id == id);
    }


    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).AnyAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
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