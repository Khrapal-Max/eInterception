//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Підтверджена частота профілю.
/// Є дочірньою сутністю aggregate MilitaryProfile.
/// </summary>
public sealed class MilitaryProfileFrequency
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Ідентифікатор профілю.
    /// </summary>
    public Guid MilitaryProfileId { get; private set; }

    /// <summary>
    /// Ідентифікатор вузла registry division,
    /// який містить frequency + актуальну назву підрозділу.
    /// </summary>
    public Guid RegistryInterceptionDivisionId { get; private set; }

    /// <summary>
    /// Дата та час додавання частоти до профілю.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------
    public static MilitaryProfileFrequency Create(
        Guid militaryProfileId,
        Guid registryInterceptionDivisionId)
    {
        if (militaryProfileId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор профілю є обов'язковим.", nameof(militaryProfileId));

        if (registryInterceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор частоти/підрозділу є обов'язковим.", nameof(registryInterceptionDivisionId));

        return new MilitaryProfileFrequency
        {
            Id = Guid.NewGuid(),
            MilitaryProfileId = militaryProfileId,
            RegistryInterceptionDivisionId = registryInterceptionDivisionId,
            CreatedAt = DateTime.UtcNow
        };
    }
}