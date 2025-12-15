namespace TodoApp.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    // Construtor p/ Entity Framework
    private User()
    {
    }

    public User(string email, string password)
    {
        Id = Guid.NewGuid();

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        Email = email;
        Password = FakeHashPassword(password);
    }

    // Metodo fake para fins de estudo
    private string FakeHashPassword(string password)
    {
        return password;
    }

    public bool VerifyPassword(string providedPassword)
    {
        return string.Equals(Password, FakeHashPassword(providedPassword));
    }
}