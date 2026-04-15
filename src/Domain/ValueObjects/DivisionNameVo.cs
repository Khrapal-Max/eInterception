//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Common.Extensions;

namespace Domain.ValueObjects;

/// <summary>
/// Value Object для назви підрозділу.
/// </summary>
public readonly record struct DivisionNameVo
{
    /// <summary>
    /// Нормалізоване значення підрозділу.
    /// </summary>
    public string Value { get; }

    private DivisionNameVo(string value)
        => Value = value;

    /// <summary>
    /// Створює VO з довільного вхідного рядка.
    /// </summary>
    public static DivisionNameVo? Create(string? value)
    {
        var normalized = StringExtensions.NormalizedOrNull(value);
        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new DivisionNameVo(normalized);
    }

    public override string ToString() => Value;
}