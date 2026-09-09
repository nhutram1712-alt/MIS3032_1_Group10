# Story Spec - US-02-01 — Thêm Asset

**Story ID:** US-02-01  
**Requirement IDs:** REQ-06, BR-01, BR-02, BR-03, BR-04  
**Design link:** TODO  
**Goal:** Cho phép Facility Manager thêm một thiết bị (Asset) mới vào hệ thống với các thông tin cơ bản để quản lý.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role `FacilityManager`.
- Mã tài sản (nếu nhập tay) chưa từng tồn tại trong hệ thống.

## HAPPY PATH
1. Facility Manager chọn chức năng thêm Asset mới.
2. Facility Manager nhập các thông tin: Name, Type, Location, và Status.
3. Facility Manager bấm nút "Thêm Asset".
4. Hệ thống validate thông tin, xác nhận Asset Type hợp lệ.
5. Hệ thống lưu bản ghi vào cơ sở dữ liệu và trả về mã 201 Created kèm dữ liệu Asset vừa tạo.

## ALTERNATE/ERROR PATHS
- Lỗi 400 Bad Request: Thiếu thông tin bắt buộc (Name, Type, Location, Status bị trống).
- Lỗi 400 Bad Request: Asset Type không hợp lệ (vi phạm BR-03, không nằm trong 5 loại quy định).
- Lỗi 409 Conflict: Asset ID bị trùng (nếu hệ thống cho phép user tự nhập ID thay vì tự sinh).
- Lỗi 403 Forbidden: Token hợp lệ nhưng User không có Role `FacilityManager`.

## DATA READ/WRITE
- **Read:** Bảng `ASSETS` (chỉ dùng để check trùng lặp Asset ID nếu cần).
- **Write:** Bảng `ASSETS` (Tạo bản ghi mới với Name, Type, Location, Status="Operational" hoặc trạng thái được chọn).

## API CONTRACT
- Method: `POST`
- Endpoint: `/api/assets`
- Request Payload: `{ "name": "Điều hòa Panasonic", "type": "Air Conditioner", "location": "Phòng A301", "status": "Operational" }`
- Response: `201 Created` kèm theo object vừa được insert (chứa cả AssetID tự sinh).

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager`.

## VALIDATION/BUSINESS RULES
- Bắt buộc phải có Location/Room (BR-04).
- Asset Type bắt buộc phải có (BR-02) và CHỈ ĐƯỢC PHÉP nằm trong 5 loại sau: `Wi-Fi`, `Air Conditioner`, `Projector`, `Light`, `Fan` (BR-03). Backend phải có check constraint hoặc Enum validation cho rule này.
- Mỗi Asset sinh ra phải có một Asset ID duy nhất (BR-01 - thường set auto-increment hoặc chuỗi GUID ở tầng DB).

## OBSERVABILITY/LOGGING
- Ghi log hành động tạo Asset mới (thông tin: FacilityManagerID thao tác, AssetID vừa tạo, Timestamp).

## TEST PLAN
- Unit Test: Gửi payload với Asset Type = "Tivi" -> Assert hệ thống trả về 400 Bad Request (Test BR-03).
- Unit Test: Gửi payload thiếu Location -> Assert hệ thống trả về 400 Bad Request (Test BR-04).
- Integration Test: Gửi payload hợp lệ đầy đủ thông tin -> Assert response 201 và kiểm tra DB có tồn tại đúng dòng dữ liệu vừa tạo.
- Integration Test: Call API bằng Token của `Requester` -> Assert response 403 Forbidden.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API validate chặt chẽ Asset Type, trả lỗi rõ ràng nếu vi phạm.
- Data lưu thành công xuống SQL Server.
- Có log thao tác để truy vết.
- Commit chứa mã Story ID US-02-01.
