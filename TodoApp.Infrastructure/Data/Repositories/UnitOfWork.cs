using TodoApp.Domain.Interfaces;

namespace TodoApp.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public ITodoRepository Todos { get; }
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        Todos = new TodoRepository(dbContext);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}