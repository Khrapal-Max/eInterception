//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using WebUI.Domain.Extensions;

namespace WebUI.Domain.ValueObjects;

/// <summary>
/// Value Object для коду ролі учасника.
/// </summary>
public record ParticipantRoleNameVo
{
    public string Value { get; }

    private ParticipantRoleNameVo(string value)
        => Value = value;

    public static ParticipantRoleNameVo? Create(string? value)
    {
        var normalized = ValueExtensions.NormalizedOrNull(value);

        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : new ParticipantRoleNameVo(normalized);
    }

    public override string ToString() => Value;
}