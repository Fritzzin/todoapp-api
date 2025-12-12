using Bogus;
using Moq;
using TodoApp.Domain.Interfaces;
using FluentAssertions;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;


namespace TodoApp.Application.Tests;

public class TodoServiceTests
{
    [Fact]
    public async Task CreateTodoAsync_WithValidData_ShouldSucceed()
    {
        // Arrange
        var faker = new Faker();
        var title = faker.Lorem.Paragraph();
        var description = faker.Lorem.Paragraph();
        var mockTodoRepository = new Mock<ITodoRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var todoService = new TodoService(mockUnitOfWork.Object, mockTodoRepository.Object);

        // Act
        var fakeGuid = await todoService.CreateTodoAsync(title, description);

        // Assert
        fakeGuid.Should().NotBeEmpty();

        // Verifica dentro do mock o comportamento
        mockTodoRepository.Verify(repo => repo.Add(It.IsAny<Todo>()), Times.Once);
        mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTodoExists_ShouldReturnTodo()
    {
        // Arrange

        /*
        var faker = new Faker();
        var title = faker.Lorem.Paragraph();
        var description = faker.Lorem.Paragraph();
        var fakeTodo = new Todo(title, description);
        OU ...
        */

        var faker = new Faker();
        var expectedTodo = new Todo(
            faker.Lorem.Sentence(),
            faker.Lorem.Paragraph()
        );
        var id = expectedTodo.Id;

        var mockTodoRepository = new Mock<ITodoRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockTodoRepository
            .Setup(r => r.FindById(id)) // Quando buscar com este Id
            .ReturnsAsync(expectedTodo); // Retorne este Todo

        var todoService = new TodoService(mockUnitOfWork.Object, mockTodoRepository.Object);

        // Act
        var fetchedTodo = await todoService.GetByIdAsync(id);

        // Assert
        fetchedTodo.Should().BeEquivalentTo(expectedTodo);

        mockTodoRepository.Verify(r => r.FindById(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTodoDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var faker = new Faker();
        var id = faker.Random.Guid();
        var mockTodoRepository = new Mock<ITodoRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockTodoRepository.Setup(r => r.FindById(id)).ReturnsAsync((Todo?)null); // Retorna um tipo Todo nulo
        var todoService = new TodoService(mockUnitOfWork.Object, mockTodoRepository.Object);

        // Act
        var fetchedTodo = await todoService.GetByIdAsync(id);

        // Assert
        fetchedTodo.Should().BeNull();
        mockTodoRepository.Verify(r => r.FindById(id), Times.Once);
    }


    [Fact]
    public async Task UpdateTodoAsync_WhenTodoExists_ShouldUpdateAndCommit()
    {
        var faker = new Faker();
        var existingTodo = new Todo(faker.Lorem.Sentence(), faker.Lorem.Paragraph());

        var newTitle = faker.Lorem.Sentence();
        var newDescription = faker.Lorem.Paragraph();
        var id = existingTodo.Id; // Manter o mesmo id do todo existente

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockTodoRepository = new Mock<ITodoRepository>();
        mockTodoRepository.Setup(r => r.FindById(id)).ReturnsAsync(existingTodo);
        var todoService = new TodoService(mockUnitOfWork.Object, mockTodoRepository.Object);

        // Act
        var isUpdated = await todoService.UpdateTodoAsync(id, newTitle, newDescription);

        // Assert
        isUpdated.Should().BeTrue();
        mockTodoRepository.Verify(repo => repo.FindById(id), Times.Once);
        mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateTodoAsync_WhenTodoDoesNotExists_ShouldReturnFalseAndNotCommit()
    {
        // Arrange
        var faker = new Faker();
   
        var id = faker.Random.Guid(); // Manter o mesmo id do todo existente
        var newTitle = faker.Lorem.Sentence();
        var newDescription = faker.Lorem.Paragraph();

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockTodoRepository = new Mock<ITodoRepository>();
        mockTodoRepository.Setup(r => r.FindById(id)).ReturnsAsync((Todo?)null);
        var todoService = new TodoService(mockUnitOfWork.Object, mockTodoRepository.Object);

        // Act
        var isUpdated = await todoService.UpdateTodoAsync(id, newTitle, newDescription);

        // Assert
        isUpdated.Should().BeFalse();
        mockTodoRepository.Verify(repo => repo.FindById(id), Times.Once);
        mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }
}