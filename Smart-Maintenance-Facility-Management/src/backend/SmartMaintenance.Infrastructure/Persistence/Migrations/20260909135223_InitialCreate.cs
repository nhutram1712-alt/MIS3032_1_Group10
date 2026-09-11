using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMaintenance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AI_PREDICTIONS",
                columns: table => new
                {
                    PredictionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PredictedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AI_PREDICTIONS", x => x.PredictionID);
                    table.CheckConstraint("CK_AIPREDICTIONS_Risk", "[RiskLevel] IN ('Low', 'Medium', 'High')");
                });

            migrationBuilder.CreateTable(
                name: "ASSETS",
                columns: table => new
                {
                    AssetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaintenanceRisk = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASSETS", x => x.AssetID);
                    table.CheckConstraint("CK_ASSETS_Status", "[Status] IN ('Operational', 'Warning', 'Maintenance', 'Out of Service')");
                    table.CheckConstraint("CK_ASSETS_Type", "[Type] IN ('Wi-Fi', 'Air Conditioner', 'Projector', 'Light', 'Fan')");
                });

            migrationBuilder.CreateTable(
                name: "IOT_DATA",
                columns: table => new
                {
                    DataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    MetricType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReadingValue = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOT_DATA", x => x.DataID);
                    table.CheckConstraint("CK_IOTDATA_MetricType", "[MetricType] IN ('temperature', 'humidity', 'power_status')");
                });

            migrationBuilder.CreateTable(
                name: "IOT_DEVICES",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOT_DEVICES", x => x.DeviceID);
                });

            migrationBuilder.CreateTable(
                name: "IOT_MAPPINGS",
                columns: table => new
                {
                    MappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOT_MAPPINGS", x => x.MappingId);
                });

            migrationBuilder.CreateTable(
                name: "MAINTENANCE_HISTORY",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAINTENANCE_HISTORY", x => x.HistoryID);
                });

            migrationBuilder.CreateTable(
                name: "MAINTENANCE_REQUESTS",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterID = table.Column<int>(type: "int", nullable: false),
                    AssetID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAINTENANCE_REQUESTS", x => x.RequestID);
                    table.CheckConstraint("CK_REQUESTS_Status", "[Status] IN ('Submitted', 'Pending', 'In Progress', 'Resolved', 'Closed', 'Rejected')");
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "WORK_ORDERS",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    TechnicianID = table.Column<int>(type: "int", nullable: false),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORK_ORDERS", x => x.OrderID);
                    table.CheckConstraint("CK_WORKORDERS_Status", "[Status] IN ('Assigned', 'In Progress', 'Completed', 'Cancelled')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AI_PREDICTIONS_AssetID_PredictedAt",
                table: "AI_PREDICTIONS",
                columns: new[] { "AssetID", "PredictedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_IOT_DATA_DeviceID_Timestamp",
                table: "IOT_DATA",
                columns: new[] { "DeviceID", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_IOT_DEVICES_ExternalId",
                table: "IOT_DEVICES",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IOT_MAPPINGS_AssetID",
                table: "IOT_MAPPINGS",
                column: "AssetID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IOT_MAPPINGS_DeviceID",
                table: "IOT_MAPPINGS",
                column: "DeviceID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Username",
                table: "USERS",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WORK_ORDERS_RequestID",
                table: "WORK_ORDERS",
                column: "RequestID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AI_PREDICTIONS");

            migrationBuilder.DropTable(
                name: "ASSETS");

            migrationBuilder.DropTable(
                name: "IOT_DATA");

            migrationBuilder.DropTable(
                name: "IOT_DEVICES");

            migrationBuilder.DropTable(
                name: "IOT_MAPPINGS");

            migrationBuilder.DropTable(
                name: "MAINTENANCE_HISTORY");

            migrationBuilder.DropTable(
                name: "MAINTENANCE_REQUESTS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "WORK_ORDERS");
        }
    }
}
