//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Сутность, яка описує перехоплення.
/// Є основним елементом журнала перехоплень.
/// </summary>
public sealed class InterceptionMessage
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Дата спостереження. Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime ObservedDate { get; private set; }

    /// <summary>
    /// Частота радіоперехоплення та назва підрозділу, не унікальна, може повторюватися.
    /// </summary>
    public Guid RegistryInterceptionDivisionId { get; private set; }

    /// <summary>
    /// ДІя - характеризує та коротко описує зміст радіоперехоплення.
    /// Дії беруться з регістру активних дій.
    /// </summary>
    public Guid RegistryInterceptionActionId { get; private set; }

    /// <summary>
    /// Кількість невідомих учасників у межах цього спостереження.
    /// НВ не створюються як окремі записи учасників.
    /// </summary>
    public int UnknownParticipantCount { get; private set; }

    /// <summary>
    /// Запис радіоперехоплення, який містить текстову інформацію про перехоплення.
    /// </summary>
    public string MessageText { get; private set; } = string.Empty;

    /// <summary>
    /// Під час створення об'єкта визначається, чи може це перехоплення бути розміщене на карті.
    /// </summary>
    public bool CanBePutOnMap { get; private set; }

    /// <summary>
    /// Дата та час створення запису перехоплення.
    /// Встановлюється при створенні об'єкта і не змінюється.
    /// Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення запису перехоплення.
    /// Встановлюється при створенні об'єкта і оновлюється щоразу, коли змінюються властивості Name, FrequencyCode або DivisionName.
    /// Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Зв'язки з відомими учасниками перехоплення.
    /// </summary>
    public IReadOnlyCollection<InterceptionParticipantLink> Participants => _participants;

    private readonly List<InterceptionParticipantLink> _participants = [];

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static InterceptionMessage Create(DateTime observedDate,
        Guid interceptionDivisionId,
        Guid interceptionActionId,
        string messageText,
        bool canBePutOnMap,
        int unknownParticipantCount = 0)
    {
        if (observedDate == default)
            throw new ArgumentException("Дата спостереження обов'язкова.", nameof(observedDate));

        if (interceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Дані підрозділа перехоплення обов'язковий.", nameof(interceptionDivisionId));

        if (interceptionActionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор дії перехоплення обов'язковий.", nameof(interceptionActionId));

        if (string.IsNullOrWhiteSpace(messageText))
            throw new ArgumentException("Текст повідомлення обов'язковий.", nameof(messageText));

        if (unknownParticipantCount < 0)
            throw new ArgumentException("Кількість невідомих учасників не може бути від'ємною.", nameof(unknownParticipantCount));

        return new InterceptionMessage
        {
            Id = Guid.NewGuid(),
            ObservedDate = observedDate.ToUniversalTime(),
            RegistryInterceptionDivisionId = interceptionDivisionId,
            RegistryInterceptionActionId = interceptionActionId,
            MessageText = messageText,
            CanBePutOnMap = canBePutOnMap,
            UnknownParticipantCount = unknownParticipantCount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(Guid interceptionDivisionId,
        Guid interceptionActionId,
        string messageText,
        bool canBePutOnMap,
        int unknownParticipantCount)
    {
        if (interceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Дані підрозділа перехоплення обов'язковий.", nameof(interceptionDivisionId));

        if (interceptionActionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор дії перехоплення обов'язковий.", nameof(interceptionActionId));

        if (string.IsNullOrWhiteSpace(messageText))
            throw new ArgumentException("Текст повідомлення обов'язковий.", nameof(messageText));

        if (unknownParticipantCount < 0)
            throw new ArgumentException("Кількість невідомих учасників не може бути від'ємною.", nameof(unknownParticipantCount));

        RegistryInterceptionDivisionId = interceptionDivisionId;
        RegistryInterceptionActionId = interceptionActionId;
        MessageText = messageText;
        CanBePutOnMap = canBePutOnMap;
        UnknownParticipantCount = unknownParticipantCount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Додає відомого учасника до перехоплення.
    /// Один і той самий учасник не може бути прив'язаний до одного перехоплення двічі.
    /// </summary>
    public void AddParticipant(Guid participantId)
    {
        if (participantId == Guid.Empty)
            throw new ArgumentException("Ід учасника є обов'язковим.", nameof(participantId));

        if (_participants.Any(x => x.RegistryInterceptionParticipantId == participantId))
            throw new InvalidOperationException("Учасник вже доданий до перехоплення.");

        var participant = InterceptionParticipantLink.Create(Id, participantId);

        _participants.Add(participant);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Видаляє учасника з перехоплення.
    /// </summary>
    public void RemoveParticipant(Guid participantId)
    {
        if (participantId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор учасника не може бути порожнім.", nameof(participantId));

        var participant = _participants
            .FirstOrDefault(x => x.RegistryInterceptionParticipantId == participantId)
            ?? throw new InvalidOperationException("Учасник не знайдений у перехопленні.");

        _participants.Remove(participant);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Встановлює кількість невідомих учасників.
    /// </summary>
    public void SetUnknownParticipantCount(int unknownParticipantCount)
    {
        if (unknownParticipantCount < 0)
            throw new ArgumentException("Кількість невідомих учасників не може бути від'ємною.", nameof(unknownParticipantCount));

        UnknownParticipantCount = unknownParticipantCount;
        UpdatedAt = DateTime.UtcNow;
    }
}
