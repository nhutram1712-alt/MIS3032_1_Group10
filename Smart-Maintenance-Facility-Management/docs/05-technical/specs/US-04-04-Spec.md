# Story Spec - US-04-04 — Xem Work Order được phân công

**Story ID:** US-04-04
**Requirement IDs:** REQ-17, REQ-18, BR-07, BR-08, NFR-01, NFR-07
**Design link:** TODO
**Goal:** Cho phép **Technician** thực hiện xem các Work Order được phân công cho mình và thông tin Asset liên quan để biết công việc cần thực hiện và Asset cần bảo trì.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Technician`** và có JWT Token hợp lệ.
- Có ít nhất một Work Order trong bảng `WORK_ORDERS` với `TechnicianID` khớp với `UserID` của Technician hiện tại.

## HAPPY PATH

### H1 — Xem danh sách Work Order được phân công
1. Technician truy cập trang "Công việc của tôi".
2. Client gửi `GET /api/work-orders` kèm JWT Token.
3. Server xác thực JWT Token, xác nhận Role là `Technician`.
4. Server truy vấn bảng `WORK_ORDERS` lọc theo `TechnicianID = UserID` của Technician.
5. Server trả về `200 OK` với danh sách Work Order được phân công cho Technician.
6. Technician thấy danh sách WO của mình với Status: `Assigned`, `In Progress`, `Completed`, `Cancelled`.

### H2 — Xem chi tiết Work Order và thông tin Asset
1. Technician click vào một Work Order cụ thể.
2. Client gửi `GET /api/work-orders/{id}`.
3. Server xác minh Work Order có `TechnicianID` khớp với Technician đang đăng nhập.
4. Server trả về `200 OK` với đầy đủ thông tin WO và thông tin Asset liên quan (join với bảng `ASSETS`).
5. Technician xem được: mô tả công việc, thông tin Asset (`Name`, `Type`, `Location`, `Status`), và lịch sử bảo trì liên quan (từ `MAINTENANCE_HISTORY`).

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `Technician`.
- **Lỗi 404 Not Found:** Xảy ra khi `WorkOrderID` không tồn tại **hoặc** Work Order tồn tại nhưng không được phân công cho Technician hiện tại (tránh information leak — trả về 404 thay vì 403).
- **Trường hợp danh sách trống:** Nếu Technician chưa được phân công WO nào, server trả về `200 OK` với array rỗng `[]`.

## DATA READ/WRITE
- **Read:** Bảng `WORK_ORDERS` — đọc các WO lọc theo `TechnicianID = UserID`; đọc theo `OrderID` cho chi tiết.
- **Read:** Bảng `ASSETS` — đọc `AssetID`, `Name`, `Type`, `Location`, `Status` của Asset liên quan đến WO (join qua `AssetID`).
- **Read:** Bảng `MAINTENANCE_REQUESTS` — đọc mô tả Request gốc để Technician hiểu ngữ cảnh công việc.
- **Read:** Bảng `MAINTENANCE_HISTORY` — đọc lịch sử bảo trì trước đây của Asset (nếu có) để Technician tham khảo.
- **Write:** Không có thao tác ghi trong Story này (chỉ xem).

## API CONTRACT

### Xem danh sách Work Order của Technician
- **Method:** `GET`
- **Endpoint:** `/api/work-orders`
- **Auth Level:** FacilityManager, Technician (JWT bắt buộc)
- **Request Payload:** None
- **Response:** `200 OK`:
```json
[
  {
    "orderId": 3,
    "requestId": 5,
    "assetId": 1,
    "assetName": "WiFi Router P201",
    "assetType": "Wi-Fi",
    "assetLocation": "P201",
    "status": "Assigned",
    "createdAt": "2026-09-02T09:00:00"
  }
]
```
  - `401 Unauthorized`: Token không hợp lệ
  - `403 Forbidden`: Không đủ quyền

### Xem chi tiết Work Order kèm thông tin Asset
- **Method:** `GET`
- **Endpoint:** `/api/work-orders/{id}`
- **Auth Level:** FacilityManager, Technician
- **Response:** `200 OK`:
```json
{
  "orderId": 3,
  "requestId": 5,
  "requestDescription": "WiFi bị mất kết nối liên tục",
  "asset": {
    "assetId": 1,
    "name": "WiFi Router P201",
    "type": "Wi-Fi",
    "location": "P201",
    "status": "Maintenance"
  },
  "status": "Assigned",
  "createdAt": "2026-09-02T09:00:00",
  "maintenanceHistory": [
    {
      "completedAt": "2026-07-10T11:00:00",
      "result": "Khởi động lại router, kết nối ổn định."
    }
  ]
}
```
  - `404 Not Found`: WorkOrderID không tồn tại hoặc không thuộc Technician

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Technician`.
- Server **chỉ trả về Work Order có `TechnicianID` khớp với UserID** của Technician đang đăng nhập.
- **BR-07:** Technician không được xem WO của Technician khác.
- **NFR-07:** Giao diện Technician chỉ hiển thị thông tin và action phù hợp (xem, cập nhật WO của mình — không có quyền phân công hoặc xem toàn bộ WO).

## VALIDATION/BUSINESS RULES
- **BR-07 (Technician chỉ thao tác WO được phân công cho mình):** Server phải filter nghiêm ngặt theo `TechnicianID`. Không bao giờ trả về WO của Technician khác.
- **BR-08 (WO phải liên kết với một Asset cụ thể):** Response chi tiết WO phải bao gồm đầy đủ thông tin Asset. Nếu `AssetID` bị null, đây là lỗi dữ liệu cần được xử lý.
- **NFR-01 (RBAC):** Technician chỉ được xem WO của mình. FacilityManager xem được tất cả WO.
- **NFR-07 (Giao diện theo Role):** Giao diện Technician không hiển thị nút "Phân công" hay "Hủy WO" — chỉ thấy thông tin và nút "Bắt đầu" / "Hoàn thành" / "Từ chối".
- **REQ-17:** Technician xem danh sách WO được phân công.
- **REQ-18:** Technician xem thông tin Asset liên quan đến WO — bắt buộc phải có trong response chi tiết.

## OBSERVABILITY/LOGGING
- Ghi log action `WorkOrderDetailViewed` bằng Serilog (mức `Debug`), bao gồm:
  - `UserID` của Technician
  - `OrderID` được xem
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Technician A chỉ thấy WO có `TechnicianID = A`, không thấy WO của Technician B.
  - GET `work-orders/{id}` với WO của Technician khác → 404.
  - Token không hợp lệ → 401.
  - Role không phải Technician → 403.
  - Response chi tiết WO bao gồm đầy đủ thông tin Asset (BR-08).
  - Response chi tiết bao gồm Maintenance History của Asset.
- **Integration Test:**
  - FM phân công WO cho Technician A → Technician A GET danh sách thấy WO.
  - Technician B GET danh sách → không thấy WO của Technician A.
  - Technician A GET chi tiết WO → thấy đầy đủ thông tin Asset và History.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Technician chỉ thấy WO được phân công cho mình (BR-07 được enforce).
- Response chi tiết WO bao gồm đầy đủ thông tin Asset (REQ-18, BR-08).
- Commit có chứa mã Story ID `US-04-04`.
