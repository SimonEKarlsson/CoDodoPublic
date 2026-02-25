using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoDodoApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UriForAssignment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOFSalesLead = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyRateInSEK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => new { x.Name, x.UriForAssignment });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Name_UriForAssignment",
                table: "Processes",
                columns: new[] { "Name", "UriForAssignment" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processes");
        }
    }
}
