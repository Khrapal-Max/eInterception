//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

public sealed class InterceptionMessage
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Дата спостереження. Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime ObservedDate { get; private set; }

    /// <summary>
    /// Частота радіоперехоплення, не унікальна, може повторюватися.
    /// </summary>
    public string? FrequencyCode { get; private set; }

    /// <summary>
    /// Назва підрозділу, не унікальна, може повторюватися.
    /// </summary>
    public string? DivisionName { get; set; }

    /// <summary>
    /// ДІя яка характеризує та коротко описує зміст радіоперехоплення.
    /// Дії переться з регістру активних дій.
    /// </summary>
    public Guid? InterceptionActionId { get; private set; }
    public InterceptionAction? InterceptionAction { get; private set; }

    /// <summary>
    /// Запис радіоперехоплення, який містить текстову інформацію про перехоплення.
    /// </summary>
    public string? MessageText { get; private set; }

    /// <summary>
    /// Дата та час створення запису перехоплення.
    /// Встановлюється при створенні об'єкта і не змінюється.
    /// Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата та час останнього оновлення запису перехоплення.
    /// Встановлюється при створенні об'єкта і оновлюється щоразу, коли змінюються властивості Name, FrequencyCode або DivisionName.
    /// Універсальна дата та час (UTC) для забезпечення узгодженості в різних часових поясах.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }
}
