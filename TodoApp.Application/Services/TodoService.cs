using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly IUnitOfWork _unitOfWork;
    private ITodoRepository _todos;

    public TodoService(IUnitOfWork unitOfWork, ITodoRepository todos )
    {
        _unitOfWork = unitOfWork;
        _todos = todos;
    }

    public async Task<Guid> CreateTodoAsync(string title, string? description)
    {
        var newTodo = new Todo(title, description);
        _todos.Add(newTodo);
        await _unitOfWork.CommitAsync();
        return newTodo.Id;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await _todos.FindAll();
    }

    public async Task<Todo?> GetByIdAsync(Guid guid)
    {
        return await _todos.FindById(guid);
    }

    public async Task<bool> UpdateTodoAsync(Guid id, string title, string? description)
    {
        var todoToUpdate = await _todos.FindById(id);

        if (todoToUpdate == null)
        {
            return false;
        }

        todoToUpdate.UpdateInfo(title, description);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var todoToDelete = await _todos.FindById(id);

        if (todoToDelete == null)
        {
            return false;
        }

        _todos.DeleteOne(todoToDelete);
        await _unitOfWork.CommitAsync();

        return true;
    }
}