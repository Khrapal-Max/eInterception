//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Дочірня сутність агрегату <see cref="InterceptionMessage"/>,
/// яка описує відомого учасника конкретного спостереження.
/// Учасник завжди посилається на канонічний профіль військової особи.
/// </summary>
public sealed class InterceptionParticipant
{
    /// <summary>
    /// Локальний ідентифікатор учасника всередині агрегату спостереження.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Позивний учасника у межах конкретного спостереження.
    /// Це snapshot-значення факту, навіть якщо профіль надалі буде змінено.
    /// </summary>
    public string Callsign { get; private set; } = string.Empty;

    /// <summary>
    /// Локально зафіксована назва підрозділу у межах конкретного спостереження.
    /// Це snapshot-значення факту, а не канонічний link на профіль підрозділу.
    /// </summary>
    public DivisionNameVo? DivisionName { get; private set; }

    /// <summary>
    /// Ідентифікатор ролі з довідника ролей учасників.
    /// Може бути відсутнім, якщо роль не визначено.
    /// </summary>
    public Guid? RegistryInterceptionParticipantRoleId { get; private set; }

    /// <summary>
    /// Ідентифікатор канонічного профілю військової особи.
    /// Є обов'язковим і встановлюється лише в момент створення учасника.
    /// </summary>
    public Guid MilitaryProfileId { get; private set; }

    /// <summary>
    /// Створює нового відомого учасника спостереження.
    /// Учасник завжди має бути прив'язаний до існуючого профілю.
    /// </summary>
    public static InterceptionParticipant Create(
        Guid militaryProfileId,
        string callsign,
        string? divisionName = null,
        Guid? registryInterceptionParticipantRoleId = null)
    {
        if (string.IsNullOrWhiteSpace(callsign))
            throw new ArgumentException("Позивний учасника є обов'язковим.", nameof(callsign));

        if (militaryProfileId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор профілю є обов'язковим.", nameof(militaryProfileId));

        return new InterceptionParticipant
        {
            Id = Guid.NewGuid(),
            Callsign = callsign.Trim(),
            DivisionName = DivisionNameVo.Create(divisionName),
            RegistryInterceptionParticipantRoleId = registryInterceptionParticipantRoleId,
            MilitaryProfileId = militaryProfileId
        };
    }
}