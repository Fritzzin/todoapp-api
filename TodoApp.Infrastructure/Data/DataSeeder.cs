using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any()) return; // Se existir algum usuario, retorne
        
        // Poderia utilizar valores no secret para nao ficar login e senha no codigo
        var user = new User("teste@toshyro.com", "teste");

        context.Users.Add(user);
        context.SaveChanges();
    }
}