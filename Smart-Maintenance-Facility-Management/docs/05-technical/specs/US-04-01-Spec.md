# Story Spec - US-04-01 — Tạo Work Order

**Story ID:** US-04-01

**Requirement IDs:** REQ-14, BR-05, BR-06, BR-08

**Design link:** TODO

**Goal:** Cho phép Facility Manager tạo một Work Order từ Maintenance Request đã tiếp nhận (hoặc từ nhu cầu bảo trì được xác định), đồng thời phân công Work Order đó cho một Technician cụ thể để chính thức bắt đầu quy trình xử lý.

### Preconditions

- Người dùng đã đăng nhập với Role `FacilityManager`.
- Maintenance Request liên quan đã tồn tại và chưa có Work Order nào được tạo (BR-06); hoặc nhu cầu bảo trì được xác định trực tiếp bởi Facility Manager (không qua Request, ví dụ theo AI Prediction).
- AssetID liên quan đã được xác định rõ ràng (BR-05) — nếu Request ban đầu chỉ ghi nhận khu vực, Facility Manager phải xác định Asset cụ thể trước khi tạo Work Order.
- TechnicianID được chọn phải tồn tại và có Role `Technician`.

### Happy path

1. Facility Manager mở một Maintenance Request đang ở trạng thái đã tiếp nhận (hoặc chọn nhu cầu bảo trì đã xác định).
2. Hệ thống kiểm tra Request chưa có Work Order liên kết.
3. Facility Manager xác nhận AssetID liên quan.
4. Facility Manager chọn Technician để phân công.
5. Facility Manager bấm Tạo Work Order.
6. Hệ thống validate dữ liệu, tạo bản ghi mới trong `WORK_ORDERS` với Status mặc định `Assigned`, lưu timestamp (CreatedAt), và trả về mã 201 Created.
7. Work Order được liên kết với Request, Asset và Technician tương ứng.

### Alternate/error paths

- Lỗi 400 Bad Request: Thiếu `requestId`, `technicianId` hoặc `assetId` trong payload.
- Lỗi 409 Conflict: Maintenance Request đã có Work Order (vi phạm BR-06) — hệ thống từ chối tạo thêm.
- Lỗi 422 Unprocessable Entity: Request chỉ xác định khu vực, chưa xác định Asset cụ thể (vi phạm BR-05) — hệ thống yêu cầu xác định Asset trước.
- Lỗi 404 Not Found: `assetId` hoặc `requestId` không tồn tại.
- Lỗi 400 Bad Request: `technicianId` không tồn tại hoặc không có Role `Technician`.
- Lỗi 403 Forbidden: User không có Role `FacilityManager`.

### Data read/write

- **Read:** Bảng `MAINTENANCE_REQUESTS` (kiểm tra Request tồn tại, Status, đã có WO hay chưa); Bảng `ASSETS` (kiểm tra AssetID tồn tại); Bảng `USERS` (kiểm tra TechnicianID tồn tại và Role = Technician).
- **Write:** Bảng `WORK_ORDERS` (Tạo bản ghi mới: RequestID, TechnicianID, AssetID, Status="Assigned", CreatedAt=GETDATE()).

### API contract

- Method: `POST`
- Endpoint: `/api/work-orders`
- Request Payload: `{ "requestId": 1, "technicianId": 2, "assetId": 1 }`
- Response: `201 Created`
- Lưu ý: Theo `api-contract.md`, việc tạo Work Order (REQ-14) và phân công Technician (REQ-15) được gộp chung trong một lệnh gọi API duy nhất cho MVP; các business rule riêng cho bước phân công (BR-07) được kiểm thử chi tiết hơn ở US-04-02.

### Authorization

- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager`.

### Validation/business rules

- Một Maintenance Request chỉ được tạo tối đa một Work Order (BR-06).
- Work Order phải liên kết với một Asset cụ thể, không được để trống AssetID (BR-08).
- Nếu Request chỉ xác định khu vực, phải xác định Asset trước khi tạo Work Order (BR-05).
- TechnicianID phải thuộc Role `Technician` hợp lệ.

### Observability/logging

- Ghi log action tạo Work Order bằng Serilog (thông tin: FacilityManagerID, RequestID, AssetID, TechnicianID, Timestamp).
- Ghi log riêng khi hệ thống từ chối tạo do vi phạm BR-06 hoặc BR-05, phục vụ audit sau này.

### Test plan

- Unit Test: Validation chặn payload thiếu trường bắt buộc; validate TechnicianID phải có Role Technician.
- Integration Test: Mock DB, tạo Work Order từ Request hợp lệ → assert response 201, Status = `Assigned`.
- Integration Test: Tạo Work Order lần 2 từ cùng một Request đã có WO → assert response lỗi phù hợp (409/tương đương).
- Integration Test: Tạo Work Order khi Request chưa xác định Asset → assert bị từ chối.

### Definition of Done

- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API chạy thành công, insert đúng dữ liệu vào SQL Server, đúng ràng buộc BR-05/BR-06/BR-08.
- Commit có chứa mã Story ID.
