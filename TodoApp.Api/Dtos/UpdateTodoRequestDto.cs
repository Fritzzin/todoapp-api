namespace TodoApp.Api.Dtos;

public record UpdateTodoRequestDto(
    string title,
    string? description
);