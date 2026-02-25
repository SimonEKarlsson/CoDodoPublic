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
                name: "Proceses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opportunity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SalesLead = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyRate = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateOnly>(type: "date", nullable: false),
                    GenerationDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proceses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proceses");
        }
    }
}
