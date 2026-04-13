//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Сутність, що описує учасника перехоплення, який є особою. 
/// Містить інформацію про учасника перехоплення, таку як його ім'я, роль та інші характеристики.
/// </summary>
public sealed class InterceptParticipant
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Позивинй, не унікальний, може повторюватися.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Частота радіоперехоплення, не унікальна, може повторюватися.
    /// </summary>
    public string FrequencyCode { get; private set; } = string.Empty;

    /// <summary>
    /// Назва підрозділу, не унікальна, може повторюватися.
    /// </summary>
    public string? DivisionName { get; private set; }

    /// <summary>
    /// Дата та час створення запису про учасника перехоплення.
    /// Встановлюється при створенні об'єкта і не змінюється.
    /// Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення запису про учасника перехоплення.
    /// Встановлюється при створенні об'єкта і оновлюється щоразу, коли змінюються властивості Name, FrequencyCode або DivisionName.
    /// Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    public static InterceptParticipant Create(string name, string frequencyCode, string? divisionName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("FrequencyCode cannot be null or whitespace.", nameof(frequencyCode));

        return new InterceptParticipant
        {
            Id = Guid.NewGuid(),
            Name = name,
            FrequencyCode = frequencyCode,
            DivisionName = divisionName,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
    }

    public void Update(string name, string frequencyCode, string? divisionName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("FrequencyCode cannot be null or whitespace.", nameof(frequencyCode));

        Name = name;
        FrequencyCode = frequencyCode;
        DivisionName = divisionName;
        UpdatedAt = DateTime.Now.ToUniversalTime();
    }
}
