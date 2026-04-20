//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptionCommandVectorTests
{
    [Fact]
    public void Create_ValidInput_ShouldCreateVector()
    {
        // Arrange
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();

        // Act
        var vector = InterceptionCommandVector.Create(fromId, toId);

        // Assert
        Assert.NotNull(vector);
        Assert.IsType<Guid>(vector.Id);
        Assert.Equal(fromId, vector.FromParticipantId);
        Assert.Equal(toId, vector.ToParticipantId);
    }

    [Fact]
    public void Create_SameParticipantIds_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var participantId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => InterceptionCommandVector.Create(participantId, participantId));
    }

    [Fact]
    public void Create_EmptyFromParticipantId_ShouldThrowArgumentException()
    {
        // Arrange
        var toId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionCommandVector.Create(Guid.Empty, toId));
    }

    [Fact]
    public void Create_EmptyToParticipantId_ShouldThrowArgumentException()
    {
        // Arrange
        var fromId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => InterceptionCommandVector.Create(fromId, Guid.Empty));
    }

    [Fact]
    public void ReferencesParticipant_ShouldReturnTrueIfReferences()
    {
        // Arrange
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        var vector = InterceptionCommandVector.Create(fromId, toId);

        // Act & Assert
        Assert.True(vector.ReferencesParticipant(fromId));
        Assert.True(vector.ReferencesParticipant(toId));
    }

    [Fact]
    public void ReferencesParticipant_ShouldReturnFalseIfNotReferences()
    {
        // Arrange
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var vector = InterceptionCommandVector.Create(fromId, toId);

        // Act & Assert
        Assert.False(vector.ReferencesParticipant(otherId));
    }
}
