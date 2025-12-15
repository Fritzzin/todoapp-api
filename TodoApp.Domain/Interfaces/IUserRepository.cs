using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface IUserRepository
{
    void Add(User user);
    Task<User?> GetByEmailAsync(string email);
}