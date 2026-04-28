//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure.Configurations;

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

        builder.OwnsOne(profile => profile.Name, name =>
        {
            name.Property(value => value.Value)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();
        });

        builder.Property(profile => profile.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(profile => profile.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Navigation(profile => profile.Name)
            .IsRequired();
    }
}
