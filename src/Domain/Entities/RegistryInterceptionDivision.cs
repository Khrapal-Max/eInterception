//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Сутність яка описує підрозділ.
/// 
/// Створюється, при доданні переховплення в журналі прехоплень коли ідентифікується нова частота.
/// Має власний регіст, де може бути оновлена.
/// </summary>
public sealed class RegistryInterceptionDivision
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Частота радіоперехоплення, унікальна не повторюється в регістрі.
    /// </summary>
    public string FrequencyCode { get; private set; } = string.Empty;

    /// <summary>
    /// Назва підрозділу, не унікальна, може повторюватися в регістрі.
    /// </summary>
    public string? DivisionName { get; private set; }     

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static RegistryInterceptionDivision Create(string frequencyCode, string? divisionName = null)
    {
        if (string.IsNullOrWhiteSpace(frequencyCode))
            throw new ArgumentException("Частота радіоперехоплення обов'язкова.", nameof(frequencyCode));

        return new RegistryInterceptionDivision
        {
            Id = Guid.NewGuid(),
            FrequencyCode = frequencyCode.Trim(),
            DivisionName = divisionName?.Trim() ?? null
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string? divisionName = null)
    {
        DivisionName = divisionName?.Trim() ?? null;
    }
}
