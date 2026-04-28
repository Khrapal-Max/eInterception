//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace WebUI.Domain.Entities;

/// <summary>
/// Дочірня сутність агрегату <see cref="InterceptionMessage"/>,
/// яка описує напрямок командного зв'язку між двома відомими учасниками
/// в межах одного спостереження.
/// </summary>
public sealed class InterceptionCommandVector
{
    /// <summary>
    /// Локальний ідентифікатор вектора всередині агрегату спостереження.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Ідентифікатор учасника-ініціатора команди.
    /// </summary>
    public Guid FromParticipantId { get; private set; }

    /// <summary>
    /// Ідентифікатор учасника-отримувача команди.
    /// </summary>
    public Guid ToParticipantId { get; private set; }

    /// <summary>
    /// Створює новий вектор командування між двома різними учасниками.
    /// </summary>
    public static InterceptionCommandVector Create(
        Guid fromParticipantId,
        Guid toParticipantId)
    {
        if (fromParticipantId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор учасника-ініціатора є обов'язковим.", nameof(fromParticipantId));

        if (toParticipantId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор учасника-отримувача є обов'язковим.", nameof(toParticipantId));

        if (fromParticipantId == toParticipantId)
            throw new InvalidOperationException("Циклічний вектор командування не дозволений.");

        return new InterceptionCommandVector
        {
            Id = Guid.NewGuid(),
            FromParticipantId = fromParticipantId,
            ToParticipantId = toParticipantId
        };
    }

    /// <summary>
    /// Перевіряє, чи посилається вектор на вказаного учасника.
    /// </summary>
    public bool ReferencesParticipant(Guid participantId)
        => FromParticipantId == participantId || ToParticipantId == participantId;
}
