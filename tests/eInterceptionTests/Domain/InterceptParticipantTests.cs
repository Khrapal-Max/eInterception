//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptParticipantTests
{
    [Fact]
    public void Create_ShouldInitializeProperties()
    {
        // Arrange
        string name = "John Doe";
        string frequencyCode = "123.4500";
        string divisionName = "Alpha Division";
        string role = "Commander";

        // Act
        var participant = InterceptParticipant.Create(name, frequencyCode, divisionName, role);

        // Assert
        Assert.NotEqual(Guid.Empty, participant.Id);
        Assert.Equal(name, participant.Name);
        Assert.Equal(frequencyCode, participant.FrequencyCode);
        Assert.Equal(role, participant.Role);
        Assert.Equal(divisionName, participant.DivisionName);
        Assert.True(participant.CreatedAt <= DateTime.UtcNow);
        Assert.True(participant.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Update_ShouldModifyProperties()
    {
        // Arrange
        var participant = InterceptParticipant.Create("John Doe", "123.4500", "Alpha Division", "Commander");
        string newName = "Jane Smith";
        string newFrequencyCode = "987.6500";
        string newDivisionName = "Beta Division";
        string newRole = "Observer";

        // Act
        participant.Update(newName, newFrequencyCode, newDivisionName, newRole);

        // Assert
        Assert.Equal(newName, participant.Name);
        Assert.Equal(newFrequencyCode, participant.FrequencyCode);
        Assert.Equal(newDivisionName, participant.DivisionName);
        Assert.Equal(newRole, participant.Role);
        Assert.True(participant.UpdatedAt > participant.CreatedAt);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        string frequencyCode = "123.4500";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptParticipant.Create(null!, frequencyCode));
        Assert.Throws<ArgumentException>(() => InterceptParticipant.Create(string.Empty, frequencyCode));
        Assert.Throws<ArgumentException>(() => InterceptParticipant.Create("   ", frequencyCode));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFrequencyCodeIsNullOrWhitespace()
    {
        // Arrange
        string name = "John Doe";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptParticipant.Create(name, null!));
        Assert.Throws<ArgumentException>(() => InterceptParticipant.Create(name, string.Empty));
        Assert.Throws<ArgumentException>(() => InterceptParticipant.Create(name, "   "));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        var participant = InterceptParticipant.Create("John Doe", "123.4500", "Alpha Division");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => participant.Update(null!, "987.6500", "Beta Division", "Observer"));
        Assert.Throws<ArgumentException>(() => participant.Update(string.Empty, "987.6500", "Beta Division", "Observer"));
        Assert.Throws<ArgumentException>(() => participant.Update("   ", "987.6500", "Beta Division", "Observer"));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenFrequencyCodeIsNullOrWhitespace()
    {
        // Arrange
        var participant = InterceptParticipant.Create("John Doe", "123.4500", "Alpha Division");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => participant.Update("Jane Smith", null!, "Beta Division", "Observer"));
        Assert.Throws<ArgumentException>(() => participant.Update("Jane Smith", string.Empty, "Beta Division", "Observer"));
        Assert.Throws<ArgumentException>(() => participant.Update("Jane Smith", "   ", "Beta Division", "Observer"));
    }
}
