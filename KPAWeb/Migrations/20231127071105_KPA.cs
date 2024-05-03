using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KPAWeb.Migrations
{
    /// <inheritdoc />
    public partial class KPA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KPAs",
                columns: table => new
                {
                    KPA_No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KPA_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weighting = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPAs", x => x.KPA_No);
                });

            migrationBuilder.CreateTable(
                name: "KPIs",
                columns: table => new
                {
                    KPI_No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KPA_Ref_No = table.Column<int>(type: "int", nullable: false),
                    KPI_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weighting = table.Column<int>(type: "int", nullable: false),
                    Total_Weight = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIs", x => x.KPI_No);
                    table.ForeignKey(
                        name: "FK_KPIs_KPAs_KPA_Ref_No",
                        column: x => x.KPA_Ref_No,
                        principalTable: "KPAs",
                        principalColumn: "KPA_No",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KPIEvidences",
                columns: table => new
                {
                    Evidence_No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KPI_Ref_No = table.Column<int>(type: "int", nullable: false),
                    Months = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    No_Of_Days = table.Column<int>(type: "int", nullable: false, computedColumnSql: "DateDiff(dd, [Start_Date], [End_Date])"),
                    Own_Score = table.Column<int>(type: "int", nullable: false),
                    Line_Manager_Score = table.Column<int>(type: "int", nullable: false),
                    Weighting = table.Column<int>(type: "int", nullable: false),
                    Final_Score = table.Column<int>(type: "double", nullable: false, computedColumnSql: "(cast([Weighting] as float) / 100) * cast([Line_Manager_Score] as Float)"),
                    File = table.Column<int>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIEvidences", x => x.Evidence_No);
                    table.ForeignKey(
                        name: "FK_KPIEvidences_KPIs_KPI_Ref_No",
                        column: x => x.KPI_Ref_No,
                        principalTable: "KPIs",
                        principalColumn: "KPI_No",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KPIEvidences_KPI_Ref_No",
                table: "KPIEvidences",
                column: "KPI_Ref_No");

            migrationBuilder.CreateIndex(
                name: "IX_KPIs_KPA_Ref_No",
                table: "KPIs",
                column: "KPA_Ref_No");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPIEvidences");

            migrationBuilder.DropTable(
                name: "KPIs");

            migrationBuilder.DropTable(
                name: "KPAs");
        }
    }
}
