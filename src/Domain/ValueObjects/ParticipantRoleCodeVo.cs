//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Common.Extensions;

namespace Domain.ValueObjects;

/// <summary>
/// Value Object для коду ролі учасника.
/// </summary>
public readonly record struct ParticipantRoleCodeVo
{
    public string Value { get; }

    private ParticipantRoleCodeVo(string value)
        => Value = value;

    public static ParticipantRoleCodeVo? Create(string? value)
    {
        var normalized = StringExtensions.NormalizedOrNull(value);

        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new ParticipantRoleCodeVo(normalized);
    }

    public override string ToString() => Value;
}