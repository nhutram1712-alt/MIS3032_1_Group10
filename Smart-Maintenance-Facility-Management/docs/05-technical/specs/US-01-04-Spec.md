# Story Spec - US-01-04 — Quản lý quyền theo Role

**Story ID:** US-01-04
**Requirement IDs:** REQ-25, NFR-01, NFR-07
**Design link:** TODO
**Goal:** Cho phép **Admin** thực hiện quản lý quyền truy cập theo Role để đảm bảo mỗi người dùng chỉ được truy cập các chức năng được phép tương ứng với Role của họ.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Admin`** và có JWT Token hợp lệ.
- Hệ thống đã định nghĩa 4 Role cố định: `Requester`, `Technician`, `FacilityManager`, `Admin` (CON-03).
- Tài khoản người dùng đã tồn tại trong bảng `USERS`.

## HAPPY PATH

### H1 — Xem phân quyền hiện tại theo Role
1. Admin truy cập trang Phân quyền.
2. Client gửi `GET /api/roles/permissions`.
3. Server trả về `200 OK` với mapping Role → danh sách quyền (permissions) đang áp dụng.
4. Admin xem được bảng phân quyền theo từng Role.

### H2 — Gán/thay đổi Role cho người dùng
1. Admin chọn User cần thay đổi Role.
2. Admin chọn Role mới từ danh sách hợp lệ.
3. Client gửi `PUT /api/users/{id}` với `{ "role": "Technician" }` (dùng chung endpoint với US-01-03).
4. Server cập nhật `Role` trong bảng `USERS`.
5. Server trả về `200 OK`.
6. Token hiện tại của User bị tác động sẽ không còn phản ánh Role mới cho đến khi họ đăng nhập lại.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `role` trong payload không hợp lệ (không thuộc danh sách 4 Role được phép).
- **Lỗi 404 Not Found:** Xảy ra khi `UserID` không tồn tại.
- **Lỗi 403 Forbidden:** User không có Role `Admin`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Write:** Bảng `USERS` — Update cột `Role` theo `UserID`.
- **Read:** Bảng `USERS` — Đọc Role hiện tại của User; đọc danh sách User để hiển thị trong trang phân quyền.

## API CONTRACT

### Gán Role cho User (dùng chung với US-01-03)
- **Method:** `PUT`
- **Endpoint:** `/api/users/{id}`
- **Auth Level:** Admin
- **Request Payload:**
```json
{
  "role": "Technician"
}
```
- **Response:** `200 OK` | `400 Bad Request` | `404 Not Found`

### Xem phân quyền theo Role (tham chiếu/đọc)
- **Method:** `GET`
- **Endpoint:** `/api/roles/permissions`
- **Auth Level:** Admin
- **Response:**
```json
{
  "Requester": ["ViewAsset", "CreateRequest", "TrackRequest"],
  "Technician": ["ViewWorkOrder", "UpdateWorkOrder", "ViewIoTAlert"],
  "FacilityManager": ["ManageAsset", "ManageRequest", "ManageWorkOrder", "ViewIoTData"],
  "Admin": ["ManageUsers", "ManageRoles", "ManageIoTMapping"]
}
```

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Admin`.
- Chính sách phân quyền (permission matrix) là cố định trong MVP; Admin chỉ có thể **gán Role** cho User, không tùy chỉnh từng quyền riêng lẻ.

## VALIDATION/BUSINESS RULES
- **CON-03 (4 Role cố định):** Role hợp lệ chỉ bao gồm `Requester`, `Technician`, `FacilityManager`, `Admin`. Không cho phép tạo Role tùy chỉnh trong MVP.
- **NFR-01 (RBAC):** Hệ thống kiểm soát quyền truy cập dựa trên Role được gán — mỗi API endpoint phải kiểm tra Role từ JWT Token.
- **NFR-07 (Giao diện theo Role):** Giao diện phải hiển thị chức năng phù hợp với Role; việc thay đổi Role cần phản ánh đúng ở lần đăng nhập tiếp theo.
- **REQ-25:** Admin có thể xem và gán quyền truy cập theo Role cho từng User.

## OBSERVABILITY/LOGGING
- Ghi log action `UserRoleUpdated` bằng Serilog, bao gồm:
  - `AdminUserID` (ai thực hiện)
  - `TargetUserID`
  - `OldRole` → `NewRole`
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Cập nhật Role với giá trị không hợp lệ → 400.
  - Cập nhật Role cho UserID không tồn tại → 404.
  - Cập nhật Role thành công → bảng `USERS` phản ánh đúng Role mới.
  - Endpoint bị gọi bởi non-Admin → 403.
- **Integration Test:**
  - Admin cập nhật Role của User từ `Requester` sang `Technician`.
  - User đăng nhập lại → JWT mới chứa Role `Technician`.
  - User thử truy cập endpoint chỉ dành cho `Technician` → `200 OK`.
  - User thử truy cập endpoint chỉ dành cho `Requester` → `403 Forbidden`.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Role-based authorization hoạt động đúng trên toàn bộ API.
- Commit có chứa mã Story ID `US-01-04`.
