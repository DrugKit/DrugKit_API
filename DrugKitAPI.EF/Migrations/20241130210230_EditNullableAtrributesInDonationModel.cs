using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugKitAPI.EF.Migrations
{
    /// <inheritdoc />
    public partial class EditNullableAtrributesInDonationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_SystemAdmins_SystemAdminId",
                table: "Donations");

            migrationBuilder.AlterColumn<int>(
                name: "SystemAdminId",
                table: "Donations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdminActionDate",
                table: "Donations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_SystemAdmins_SystemAdminId",
                table: "Donations",
                column: "SystemAdminId",
                principalTable: "SystemAdmins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_SystemAdmins_SystemAdminId",
                table: "Donations");

            migrationBuilder.AlterColumn<int>(
                name: "SystemAdminId",
                table: "Donations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdminActionDate",
                table: "Donations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_SystemAdmins_SystemAdminId",
                table: "Donations",
                column: "SystemAdminId",
                principalTable: "SystemAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
