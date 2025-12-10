using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Infrastructure.Data.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _dbContext;

    public TodoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Todo todo)
    {
        _dbContext.Todos.Add(todo);
    }

    public async Task<Todo?> FindById(Guid id)
    {
        var todo = await _dbContext.Todos.FindAsync(id);
        // Não necessário estourar exceção. Nao é responsabilidade do repository. Retornar null
        return todo ?? null;
    }

    public async Task<IEnumerable<Todo>> FindAll()
    {
        return await _dbContext.Todos.ToListAsync();
    }

    public void DeleteOne(Todo todo)
    {
        _dbContext.Todos.Remove(todo);
    }
}