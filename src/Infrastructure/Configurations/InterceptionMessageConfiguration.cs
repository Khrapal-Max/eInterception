//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Конфігурація агрегату спостереження перехоплення.
/// </summary>
public sealed class InterceptionMessageConfiguration : IEntityTypeConfiguration<InterceptionMessage>
{
    public void Configure(EntityTypeBuilder<InterceptionMessage> builder)
    {
        builder.ToTable("interception_messages");

        builder.HasKey(message => message.Id);

        builder.Property(message => message.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(message => message.ObservedDate)
            .HasColumnName("observed_date")
            .IsRequired();

        builder.Property(message => message.FrequencyCode)
            .HasColumnName("frequency_code")
            .IsRequired();

        builder.Property(message => message.DivisionName)
            .HasColumnName("division_name");

        builder.Property(message => message.RegistryInterceptionActionId)
            .HasColumnName("registry_interception_action_id")
            .IsRequired();

        builder.Property(message => message.UnknownParticipantCount)
            .HasColumnName("unknown_participant_count")
            .IsRequired();

        builder.Property(message => message.MessageText)
            .HasColumnName("message_text")
            .IsRequired();

        builder.Property(message => message.CanBePutOnMap)
            .HasColumnName("can_be_put_on_map")
            .IsRequired();

        builder.Property(message => message.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(message => message.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne<RegistryInterceptionAction>()
            .WithMany()
            .HasForeignKey(message => message.RegistryInterceptionActionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(message => message.ObservedDate);
        builder.HasIndex(message => message.FrequencyCode);
        builder.HasIndex(message => message.RegistryInterceptionActionId);

        ConfigureParticipants(builder);
        ConfigureCommandVectors(builder);
    }

    private static void ConfigureParticipants(EntityTypeBuilder<InterceptionMessage> builder)
    {
        builder.OwnsMany(message => message.Participants, participantBuilder =>
        {
            participantBuilder.ToTable("interception_participants");

            participantBuilder.WithOwner()
                .HasForeignKey("interception_message_id");

            participantBuilder.Property<Guid>("interception_message_id")
                .HasColumnName("interception_message_id")
                .IsRequired();

            participantBuilder.HasKey(participant => participant.Id);

            participantBuilder.Property(participant => participant.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            participantBuilder.Property(participant => participant.Callsign)
                .HasColumnName("callsign")
                .IsRequired();

            participantBuilder.Property(participant => participant.DivisionName)
                .HasColumnName("division_name")
                .HasConversion(
                    division => division.HasValue ? division.Value.Value : null,
                    value => DivisionNameVo.Create(value));

            participantBuilder.Property(participant => participant.RegistryInterceptionParticipantRoleId)
                .HasColumnName("registry_interception_participant_role_id");

            participantBuilder.Property(participant => participant.MilitaryProfileId)
                .HasColumnName("military_profile_id")
                .IsRequired();

            participantBuilder.HasOne<MilitaryProfile>()
                .WithMany()
                .HasForeignKey(participant => participant.MilitaryProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            participantBuilder.HasOne<RegistryInterceptionParticipantRole>()
                .WithMany()
                .HasForeignKey(participant => participant.RegistryInterceptionParticipantRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            participantBuilder.HasIndex("interception_message_id");
            participantBuilder.HasIndex(participant => participant.MilitaryProfileId);
            participantBuilder.HasIndex(participant => participant.RegistryInterceptionParticipantRoleId);
        });

        builder.Navigation(message => message.Participants)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureCommandVectors(EntityTypeBuilder<InterceptionMessage> builder)
    {
        builder.OwnsMany(message => message.CommandVectors, vectorBuilder =>
        {
            vectorBuilder.ToTable("interception_command_vectors");

            vectorBuilder.WithOwner()
                .HasForeignKey("interception_message_id");

            vectorBuilder.Property<Guid>("interception_message_id")
                .HasColumnName("interception_message_id")
                .IsRequired();

            vectorBuilder.HasKey(vector => vector.Id);

            vectorBuilder.Property(vector => vector.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            vectorBuilder.Property(vector => vector.FromParticipantId)
                .HasColumnName("from_participant_id")
                .IsRequired();

            vectorBuilder.Property(vector => vector.ToParticipantId)
                .HasColumnName("to_participant_id")
                .IsRequired();

            vectorBuilder.HasIndex("interception_message_id");
            vectorBuilder.HasIndex(vector => new { vector.FromParticipantId, vector.ToParticipantId });
        });

        builder.Navigation(message => message.CommandVectors)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
