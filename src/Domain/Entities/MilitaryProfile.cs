//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Канонічний профіль особи.
/// Є фінальним aggregate root для звітів, аналітики та зведення
/// кількох оперативних проявів однієї особи в один центр.
/// </summary>
public sealed class MilitaryProfile
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Назва особи (позивний).
    /// Саме ця назва повинна використовуватись у звітах.
    /// </summary>
    public string Callsing { get; private set; } = string.Empty;

    /// <summary>
    /// Канонічний підрозділ профілю.
    /// Якщо ще не визначений - null.
    /// </summary>
    public Guid? RegistryInterceptionDivisionId { get; private set; }

    /// <summary>
    /// Роль особи, може бути не визначеною.
    /// </summary>
    public Guid? RegistryInterceptionParticipantRoleId { get; private set; }

    /// <summary>
    /// Підтверджені частоти профілю.
    /// Використовуються для фінальної аналітики і побудови зв'язків.
    /// </summary>
    public IReadOnlyCollection<MilitaryProfileFrequency> Frequencies => _frequencies;

    private readonly List<MilitaryProfileFrequency> _frequencies = [];

    /// <summary>
    /// Дата та час створення профілю.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення профілю.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------

    /// <summary>
    /// Створення профілю військової особи на основі запису учасника перехоплення. 
    /// Назва профілю буде встановлена на основі назви учасника перехоплення, 
    /// а роль буде встановлена на основі ролі учасника перехоплення.
    /// 
    /// Новий профіль створюється для кожного нового учасника перехоплення з регістру учасників перехоплень.
    /// </summary>
    public static MilitaryProfile Create(string callsing,
        Guid registryInterceptionDivisionId,
        Guid? registryInterceptionParticipantRoleId = null)
    {
        if (string.IsNullOrWhiteSpace(callsing))
            throw new ArgumentException("Назва особи не може бути порожньою.", nameof(callsing));

        if (registryInterceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор підрозділу не може бути порожнім.", nameof(registryInterceptionDivisionId));

        var profile = new MilitaryProfile
        {
            Id = Guid.NewGuid(),
            Callsing = callsing.Trim(),
            RegistryInterceptionDivisionId = registryInterceptionDivisionId,
            RegistryInterceptionParticipantRoleId = registryInterceptionParticipantRoleId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        profile._frequencies.Add(
            MilitaryProfileFrequency.Create(profile.Id, registryInterceptionDivisionId));

        return profile;
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Дозволяє змінити підрозділ профілю. Якщо підрозділ не визначений, то він буде встановлений.
    /// </summary>
    public void ChangeDivision(Guid registryInterceptionDivisionId)
    {
        if (registryInterceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор підрозділу не може бути порожнім.", nameof(registryInterceptionDivisionId));

        RegistryInterceptionDivisionId = registryInterceptionDivisionId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Дозволяє очистити підрозділ профілю. Після цього підрозділ буде не визначений.
    /// </summary>
    public void ClearDivision()
    {
        RegistryInterceptionDivisionId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Додає підтверджену частоту профілю.
    /// Частота фіксується через registry division, щоб зберігати live-зв'язок
    /// з актуальною назвою підрозділу.
    /// </summary>
    public void AddFrequency(Guid registryInterceptionDivisionId)
    {
        if (registryInterceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор частоти/підрозділу не може бути порожнім.", nameof(registryInterceptionDivisionId));

        if (_frequencies.Any(x => x.RegistryInterceptionDivisionId == registryInterceptionDivisionId))
            return;

        _frequencies.Add(MilitaryProfileFrequency.Create(Id, registryInterceptionDivisionId));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Видаляє підтверджену частоту профілю.
    /// У профілю має залишатися хоча б одна підтверджена частота.
    /// </summary>
    public void RemoveFrequency(Guid registryInterceptionDivisionId)
    {
        if (registryInterceptionDivisionId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор частоти/підрозділу не може бути порожнім.", nameof(registryInterceptionDivisionId));

        var item = _frequencies
            .FirstOrDefault(x => x.RegistryInterceptionDivisionId == registryInterceptionDivisionId)
            ?? throw new InvalidOperationException("Частота профілю не знайдена.");

        if (_frequencies.Count == 1)
            throw new InvalidOperationException("У профілю має залишатися хоча б одна частота.");

        _frequencies.Remove(item);

        if (RegistryInterceptionDivisionId == registryInterceptionDivisionId)
            RegistryInterceptionDivisionId = _frequencies.First().RegistryInterceptionDivisionId;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Зміна ролі військової особи в профілі. Якщо роль не визначена, то вона буде встановлена.
    /// </summary>
    public void ChangeRole(Guid registryInterceptionParticipantRoleId)
    {
        if (registryInterceptionParticipantRoleId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор ролі не може бути порожнім.", nameof(registryInterceptionParticipantRoleId));

        RegistryInterceptionParticipantRoleId = registryInterceptionParticipantRoleId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Видалення ролі військової особи з профілю. Після цього роль буде не визначена.
    /// </summary>
    public void ClearRole()
    {
        RegistryInterceptionParticipantRoleId = null;
        UpdatedAt = DateTime.UtcNow;
    }
}