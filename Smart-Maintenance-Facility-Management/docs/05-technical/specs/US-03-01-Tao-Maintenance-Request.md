# Story Spec

Story ID: US-03-01
Requirement IDs: REQ-04, BR-05
Design link: TODO
Goal: Cho phép Requester tạo một Maintenance Request mới để báo cáo sự cố liên quan đến Asset hoặc khu vực.

Preconditions:

- Người dùng đã đăng nhập với Role `Requester`.
- Thiết bị (Asset) đã tồn tại trong hệ thống.

Happy path:

1. Requester chọn chức năng tạo Request.
2. Requester nhập Description (mô tả lỗi) và chọn AssetID từ danh sách.
3. Requester bấm Submit.
4. Hệ thống validate thông tin, lưu vào DB và trả về mã 201 Created.
5. Request mới có trạng thái mặc định là `Submitted`.

Alternate/error paths:

- Lỗi 400 Bad Request: Gửi thiếu AssetID hoặc Description bị trống.
- Lỗi 404 Not Found: AssetID được chọn không tồn tại trong DB.
- Lỗi 403 Forbidden: User không có Role `Requester`.

Data read/write:

- **Write:** Bảng `MAINTENANCE_REQUESTS` (Tạo bản ghi mới với RequesterID lấy từ JWT token, AssetID, Description, Status="Submitted", CreatedAt=GETDATE()).
- **Read:** Bảng `ASSETS` (Để kiểm tra AssetID có tồn tại hay không).

API contract:

- Method: `POST`
- Endpoint: `/api/requests`
- Request Payload: `{ "assetId": 1, "description": "Điều hòa không mát" }`
- Response: `201 Created`

Authorization:

- Kiểm tra JWT Token, bắt buộc Role phải là `Requester`.

Validation/business rules:

- Bắt buộc phải có AssetID (BR-05). Trong MVP, bắt buộc truyền AssetID > 0.
- Description độ dài tối đa 500 ký tự.

Observability/logging:

- Ghi log action tạo Request bằng Serilog (thông tin: RequesterID, AssetID, Timestamp).

Test plan:

- Unit Test: Controller nhận đúng payload, validation chặn payload rỗng.
- Integration Test: Mock DB, tạo 1 Request với AssetID hợp lệ -> Assert response 201 và trạng thái `Submitted`.

Definition of Done:

- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API chạy thành công, insert đúng dữ liệu vào SQL Server.
- Commit có chứa mã Story ID.

