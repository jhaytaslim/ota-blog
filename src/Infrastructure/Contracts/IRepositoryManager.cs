using Infrastructure.Contracts;

namespace Infrastructure.Contracts;
public interface IRepositoryManager
{
    Task SaveChangesAsync();
    Task BeginTransaction(Func<Task> action);
}

