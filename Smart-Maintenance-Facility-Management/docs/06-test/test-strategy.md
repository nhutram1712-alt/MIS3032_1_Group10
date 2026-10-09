# Chiến lược kiểm thử

**Dự án:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**Người lập:** QA  
**Cập nhật:** 18/09/2026

Hệ thống có 4 vai trò (Admin, Facility Manager, Technician, Requester), nên test phải phủ cả API và giao diện theo quyền. Ưu tiên luồng thật: đăng nhập → tạo yêu cầu → phân công WO → kỹ thuật xử lý, cộng với IoT/AI vì đây là điểm demo.

Môi trường test local: frontend `http://localhost:5173`, API `http://127.0.0.1:5031`, mật khẩu demo `Due@2026`.

---

## 1. Tầng test

| Tầng | Làm gì | Trên dự án này |
|---|---|---|
| Unit | Logic nhỏ, không cần UI | Đổi role (QT-1..4), validate tài sản/yêu cầu, không tạo WO trùng, IoT không map trùng, AI không tự tạo phiếu |
| Integration | Gọi API thật | Login JWT, CRUD tài sản/YC/WO, ingest IoT + API key, prediction, tạo mapping chỉ gửi `assetId` |
| E2E | Đi trên trình duyệt | Login đúng/sai, Facility Manager phân công từ hàng chờ |
| Phi chức năng | Quyền, bảo mật, UI theo role | 401/403, không lộ mật khẩu, menu ẩn đúng role |

Hiện tại:

- Backend: `dotnet test` — 68 pass / 0 fail (chạy lại 18/09/2026).
- Playwright: 3 pass (login sai, login manager, phân công WO).
- Manual: 42 pass / 8 fail trên 50 case — chi tiết trong `testcase.md`.

Test tự động xanh không có nghĩa là hết lỗi trên UI. 8 case fail còn lại chủ yếu chữ tiếng Anh và chặn route chưa chặt.

---

## 2. Phủ theo module

| Module | Đã cover | Còn hở |
|---|---|---|
| Auth / User | Login, logout, QT đổi role | — |
| Tài sản | Tạo hợp lệ, Requester 403 | — |
| Yêu cầu / WO | Tạo YC, phân công, không WO trùng | Copy “Work Order” chưa Việt hết |
| IoT | Ingest + API key, mapping tự sinh `SENSOR_{id}` | Chưa Unmap / sửa Device ID trên UI; cột chỉ số còn raw |
| AI | Lưu risk, không side-effect WO | Nhãn High/Medium chưa Việt |
| Phân quyền UI | Menu theo role | Gõ thẳng `/admin/*`, `/predictions` vẫn vào được trang |

---

## 3. Mười case mẫu

| ID | Case | Trace | Mong đợi | Cách test |
|---|---|---|---|---|
| TC-01 | Login `manager` / `Due@2026` | US-01-01 | Vào app, role Facility Manager | Manual + Playwright |
| TC-02 | Sai mật khẩu | US-01-01 | Báo lỗi, ở lại trang login | Manual + Playwright |
| TC-03 | FM tạo tài sản hợp lệ | US-02-01 | 201, có trên list | Automated |
| TC-04 | Requester POST tài sản | NFR-01 | 403 | Automated |
| TC-05 | Requester tạo yêu cầu | US-03-01 | 201, trạng thái Submitted | Automated |
| TC-06 | Tạo WO lần 2 cùng một YC | US-04-01 | 409 | Automated |
| TC-07 | Gán role Admin cho Technician | US-01-03 | 400 | Automated |
| TC-08 | Sửa tài khoản Admin | US-01-03 | 403 | Automated |
| TC-09 | Chạy prediction | US-06-01 | Chỉ lưu risk, không tạo WO | Automated |
| TC-10 | Requester gọi API prediction | NFR-01 | 403 | Automated |

Bộ đủ 50 case (kèm kết quả) nằm ở `testcase.md`.

---

## 4. Kịch bản E2E

**A. Facility Manager phân công việc**

- Có yêu cầu đang chờ, chưa có WO.
- FM chọn kỹ thuật viên, nhấn Phân công → WO = Đã phân công, YC chuyển Đang xử lý.
- Không tạo được WO thứ hai cho cùng YC.
- Technician nhấn Bắt đầu → Đang thực hiện, rồi nhập kết quả và Hoàn thành.
- AI không tự tạo thêm phiếu.

**B. Admin gắn cảm biến**

- Còn tài sản chưa map.
- Admin chọn máy, nhấn Tạo mapping (không nhập Device ID) → mã `SENSOR_{id}`, máy biến khỏi dropdown.
- Map lại cùng tài sản thì bị từ chối.

**C. Admin đổi vai trò nhân sự**

- Chỉ luân chuyển Technician ↔ Facility Manager.
- Không nâng Requester, không đụng tài khoản Admin.

Playwright hiện cover login + đoạn phân công của kịch bản A. Phần Technician hoàn thành vẫn test tay.

---

## 5. Khi nào chạy lại

Sau mỗi lần sửa bug, chạy lại từ đầu, không lấy kết quả cũ:

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
cd src/frontend && npm run test:e2e
```

Cập nhật cột Result trong `testcase.md`. Test BE pass không được ghi thành “cả bộ manual pass”.
