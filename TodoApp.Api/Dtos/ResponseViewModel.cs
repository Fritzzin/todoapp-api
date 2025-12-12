namespace TodoApp.Api.Dtos;

using Microsoft.AspNetCore.Http;
using System.Text.Json;

public class ResponseViewModel<T> : IResult
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public List<string>? Errors { get; private set; }

    public int StatusCode { get; private set; }

    public ResponseViewModel(int statusCode)
    {
        Success = true;
        Data = default;
        Errors = null;
        StatusCode = statusCode;
    }

    public ResponseViewModel(T data, int statusCode = 200)
    {
        Success = true;
        Data = data;
        Errors = null;
        StatusCode = statusCode;
    }

    public ResponseViewModel(List<string> errors, int statusCode = 400)
    {
        Success = false;
        Data = default;
        Errors = errors;
        StatusCode = statusCode;
    }

    public ResponseViewModel(string error, int statusCode = 400)
    {
        Success = false;
        Data = default;
        Errors = [error];
        StatusCode = statusCode;
    }

    // Implementar este metodo de IResult
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        // Coloca nosso status code no Response
        httpContext.Response.StatusCode = StatusCode;

        if (StatusCode != StatusCodes.Status204NoContent)
        {
            // Serializa nossa resposta para json e encaixa no corpo do response
            await httpContext.Response.WriteAsJsonAsync(this);
        }
    }
}

public static class ResponseViewModel
{
    public static ResponseViewModel<object> Success(int statusCode = 200)
    {
        return new ResponseViewModel<object>(statusCode);
    }

    public static ResponseViewModel<T> Success<T>(T data, int statusCode = 200)
    {
        return new ResponseViewModel<T>(data, statusCode);
    }

    public static ResponseViewModel<object> Failure(List<string> errors, int statusCode = 400)
    {
        return new ResponseViewModel<object>(errors, statusCode);
    }

    public static ResponseViewModel<object> Failure(string error, int statusCode = 400)
    {
        return new ResponseViewModel<object>(error, statusCode);
    }
}