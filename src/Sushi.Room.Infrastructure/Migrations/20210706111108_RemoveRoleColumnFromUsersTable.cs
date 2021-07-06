using Microsoft.EntityFrameworkCore.Migrations;

namespace Sushi.Room.Infrastructure.Migrations
{
    public partial class RemoveRoleColumnFromUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
