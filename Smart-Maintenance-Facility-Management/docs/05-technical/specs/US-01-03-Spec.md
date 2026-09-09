# Story Spec - US-01-03 — Quản lý tài khoản người dùng

**Story ID:** US-01-03
**Requirement IDs:** REQ-24, NFR-01, NFR-02
**Design link:** TODO
**Goal:** Cho phép **Admin** thực hiện quản lý tài khoản người dùng (tạo, xem, cập nhật, vô hiệu hóa) để duy trì quyền truy cập vào hệ thống cho các thành viên.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Admin`** và có JWT Token hợp lệ.
- Hệ thống đã có cấu trúc bảng `USERS` với các trường: `UserID`, `Username`, `PasswordHash`, `Role`.

## HAPPY PATH

### H1 — Tạo tài khoản mới
1. Admin truy cập trang Quản lý người dùng.
2. Admin chọn "Tạo tài khoản mới", nhập `Username`, `Password`, chọn `Role`.
3. Client gửi `POST /api/users` với payload `{ "username": "...", "password": "...", "role": "..." }`.
4. Server kiểm tra `Username` chưa tồn tại, hash password, insert vào bảng `USERS`.
5. Server trả về `201 Created`.

### H2 — Xem danh sách tài khoản
1. Admin truy cập trang Quản lý người dùng.
2. Client gửi `GET /api/users`.
3. Server trả về `200 OK` với danh sách Users (không bao gồm `PasswordHash`).

### H3 — Cập nhật tài khoản
1. Admin chọn tài khoản cần cập nhật (đổi Role hoặc reset Password).
2. Client gửi `PUT /api/users/{id}` với payload `{ "role": "Technician" }`.
3. Server cập nhật bảng `USERS`, trả về `200 OK`.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `username` đã tồn tại (khi tạo mới), hoặc payload thiếu trường bắt buộc, hoặc `role` không hợp lệ (không phải `Requester`, `Technician`, `FacilityManager`, `Admin`).
- **Lỗi 404 Not Found:** Xảy ra khi `UserID` không tồn tại (khi cập nhật).
- **Lỗi 403 Forbidden:** User không có Role `Admin`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Write:** Bảng `USERS` — Insert record mới khi tạo tài khoản (ghi `Username`, `PasswordHash`, `Role`); Update `Role` hoặc `PasswordHash` khi cập nhật.
- **Read:** Bảng `USERS` — Đọc danh sách toàn bộ Users (khi hiển thị); đọc theo `UserID` (khi cập nhật); kiểm tra trùng `Username` (khi tạo mới).

## API CONTRACT

### Tạo tài khoản
- **Method:** `POST`
- **Endpoint:** `/api/users`
- **Auth Level:** Admin
- **Request Payload:**
```json
{
  "username": "tech_tran",
  "password": "P@ssw0rd",
  "role": "Technician"
}
```
- **Response:** `201 Created` | `400 Bad Request`

### Xem danh sách
- **Method:** `GET`
- **Endpoint:** `/api/users`
- **Auth Level:** Admin
- **Response:** `200 OK: [{ "userId": 1, "username": "...", "role": "..." }]`

### Cập nhật tài khoản
- **Method:** `PUT`
- **Endpoint:** `/api/users/{id}`
- **Auth Level:** Admin
- **Request Payload:**
```json
{
  "role": "FacilityManager"
}
```
- **Response:** `200 OK` | `404 Not Found` | `400 Bad Request`

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Admin`.
- Admin không được tự xóa tài khoản của chính mình (tránh lockout).

## VALIDATION/BUSINESS RULES
- **CON-03 (4 Role hợp lệ):** `Role` phải là một trong `Requester`, `Technician`, `FacilityManager`, `Admin`. Từ chối Role không nằm trong danh sách này.
- **NFR-01 (RBAC):** Chỉ Admin mới có quyền truy cập endpoint quản lý tài khoản.
- **NFR-02 (Bảo mật):** Password phải được hash trước khi lưu. Không bao giờ trả về `PasswordHash` trong response API.
- **REQ-24:** Admin có thể quản lý tài khoản — tạo, xem, cập nhật. MVP không yêu cầu xóa vĩnh viễn; có thể vô hiệu hóa nếu cần.

## OBSERVABILITY/LOGGING
- Ghi log action `UserCreated`, `UserUpdated` bằng Serilog, bao gồm:
  - `AdminUserID` (ai thực hiện)
  - `TargetUserID` / `Username` bị tác động
  - Action và giá trị thay đổi (không ghi password)
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Tạo tài khoản với Username đã tồn tại → 400.
  - Tạo tài khoản với Role không hợp lệ → 400.
  - Tạo tài khoản hợp lệ → password được hash, record được insert.
  - Cập nhật Role cho UserID không tồn tại → 404.
  - Truy cập endpoint không phải Admin → 403.
- **Integration Test:**
  - Tạo tài khoản mới, sau đó dùng tài khoản đó đăng nhập → thành công.
  - Cập nhật Role → đăng nhập lại, token mới phản ánh Role mới.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API tạo/cập nhật tài khoản hoạt động đúng, password được hash an toàn.
- Commit có chứa mã Story ID `US-01-03`.
