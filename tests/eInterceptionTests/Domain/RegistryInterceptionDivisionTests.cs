//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class RegistryInterceptionDivisionTests
{
    [Fact]
    public void Create_ShouldReturnValidRegistryInterceptionDivision()
    {
        // Arrange
        var frequencyCode = "123.45";
        var divisionName = "Test Division";

        // Act
        var division = RegistryInterceptionDivision.Create(frequencyCode, divisionName);

        // Assert
        Assert.NotNull(division);
        Assert.Equal(frequencyCode, division.FrequencyCode.Value);
        Assert.Equal(divisionName, division.DivisionName?.Value);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFrequencyCodeIsNull()
    {
        // Arrange
        string? frequencyCode = null;
        var divisionName = "Test Division";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => RegistryInterceptionDivision.Create(frequencyCode!, divisionName));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFrequencyCodeIsEmpty()
    {
        // Arrange
        var emptyFrequencyCode = string.Empty;
        var divisionName = "Test Division";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => RegistryInterceptionDivision.Create(emptyFrequencyCode, divisionName));
    }

    [Fact]
    public void Create_ShouldTrimWhitespaceFromInputs()
    {
        // Arrange
        var frequencyCode = " 123.45 ";
        var divisionName = " Test Division ";

        // Act
        var division = RegistryInterceptionDivision.Create(frequencyCode, divisionName);

        // Assert
        Assert.NotNull(division);
        Assert.Equal("123.45", division.FrequencyCode.Value);
        Assert.Equal("Test Division", division.DivisionName?.Value);
    }

    [Fact]
    public void UpdateDivisionName_ShouldUpdateDivisionName()
    {
        // Arrange
        var frequencyCode = "123.45";
        var divisionName = "Test Division";
        var division = RegistryInterceptionDivision.Create(frequencyCode, divisionName);
        var newDivisionName = "Updated Division";

        // Act
        division.Update(frequencyCode, newDivisionName);

        // Assert
        Assert.Equal(newDivisionName, division.DivisionName?.Value);
    }
}
