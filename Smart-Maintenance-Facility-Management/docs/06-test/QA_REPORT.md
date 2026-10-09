# Báo cáo QA

**Dự án:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**Ngày báo cáo:** 18/09/2026  
**Build:** frontend Vite + backend `.run/out19`

Báo cáo vòng test sau khi tách trang Admin, mapping IoT tự sinh Device ID, và gộp hàng chờ phân công vào Work Order.

---

## 1. Phạm vi

Đã test EPIC-01 đến EPIC-06: đăng nhập/phân quyền, tài sản, yêu cầu sự cố, work order, IoT, AI.

Gồm:

- Test tự động backend + 3 case Playwright
- Test tay theo `testcase.md`
- Rà bug, code review, quyền/NFR

Không nằm trong vòng này: host production, HTTPS, load test.

Demo public từng bật ngrok; lúc kiểm tra 18/09 tunnel đã tắt (`ERR_NGROK_3200`) nên chưa smoke được trên URL public. Chạy local vẫn dùng được.

---

## 2. Môi trường

| Thành phần | Chi tiết |
|---|---|
| Backend | ASP.NET Core 9, `http://127.0.0.1:5031`, InMemory |
| Frontend | React + Vite, `http://localhost:5173` |
| Tài khoản | `admin` / `manager` / `requester` / `tech1`, mật khẩu `Due@2026` |
| Dữ liệu seed | khoảng 78 tài sản, 71 đã map IoT, còn 7 chưa map |
| Trình duyệt | Chrome, Edge |

---

## 3. Kết quả

| Hạng mục | Kết quả |
|---|---|
| Manual TC-01 → TC-60 (50 case chạy) | 42 pass / 8 fail |
| 10 case mẫu (TC-01 → TC-10) | 10/10 pass |
| `dotnet test` | 68 pass / 0 fail |
| Playwright | 3 pass / 0 fail |
| Bug Critical / High còn mở | 0 |
| Bug Medium / Low còn mở | 8 |

Tài liệu kèm theo: `test-strategy.md`, `testcase.md`, `bug-log.md`, `code-review.md`, `security-nfr.md`, `qa-verification.md`.

---

## 4. Việc còn mở

| Mã | Mức | Tóm tắt |
|---|---|---|
| BUG-007 | Medium | Cột chỉ số Alerts còn `power_status`, `temperature` |
| BUG-008 | Low | Rủi ro AI còn `High` / `Medium` |
| BUG-009 | Medium | Admin IoT không gỡ mapping trên UI |
| BUG-010 | Medium | Không sửa Device ID trên UI (API PUT đã có) |
| BUG-011 | Medium | `/admin/*` chưa chặn từ router, chỉ báo trong trang |
| BUG-012 | Low | Chưa chứng minh được interval lấy mẫu IoT |
| BUG-013 | Medium | Requester gõ `/predictions` hoặc `/alerts` vẫn vào trang, hiện lỗi thô |
| BUG-014 | Low | Trang Work Order còn chữ tiếng Anh (tên menu, nút Đổi KT) |

Đã đóng trong vòng này: BUG-006 (có Playwright), BUG-015 (test mapping auto Device ID).

Ngày 09/10/2026 nhóm mở lại UI local, 6 lỗi UX/guard trên vẫn còn (trừ BUG-012 chưa đo lại).

---

## 5. Rủi ro

- 8 fail đều không chặn luồng chính, nhưng nhìn thấy ngay khi demo (chữ Anh, trang admin lọt URL).
- Backend InMemory: restart là mất data, phải seed lại.
- Playwright mới cover login và phân công, chưa đi hết Technician hoàn thành.
- URL ngrok free hay chết / đổi subdomain — không dùng làm môi trường chấm.

Kết luận local: **pass có điều kiện**. Có thể demo trên máy, chưa đưa production.

---

## 6. Kết luận

QA chốt **pass có điều kiện** ngày 18/09/2026.

- Luồng bắt buộc (login, tạo YC, phân công, quyền API) đạt.
- Không còn bug Critical/High.
- Còn 8 lỗi Medium/Low, chủ yếu UI và chặn route.
- Không ghi “100% pass”. Đúng số là **42/50** test tay, backend 68/68, Playwright 3/3.
