//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Довідникова роль учасника перехоплення.
/// Унікальна по назві і використовується для нормалізації ролей у системі.
/// Має власний регістр, який містить всі можливі ролі осіб. 
/// Створюється і оновлюється лише в реігстрі ролей учасників перехоплення, а не в журналі перехоплень.
/// </summary>
public sealed class RegistryInterceptionParticipantRole
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Назва ролі (посада). Є унікальною в межах регістру ролей осіб. 
    /// </summary>
    public ParticipantRoleNameVo Name { get; private set; }

    /// <summary>
    /// Опис ролі, який надає додаткову інформацію про роль учасника перехоплення. 
    /// Не є обов'язковим і може бути порожнім.
    /// </summary>
    public string? Description { get; private set; }

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static RegistryInterceptionParticipantRole Create(string name, string? description = null)
    {
        var normalizedName = ParticipantRoleNameVo.Create(name)
            ?? throw new ArgumentException("Назва ролі обов'язкова.", nameof(name));

        return new RegistryInterceptionParticipantRole
        {
            Id = Guid.NewGuid(),
            Name = normalizedName,
            Description = description?.Trim() ?? null
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------
    public void Update(string name, string? description = null)
    {
        var normalizedName = ParticipantRoleNameVo.Create(name)
           ?? throw new ArgumentException("Назва ролі обов'язкова.", nameof(name));

        Name = normalizedName;
        Description = description?.Trim() ?? null;
    }
}
