using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharingApp.Migrations
{
    public partial class UpdateContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "Contacts",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Contacts");
        }
    }
}
