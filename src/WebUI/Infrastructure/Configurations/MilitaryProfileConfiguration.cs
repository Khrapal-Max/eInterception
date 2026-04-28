//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure.Configurations;

/// <summary>
/// Конфігурація канонічного профілю військової особи.
/// </summary>
public sealed class MilitaryProfileConfiguration : IEntityTypeConfiguration<MilitaryProfile>
{
    public void Configure(EntityTypeBuilder<MilitaryProfile> builder)
    {
        builder.ToTable("military_profiles");

        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(profile => profile.Callsign)
            .HasColumnName("callsign")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(profile => profile.DivisionProfileId)
            .HasColumnName("division_profile_id");

        builder.Property(profile => profile.RegistryInterceptionParticipantRoleId)
            .HasColumnName("registry_interception_participant_role_id");

        builder.Property(profile => profile.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(profile => profile.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne<DivisionProfile>()
            .WithMany()
            .HasForeignKey(profile => profile.DivisionProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<RegistryInterceptionParticipantRole>()
            .WithMany()
            .HasForeignKey(profile => profile.RegistryInterceptionParticipantRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(profile => profile.DivisionProfileId)
            .HasDatabaseName("ix_military_profiles_division_profile_id");

        builder.HasIndex(profile => profile.RegistryInterceptionParticipantRoleId)
            .HasDatabaseName("ix_military_profiles_registry_interception_participant_role_id");

        builder.OwnsMany(profile => profile.Frequencies, frequencies =>
        {
            frequencies.ToTable("military_profile_frequencies");

            frequencies.WithOwner()
                .HasForeignKey("MilitaryProfileId");

            frequencies.Property<Guid>("MilitaryProfileId")
                .HasColumnName("military_profile_id")
                .IsRequired();

            frequencies.HasKey(frequency => frequency.Id);

            frequencies.Property(frequency => frequency.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            frequencies.OwnsOne(frequency => frequency.FrequencyCode, frequencyCode =>
            {
                frequencyCode.Property(value => value.Value)
                    .HasColumnName("frequency_code")
                    .HasMaxLength(50)
                    .IsRequired();
            });

            frequencies.Property(frequency => frequency.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            frequencies.HasIndex("MilitaryProfileId")
                .HasDatabaseName("ix_military_profile_frequencies_military_profile_id");

            frequencies.Navigation(frequency => frequency.FrequencyCode)
                .IsRequired();
        });

        builder.Navigation(profile => profile.Frequencies)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
