//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Сутність, що описує віськову одиницю, яка є особою.
/// Містить інформцію про військову одиницю, таку як її назва, тип та інші характеристики.
/// </summary>
public sealed class MilitaryUnit
{
    public Guid Id { get; private set; }

    public string DisplayName { get; private set; } = string.Empty;
}
