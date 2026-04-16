//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Extensions;

namespace Domain.ValueObjects;

/// <summary>
/// Value Object для коду дії перехоплення.
/// Є стабільним доменним кодом, а не технічним Id запису регістру.
/// </summary>
public readonly record struct InterceptionActionNameVo
{
    public string Value { get; }

    private InterceptionActionNameVo(string value)
        => Value = value;

    public static InterceptionActionNameVo? Create(string? value)
    {
        var normalized = ValueExtensions.NormalizedOrNull(value);

        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new InterceptionActionNameVo(normalized);
    }

    public override string ToString() => Value;
}