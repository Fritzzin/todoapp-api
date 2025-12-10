using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Api.Filters;

namespace TodoApp.Api.Endpoints;

public static class TodosEndpoints
{
    public static void MapTodosEndpoints(this WebApplication app)
    {
        app.MapPost("/todos", async (CreateTodoRequestDto request, ITodoService todoService) =>
        {
            var newTodoId = await todoService.CreateTodoAsync(request.Title, request.Description);

            return Results.Created($"/todos/{newTodoId}", newTodoId);
        }).AddEndpointFilter<ValidationFilter<CreateTodoRequestDto>>();

        app.MapGet("/todos", async (ITodoService todoService) => await todoService.GetAllAsync());

        app.MapGet("/todos/{id}", async (Guid id, ITodoService todoService) =>
        {
            var result = await todoService.GetByIdAsync(id);

            return result == null ? Results.NotFound() : Results.Json(result);
        });

        app.MapPut("/todos/{id}", async (Guid id, UpdateTodoRequestDto request, ITodoService todoService) =>
        {
            var isUpdated = await todoService.UpdateTodoAsync(id, request.title, request.description);

            return isUpdated ? Results.NoContent() : Results.NotFound();
        });

        app.MapDelete("/todos/{id}", async (Guid id, ITodoService todoService) =>
        {
            var isDeleted = await todoService.DeleteTodoAsync(id);

            return isDeleted ? Results.NoContent() : Results.NotFound();
        });
    }
}