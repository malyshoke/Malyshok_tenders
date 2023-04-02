using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LR2_Malyshok.Migrations
{
    /// <inheritdoc />
    public partial class help : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyRating = table.Column<float>(type: "real", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Tender",
                columns: table => new
                {
                    TenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenderStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenderEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenderBudget = table.Column<float>(type: "real", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tender", x => x.TenderId);
                    table.ForeignKey(
                        name: "FK_Tender_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "Tendering",
                columns: table => new
                {
                    TenderingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentBid = table.Column<float>(type: "real", nullable: false),
                    TenderId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tendering", x => x.TenderingId);
                    table.ForeignKey(
                        name: "FK_Tendering_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tendering_Tender_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tender",
                        principalColumn: "TenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tender_CompanyId",
                table: "Tender",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tendering_CompanyId",
                table: "Tendering",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tendering_TenderId",
                table: "Tendering",
                column: "TenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tendering");

            migrationBuilder.DropTable(
                name: "Tender");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
