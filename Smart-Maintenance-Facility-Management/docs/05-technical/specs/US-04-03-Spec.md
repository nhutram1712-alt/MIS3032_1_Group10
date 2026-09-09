# Story Spec - US-04-03 — Theo dõi Work Order

**Story ID:** US-04-03
**Requirement IDs:** REQ-16, BR-17, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện theo dõi trạng thái Work Order để giám sát tiến độ thực hiện công việc bảo trì.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Có ít nhất một Work Order đã được tạo trong bảng `WORK_ORDERS`.

## HAPPY PATH

### H1 — Xem danh sách Work Order
1. Facility Manager truy cập trang Quản lý Work Order.
2. Client gửi `GET /api/work-orders` kèm JWT Token.
3. Server xác thực Role `FacilityManager`, trả về danh sách toàn bộ Work Order.
4. Facility Manager thấy danh sách WO với các Status: `Assigned`, `In Progress`, `Completed`, `Cancelled`.

### H2 — Xem chi tiết một Work Order
1. Facility Manager click vào một Work Order cụ thể.
2. Client gửi `GET /api/work-orders/{id}`.
3. Server trả về `200 OK` với đầy đủ thông tin: `OrderID`, `RequestID`, `TechnicianID`, `AssetID`, `Status`, `RejectionReason`, `CreatedAt`.

### H3 — Hủy Work Order (Cancelled)
1. Facility Manager quyết định hủy Work Order (ví dụ: Technician từ chối, cần tái phân công hoặc không còn cần thiết).
2. Client gửi `PATCH /api/work-orders/{id}/status` với `{ "status": "Cancelled" }`.
3. Server cập nhật `Status` = `Cancelled`, trả về `200 OK`.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `status` không hợp lệ (không thuộc `Assigned`, `In Progress`, `Completed`, `Cancelled`); hoặc cố gắng chuyển từ `Completed` sang trạng thái khác (WO đã hoàn thành không thể thay đổi).
- **Lỗi 404 Not Found:** Xảy ra khi `WorkOrderID` không tồn tại.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `WORK_ORDERS` — đọc toàn bộ WO (FM thấy tất cả); đọc theo `OrderID` cho chi tiết.
- **Read:** Bảng `ASSETS` — đọc thông tin Asset liên quan để hiển thị.
- **Read:** Bảng `USERS` — đọc thông tin Technician được phân công (`TechnicianID`).
- **Read:** Bảng `MAINTENANCE_REQUESTS` — đọc thông tin Request gốc liên quan.
- **Write:** Bảng `WORK_ORDERS` — Update `Status` = `Cancelled` khi FM hủy WO.

## API CONTRACT

### Xem danh sách Work Order
- **Method:** `GET`
- **Endpoint:** `/api/work-orders`
- **Auth Level:** FacilityManager (JWT bắt buộc)
- **Request Payload:** None
- **Response:** `200 OK`:
```json
[
  {
    "orderId": 3,
    "requestId": 5,
    "technicianId": 7,
    "technicianName": "tech_tran",
    "assetId": 1,
    "assetName": "WiFi Router P201",
    "status": "In Progress",
    "createdAt": "2026-09-02T09:00:00"
  }
]
```

### Xem chi tiết Work Order
- **Method:** `GET`
- **Endpoint:** `/api/work-orders/{id}`
- **Auth Level:** FacilityManager, Technician
- **Response:** `200 OK` | `404 Not Found`

### Hủy Work Order
- **Method:** `PATCH`
- **Endpoint:** `/api/work-orders/{id}`
- **Auth Level:** FacilityManager
- **Request Payload:**
```json
{
  "status": "Cancelled"
}
```
- **Response:** `200 OK` | `400 Bad Request` | `404 Not Found`

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager`.
- FM xem được **toàn bộ** Work Order của mọi Technician.
- FM có thể hủy (`Cancelled`) Work Order, nhưng không thể tự chuyển sang `Completed` (chỉ Technician hoàn thành WO).

## VALIDATION/BUSINESS RULES
- **BR-17 (Work Order Status lifecycle):** Thứ tự chuyển trạng thái hợp lệ:
  - `Assigned` → `In Progress` (Technician bắt đầu thực hiện)
  - `In Progress` → `Completed` (Technician hoàn thành — chỉ Technician mới làm được)
  - `Assigned` → `Cancelled` (FM hủy)
  - `In Progress` → `Cancelled` (FM hủy)
  - `Completed` → *không thể chuyển sang trạng thái nào* (trạng thái cuối)
  - `Cancelled` → *không thể chuyển sang trạng thái nào* (trạng thái cuối)
- **NFR-03 (Tính nhất quán dữ liệu):** Khi WO bị `Cancelled`, Maintenance Request liên quan cần được FM xem xét lại (có thể tạo WO mới hoặc `Reject` Request).
- **NFR-04 (Lưu timestamp):** `CreatedAt` đã có trong bảng. Khuyến nghị log thời điểm mỗi lần thay đổi Status.
- **REQ-16:** Facility Manager theo dõi WO với đầy đủ 4 trạng thái theo BR-17.

## OBSERVABILITY/LOGGING
- Ghi log action `WorkOrderStatusViewed`, `WorkOrderCancelled` bằng Serilog, bao gồm:
  - `UserID` (Facility Manager)
  - `OrderID`
  - `OldStatus` → `NewStatus` (khi có thay đổi)
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - GET danh sách WO với Role `FacilityManager` → thấy tất cả WO.
  - GET danh sách WO với Role `Requester` → 403.
  - PATCH `Cancelled` cho WO đang `Assigned` → 200 OK.
  - PATCH `Cancelled` cho WO đang `In Progress` → 200 OK.
  - PATCH bất kỳ từ `Completed` → 400 (trạng thái cuối).
  - PATCH với `status` không hợp lệ → 400.
  - PATCH với `WorkOrderID` không tồn tại → 404.
- **Integration Test:**
  - FM tạo WO và phân công Technician → FM GET danh sách thấy WO.
  - Technician cập nhật WO → FM GET lại thấy Status đã đổi.
  - FM PATCH `Cancelled` → WO không còn hiển thị ở trạng thái active.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- BR-17 lifecycle được enforce đúng — đặc biệt `Completed` là trạng thái cuối không thể đổi ngược.
- FM xem được toàn bộ Work Order.
- Commit có chứa mã Story ID `US-04-03`.
