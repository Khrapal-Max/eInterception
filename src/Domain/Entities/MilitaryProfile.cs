//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Канонічний профіль військової особи.
/// Є фінальним aggregate root для зведення оперативних проявів однієї особи
/// та побудови аналітичних read-model.
/// </summary>
public sealed class MilitaryProfile
{
    /// <summary>
    /// Ідентифікатор профілю.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Канонічний позивний особи.
    /// Є обов'язковим для профілю.
    /// </summary>
    public string Callsign { get; private set; } = string.Empty;

    /// <summary>
    /// Актуальний підрозділ профілю.
    /// Може бути відсутнім, якщо підрозділ ще не визначено.
    /// </summary>
    public DivisionNameVo? DivisionName { get; private set; }

    /// <summary>
    /// Ідентифікатор ролі з довідника ролей.
    /// Може бути відсутнім, якщо роль ще не визначено.
    /// </summary>
    public Guid? RegistryInterceptionParticipantRoleId { get; private set; }

    /// <summary>
    /// Дата та час створення профілю у форматі UTC.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення профілю у форматі UTC.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Підтверджені частоти профілю.
    /// Колекція зберігається як набір value object без окремої дочірньої сутності.
    /// </summary>
    public IReadOnlyCollection<FrequencyCodeVo> Frequencies => _frequencies;
    private readonly List<FrequencyCodeVo> _frequencies = [];

    /// <summary>
    /// Створює новий профіль військової особи.
    /// Позивний та щонайменше одна частота є обов'язковими.
    /// Підрозділ і роль можуть бути відсутніми на момент створення.
    /// </summary>
    public static MilitaryProfile Create(
        string callsign,
        string frequencyCode,
        string? divisionName = null,
        Guid? registryInterceptionParticipantRoleId = null)
    {
        var normalizedCallsign = NormalizeCallsign(callsign);
        var normalizedFrequency = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Код частоти є обов'язковим.", nameof(frequencyCode));

        if (registryInterceptionParticipantRoleId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор ролі не може дорівнювати порожньому значенню.", nameof(registryInterceptionParticipantRoleId));

        var profile = new MilitaryProfile
        {
            Id = Guid.NewGuid(),
            Callsign = normalizedCallsign,
            DivisionName = DivisionNameVo.Create(divisionName),
            RegistryInterceptionParticipantRoleId = registryInterceptionParticipantRoleId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        profile._frequencies.Add(normalizedFrequency);

        return profile;
    }

    /// <summary>
    /// Оновлює основні реквізити профілю.
    ///
    /// Позивний залишається обов'язковим. Підрозділ і роль можуть бути очищені,
    /// якщо для них передано <c>null</c>.
    /// </summary>
    public void Update(string callsign, string? divisionName, Guid? registryInterceptionParticipantRoleId)
    {
        if (registryInterceptionParticipantRoleId == Guid.Empty)
            throw new ArgumentException("Порожній ідентифікатор ролі не допускається. Використовуйте null для очищення ролі.", nameof(registryInterceptionParticipantRoleId));

        Callsign = NormalizeCallsign(callsign);
        DivisionName = DivisionNameVo.Create(divisionName);
        RegistryInterceptionParticipantRoleId = registryInterceptionParticipantRoleId;
        UpdatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Додає нову підтверджену частоту профілю.
    /// </summary>
    public void AddFrequency(string frequencyCode)
    {
        var normalizedFrequency = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Код частоти не може бути порожнім.", nameof(frequencyCode));

        if (_frequencies.Contains(normalizedFrequency))
            return;

        _frequencies.Add(normalizedFrequency);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Видаляє підтверджену частоту профілю.
    /// У профілю має залишатися щонайменше одна частота.
    /// </summary>
    public void RemoveFrequency(string frequencyCode)
    {
        var normalizedFrequency = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Код частоти не може бути порожнім.", nameof(frequencyCode));

        if (_frequencies.Count == 1 && _frequencies.Contains(normalizedFrequency))
            throw new InvalidOperationException("У профілю має залишатися щонайменше одна частота.");

        if (!_frequencies.Remove(normalizedFrequency))
            throw new InvalidOperationException("Частоту профілю не знайдено.");

        UpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeCallsign(string callsign)
    {
        if (string.IsNullOrWhiteSpace(callsign))
            throw new ArgumentException("Позивний профілю є обов'язковим.", nameof(callsign));

        return callsign.Trim();
    }
}
