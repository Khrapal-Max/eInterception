//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure.Configurations;

/// <summary>
/// Конфігурація часового зв'язку частоти з профілем підрозділу.
/// </summary>
public sealed class DivisionFrequencyAssignmentConfiguration : IEntityTypeConfiguration<DivisionFrequencyAssignment>
{
    public void Configure(EntityTypeBuilder<DivisionFrequencyAssignment> builder)
    {
        builder.ToTable("division_frequency_assignments");

        builder.HasKey(assignment => assignment.Id);

        builder.Property(assignment => assignment.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(assignment => assignment.DivisionProfileId)
            .HasColumnName("division_profile_id")
            .IsRequired();

        builder.OwnsOne(assignment => assignment.FrequencyCode, frequency =>
        {
            frequency.Property(value => value.Value)
                .HasColumnName("frequency_code")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.Property(assignment => assignment.ActiveFrom)
            .HasColumnName("active_from")
            .IsRequired();

        builder.Property(assignment => assignment.ActiveTo)
            .HasColumnName("active_to");

        builder.Ignore(assignment => assignment.IsActive);

        builder.HasOne<DivisionProfile>()
            .WithMany()
            .HasForeignKey(assignment => assignment.DivisionProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(assignment => assignment.DivisionProfileId)
            .HasDatabaseName("ix_division_frequency_assignments_division_profile_id");

        builder.Navigation(assignment => assignment.FrequencyCode)
            .IsRequired();
    }
}
