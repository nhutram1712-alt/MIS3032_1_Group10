namespace SmartMaintenance.Application.Requests;

public sealed class CreateRequestPayload
{
    public int? AssetId { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateRequestStatusPayload
{
    public string? Status { get; set; }
}

public sealed class RequestResponse
{
    public int RequestId { get; set; }
    public int RequesterId { get; set; }
    public int? AssetId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public sealed class RequestHistoryResponse
{
    public int RequestId { get; set; }
    public int? WorkOrderId { get; set; }
    public int? TechnicianId { get; set; }
    public string? Result { get; set; }
    public DateTime? CompletedAt { get; set; }
}
