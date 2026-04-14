//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptionActionTests
{
    [Fact]
    public void Create_ShouldReturnValidInterceptionAction()
    {
        // Arrange
        string name = "Спостереження";
        string description = "Опис дії спостереження";

        // Act
        var action = InterceptionAction.Create(name, description);

        // Assert
        Assert.NotNull(action);
        Assert.Equal(name, action.Name);
        Assert.Equal(description, action.Description);
    }

    [Fact]
    public void Update_ShouldModifyProperties()
    {
        // Arrange
        var action = InterceptionAction.Create("Спостереження", "Опис дії спостереження");
        string newName = "Загроза";
        string newDescription = "Опис дії загрози";

        // Act
        action.Update(newName, newDescription);

        // Assert
        Assert.Equal(newName, action.Name);
        Assert.Equal(newDescription, action.Description);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        string invalidName = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionAction.Create(invalidName));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        var action = InterceptionAction.Create("Спостереження", "Опис дії спостереження");
        string invalidName = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => action.Update(invalidName));
    }
}
