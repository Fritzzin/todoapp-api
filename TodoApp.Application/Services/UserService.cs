using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

public class UserService : IUserService
{
    // private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;

    // public UserService(IUnitOfWork unitOfWork, IUserRepository repository)
    // {
    //     _repository = repository;
    //     _unitOfWork = unitOfWork;
    // }
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _repository.GetByEmailAsync(email);
    }
}