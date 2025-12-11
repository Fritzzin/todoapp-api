using FluentValidation;
using TodoApp.Api.Dtos;

namespace TodoApp.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Tenta encontrar o argumento do tipo T (nosso DTO) nos parâmetros do endpoint
        var argument = context.Arguments.FirstOrDefault(argument => argument?.GetType() == typeof(T));

        if (argument is null)
        {
            return Results.Problem("Could not find argument of type " + typeof(T).Name);
        }

        // Valida o argumento
        var validationResult = await _validator.ValidateAsync((T)argument);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            var errorResponse = new ResponseViewModel<object>(errorMessages);
            return Results.BadRequest(errorResponse);

            // Se for inválido, retorna um 400 Bad Request com os erros
            // return Results.ValidationProblem(validationResult.ToDictionary());
        }

        // Se for válido, continua para o próximo filtro ou para o handler do endpoint
        return await next(context);
    }
}