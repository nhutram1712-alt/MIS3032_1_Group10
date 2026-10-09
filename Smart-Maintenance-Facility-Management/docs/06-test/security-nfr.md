# Kiểm tra bảo mật và yêu cầu phi chức năng

**Dự án:** Smart Maintenance & Facility Management  
**Ngày:** 18/09/2026  
**Tham chiếu:** NFR-01 → NFR-08

Máy local, không TLS. Phần hardening cloud (WAF, secret manager, chứng chỉ) không nằm trong demo.

---

## 1. Bảo mật

| ID | Kiểm tra | Cách xem | Kết quả |
|---|---|---|---|
| SEC-01 | API phải có JWT, trừ login và ingest IoT | Controller + middleware | Pass |
| SEC-02 | Phân quyền theo role | `[Authorize(Roles)]`, TC-04/07/08/10 | Pass |
| SEC-03 | Mật khẩu hash, không trả hash ra client | BCrypt | Pass |
| SEC-04 | Logout vô hiệu token | Blacklist `jti` | Pass |
| SEC-05 | IoT bắt API key | Header `X-API-Key`, TC-40 | Pass |
| SEC-06 | Không leo thang qua PUT users | QT-2/3/4 | Pass |
| SEC-07 | Không commit secret | Chỉ `.env.example` | Pass |
| SEC-08 | Lỗi trả JSON `{ "error": "..." }` | Exception middleware | Pass |
| SEC-09 | FE chặn `/admin`, `/alerts`, `/predictions` đúng role | Đi URL trên trình duyệt | **Fail** — BUG-011, BUG-013. API vẫn chặn, không gọi được API đặc quyền |

---

## 2. NFR

| NFR | Cách verify | Kết quả |
|---|---|---|
| NFR-01 RBAC | 401/403 tự động + menu theo role | API đạt. FE còn hở deep-link |
| NFR-02 Dữ liệu đăng nhập | Hash + DTO không lộ password | Pass |
| NFR-03 YC / WO / history khớp | WO trùng 409; complete có history | Pass |
| NFR-04 Có timestamp | `createdAt`, `detectedAt`, `predictedAt` | Pass |
| NFR-05 Chu kỳ lấy mẫu IoT | Tìm config / UI | Chưa chứng minh — BUG-012 |
| NFR-06 Chịu tải | Chưa load test | Để sau |
| NFR-07 UI theo role, tiếng Việt | Menu 4 role; đọc Alerts / AI / WO | Một phần. Còn chữ Anh (TC-38, 48, 52) |
| NFR-08 Prediction gắn tài sản + thời điểm | TC-09, TC-51, tên không `#id` | Pass |

---

## 3. Kiểm tra nhanh khác

| Việc | Kết quả |
|---|---|
| Login + list trên local, cảm giác không đơ | Ổn |
| Tab / Enter trên form login, admin | Ổn |
| Frontend không nhét secret | Ổn |
| Tạo/sửa user có ghi nhận | Ổn. Audit bước WO để làm thêm nếu cần |
| Device ID tự sinh không lộ key | Ổn (`SENSOR_{id}`) |

Không có lỗ hổng Critical. Hai chỗ FE guard (BUG-011, BUG-013) chỉ lộ trang, không vượt được API.
