# Story Spec - US-01-01 — Đăng nhập hệ thống

**Story ID:** US-01-01
**Requirement IDs:** REQ-01, NFR-01, NFR-02
**Design link:** TODO
**Goal:** Cho phép **User** thực hiện đăng nhập bằng tài khoản của hệ thống để có thể truy cập và sử dụng các chức năng phù hợp với Role được gán.

## PRECONDITIONS
- Người dùng **chưa đăng nhập** (chưa có JWT Token hợp lệ).
- Tài khoản người dùng đã được **Admin tạo sẵn** trong hệ thống (bảng `USERS`).
- Hệ thống không sử dụng SSO — xác thực bằng tài khoản nội bộ (CON-08).

## HAPPY PATH
1. Người dùng truy cập trang đăng nhập (public, không yêu cầu token).
2. Người dùng nhập `username` và `password`.
3. Client gửi `POST /api/auth/login` với payload `{ "username": "...", "password": "..." }`.
4. Server tra cứu bảng `USERS` theo `Username`, kiểm tra `PasswordHash` với password nhập vào.
5. Nếu hợp lệ, server sinh JWT Token chứa thông tin `UserID`, `Role`, thời gian hết hạn.
6. Server trả về `200 OK` với `{ "token": "jwt_string", "role": "..." }`.
7. Client lưu token vào bộ nhớ phiên (session/localStorage) và chuyển hướng người dùng đến trang chính phù hợp với Role.

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi `username` không tồn tại trong hệ thống hoặc `password` không khớp với `PasswordHash`.
- **Lỗi 400 Bad Request:** Xảy ra khi payload thiếu trường `username` hoặc `password`, hoặc các trường này để trống.
- **Lỗi 403 Forbidden:** Không áp dụng cho endpoint public này.

## DATA READ/WRITE
- **Read:** Bảng `USERS` — đọc theo `Username` để lấy `PasswordHash` và `Role` nhằm xác thực và sinh token.
- **Write:** Không ghi dữ liệu vào database trong flow đăng nhập cơ bản (MVP không yêu cầu lưu session log).

## API CONTRACT
- **Method:** `POST`
- **Endpoint:** `/api/auth/login`
- **Auth Level:** Public (không cần JWT)
- **Request Payload:**
```json
{
  "username": "fm_nguyen",
  "password": "P@ssw0rd"
}
```
- **Response:**
  - `200 OK`: `{ "token": "eyJhbGci...", "role": "FacilityManager" }`
  - `401 Unauthorized`: Sai thông tin đăng nhập
  - `400 Bad Request`: Thiếu trường bắt buộc

## AUTHORIZATION
- Endpoint **Public** — không yêu cầu JWT Token.
- Sau khi đăng nhập thành công, JWT Token được sử dụng cho toàn bộ các request tiếp theo.
- Token phải chứa `Role` hợp lệ: `Requester`, `Technician`, `FacilityManager`, hoặc `Admin`.

## VALIDATION/BUSINESS RULES
- **NFR-01 (Role-Based Access Control):** JWT Token sinh ra phải nhúng `Role` của người dùng để hệ thống có thể kiểm soát quyền truy cập ở các endpoint tiếp theo.
- **NFR-02 (Bảo mật thông tin xác thực):** `password` phải được so sánh với `PasswordHash` đã được lưu (sử dụng thuật toán hash an toàn, ví dụ BCrypt). Không lưu password dạng plaintext.
- **CON-08 (Không tích hợp SSO):** Chỉ sử dụng tài khoản nội bộ của hệ thống; không tích hợp bên ngoài.
- **ASM-01 (Người dùng có tài khoản hợp lệ):** Hệ thống giả định tài khoản đã được tạo sẵn bởi Admin.

## OBSERVABILITY/LOGGING
- Ghi log action `UserLogin` bằng Serilog, bao gồm:
  - `Username` (không ghi password)
  - Kết quả: `Success` / `Failed`
  - `Timestamp`
  - IP Address của client (nếu có)

## TEST PLAN
- **Unit Test:**
  - Kiểm tra hash password khớp → trả về token.
  - Kiểm tra hash password không khớp → trả về 401.
  - Kiểm tra username không tồn tại → trả về 401.
  - Kiểm tra payload thiếu `username` hoặc `password` → trả về 400.
- **Integration Test:**
  - Gửi request `POST /api/auth/login` với credentials hợp lệ → nhận được JWT Token và `role` đúng.
  - Gửi request với credentials sai → nhận `401`.
  - Dùng JWT Token nhận được để gọi một endpoint yêu cầu xác thực → nhận `200 OK`.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API chạy thành công, trả về JWT Token với Role chính xác.
- Password được hash bằng thuật toán an toàn, không lưu plaintext.
- Commit có chứa mã Story ID `US-01-01`.
