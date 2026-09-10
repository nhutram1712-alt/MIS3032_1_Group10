# Output #26 — Test Strategy

**Project:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**Status:** Approved for QA package (pre-release)  
**Owner:** QA  
**Date:** 2026-09-10

---

## 1. Testing pyramid (theo giáo trình)

| Lớp | Mục tiêu gợi ý | Dùng cho (dự án này) |
|---|---|---|
| Unit | 60–70% test logic | Role rules (QT-1..4), validators asset/request, WO create rules, IoT mapping BR-13, prediction BR-10 |
| Integration | 15–25% | API + auth JWT, create asset/request/WO, IoT ingest + get data, prediction generate/get |
| E2E | 5–15% | Login theo role → critical journeys (Request→WO→Complete; Admin user rotate; IoT mapping) |
| Non-functional | bổ sung | RBAC 401/403, secrets/.env.example, latency smoke local, keyboard form cơ bản, audit user create/update |

**Hiện trạng:** Unit + Integration automated trong `SmartMaintenance.Tests` (**65 passed / 0 failed**, 2026-09-10). E2E UI chủ yếu **Manual** (Playwright để Final).

---

## 2. Coverage theo layer (case mẫu của nhóm)

| Layer | Coverage |
|---|---|
| Unit | User role steel rules; asset type/status validation; request description rules; WO duplicate/asset match; IoT unmapped device; prediction invalid risk / no IoT data |
| Integration | `POST /api/auth/login`; `POST /api/assets`; `POST /api/requests`; `POST /api/work-orders`; `POST /api/iot/ingest` (API key); `GET /api/assets/{id}/iot-data`; `GET /api/assets/{id}/prediction`; `PUT /api/users/{id}` QT cases; `GET /api/roles/permissions` |
| E2E | Xem mục *E2E Scenario* bên dưới + `testcase.md` TC-UI-* |
| Non-functional | NFR-01 RBAC; NFR-02 password hash; NFR-07 UI theo role; NFR-08 prediction timestamps; no secrets in repo |

---

## 3. Mười test cases mẫu (format Output #26 / bảng TC giáo trình)

| ID | Case | Trace | Expected | Mode |
|---|---|---|---|---|
| TC-01 | Login đúng `manager` / `Due@2026` | US-01-01 AC1 · REQ-01 | 200 JWT + role FacilityManager | Manual |
| TC-02 | Login sai password | US-01-01 AC2 | 401 + thông báo lỗi | Manual |
| TC-03 | FM tạo asset hợp lệ | US-02-01 · REQ | 201 + asset persisted | Automated |
| TC-04 | Requester POST asset | NFR-01 | 403 | Automated |
| TC-05 | Requester tạo request | US-03-01 AC1 | 201 status Submitted | Automated |
| TC-06 | FM tạo WO lần 2 cùng request | US-04-01 · BR | 409 | Automated |
| TC-07 | PUT gán Admin cho Technician | US-01-03 QT-3 | 400 cannot assign Admin via PUT | Automated |
| TC-08 | PUT sửa tài khoản Admin | US-01-03 QT-4 | 403 Admin accounts cannot be modified | Automated |
| TC-09 | Prediction hợp lệ không tạo WO | US-06-01 · BR-10 | Risk lưu; WO count không tăng; Asset Status không đổi | Automated |
| TC-10 | Requester GET prediction | NFR-01 · US-06 | 403 | Automated |

> Bộ TC đầy đủ hơn: `testcase.md`.

---

## 4. E2E Scenario mẫu (Output #27)

**Scenario:** Facility Manager xử lý sự cố an toàn từ Request → Work Order → hoàn thành

```
Given Facility Manager đã đăng nhập (manager / Due@2026)
  And tồn tại Asset Operational và Request Status = Pending/Submitted có AssetId
When FM tạo Work Order với Technician hợp lệ (tech1)
Then hệ thống trả 201 và WO Status = Assigned
  And không tạo được WO thứ hai cho cùng Request (409)
When Technician tech1 đăng nhập và chọn Bắt đầu
Then WO Status = In Progress
When Technician nhập result và chọn Hoàn thành
Then WO Status = Completed
  And Maintenance History có result + completedAt
  And AI Prediction (nếu có) không tự tạo thêm Work Order (BR-10)
```

**Scenario phụ — Admin luân chuyển nhân sự (không leo thang đặc quyền):**

```
Given Admin đã đăng nhập
  And User target đang là Technician
When Admin đổi Role sang FacilityManager qua PUT /api/users/{id}
Then 200 OK và Role mới = FacilityManager
When Admin thử gán Role = Admin cho user đó
Then 400 Bad Request (QT-3)
When Admin thử sửa tài khoản Role = Admin
Then 403 Forbidden (QT-4)
```

---

## 5. Completion gate

Sau mỗi vòng sửa bug/review: chạy lại **fresh**

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

Không dùng kết quả test cũ để khẳng định bản mới pass (theo giáo trình Output #25 completion gate).
