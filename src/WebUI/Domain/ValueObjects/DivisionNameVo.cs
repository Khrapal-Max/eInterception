//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.Extensions;

namespace WebUI.Domain.ValueObjects;

/// <summary>
/// Value Object для назви підрозділу.
/// </summary>
public record DivisionNameVo
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
        var normalized = ValueExtensions.NormalizedOrNull(value);
        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new DivisionNameVo(normalized);
    }

    public override string ToString() => Value;
}