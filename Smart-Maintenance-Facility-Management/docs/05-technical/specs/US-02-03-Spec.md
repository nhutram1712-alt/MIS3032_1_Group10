# Story Spec - US-02-03 — Quản lý trạng thái Asset

**Story ID:** US-02-03
**Requirement IDs:** REQ-08, BR-16, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện xem thông tin Asset và thay đổi thủ công Asset Status để duy trì trạng thái hiện tại của Asset phản ánh đúng thực tế vận hành.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Asset cần xem/cập nhật trạng thái đã tồn tại trong bảng `ASSETS`.

## HAPPY PATH

### H1 — Xem thông tin và trạng thái Asset
1. Facility Manager truy cập trang danh sách Asset.
2. Client gửi `GET /api/assets` (hoặc `GET /api/assets/{id}` cho chi tiết).
3. Server trả về `200 OK` với thông tin Asset bao gồm `Status` hiện tại.
4. Facility Manager xem được trạng thái Asset: `Operational`, `Warning`, `Maintenance`, hoặc `Out of Service`.

### H2 — Thay đổi thủ công Asset Status
1. Facility Manager chọn Asset cần thay đổi trạng thái, click "Cập nhật trạng thái".
2. Facility Manager chọn trạng thái mới từ danh sách: `Operational`, `Warning`, `Maintenance`, `Out of Service`.
3. Client gửi `PATCH /api/assets/{id}/status` với payload `{ "status": "Maintenance" }`.
4. Server xác thực JWT Token và Role `FacilityManager`.
5. Server kiểm tra `AssetID` tồn tại.
6. Server kiểm tra `status` hợp lệ.
7. Server thực hiện `UPDATE` cột `Status` trong bảng `ASSETS`.
8. Server trả về `200 OK`.
9. Facility Manager thấy trạng thái mới được phản ánh ngay lập tức.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `status` trong payload không hợp lệ (không phải `Operational`, `Warning`, `Maintenance`, `Out of Service`).
- **Lỗi 404 Not Found:** Xảy ra khi `AssetID` trong URL không tồn tại.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager` — **chỉ Facility Manager mới được thay đổi thủ công Asset Status (BR-16)**.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Write:** Bảng `ASSETS` — Update cột `Status` theo `AssetID` khi Facility Manager thay đổi thủ công.
- **Read:** Bảng `ASSETS` — Đọc `AssetID`, `Name`, `Type`, `Location`, `Status` để hiển thị thông tin và trạng thái hiện tại.

## API CONTRACT

### Xem thông tin Asset
- **Method:** `GET`
- **Endpoint:** `/api/assets` hoặc `/api/assets/{id}`
- **Auth Level:** Requester, FacilityManager, Technician
- **Response:** `200 OK: [{ "assetId": 1, "name": "...", "type": "...", "location": "...", "status": "Operational" }]`

### Cập nhật trạng thái Asset
- **Method:** `PATCH`
- **Endpoint:** `/api/assets/{id}/status`
- **Auth Level:** FacilityManager
- **Request Payload:**
```json
{
  "status": "Maintenance"
}
```
- **Response:**
  - `200 OK`: Cập nhật thành công
  - `400 Bad Request`: Status không hợp lệ
  - `404 Not Found`: AssetID không tồn tại
  - `403 Forbidden`: Không đủ quyền

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager` cho endpoint `PATCH /api/assets/{id}/status`.
- **BR-16:** TUYỆT ĐỐI chỉ `FacilityManager` được phép thay đổi `Status` thủ công — Requester, Technician không được phép.

## VALIDATION/BUSINESS RULES
- **BR-16 (Chỉ Facility Manager thay đổi Asset Status thủ công):** Đây là Business Rule cốt lõi của Story này. Backend phải enforce kiểm tra Role nghiêm ngặt trên endpoint PATCH. Bất kỳ Role nào khác đều nhận `403 Forbidden`.
- **Asset Status hợp lệ:** `Status` chỉ được nhận một trong bốn giá trị: `Operational`, `Warning`, `Maintenance`, `Out of Service`. Từ chối bất kỳ giá trị nào khác với `400 Bad Request`.
- **NFR-03 (Tính nhất quán dữ liệu):** Việc thay đổi `Status` thủ công không ảnh hưởng đến các Maintenance Request hoặc Work Order đang mở liên quan — chúng vẫn giữ nguyên trạng thái của mình.
- **NFR-04 (Lưu timestamp):** Khuyến nghị log lại thời điểm thay đổi `Status` để traceability (có thể qua Observability/Logging).
- **REQ-08:** Facility Manager xem và thay đổi Asset Status là yêu cầu bắt buộc (Must).

## OBSERVABILITY/LOGGING
- Ghi log action `AssetStatusUpdated` bằng Serilog, bao gồm:
  - `UserID` (Facility Manager thực hiện)
  - `AssetID`
  - `OldStatus` → `NewStatus`
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - PATCH với `status` không hợp lệ → 400.
  - PATCH với `AssetID` không tồn tại → 404.
  - PATCH bởi Role `Requester` → 403.
  - PATCH bởi Role `Technician` → 403.
  - PATCH bởi `FacilityManager` hợp lệ → 200 OK, bảng `ASSETS` được update.
- **Integration Test:**
  - Gửi `PATCH /api/assets/1/status` với `{ "status": "Out of Service" }` bằng token FacilityManager → 200 OK.
  - Gửi `GET /api/assets/1` → xác nhận `status` là `Out of Service`.
  - Thử PATCH bằng token Requester → 403 Forbidden.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- BR-16 được enforce chính xác — chỉ FacilityManager thay đổi được Status.
- API trả về đúng Status hợp lệ, từ chối giá trị không hợp lệ.
- Commit có chứa mã Story ID `US-02-03`.
