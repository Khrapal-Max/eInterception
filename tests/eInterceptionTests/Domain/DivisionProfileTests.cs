//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class DivisionProfileTests
{
    [Fact]
    public void CreateDivisionProfile_ValidData_ShouldSucceed()
    {
        // Arrange
        var name = "  128 ОГШБр  ";

        // Act
        var profile = DivisionProfile.Create(name);

        // Assert
        Assert.NotEqual(Guid.Empty, profile.Id);
        Assert.Equal("128 ОГШБр", profile.Name.Value);
        Assert.True(profile.CreatedAt <= DateTime.UtcNow);
        Assert.True(profile.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void CreateDivisionProfile_MissingName_ShouldThrow()
    {
        // Arrange
        var name = " ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionProfile.Create(name));
    }

    [Fact]
    public void RenameDivisionProfile_ValidName_ShouldSucceed()
    {
        // Arrange
        var profile = DivisionProfile.Create("128 ОГШБр");

        // Act
        profile.Rename(" 82 ОДШБр ");

        // Assert
        Assert.Equal("82 ОДШБр", profile.Name.Value);
        Assert.True(profile.UpdatedAt >= profile.CreatedAt);
    }
}