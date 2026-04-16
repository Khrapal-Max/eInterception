//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

using Domain.ValueObjects;

/// <summary>
/// Aggregate root спостереження перехоплення.
/// Містить факт спостереження, відомих учасників, кількість невідомих учасників
/// та вектори командування між відомими учасниками.
/// </summary>
public sealed class InterceptionMessage
{
    /// <summary>
    /// Ідентифікатор спостереження.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Дата та час фактичного спостереження перехоплення у форматі UTC.
    /// </summary>
    public DateTime ObservedDate { get; private set; }

    /// <summary>
    /// Частота радіосигналу, на якій було зафіксовано перехоплення.
    /// </summary>
    public FrequencyCodeVo FrequencyCode { get; private set; }

    /// <summary>
    /// Підрозділ, якому атрибутовано спостереження.
    /// Може бути відсутнім, якщо підрозділ не визначено.
    /// </summary>
    public DivisionNameVo? DivisionName { get; private set; }

    /// <summary>
    /// Ідентифікатор дії з довідника дій перехоплення.
    /// Live-reference на довідник використовується у read-side та application layer.
    /// </summary>
    public Guid RegistryInterceptionActionId { get; private set; }

    /// <summary>
    /// Кількість невідомих учасників, які були присутні у спостереженні,
    /// але не були внесені як окремі відомі учасники.
    /// </summary>
    public int UnknownParticipantCount { get; private set; }

    /// <summary>
    /// Текст спостереження.
    /// </summary>
    public string MessageText { get; private set; } = string.Empty;

    /// <summary>
    /// Ознака того, що спостереження достатньо деталізоване для нанесення на карту.
    /// </summary>
    public bool CanBePutOnMap { get; private set; }

    /// <summary>
    /// Дата та час створення агрегату у форматі UTC.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення агрегату у форматі UTC.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Відомі учасники спостереження.
    /// </summary>
    public IReadOnlyCollection<InterceptionParticipant> Participants => _participants;
    private readonly List<InterceptionParticipant> _participants = [];

    /// <summary>
    /// Вектори командування між відомими учасниками спостереження.
    /// </summary>
    public IReadOnlyCollection<InterceptionCommandVector> CommandVectors => _commandVectors;
    private readonly List<InterceptionCommandVector> _commandVectors = [];

    /// <summary>
    /// Створює нове спостереження перехоплення.
    /// </summary>
    public static InterceptionMessage Create(
        DateTime observedDate,
        string frequencyCode,
        string? divisionName,
        Guid registryInterceptionActionId,
        string messageText,
        bool canBePutOnMap,
        int unknownParticipantCount = 0)
    {
        if (observedDate == default)
            throw new ArgumentException("Дата та час спостереження є обов'язковими.", nameof(observedDate));

        if (registryInterceptionActionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор дії є обов'язковим.", nameof(registryInterceptionActionId));

        if (unknownParticipantCount < 0)
            throw new ArgumentException("Кількість невідомих учасників не може бути від'ємною.", nameof(unknownParticipantCount));

        var normalizedFrequencyCode = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Частота перехоплення є обов'язковою.", nameof(frequencyCode));

        return new InterceptionMessage
        {
            Id = Guid.NewGuid(),
            ObservedDate = observedDate.ToUniversalTime(),
            FrequencyCode = normalizedFrequencyCode,
            DivisionName = DivisionNameVo.Create(divisionName),
            RegistryInterceptionActionId = registryInterceptionActionId,
            MessageText = messageText?.Trim() ?? string.Empty,
            CanBePutOnMap = canBePutOnMap,
            UnknownParticipantCount = unknownParticipantCount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Оновлює атрибути спостереження, що не порушують межі агрегату.
    /// </summary>
    public void Update(
        string frequencyCode,
        string? divisionName,
        Guid registryInterceptionActionId,
        bool canBePutOnMap,
        int unknownParticipantCount = 0)
    {
        if (registryInterceptionActionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор дії є обов'язковим.", nameof(registryInterceptionActionId));

        if (unknownParticipantCount < 0)
            throw new ArgumentException("Кількість невідомих учасників не може бути від'ємною.", nameof(unknownParticipantCount));

        var normalizedFrequencyCode = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Частота перехоплення є обов'язковою.", nameof(frequencyCode));

        FrequencyCode = normalizedFrequencyCode;
        DivisionName = DivisionNameVo.Create(divisionName);
        RegistryInterceptionActionId = registryInterceptionActionId;
        CanBePutOnMap = canBePutOnMap;
        UnknownParticipantCount = unknownParticipantCount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Додає відомого учасника до спостереження.
    /// Учасник має бути попередньо зіставлений з існуючим або щойно створеним профілем.
    /// </summary>
    public Guid AddParticipant(
        string callsign,
        Guid militaryProfileId,
        Guid? registryInterceptionParticipantRoleId = null)
    {
        if (string.IsNullOrWhiteSpace(callsign))
            throw new ArgumentException("Позивний учасника є обов'язковим.", nameof(callsign));

        if (militaryProfileId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор військового профілю є обов'язковим.", nameof(militaryProfileId));

        if (registryInterceptionParticipantRoleId == Guid.Empty)
            throw new ArgumentException("Порожній ідентифікатор ролі не допускається. Використовуйте null для очищення ролі.",
                nameof(registryInterceptionParticipantRoleId));

        var participant = InterceptionParticipant.Create(
            callsign,
            militaryProfileId,
            registryInterceptionParticipantRoleId);

        _participants.Add(participant);
        UpdatedAt = DateTime.UtcNow;

        return participant.Id;
    }

    /// <summary>
    /// Оновлює учасника без видалення та повторного створення.
    /// Це дозволяє зберегти локальну ідентичність учасника та пов'язані з ним вектори.
    /// </summary>
    public void UpdateParticipant(
        Guid participantId,
        string callsign,
        Guid? registryInterceptionParticipantRoleId = null)
    {
        var participant = FindParticipant(participantId);

        participant.Update(
            callsign,
            registryInterceptionParticipantRoleId);

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Видаляє учасника зі спостереження.
    /// Усі вектори командування, що посилаються на цього учасника,
    /// видаляються каскадно для збереження цілісності агрегату.
    /// </summary>
    public void RemoveParticipant(Guid participantId)
    {
        var participant = FindParticipant(participantId);

        _commandVectors.RemoveAll(vector => vector.ReferencesParticipant(participantId));
        _participants.Remove(participant);

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Додає новий вектор командування між двома існуючими учасниками.
    /// </summary>
    public void AddCommandVector(Guid fromParticipantId, Guid toParticipantId)
    {
        if (fromParticipantId == toParticipantId)
            throw new InvalidOperationException("Циклічний вектор командування не дозволений.");

        EnsureParticipantExists(fromParticipantId, nameof(fromParticipantId));
        EnsureParticipantExists(toParticipantId, nameof(toParticipantId));

        if (_commandVectors.Any(v => v.FromParticipantId == fromParticipantId && v.ToParticipantId == toParticipantId))
            return;

        _commandVectors.Add(InterceptionCommandVector.Create(fromParticipantId, toParticipantId));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Видаляє існуючий вектор командування.
    /// </summary>
    public void RemoveCommandVector(Guid fromParticipantId, Guid toParticipantId)
    {
        var vector = _commandVectors.FirstOrDefault(v =>
                v.FromParticipantId == fromParticipantId &&
                v.ToParticipantId == toParticipantId)
            ?? throw new InvalidOperationException("Вектор командування не знайдено.");

        _commandVectors.Remove(vector);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Встановлює кількість невідомих учасників спостереження.
    /// </summary>
    public void SetUnknownParticipantCount(int value)
    {
        if (value < 0)
            throw new ArgumentException("Кількість невідомих учасників не може бути від'ємною.", nameof(value));

        UnknownParticipantCount = value;
        UpdatedAt = DateTime.UtcNow;
    }

    private InterceptionParticipant FindParticipant(Guid participantId)
        => _participants.FirstOrDefault(x => x.Id == participantId)
            ?? throw new InvalidOperationException("Учасника спостереження не знайдено.");

    private void EnsureParticipantExists(Guid participantId, string paramName)
    {
        if (participantId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор учасника є обов'язковим.", paramName);

        if (_participants.All(x => x.Id != participantId))
            throw new InvalidOperationException("Посилання на учасника спостереження відсутнє.");
    }
}
