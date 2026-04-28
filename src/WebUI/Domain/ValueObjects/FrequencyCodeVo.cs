//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.Extensions;

namespace WebUI.Domain.ValueObjects;

/// <summary>
/// Value Object для частоти у нормалізованому вигляді.
/// </summary>
public record FrequencyCodeVo
{
    /// <summary>
    /// Нормалізоване значення частоти.
    /// </summary>
    public string Value { get; }

    private FrequencyCodeVo(string value)
        => Value = value;

    /// <summary>
    /// Створює VO з довільного вхідного рядка.
    /// </summary>
    public static FrequencyCodeVo? Create(string? value)
    {
        var normalized = ValueExtensions.NormalizedOrNull(value);
        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new FrequencyCodeVo(normalized);
    }

    public override string ToString() => Value;
}