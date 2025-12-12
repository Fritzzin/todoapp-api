using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Api.Filters;

namespace TodoApp.Api.Endpoints;

public static class TodosEndpoints
{
    public static void AddEndpointsTodos(this WebApplication app)
    {
        app.MapPost("/todos", async (CreateTodoRequestDto request, ITodoService todoService) =>
            {
                var newTodoId = await todoService.CreateTodoAsync(request.Title, request.Description);
                return Results.Created($"/todos/{newTodoId}",
                    ResponseViewModel.Success(newTodoId, StatusCodes.Status201Created));
            })
            .AddEndpointFilter<ValidationFilter<CreateTodoRequestDto>>()
            .WithDescription("Cria um novo item.");

        app.MapGet("/todos", async (ITodoService todoService) =>
            {
                var todos = await todoService.GetAllAsync();
                return Results.Ok(ResponseViewModel.Success(todos));
            })
            .WithDescription("Busca todos os items.");

        app.MapGet("/todos/{id}", async (Guid id, ITodoService todoService) =>
            {
                var result = await todoService.GetByIdAsync(id);

                if (result is null)
                {
                    return Results.NotFound(ResponseViewModel.Failure("Item nao  encontrado",
                        StatusCodes.Status404NotFound));
                }

                return Results.Ok(ResponseViewModel.Success(result));
            })
            .WithDescription("Busca um dos items baseado no ID");

        app.MapPut("/todos/{id}", async (Guid id, UpdateTodoRequestDto request, ITodoService todoService) =>
            {
                var isUpdated = await todoService.UpdateTodoAsync(id, request.Title, request.Description);

                if (!isUpdated)
                {
                    return Results.NotFound(ResponseViewModel.Failure($"Falha ao editar {id}",
                        StatusCodes.Status404NotFound));
                }

                return Results.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateTodoRequestDto>>()
            .WithDescription("Atualiza item com novas informações.");

        app.MapPut("/todos/{id}/done", async (Guid id, ITodoService todoService) =>
        {
            var isMarked = await todoService.UpdateTodoAsDoneAsync(id);
            if (!isMarked)
            {
                return Results.NotFound(ResponseViewModel.Failure($"Falha ao marcar {id} como feito!",
                    StatusCodes.Status404NotFound));
            }

            return Results.NoContent();
        }).WithDescription("Atualiza item como feito.");
        
        app.MapPut("/todos/{id}/undone", async (Guid id, ITodoService todoService) =>
        {
            var isMarked = await todoService.UpdateTodoAsUndoneAsync(id);
            if (!isMarked)
            {
                return Results.NotFound(ResponseViewModel.Failure($"Falha ao marcar {id} como não feito!",
                    StatusCodes.Status404NotFound));
            }

            return Results.NoContent();
        }).WithDescription("Atualiza item como não feito.");

        

        app.MapDelete("/todos/{id}", async (Guid id, ITodoService todoService) =>
            {
                var isDeleted = await todoService.DeleteTodoAsync(id);

                if (!isDeleted)
                {
                    return Results.NotFound(ResponseViewModel.Failure($"Falha ao deletar {id}",
                        StatusCodes.Status404NotFound));
                }

                return Results.NoContent();
            })
            .WithDescription("Remove item.");
    }
}