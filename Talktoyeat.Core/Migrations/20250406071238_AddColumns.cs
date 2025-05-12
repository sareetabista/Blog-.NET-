using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talktoyeat.Core.Migrations
{
    public partial class AddColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Groups_GroupId",
                table: "Authors");

          
         
        
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "CredibilityScore",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CredibilityScore",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Authors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_GroupId",
                table: "Authors",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Groups_GroupId",
                table: "Authors",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
