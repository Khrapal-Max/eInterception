//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Дочірня сутність агрегату <see cref="MilitaryProfile"/>,
/// яка фіксує підтверджену частоту військового профілю.
/// </summary>
public sealed class MilitaryProfileFrequency
{
    /// <summary>
    /// Локальний ідентифікатор частоти профілю.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Код підтвердженої частоти.
    /// </summary>
    public FrequencyCodeVo FrequencyCode { get; private set; }

    /// <summary>
    /// Дата та час додавання частоти у форматі UTC.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Створює підтверджену частоту профілю.
    /// </summary>
    public static MilitaryProfileFrequency Create(string frequencyCode)
    {
        var normalizedFrequency = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Код частоти є обов'язковим.", nameof(frequencyCode));

        return new MilitaryProfileFrequency
        {
            Id = Guid.NewGuid(),
            FrequencyCode = normalizedFrequency,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Перевіряє, чи відповідає частота вказаному коду.
    /// </summary>
    public bool HasFrequency(string frequencyCode)
    {
        var normalizedFrequency = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Код частоти є обов'язковим.", nameof(frequencyCode));

        return FrequencyCode == normalizedFrequency;
    }
}