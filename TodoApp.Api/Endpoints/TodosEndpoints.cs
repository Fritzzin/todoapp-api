using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Api.Filters;
using TodoApp.Domain.Entities;

namespace TodoApp.Api.Endpoints;

public static class TodosEndpoints
{
    public static void AddEndpointsTodos(this WebApplication app)
    {
        app.MapPost("/todos", async (CreateTodoRequestDto request, ITodoService todoService) =>
            {
                var newTodoId = await todoService.CreateTodoAsync(request.Title, request.Description);

                var response = new ResponseViewModel<Guid>(newTodoId);

                return Results.Created($"/todos/{newTodoId}", response);
            })
            .AddEndpointFilter<ValidationFilter<CreateTodoRequestDto>>()
            .WithDescription("Creates a Todo");

        app.MapGet("/todos", async (ITodoService todoService) =>
            {
                var todos = await todoService.GetAllAsync();
                var response = new ResponseViewModel<IEnumerable<Todo>>(todos);
                return Results.Ok(response);
            })
            .WithDescription("Gets all todos");

        app.MapGet("/todos/{id}", async (Guid id, ITodoService todoService) =>
            {
                var result = await todoService.GetByIdAsync(id);

                if (result is null)
                {
                    var responseFailure = new ResponseViewModel<Todo>("Resultado nao encontrado");
                    return Results.NotFound(responseFailure);
                }

                var responseSuccess = new ResponseViewModel<Todo>(result);
                return Results.Ok(responseSuccess);
            })
            .WithDescription("Get one of the todos based on ID");

        app.MapPut("/todos/{id}", async (Guid id, UpdateTodoRequestDto request, ITodoService todoService) =>
            {
                var isUpdated = await todoService.UpdateTodoAsync(id, request.Title, request.Description);

                if (!isUpdated)
                {
                    return Results.NotFound(new ResponseViewModel<bool>(false));
                }

                return Results.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateTodoRequestDto>>()
            .WithDescription("Updates one of the todos based on ID");

        app.MapDelete("/todos/{id}", async (Guid id, ITodoService todoService) =>
            {
                var isDeleted = await todoService.DeleteTodoAsync(id);

                if (!isDeleted)
                {
                    var responseFailure = ResponseViewModel.Failure("Falha ao deletar Todo", 404);
                    return Results.NotFound(responseFailure);
                }

                var responseSuccess = ResponseViewModel.Success(204);
                return Results.Ok(responseSuccess);
            })
            .WithDescription("Deletes one of the todos based on ID");
    }
}