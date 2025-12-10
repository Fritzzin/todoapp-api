namespace TodoApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITodoRepository Todos { get; }
    
    Task<int> CommitAsync();
}