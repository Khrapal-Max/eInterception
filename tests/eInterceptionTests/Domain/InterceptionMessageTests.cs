//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptionMessageTests
{
    [Fact]
    public void CreateInterceptionMessage_ShouldInitializeProperties()
    {
        // Arrange
        var observedDate = DateTime.UtcNow;
        var frequencyCode = "123.45 MHz";
        var divisionName = "Division A";
        var interceptionAction = "Moving";
        var unknownParticipantCount = 3;
        var messageText = "Test interception message.";
        var isCanBePutOnMap = true;

        // Act
        var interceptionMessage = InterceptionMessage.Create(
            observedDate,
            frequencyCode,
            divisionName,
            RegistryInterceptionAction.Create(interceptionAction),
            messageText,
            isCanBePutOnMap,
            unknownParticipantCount
        );

        // Assert
        Assert.Equal(observedDate, interceptionMessage.ObservedDate);
        Assert.Equal(frequencyCode, interceptionMessage.FrequencyCode);
        Assert.Equal(divisionName, interceptionMessage.DivisionName);
        Assert.Equal(interceptionAction, interceptionMessage.InterceptionAction?.Name);
        Assert.Equal(unknownParticipantCount, interceptionMessage.UnknownParticipantCount);
        Assert.Equal(messageText, interceptionMessage.MessageText);
        Assert.Equal(isCanBePutOnMap, interceptionMessage.IsCanBePutOnMap);
        Assert.NotEqual(default, interceptionMessage.CreatedAt);
    }

    [Fact]
    public void CreateInterceptionMessage_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var observedDate = DateTime.UtcNow;
        var frequencyCode = "123.45 MHz";
        var divisionName = "Division A";
        var interceptionAction = "Moving";
        var messageText = "Test interception message.";
        var isCanBePutOnMap = true;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(
            default,
            frequencyCode,
            divisionName,
            RegistryInterceptionAction.Create(interceptionAction),
            messageText,
            isCanBePutOnMap
        ));
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(
            observedDate,
            string.Empty,
            divisionName,
            RegistryInterceptionAction.Create(interceptionAction),
            messageText,
            isCanBePutOnMap
        ));
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(
            observedDate,
            frequencyCode,
            divisionName,
            RegistryInterceptionAction.Create(interceptionAction),
            string.Empty,
            isCanBePutOnMap
        ));
    }

    [Fact]
    public void UpdateInterceptionMessage_ShouldUpdateProperties()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );
        var newFrequencyCode = "543.21 MHz";
        var newDivisionName = "Division B";
        var newInterceptionAction = RegistryInterceptionAction.Create("Stationary");
        var newMessageText = "Updated interception message.";
        var newIsCanBePutOnMap = false;
        var unknownParticipantCount = 2;

        // Act
        interceptionMessage.Update(
            newFrequencyCode,
            newDivisionName,
            newInterceptionAction,
            newMessageText,
            newIsCanBePutOnMap,
            unknownParticipantCount
        );

        // Assert
        Assert.Equal(newFrequencyCode, interceptionMessage.FrequencyCode);
        Assert.Equal(newDivisionName, interceptionMessage.DivisionName);
        Assert.Equal(newInterceptionAction.Name, interceptionMessage.InterceptionAction?.Name);
        Assert.Equal(newMessageText, interceptionMessage.MessageText);
        Assert.Equal(newIsCanBePutOnMap, interceptionMessage.IsCanBePutOnMap);
        Assert.Equal(unknownParticipantCount, interceptionMessage.UnknownParticipantCount);
        Assert.True(interceptionMessage.UpdatedAt > interceptionMessage.CreatedAt);
    }

    [Fact]
    public void UpdateInterceptionMessage_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => interceptionMessage.Update(
            string.Empty,
            "Division B",
            RegistryInterceptionAction.Create("Stationary"),
            "Updated interception message.",
            false,
            2
        ));
        Assert.Throws<ArgumentException>(() => interceptionMessage.Update(
            "543.21 MHz",
            "Division B",
            RegistryInterceptionAction.Create("Stationary"),
            string.Empty,
            false,
            2
        ));
    }

    [Fact]
    public void AddParticipant_ShouldAddParticipantToInterceptionMessage()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );
        var participant = RegistryInterceptionParticipant.Create(
            "Participant A",
            "Role A"
        );

        // Act
        interceptionMessage.AddParticipant(participant);

        // Assert
        Assert.Contains(participant, interceptionMessage.Participants);
    }

    [Fact]
    public void AddParticipant_ShouldThrowException_WhenParticipantIsNull()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => interceptionMessage.AddParticipant(null!));
    }

    [Fact]
    public void AddParticipant_ShouldThrowException_WhenParticipantAlreadyExists()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );
        var participant = RegistryInterceptionParticipant.Create(
            "Participant A",
            "Role A"
        );

        interceptionMessage.AddParticipant(participant);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => interceptionMessage.AddParticipant(participant));
    }

    [Fact]
    public void RemoveParticipant_ShouldRemoveParticipantFromInterceptionMessage()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );
        var participant = RegistryInterceptionParticipant.Create(
            "Participant A",
            "Role A"
        );
        interceptionMessage.AddParticipant(participant);

        // Act
        interceptionMessage.RemoveParticipant(participant.Id);

        // Assert
        Assert.DoesNotContain(participant, interceptionMessage.Participants);
    }

    [Fact]
    public void RemoveParticipant_ShouldThrowException_WhenParticipantDoesNotExist()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );
        var nonExistentParticipantId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => interceptionMessage.RemoveParticipant(nonExistentParticipantId));
    }

    [Fact]
    public void RemoveParticipant_ShouldThrowException_WhenParticipantIdIsEmpty()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => interceptionMessage.RemoveParticipant(Guid.Empty));
    }

    [Fact]
    public void Update_ShouldUpdateUpdatedAtProperty()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );
        var initialUpdatedAt = interceptionMessage.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(1000); // Ensure time difference
        interceptionMessage.Update(
            "543.21 MHz",
            "Division B",
            RegistryInterceptionAction.Create("Stationary"),
            "Updated interception message.",
            false,
            2
        );

        // Assert
        Assert.True(interceptionMessage.UpdatedAt > initialUpdatedAt);
    }

    [Fact]
    public void SetUnknownParticipantCount_ShouldUpdateUnknownParticipantCount()
    {
        // Arrange
        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            "123.45 MHz",
            "Division A",
            RegistryInterceptionAction.Create("Moving"),
            "Test interception message.",
            true
        );

        // Act
        interceptionMessage.SetUnknownParticipantCount(5);

        // Assert
        Assert.Equal(5, interceptionMessage.UnknownParticipantCount);
    }
}
