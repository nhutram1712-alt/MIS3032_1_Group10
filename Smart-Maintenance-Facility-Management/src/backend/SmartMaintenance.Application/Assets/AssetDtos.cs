namespace SmartMaintenance.Application.Assets;

public sealed class CreateAssetRequest
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
}

public sealed class UpdateAssetRequest
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Location { get; set; }
}

public sealed class UpdateAssetStatusRequest
{
    public string? Status { get; set; }
}

public sealed class AssetResponse
{
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? MaintenanceRisk { get; set; }
    public DateTime CreatedAt { get; set; }
}
