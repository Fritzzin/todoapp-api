using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;

namespace TodoApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoint(this WebApplication app)
    {
        var authGroup = app.MapGroup("/login").WithTags("Authentication");
        authGroup.MapPost("/", async (LoginRequestDto request, ITokenService tokenService) =>
        {
            var tokenString = await tokenService.AuthenticateAsync(request.Email, request.Password);

            if (tokenString is null)
            {
                var failureResponse =
                    ResponseViewModel.Failure("Erro de Autenticacao", StatusCodes.Status401Unauthorized);
                return Results.Json(failureResponse, statusCode: failureResponse.StatusCode);
            }

            var successResponse = ResponseViewModel.Success(new { Token = tokenString });
            return Results.Ok(successResponse);
        });
    }
}