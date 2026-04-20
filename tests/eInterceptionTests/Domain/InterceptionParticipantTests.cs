//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptionParticipantTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidData()
    {
        // Arrange
        var militaryProfileId = Guid.NewGuid();
        var callsign = "Alpha";
        var divisionName = "1st Division";
        var roleId = Guid.NewGuid();

        // Act
        var participant = InterceptionParticipant.Create(militaryProfileId, callsign, divisionName, roleId);

        // Assert
        Assert.NotNull(participant);
        Assert.Equal(militaryProfileId, participant.MilitaryProfileId);
        Assert.Equal(callsign, participant.Callsign);
        Assert.Equal(divisionName, participant.DivisionName?.Value);
        Assert.Equal(roleId, participant.RegistryInterceptionParticipantRoleId);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenCallsignIsEmpty()
    {
        // Arrange
        var militaryProfileId = Guid.NewGuid();
        var callsign = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionParticipant.Create(militaryProfileId, callsign));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenMilitaryProfileIdIsEmpty()
    {
        // Arrange
        var militaryProfileId = Guid.Empty;
        var callsign = "Bravo";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionParticipant.Create(militaryProfileId, callsign));
    }
}
