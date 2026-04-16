//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Участь конкретного учасника в конкретному спостереженні.
/// Це не реєстр учасників, а link усередині aggregate InterceptionMessage.
/// </summary>
public sealed class InterceptionParticipantLink
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Ид спостереження, до якого належить цей учасник.
    /// </summary>
    public Guid InterceptionMessageId { get; private set; }

    /// <summary>
    /// Ид учасника, який може бути відомим (з реєстру учасників).
    /// </summary>
    public Guid RegistryInterceptionParticipantId { get; private set; }

    /// <summary>
    /// Дата створення запису участі. Встановлюється при створенні об'єкта і не змінюється.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public static InterceptionParticipantLink Create(
        Guid interceptionMessageId,
        Guid participantId)
    {
        if (interceptionMessageId == Guid.Empty)
            throw new ArgumentException("Ид спостереження є обов'язковим.");

        if (participantId == Guid.Empty)
            throw new ArgumentException("Ід учасника є обов'язковим.");

        return new InterceptionParticipantLink
        {
            Id = Guid.NewGuid(),
            InterceptionMessageId = interceptionMessageId,
            RegistryInterceptionParticipantId = participantId,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}