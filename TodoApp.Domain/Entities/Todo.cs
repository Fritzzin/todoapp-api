namespace TodoApp.Domain.Entities;

public class Todo
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDone { get; private set; }

    // Construtor para Entity Framework
    private Todo()
    {
    }

    public Todo(string title, string? description)
    {
        Id = Guid.NewGuid();

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title), "Título não pode ser vazio.");
        }

        Title = title;
        if (description != null)
        {
            Description = description;
        }

        CreatedAt = DateTime.UtcNow;
        IsDone = false;
    }

    public void MarkAsDone()
    {
        IsDone = true;
    }

    public void MarkAsUndone()
    {
        IsDone = false;
    }

    private void UpdateTitle(string newTitle)
    {
        Title = newTitle;
    }

    private void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }

    public void UpdateInfo(string newTitle, string? newDescription)
    {
        if (IsDone)
        {
            throw new InvalidOperationException("Não foi possível atualizar uma tarefa já concluída!");
        }

        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentNullException(nameof(newTitle), "Titulo nao pode ser vazio.");
        }

        if (newDescription != null)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentNullException(nameof(newTitle), "Descricao nao pode ser vazio.");
            }
        }

        UpdateTitle(newTitle);
        if (newDescription != null) UpdateDescription(newDescription);
    }
}