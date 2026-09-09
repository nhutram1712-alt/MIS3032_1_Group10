# Story Spec - US-04-06 — Hoàn thành Work Order và lưu lịch sử bảo trì

**Story ID:** US-04-06
**Requirement IDs:** REQ-21, REQ-22, REQ-23, BR-09, BR-18, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Technician** thực hiện ghi nhận kết quả kiểm tra, sửa chữa hoặc bảo trì và hoàn thành Work Order để kết quả bảo trì được lưu lại vào Maintenance History của Asset và kích hoạt quá trình đóng Request.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Technician`** và có JWT Token hợp lệ.
- Work Order cần hoàn thành đã được phân công cho Technician này với Status `In Progress`.
- `AssetID` liên kết với Work Order đã được xác định (BR-08).

## HAPPY PATH
1. Technician đã thực hiện xong công việc bảo trì thực tế.
2. Technician truy cập chi tiết Work Order (Status `In Progress`).
3. Technician nhập kết quả bảo trì vào form: mô tả công việc đã thực hiện, kết quả kiểm tra.
4. Technician click "Hoàn thành Work Order".
5. Client gửi `PATCH /api/work-orders/{id}` với payload:
```json
{
  "status": "Completed",
  "result": "Đã thay thế bộ thu WiFi, kiểm tra kết nối ổn định trong 30 phút."
}
```
6. Server xác thực JWT Token, Role `Technician`, và `TechnicianID` trong WO khớp với UserID.
7. Server kiểm tra WO có Status `In Progress` (bắt buộc trước khi `Completed`).
8. Server **đồng thời** thực hiện 2 thao tác trong một transaction:
   a. Update `Status` = `Completed` trong bảng `WORK_ORDERS`.
   b. Insert record mới vào bảng `MAINTENANCE_HISTORY` với `OrderID`, `AssetID`, `result`, `completedAt = now()`.
9. Server cập nhật `Status` của Maintenance Request liên quan sang `Resolved` (nếu có `RequestID`).
10. Server trả về `201 Created` hoặc `200 OK`.
11. Facility Manager nhận thấy Maintenance Request chuyển sang `Resolved` và có thể xác nhận đóng (US-03-04).

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `result` (kết quả bảo trì) để trống — BR-09 yêu cầu WO hoàn thành phải có kết quả; hoặc WO chưa ở Status `In Progress` (ví dụ: cố hoàn thành từ `Assigned`); hoặc `status` trong payload không phải `Completed`.
- **Lỗi 403 Forbidden — Vi phạm BR-07:** Technician cố hoàn thành Work Order không được phân công cho mình.
- **Lỗi 404 Not Found:** Xảy ra khi `WorkOrderID` không tồn tại.
- **Lỗi 500 Internal Server Error:** Xảy ra khi transaction ghi vào cả `WORK_ORDERS` và `MAINTENANCE_HISTORY` thất bại — phải rollback toàn bộ để tránh dữ liệu không nhất quán.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `WORK_ORDERS` — đọc theo `OrderID` để kiểm tra `TechnicianID`, `Status`, `AssetID`, `RequestID` trước khi update.
- **Write:** Bảng `WORK_ORDERS` — Update `Status` = `Completed` theo `OrderID` **(trong transaction)**.
- **Write:** Bảng `MAINTENANCE_HISTORY` — Insert record mới **(trong cùng transaction)**:
  - `WorkOrderID` (FK → WORK_ORDERS)
  - `AssetID` (FK → ASSETS)
  - `Result` (kết quả bảo trì do Technician nhập)
  - `CompletedAt` = timestamp hiện tại
- **Write:** Bảng `MAINTENANCE_REQUESTS` — Update `Status` = `Resolved` theo `RequestID` của WO (nếu WO có `RequestID`).

## API CONTRACT
- **Method:** `PATCH`
- **Endpoint:** `/api/work-orders/{id}`
- **Auth Level:** Technician (JWT bắt buộc)
- **Request Payload:**
```json
{
  "status": "Completed",
  "result": "Đã thay thế bộ thu WiFi, kiểm tra kết nối ổn định trong 30 phút. Router hoạt động bình thường."
}
```
- **Response:**
  - `200 OK`: Work Order hoàn thành, Maintenance History đã được lưu
  - `400 Bad Request`: `result` trống, hoặc WO không ở trạng thái `In Progress`, hoặc `status` không phải `Completed`
  - `403 Forbidden`: Technician không phải người được phân công
  - `404 Not Found`: WorkOrderID không tồn tại
  - `500 Internal Server Error`: Transaction thất bại

**Response body (thành công):**
```json
{
  "orderId": 3,
  "status": "Completed",
  "maintenanceHistoryId": 12,
  "assetId": 1,
  "result": "Đã thay thế bộ thu WiFi...",
  "completedAt": "2026-09-05T14:30:00"
}
```

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Technician`.
- **BR-07:** Server phải xác minh `WORK_ORDERS.TechnicianID = JWT.UserID` trước khi cho phép hoàn thành.

## VALIDATION/BUSINESS RULES
- **BR-09 (WO hoàn thành phải có kết quả lưu vào Maintenance History):** Đây là Business Rule cốt lõi nhất của Story này. Không được cập nhật Status = `Completed` mà không đồng thời insert vào `MAINTENANCE_HISTORY`. Hai thao tác này **phải được thực hiện trong một database transaction** — nếu một bước thất bại, phải rollback toàn bộ.
- **BR-18 (Điều kiện để Request được Closed):** Khi WO chuyển sang `Completed`, hệ thống phải tự động chuyển Maintenance Request liên quan sang `Resolved`. Đây là bước tiên quyết để Facility Manager có thể `Close` Request (US-03-04).
- **BR-07 (Technician chỉ hoàn thành WO của mình):** Ownership check bắt buộc trước khi ghi bất kỳ dữ liệu nào.
- **Validation `result`:** Trường `result` là bắt buộc và không được để trống. Đây là nội dung mô tả công việc đã thực hiện được lưu vào `MAINTENANCE_HISTORY`.
- **Validation Status transition:** WO phải có Status `In Progress` trước khi chuyển sang `Completed`. Nếu Status là `Assigned` hoặc `Cancelled` → 400 Bad Request.
- **NFR-03 (Tính nhất quán dữ liệu):** Toàn bộ chuỗi dữ liệu phải nhất quán sau khi hoàn thành: `WORK_ORDERS.Status = Completed` + `MAINTENANCE_HISTORY` có record + `MAINTENANCE_REQUESTS.Status = Resolved`.
- **NFR-04 (Lưu timestamp):** `CompletedAt` trong `MAINTENANCE_HISTORY` phải là server-side timestamp (không tin tưởng client-side timestamp).
- **REQ-21:** Technician ghi nhận kết quả kiểm tra, sửa chữa, bảo trì.
- **REQ-22:** Technician hoàn thành Work Order sau khi thực hiện và ghi nhận kết quả.
- **REQ-23:** Hệ thống lưu kết quả vào Maintenance History của Asset.

## OBSERVABILITY/LOGGING
- Ghi log action `WorkOrderCompleted` và `MaintenanceHistoryCreated` bằng Serilog, bao gồm:
  - `UserID` (Technician thực hiện)
  - `OrderID`
  - `AssetID`
  - `MaintenanceHistoryID` vừa được tạo
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Hoàn thành WO hợp lệ (Status `In Progress`, `result` có nội dung) → 200 OK, `WORK_ORDERS.Status = Completed`, record `MAINTENANCE_HISTORY` được insert.
  - Hoàn thành WO với `result` trống → 400 (vi phạm BR-09).
  - Hoàn thành WO có Status `Assigned` (chưa `In Progress`) → 400.
  - Hoàn thành WO của Technician khác → 403 (vi phạm BR-07).
  - WorkOrderID không tồn tại → 404.
  - Transaction: nếu insert `MAINTENANCE_HISTORY` thất bại → `WORK_ORDERS` không được update (rollback).
  - Sau khi WO `Completed`, `MAINTENANCE_REQUESTS.Status` tự động chuyển sang `Resolved`.
- **Integration Test:**
  - Full lifecycle: FM tạo WO → FM phân công → Technician `In Progress` → Technician `Completed` với result → GET `MAINTENANCE_HISTORY` thấy record → Maintenance Request chuyển `Resolved` → FM xác nhận Close.
  - Technician thứ hai thử hoàn thành WO không phải của mình → 403.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- **BR-09:** Hoàn thành WO luôn kéo theo insert `MAINTENANCE_HISTORY` trong cùng một transaction.
- **BR-18:** Maintenance Request tự động chuyển sang `Resolved` khi WO `Completed`.
- `CompletedAt` là server-side timestamp, không tin client.
- Commit có chứa mã Story ID `US-04-06`.
