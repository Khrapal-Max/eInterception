//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// owned type агрегату спостереження, що представляє вектор перехоплення.
/// </summary>
public sealed class InterceptionCommandVector
{
    public Guid Id { get; private set; }

    /// <summary>
    /// ид спостереження, до якого належить цей вектор перехоплення.
    /// </summary>
    public Guid InterceptionMessageId { get; private set; }

    /// <summary>
    /// Ид учасника перехоплення, який є ініціатором команди.
    /// </summary>
    public Guid FromParticipantLinkId { get; private set; }

    /// <summary>
    /// Ид учасника перехоплення, який є отримувачем команди.
    /// </summary>
    public Guid ToParticipantLinkId { get; private set; }

    public static InterceptionCommandVector Create(
        Guid interceptionMessageId,
        Guid fromParticipantLinkId,
        Guid toParticipantLinkId)
    {
        if (interceptionMessageId == Guid.Empty)
            throw new ArgumentNullException(nameof(interceptionMessageId), "Ид спостереження не може бути порожнім.");

        if (fromParticipantLinkId == Guid.Empty)
            throw new ArgumentNullException(nameof(fromParticipantLinkId), "Ид учасника обов'язкове.");

        if (toParticipantLinkId == Guid.Empty)
            throw new ArgumentNullException(nameof(toParticipantLinkId), "Ид учасника обов'язкове.");

        if (fromParticipantLinkId == toParticipantLinkId)
            throw new InvalidOperationException("Циклічне призначення не можливе.");

        return new InterceptionCommandVector
        {
            Id = Guid.NewGuid(),
            InterceptionMessageId = interceptionMessageId,
            FromParticipantLinkId = fromParticipantLinkId,
            ToParticipantLinkId = toParticipantLinkId
        };
    }
}