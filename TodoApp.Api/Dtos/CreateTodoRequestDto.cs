namespace TodoApp.Api.Dtos;

public record CreateTodoRequestDto(
    string Title,
    string? Description
);