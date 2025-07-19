using System.Linq.Expressions;

namespace UVS.Common.Domain;

public interface IRepository<T> 
    where T : Entity
{
    Task<Guid> CreateAsync(T entity);
    
    Task<bool> UpdateAsync(T entity);
    
    Task<bool> DeleteAsync(Guid id);
    
    Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);
    
    Task<IReadOnlyCollection<T>> GetAllAsync();
    
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    
    Task<PagedResult<T>> GetPagedAsync(int page = 1, int pageSize = 20);
    
    Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int page, int pageSize);

    Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate,Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int page, int pageSize);


}