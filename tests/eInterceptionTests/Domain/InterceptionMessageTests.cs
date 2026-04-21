//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptionMessageTests
{
    [Fact]
    public void InterceptionMessage_Ctor_ShouldInitializeProperties()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        var frequencyCode = "152.1500";
        var divisionName = "Division A";
        var action = RegistryInterceptionAction.Create("Action 1", "Description of Action 1");
        var messageText = "Interception message text";
        bool canBePutOnMap = true;
        var unknownParticipantCount = 2;

        // Act
        var message = InterceptionMessage.Create(dateTime, frequencyCode, divisionName, action.Id, messageText, canBePutOnMap, unknownParticipantCount);

        // Assert
        Assert.IsType<Guid>(message.Id);
        Assert.Equal(dateTime, message.ObservedDate);
        Assert.Equal(frequencyCode, message.FrequencyCode.Value);
        Assert.Equal(divisionName, message.DivisionName?.Value);
        Assert.Equal(action.Id, message.RegistryInterceptionActionId);
        Assert.Equal(messageText, message.MessageText);
        Assert.Equal(canBePutOnMap, message.CanBePutOnMap);
        Assert.Equal(unknownParticipantCount, message.UnknownParticipantCount);
        Assert.True(message.CreatedAt > DateTime.MinValue);
        Assert.True(message.UpdatedAt > DateTime.MinValue);
    }

    [Fact]
    public void InterceptionMessage_Ctor_ShouldThrowException_ForInvalidDate()
    {
        // Arrange
        var frequencyCode = "152.1500";
        var divisionName = "Division A";
        var messageText = "Interception message text";
        var registryInterceptionActionId = Guid.NewGuid();
        bool canBePutOnMap = true;
        var unknownParticipantCount = 2;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(default, frequencyCode, divisionName, registryInterceptionActionId, messageText, canBePutOnMap, unknownParticipantCount));
    }

    [Fact]
    public void InterceptionMessage_Ctor_ShouldThrowException_ForInvalidActionId()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        var divisionName = "Division A";
        var messageText = "Interception message text";
        var registryInterceptionActionId = Guid.Empty;
        bool canBePutOnMap = true;
        var unknownParticipantCount = 2;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(dateTime, "InvalidFrequency", divisionName, registryInterceptionActionId, messageText, canBePutOnMap, unknownParticipantCount));
    }

    [Fact]
    public void InterceptionMessage_Ctor_ShouldThrowException_ForInvalidUnknownParticipantCount()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        var frequencyCode = "152.1500";
        var divisionName = "Division A";
        var messageText = "Interception message text";
        var registryInterceptionActionId = Guid.NewGuid();
        bool canBePutOnMap = true;
        var unknownParticipantCount = -2;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(dateTime, frequencyCode, divisionName, registryInterceptionActionId, messageText, canBePutOnMap, unknownParticipantCount));
    }

    [Fact]
    public void InterceptionMessage_Ctor_ShouldThrowException_ForInvalidFrequencyCode()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        var divisionName = "Division A";
        var registryInterceptionActionId = Guid.NewGuid();
        bool canBePutOnMap = true;
        var unknownParticipantCount = 2;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(dateTime, null!, divisionName, registryInterceptionActionId, string.Empty, canBePutOnMap, unknownParticipantCount));
    }

    [Fact]
    public void InterceptionMessage_Update_ShouldModifyProperties()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var newFrequencyCode = "153.0000";
        var newDivisionName = "Division B";
        var newActionId = Guid.NewGuid();
        bool newCanBePutOnMap = false;
        var newUnknownParticipantCount = 3;

        // Act
        message.Update(newFrequencyCode, newDivisionName, newActionId, newCanBePutOnMap, newUnknownParticipantCount);

        // Assert
        Assert.Equal(newFrequencyCode, message.FrequencyCode.Value);
        Assert.Equal(newDivisionName, message.DivisionName?.Value);
        Assert.Equal(newActionId, message.RegistryInterceptionActionId);
        Assert.Equal(newCanBePutOnMap, message.CanBePutOnMap);
        Assert.Equal(newUnknownParticipantCount, message.UnknownParticipantCount);
    }

    [Fact]
    public void InterceptionMessage_Update_ShouldThrowException_ForInvalidActionId()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => message.Update("153.0000", "Division B", Guid.Empty, false, 3));
    }

    [Fact]
    public void InterceptionMessage_Update_ShouldThrowException_ForInvalidUnknownParticipantCount()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => message.Update("153.0000", "Division B", Guid.NewGuid(), false, -3));
    }

    [Fact]
    public void InterceptionMessage_Update_ShouldThrowException_ForInvalidFrequencyCode()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => message.Update(null!, "Division B", Guid.NewGuid(), false, 3));
    }

    [Fact]
    public void InterceptionMessage_Update_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);
        var initialUpdatedAt = message.UpdatedAt;

        // Act
        message.Update("153.0000", "Division B", Guid.NewGuid(), false, 3);

        // Assert
        Assert.True(message.UpdatedAt > initialUpdatedAt);
    }

    [Fact]
    public void InterceptionMessage_AddParticipant_ShouldAddParticipant()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var participant = InterceptionParticipant.Create(Guid.NewGuid(), "Callsign 1", "Division A", Guid.NewGuid());
        var militaryprofile = MilitaryProfile.Create(participant.Callsign, "152.1500", Guid.NewGuid(), Guid.NewGuid());

        // Act
        message.AddParticipant(militaryprofile.Id, participant.Callsign);

        // Assert
        Assert.Contains(message.Participants, p => p.Callsign == participant.Callsign && p.MilitaryProfileId == militaryprofile.Id);
    }

    [Fact]
    public void InterceptionMessage_AddParticipant_ShouldThrowException_ForInvalidCallsign()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var militaryprofile = MilitaryProfile.Create("Callsign 1", "152.1500", Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        Assert.Throws<ArgumentException>(() => message.AddParticipant(militaryprofile.Id, null!));
    }

    [Fact]
    public void InterceptionMessage_AddParticipant_ShouldThrowException_ForInvalidMilitaryProfileId()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => message.AddParticipant(Guid.Empty, "Callsign 1"));
    }

    [Fact]
    public void InterceptionMessage_AddParticipant_ShouldThrowException_ForinvalidRoleId()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => message.AddParticipant(Guid.NewGuid(), "Callsign 1", "Division A", Guid.Empty));
    }

    [Fact]
    public void InterceptionMessage_RemoveParticipant_ShouldRemoveParticipant()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);
        var callsign = "Callsign 1";

        var militaryprofile = MilitaryProfile.Create(callsign, "152.1500", Guid.NewGuid(), Guid.NewGuid());
        message.AddParticipant(militaryprofile.Id, callsign);

        // Act
        var participantId = message.Participants.First().Id;
        message.RemoveParticipant(participantId);

        // Assert
        Assert.DoesNotContain(message.Participants, p => p.Callsign == callsign && p.MilitaryProfileId == militaryprofile.Id);
    }

    [Fact]
    public void InterceptionMessage_RemoveParticipant_ShouldThrowException_ForNonExistingParticipantId()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => message.RemoveParticipant(Guid.NewGuid()));
    }

    [Fact]
    public void InterceptionMessage_AddCommandVector_ShouldAddCommandVector()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var fromParticipantCallsign = "Callsign 1";
        var militaryprofileFromParticipamtId = Guid.NewGuid();

        var toParticipantCallsign = "Callsign 2";
        var militaryprofileToParticipamtId = Guid.NewGuid();

        message.AddParticipant(militaryprofileFromParticipamtId, fromParticipantCallsign);
        message.AddParticipant(militaryprofileToParticipamtId, toParticipantCallsign);

        var fromParticipantid = message.Participants.First(p => p.Callsign == fromParticipantCallsign).Id;
        var toParticipantid = message.Participants.First(p => p.Callsign == toParticipantCallsign).Id;

        // Act
        message.AddCommandVector(fromParticipantid, toParticipantid);

        // Assert
        Assert.Contains(message.CommandVectors, cv => cv.FromParticipantId == fromParticipantid && cv.ToParticipantId == toParticipantid);
    }

    [Fact]
    public void InterceptionMessage_AddCommandVector_ShouldThrowException_ForNonExistingFromParticipantId()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);
        var toParticipant = InterceptionParticipant.Create(Guid.NewGuid(), "Callsign 2", "Division B", Guid.NewGuid());
        message.AddParticipant(toParticipant.MilitaryProfileId, toParticipant.Callsign);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => message.AddCommandVector(Guid.NewGuid(), toParticipant.Id));
    }

    [Fact]
    public void InterceptionMessage_AddCommandVector_ShouldThrowException_ForNonExistingToParticipantId()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);
        var fromParticipant = InterceptionParticipant.Create(Guid.NewGuid(), "Callsign 1", "Division A", Guid.NewGuid());
        message.AddParticipant(fromParticipant.MilitaryProfileId, fromParticipant.Callsign);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => message.AddCommandVector(fromParticipant.Id, Guid.NewGuid()));
    }

    [Fact]
    public void InterceptionMessage_RemoveCommandVector_ShouldRemoveCommandVector()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var fromParticipantCallsign = "Callsign 1";
        var militaryprofileFromParticipamtId = Guid.NewGuid();
        var toParticipantCallsign = "Callsign 2";
        var militaryprofileToParticipamtId = Guid.NewGuid();

        message.AddParticipant(militaryprofileFromParticipamtId, fromParticipantCallsign);
        message.AddParticipant(militaryprofileToParticipamtId, toParticipantCallsign);

        var fromParticipantid = message.Participants.First(p => p.Callsign == fromParticipantCallsign).Id;
        var toParticipantid = message.Participants.First(p => p.Callsign == toParticipantCallsign).Id;

        message.AddCommandVector(fromParticipantid, toParticipantid);

        // Act
        message.RemoveCommandVector(fromParticipantid, toParticipantid);

        // Assert
        Assert.DoesNotContain(message.CommandVectors, cv => cv.FromParticipantId == fromParticipantid && cv.ToParticipantId == toParticipantid);
    }

    [Fact]
    public void InterceptionMessage_RemoveCommandVector_ShouldThrowException_ForNonExistingCommandVector()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var fromParticipantCallsign = "Callsign 1";
        var militaryprofileFromParticipamtId = Guid.NewGuid();
        var toParticipantCallsign = "Callsign 2";
        var militaryprofileToParticipamtId = Guid.NewGuid();

        message.AddParticipant(militaryprofileFromParticipamtId, fromParticipantCallsign);
        message.AddParticipant(militaryprofileToParticipamtId, toParticipantCallsign);

        var fromParticipantid = message.Participants.First(p => p.Callsign == fromParticipantCallsign).Id;
        var toParticipantid = message.Participants.First(p => p.Callsign == toParticipantCallsign).Id;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => message.RemoveCommandVector(fromParticipantid, toParticipantid));
    }

    [Fact]
    public void InterceptionMessage_Update_ShouldNotModifyParticipantsOrCommandVectors()
    {
        // Arrange
        var message = InterceptionMessage.Create(DateTime.UtcNow, "152.1500", "Division A", Guid.NewGuid(), "Initial message text", true, 2);

        var fromParticipantCallsign = "Callsign 1";
        var militaryprofileFromParticipamtId = Guid.NewGuid();
        var toParticipantCallsign = "Callsign 2";
        var militaryprofileToParticipamtId = Guid.NewGuid();

        message.AddParticipant(militaryprofileFromParticipamtId, fromParticipantCallsign);
        message.AddParticipant(militaryprofileToParticipamtId, toParticipantCallsign);

        var fromParticipantid = message.Participants.First(p => p.Callsign == fromParticipantCallsign).Id;
        var toParticipantid = message.Participants.First(p => p.Callsign == toParticipantCallsign).Id;

        message.AddCommandVector(fromParticipantid, toParticipantid);

        var initialParticipants = message.Participants.ToList();
        var initialCommandVectors = message.CommandVectors.ToList();
        // Act
        message.Update("153.0000", "Division B", Guid.NewGuid(), false, 3);

        // Assert
        Assert.Equal(initialParticipants.Count, message.Participants.Count);
        Assert.Equal(initialCommandVectors.Count, message.CommandVectors.Count);
    }
}
