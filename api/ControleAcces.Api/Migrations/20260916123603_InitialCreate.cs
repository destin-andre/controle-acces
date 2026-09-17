using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ControleAcces.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    IdUtilisateur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prenom = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Poste = table.Column<string>(type: "text", nullable: false),
                    EnActivite = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.IdUtilisateur);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    IdZone = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomZone = table.Column<string>(type: "text", nullable: false),
                    NiveauSecurite = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.IdZone);
                });

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    IdBadge = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UidBadge = table.Column<string>(type: "text", nullable: false),
                    DateAttribution = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Actif = table.Column<bool>(type: "boolean", nullable: false),
                    IdUtilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.IdBadge);
                    table.ForeignKey(
                        name: "FK_Badges_Utilisateurs_IdUtilisateur",
                        column: x => x.IdUtilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empreintes",
                columns: table => new
                {
                    IdEmpreinte = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplatesBiometrique = table.Column<byte[]>(type: "bytea", nullable: false),
                    DateEnrolement = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdUtilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empreintes", x => x.IdEmpreinte);
                    table.ForeignKey(
                        name: "FK_Empreintes_Utilisateurs_IdUtilisateur",
                        column: x => x.IdUtilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DroitsAcces",
                columns: table => new
                {
                    IdDroit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateDebut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdBadge = table.Column<int>(type: "integer", nullable: false),
                    IdZone = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroitsAcces", x => x.IdDroit);
                    table.ForeignKey(
                        name: "FK_DroitsAcces_Badges_IdBadge",
                        column: x => x.IdBadge,
                        principalTable: "Badges",
                        principalColumn: "IdBadge",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DroitsAcces_Zones_IdZone",
                        column: x => x.IdZone,
                        principalTable: "Zones",
                        principalColumn: "IdZone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogAcces",
                columns: table => new
                {
                    IdLog = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Horodatage = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Methode = table.Column<string>(type: "text", nullable: false),
                    Resultat = table.Column<string>(type: "text", nullable: false),
                    IdBadge = table.Column<int>(type: "integer", nullable: false),
                    IdZone = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAcces", x => x.IdLog);
                    table.ForeignKey(
                        name: "FK_LogAcces_Badges_IdBadge",
                        column: x => x.IdBadge,
                        principalTable: "Badges",
                        principalColumn: "IdBadge",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogAcces_Zones_IdZone",
                        column: x => x.IdZone,
                        principalTable: "Zones",
                        principalColumn: "IdZone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_IdUtilisateur",
                table: "Badges",
                column: "IdUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_DroitsAcces_IdBadge",
                table: "DroitsAcces",
                column: "IdBadge");

            migrationBuilder.CreateIndex(
                name: "IX_DroitsAcces_IdZone",
                table: "DroitsAcces",
                column: "IdZone");

            migrationBuilder.CreateIndex(
                name: "IX_Empreintes_IdUtilisateur",
                table: "Empreintes",
                column: "IdUtilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogAcces_IdBadge",
                table: "LogAcces",
                column: "IdBadge");

            migrationBuilder.CreateIndex(
                name: "IX_LogAcces_IdZone",
                table: "LogAcces",
                column: "IdZone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DroitsAcces");

            migrationBuilder.DropTable(
                name: "Empreintes");

            migrationBuilder.DropTable(
                name: "LogAcces");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
