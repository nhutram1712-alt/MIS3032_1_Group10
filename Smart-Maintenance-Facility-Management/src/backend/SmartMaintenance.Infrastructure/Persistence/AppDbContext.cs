using Microsoft.EntityFrameworkCore;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Asset> AssetsSet => Set<Asset>();
    public DbSet<MaintenanceRequest> MaintenanceRequestsSet => Set<MaintenanceRequest>();
    public DbSet<WorkOrder> WorkOrdersSet => Set<WorkOrder>();
    public DbSet<IotDevice> IotDevicesSet => Set<IotDevice>();
    public DbSet<IotMapping> IotMappingsSet => Set<IotMapping>();
    public DbSet<IotData> IotDataSet => Set<IotData>();
    public DbSet<IotAlert> IotAlertsSet => Set<IotAlert>();
    public DbSet<MaintenanceHistory> MaintenanceHistoriesSet => Set<MaintenanceHistory>();
    public DbSet<AiPrediction> AiPredictionsSet => Set<AiPrediction>();

    IQueryable<User> IAppDbContext.Users => Users;
    IQueryable<Asset> IAppDbContext.Assets => AssetsSet;
    IQueryable<MaintenanceRequest> IAppDbContext.MaintenanceRequests => MaintenanceRequestsSet;
    IQueryable<WorkOrder> IAppDbContext.WorkOrders => WorkOrdersSet;
    IQueryable<IotDevice> IAppDbContext.IotDevices => IotDevicesSet;
    IQueryable<IotMapping> IAppDbContext.IotMappings => IotMappingsSet;
    IQueryable<IotData> IAppDbContext.IotData => IotDataSet;
    IQueryable<IotAlert> IAppDbContext.IotAlerts => IotAlertsSet;
    IQueryable<MaintenanceHistory> IAppDbContext.MaintenanceHistories => MaintenanceHistoriesSet;
    IQueryable<AiPrediction> IAppDbContext.AiPredictions => AiPredictionsSet;

    public void AddUser(User user) => Users.Add(user);
    public void AddAsset(Asset asset) => AssetsSet.Add(asset);
    public void AddMaintenanceRequest(MaintenanceRequest request) => MaintenanceRequestsSet.Add(request);
    public void AddWorkOrder(WorkOrder workOrder) => WorkOrdersSet.Add(workOrder);
    public void AddIotDevice(IotDevice device) => IotDevicesSet.Add(device);
    public void AddIotMapping(IotMapping mapping) => IotMappingsSet.Add(mapping);
    public void AddIotData(IotData data) => IotDataSet.Add(data);
    public void AddIotAlert(IotAlert alert) => IotAlertsSet.Add(alert);
    public void AddMaintenanceHistory(MaintenanceHistory history) => MaintenanceHistoriesSet.Add(history);
    public void AddAiPrediction(AiPrediction prediction) => AiPredictionsSet.Add(prediction);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("USERS");
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasColumnName("UserID");
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.PasswordHash).HasMaxLength(200).IsRequired();
            e.Property(x => x.Role).HasMaxLength(50).IsRequired();
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Asset>(e =>
        {
            e.ToTable("ASSETS", t =>
            {
                t.HasCheckConstraint(
                    "CK_ASSETS_Type",
                    $"[Type] IN ('{AssetTypes.WiFi}', '{AssetTypes.AirConditioner}', '{AssetTypes.Projector}', '{AssetTypes.Light}', '{AssetTypes.Fan}')");
                t.HasCheckConstraint(
                    "CK_ASSETS_Status",
                    $"[Status] IN ('{AssetStatuses.Operational}', '{AssetStatuses.Warning}', '{AssetStatuses.Maintenance}', '{AssetStatuses.OutOfService}')");
            });
            e.HasKey(x => x.AssetId);
            e.Property(x => x.AssetId).HasColumnName("AssetID");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Type).HasMaxLength(50).IsRequired();
            e.Property(x => x.Location).HasMaxLength(200).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).IsRequired();
            e.Property(x => x.MaintenanceRisk).HasMaxLength(20);
            e.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<MaintenanceRequest>(e =>
        {
            e.ToTable("MAINTENANCE_REQUESTS", t =>
            {
                t.HasCheckConstraint(
                    "CK_REQUESTS_Status",
                    "[Status] IN ('Submitted', 'Pending', 'In Progress', 'Resolved', 'Closed', 'Rejected')");
            });
            e.HasKey(x => x.RequestId);
            e.Property(x => x.RequestId).HasColumnName("RequestID");
            e.Property(x => x.RequesterId).HasColumnName("RequesterID").IsRequired();
            e.Property(x => x.AssetId).HasColumnName("AssetID");
            e.Property(x => x.Description).HasMaxLength(500).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<WorkOrder>(e =>
        {
            e.ToTable("WORK_ORDERS", t =>
            {
                t.HasCheckConstraint(
                    "CK_WORKORDERS_Status",
                    "[Status] IN ('Assigned', 'In Progress', 'Completed', 'Cancelled')");
            });
            e.HasKey(x => x.OrderId);
            e.Property(x => x.OrderId).HasColumnName("OrderID");
            e.Property(x => x.RequestId).HasColumnName("RequestID").IsRequired();
            e.HasIndex(x => x.RequestId).IsUnique();
            e.Property(x => x.TechnicianId).HasColumnName("TechnicianID").IsRequired();
            e.Property(x => x.AssetId).HasColumnName("AssetID").IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).IsRequired();
            e.Property(x => x.RejectionReason).HasMaxLength(500);
            e.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<IotDevice>(e =>
        {
            e.ToTable("IOT_DEVICES");
            e.HasKey(x => x.DeviceId);
            e.Property(x => x.DeviceId).HasColumnName("DeviceID");
            e.Property(x => x.ExternalId).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.ExternalId).IsUnique();
            e.Property(x => x.DeviceName).HasMaxLength(200);
            e.Property(x => x.DeviceType).HasMaxLength(100);
        });

        modelBuilder.Entity<IotMapping>(e =>
        {
            e.ToTable("IOT_MAPPINGS");
            e.HasKey(x => x.MappingId);
            e.Property(x => x.AssetId).HasColumnName("AssetID").IsRequired();
            e.Property(x => x.DeviceId).HasColumnName("DeviceID").IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.HasIndex(x => x.AssetId).IsUnique();
            e.HasIndex(x => x.DeviceId).IsUnique();
        });

        modelBuilder.Entity<IotData>(e =>
        {
            e.ToTable("IOT_DATA", t =>
            {
                t.HasCheckConstraint(
                    "CK_IOTDATA_MetricType",
                    "[MetricType] IN ('temperature', 'humidity', 'power_status')");
            });
            e.HasKey(x => x.DataId);
            e.Property(x => x.DataId).HasColumnName("DataID");
            e.Property(x => x.DeviceId).HasColumnName("DeviceID").IsRequired();
            e.Property(x => x.MetricType).HasMaxLength(50).IsRequired();
            e.Property(x => x.ReadingValue).IsRequired();
            e.Property(x => x.Timestamp).IsRequired();
            e.HasIndex(x => new { x.DeviceId, x.Timestamp });
        });

        modelBuilder.Entity<IotAlert>(e =>
        {
            e.ToTable("IOT_ALERTS", t =>
            {
                t.HasCheckConstraint(
                    "CK_IOTALERTS_Severity",
                    "[Severity] IN ('Low', 'Medium', 'High')");
            });
            e.HasKey(x => x.AlertId);
            e.Property(x => x.AlertId).HasColumnName("AlertID");
            e.Property(x => x.DataId).HasColumnName("DataID").IsRequired();
            e.Property(x => x.AssetId).HasColumnName("AssetID").IsRequired();
            e.Property(x => x.MetricType).HasMaxLength(100).IsRequired();
            e.Property(x => x.ReadingValue).IsRequired();
            e.Property(x => x.Threshold).IsRequired();
            e.Property(x => x.Severity).HasMaxLength(20).IsRequired();
            e.Property(x => x.DetectedAt).IsRequired();
            e.HasIndex(x => new { x.AssetId, x.DetectedAt });
        });

        modelBuilder.Entity<MaintenanceHistory>(e =>
        {
            e.ToTable("MAINTENANCE_HISTORY");
            e.HasKey(x => x.HistoryId);
            e.Property(x => x.HistoryId).HasColumnName("HistoryID");
            e.Property(x => x.AssetId).HasColumnName("AssetID").IsRequired();
            e.Property(x => x.WorkOrderId).HasColumnName("OrderID");
            e.Property(x => x.Result).HasMaxLength(500).IsRequired();
            e.Property(x => x.CompletedAt).IsRequired();
        });

        modelBuilder.Entity<AiPrediction>(e =>
        {
            e.ToTable("AI_PREDICTIONS", t =>
            {
                t.HasCheckConstraint(
                    "CK_AIPREDICTIONS_Risk",
                    "[RiskLevel] IN ('Low', 'Medium', 'High')");
            });
            e.HasKey(x => x.PredictionId);
            e.Property(x => x.PredictionId).HasColumnName("PredictionID");
            e.Property(x => x.AssetId).HasColumnName("AssetID").IsRequired();
            e.Property(x => x.RiskLevel).HasMaxLength(20).IsRequired();
            e.Property(x => x.PredictedAt).IsRequired();
            e.Property(x => x.BasedOnSampleData).IsRequired().HasDefaultValue(false);
            e.HasIndex(x => new { x.AssetId, x.PredictedAt });
        });
    }
}
