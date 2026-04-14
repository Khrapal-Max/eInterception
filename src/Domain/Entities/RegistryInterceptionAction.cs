//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Сутність, що описує дію перехоплення.
/// Має власний регістр, який містить всі можливі дії перехоплення, 
/// що використовуються для категоризації та опису змісту радіоперехоплення.
/// 
/// Створюється або онволюється лише в регістрі
/// </summary>
public sealed class RegistryInterceptionAction
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Назва дії. Є унікальною в межах регістру дій перехоплення. 
    /// Використовується для категоризації та опису змісту радіоперехоплення. 
    /// Наприклад: "Спостереження", "Загроза", "Підтримка" тощо.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Опис дії, який надає додаткову інформацію про зміст дії перехоплення. 
    /// Не є обов'язковим і може бути порожнім.
    /// </summary>
    public string? Description { get; private set; }

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static RegistryInterceptionAction Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва дії обов'язкова.", nameof(name));

        return new RegistryInterceptionAction
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? null
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва дії обов'язкова.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim() ?? null;
    }
}