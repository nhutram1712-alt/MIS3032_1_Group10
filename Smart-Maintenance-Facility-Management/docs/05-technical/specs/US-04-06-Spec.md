
# Story Spec

**Story ID:** US-04-06

**Requirement IDs:** REQ-21, REQ-22, REQ-23, BR-09, BR-18

**Design link:** TODO

**Goal:** Cho phép Technician ghi nhận kết quả kiểm tra/sửa chữa/bảo trì và hoàn thành Work Order được phân công, đồng thời hệ thống tự động lưu kết quả vào Maintenance History của Asset.

### Preconditions

- Người dùng đã đăng nhập với Role `Technician`.
- Work Order đang ở trạng thái `In Progress` và được phân công cho chính Technician đang thao tác (BR-07).
- Technician đã thực hiện kiểm tra/sửa chữa thực tế trước khi ghi nhận kết quả.

### Happy path

1. Technician mở Work Order được phân công cho mình.
2. Technician nhập kết quả kiểm tra, sửa chữa hoặc bảo trì (ghi chú, nguyên nhân, hành động đã thực hiện).
3. Technician bấm Hoàn thành Work Order.
4. Hệ thống validate: kết quả đã được nhập đầy đủ.
5. Hệ thống cập nhật `WORK_ORDERS.Status = "Completed"`, lưu timestamp.
6. Hệ thống tự động tạo bản ghi mới trong `MAINTENANCE_HISTORY`, liên kết với AssetID và WorkOrderID tương ứng.
7. Hệ thống trả về 200 OK.

### Alternate/error paths

- Lỗi 400 Bad Request: Technician cố hoàn thành Work Order nhưng chưa nhập kết quả (vi phạm BR-09).
- Lỗi 403 Forbidden: Work Order không được phân công cho Technician hiện tại (vi phạm BR-07).
- Lỗi 409 Conflict: Work Order không ở trạng thái `In Progress` (ví dụ đã `Completed` hoặc `Cancelled`) — không cho phép hoàn thành lại.
- Lỗi 400 Bad Request: Payload thiếu `status` hoặc giá trị `status` không hợp lệ.

### Data read/write

- **Read:** Bảng `WORK_ORDERS` (kiểm tra Status hiện tại, TechnicianID có khớp User đang thao tác không).
- **Write:** Bảng `WORK_ORDERS` (Cập nhật Status="Completed"); Bảng `MAINTENANCE_HISTORY` (Tạo bản ghi mới: AssetID, WorkOrderID, kết quả, thời điểm hoàn thành).
- **Gap cần lưu ý:** `data-requirements.md` hiện chưa liệt kê đầy đủ cột của `MAINTENANCE_HISTORY`, và `WORK_ORDERS` mới chỉ có cột `RejectionReason` dùng cho việc từ chối, chưa có cột riêng lưu nội dung kết quả bảo trì thành công (ví dụ `ResultNotes`). Cần BA/DBA bổ sung schema này trước khi implement.

### API contract

- Method: `PATCH`
- Endpoint: `/api/work-orders/{id}`
- Request Payload: `{ "status": "Completed", "rejectionReason": "" }`
- Response: `200 OK`
- **Gap cần lưu ý:** API Contract hiện tại chỉ có field tự do là `rejectionReason` dùng chung cho cả path "từ chối" và path "hoàn thành". Chưa có field riêng để Technician nhập nội dung kết quả bảo trì (ví dụ `resolutionNotes`). Đề xuất bổ sung field này vào payload trước khi hiện thực hoá Story.

### Authorization

- Kiểm tra JWT Token, bắt buộc Role phải là `Technician`, và `WorkOrder.TechnicianID` phải trùng với UserID trong token.

### Validation/business rules

- Work Order hoàn thành phải có kết quả được ghi nhận trước đó, nếu không hệ thống từ chối (BR-09).
- Chỉ Technician được phân công mới được cập nhật Work Order (BR-07).
- Work Order sử dụng lifecycle `Assigned → In Progress → Completed/Cancelled` (BR-17); không cho phép chuyển trực tiếp từ `Assigned` sang `Completed` nếu chưa qua `In Progress`.
- Kết quả hoàn thành phải được lưu vào Maintenance History (BR-09).
- Việc hoàn thành Work Order không tự động đóng Maintenance Request — Request chỉ `Closed` sau khi Facility Manager xác nhận kết quả riêng (BR-18, xem US-03-04).

### Observability/logging

- Ghi log action hoàn thành Work Order bằng Serilog (TechnicianID, WorkOrderID, AssetID, Timestamp, độ dài nội dung kết quả).
- Ghi log riêng khi ghi vào Maintenance History để phục vụ truy vết dữ liệu nguồn cho AI Prediction (REQ-29).

### Test plan

- Unit Test: Validation chặn hoàn thành khi thiếu kết quả; chặn Technician không được phân công.
- Integration Test: Mock DB, hoàn thành Work Order hợp lệ → assert Status = `Completed` và có bản ghi mới trong `MAINTENANCE_HISTORY`.
- Integration Test: Hoàn thành Work Order đã ở trạng thái `Completed`/`Cancelled` → assert bị từ chối.
- Integration Test: Technician khác cố cập nhật Work Order không thuộc mình → assert 403.

### Definition of Done

- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API chạy thành công, cập nhật đúng Work Order và tạo đúng bản ghi Maintenance History trong SQL Server.
- Schema `MAINTENANCE_HISTORY` và field kết quả trong payload đã được BA/DBA xác nhận trước khi release.
- Commit có chứa mã Story ID.
