//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.Entities;

namespace eInterceptionTests.Domain;

public class DivisionFrequencyAssignmentTests
{
    [Fact]
    public void Create_ShouldCreateActiveAssignment()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow;

        // Act
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);

        // Assert
        Assert.NotNull(assignment);
        Assert.Equal(divisionProfileId, assignment.DivisionProfileId);
        Assert.Equal(frequencyCode, assignment.FrequencyCode.Value);
        Assert.Equal(activeFromUtc, assignment.ActiveFrom);
        Assert.Null(assignment.ActiveTo);
        Assert.True(assignment.IsActive);
    }

    [Fact]
    public void Close_ShouldSetActiveToAndMarkAsInactive()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow.AddHours(-1);
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        var activeToUtc = DateTime.UtcNow;

        // Act
        assignment.Close(activeToUtc);

        // Assert
        Assert.Equal(activeToUtc, assignment.ActiveTo);
        Assert.False(assignment.IsActive);
    }

    [Fact]
    public void Create_ShouldThrowExceptionForInvalidParameters()
    {
        // Arrange
        var validDivisionProfileId = Guid.NewGuid();
        var validFrequencyCode = "123.45";
        var validActiveFromUtc = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(Guid.Empty, validFrequencyCode, validActiveFromUtc));
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(validDivisionProfileId, null!, validActiveFromUtc));
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(validDivisionProfileId, validFrequencyCode, default));
    }

    [Fact]
    public void Close_ShouldThrowExceptionForInvalidActiveTo()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow.AddHours(-1);
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => assignment.Close(activeFromUtc.AddHours(-1)));
    }

    [Fact]
    public void Close_ShouldThrowExceptionIfAlreadyClosed()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow.AddHours(-2);
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        assignment.Close(DateTime.UtcNow.AddHours(-1));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => assignment.Close(DateTime.UtcNow));
    }

    [Fact]
    public void Create_ShouldNormalizeFrequencyCode()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = " 123.45 ";
        var activeFromUtc = DateTime.UtcNow;

        // Act
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);

        // Assert
        Assert.Equal("123.45", assignment.FrequencyCode.Value);
    }

    [Fact]
    public void Create_ShouldThrowExceptionForEmptyFrequencyCode()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var emptyFrequencyCode = "   ";
        var activeFromUtc = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(divisionProfileId, emptyFrequencyCode, activeFromUtc));
    }

    [Fact]
    public void Create_ShouldThrowExceptionForNullFrequencyCode()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        string? nullFrequencyCode = null;
        var activeFromUtc = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(divisionProfileId, nullFrequencyCode!, activeFromUtc));
    }

    [Fact]
    public void Create_ShouldThrowExceptionForDefaultActiveFrom()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var defaultActiveFrom = default(DateTime);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, defaultActiveFrom));
    }

    [Fact]
    public void Close_ShouldThrowExceptionForDefaultActiveTo()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow.AddHours(-1);
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        var defaultActiveTo = default(DateTime);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => assignment.Close(defaultActiveTo));
    }

    [Fact]
    public void Close_ShouldThrowExceptionIfActiveToIsBeforeActiveFrom()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow;
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        var activeToBeforeActiveFrom = activeFromUtc.AddHours(-1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => assignment.Close(activeToBeforeActiveFrom));
    }

    [Fact]
    public void Create_ShouldTrimFrequencyCode()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "  123.45  ";
        var activeFromUtc = DateTime.UtcNow;

        // Act
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);

        // Assert
        Assert.Equal("123.45", assignment.FrequencyCode.Value);
    }

    [Fact]
    public void Create_ShouldThrowExceptionForWhitespaceFrequencyCode()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var whitespaceFrequencyCode = "   ";
        var activeFromUtc = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(divisionProfileId, whitespaceFrequencyCode, activeFromUtc));
    }

    [Fact]
    public void Create_ShouldThrowExceptionForEmptyGuid()
    {
        // Arrange
        var emptyGuid = Guid.Empty;
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(emptyGuid, frequencyCode, activeFromUtc));
    }

    [Fact]
    public void Create_ShouldThrowExceptionForDefaultActiveFromUtc()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var defaultActiveFromUtc = default(DateTime);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, defaultActiveFromUtc));
    }

    [Fact]
    public void IsActive_ShouldReturnTrueWhenActiveToIsNull()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow;
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);

        // Act
        var isActive = assignment.IsActive;

        // Assert
        Assert.True(isActive);
    }

    [Fact]
    public void IsActive_ShouldReturnFalseWhenActiveToIsNotNull()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow.AddHours(-1);
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        assignment.Close(DateTime.UtcNow);

        // Act
        var isActive = assignment.IsActive;

        // Assert
        Assert.False(isActive);
    }

    [Fact]
    public void IsActiveAt_ShouldReturnTrueWhenMomentIsBetweenActiveFromAndActiveTo()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow.AddHours(-2);
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        var momentUtc = DateTime.UtcNow.AddHours(-1);

        // Act
        var isActiveAtMoment = assignment.IsActiveAt(momentUtc);

        // Assert
        Assert.True(isActiveAtMoment);
    }

    [Fact]
    public void IsActiveAt_ShouldReturnFalseWhenMomentIsBeforeActiveFrom()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow;
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        var momentUtc = DateTime.UtcNow.AddHours(-1);

        // Act
        var isActiveAtMoment = assignment.IsActiveAt(momentUtc);

        // Assert
        Assert.False(isActiveAtMoment);
    }

    [Fact]
    public void IsActiveAt_ShouldExceptWhenMomentIsDefault()
    {
        // Arrange
        var divisionProfileId = Guid.NewGuid();
        var frequencyCode = "123.45";
        var activeFromUtc = DateTime.UtcNow;
        var assignment = DivisionFrequencyAssignment.Create(divisionProfileId, frequencyCode, activeFromUtc);
        var defaultMomentUtc = default(DateTime);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => assignment.IsActiveAt(defaultMomentUtc));
    }
}
