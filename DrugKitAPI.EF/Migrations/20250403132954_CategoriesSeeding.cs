using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugKitAPI.EF.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ImageUrl" },
                values: new object[,]
                {
                    { 1, "Pain Relief", "Images/CategoriesImages/Pain_Relief.svg" },
                    { 2, "Skin", "Images/CategoriesImages/Skin.svg" },
                    { 3, "Heart", "Images/CategoriesImages/Heart.svg" },
                    { 4, "Headache", "Images/CategoriesImages/Headache.svg" },
                    { 5, "Muscles", "Images/CategoriesImages/Muscles.svg" },
                    { 6, "Eye & Ear", "Images/CategoriesImages/Eye&Ear.svg" },
                    { 7, "Cold & Flu", "Images/CategoriesImages/Cold&Flu.svg" },
                    { 8, "Diabetes", "Images/CategoriesImages/Diabetes.svg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }
    }
}
