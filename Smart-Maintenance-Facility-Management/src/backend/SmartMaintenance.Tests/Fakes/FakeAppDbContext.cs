using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Domain.Entities;

namespace SmartMaintenance.Tests.Fakes;

public sealed class FakeAppDbContext : IAppDbContext
{
    private int _userId = 1;
    private int _assetId = 1;
    private int _requestId = 1;
    private int _orderId = 1;
    private int _deviceId = 1;
    private int _mappingId = 1;
    private int _dataId = 1;
    private int _alertId = 1;
    private int _historyId = 1;
    private int _predictionId = 1;

    public List<User> UserList { get; } = [];
    public List<Asset> AssetList { get; } = [];
    public List<MaintenanceRequest> RequestList { get; } = [];
    public List<WorkOrder> WorkOrderList { get; } = [];
    public List<IotDevice> DeviceList { get; } = [];
    public List<IotMapping> MappingList { get; } = [];
    public List<IotData> DataList { get; } = [];
    public List<IotAlert> AlertList { get; } = [];
    public List<MaintenanceHistory> HistoryList { get; } = [];
    public List<AiPrediction> PredictionList { get; } = [];

    public IQueryable<User> Users => UserList.AsQueryable();
    public IQueryable<Asset> Assets => AssetList.AsQueryable();
    public IQueryable<MaintenanceRequest> MaintenanceRequests => RequestList.AsQueryable();
    public IQueryable<WorkOrder> WorkOrders => WorkOrderList.AsQueryable();
    public IQueryable<IotDevice> IotDevices => DeviceList.AsQueryable();
    public IQueryable<IotMapping> IotMappings => MappingList.AsQueryable();
    public IQueryable<IotData> IotData => DataList.AsQueryable();
    public IQueryable<IotAlert> IotAlerts => AlertList.AsQueryable();
    public IQueryable<MaintenanceHistory> MaintenanceHistories => HistoryList.AsQueryable();
    public IQueryable<AiPrediction> AiPredictions => PredictionList.AsQueryable();

    public User SeedUser(User user)
    {
        user.UserId = _userId++;
        UserList.Add(user);
        return user;
    }

    public void AddUser(User user)
    {
        if (user.UserId == 0)
            user.UserId = _userId++;
        UserList.Add(user);
    }

    public void AddAsset(Asset asset)
    {
        if (asset.AssetId == 0)
            asset.AssetId = _assetId++;
        AssetList.Add(asset);
    }

    public void AddMaintenanceRequest(MaintenanceRequest request)
    {
        if (request.RequestId == 0)
            request.RequestId = _requestId++;
        RequestList.Add(request);
    }

    public void AddWorkOrder(WorkOrder workOrder)
    {
        if (workOrder.OrderId == 0)
            workOrder.OrderId = _orderId++;
        WorkOrderList.Add(workOrder);
    }

    public void AddIotDevice(IotDevice device)
    {
        if (device.DeviceId == 0)
            device.DeviceId = _deviceId++;
        DeviceList.Add(device);
    }

    public void AddIotMapping(IotMapping mapping)
    {
        if (mapping.MappingId == 0)
            mapping.MappingId = _mappingId++;
        MappingList.Add(mapping);
    }

    public void AddIotData(IotData data)
    {
        if (data.DataId == 0)
            data.DataId = _dataId++;
        DataList.Add(data);
    }

    public void AddIotAlert(IotAlert alert)
    {
        if (alert.AlertId == 0)
            alert.AlertId = _alertId++;
        AlertList.Add(alert);
    }

    public void AddMaintenanceHistory(MaintenanceHistory history)
    {
        if (history.HistoryId == 0)
            history.HistoryId = _historyId++;
        HistoryList.Add(history);
    }

    public void AddAiPrediction(AiPrediction prediction)
    {
        if (prediction.PredictionId == 0)
            prediction.PredictionId = _predictionId++;
        PredictionList.Add(prediction);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => Task.FromResult(1);
}
