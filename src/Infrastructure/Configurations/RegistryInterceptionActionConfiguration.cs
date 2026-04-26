//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Конфігурація довідника дій перехоплення.
/// </summary>
public sealed class RegistryInterceptionActionConfiguration : IEntityTypeConfiguration<RegistryInterceptionAction>
{
    public void Configure(EntityTypeBuilder<RegistryInterceptionAction> builder)
    {
        builder.ToTable("registry_interception_actions");

        builder.HasKey(action => action.Id);

        builder.Property(action => action.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(action => action.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(action => action.Description)
            .HasColumnName("description");

        builder.HasIndex(action => action.Name)
            .IsUnique();
    }
}
