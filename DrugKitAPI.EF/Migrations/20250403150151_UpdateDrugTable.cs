using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugKitAPI.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDrugTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "oldprice",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "units",
                table: "Drugs");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Drugs",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Drugs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "dosage_form",
                table: "Drugs",
                newName: "Dosage_form");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Drugs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "company",
                table: "Drugs",
                newName: "Company");

            migrationBuilder.RenameColumn(
                name: "barcode",
                table: "Drugs",
                newName: "Barcode");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Drugs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "imported",
                table: "Drugs",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "arabic",
                table: "Drugs",
                newName: "Side_effects");

            migrationBuilder.AddColumn<string>(
                name: "Entire_comp",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Primary_comp",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entire_comp",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "Primary_comp",
                table: "Drugs");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Drugs",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Drugs",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Dosage_form",
                table: "Drugs",
                newName: "dosage_form");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Drugs",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "Drugs",
                newName: "company");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "Drugs",
                newName: "barcode");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Drugs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Drugs",
                newName: "imported");

            migrationBuilder.RenameColumn(
                name: "Side_effects",
                table: "Drugs",
                newName: "arabic");

            migrationBuilder.AddColumn<string>(
                name: "active",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "oldprice",
                table: "Drugs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "units",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
