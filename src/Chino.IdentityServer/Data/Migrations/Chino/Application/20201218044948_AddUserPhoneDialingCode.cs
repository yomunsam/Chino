using Microsoft.EntityFrameworkCore.Migrations;

namespace Chino.IdentityServer.Data.Migrations.Chino.Application
{
    public partial class AddUserPhoneDialingCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhoneDialingCode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneDialingCode",
                table: "AspNetUsers");
        }
    }
}
