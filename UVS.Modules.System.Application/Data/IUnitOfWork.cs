namespace UVS.Modules.System.Application.Data;

public interface IUnitOfWork
{
   Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);   
}