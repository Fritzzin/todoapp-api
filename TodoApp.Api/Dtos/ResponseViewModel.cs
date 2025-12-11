namespace TodoApp.Api.Dtos;

public class ResponseViewModel<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public List<string>? Errors { get; private set; }

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