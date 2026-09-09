# Story Spec - US-01-02 — Xem Asset được phép truy cập

**Story ID:** US-01-02
**Requirement IDs:** REQ-02, BR-04, NFR-01, NFR-07
**Design link:** TODO
**Goal:** Cho phép **Requester** thực hiện xem danh sách Asset thuộc phòng/khu vực được phép sử dụng để chỉ tiếp cận thông tin Asset liên quan đến mình.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Requester`** và có JWT Token hợp lệ.
- Hệ thống đã có dữ liệu Asset trong bảng `ASSETS` với thông tin `Location` đã được thiết lập.
- Requester đã được Admin gán `Location`/phòng cho phép truy cập (thông qua thuộc tính trong bảng `USERS` hoặc cơ chế mapping tương đương).

## HAPPY PATH
1. Requester đã đăng nhập, truy cập trang danh sách Asset.
2. Client gửi `GET /api/assets` kèm JWT Token trong Authorization header.
3. Server xác thực JWT Token, xác nhận Role là `Requester`.
4. Server đọc `Location` được phép của Requester từ profile.
5. Server truy vấn bảng `ASSETS` lọc theo `Location` phù hợp với Requester.
6. Server trả về `200 OK` với danh sách Asset đã được lọc: `[{ "assetId": 1, "name": "...", "type": "...", "location": "...", "status": "..." }]`.
7. Requester xem danh sách Asset thuộc phạm vi được phép.

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `Requester` (hoặc Role không có quyền xem Asset).
- **Trường hợp danh sách trống:** Nếu không có Asset nào thuộc Location của Requester, server trả về `200 OK` với array rỗng `[]` — không phải lỗi.

## DATA READ/WRITE
- **Read:** Bảng `ASSETS` — đọc các trường `AssetID`, `Name`, `Type`, `Location`, `Status` được lọc theo Location của Requester.
- **Read:** Bảng `USERS` — đọc thông tin Location/phạm vi được phép của Requester hiện tại.
- **Write:** Không có thao tác ghi.

## API CONTRACT
- **Method:** `GET`
- **Endpoint:** `/api/assets`
- **Auth Level:** Requester, FacilityManager, Technician (JWT bắt buộc)
- **Request Payload:** None (có thể dùng query param `?location=X` nếu cần)
- **Response:**
  - `200 OK`:
```json
[
  {
    "assetId": 1,
    "name": "WiFi Router P201",
    "type": "Wi-Fi",
    "location": "P201",
    "status": "Operational"
  }
]
```
  - `401 Unauthorized`: Token không hợp lệ
  - `403 Forbidden`: Không đủ quyền

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Requester`.
- Server **phải lọc dữ liệu** theo Location được phép của Requester — không trả về toàn bộ Asset.
- NFR-07: Giao diện chỉ hiển thị thông tin và chức năng phù hợp với Role `Requester`.

## VALIDATION/BUSINESS RULES
- **BR-04 (Mỗi Asset phải có Location/Room):** Asset không có Location sẽ không hiển thị cho Requester. Server phải đảm bảo filter theo Location là bắt buộc.
- **NFR-01 (Role-Based Access Control):** Chỉ Requester với Location phù hợp mới thấy Asset tương ứng. Facility Manager và Technician có quy tắc truy cập khác.
- **NFR-07 (Giao diện phù hợp với Role):** Requester chỉ thấy thông tin Asset ở mức xem — không có nút chỉnh sửa hay thay đổi trạng thái.
- **REQ-02:** Requester KHÔNG được xem Asset ngoài phạm vi Location được phép.

## OBSERVABILITY/LOGGING
- Ghi log action `AssetListViewed` bằng Serilog, bao gồm:
  - `UserID` của Requester
  - `Location` filter được áp dụng
  - Số lượng Asset trả về
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Kiểm tra filter theo Location hoạt động đúng — chỉ trả về Asset thuộc Location của Requester.
  - Kiểm tra Requester không thấy Asset thuộc Location khác.
  - Kiểm tra token không hợp lệ → 401.
  - Kiểm tra Role không phải Requester → 403.
- **Integration Test:**
  - Gửi `GET /api/assets` với JWT của Requester có Location `P201` → chỉ nhận Asset ở `P201`.
  - Gửi `GET /api/assets` với JWT của Requester không có Location được gán → nhận `200 OK` với array rỗng.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API lọc đúng Asset theo Location của Requester, không lộ Asset ngoài phạm vi.
- Commit có chứa mã Story ID `US-01-02`.
