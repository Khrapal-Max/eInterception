//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Сутність, що описує учасника перехоплення, який є особою. 
/// Містить інформацію про учасника перехоплення, таку як його ім'я, роль та інші характеристики.
/// 
/// <list type="bullet">
///     <item>створюються в результаті радіоперехоплення, коли ідентифікується новий учасник (відсутній в регістрі учасників перехоплення)</item>
///     <item>або оновлюється інформація про існуючого учасника (зміна позивного, частоти, ролі або підрозділу)</item>
///     <list type="bullet">    
///         <item>в радіоперехоплені</item>
///         <item>в регістрі учасників перехоплення</item>
///     </list>   
/// </list>
///     
/// Є унікальним за комбінацією позивного, частоти та підрозділу.
/// </summary>
public sealed class RegistryInterceptionParticipant
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
    /// Назва ролі учасника перехоплення, не унікальна,
    /// може повторюватися. Наприклад: "Командир", "Радист", "Спостерігач" тощо.
    /// </summary>
    public string? Role { get; private set; }

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

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static RegistryInterceptionParticipant Create(string name, string frequencyCode, string? divisionName = null, string? role = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я учасника обов'язкове.", nameof(name));

        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("Частота радіоперехоплення обов'язкова.", nameof(frequencyCode));

        return new RegistryInterceptionParticipant
        {
            Id = Guid.NewGuid(),
            Name = name,
            FrequencyCode = frequencyCode,
            DivisionName = divisionName,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string name, string frequencyCode, string? divisionName = null, string? role = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я учасника обов'язкове.", nameof(name));

        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("Частота радіоперехоплення обов'язкова.", nameof(frequencyCode));

        Name = name;
        FrequencyCode = frequencyCode;
        DivisionName = divisionName;
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }
}
