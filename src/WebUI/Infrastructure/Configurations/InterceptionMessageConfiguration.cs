//-----------------------------------------------------------------------------
// All rights by agreement of the developer. Author data on GitHub Khrapal M.G.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebUI.Domain.Entities;

namespace WebUI.Infrastructure.Configurations;

/// <summary>
/// Конфігурація агрегату спостереження.
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

        builder.OwnsOne(message => message.FrequencyCode, frequency =>
        {
            frequency.Property(value => value.Value)
                .HasColumnName("frequency_code")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(message => message.DivisionName, division =>
        {
            division.Property(value => value.Value)
                .HasColumnName("division_name")
                .HasMaxLength(250);
        });

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

        builder.HasIndex(message => message.ObservedDate)
            .HasDatabaseName("ix_interception_messages_observed_date");

        builder.HasIndex(message => message.RegistryInterceptionActionId)
            .HasDatabaseName("ix_interception_messages_registry_interception_action_id");

        builder.Navigation(message => message.FrequencyCode)
            .IsRequired();

        builder.OwnsMany(message => message.Participants, participants =>
        {
            participants.ToTable("interception_participants");

            participants.WithOwner()
                .HasForeignKey("InterceptionMessageId");

            participants.Property<Guid>("InterceptionMessageId")
                .HasColumnName("interception_message_id")
                .IsRequired();

            participants.HasKey(participant => participant.Id);

            participants.Property(participant => participant.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            participants.Property(participant => participant.Callsign)
                .HasColumnName("callsign")
                .HasMaxLength(250)
                .IsRequired();

            participants.OwnsOne(participant => participant.DivisionName, division =>
            {
                division.Property(value => value.Value)
                    .HasColumnName("division_name")
                    .HasMaxLength(250);
            });

            participants.Property(participant => participant.RegistryInterceptionParticipantRoleId)
                .HasColumnName("registry_interception_participant_role_id");

            participants.Property(participant => participant.MilitaryProfileId)
                .HasColumnName("military_profile_id")
                .IsRequired();

            participants.HasOne<MilitaryProfile>()
                .WithMany()
                .HasForeignKey(participant => participant.MilitaryProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            participants.HasOne<RegistryInterceptionParticipantRole>()
                .WithMany()
                .HasForeignKey(participant => participant.RegistryInterceptionParticipantRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            participants.HasIndex("InterceptionMessageId")
                .HasDatabaseName("ix_interception_participants_interception_message_id");

            participants.HasIndex(participant => participant.MilitaryProfileId)
                .HasDatabaseName("ix_interception_participants_military_profile_id");

            participants.HasIndex(participant => participant.RegistryInterceptionParticipantRoleId)
                .HasDatabaseName("ix_interception_participants_registry_interception_participant_role_id");
        });

        builder.Navigation(message => message.Participants)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(message => message.CommandVectors, vectors =>
        {
            vectors.ToTable("interception_command_vectors");

            vectors.WithOwner()
                .HasForeignKey("InterceptionMessageId");

            vectors.Property<Guid>("InterceptionMessageId")
                .HasColumnName("interception_message_id")
                .IsRequired();

            vectors.HasKey(vector => vector.Id);

            vectors.Property(vector => vector.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            vectors.Property(vector => vector.FromParticipantId)
                .HasColumnName("from_participant_id")
                .IsRequired();

            vectors.Property(vector => vector.ToParticipantId)
                .HasColumnName("to_participant_id")
                .IsRequired();

            vectors.HasIndex("InterceptionMessageId")
                .HasDatabaseName("ix_interception_command_vectors_interception_message_id");

            vectors.HasIndex(vector => new { vector.FromParticipantId, vector.ToParticipantId })
                .HasDatabaseName("ix_interception_command_vectors_from_to_participants");
        });

        builder.Navigation(message => message.CommandVectors)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
