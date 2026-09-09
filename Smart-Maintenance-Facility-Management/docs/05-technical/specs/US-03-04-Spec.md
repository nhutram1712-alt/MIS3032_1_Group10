# Story Spec - US-03-04 — Xác nhận và đóng Maintenance Request

**Story ID:** US-03-04
**Requirement IDs:** REQ-13, BR-18, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện xác nhận kết quả bảo trì trước khi đóng Maintenance Request để đảm bảo Request chỉ được Closed sau khi kết quả đã được kiểm tra và chấp thuận.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Maintenance Request có Status `Resolved` đã tồn tại — tức là Work Order liên quan đã được Technician hoàn thành (Status `Completed`).
- Work Order liên quan đã có kết quả được lưu vào `MAINTENANCE_HISTORY` (US-04-06 đã hoàn thành trước).

## HAPPY PATH
1. Facility Manager truy cập danh sách Maintenance Request.
2. Facility Manager thấy Request có Status `Resolved` (Technician đã hoàn thành Work Order).
3. Facility Manager xem kết quả bảo trì từ `MAINTENANCE_HISTORY` liên quan để kiểm tra.
4. Facility Manager xác nhận kết quả hợp lệ và đóng Request.
5. Client gửi `PATCH /api/requests/{id}/status` với `{ "status": "Closed" }`.
6. Server kiểm tra Request hiện tại có Status `Resolved` (tiền điều kiện để chuyển sang `Closed`).
7. Server kiểm tra Work Order liên quan đã có Status `Completed`.
8. Server cập nhật `Status` = `Closed` trong bảng `MAINTENANCE_REQUESTS`.
9. Server trả về `200 OK`.
10. Requester (khi xem lại) thấy Request của mình đã `Closed`.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request — Vi phạm BR-18:** Xảy ra khi Facility Manager cố gắng chuyển Request sang `Closed` nhưng Work Order liên quan **chưa** có Status `Completed` (Technician chưa hoàn thành bảo trì). Server phải từ chối và trả về thông báo lỗi rõ ràng.
- **Lỗi 400 Bad Request — Transition không hợp lệ:** Xảy ra khi Status hiện tại của Request không phải `Resolved` (ví dụ: cố `Close` một Request đang `In Progress`).
- **Lỗi 404 Not Found:** Xảy ra khi `RequestID` không tồn tại.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `MAINTENANCE_REQUESTS` — đọc `Status` hiện tại của Request trước khi chuyển sang `Closed`.
- **Read:** Bảng `WORK_ORDERS` — đọc `Status` của Work Order liên quan (`RequestID` → `OrderID`) để xác minh Work Order đã `Completed` (enforce BR-18).
- **Read:** Bảng `MAINTENANCE_HISTORY` — đọc kết quả bảo trì để Facility Manager xem và xác nhận.
- **Write:** Bảng `MAINTENANCE_REQUESTS` — Update `Status` = `Closed` theo `RequestID`.

## API CONTRACT

### Đóng Maintenance Request
- **Method:** `PATCH`
- **Endpoint:** `/api/requests/{id}/status`
- **Auth Level:** FacilityManager
- **Request Payload:**
```json
{
  "status": "Closed"
}
```
- **Response:**
  - `200 OK`: Request đã được đóng thành công
  - `400 Bad Request`: Work Order liên quan chưa Completed, hoặc transition không hợp lệ
  - `404 Not Found`: RequestID không tồn tại
  - `403 Forbidden`: Không đủ quyền

**Lưu ý:** Endpoint dùng chung với US-03-03. Logic server phân biệt việc chuyển sang `Closed` yêu cầu validation đặc biệt (kiểm tra WO Completed).

### Xem kết quả bảo trì trước khi đóng
- **Method:** `GET`
- **Endpoint:** `/api/requests/{id}/history`
- **Auth Level:** FacilityManager
- **Response:** `200 OK`:
```json
{
  "requestId": 5,
  "workOrderId": 3,
  "technicianId": 7,
  "result": "Đã thay thế bộ phát WiFi, kết nối ổn định.",
  "completedAt": "2026-09-05T14:00:00"
}
```

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager`.
- Chỉ Facility Manager mới có quyền đóng (Close) Maintenance Request — Requester và Technician không được phép.

## VALIDATION/BUSINESS RULES
- **BR-18 (Điều kiện đóng Request — QUAN TRỌNG NHẤT):** Maintenance Request **chỉ được chuyển sang `Closed`** khi:
  1. Technician đã hoàn thành Work Order liên quan (`WO.Status = Completed`), VÀ
  2. Facility Manager chủ động xác nhận và thực hiện hành động Closed.
  
  Server phải kiểm tra cả hai điều kiện này trước khi cho phép transition. Nếu WO chưa `Completed`, trả về `400 Bad Request` với message rõ ràng: `"Work Order chưa hoàn thành. Không thể đóng Request."`

- **BR-14 (Lifecycle hợp lệ):** Chỉ Request có Status `Resolved` mới được chuyển sang `Closed`. Transition từ bất kỳ Status nào khác (ví dụ `In Progress` → `Closed`) là không hợp lệ và phải bị từ chối.

- **NFR-03 (Tính nhất quán dữ liệu):** Khi Request được `Closed`, trạng thái của toàn bộ lifecycle (Request → Work Order → Maintenance History) phải nhất quán. Không được `Close` Request khi WO vẫn đang `In Progress` hoặc `Assigned`.

- **NFR-04 (Lưu timestamp):** Khuyến nghị ghi lại thời điểm Request được `Closed` vào log để audit trail đầy đủ.

## OBSERVABILITY/LOGGING
- Ghi log action `MaintenanceRequestClosed` bằng Serilog, bao gồm:
  - `UserID` (Facility Manager xác nhận)
  - `RequestID`
  - `WorkOrderID` liên quan đã được verify
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Chuyển Request sang `Closed` khi WO đã `Completed` → 200 OK.
  - Chuyển Request sang `Closed` khi WO vẫn `In Progress` → 400 Bad Request (vi phạm BR-18).
  - Chuyển Request sang `Closed` khi WO vẫn `Assigned` → 400 Bad Request.
  - Chuyển Request sang `Closed` khi Request đang `In Progress` (không phải `Resolved`) → 400 Bad Request.
  - Chuyển sang `Closed` khi RequestID không tồn tại → 404.
  - Thực hiện bởi Role `Requester` → 403.
  - Thực hiện bởi Role `Technician` → 403.
- **Integration Test:**
  - Full lifecycle: Requester tạo Request → FM tiếp nhận → FM tạo WO → Technician hoàn thành WO → Request chuyển `Resolved` → FM xem kết quả → FM đóng Request → Status `Closed`.
  - FM cố gắng đóng Request trước khi Technician hoàn thành WO → 400.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- BR-18 được enforce hoàn toàn: không thể `Close` Request khi WO chưa `Completed`.
- Lifecycle Maintenance Request hoàn chỉnh và nhất quán.
- Commit có chứa mã Story ID `US-03-04`.
