//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

using Domain.ValueObjects;

/// <summary>
/// Агрегат спостреження перехоплення, що містить інформацію про спостереження, учасників та їх зв'язки.
/// </summary>
public sealed class InterceptionMessage
{
    public Guid Id { get; private set; }

    /// <summary>
    /// дата та час спостереження перехоплення. Важливо, що це не дата створення повідомлення, 
    /// а саме дата спостереження факту перехоплення, яка може відрізнятися від часу створення запису в базі даних.
    /// Це дозволяє коректно відображати хронологію подій навіть якщо повідомлення було створено з затримкою після факту спостереження.
    /// </summary>
    public DateTime ObservedDate { get; private set; }

    /// <summary>
    /// Частота раідо сигналу, на якій було зафіксовано перехоплення.
    /// </summary>
    public FrequencyCodeVo FrequencyCode { get; private set; }

    /// <summary>
    /// Назва підрозділу, який здійснив перехоплення.
    /// Можливо бути відсутньою, якщо інформація про підрозділ не була надана або невідома на момент створення повідомлення.
    /// </summary>
    public DivisionNameVo? DivisionName { get; private set; }

    /// <summary>
    /// Ид дії, яка була зареєстрована в реєстрі перехоплень. Це дозволяє зв'язати повідомлення з конкретною дією,
    /// навіть якщо повідомлення було створено з затримкою після факту спостереження.
    /// </summary>
    public Guid RegistryInterceptionActionId { get; private set; }

    /// <summary>
    /// Кількість невідомих учасників, які були зафіксовані під час спостереження перехоплення, 
    /// але не були ідентифіковані або додані як окремі учасники.
    /// </summary>
    public int UnknownParticipantCount { get; private set; }

    /// <summary>
    /// Тект спостереження перехоплення, який може містити додаткову інформацію про обставини спостереження,
    /// </summary>
    public string MessageText { get; private set; } = string.Empty;

    /// <summary>
    /// Визначення, чи можно нанести інформацію про це перехоплення на карту.
    /// Це може залежати від наявності географічних координат або інших даних,
    /// які дозволяють точно визначити місцезнаходження події.
    /// </summary>
    public bool CanBePutOnMap { get; private set; }

    /// <summary>
    /// Дата та час створення запису в базі даних. 
    /// Відображає момент, коли інформація про перехоплення була зафіксована в системі,
    /// в форматі UTC для забезпечення уніфікації та коректного відображення хронології подій незалежно від часових поясів.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення запису в базі даних. 
    /// Відображає момент, коли інформація про перехоплення була востаннє змінена або доповнена,
    /// в форматі UTC для забезпечення уніфікації та коректного відображення хронології подій незалежно від часових поясів.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Список відомих учасників, які були зафіксовані під час спостереження перехоплення.
    /// </summary>
    public IReadOnlyCollection<InterceptionParticipant> Participants => _participants;
    private readonly List<InterceptionParticipant> _participants = [];

    /// <summary>
    /// Список внесених діректорних зв'язків між учасниками, які відображають командні або інші відносини між ними.
    /// </summary>
    public IReadOnlyCollection<InterceptionCommandVector> CommandVectors => _commandVectors;
    private readonly List<InterceptionCommandVector> _commandVectors = [];


    //------------------------------------------------------------------------------------------------
    // Factory method to create a new InterceptionMessage with validation and normalization of inputs.
    //------------------------------------------------------------------------------------------------
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
            throw new ArgumentNullException(nameof(observedDate), "Дата та час спостереження обов'язкові");

        if (registryInterceptionActionId == Guid.Empty)
            throw new ArgumentNullException(nameof(registryInterceptionActionId), "Ид дії обов'язковий");

        if (unknownParticipantCount < 0)
            throw new ArgumentNullException(nameof(unknownParticipantCount), "Кількість невідомих учасників не може бути від'ємною.");

        var normalizedFrequencyCode = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentNullException(nameof(frequencyCode), "Частота радіо перехвата обов'язкова.");

        var normalizedDivisionName = DivisionNameVo.Create(divisionName);

        return new InterceptionMessage
        {
            Id = Guid.NewGuid(),
            ObservedDate = observedDate.ToUniversalTime(),
            FrequencyCode = normalizedFrequencyCode,
            DivisionName = normalizedDivisionName,
            RegistryInterceptionActionId = registryInterceptionActionId,
            MessageText = messageText.Trim(),
            CanBePutOnMap = canBePutOnMap,
            UnknownParticipantCount = unknownParticipantCount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    //------------------------------------------------------------------------------------------------
    // Behavior methods to manage participants and command vectors, ensuring that all operations
    // maintain the integrity of the aggregate and update timestamps accordingly.
    //------------------------------------------------------------------------------------------------

    public void Update(
        string frequencyCode,
        string? divisionName,
        Guid registryInterceptionActionId,
        bool canBePutOnMap,
        int unknownParticipantCount = 0)
    {
        if (registryInterceptionActionId == Guid.Empty)
            throw new ArgumentNullException(nameof(registryInterceptionActionId), "Ид дії обов'язковий");

        if (unknownParticipantCount < 0)
            throw new ArgumentNullException(nameof(unknownParticipantCount), "Кількість невідомих учасників не може бути від'ємною.");

        var normalizedFrequencyCode = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentNullException(nameof(frequencyCode), "Частота радіо перехвата обов'язкова.");

        var normalizedDivisionName = DivisionNameVo.Create(divisionName);

        FrequencyCode = normalizedFrequencyCode;
        DivisionName = normalizedDivisionName;
        RegistryInterceptionActionId = registryInterceptionActionId;
        CanBePutOnMap = canBePutOnMap;
        UnknownParticipantCount = unknownParticipantCount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddParticipant(
        string displayName,
        Guid? registryInterceptionParticipantRoleId = null,
        Guid? militaryProfileId = null)
    {
        var p = InterceptionParticipant.Create(Id, displayName, registryInterceptionParticipantRoleId, militaryProfileId);

        _participants.Add(p);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveParticipant(Guid participantId)
    {
        var p = _participants.FirstOrDefault(x => x.Id == participantId)
            ?? throw new InvalidOperationException("Учасник спостереженян не знайдений.");

        _participants.Remove(p);
        UpdatedAt = DateTime.UtcNow;
    }

    public void LinkParticipantToMilitaryProfile(Guid participantLinkId, Guid militaryProfileId)
    {
        if (militaryProfileId == Guid.Empty)
            throw new ArgumentNullException(nameof(militaryProfileId), "Ид профіля обов'язковий.");

        var p = _participants.FirstOrDefault(x => x.Id == participantLinkId)
            ?? throw new InvalidOperationException("Учасник спостереженян не знайдений.");

        p.LinkToMilitaryProfile(militaryProfileId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddCommandVector(Guid fromParticipantId, Guid toParticipantId)
    {
        if (fromParticipantId == toParticipantId)
            throw new InvalidOperationException("Циклічний вектор не дозволено.");

        var fromExists = _participants.Any(x => x.Id == fromParticipantId);
        var toExists = _participants.Any(x => x.Id == toParticipantId);

        if (!fromExists || !toExists)
            throw new InvalidOperationException("Посилання на учасника відсутнє.");

        if (_commandVectors.Any(v => v.FromParticipantLinkId == fromParticipantId
            && v.ToParticipantLinkId == toParticipantId))
            return;

        _commandVectors.Add(InterceptionCommandVector.Create(Id, fromParticipantId, toParticipantId));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveCommandVector(Guid fromParticipantId, Guid toParticipantId)
    {
        var v = _commandVectors
            .FirstOrDefault(v => v.FromParticipantLinkId == fromParticipantId
            && v.ToParticipantLinkId == toParticipantId)
            ?? throw new InvalidOperationException("Вектор не знайдений.");

        _commandVectors.Remove(v);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetUnknownParticipantCount(int value)
    {
        if (value < 0)
            throw new ArgumentNullException(nameof(value), "Кількість невідомих учасників не може бути від'ємною.");

        UnknownParticipantCount = value;
        UpdatedAt = DateTime.UtcNow;
    }
}