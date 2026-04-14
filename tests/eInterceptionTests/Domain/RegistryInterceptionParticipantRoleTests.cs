//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class RegistryInterceptionParticipantRoleTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidData()
    {
        // Arrange
        string name = "Спостерігач";
        string description = "Особа, яка спостерігає за радіоперехопленням.";

        // Act
        var role = RegistryInterceptionParticipantRole.Create(name, description);

        // Assert
        Assert.NotNull(role);
        Assert.Equal(name, role.Name);
        Assert.Equal(description, role.Description);
    }

    [Fact]
    public void Create_ShouldTrimNameAndDescription()
    {
        // Arrange
        string name = "  Спостерігач  ";
        string description = "  Особа, яка спостерігає за радіоперехопленням.  ";

        // Act
        var role = RegistryInterceptionParticipantRole.Create(name, description);

        // Assert
        Assert.NotNull(role);
        Assert.Equal("Спостерігач", role.Name);
        Assert.Equal("Особа, яка спостерігає за радіоперехопленням.", role.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldThrowException_WhenNameIsNullOrWhitespace(string name)
    {
        // Arrange
        string description = "Опис ролі.";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipantRole.Create(name, description));
    }

    [Fact]
    public void Update_ShouldSucceed_WithValidData()
    {
        // Arrange
        var role = RegistryInterceptionParticipantRole.Create("Спостерігач", "Опис ролі.");
        string newName = "Аналітик";
        string newDescription = "Особа, яка аналізує радіоперехоплення.";

        // Act
        role.Update(newName, newDescription);

        // Assert
        Assert.Equal(newName, role.Name);
        Assert.Equal(newDescription, role.Description);
    }
}