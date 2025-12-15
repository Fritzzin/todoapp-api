namespace TodoApp.Application.Interfaces;

public interface ITokenService
{
   Task<string?> AuthenticateAsync(string email, string password); 
}