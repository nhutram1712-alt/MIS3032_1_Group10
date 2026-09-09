# Story Spec - US-03-03 — Tiếp nhận và xử lý Maintenance Request

**Story ID:** US-03-03
**Requirement IDs:** REQ-13, BR-05, BR-14, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện tiếp nhận và xử lý Maintenance Request để các vấn đề được báo cáo có thể được giải quyết chính thức.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Có ít nhất một Maintenance Request với Status `Submitted` tồn tại trong bảng `MAINTENANCE_REQUESTS`.

## HAPPY PATH

### H1 — Xem danh sách Maintenance Request
1. Facility Manager truy cập trang Quản lý Maintenance Request.
2. Client gửi `GET /api/requests` kèm JWT Token.
3. Server xác thực Role `FacilityManager`, trả về toàn bộ Maintenance Request (không bị lọc theo Requester).
4. Facility Manager xem danh sách tất cả Request với Status hiện tại.

### H2 — Tiếp nhận Request (chuyển sang Pending)
1. Facility Manager chọn một Request có Status `Submitted`.
2. Facility Manager xác nhận tiếp nhận — chuyển Status thành `Pending`.
3. Client gửi `PATCH /api/requests/{id}/status` với `{ "status": "Pending" }`.
4. Server cập nhật `Status` trong bảng `MAINTENANCE_REQUESTS`.
5. Server trả về `200 OK`.

### H3 — Bắt đầu xử lý Request (chuyển sang In Progress)
1. Facility Manager chuyển Status của Request thành `In Progress` khi bắt đầu xử lý (thường đi kèm với việc tạo Work Order ở US-04-01).
2. Client gửi `PATCH /api/requests/{id}/status` với `{ "status": "In Progress" }`.
3. Server cập nhật `Status`, trả về `200 OK`.

### H4 — Từ chối Request (chuyển sang Rejected)
1. Facility Manager xem xét Request và quyết định từ chối (không hợp lệ hoặc không cần bảo trì).
2. Client gửi `PATCH /api/requests/{id}/status` với `{ "status": "Rejected" }`.
3. Server cập nhật `Status`, trả về `200 OK`.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `status` trong payload không hợp lệ (không thuộc 6 giá trị được phép); hoặc chuyển Status theo thứ tự không hợp lệ trong lifecycle (ví dụ: từ `Closed` sang `Pending`).
- **Lỗi 404 Not Found:** Xảy ra khi `RequestID` không tồn tại trong bảng `MAINTENANCE_REQUESTS`.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `MAINTENANCE_REQUESTS` — đọc toàn bộ Request (Facility Manager thấy tất cả, không bị filter); đọc theo `RequestID` để lấy chi tiết.
- **Read:** Bảng `ASSETS` — đọc thông tin Asset liên quan (`Name`, `Location`, `Type`) để hiển thị ngữ cảnh.
- **Write:** Bảng `MAINTENANCE_REQUESTS` — Update cột `Status` theo `RequestID`.

## API CONTRACT

### Xem danh sách Request (FM thấy tất cả)
- **Method:** `GET`
- **Endpoint:** `/api/requests`
- **Auth Level:** Requester, FacilityManager (JWT bắt buộc)
- **Request Payload:** None
- **Response:** `200 OK`:
```json
[
  {
    "requestId": 5,
    "requesterId": 3,
    "assetId": 1,
    "assetName": "WiFi Router P201",
    "description": "WiFi bị mất kết nối liên tục",
    "status": "Submitted",
    "createdAt": "2026-09-01T08:30:00"
  }
]
```

### Cập nhật Status Request
- **Method:** `PATCH`
- **Endpoint:** `/api/requests/{id}/status`
- **Auth Level:** FacilityManager
- **Request Payload:**
```json
{
  "status": "Pending"
}
```
- **Response:**
  - `200 OK`: Cập nhật thành công
  - `400 Bad Request`: Status không hợp lệ hoặc vi phạm lifecycle
  - `404 Not Found`: RequestID không tồn tại
  - `403 Forbidden`: Không đủ quyền

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager` cho `PATCH /api/requests/{id}/status`.
- Facility Manager được xem **toàn bộ** Request từ mọi Requester (khác với Requester chỉ thấy Request của mình).

## VALIDATION/BUSINESS RULES
- **BR-05 (Request phải xác định Asset hoặc khu vực):** Khi Facility Manager xử lý, nếu Request ban đầu chỉ xác định khu vực (không có `AssetID` cụ thể), cần xác định Asset trước khi chuyển sang tạo Work Order. Hệ thống nên cảnh báo nếu `AssetID` vẫn còn trống.
- **BR-14 (Maintenance Request Status lifecycle):** Thứ tự chuyển trạng thái hợp lệ:
  - `Submitted` → `Pending` (tiếp nhận)
  - `Pending` → `In Progress` (bắt đầu xử lý)
  - `Pending` → `Rejected` (từ chối)
  - `In Progress` → `Resolved` (sau khi WO hoàn thành — xem US-03-04)
  - `Resolved` → `Closed` (sau khi FM xác nhận — xem US-03-04)
  - Server phải validate transition hợp lệ, từ chối transition ngược chiều.
- **NFR-03 (Tính nhất quán dữ liệu):** Khi Request được chuyển sang `In Progress`, phải đảm bảo có hoặc sắp có Work Order liên quan (tính nhất quán nghiệp vụ).
- **NFR-04 (Lưu timestamp):** `CreatedAt` đã có trong bảng. Khuyến nghị log `UpdatedAt` khi Status thay đổi để audit trail.

## OBSERVABILITY/LOGGING
- Ghi log action `MaintenanceRequestStatusUpdated` bằng Serilog, bao gồm:
  - `UserID` (Facility Manager thực hiện)
  - `RequestID`
  - `OldStatus` → `NewStatus`
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Chuyển từ `Submitted` → `Pending` → 200 OK.
  - Chuyển từ `Pending` → `In Progress` → 200 OK.
  - Chuyển từ `Pending` → `Rejected` → 200 OK.
  - Chuyển từ `Closed` → `Pending` (không hợp lệ) → 400.
  - PATCH với `status` không hợp lệ → 400.
  - PATCH với `RequestID` không tồn tại → 404.
  - PATCH bởi Role `Requester` → 403.
- **Integration Test:**
  - Requester tạo Request → FM GET danh sách thấy Request với status `Submitted`.
  - FM PATCH → `Pending` → Requester GET lại thấy status `Pending`.
  - FM PATCH → `In Progress` → Requester xác nhận.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API enforce đúng lifecycle transition theo BR-14.
- Facility Manager xem được toàn bộ Request, Requester chỉ xem Request của mình.
- Commit có chứa mã Story ID `US-03-03`.
