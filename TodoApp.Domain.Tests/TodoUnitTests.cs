using Bogus;
using FluentAssertions;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Tests;

public class TodoUnitTests
{
    [Fact]
    public void CreateTodo_WithValidTitle_ShouldSucceed()
    {
        // Arrange
        var faker = new Faker();
        var title = faker.Lorem.Sentence();
        var description = faker.Lorem.Paragraph();

        var bogusTodoGenerator = new Faker<Todo>()
            .CustomInstantiator(f => new Todo(title, description));

        // Act
        var fakeTodo = bogusTodoGenerator.Generate();

        // Assert
        fakeTodo.Should().NotBeNull();
        fakeTodo.Title.Should().Be(title);
        fakeTodo.Description.Should().Be(description);
        fakeTodo.IsDone.Should().BeFalse();
        fakeTodo.Id.Should().NotBeEmpty();
        fakeTodo.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateTodo_WithInvalidTitle_ShouldThrow(String invalidTitle)
    {
        // Arrange
        var faker = new Faker();
        var description = faker.Lorem.Paragraph();

        // Act
        Action act = () => new Todo(invalidTitle, description);

        // Assert
        act.Should().Throw<ArgumentNullException>();
   }
}