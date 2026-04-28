//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure.Configurations;

/// <summary>
/// Конфігурація довідника ролей учасників перехоплення.
/// </summary>
public sealed class RegistryInterceptionParticipantRoleConfiguration : IEntityTypeConfiguration<RegistryInterceptionParticipantRole>
{
    public void Configure(EntityTypeBuilder<RegistryInterceptionParticipantRole> builder)
    {
        builder.ToTable("registry_interception_participant_roles");

        builder.HasKey(role => role.Id);

        builder.Property(role => role.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.OwnsOne(role => role.Name, name =>
        {
            name.Property(value => value.Value)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();
        });

        builder.Property(role => role.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Navigation(role => role.Name)
            .IsRequired();
    }
}
