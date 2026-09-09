# Story Spec - US-02-04 — Xem trạng thái Asset

**Story ID:** US-02-04
**Requirement IDs:** REQ-03, BR-04, NFR-01, NFR-07
**Design link:** TODO
**Goal:** Cho phép **Requester** thực hiện xem trạng thái hiện tại của Asset mà mình được phép truy cập để biết Asset đang hoạt động bình thường hay đang gặp vấn đề cần báo cáo.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Requester`** và có JWT Token hợp lệ.
- Hệ thống đã có dữ liệu Asset với `Status` và `Location` trong bảng `ASSETS`.
- Requester đã được gán `Location`/phạm vi truy cập hợp lệ.

## HAPPY PATH
1. Requester đã đăng nhập, truy cập trang danh sách Asset hoặc trang chi tiết một Asset cụ thể.
2. Client gửi `GET /api/assets` (hoặc `GET /api/assets/{id}`) kèm JWT Token.
3. Server xác thực JWT Token, xác nhận Role là `Requester`.
4. Server lọc Asset theo `Location` được phép của Requester (kế thừa logic từ US-01-02).
5. Server trả về `200 OK` với danh sách Asset bao gồm trường `status`.
6. Requester xem được trạng thái từng Asset: `Operational`, `Warning`, `Maintenance`, hoặc `Out of Service`.
7. Nếu Asset có `Status = Warning` hoặc `Out of Service`, Requester nhận biết được vấn đề và có thể tạo Maintenance Request (US-03-01).

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `Requester` (hoặc Role không có quyền xem Asset).
- **Lỗi 404 Not Found:** Xảy ra khi gọi `GET /api/assets/{id}` nhưng `AssetID` không tồn tại **hoặc** Asset không thuộc Location được phép của Requester (trả về 404 thay vì 403 để tránh information leak).
- **Trường hợp danh sách trống:** Nếu không có Asset nào trong Location được phép, server trả về `200 OK` với array rỗng `[]`.

## DATA READ/WRITE
- **Read:** Bảng `ASSETS` — Đọc các trường `AssetID`, `Name`, `Type`, `Location`, `Status` được lọc theo Location của Requester.
- **Write:** Không có thao tác ghi. Requester chỉ có quyền **xem**, không có quyền thay đổi trạng thái Asset.

## API CONTRACT
- **Method:** `GET`
- **Endpoint:** `/api/assets` (danh sách) hoặc `/api/assets/{id}` (chi tiết)
- **Auth Level:** Requester, FacilityManager, Technician (JWT bắt buộc)
- **Request Payload:** None
- **Response:**
  - `200 OK` (danh sách):
```json
[
  {
    "assetId": 3,
    "name": "Air Conditioner P301",
    "type": "Air Conditioner",
    "location": "P301",
    "status": "Warning"
  },
  {
    "assetId": 4,
    "name": "Projector P301",
    "type": "Projector",
    "location": "P301",
    "status": "Operational"
  }
]
```
  - `401 Unauthorized`: Token không hợp lệ
  - `403 Forbidden`: Không đủ quyền
  - `404 Not Found`: Asset không tồn tại hoặc ngoài phạm vi

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Requester`.
- Requester chỉ được xem Asset thuộc Location được phép — server **phải filter**, không trả về toàn bộ.
- Requester **không có quyền** gọi endpoint `PATCH /api/assets/{id}/status` (chỉ FacilityManager — BR-16).

## VALIDATION/BUSINESS RULES
- **BR-04 (Asset phải có Location):** Filter theo Location là bắt buộc. Asset không có Location không được hiển thị.
- **BR-16 (Chỉ FM thay đổi Status):** Requester chỉ được đọc `Status`, không được ghi. Backend phải từ chối mọi request PATCH từ Requester.
- **NFR-01 (RBAC):** Chỉ cho phép Requester xem Asset thuộc phạm vi Location được phép. Không lộ Asset ngoài phạm vi.
- **NFR-07 (Giao diện theo Role):** Giao diện dành cho Requester chỉ hiển thị `Status` ở chế độ xem — không có nút "Thay đổi trạng thái". Nếu thấy `Warning` hoặc `Out of Service`, UI nên gợi ý Requester tạo Maintenance Request.
- **REQ-03:** Requester có thể xem trạng thái hiện tại của Asset thuộc phạm vi được phép — đây là yêu cầu Should.

## OBSERVABILITY/LOGGING
- Ghi log action `AssetStatusViewed` bằng Serilog (ở mức `Debug` hoặc `Information`), bao gồm:
  - `UserID` của Requester
  - `Location` filter được áp dụng
  - Số lượng Asset trả về
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Requester với Location `P301` → chỉ nhận Asset ở `P301`.
  - Requester không nhận được Asset thuộc Location khác.
  - Token không hợp lệ → 401.
  - Gọi `GET /api/assets/{id}` với AssetID không thuộc Location của Requester → 404.
  - Requester thử gọi `PATCH /api/assets/{id}/status` → 403.
- **Integration Test:**
  - Đăng nhập với Requester tại Location `P301`.
  - Gửi `GET /api/assets` → nhận danh sách Asset ở `P301` với `status` của từng Asset.
  - Một trong các Asset có `status = Warning` → UI hiển thị đúng.
  - Xác nhận Requester không thấy Asset ở `P101`.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API lọc đúng Asset theo Location của Requester, bao gồm trường `status`.
- Requester không thể thay đổi `status` (PATCH bị từ chối với 403).
- Commit có chứa mã Story ID `US-02-04`.
