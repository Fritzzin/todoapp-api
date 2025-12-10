using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface ITodoRepository
{
    void Add(Todo todo);
    Task<Todo?> FindById(Guid id);
    Task<IEnumerable<Todo>> FindAll();
    void DeleteOne(Todo todo);
}