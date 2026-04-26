//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

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

        builder.HasIndex(profile => profile.Callsign);
        builder.HasIndex(profile => profile.DivisionProfileId);
        builder.HasIndex(profile => profile.RegistryInterceptionParticipantRoleId);

        ConfigureFrequencies(builder);
    }

    private static void ConfigureFrequencies(EntityTypeBuilder<MilitaryProfile> builder)
    {
        builder.OwnsMany(profile => profile.Frequencies, frequencyBuilder =>
        {
            frequencyBuilder.ToTable("military_profile_frequencies");

            frequencyBuilder.WithOwner()
                .HasForeignKey("military_profile_id");

            frequencyBuilder.Property<Guid>("military_profile_id")
                .HasColumnName("military_profile_id")
                .IsRequired();

            frequencyBuilder.HasKey(frequency => frequency.Id);

            frequencyBuilder.Property(frequency => frequency.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            frequencyBuilder.Property(frequency => frequency.FrequencyCode)
                .HasColumnName("frequency_code")
                .IsRequired();

            frequencyBuilder.Property(frequency => frequency.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            frequencyBuilder.HasIndex("military_profile_id");

            frequencyBuilder.HasIndex("military_profile_id", "frequency_code")
                .IsUnique();
        });

        builder.Navigation(profile => profile.Frequencies)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
