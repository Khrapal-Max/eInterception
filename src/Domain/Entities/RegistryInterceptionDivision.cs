//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.ValueObjects;

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
    public FrequencyCodeVo FrequencyCode { get; private set; }

    /// <summary>
    /// Назва підрозділу, не унікальна, може повторюватися в регістрі.
    /// </summary>
    public DivisionNameVo? DivisionName { get; private set; }

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static RegistryInterceptionDivision Create(string frequencyCode, string? divisionName = null)
    {
        var normalizedFrequencyCode = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentNullException(nameof(frequencyCode), "Частота радіоперехоплення обов'язкова.");

        var normalizedDivisionName = DivisionNameVo.Create(divisionName);

        return new RegistryInterceptionDivision
        {
            Id = Guid.NewGuid(),
            FrequencyCode = normalizedFrequencyCode,
            DivisionName = normalizedDivisionName
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string? divisionName = null)
    {
        DivisionName = DivisionNameVo.Create(divisionName);
    }
}
