namespace TodoApp.Api.Dtos;

public class ResponseViewModel<T>
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

    public ResponseViewModel(T data, int statusCode = StatusCodes.Status200OK)
    {
        Success = true;
        Data = data;
        Errors = null;
        StatusCode = statusCode;
    }

    public ResponseViewModel(List<string> errors, int statusCode = StatusCodes.Status400BadRequest)
    {
        Success = false;
        Data = default;
        Errors = errors;
        StatusCode = statusCode;
    }

    public ResponseViewModel(string error, int statusCode = StatusCodes.Status400BadRequest)
    {
        Success = false;
        Data = default;
        Errors = [error];
        StatusCode = statusCode;
    }
}

public static class ResponseViewModel
{
    public static ResponseViewModel<object> Success(int statusCode = StatusCodes.Status200OK)
    {
        return new ResponseViewModel<object>(statusCode);
    }

    public static ResponseViewModel<T> Success<T>(T data, int statusCode = StatusCodes.Status200OK)
    {
        return new ResponseViewModel<T>(data, statusCode);
    }

    public static ResponseViewModel<object> Failure(List<string> errors,
        int statusCode = StatusCodes.Status400BadRequest)
    {
        return new ResponseViewModel<object>(errors, statusCode);
    }

    public static ResponseViewModel<object> Failure(string error, int statusCode = StatusCodes.Status400BadRequest)
    {
        return new ResponseViewModel<object>(error, statusCode);
    }
}