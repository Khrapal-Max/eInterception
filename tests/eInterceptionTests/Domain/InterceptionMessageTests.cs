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
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var unknownParticipantCount = 3;
        var messageText = "Test interception message.";
        var сanBePutOnMap = true;

        // Act
        var interceptionMessage = InterceptionMessage.Create(
            observedDate,
            interceptionDivision.Id,
            interceptionAction.Id,
            messageText,
            сanBePutOnMap,
            unknownParticipantCount
        );

        // Assert
        Assert.Equal(observedDate, interceptionMessage.ObservedDate);
        Assert.Equal(interceptionDivision.Id, interceptionMessage.RegistryInterceptionDivisionId);
        Assert.Equal(interceptionAction.Id, interceptionMessage.RegistryInterceptionActionId);
        Assert.Equal(unknownParticipantCount, interceptionMessage.UnknownParticipantCount);
        Assert.Equal(messageText, interceptionMessage.MessageText);
        Assert.Equal(сanBePutOnMap, interceptionMessage.CanBePutOnMap);
        Assert.NotEqual(default, interceptionMessage.CreatedAt);
    }

    [Fact]
    public void CreateInterceptionMessage_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var observedDate = DateTime.UtcNow;
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var messageText = "Test interception message.";
        var сanBePutOnMap = true;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(
            default,
            interceptionDivision.Id,
            interceptionAction.Id,
            messageText,
            сanBePutOnMap
        ));
        Assert.Throws<ArgumentException>(() => InterceptionMessage.Create(
            observedDate,
            interceptionDivision.Id,
            interceptionAction.Id,
            string.Empty,
            сanBePutOnMap
        ));
    }

    [Fact]
    public void UpdateInterceptionMessage_ShouldUpdateProperties()
    {
        // Arrange
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");
        var interceptionAction = RegistryInterceptionAction.Create("Moving");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );

        var newInterceptionDivision = RegistryInterceptionDivision.Create("543.21 MHz", "Division B");
        var newInterceptionAction = RegistryInterceptionAction.Create("Stationary");
        var newMessageText = "Updated interception message.";
        var newCanBePutOnMap = false;
        var unknownParticipantCount = 2;

        // Act
        interceptionMessage.Update(
            newInterceptionDivision.Id,
            newInterceptionAction.Id,
            newMessageText,
            newCanBePutOnMap,
            unknownParticipantCount
        );

        // Assert
        Assert.Equal(newInterceptionDivision.Id, interceptionMessage.RegistryInterceptionDivisionId);
        Assert.Equal(newInterceptionAction.Id, interceptionMessage.RegistryInterceptionActionId);
        Assert.Equal(newMessageText, interceptionMessage.MessageText);
        Assert.Equal(newCanBePutOnMap, interceptionMessage.CanBePutOnMap);
        Assert.Equal(unknownParticipantCount, interceptionMessage.UnknownParticipantCount);
        Assert.True(interceptionMessage.UpdatedAt > interceptionMessage.CreatedAt);
    }

    [Fact]
    public void UpdateInterceptionMessage_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var newInterceptionAction = RegistryInterceptionAction.Create("Stationary");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");
        var newInterceptionDivision = RegistryInterceptionDivision.Create("543.21 MHz", "Division B");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => interceptionMessage.Update(
            Guid.Empty,
            newInterceptionAction.Id,
            "Updated interception message.",
            false,
            2
        ));
        Assert.Throws<ArgumentException>(() => interceptionMessage.Update(
            newInterceptionDivision.Id,
            Guid.Empty,
            "Updated interception message.",
            false,
            2
        ));
        Assert.Throws<ArgumentException>(() => interceptionMessage.Update(
            newInterceptionDivision.Id,
            newInterceptionAction.Id,
            string.Empty,
            false,
            2
        ));
    }

    [Fact]
    public void AddParticipant_ShouldAddParticipantToInterceptionMessage()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );
        var participant = Guid.NewGuid();

        // Act
        interceptionMessage.AddParticipant(participant);

        // Assert
        Assert.Contains(interceptionMessage.Participants, x => x.RegistryInterceptionParticipantId == participant);
    }

    [Fact]
    public void AddParticipant_ShouldThrowException_WhenParticipantIsNull()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => interceptionMessage.AddParticipant(Guid.Empty));
    }

    [Fact]
    public void AddParticipant_ShouldThrowException_WhenParticipantAlreadyExists()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );
        var participant = Guid.NewGuid();

        interceptionMessage.AddParticipant(participant);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => interceptionMessage.AddParticipant(participant));
    }

    [Fact]
    public void RemoveParticipant_ShouldRemoveParticipantFromInterceptionMessage()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );
        var participant = Guid.NewGuid();
        interceptionMessage.AddParticipant(participant);

        // Act
        interceptionMessage.RemoveParticipant(participant);

        // Assert
        Assert.DoesNotContain(interceptionMessage.Participants, x => x.RegistryInterceptionParticipantId == participant);
    }

    [Fact]
    public void RemoveParticipant_ShouldThrowException_WhenParticipantDoesNotExist()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
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
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
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
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var newInterceptionAction = RegistryInterceptionAction.Create("Stationary");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );
        var initialUpdatedAt = interceptionMessage.UpdatedAt;
        var newInterceptionDivision = RegistryInterceptionDivision.Create("543.21 MHz", "Division B");

        // Act
        Thread.Sleep(1000); // Ensure time difference
        interceptionMessage.Update(
            newInterceptionDivision.Id,
            newInterceptionAction.Id,
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
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );

        // Act
        interceptionMessage.SetUnknownParticipantCount(5);

        // Assert
        Assert.Equal(5, interceptionMessage.UnknownParticipantCount);
    }

    [Fact]
    public void SetUnknownParticipantCount_ShouldThrowException_WhenCountIsNegative()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => interceptionMessage.SetUnknownParticipantCount(-1));
    }

    [Fact]
    public void CanBePutOnMap_UpdateShouldChangeCanBePutOnMapValue()
    {
        // Arrange
        var interceptionAction = RegistryInterceptionAction.Create("Moving");
        var interceptionDivision = RegistryInterceptionDivision.Create("123.45 MHz", "Division A");

        var interceptionMessage = InterceptionMessage.Create(
            DateTime.UtcNow,
            interceptionDivision.Id,
            interceptionAction.Id,
            "Test interception message.",
            true
        );

        var newInterceptionDivision = RegistryInterceptionDivision.Create("543.21 MHz", "Division B");

        // Act
        interceptionMessage.Update(
            newInterceptionDivision.Id,
            interceptionAction.Id,
            "Updated interception message.",
            false,
            2
        );

        // Assert
        Assert.False(interceptionMessage.CanBePutOnMap);
    }
}
