//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class RegistryInterceptionParticipantTests
{
    [Fact]
    public void Create_ShouldInitializeProperties()
    {
        // Arrange
        var name = "John Doe";
        var frequencyCode = "123.4500";
        var divisionName = "Alpha Division";
        var roleId = Guid.NewGuid();

        // Act
        var participant = RegistryInterceptionParticipant.Create(name, frequencyCode, divisionName, roleId);

        // Assert
        Assert.NotEqual(Guid.Empty, participant.Id);
        Assert.Equal(name, participant.Name);
        Assert.Equal(frequencyCode, participant.FrequencyCode.Value);
        Assert.Equal(roleId, participant.RegistryInterceptionParticipantRoleId);
        Assert.Equal(divisionName, participant.DivisionName?.Value);
        Assert.True(participant.CreatedAt <= DateTime.UtcNow);
        Assert.True(participant.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Update_ShouldModifyProperties()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        var participant = RegistryInterceptionParticipant.Create("John Doe", "123.4500", "Alpha Division", roleId);
        var newName = "Jane Smith";
        var newFrequencyCode = "987.6500";
        var newDivisionName = "Beta Division";
        var newRoleId = Guid.NewGuid();

        // Act
        participant.Update(newName, newFrequencyCode, newDivisionName, newRoleId);

        // Assert
        Assert.Equal(newName, participant.Name);
        Assert.Equal(newFrequencyCode, participant.FrequencyCode.Value);
        Assert.Equal(newDivisionName, participant.DivisionName?.Value);
        Assert.Equal(newRoleId, participant.RegistryInterceptionParticipantRoleId);
        Assert.True(participant.UpdatedAt > participant.CreatedAt);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        string frequencyCode = "123.4500";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipant.Create(null!, frequencyCode));
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipant.Create(string.Empty, frequencyCode));
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipant.Create("   ", frequencyCode));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenFrequencyCodeIsNullOrWhitespace()
    {
        // Arrange
        string name = "John Doe";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipant.Create(name, null!));
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipant.Create(name, string.Empty));
        Assert.Throws<ArgumentException>(() => RegistryInterceptionParticipant.Create(name, "   "));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenNameIsNullOrWhitespace()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        var participant = RegistryInterceptionParticipant.Create("John Doe", "123.4500", "Alpha Division");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => participant.Update(null!, "987.6500", "Beta Division", roleId));
        Assert.Throws<ArgumentException>(() => participant.Update(string.Empty, "987.6500", "Beta Division", roleId));
        Assert.Throws<ArgumentException>(() => participant.Update("   ", "987.6500", "Beta Division", roleId));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenFrequencyCodeIsNullOrWhitespace()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        var participant = RegistryInterceptionParticipant.Create("John Doe", "123.4500", "Alpha Division");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => participant.Update("Jane Smith", null!, "Beta Division", roleId));
        Assert.Throws<ArgumentException>(() => participant.Update("Jane Smith", string.Empty, "Beta Division", roleId));
        Assert.Throws<ArgumentException>(() => participant.Update("Jane Smith", "   ", "Beta Division", roleId));
    }
}
