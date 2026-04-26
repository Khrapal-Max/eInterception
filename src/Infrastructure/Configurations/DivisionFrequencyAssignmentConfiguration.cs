//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

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

        builder.Property(assignment => assignment.FrequencyCode)
            .HasColumnName("frequency_code")
            .IsRequired();

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

        builder.HasIndex(assignment => assignment.DivisionProfileId);
        builder.HasIndex(assignment => assignment.FrequencyCode);
        builder.HasIndex(assignment => new { assignment.FrequencyCode, assignment.ActiveFrom });
    }
}
