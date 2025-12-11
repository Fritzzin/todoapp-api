namespace TodoApp.Api.Dtos;

public record UpdateTodoRequestDto(
    string Title,
    string? Description
);