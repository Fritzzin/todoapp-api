using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Api.Filters;

namespace TodoApp.Api.Endpoints;

public static class TodosEndpoints
{
    public static void AddEndpointsTodos(this WebApplication app)
    {
        var todoGroup = app.MapGroup("/todos")
            .WithTags("Todos")
            .RequireAuthorization();


        todoGroup.MapPost("/", async (CreateTodoRequestDto request, ITodoService todoService) =>
            {
                var newTodoId = await todoService.CreateTodoAsync(request.Title, request.Description);

                return Results.Json(
                    data: ResponseViewModel.Success(newTodoId),
                    statusCode: StatusCodes.Status201Created
                );
            })
            .AddEndpointFilter<ValidationFilter<CreateTodoRequestDto>>()
            .WithDescription("Cria um novo item.");

        todoGroup.MapGet("/", async (ITodoService todoService) =>
            {
                var todos = await todoService.GetAllAsync();
                return Results.Json(
                    data: ResponseViewModel.Success(todos),
                    statusCode: StatusCodes.Status200OK
                );
            })
            .WithDescription("Busca todos os items.");

        todoGroup.MapGet("/{id}", async (Guid id, ITodoService todoService) =>
            {
                var result = await todoService.GetByIdAsync(id);

                if (result is null)
                {
                    return Results.Json(
                        data: ResponseViewModel.Failure("Item não encontrado"),
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                return Results.Json(
                    data: ResponseViewModel.Success(result),
                    statusCode: StatusCodes.Status200OK);
            })
            .WithDescription("Busca um dos items baseado no ID");

        todoGroup.MapPut("/{id}", async (Guid id, UpdateTodoRequestDto request, ITodoService todoService) =>
            {
                var isUpdated = await todoService.UpdateTodoAsync(id, request.Title, request.Description);

                if (!isUpdated)
                {
                    return Results.Json(
                        data: ResponseViewModel.Failure($"Falha ao editar {id}"),
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                return Results.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateTodoRequestDto>>()
            .WithDescription("Atualiza item com novas informações.");

        todoGroup.MapPut("/{id}/done", async (Guid id, ITodoService todoService) =>
            {
                var isMarked = await todoService.UpdateTodoAsDoneAsync(id);
                if (!isMarked)
                {
                    return Results.Json(
                        data: ResponseViewModel.Failure($"Falha ao marcar {id} como feito!"),
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                return Results.NoContent();
            })
            .WithDescription("Atualiza item como feito.");

        todoGroup.MapPut("/{id}/undone", async (Guid id, ITodoService todoService) =>
            {
                var isMarked = await todoService.UpdateTodoAsUndoneAsync(id);
                if (!isMarked)
                {
                    return Results.Json(
                        data: ResponseViewModel.Failure($"Falha ao marcar {id} como não feito!"),
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                return Results.NoContent();
            })
            .WithDescription("Atualiza item como não feito.");


        todoGroup.MapDelete("/{id}", async (Guid id, ITodoService todoService) =>
            {
                var isDeleted = await todoService.DeleteTodoAsync(id);

                if (!isDeleted)
                {
                    return Results.Json(
                        data: ResponseViewModel.Failure($"Falha ao deletar {id}!"),
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                return Results.NoContent();
            })
            .WithDescription("Remove item.");
    }
}