//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Конфігурація канонічного профілю підрозділу.
/// </summary>
public sealed class DivisionProfileConfiguration : IEntityTypeConfiguration<DivisionProfile>
{
    public void Configure(EntityTypeBuilder<DivisionProfile> builder)
    {
        builder.ToTable("division_profiles");

        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(profile => profile.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(profile => profile.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(profile => profile.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(profile => profile.Name)
            .IsUnique();
    }
}
