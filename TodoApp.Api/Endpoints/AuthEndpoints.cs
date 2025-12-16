using System.Runtime.InteropServices.ComTypes;
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
                return Results.Json(
                    data: ResponseViewModel.Failure("Erro de Autenticação"),
                    statusCode: StatusCodes.Status401Unauthorized
                );
            }

            return Results.Json(
                data: ResponseViewModel.Success(tokenString),
                statusCode: StatusCodes.Status200OK
            );
        });
    }
}