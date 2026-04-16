//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;

namespace eInterceptionTests.Domain;

public class InterceptionParticipantLinkTests
{
    [Fact]
    public void Create_ShouldCreateValidLink()
    {
        // Arrange
        var interceptionMessageId = Guid.NewGuid();
        var participantId = Guid.NewGuid();

        // Act
        var link = InterceptionParticipantLink.Create(interceptionMessageId, participantId);

        // Assert
        Assert.NotNull(link);
        Assert.Equal(interceptionMessageId, link.InterceptionMessageId);
        Assert.Equal(participantId, link.RegistryInterceptionParticipantId);
        Assert.NotEqual(Guid.Empty, link.Id);
        Assert.True(link.CreatedAtUtc <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenInterceptionMessageIdIsEmpty()
    {
        // Arrange
        var participantId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            InterceptionParticipantLink.Create(Guid.Empty, participantId));

        Assert.Equal("Ид спостереження є обов'язковим.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenParticipantIdIsEmpty()
    {
        // Arrange
        var interceptionMessageId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            InterceptionParticipantLink.Create(interceptionMessageId, Guid.Empty));

        Assert.Equal("Ід учасника є обов'язковим.", exception.Message);
    }
}
