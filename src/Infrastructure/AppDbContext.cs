//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Основний EF Core контекст застосунку.
/// </summary>
/// <remarks>
/// Ініціалізує новий екземпляр контексту.
/// </remarks>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    /// <summary>
    /// Спостереження перехоплення.
    /// </summary>
    public DbSet<InterceptionMessage> InterceptionMessages { get; set; }

    /// <summary>
    /// Канонічні профілі військових осіб.
    /// </summary>
    public DbSet<MilitaryProfile> MilitaryProfiles { get; set; }

    /// <summary>
    /// Канонічні профілі підрозділів.
    /// </summary>
    public DbSet<DivisionProfile> DivisionProfiles { get; set; }

    /// <summary>
    /// Історичні зв'язки частоти з профілем підрозділу.
    /// </summary>
    public DbSet<DivisionFrequencyAssignment> DivisionFrequencyAssignments { get; set; }

    /// <summary>
    /// Довідник дій перехоплення.
    /// </summary>
    public DbSet<RegistryInterceptionAction> RegistryInterceptionActions { get; set; }

    /// <summary>
    /// Довідник ролей учасників перехоплення.
    /// </summary>
    public DbSet<RegistryInterceptionParticipantRole> RegistryInterceptionParticipantRoles { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
