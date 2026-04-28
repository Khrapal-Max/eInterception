using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "division_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_division_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "registry_interception_actions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registry_interception_actions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "registry_interception_participant_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registry_interception_participant_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "division_frequency_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    division_profile_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    frequency_code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    active_from = table.Column<DateTime>(type: "TEXT", nullable: false),
                    active_to = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_division_frequency_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_division_frequency_assignments_division_profiles_division_profile_id",
                        column: x => x.division_profile_id,
                        principalTable: "division_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "interception_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    observed_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    frequency_code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    division_name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    registry_interception_action_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    unknown_participant_count = table.Column<int>(type: "INTEGER", nullable: false),
                    message_text = table.Column<string>(type: "TEXT", nullable: false),
                    can_be_put_on_map = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interception_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_interception_messages_registry_interception_actions_registry_interception_action_id",
                        column: x => x.registry_interception_action_id,
                        principalTable: "registry_interception_actions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "military_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    callsign = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    division_profile_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    registry_interception_participant_role_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_military_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_military_profiles_division_profiles_division_profile_id",
                        column: x => x.division_profile_id,
                        principalTable: "division_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_military_profiles_registry_interception_participant_roles_registry_interception_participant_role_id",
                        column: x => x.registry_interception_participant_role_id,
                        principalTable: "registry_interception_participant_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "interception_command_vectors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    from_participant_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    to_participant_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    interception_message_id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interception_command_vectors", x => x.id);
                    table.ForeignKey(
                        name: "FK_interception_command_vectors_interception_messages_interception_message_id",
                        column: x => x.interception_message_id,
                        principalTable: "interception_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interception_participants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    callsign = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    division_name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    registry_interception_participant_role_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    military_profile_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    interception_message_id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interception_participants", x => x.id);
                    table.ForeignKey(
                        name: "FK_interception_participants_interception_messages_interception_message_id",
                        column: x => x.interception_message_id,
                        principalTable: "interception_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_interception_participants_military_profiles_military_profile_id",
                        column: x => x.military_profile_id,
                        principalTable: "military_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_interception_participants_registry_interception_participant_roles_registry_interception_participant_role_id",
                        column: x => x.registry_interception_participant_role_id,
                        principalTable: "registry_interception_participant_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "military_profile_frequencies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    frequency_code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    military_profile_id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_military_profile_frequencies", x => x.id);
                    table.ForeignKey(
                        name: "FK_military_profile_frequencies_military_profiles_military_profile_id",
                        column: x => x.military_profile_id,
                        principalTable: "military_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_division_frequency_assignments_division_profile_id",
                table: "division_frequency_assignments",
                column: "division_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_interception_command_vectors_from_to_participants",
                table: "interception_command_vectors",
                columns: new[] { "from_participant_id", "to_participant_id" });

            migrationBuilder.CreateIndex(
                name: "ix_interception_command_vectors_interception_message_id",
                table: "interception_command_vectors",
                column: "interception_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_interception_messages_observed_date",
                table: "interception_messages",
                column: "observed_date");

            migrationBuilder.CreateIndex(
                name: "ix_interception_messages_registry_interception_action_id",
                table: "interception_messages",
                column: "registry_interception_action_id");

            migrationBuilder.CreateIndex(
                name: "ix_interception_participants_interception_message_id",
                table: "interception_participants",
                column: "interception_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_interception_participants_military_profile_id",
                table: "interception_participants",
                column: "military_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_interception_participants_registry_interception_participant_role_id",
                table: "interception_participants",
                column: "registry_interception_participant_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_military_profile_frequencies_military_profile_id",
                table: "military_profile_frequencies",
                column: "military_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_military_profiles_division_profile_id",
                table: "military_profiles",
                column: "division_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_military_profiles_registry_interception_participant_role_id",
                table: "military_profiles",
                column: "registry_interception_participant_role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "division_frequency_assignments");

            migrationBuilder.DropTable(
                name: "interception_command_vectors");

            migrationBuilder.DropTable(
                name: "interception_participants");

            migrationBuilder.DropTable(
                name: "military_profile_frequencies");

            migrationBuilder.DropTable(
                name: "interception_messages");

            migrationBuilder.DropTable(
                name: "military_profiles");

            migrationBuilder.DropTable(
                name: "registry_interception_actions");

            migrationBuilder.DropTable(
                name: "division_profiles");

            migrationBuilder.DropTable(
                name: "registry_interception_participant_roles");
        }
    }
}
