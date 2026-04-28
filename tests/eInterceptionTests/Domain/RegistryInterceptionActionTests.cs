//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.Entities;

namespace eInterceptionTests.Domain;

public class RegistryInterceptionActionTests
{
    [Fact]
    public void Create_ShouldReturnValidRegistryInterceptionAction()
    {
        // Arrange
        string name = "Спостереження";
        string description = "Опис дії спостереження";

        // Act
        var action = RegistryInterceptionAction.Create(name, description);

        // Assert
        Assert.NotNull(action);
        Assert.Equal(name, action.Name.Value);
        Assert.Equal(description, action.Description);
    }

    [Fact]
    public void Update_ShouldModifyProperties()
    {
        // Arrange
        var action = RegistryInterceptionAction.Create("Спостереження", "Опис дії спостереження");
        string newName = "Загроза";
        string newDescription = "Опис дії загрози";

        // Act
        action.Update(newName, newDescription);

        // Assert
        Assert.Equal(newName, action.Name.Value);
        Assert.Equal(newDescription, action.Description);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        string invalidName = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => RegistryInterceptionAction.Create(invalidName));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        var action = RegistryInterceptionAction.Create("Спостереження", "Опис дії спостереження");
        string invalidName = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => action.Update(invalidName));
    }
}
