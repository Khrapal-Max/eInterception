//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Канонічний часовий зв'язок між частотою та профілем підрозділу.
/// 
/// Правило предметної області:
/// одна й та сама частота може бути активною лише в одному профілі підрозділу
/// в один і той самий момент часу.
/// </summary>
public sealed class DivisionFrequencyAssignment
{
    /// <summary>
    /// Ідентифікатор зв'язку.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Ідентифікатор профілю підрозділу, якому належить частота.
    /// </summary>
    public Guid DivisionProfileId { get; private set; }

    /// <summary>
    /// Частота, для якої зафіксовано належність.
    /// </summary>
    public FrequencyCodeVo FrequencyCode { get; private set; }

    /// <summary>
    /// Початок дії зв'язку у форматі UTC.
    /// </summary>
    public DateTime ActiveFrom { get; private set; }

    /// <summary>
    /// Кінець дії зв'язку у форматі UTC.
    /// Null означає, що зв'язок активний зараз.
    /// </summary>
    public DateTime? ActiveTo { get; private set; }

    /// <summary>
    /// Ознака активного зв'язку.
    /// </summary>
    public bool IsActive => ActiveTo is null;

    /// <summary>
    /// Створює новий активний зв'язок між частотою та профілем підрозділу.
    /// </summary>
    public static DivisionFrequencyAssignment Create(
        Guid divisionProfileId,
        string frequencyCode,
        DateTime activeFromUtc)
    {
        if (divisionProfileId == Guid.Empty)
            throw new ArgumentException("Ідентифікатор профілю підрозділу є обов'язковим.", nameof(divisionProfileId));

        if (activeFromUtc == default)
            throw new ArgumentException("Дата початку дії зв'язку є обов'язковою.", nameof(activeFromUtc));

        var normalizedFrequency = FrequencyCodeVo.Create(frequencyCode)
            ?? throw new ArgumentException("Код частоти є обов'язковим.", nameof(frequencyCode));

        return new DivisionFrequencyAssignment
        {
            Id = Guid.NewGuid(),
            DivisionProfileId = divisionProfileId,
            FrequencyCode = normalizedFrequency,
            ActiveFrom = activeFromUtc.ToUniversalTime(),
            ActiveTo = null
        };
    }

    /// <summary>
    /// Завершує дію поточного зв'язку.
    /// Після цього частота може бути передана іншому підрозділу окремим новим записом.
    /// </summary>
    public void Close(DateTime activeToUtc)
    {
        if (activeToUtc == default)
            throw new ArgumentException("Дата завершення дії зв'язку є обов'язковою.", nameof(activeToUtc));

        if (ActiveTo is not null)
            throw new InvalidOperationException("Зв'язок частоти з підрозділом уже завершено.");

        var normalizedActiveTo = activeToUtc.ToUniversalTime();

        if (normalizedActiveTo < ActiveFrom)
            throw new InvalidOperationException("Дата завершення не може бути раніше дати початку дії.");

        ActiveTo = normalizedActiveTo;
    }

    /// <summary>
    /// Перевіряє, чи був зв'язок активним на вказаний момент часу.
    /// </summary>
    public bool IsActiveAt(DateTime momentUtc)
    {
        if (momentUtc == default)
            throw new ArgumentException("Момент часу є обов'язковим.", nameof(momentUtc));

        var normalizedMoment = momentUtc.ToUniversalTime();

        return normalizedMoment >= ActiveFrom
               && (ActiveTo is null || normalizedMoment < ActiveTo.Value);
    }
}