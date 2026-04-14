//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Довідникова роль учасника перехоплення.
/// Унікальна по назві і використовується для нормалізації ролей у системі.
/// Має власний регістр, який містить всі можливі ролі осіб. 
/// </summary>
public sealed class InterceptionParticipantRole
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Назва ролі (посада). Є унікальною в межах регістру ролей осіб. 
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Опис ролі, який надає додаткову інформацію про роль учасника перехоплення. 
    /// Не є обов'язковим і може бути порожнім.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static InterceptionParticipantRole Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва ролі обов'язкова.", nameof(name));

        return new InterceptionParticipantRole
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва ролі обов'язкова.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }
}
