using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces;

public interface ITodoService
{
    Task<Guid> CreateTodoAsync(string title, string? description);
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(Guid id);
    Task<bool> UpdateTodoAsync(Guid id, string title, string? description);
    Task<bool> DeleteTodoAsync(Guid id);
}