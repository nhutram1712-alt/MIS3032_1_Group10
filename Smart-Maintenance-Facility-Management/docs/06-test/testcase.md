# Test Cases

**Project:** Smart Maintenance & Facility Management  
**Format:** theo giáo trình Output #26 (bảng `ID | Case | Trace | Expected | Mode`)  
**Build under test:** FE Vite + BE `.run/out19` (2026-09-10 chiều)  
**Environment:** FE `http://127.0.0.1:5173` · BE `http://127.0.0.1:5031` · seed password `Due@2026`  
**Last run:** 2026-09-10

> Kết quả vòng này: **Pass 40 / Fail 10** (~**20% Fail**) trên 50 case.  
> Automated BE vẫn green (`65/0`) — các Fail chủ yếu ở **manual / UX / gap coverage** sau đợt refactor UI (Admin split, IoT mapping, WO queue).

---

## A. Core suite (mẫu giáo trình — 10 cases)

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-01 | Login manager thành công | US-01-01 AC1 · REQ-01 | Vào app, role FacilityManager | Manual | Pass |
| TC-02 | Login sai mật khẩu | US-01-01 AC2 | 401 / lỗi rõ | Manual | Pass |
| TC-03 | FM tạo asset hợp lệ | US-02-01 | 201 + xuất list | Automated | Pass |
| TC-04 | Requester tạo asset | NFR-01 | 403 | Automated | Pass |
| TC-05 | Requester tạo request có mô tả | US-03-01 AC1 | 201 Submitted | Automated | Pass |
| TC-06 | Tạo WO trùng request | US-04-01 | 409 | Automated | Pass |
| TC-07 | PUT role=Admin cho Technician | US-01-03 QT-3 | 400 | Automated | Pass |
| TC-08 | PUT sửa user Admin | US-01-03 QT-4 | 403 | Automated | Pass |
| TC-09 | AI prediction không tạo WO / không đổi Status | US-06-01 · BR-10 | Persist risk only | Automated | Pass |
| TC-10 | Requester gọi prediction API | NFR-01 | 403 | Automated | Pass |

---

## B. Extended suite (P0/P1) — cập nhật theo code mới

### EPIC-01 Auth & Users

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-11 | Logout blacklist token | US-01-02 | Token cũ không dùng lại được | Manual/API | Pass |
| TC-12 | UI 4 role khác menu (Admin: Người dùng + IoT tách) | US-01-01 AC3 · NFR-07 | Nav đúng quyền; không còn 1 trang Admin gộp | Manual | Pass |
| TC-13 | PUT Requester → Technician | US-01-03 QT-2 | 400 | Automated | Pass |
| TC-14 | Tech ↔ FacilityManager | US-01-03 QT-1 | 200 | Automated | Pass |
| TC-15 | GET /api/roles/permissions | US-01-04 | 200 ma trận 4 role (API) | API | Pass |
| TC-16 | Admin Users: không còn ma trận permission trên UI; role Admin/Requester read-only | US-01-03/04 | Đúng thiết kế MVP | Manual | Pass |
| TC-17 | Deep-link `/admin/users` hoặc `/admin/iot` khi không phải Admin | NFR-01 · NFR-07 | Redirect / chặn route (RequireRoles) | Manual | **Fail** |

### EPIC-02 Assets

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-18 | Type asset không hợp lệ | US-02-01 | 400 | Automated | Pass |
| TC-19 | Anonymous POST asset | NFR-01 | 401 | Automated | Pass |
| TC-20 | FM cập nhật thông tin asset | US-02-03 | 200 | Manual | Pass |
| TC-21 | FM đổi status Out of Service | US-02-04 | 200 + pill UI | Manual | Pass |
| TC-22 | Requester không thấy menu/route Assets catalog | NFR-07 | Nav ẩn + `/assets` redirect | Manual | Pass |

### EPIC-03 Requests

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-23 | Description rỗng | US-03-01 | 400 | Automated | Pass |
| TC-24 | AssetId không tồn tại | US-03-01 | 404 | Automated | Pass |
| TC-25 | FM tạo request API | NFR-01 | 403 | Automated | Pass |
| TC-26 | FM mở `/requests` → redirect Work Order | UX merge | Redirect `/work-orders` | Manual | Pass |
| TC-27 | Requester chọn vị trí → thiết bị khi tạo YC | US-03-01 | Dropdown theo phòng | Manual | Pass |
| TC-28 | Sau FM phân công WO, requester thấy trạng thái đã sync | BR-14 sync | Pending / đang xử lý… | Manual | Pass |

### EPIC-04 Work Orders

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-29 | FM tạo WO hợp lệ (Submitted) | US-04-01 | 201 Assigned | Automated | Pass |
| TC-30 | Request chưa có asset | US-04-01 | 422 | Automated | Pass |
| TC-31 | Requester tạo WO | NFR-01 | 403 | Automated | Pass |
| TC-32 | FM queue: phân công từ YC chờ | US-04-02 | Tạo WO + biến mất khỏi queue | Manual | Pass |
| TC-33 | FM đổi kỹ thuật viên | US-04-02 | 200 | Manual | Pass |
| TC-34 | Tech Start → In Progress | US-04-03 | 200 | Manual | Pass |
| TC-35 | Tech từ chối kèm lý do | US-04-05 | Có rejectionReason | Manual | Pass |
| TC-36 | Tech Complete + result | US-04-06 · BR-09 | Completed + history | Manual | Pass |
| TC-37 | Tech khác thao tác WO không phải mình | BR-07 | 403 | Manual/API | Pass |
| TC-38 | Một số pill/status còn copy Anh–Việt lẫn trên WO | NFR-07 UX | Thuần Việt nhất quán | Manual | **Fail** |

### EPIC-05 IoT

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-39 | Ingest mapped device + API key | US-05-03 | 201 persist | Automated | Pass |
| TC-40 | Bad API key | Security | 401 | Automated | Pass |
| TC-41 | Unmapped device | US-05-03 | Không persist | Automated | Pass |
| TC-42 | Mapping trùng asset (BR-13) | US-05-01 · BR-13 | 400 | Automated | Pass |
| TC-43 | Admin tạo mapping: chỉ hiện tài sản **chưa map**; Device ID **tự sinh** `SENSOR_{id}` | US-05-01 | Không nhập tay; không chọn đã map | Manual | Pass |
| TC-44 | Seed còn ≥1 tài sản chưa map để demo | Seed | Có unmapped (hiện 7) | Manual | Pass |
| TC-45 | Admin IoT: Unmap / xóa mapping trên UI | US-05-01 | Có thao tác gỡ liên kết | Manual | **Fail** |
| TC-46 | Admin IoT: sửa Device ID sau khi map (API PUT có, UI không) | US-05-02 | Có form/update | Manual | **Fail** |
| TC-47 | FM/Tech xem alerts | US-05-04 | Có severity/threshold | Manual | Pass |
| TC-48 | Alerts: cột chỉ số hiển thị label người dùng (không raw `power_status`) | NFR-07 | Label Việt / human-readable | Manual | **Fail** |

### EPIC-06 AI

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-49 | AI timeout giữ risk cũ | US-06-01 | Không overwrite sai | Automated | Pass |
| TC-50 | Thiếu IoT data | US-06-01 | 404 / không save | Automated | Pass |
| TC-51 | FM dashboard predictions; tên tài sản **không** prefix `#id` | US-06-02 · NFR-08 | Chỉ tên + location | Manual | Pass |
| TC-52 | Predictions: mức rủi ro Việt hóa (Cao/Trung bình) | NFR-07 | Không để nguyên `High`/`Medium` | Manual | **Fail** |

### UI smoke / NFR / coverage gaps

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-53 | Admin Users + IoT tách trang, overview Admin ổn | NFR-07 | Số liệu + 2 nav | Manual | Pass |
| TC-54 | FM flow: queue phân công → Tech hoàn thành | EPIC-03/04 | Happy path | Manual | Pass |
| TC-55 | Tech nhận và hoàn thành WO | EPIC-04 | Actions đúng TT | Manual | Pass |
| TC-56 | Requester chỉ thấy chức năng được phép | NFR-07 | Không Assets catalog / Admin | Manual | Pass |
| TC-57 | Requester gõ URL `/predictions` hoặc `/alerts` | NFR-07 | Chặn route hoặc empty có hướng dẫn (không lỗi thô) | Manual | **Fail** |
| TC-58 | Playwright/Cypress E2E critical journey | Output #27 | Suite E2E FE tồn tại | Manual | **Fail** |
| TC-59 | NFR-05 cấu hình IoT interval | NFR-05 | Có config/UI hoặc documented default có chứng minh | Manual | **Fail** |
| TC-60 | Automated regression cho `POST /api/iot-mappings` chỉ `assetId` (auto deviceId) | US-05-01 | Có unit/integration case | Automated | **Fail** |

---

## C. Summary vòng test

| Suite | Pass | Fail | Fail rate |
|---|---:|---:|---:|
| Core TC-01..10 | 10 | 0 | 0% |
| Extended TC-11..60 | 30 | 10 | 25% |
| **Tổng** | **40** | **10** | **~20%** |

### Fail → Bug map

| TC | Bug |
|---|---|
| TC-17 | BUG-011 |
| TC-38 | BUG-014 |
| TC-45 | BUG-009 |
| TC-46 | BUG-010 |
| TC-48 | BUG-007 |
| TC-52 | BUG-008 |
| TC-57 | BUG-013 |
| TC-58 | BUG-006 |
| TC-59 | BUG-012 |
| TC-60 | BUG-015 |

---

## D. Automated evidence (BE)

```text
dotnet test → Passed 65 / Failed 0 (2026-09-10, fresh)
```

> Giáo trình: không dùng kết quả test cũ sau khi sửa code — xem `qa-verification.md`.  
> Lưu ý: **65 automated Pass ≠ 100% QA package Pass** (manual Fail ~20% vẫn mở).
