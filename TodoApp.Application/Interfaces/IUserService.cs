using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces;

public interface IUserService
{
   Task<User?> GetUserByEmailAsync(string email);
}