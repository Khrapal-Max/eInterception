//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Extensions;

namespace Domain.ValueObjects;

/// <summary>
/// Value Object для коду дії перехоплення.
/// Є стабільним доменним кодом, а не технічним Id запису регістру.
/// </summary>
public readonly record struct InterceptionActionCodeVo
{
    public string Value { get; }

    private InterceptionActionCodeVo(string value)
        => Value = value;

    public static InterceptionActionCodeVo? Create(string? value)
    {
        var normalized = ValueExtensions.NormalizedOrNull(value);

        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new InterceptionActionCodeVo(normalized);
    }

    public override string ToString() => Value;
}