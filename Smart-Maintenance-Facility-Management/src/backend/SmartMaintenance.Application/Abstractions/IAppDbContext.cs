using SmartMaintenance.Domain.Entities;

namespace SmartMaintenance.Application.Abstractions;

public interface IAppDbContext
{
    IQueryable<User> Users { get; }
    IQueryable<Asset> Assets { get; }
    IQueryable<MaintenanceRequest> MaintenanceRequests { get; }
    IQueryable<WorkOrder> WorkOrders { get; }
    IQueryable<IotDevice> IotDevices { get; }
    IQueryable<IotMapping> IotMappings { get; }
    IQueryable<IotData> IotData { get; }
    IQueryable<IotAlert> IotAlerts { get; }
    IQueryable<MaintenanceHistory> MaintenanceHistories { get; }
    IQueryable<AiPrediction> AiPredictions { get; }

    void AddUser(User user);
    void AddAsset(Asset asset);
    void AddMaintenanceRequest(MaintenanceRequest request);
    void AddWorkOrder(WorkOrder workOrder);
    void AddIotDevice(IotDevice device);
    void AddIotMapping(IotMapping mapping);
    void AddIotData(IotData data);
    void AddIotAlert(IotAlert alert);
    void AddMaintenanceHistory(MaintenanceHistory history);
    void AddAiPrediction(AiPrediction prediction);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
