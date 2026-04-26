//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class MilitaryProfileTests
{
    [Fact]
    public void CreateMilitaryProfile_ValidData_ShouldSucceed()
    {
        // Arrange
        var callsign = "Alpha";
        var frequency = "123.45";
        var divisionId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        // Act
        var profile = MilitaryProfile.Create(callsign, frequency, divisionId, roleId);

        // Assert
        Assert.NotNull(profile);
        Assert.NotEqual(Guid.Empty, profile.Id);
        Assert.Equal(callsign, profile.Callsign);
        Assert.Single(profile.Frequencies);
        Assert.Equal(frequency, profile.Frequencies.First().FrequencyCode.Value);
        Assert.Equal(divisionId, profile.DivisionProfileId);
        Assert.Equal(roleId, profile.RegistryInterceptionParticipantRoleId);
        Assert.True(profile.CreatedAt <= DateTime.UtcNow);
        Assert.True(profile.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void CreateMilitaryProfile_MissingCallsign_ShouldThrow()
    {
        // Arrange
        string? callsign = null;
        var frequency = "123.45";
        var roleId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => MilitaryProfile.Create(callsign!, frequency, null, roleId));
    }

    [Fact]
    public void CreateMilitaryProfile_MissingFrequency_ShouldThrow()
    {
        // Arrange
        var callsign = "Bravo";
        string? frequency = null;
        var roleId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => MilitaryProfile.Create(callsign, frequency!, null, roleId));
    }

    [Fact]
    public void CreateMilitaryProfile_EmptyDivisionProfileId_ShouldThrow()
    {
        // Arrange
        var callsign = "Charlie";
        var frequency = "456.78";
        var emptyDivisionId = Guid.Empty;
        var roleId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => MilitaryProfile.Create(callsign, frequency, emptyDivisionId, roleId));
    }

    [Fact]
    public void CreateMilitaryProfile_EmptyRoleId_ShouldThrow()
    {
        // Arrange
        var callsign = "Charlie";
        var frequency = "456.78";
        var emptyRoleId = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => MilitaryProfile.Create(callsign, frequency, null, emptyRoleId));
    }

    [Fact]
    public void UpdateMilitaryProfile_ValidData_ShouldSucceed()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Delta", "789.01", Guid.NewGuid(), Guid.NewGuid());
        var originalUpdatedAt = profile.UpdatedAt;
        var newCallsign = "Echo";
        var newDivisionId = Guid.NewGuid();
        var newRoleId = Guid.NewGuid();

        // Act
        profile.Update(newCallsign, newDivisionId, newRoleId);

        // Assert
        Assert.Equal(newCallsign, profile.Callsign);
        Assert.Equal(newDivisionId, profile.DivisionProfileId);
        Assert.Equal(newRoleId, profile.RegistryInterceptionParticipantRoleId);
        Assert.True(profile.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void UpdateMilitaryProfile_ClearOptionalFields_ShouldSucceed()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Foxtrot", "321.09", Guid.NewGuid(), Guid.NewGuid());
        var newCallsign = "Golf";

        // Act
        profile.Update(newCallsign, null, null);

        // Assert
        Assert.Equal(newCallsign, profile.Callsign);
        Assert.Null(profile.DivisionProfileId);
        Assert.Null(profile.RegistryInterceptionParticipantRoleId);
        Assert.True(profile.UpdatedAt >= profile.CreatedAt);
    }

    [Fact]
    public void UpdateMilitaryProfile_MissingCallsign_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Hotel", "654.32", Guid.NewGuid(), Guid.NewGuid());
        string? newCallsign = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profile.Update(newCallsign!, Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public void UpdateMilitaryProfile_EmptyDivisionProfileId_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("India", "987.65", Guid.NewGuid(), Guid.NewGuid());
        var newCallsign = "Juliet";
        var emptyDivisionId = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profile.Update(newCallsign, emptyDivisionId, Guid.NewGuid()));
    }

    [Fact]
    public void UpdateMilitaryProfile_EmptyRoleId_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("India", "987.65", Guid.NewGuid(), Guid.NewGuid());
        var newCallsign = "Juliet";
        var emptyRoleId = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profile.Update(newCallsign, Guid.NewGuid(), emptyRoleId));
    }

    [Fact]
    public void CreateMilitaryProfile_NormalizesCallsignAndFrequency()
    {
        // Arrange
        var callsign = "  Kilo  ";
        var frequency = "  111.22  ";
        var roleId = Guid.NewGuid();

        // Act
        var profile = MilitaryProfile.Create(callsign, frequency, null, roleId);

        // Assert
        Assert.Equal("Kilo", profile.Callsign);
        Assert.Equal("111.22", profile.Frequencies.First().FrequencyCode.Value);
    }

    [Fact]
    public void UpdateMilitaryProfile_NormalizesCallsign()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Lima", "222.33", null, Guid.NewGuid());
        var newCallsign = "  Mike  ";

        // Act
        profile.Update(newCallsign, null, null);

        // Assert
        Assert.Equal("Mike", profile.Callsign);
    }

    [Fact]
    public void AddFrequency_ValidFrequency_ShouldSucceed()
    {
        // Arrange
        var profile = MilitaryProfile.Create("November", "333.44", null, Guid.NewGuid());
        var additionalFrequency = "444.55";

        // Act
        profile.AddFrequency(additionalFrequency);

        // Assert
        Assert.Equal(2, profile.Frequencies.Count);
        Assert.Contains(profile.Frequencies, f => f.FrequencyCode.Value == additionalFrequency);
    }

    [Fact]
    public void AddFrequency_DuplicateFrequency_ShouldNotAdd()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Oscar", "555.66", null, Guid.NewGuid());
        var duplicateFrequency = "555.66";

        // Act
        profile.AddFrequency(duplicateFrequency);

        // Assert
        Assert.Single(profile.Frequencies);
    }

    [Fact]
    public void AddFrequency_NormalizedDuplicateFrequency_ShouldNotAdd()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Oscar", "555.66", null, Guid.NewGuid());
        var duplicateFrequency = "  555.66  ";

        // Act
        profile.AddFrequency(duplicateFrequency);

        // Assert
        Assert.Single(profile.Frequencies);
    }

    [Fact]
    public void AddFrequency_InvalidFrequency_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Papa", "666.77", null, Guid.NewGuid());
        var invalidFrequency = default(string);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profile.AddFrequency(invalidFrequency!));
    }

    [Fact]
    public void RemoveFrequency_ValidFrequency_ShouldSucceed()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Quebec", "777.88", null, Guid.NewGuid());
        var additionalFrequency = "888.99";
        profile.AddFrequency(additionalFrequency);

        // Act
        profile.RemoveFrequency("777.88");

        // Assert
        Assert.Single(profile.Frequencies);
        Assert.Contains(profile.Frequencies, f => f.FrequencyCode.Value == additionalFrequency);
    }

    [Fact]
    public void RemoveFrequency_NormalizedFrequency_ShouldSucceed()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Quebec", "777.88", null, Guid.NewGuid());
        var additionalFrequency = "888.99";
        profile.AddFrequency(additionalFrequency);

        // Act
        profile.RemoveFrequency("  777.88  ");

        // Assert
        Assert.Single(profile.Frequencies);
        Assert.Contains(profile.Frequencies, f => f.FrequencyCode.Value == additionalFrequency);
    }

    [Fact]
    public void RemoveFrequency_LastRemainingFrequency_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Papa", "666.77", null, Guid.NewGuid());
        var frequencyToRemove = "666.77";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => profile.RemoveFrequency(frequencyToRemove));
    }

    [Fact]
    public void RemoveFrequency_InvalidFrequency_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Romeo", "999.00", null, Guid.NewGuid());
        var invalidFrequency = default(string);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => profile.RemoveFrequency(invalidFrequency!));
    }

    [Fact]
    public void RemoveFrequency_NonExistingFrequency_ShouldThrow()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Sierra", "000.11", null, Guid.NewGuid());
        var nonExistingFrequency = "111.22";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => profile.RemoveFrequency(nonExistingFrequency));
    }

    [Fact]
    public void MilitaryProfile_UpdatedAt_ShouldUpdateOnProfileUpdate()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Sierra", "000.11", null, Guid.NewGuid());
        var originalUpdatedAt = profile.UpdatedAt;

        // Act
        profile.Update("Sierra Updated", null, null);

        // Assert
        Assert.True(profile.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void MilitaryProfile_UpdatedAt_ShouldUpdateOnAddFrequency()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Tango", "000.11", null, Guid.NewGuid());
        var originalUpdatedAt = profile.UpdatedAt;

        // Act
        profile.AddFrequency("000.12");

        // Assert
        Assert.True(profile.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void MilitaryProfile_UpdatedAt_ShouldUpdateOnRemoveFrequency()
    {
        // Arrange
        var profile = MilitaryProfile.Create("Uniform", "000.11", null, Guid.NewGuid());
        profile.AddFrequency("000.12");
        var originalUpdatedAt = profile.UpdatedAt;

        // Act
        profile.RemoveFrequency("000.12");

        // Assert
        Assert.True(profile.UpdatedAt >= originalUpdatedAt);
    }
}
