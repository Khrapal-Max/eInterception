//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

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

        builder.Property(role => role.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(role => role.Description)
            .HasColumnName("description");

        builder.HasIndex(role => role.Name)
            .IsUnique();
    }
}
