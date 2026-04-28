//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.ValueObjects;

namespace WebUI.Domain.Entities;

/// <summary>
/// Канонічний профіль підрозділу.
/// 
/// Профіль відповідає за ідентичність підрозділу: його назву та життєвий цикл.
/// Прив'язка частот до підрозділу винесена в окрему модель
/// <see cref="DivisionFrequencyAssignment"/>, оскільки одна частота
/// може бути активною лише в одному профілі підрозділу в один момент часу.
/// </summary>
public sealed class DivisionProfile
{
    /// <summary>
    /// Ідентифікатор профілю підрозділу.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Канонічна назва підрозділу.
    /// </summary>
    public DivisionNameVo Name { get; private set; } = default!;

    /// <summary>
    /// Дата та час створення профілю у форматі UTC.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення профілю у форматі UTC.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Створює новий канонічний профіль підрозділу.
    /// </summary>
    public static DivisionProfile Create(string name)
    {
        var normalizedName = DivisionNameVo.Create(name)
            ?? throw new ArgumentException("Назва підрозділу є обов'язковою.", nameof(name));

        return new DivisionProfile
        {
            Id = Guid.NewGuid(),
            Name = normalizedName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Оновлює канонічну назву підрозділу.
    /// </summary>
    public void Rename(string name)
    {
        Name = DivisionNameVo.Create(name)
            ?? throw new ArgumentException("Назва підрозділу є обов'язковою.", nameof(name));

        UpdatedAt = DateTime.UtcNow;
    }
}