using FluentValidation;
using TodoApp.Api.Dtos;

namespace TodoApp.Api.Validators;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequestDto>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(todo => todo.Title)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Titulo deve ter no minimo 3 caracteres")
            .MaximumLength(255).WithMessage("Titulo deve ter no maximo 255 caracteres");

        RuleFor(todo => todo.Description)
            .MaximumLength(500).WithMessage("Descrição deve ter no maximo 500 caracteres");
    }
}