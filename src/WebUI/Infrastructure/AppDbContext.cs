//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure;

/// <summary>
/// Основний контекст бази даних застосунку.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<InterceptionMessage> InterceptionMessages { get; set; }
    public DbSet<MilitaryProfile> MilitaryProfiles { get; set; }
    public DbSet<DivisionProfile> DivisionProfiles { get; set; }
    public DbSet<DivisionFrequencyAssignment> DivisionFrequencyAssignments { get; set; }
    public DbSet<RegistryInterceptionAction> RegistryInterceptionActions { get; set; }
    public DbSet<RegistryInterceptionParticipantRole> RegistryInterceptionParticipantRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
