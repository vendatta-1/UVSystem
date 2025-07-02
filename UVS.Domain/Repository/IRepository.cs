using System.Linq.Expressions;
using UVS.Domain.Common;

namespace UVS.Domain.Repository;

public interface IRepository<T> 
    where T : Entity
{
    Task<Result<Guid>> CreateAsync(T entity);
    
    Task<Result> UpdateAsync(T entity);
    
    Task<Result> DeleteAsync(Guid id);
    
    Task<Result> DeleteAsync(Expression<Func<T, bool>> predicate);
    
    Task<Result<IReadOnlyCollection<T>>> GetAllAsync();
    
    Task<Result<IReadOnlyCollection<T>>> GetAllAsync(Expression<Func<T, bool>> predicate);
    
    Task<Result<T?>> GetByIdAsync(Guid id);
    Task<Result<T?>> GetAsync(Expression<Func<T, bool>> predicate);
    
    Task<Result> ExistsAsync(Expression<Func<T, bool>> predicate);
    
    Task<Result<int>> CountAsync(Expression<Func<T, bool>> predicate);
    
    Task<PagedResult<T>> GetPagedAsync(int page = 1, int pageSize = 20);
    
    Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int page, int pageSize);

    Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate,Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int page, int pageSize);


}