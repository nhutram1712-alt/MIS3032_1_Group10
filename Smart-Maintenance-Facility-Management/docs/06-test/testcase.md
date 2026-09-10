# Test Cases

**Project:** Smart Maintenance & Facility Management  
**Format:** theo giáo trình Output #26 (bảng `ID | Case | Trace | Expected | Mode`)  
**Last run:** 2026-09-10  
**Environment:** FE `http://localhost:5173` · BE `http://localhost:5031` · seed password `Due@2026`

---

## A. Core suite (mẫu giáo trình — 10 cases)

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-01 | Search/login manager thành công | US-01-01 AC1 · REQ-01 | Vào app, role FacilityManager | Manual | Pass |
| TC-02 | Login sai mật khẩu | US-01-01 AC2 | 401 / lỗi rõ | Manual | Pass |
| TC-03 | FM tạo asset hợp lệ | US-02-01 | 201 + xuất list | Automated | Pass |
| TC-04 | Requester tạo asset | NFR-01 | 403 | Automated | Pass |
| TC-05 | Requester tạo request có mô tả | US-03-01 AC1 | 201 Submitted | Automated | Pass |
| TC-06 | Tạo WO trùng request | US-04-01 | 409 | Automated | Pass |
| TC-07 | PUT role=Admin cho Technician | US-01-03 QT-3 | 400 | Automated | Pass |
| TC-08 | PUT sửa user Admin | US-01-03 QT-4 | 403 | Automated | Pass |
| TC-09 | AI prediction không tạo WO / không đổi Status | US-06-01 · BR-10 | Persist risk only | Automated | Pass |
| TC-10 | Requester gọi prediction | NFR-01 | 403 | Automated | Pass |

---

## B. Extended suite (P0/P1)

### EPIC-01 Auth & Users

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-11 | Logout blacklist token | US-01-02 | Token cũ không dùng lại được | Manual/API | Pass |
| TC-12 | UI 4 role khác menu | US-01-01 AC3 · NFR-07 | Nav đúng quyền | Manual | Pass |
| TC-13 | PUT Requester → Technician | US-01-03 QT-2 | 400 Cannot change role of a Requester | Automated | Pass |
| TC-14 | Tech ↔ FacilityManager | US-01-03 QT-1 · US-01-04 | 200 | Automated | Pass |
| TC-15 | GET /api/roles/permissions | US-01-04 | 200 ma trận 4 role | API | Pass |
| TC-16 | Admin UI không dropdown cho Admin/Requester | US-01-03/04 | Read-only role | Manual | Pass |

### EPIC-02 Assets

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-17 | Type asset không hợp lệ | US-02-01 | 400 | Automated | Pass |
| TC-18 | Anonymous POST asset | NFR-01 | 401 | Automated | Pass |
| TC-19 | FM cập nhật thông tin asset | US-02-03 | 200 | Manual | Pass |
| TC-20 | FM đổi status Out of Service | US-02-04 | 200 + pill UI | Manual | Pass |

### EPIC-03 Requests

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-21 | Description rỗng | US-03-01 | 400 | Automated | Pass |
| TC-22 | AssetId không tồn tại | US-03-01 | 404 | Automated | Pass |
| TC-23 | FM tạo request | NFR-01 | 403 | Automated | Pass |
| TC-24 | FM chuyển status theo BR-14 | US-03-03 | Transition hợp lệ; invalid 400 | Manual/API | Pass |
| TC-25 | FM xem history sau WO completed | US-03-04 · BR-18 | Có result | Manual | Pass |

### EPIC-04 Work Orders

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-26 | FM tạo WO hợp lệ | US-04-01 | 201 Assigned | Automated | Pass |
| TC-27 | Request chưa có asset | US-04-01 | 422 | Automated | Pass |
| TC-28 | Requester tạo WO | NFR-01 | 403 | Automated | Pass |
| TC-29 | FM đổi kỹ thuật viên | US-04-02 | 200 | Manual | Pass |
| TC-30 | Tech Start → In Progress | US-04-03 | 200 | Manual | Pass |
| TC-31 | Tech từ chối kèm lý do | US-04-05 | Có rejectionReason | Manual | Pass |
| TC-32 | Tech Complete + result | US-04-06 · BR-09 | Completed + history | Manual | Pass |
| TC-33 | FM hủy WO Assigned | US-04-02 | Cancelled | Manual | Pass |
| TC-34 | Tech khác thao tác WO không phải mình | BR-07 | 403 | Manual/API | Pass |

### EPIC-05 IoT

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-35 | Ingest mapped device + API key | US-05-03 | 201 persist metrics | Automated | Pass |
| TC-36 | Bad API key | Security | 401 | Automated | Pass |
| TC-37 | Unmapped device | US-05-03 | Không persist / lỗi nghiệp vụ | Automated | Pass |
| TC-38 | Mapping trùng asset (BR-13) | US-05-01 · BR-13 | 400 | Automated/Unit | Pass |
| TC-39 | Requester GET iot-data | NFR-01 | 403 | Automated | Pass |
| TC-40 | FM/Tech xem alerts | US-05-04 | Có severity/threshold | Manual | Pass |

### EPIC-06 AI

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-41 | AI timeout giữ risk cũ | US-06-01 | Không overwrite sai | Automated | Pass |
| TC-42 | Thiếu IoT data | US-06-01 | 404 / không save | Automated | Pass |
| TC-43 | FM dashboard predictions | US-06-02 · NFR-08 | Có assetId + predictedAt | Manual | Pass |

### UI smoke

| ID | Case | Trace | Expected | Mode | Result |
|---|---|---|---|---|---|
| TC-44 | Admin overview + users + permissions | NFR-07 | Số liệu + rule UI | Manual | Pass |
| TC-45 | FM flow Asset→Request→WO | EPIC-02..04 | Happy path | Manual | Pass |
| TC-46 | Tech nhận và hoàn thành WO | EPIC-04 | Actions đúng trạng thái | Manual | Pass |
| TC-47 | Requester chỉ thấy chức năng được phép | NFR-07 | Không Admin/WO manage | Manual | Pass |

---

## C. Automated evidence

```text
dotnet test → Passed 65 / Failed 0 (2026-09-10)
```

Chi tiết verification: `qa-verification.md` · Tổng hợp: `QA_REPORT.md`
