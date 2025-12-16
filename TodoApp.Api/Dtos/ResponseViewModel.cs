namespace TodoApp.Api.Dtos;

public class ResponseViewModel<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public List<string>? Errors { get; private set; }

    public ResponseViewModel()
    {
        Success = true;
        Data = default;
        Errors = null;
    }

    public ResponseViewModel(T data)
    {
        Success = true;
        Data = data;
        Errors = null;
    }

    public ResponseViewModel(List<string> errors)
    {
        Success = false;
        Data = default;
        Errors = errors;
    }

    public ResponseViewModel(string error)
    {
        Success = false;
        Data = default;
        Errors = [error];
    }
}

public static class ResponseViewModel
{
    public static ResponseViewModel<object> Success()
    {
        return new ResponseViewModel<object>();
    }

    public static ResponseViewModel<T> Success<T>(T data)
    {
        return new ResponseViewModel<T>(data);
    }

    public static ResponseViewModel<object> Failure(List<string> errors)
    {
        return new ResponseViewModel<object>(errors);
    }

    public static ResponseViewModel<object> Failure(string error)
    {
        return new ResponseViewModel<object>(error);
    }
}