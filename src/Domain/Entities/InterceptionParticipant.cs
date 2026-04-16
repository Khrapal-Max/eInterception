//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

namespace Domain.Entities;

/// <summary>
/// Owned entity агрегату спостереження, що представляє учасника перехоплення. 
/// Учасник перехоплення може бути пов'язаний з військовим профілем.
/// </summary>
public sealed class InterceptionParticipant
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Ид спостереження, до якого належить цей учасник перехоплення.
    /// </summary>
    public Guid InterceptionMessageId { get; private set; }

    /// <summary>
    /// Позивний учасника перехоплення, який використовується для ідентифікації його під час спостереження.
    /// </summary>
    public string Callsign { get; private set; } = string.Empty;

    /// <summary>
    /// Роль особи в спостереженні, яка може бути використана для визначення її функції або обов'язків під час перехоплення.
    /// </summary>
    public Guid? RegistryInterceptionParticipantRoleId { get; private set; }

    /// <summary>
    /// Ид профіля особи, який може бути пов'язаний з цим учасником перехоплення.
    /// Це дозволяє зв'язати учасника з конкретним військовим профілем для отримання додаткової інформації про нього.
    /// </summary>
    public Guid? MilitaryProfileId { get; private set; }

    //------------------------------------------------------------------------------------------------
    // Factory method to create a new InterceptionMessage with validation and normalization of inputs.
    //------------------------------------------------------------------------------------------------
    public static InterceptionParticipant Create(
        Guid interceptionMessageId,
        string callsign,
        Guid? registryInterceptionParticipantRoleId,
        Guid? militaryProfileId)
    {
        if (interceptionMessageId == Guid.Empty)
            throw new ArgumentNullException(nameof(interceptionMessageId), "Ид спостереження не може бути порожнім.");

        if (string.IsNullOrWhiteSpace(callsign))
            throw new ArgumentNullException(nameof(callsign), "Позивний особи обов'язковий.");

        if (militaryProfileId == Guid.Empty)
            throw new ArgumentNullException(nameof(militaryProfileId), "Ид профіля обов'язковий.");

        return new InterceptionParticipant
        {
            Id = Guid.NewGuid(),
            InterceptionMessageId = interceptionMessageId,
            Callsign = callsign.Trim(),
            RegistryInterceptionParticipantRoleId = registryInterceptionParticipantRoleId,
            MilitaryProfileId = militaryProfileId
        };
    }

    //------------------------------------------------------------------------------------------------
    // Behavior methods to manage participants and command vectors, ensuring that all operations
    // maintain the integrity of the aggregate and update timestamps accordingly.
    //------------------------------------------------------------------------------------------------
    public void LinkToMilitaryProfile(Guid militaryProfileId)
    {
        if (militaryProfileId == Guid.Empty)
            throw new ArgumentNullException(nameof(militaryProfileId));

        MilitaryProfileId = militaryProfileId;
    }
}