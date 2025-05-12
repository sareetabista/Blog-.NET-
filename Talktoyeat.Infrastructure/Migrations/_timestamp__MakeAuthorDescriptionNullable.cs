
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talktoyeat.Infrastructure.Data.Migrations
{
    public partial class UpdateDescriptionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Authors",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.Sql("UPDATE Authors SET Description = 'Default description' WHERE Description IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Authors",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}