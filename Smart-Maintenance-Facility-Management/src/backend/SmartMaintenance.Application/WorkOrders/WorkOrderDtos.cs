using SmartMaintenance.Application.Assets;

namespace SmartMaintenance.Application.WorkOrders;

public sealed class CreateWorkOrderPayload
{
    public int? RequestId { get; set; }
    public int? TechnicianId { get; set; }
    public int? AssetId { get; set; }
}

public sealed class PatchWorkOrderPayload
{
    public int? TechnicianId { get; set; }
    public string? Status { get; set; }
    public string? RejectionReason { get; set; }
    public string? Result { get; set; }
}

public class WorkOrderResponse
{
    public int OrderId { get; set; }
    public int RequestId { get; set; }
    public int TechnicianId { get; set; }
    public int AssetId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class WorkOrderHistoryItem
{
    public int HistoryId { get; set; }
    public int AssetId { get; set; }
    public int? WorkOrderId { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
}

public sealed class WorkOrderDetailResponse : WorkOrderResponse
{
    public AssetResponse? Asset { get; set; }
    public IReadOnlyList<WorkOrderHistoryItem> MaintenanceHistory { get; set; } = [];
}
