//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

public sealed class InterceptionMessage
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Дата спостереження. Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime ObservedDate { get; private set; }

    /// <summary>
    /// Частота радіоперехоплення, не унікальна, може повторюватися.
    /// </summary>
    public string FrequencyCode { get; private set; } = string.Empty;

    /// <summary>
    /// Назва підрозділу, не унікальна, може повторюватися.
    /// </summary>
    public string? DivisionName { get; set; }

    /// <summary>
    /// ДІя - характеризує та коротко описує зміст радіоперехоплення.
    /// Дії беруться з регістру активних дій.
    /// </summary>
    public Guid? InterceptionActionId { get; private set; }
    public InterceptionAction? InterceptionAction { get; private set; }

    /// <summary>
    /// Запис радіоперехоплення, який містить текстову інформацію про перехоплення.
    /// </summary>
    public string MessageText { get; private set; } = string.Empty;

    /// <summary>
    /// Під час створення об'єкта визначається, чи може це перехоплення бути розміщене на карті.
    /// </summary>
    public bool IsCanBePutOnMap { get; private set; }

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

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static InterceptionMessage Create(DateTime observedDate,
        string frequencyCode,
        string? divisionName,
        InterceptionAction interceptionAction,
        string messageText,
        bool isCanBePutOnMap)
    {
        ArgumentNullException.ThrowIfNull(interceptionAction);

        if (observedDate == default)
            throw new ArgumentException("Дата спостереження обов'язкова.", nameof(observedDate));

        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("Частота радіоперехоплення обов'язкова.", nameof(frequencyCode));

        if (string.IsNullOrWhiteSpace(messageText))
            throw new ArgumentException("Текст повідомлення обов'язковий.", nameof(messageText));

        return new InterceptionMessage
        {
            Id = Guid.NewGuid(),
            ObservedDate = observedDate.ToUniversalTime(),
            FrequencyCode = frequencyCode,
            DivisionName = divisionName,
            InterceptionActionId = interceptionAction.Id,
            MessageText = messageText,
            IsCanBePutOnMap = isCanBePutOnMap,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string frequencyCode,
        string? divisionName,
        InterceptionAction interceptionAction,
        string messageText,
        bool isCanBePutOnMap)
    {
        ArgumentNullException.ThrowIfNull(interceptionAction);

        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("Частота радіоперехоплення обов'язкова.", nameof(frequencyCode));

        if (string.IsNullOrWhiteSpace(messageText))
            throw new ArgumentException("Текст повідомлення обов'язковий.", nameof(messageText));

        FrequencyCode = frequencyCode;
        DivisionName = divisionName;
        InterceptionActionId = interceptionAction.Id;
        MessageText = messageText;
        IsCanBePutOnMap = isCanBePutOnMap;
        UpdatedAt = DateTime.UtcNow;
    }
}
