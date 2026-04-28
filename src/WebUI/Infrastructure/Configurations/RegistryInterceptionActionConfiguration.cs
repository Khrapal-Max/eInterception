//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure.Configurations;

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

        builder.OwnsOne(action => action.Name, name =>
        {
            name.Property(value => value.Value)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();
        });

        builder.Property(action => action.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Navigation(action => action.Name)
            .IsRequired();
    }
}
