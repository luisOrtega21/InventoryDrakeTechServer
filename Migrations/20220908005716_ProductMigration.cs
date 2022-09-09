using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORY.SERVER.Migrations
{
    public partial class ProductMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<float>(type: "real", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Date", "DateCreated", "DateUpdated", "Description", "Name", "Price", "Quantity", "Reference" },
                values: new object[] { new Guid("2518af24-6012-4dba-8a33-34d4a2b865d9"), new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manu", "Manubrio", 40000f, 10, "Man" });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Date", "DateCreated", "DateUpdated", "Description", "Name", "Price", "Quantity", "Reference" },
                values: new object[] { new Guid("7151b821-5ea1-4e04-b14f-d5cf3fbcb3de"), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GMW", "Llanta", 60000f, 10, "LL" });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Date", "DateCreated", "DateUpdated", "Description", "Name", "Price", "Quantity", "Reference" },
                values: new object[] { new Guid("ec2bed18-bafd-46e2-a0f2-69bbe9a8ca46"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fren", "Frenos", 20000f, 10, "Fren" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
