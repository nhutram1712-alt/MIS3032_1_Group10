using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMaintenance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIotAlertsAndPredictionFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IOT_MAPPINGS",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "IOT_DEVICES",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "IOT_DEVICES",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnSampleData",
                table: "AI_PREDICTIONS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IOT_ALERTS",
                columns: table => new
                {
                    AlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataID = table.Column<int>(type: "int", nullable: false),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    MetricType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReadingValue = table.Column<double>(type: "float", nullable: false),
                    Threshold = table.Column<double>(type: "float", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOT_ALERTS", x => x.AlertID);
                    table.CheckConstraint("CK_IOTALERTS_Severity", "[Severity] IN ('Low', 'Medium', 'High')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IOT_ALERTS_AssetID_DetectedAt",
                table: "IOT_ALERTS",
                columns: new[] { "AssetID", "DetectedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IOT_ALERTS");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IOT_MAPPINGS");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "IOT_DEVICES");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "IOT_DEVICES");

            migrationBuilder.DropColumn(
                name: "BasedOnSampleData",
                table: "AI_PREDICTIONS");
        }
    }
}
