using Microsoft.EntityFrameworkCore;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Infrastructure.Persistence;

public static class DbSeeder
{
    public const string DemoPassword = "Due@2026";

    public static async Task SeedAsync(AppDbContext db)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(DemoPassword);
        var demos = new (string Username, string Role, string FullName)[]
        {
            ("manager", UserRoles.FacilityManager, "Nguyễn Văn Quản"),
            ("requester", UserRoles.Requester, "Nguyễn Thị Yêu Cầu"),
            ("requester2", UserRoles.Requester, "Phạm Văn Báo Cáo"),
            ("tech1", UserRoles.Technician, "Trần Văn Kỹ"),
            ("tech2", UserRoles.Technician, "Lê Thị Sửa Chữa"),
            ("tech3", UserRoles.Technician, "Hoàng Minh Bảo Trì"),
            ("admin", UserRoles.Admin, "Lê Thị Quản Trị")
        };

        foreach (var (username, role, fullName) in demos)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null)
            {
                db.Users.Add(new User
                {
                    Username = username,
                    PasswordHash = hash,
                    Role = role,
                    FullName = fullName,
                    IsActive = true
                });
            }
            else
            {
                user.PasswordHash = hash;
                user.Role = role;
                user.FullName = fullName;
                user.IsActive = true;
            }
        }

        await db.SaveChangesAsync();
        await SeedCampusDataAsync(db);
        await EnsureUnmappedDemoAssetsAsync(db);
        await EnsureTelemetryAsync(db);
    }

    /// <summary>Tài sản cố ý chưa IoT-map — dùng demo form “Tạo liên kết mới”.</summary>
    private static readonly (string Name, string Type, string Location, string Status, string Risk)[] DemoUnmappedAssets =
    [
        ("Điều hòa Casper F201", AssetTypes.AirConditioner, "Phòng F201", AssetStatuses.Operational, RiskLevels.Low),
        ("Máy chiếu Vivitek F201", AssetTypes.Projector, "Phòng F201", AssetStatuses.Operational, RiskLevels.Low),
        ("Đèn LED F201", AssetTypes.Light, "Phòng F201", AssetStatuses.Operational, RiskLevels.Low),
        ("Quạt trần F201", AssetTypes.Fan, "Phòng F201", AssetStatuses.Warning, RiskLevels.Medium),
        ("Access Point F201", AssetTypes.WiFi, "Phòng F201", AssetStatuses.Operational, RiskLevels.Low),
        ("Điều hòa Sharp G101", AssetTypes.AirConditioner, "Phòng G101", AssetStatuses.Operational, RiskLevels.Low),
        ("Máy chiếu Acer G101", AssetTypes.Projector, "Phòng G101", AssetStatuses.Operational, RiskLevels.Low)
    ];

    private static async Task EnsureUnmappedDemoAssetsAsync(AppDbContext db)
    {
        var managerId = await db.Users.Where(u => u.Username == "manager").Select(u => u.UserId).SingleAsync();
        var now = DateTime.UtcNow;
        var existing = await db.AssetsSet.Select(a => a.Name).ToListAsync();
        var existingSet = existing.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var toAdd = DemoUnmappedAssets
            .Where(x => !existingSet.Contains(x.Name))
            .Select((x, i) => new Asset
            {
                Name = x.Name,
                Type = x.Type,
                Location = x.Location,
                Status = x.Status,
                MaintenanceRisk = x.Risk,
                CreatedAt = now.AddDays(-(i + 1)),
                CreatedByUserId = managerId
            })
            .ToList();

        if (toAdd.Count == 0) return;
        db.AssetsSet.AddRange(toAdd);
        await db.SaveChangesAsync();
    }

    private static async Task SeedCampusDataAsync(AppDbContext db)
    {
        if (await db.AssetsSet.AnyAsync()) return;

        var managerId = await db.Users.Where(u => u.Username == "manager").Select(u => u.UserId).SingleAsync();
        var requesterId = await db.Users.Where(u => u.Username == "requester").Select(u => u.UserId).SingleAsync();
        var requester2Id = await db.Users.Where(u => u.Username == "requester2").Select(u => u.UserId).SingleAsync();
        var tech1 = await db.Users.Where(u => u.Username == "tech1").Select(u => u.UserId).SingleAsync();
        var tech2 = await db.Users.Where(u => u.Username == "tech2").Select(u => u.UserId).SingleAsync();
        var tech3 = await db.Users.Where(u => u.Username == "tech3").Select(u => u.UserId).SingleAsync();
        var now = DateTime.UtcNow;

        Asset A(string name, string type, string loc, string status, string risk, int daysAgo) => new()
        {
            Name = name,
            Type = type,
            Location = loc,
            Status = status,
            MaintenanceRisk = risk,
            CreatedAt = now.AddDays(-daysAgo),
            CreatedByUserId = managerId
        };

        // Mỗi phòng/khu vực có nhiều thiết bị (điều hòa, máy chiếu, đèn, quạt, Wi-Fi…)
        var assets = new List<Asset>
        {
            // Phòng A301
            A("Điều hòa Panasonic A301", AssetTypes.AirConditioner, "Phòng A301", AssetStatuses.Warning, RiskLevels.Medium, 12),
            A("Máy chiếu Epson A301", AssetTypes.Projector, "Phòng A301", AssetStatuses.Operational, RiskLevels.Low, 11),
            A("Đèn LED A301", AssetTypes.Light, "Phòng A301", AssetStatuses.Operational, RiskLevels.Low, 10),
            A("Quạt trần A301", AssetTypes.Fan, "Phòng A301", AssetStatuses.Operational, RiskLevels.Low, 9),
            A("Access Point A301", AssetTypes.WiFi, "Phòng A301", AssetStatuses.Operational, RiskLevels.Low, 9),
            // Phòng A302
            A("Điều hòa Daikin A302", AssetTypes.AirConditioner, "Phòng A302", AssetStatuses.Operational, RiskLevels.Low, 11),
            A("Máy chiếu Sony A302", AssetTypes.Projector, "Phòng A302", AssetStatuses.Warning, RiskLevels.Medium, 10),
            A("Đèn LED A302", AssetTypes.Light, "Phòng A302", AssetStatuses.Operational, RiskLevels.Low, 9),
            A("Quạt trần A302", AssetTypes.Fan, "Phòng A302", AssetStatuses.Operational, RiskLevels.Low, 8),
            A("Access Point A302", AssetTypes.WiFi, "Phòng A302", AssetStatuses.Operational, RiskLevels.Low, 8),
            // Phòng B201
            A("Điều hòa LG B201", AssetTypes.AirConditioner, "Phòng B201", AssetStatuses.Operational, RiskLevels.Low, 10),
            A("Máy chiếu Epson B201", AssetTypes.Projector, "Phòng B201", AssetStatuses.Operational, RiskLevels.Low, 10),
            A("Đèn LED B201", AssetTypes.Light, "Phòng B201", AssetStatuses.Warning, RiskLevels.Medium, 8),
            A("Quạt trần B201", AssetTypes.Fan, "Phòng B201", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Access Point B201", AssetTypes.WiFi, "Phòng B201", AssetStatuses.Warning, RiskLevels.Medium, 7),
            // Phòng B105
            A("Điều hòa Carrier B105", AssetTypes.AirConditioner, "Phòng B105", AssetStatuses.Warning, RiskLevels.Medium, 9),
            A("Máy chiếu Sony B105", AssetTypes.Projector, "Phòng B105", AssetStatuses.Warning, RiskLevels.Medium, 7),
            A("Đèn LED B105", AssetTypes.Light, "Phòng B105", AssetStatuses.Operational, RiskLevels.Low, 6),
            A("Quạt trần B105", AssetTypes.Fan, "Phòng B105", AssetStatuses.Operational, RiskLevels.Low, 5),
            A("Access Point B105", AssetTypes.WiFi, "Phòng B105", AssetStatuses.Operational, RiskLevels.Low, 5),
            // Phòng C102
            A("Điều hòa Daikin C102", AssetTypes.AirConditioner, "Phòng C102", AssetStatuses.Operational, RiskLevels.Low, 8),
            A("Máy chiếu Optoma C102", AssetTypes.Projector, "Phòng C102", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Đèn LED C102", AssetTypes.Light, "Phòng C102", AssetStatuses.Warning, RiskLevels.Medium, 5),
            A("Quạt trần C102", AssetTypes.Fan, "Phòng C102", AssetStatuses.OutOfService, RiskLevels.High, 4),
            A("Access Point C102", AssetTypes.WiFi, "Phòng C102", AssetStatuses.Operational, RiskLevels.Low, 4),
            // Phòng C201
            A("Điều hòa Panasonic C201", AssetTypes.AirConditioner, "Phòng C201", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Máy chiếu BenQ C201", AssetTypes.Projector, "Phòng C201", AssetStatuses.Operational, RiskLevels.Low, 6),
            A("Đèn LED C201", AssetTypes.Light, "Phòng C201", AssetStatuses.Operational, RiskLevels.Low, 5),
            A("Quạt trần C201", AssetTypes.Fan, "Phòng C201", AssetStatuses.Operational, RiskLevels.Low, 3),
            A("Access Point C201", AssetTypes.WiFi, "Phòng C201", AssetStatuses.Operational, RiskLevels.Low, 3),
            // Lab CNTT 1
            A("Điều hòa LG Lab CNTT", AssetTypes.AirConditioner, "Lab CNTT 1", AssetStatuses.Operational, RiskLevels.Low, 8),
            A("Máy chiếu Optoma Lab CNTT", AssetTypes.Projector, "Lab CNTT 1", AssetStatuses.Operational, RiskLevels.Low, 6),
            A("Đèn LED Lab CNTT", AssetTypes.Light, "Lab CNTT 1", AssetStatuses.Operational, RiskLevels.Low, 5),
            A("Quạt hộp Lab CNTT", AssetTypes.Fan, "Lab CNTT 1", AssetStatuses.Warning, RiskLevels.Medium, 4),
            A("Access Point Lab CNTT", AssetTypes.WiFi, "Lab CNTT 1", AssetStatuses.Operational, RiskLevels.Low, 4),
            // Hội trường A
            A("Điều hòa LG Hội trường", AssetTypes.AirConditioner, "Hội trường A", AssetStatuses.Maintenance, RiskLevels.High, 9),
            A("Máy chiếu ViewSonic Hội trường", AssetTypes.Projector, "Hội trường A", AssetStatuses.Operational, RiskLevels.Low, 8),
            A("Đèn LED Hội trường A", AssetTypes.Light, "Hội trường A", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Quạt trần Hội trường A", AssetTypes.Fan, "Hội trường A", AssetStatuses.Operational, RiskLevels.Low, 6),
            A("Access Point Hội trường A", AssetTypes.WiFi, "Hội trường A", AssetStatuses.Operational, RiskLevels.Low, 5),
            // Lab Hóa 2
            A("Điều hòa Toshiba Lab Hóa", AssetTypes.AirConditioner, "Lab Hóa 2", AssetStatuses.Maintenance, RiskLevels.High, 13),
            A("Máy chiếu Epson Lab Hóa", AssetTypes.Projector, "Lab Hóa 2", AssetStatuses.Operational, RiskLevels.Low, 10),
            A("Đèn LED Lab Hóa", AssetTypes.Light, "Lab Hóa 2", AssetStatuses.Warning, RiskLevels.Medium, 8),
            A("Quạt hút Lab Hóa", AssetTypes.Fan, "Lab Hóa 2", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Access Point Lab Hóa", AssetTypes.WiFi, "Lab Hóa 2", AssetStatuses.Warning, RiskLevels.Medium, 7),
            // Phòng thi D01
            A("Điều hòa Mitsubishi D01", AssetTypes.AirConditioner, "Phòng thi D01", AssetStatuses.Operational, RiskLevels.Low, 6),
            A("Máy chiếu Casio D01", AssetTypes.Projector, "Phòng thi D01", AssetStatuses.Operational, RiskLevels.Low, 5),
            A("Đèn LED D01", AssetTypes.Light, "Phòng thi D01", AssetStatuses.Operational, RiskLevels.Low, 4),
            A("Quạt đứng phòng thi", AssetTypes.Fan, "Phòng thi D01", AssetStatuses.Warning, RiskLevels.Medium, 1),
            A("Access Point D01", AssetTypes.WiFi, "Phòng thi D01", AssetStatuses.Operational, RiskLevels.Low, 2),
            // Phòng họp E01
            A("Điều hòa Daikin E01", AssetTypes.AirConditioner, "Phòng họp E01", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Máy chiếu BenQ E01", AssetTypes.Projector, "Phòng họp E01", AssetStatuses.Operational, RiskLevels.Low, 6),
            A("Đèn LED phòng họp E01", AssetTypes.Light, "Phòng họp E01", AssetStatuses.Warning, RiskLevels.Medium, 5),
            A("Quạt trần E01", AssetTypes.Fan, "Phòng họp E01", AssetStatuses.Operational, RiskLevels.Low, 4),
            A("Access Point E01", AssetTypes.WiFi, "Phòng họp E01", AssetStatuses.Operational, RiskLevels.Low, 4),
            // Khu vực chung
            A("Access Point tầng 1", AssetTypes.WiFi, "Hành lang tầng 1", AssetStatuses.Operational, RiskLevels.Low, 8),
            A("Đèn LED hành lang tầng 1", AssetTypes.Light, "Hành lang tầng 1", AssetStatuses.Operational, RiskLevels.Low, 7),
            A("Access Point tầng 2", AssetTypes.WiFi, "Hành lang tầng 2", AssetStatuses.Warning, RiskLevels.Medium, 5),
            A("Đèn LED hành lang tầng 2", AssetTypes.Light, "Hành lang tầng 2", AssetStatuses.Maintenance, RiskLevels.Medium, 6),
            A("Access Point tầng 3", AssetTypes.WiFi, "Hành lang tầng 3", AssetStatuses.Warning, RiskLevels.Medium, 7),
            A("Đèn LED hành lang tầng 3", AssetTypes.Light, "Hành lang tầng 3", AssetStatuses.Operational, RiskLevels.Low, 4),
            A("Đèn LED sảnh chính", AssetTypes.Light, "Sảnh chính", AssetStatuses.Operational, RiskLevels.Low, 3),
            A("Access Point sảnh chính", AssetTypes.WiFi, "Sảnh chính", AssetStatuses.Operational, RiskLevels.Low, 3),
            A("Đèn LED cầu thang B", AssetTypes.Light, "Cầu thang B", AssetStatuses.OutOfService, RiskLevels.High, 2),
            A("Access Point cầu thang B", AssetTypes.WiFi, "Cầu thang B", AssetStatuses.Operational, RiskLevels.Low, 2),
            A("Đèn LED bãi xe", AssetTypes.Light, "Bãi xe phía Nam", AssetStatuses.OutOfService, RiskLevels.High, 4),
            A("Access Point bãi xe", AssetTypes.WiFi, "Bãi xe phía Nam", AssetStatuses.Warning, RiskLevels.Medium, 4),
            A("Access Point ký túc xá", AssetTypes.WiFi, "KTX khu A", AssetStatuses.OutOfService, RiskLevels.High, 6),
            A("Đèn LED KTX khu A", AssetTypes.Light, "KTX khu A", AssetStatuses.Warning, RiskLevels.Medium, 5),
            A("Access Point thư viện", AssetTypes.WiFi, "Thư viện tầng 2", AssetStatuses.Operational, RiskLevels.Low, 5),
            A("Access Point khu tự học", AssetTypes.WiFi, "Khu tự học tầng 1", AssetStatuses.Warning, RiskLevels.Medium, 3)
        };

        db.AssetsSet.AddRange(assets);
        await db.SaveChangesAsync();

        Asset Find(string name) => assets.First(a => a.Name == name);

        MaintenanceRequest R(int requester, Asset asset, string desc, string status, double daysAgo) => new()
        {
            RequesterId = requester,
            AssetId = asset.AssetId,
            Description = desc,
            Status = status,
            CreatedAt = now.AddDays(-daysAgo)
        };

        var reqs = new[]
        {
            R(requesterId, Find("Điều hòa Panasonic A301"), "Điều hòa A301 kêu to, làm mát chậm khi giờ học.", RequestStatuses.InProgress, 2),
            R(requesterId, Find("Đèn LED hành lang tầng 2"), "Đèn hành lang tầng 2 nhấp nháy, cần kiểm tra ballast.", RequestStatuses.Pending, 1),
            R(requesterId, Find("Quạt trần C102"), "Quạt trần C102 không chạy, có mùi khét khi bật.", RequestStatuses.Submitted, 0.3),
            R(requester2Id, Find("Điều hòa LG Hội trường"), "Điều hòa hội trường không lạnh đều, tiếng ồn lớn.", RequestStatuses.Resolved, 5),
            R(requester2Id, Find("Máy chiếu Sony B105"), "Máy chiếu B105 báo bóng đèn yếu, hình mờ.", RequestStatuses.InProgress, 1.5),
            R(requesterId, Find("Access Point tầng 2"), "Wi-Fi tầng 2 mất kết nối ngắt quãng buổi chiều.", RequestStatuses.Pending, 0.8),
            R(requester2Id, Find("Đèn LED cầu thang B"), "Đèn cầu thang B cháy hoàn toàn, tối nguy hiểm.", RequestStatuses.Closed, 8),
            R(requesterId, Find("Quạt đứng phòng thi"), "Quạt đứng phòng thi rung mạnh khi tốc độ cao.", RequestStatuses.Submitted, 0.2),
            R(requester2Id, Find("Điều hòa Daikin A302"), "Điều hòa A302 nhỏ giọt nước gần bảng điện.", RequestStatuses.Rejected, 4),
            R(requesterId, Find("Máy chiếu Optoma Lab CNTT"), "Máy chiếu Lab CNTT không nhận HDMI từ laptop giảng viên.", RequestStatuses.Pending, 0.5),
            R(requester2Id, Find("Điều hòa Carrier B105"), "Điều hòa B105 báo lỗi E5, tắt đột ngột giữa giờ.", RequestStatuses.InProgress, 1.1),
            R(requesterId, Find("Điều hòa Toshiba Lab Hóa"), "Lab Hóa nóng bất thường, điều hòa Toshiba không thổi lạnh.", RequestStatuses.Pending, 0.9),
            R(requester2Id, Find("Máy chiếu Sony A302"), "Máy chiếu A302 bị lệch hình, cần căn chỉnh lại.", RequestStatuses.InProgress, 1.8),
            R(requesterId, Find("Access Point tầng 3"), "Wi-Fi tầng 3 chậm, sinh viên khó join lớp online.", RequestStatuses.Pending, 0.7),
            R(requester2Id, Find("Access Point ký túc xá"), "Access Point KTX mất nguồn, cả dãy không có mạng.", RequestStatuses.InProgress, 0.55),
            R(requesterId, Find("Đèn LED phòng họp E01"), "Đèn phòng họp E01 chập chờn khi bật máy chiếu.", RequestStatuses.Pending, 1.3),
            R(requester2Id, Find("Đèn LED bãi xe"), "Đèn bãi xe phía Nam hỏng 4/6 bóng, tối nguy hiểm.", RequestStatuses.Resolved, 6),
            R(requesterId, Find("Quạt trần A301"), "Quạt trần A301 kêu rít khi tốc độ cao.", RequestStatuses.InProgress, 2.2),
            R(requester2Id, Find("Quạt hộp Lab CNTT"), "Quạt hộp Lab CNTT không đảo chiều, nóng máy.", RequestStatuses.Pending, 0.45),
            R(requesterId, Find("Máy chiếu Epson B201"), "Máy chiếu B201 hình bị sọc xanh định kỳ.", RequestStatuses.Closed, 9),
            R(requester2Id, Find("Access Point tầng 1"), "Access Point tầng 1 sóng yếu góc cầu thang.", RequestStatuses.Resolved, 5.5),
            R(requesterId, Find("Đèn LED sảnh chính"), "Đèn sảnh chính tối một phía, cần thay driver.", RequestStatuses.Closed, 10),
            R(requester2Id, Find("Máy chiếu ViewSonic Hội trường"), "Máy chiếu hội trường không lên nguồn sau sự kiện.", RequestStatuses.Rejected, 3.2),
            R(requesterId, Find("Điều hòa Mitsubishi D01"), "Điều hòa phòng thi lọc bẩn, có mùi khi mở máy.", RequestStatuses.Resolved, 7.5),
            R(requester2Id, Find("Access Point Lab CNTT"), "Wi-Fi Lab CNTT rớt khi số lượng thiết bị cao.", RequestStatuses.InProgress, 1.6),
            R(requesterId, Find("Máy chiếu Casio D01"), "Máy chiếu phòng thi remote không bắt tín hiệu.", RequestStatuses.Pending, 0.25),
            R(requester2Id, Find("Đèn LED hành lang tầng 3"), "Đèn hành lang tầng 3 tối đoạn cuối, cần kiểm tra dây.", RequestStatuses.Submitted, 0.15),
            R(requesterId, Find("Quạt trần C201"), "Quạt trần C201 quay chậm dù đặt tốc độ cao.", RequestStatuses.Pending, 1.4),
            R(requester2Id, Find("Đèn LED A302"), "Đèn A302 tối góc bảng, khó nhìn slide.", RequestStatuses.Submitted, 0.1),
            R(requesterId, Find("Máy chiếu Epson A301"), "Máy chiếu A301 không nhận tín hiệu HDMI.", RequestStatuses.Pending, 0.4),
            R(requesterId, Find("Máy chiếu BenQ C201"), "Máy chiếu C201 hình bị cắt mép trên.", RequestStatuses.Submitted, 0.12),
            R(requester2Id, Find("Đèn LED B201"), "Đèn B201 tối nửa phòng khi tắt máy chiếu.", RequestStatuses.Submitted, 0.18),
            R(requesterId, Find("Điều hòa Panasonic C201"), "Điều hòa C201 kêu lạch cạch khi khởi động.", RequestStatuses.Submitted, 0.22),
            R(requester2Id, Find("Quạt trần B105"), "Quạt trần B105 không đảo chiều, nóng góc lớp.", RequestStatuses.Submitted, 0.28),
            R(requesterId, Find("Máy chiếu Epson Lab Hóa"), "Máy chiếu Lab Hóa không kết nối VGA từ máy tính bàn.", RequestStatuses.Submitted, 0.32),
            R(requester2Id, Find("Đèn LED D01"), "Đèn phòng thi D01 nhấp nháy khu vực lối vào.", RequestStatuses.Submitted, 0.36),
            R(requesterId, Find("Access Point Hội trường A"), "Wi-Fi hội trường yếu khi sự kiện đông người.", RequestStatuses.Submitted, 0.4)
        };

        db.MaintenanceRequestsSet.AddRange(reqs);
        await db.SaveChangesAsync();

        WorkOrder W(MaintenanceRequest req, int tech, string status, double daysAgo, string? reject = null) => new()
        {
            RequestId = req.RequestId,
            TechnicianId = tech,
            AssetId = req.AssetId!.Value,
            Status = status,
            RejectionReason = reject,
            CreatedAt = now.AddDays(-daysAgo)
        };

        var woAssigned = W(reqs[0], tech1, WorkOrderStatuses.Assigned, 1);
        var woInProgress = W(reqs[4], tech2, WorkOrderStatuses.InProgress, 1.2);
        var woCompleted1 = W(reqs[3], tech3, WorkOrderStatuses.Completed, 4.5);
        var woCompleted2 = W(reqs[6], tech1, WorkOrderStatuses.Completed, 7);
        var woCancelled = W(reqs[8], tech2, WorkOrderStatuses.Cancelled, 3.5, "Thiếu linh kiện phù hợp, cần đặt hàng.");
        var woAssigned2 = W(reqs[1], tech3, WorkOrderStatuses.Assigned, 0.6);
        var woInProgress2 = W(reqs[5], tech1, WorkOrderStatuses.InProgress, 0.4);
        var woAssigned3 = W(reqs[9], tech2, WorkOrderStatuses.Assigned, 0.3);
        var woAssigned4 = W(reqs[10], tech1, WorkOrderStatuses.Assigned, 0.85);
        var woAssigned5 = W(reqs[11], tech2, WorkOrderStatuses.Assigned, 0.7);
        var woInProgress3 = W(reqs[12], tech3, WorkOrderStatuses.InProgress, 1.4);
        var woAssigned6 = W(reqs[13], tech1, WorkOrderStatuses.Assigned, 0.5);
        var woInProgress4 = W(reqs[14], tech2, WorkOrderStatuses.InProgress, 0.35);
        var woAssigned7 = W(reqs[15], tech3, WorkOrderStatuses.Assigned, 1.05);
        var woCompleted3 = W(reqs[16], tech1, WorkOrderStatuses.Completed, 5.2);
        var woInProgress5 = W(reqs[17], tech3, WorkOrderStatuses.InProgress, 1.9);
        var woAssigned8 = W(reqs[18], tech2, WorkOrderStatuses.Assigned, 0.28);
        var woCompleted4 = W(reqs[19], tech3, WorkOrderStatuses.Completed, 8.2);
        var woCompleted5 = W(reqs[20], tech1, WorkOrderStatuses.Completed, 4.8);
        var woCompleted6 = W(reqs[21], tech2, WorkOrderStatuses.Completed, 9.1);
        var woCancelled2 = W(reqs[22], tech3, WorkOrderStatuses.Cancelled, 2.8, "Thiết bị hết bảo hành, chờ phê duyệt mua mới.");
        var woCompleted7 = W(reqs[23], tech1, WorkOrderStatuses.Completed, 6.4);
        var woInProgress6 = W(reqs[24], tech2, WorkOrderStatuses.InProgress, 1.35);
        var woAssigned9 = W(reqs[25], tech3, WorkOrderStatuses.Assigned, 0.18);
        var woCancelled3 = W(reqs[27], tech1, WorkOrderStatuses.Cancelled, 1.15, "Trùng lịch bảo trì định kỳ tuần sau, tạm hủy WO này.");
        // reqs[2], [7], [26], [28], [29] remain Submitted without WO — queue “Chờ phân công”

        var allWos = new[]
        {
            woAssigned, woInProgress, woCompleted1, woCompleted2,
            woCancelled, woAssigned2, woInProgress2, woAssigned3,
            woAssigned4, woAssigned5, woInProgress3, woAssigned6,
            woInProgress4, woAssigned7, woCompleted3, woInProgress5,
            woAssigned8, woCompleted4, woCompleted5, woCompleted6,
            woCancelled2, woCompleted7, woInProgress6, woAssigned9,
            woCancelled3
        };

        db.WorkOrdersSet.AddRange(allWos);

        foreach (var wo in allWos)
        {
            var req = reqs.First(r => r.RequestId == wo.RequestId);
            req.Status = wo.Status switch
            {
                WorkOrderStatuses.Assigned => RequestStatuses.Pending,
                WorkOrderStatuses.InProgress => RequestStatuses.InProgress,
                WorkOrderStatuses.Completed => RequestStatuses.Closed,
                WorkOrderStatuses.Cancelled => RequestStatuses.Rejected,
                _ => req.Status
            };
        }

        await db.SaveChangesAsync();

        db.MaintenanceHistoriesSet.AddRange(
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted1.OrderId,
                AssetId = Find("Điều hòa LG Hội trường").AssetId,
                Result = "Vệ sinh dàn lạnh, nạp gas bổ sung, kiểm tra êm trở lại.",
                CompletedAt = now.AddDays(-4)
            },
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted2.OrderId,
                AssetId = Find("Đèn LED cầu thang B").AssetId,
                Result = "Thay bóng LED mới, kiểm tra tiếp địa cầu thang.",
                CompletedAt = now.AddDays(-6.5)
            },
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted3.OrderId,
                AssetId = Find("Đèn LED bãi xe").AssetId,
                Result = "Thay 4 bóng LED chống nước, kiểm tra tủ điện bãi xe.",
                CompletedAt = now.AddDays(-5)
            },
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted4.OrderId,
                AssetId = Find("Máy chiếu Epson B201").AssetId,
                Result = "Cập nhật firmware máy chiếu, thay cáp HDMI.",
                CompletedAt = now.AddDays(-8)
            },
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted5.OrderId,
                AssetId = Find("Access Point tầng 1").AssetId,
                Result = "Điều chỉnh anten và kênh Wi-Fi tầng 1.",
                CompletedAt = now.AddDays(-4.5)
            },
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted6.OrderId,
                AssetId = Find("Đèn LED sảnh chính").AssetId,
                Result = "Thay driver LED sảnh, cân sáng hai phía.",
                CompletedAt = now.AddDays(-9)
            },
            new MaintenanceHistory
            {
                WorkOrderId = woCompleted7.OrderId,
                AssetId = Find("Điều hòa Mitsubishi D01").AssetId,
                Result = "Vệ sinh lọc gió, kiểm tra gas điều hòa phòng thi.",
                CompletedAt = now.AddDays(-6)
            });
        await db.SaveChangesAsync();
    }

    private static async Task EnsureTelemetryAsync(AppDbContext db)
    {
        var now = DateTime.UtcNow;
        var assets = await db.AssetsSet.ToListAsync();
        if (assets.Count == 0) return;

        var mappedIds = await db.IotMappingsSet.Select(m => m.AssetId).ToListAsync();
        var predictedIds = await db.AiPredictionsSet.Select(p => p.AssetId).Distinct().ToListAsync();
        var leaveUnmapped = DemoUnmappedAssets.Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var asset in assets)
        {
            if (!mappedIds.Contains(asset.AssetId) && !leaveUnmapped.Contains(asset.Name))
            {
                var readings = DemoTelemetry(asset);
                var device = new IotDevice
                {
                    ExternalId = $"SENSOR_{asset.AssetId}",
                    DeviceName = $"Sensor {asset.Name}",
                    DeviceType = asset.Type switch
                    {
                        AssetTypes.AirConditioner => "temperature_sensor",
                        AssetTypes.WiFi => "wifi_analyzer",
                        _ => "multi_sensor"
                    }
                };
                db.IotDevicesSet.Add(device);
                await db.SaveChangesAsync();
                db.IotMappingsSet.Add(new IotMapping
                {
                    AssetId = asset.AssetId,
                    DeviceId = device.DeviceId,
                    CreatedAt = now.AddDays(-3)
                });

                IotData? alertSource = null;
                foreach (var (metric, value) in readings)
                {
                    var row = new IotData
                    {
                        DeviceId = device.DeviceId,
                        MetricType = metric,
                        ReadingValue = value,
                        Timestamp = now.AddMinutes(-18)
                    };
                    db.IotDataSet.Add(row);
                    if (metric == MetricTypes.Temperature && value > 40)
                        alertSource = row;
                    if (metric == MetricTypes.PowerStatus && value == 0)
                        alertSource ??= row;
                }

                await db.SaveChangesAsync();

                if (alertSource is not null)
                {
                    var threshold = alertSource.MetricType == MetricTypes.PowerStatus ? 0d : 40d;
                    db.IotAlertsSet.Add(new IotAlert
                    {
                        DataId = alertSource.DataId,
                        AssetId = asset.AssetId,
                        MetricType = alertSource.MetricType,
                        ReadingValue = alertSource.ReadingValue,
                        Threshold = threshold,
                        Severity = AlertSeverities.High,
                        DetectedAt = now.AddMinutes(-17)
                    });
                }
            }

            if (!predictedIds.Contains(asset.AssetId))
            {
                var risk = asset.MaintenanceRisk ?? RiskFromStatus(asset.Status);
                db.AiPredictionsSet.Add(new AiPrediction
                {
                    AssetId = asset.AssetId,
                    RiskLevel = risk,
                    PredictedAt = now.AddHours(-2),
                    BasedOnSampleData = true
                });
                asset.MaintenanceRisk = risk;
            }
        }

        await db.SaveChangesAsync();
    }

    private static string RiskFromStatus(string status) => status switch
    {
        AssetStatuses.OutOfService => RiskLevels.High,
        AssetStatuses.Warning or AssetStatuses.Maintenance => RiskLevels.Medium,
        _ => RiskLevels.Low
    };

    private static (string Metric, double Value)[] DemoTelemetry(Asset asset)
    {
        return asset.Type switch
        {
            AssetTypes.AirConditioner =>
            [
                (MetricTypes.Temperature, asset.Status == AssetStatuses.Warning ? 31.4 : 24.5),
                (MetricTypes.Humidity, 68.0),
                (MetricTypes.PowerStatus, 1.0)
            ],
            AssetTypes.Projector =>
            [
                (MetricTypes.Temperature, 24.1),
                (MetricTypes.Humidity, 51.0),
                (MetricTypes.PowerStatus, 1.0)
            ],
            AssetTypes.WiFi =>
            [
                (MetricTypes.Temperature, 27.2),
                (MetricTypes.Humidity, 55.0),
                (MetricTypes.PowerStatus, 1.0)
            ],
            AssetTypes.Light =>
            [
                (MetricTypes.Temperature, asset.Status == AssetStatuses.Maintenance ? 42.8 : 28.0),
                (MetricTypes.Humidity, 60.0),
                (MetricTypes.PowerStatus, asset.Status == AssetStatuses.OutOfService ? 0.0 : 1.0)
            ],
            _ =>
            [
                (MetricTypes.Temperature, 38.5),
                (MetricTypes.Humidity, 52.0),
                (MetricTypes.PowerStatus, asset.Status == AssetStatuses.OutOfService ? 0.0 : 1.0)
            ]
        };
    }
}
