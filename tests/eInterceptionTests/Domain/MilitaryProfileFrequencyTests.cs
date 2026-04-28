//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.Entities;

namespace eInterceptionTests.Domain;

public class MilitaryProfileFrequencyTests
{
    [Fact]
    public void Create_ValidFrequency_ShouldSucceed()
    {
        // Arrange
        var frequency = "123.45";

        // Act
        var profileFrequency = MilitaryProfileFrequency.Create(frequency);

        // Assert
        Assert.NotEqual(Guid.Empty, profileFrequency.Id);
        Assert.Equal(frequency, profileFrequency.FrequencyCode.Value);
        Assert.True(profileFrequency.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_ShouldNormalizeFrequency()
    {
        // Arrange
        var frequency = "  123.45  ";

        // Act
        var profileFrequency = MilitaryProfileFrequency.Create(frequency);

        // Assert
        Assert.Equal("123.45", profileFrequency.FrequencyCode.Value);
    }

    [Fact]
    public void Create_MissingFrequency_ShouldThrow()
    {
        // Arrange
        string? frequency = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => MilitaryProfileFrequency.Create(frequency!));
    }

    [Fact]
    public void Create_EmptyFrequency_ShouldThrow()
    {
        // Arrange
        var frequency = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => MilitaryProfileFrequency.Create(frequency));
    }

    [Fact]
    public void HasFrequency_SameFrequency_ShouldReturnTrue()
    {
        // Arrange
        var profileFrequency = MilitaryProfileFrequency.Create("123.45");

        // Act
        var result = profileFrequency.HasFrequency("123.45");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasFrequency_NormalizedSameFrequency_ShouldReturnTrue()
    {
        // Arrange
        var profileFrequency = MilitaryProfileFrequency.Create("123.45");

        // Act
        var result = profileFrequency.HasFrequency("  123.45  ");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasFrequency_DifferentFrequency_ShouldReturnFalse()
    {
        // Arrange
        var profileFrequency = MilitaryProfileFrequency.Create("123.45");

        // Act
        var result = profileFrequency.HasFrequency("543.21");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasFrequency_MissingFrequency_ShouldThrow()
    {
        // Arrange
        var profileFrequency = MilitaryProfileFrequency.Create("123.45");
        string? frequency = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profileFrequency.HasFrequency(frequency!));
    }

    [Fact]
    public void HasFrequency_EmptyFrequency_ShouldThrow()
    {
        // Arrange
        var profileFrequency = MilitaryProfileFrequency.Create("123.45");
        var frequency = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profileFrequency.HasFrequency(frequency));
    }
}
