using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Api.Filters;

namespace TodoApp.Api.Endpoints;

public static class TodosEndpointsIResult
{
    public static void AddEndpointsTodosIResults(this WebApplication app)
    {
        app.MapPost("/todos", async (CreateTodoRequestDto request, ITodoService todoService) =>
            {
                var newTodoId = await todoService.CreateTodoAsync(request.Title, request.Description);
                return ResponseViewModel.Success(newTodoId, StatusCodes.Status201Created);
            })
            .AddEndpointFilter<ValidationFilter<CreateTodoRequestDto>>()
            .WithDescription("Creates a Todo");

        app.MapGet("/todos", async (ITodoService todoService) =>
            {
                var todos = await todoService.GetAllAsync();
                return ResponseViewModel.Success(todos);
            })
            .WithDescription("Gets all todos");

        app.MapGet("/todos/{id}", async (Guid id, ITodoService todoService) =>
            {
                var result = await todoService.GetByIdAsync(id);

                if (result is null)
                {
                    return (IResult)ResponseViewModel.Failure("Resultado não encontrado",
                        StatusCodes.Status404NotFound);
                }


                return (IResult)ResponseViewModel.Success(result);
            })
            .WithDescription("Get one of the todos based on ID");

        app.MapPut("/todos/{id}", async (Guid id, UpdateTodoRequestDto request, ITodoService todoService) =>
            {
                var isUpdated = await todoService.UpdateTodoAsync(id, request.Title, request.Description);

                if (!isUpdated)
                {
                    return ResponseViewModel.Failure($"Erro ao atualizar {id}", StatusCodes.Status404NotFound);
                }

                return ResponseViewModel.Success(204);
            })
            .AddEndpointFilter<ValidationFilter<UpdateTodoRequestDto>>()
            .WithDescription("Updates one of the todos based on ID");

        app.MapDelete("/todos/{id}", async (Guid id, ITodoService todoService) =>
            {
                var isDeleted = await todoService.DeleteTodoAsync(id);

                if (!isDeleted)
                {
                    return ResponseViewModel.Failure("Falha ao deletar Todo", StatusCodes.Status404NotFound);
                }

                return ResponseViewModel.Success();
            })
            .WithDescription("Deletes one of the todos based on ID");
    }
}