# Story Spec - US-03-02 — Theo dõi Maintenance Request

**Story ID:** US-03-02
**Requirement IDs:** REQ-05, BR-14, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Requester** thực hiện theo dõi trạng thái của Maintenance Request đã tạo để biết tiến độ xử lý yêu cầu của mình.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Requester`** và có JWT Token hợp lệ.
- Requester đã có ít nhất một Maintenance Request đã được tạo trong bảng `MAINTENANCE_REQUESTS` (liên kết qua `RequesterID`).

## HAPPY PATH
1. Requester truy cập trang "Yêu cầu của tôi" hoặc trang danh sách Maintenance Request.
2. Client gửi `GET /api/requests` kèm JWT Token trong Authorization header.
3. Server xác thực JWT Token, xác nhận Role là `Requester`.
4. Server truy vấn bảng `MAINTENANCE_REQUESTS` lọc theo `RequesterID` của người dùng hiện tại.
5. Server trả về `200 OK` với danh sách Maintenance Request kèm `Status` hiện tại của từng request.
6. Requester xem được danh sách yêu cầu của mình với các trạng thái: `Submitted`, `Pending`, `In Progress`, `Resolved`, `Closed`, hoặc `Rejected`.
7. Requester click vào một request cụ thể để xem chi tiết (`GET /api/requests/{id}`).

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `Requester`.
- **Lỗi 404 Not Found:** Xảy ra khi gọi chi tiết `GET /api/requests/{id}` nhưng `RequestID` không tồn tại hoặc không thuộc về Requester hiện tại.
- **Trường hợp danh sách trống:** Nếu Requester chưa tạo request nào, server trả về `200 OK` với array rỗng `[]` — không phải lỗi.

## DATA READ/WRITE
- **Read:** Bảng `MAINTENANCE_REQUESTS` — đọc các trường `RequestID`, `AssetID`, `Description`, `Status`, `CreatedAt` lọc theo `RequesterID`.
- **Read:** Bảng `ASSETS` — đọc `Name`, `Location` của Asset liên quan để hiển thị thông tin ngữ cảnh cho Requester.
- **Write:** Không có thao tác ghi (chỉ đọc để theo dõi).

## API CONTRACT

### Xem danh sách Request của Requester
- **Method:** `GET`
- **Endpoint:** `/api/requests`
- **Auth Level:** Requester, FacilityManager (JWT bắt buộc)
- **Request Payload:** None
- **Response:**
  - `200 OK`:
```json
[
  {
    "requestId": 5,
    "assetId": 1,
    "assetName": "WiFi Router P201",
    "description": "WiFi bị mất kết nối liên tục",
    "status": "In Progress",
    "createdAt": "2026-09-01T08:30:00"
  }
]
```
  - `401 Unauthorized`: Token không hợp lệ
  - `403 Forbidden`: Không đủ quyền

### Xem chi tiết một Request
- **Method:** `GET`
- **Endpoint:** `/api/requests/{id}`
- **Auth Level:** Requester, FacilityManager
- **Response:** `200 OK` với đầy đủ thông tin request | `404 Not Found`

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Requester`.
- Server **chỉ trả về Request của chính Requester đó** — lọc theo `RequesterID = UserID` từ JWT Token. Requester không được xem Request của người khác.

## VALIDATION/BUSINESS RULES
- **BR-14 (Maintenance Request Status lifecycle):** Trạng thái hợp lệ của Maintenance Request gồm: `Submitted`, `Pending`, `In Progress`, `Resolved`, `Closed`, `Rejected`. Hệ thống phải hiển thị đúng các trạng thái này theo lifecycle nghiệp vụ.
- **NFR-03 (Tính nhất quán dữ liệu):** `Status` của Request phải phản ánh đúng trạng thái thực tế dựa trên lifecycle: `Submitted` → `Pending` → `In Progress` → `Resolved` → `Closed` hoặc `Rejected`.
- **NFR-04 (Lưu timestamp):** `CreatedAt` phải được hiển thị để Requester biết thời điểm tạo yêu cầu.
- **REQ-05:** Requester có thể theo dõi tất cả 6 trạng thái lifecycle của Maintenance Request.

## OBSERVABILITY/LOGGING
- Ghi log action `MaintenanceRequestListViewed` bằng Serilog, bao gồm:
  - `UserID` của Requester
  - Số lượng Request trả về
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Requester A chỉ thấy Request của mình, không thấy Request của Requester B.
  - Token không hợp lệ → 401.
  - Role không phải Requester → 403.
  - Gọi `GET /api/requests/{id}` với RequestID của người khác → 404.
  - Request với mọi trạng thái hợp lệ (Submitted, Pending, In Progress, Resolved, Closed, Rejected) đều được trả về đúng.
- **Integration Test:**
  - Requester tạo Request (POST), sau đó GET danh sách → thấy Request vừa tạo với status `Submitted`.
  - Facility Manager cập nhật status → Requester GET lại → thấy status mới.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API lọc đúng Request của Requester, hiển thị đầy đủ 6 trạng thái theo BR-14.
- Commit có chứa mã Story ID `US-03-02`.
