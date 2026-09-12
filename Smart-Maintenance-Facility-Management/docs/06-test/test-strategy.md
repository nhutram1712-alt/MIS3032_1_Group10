# Output #26 — Test Strategy

**Project:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**Status:** Updated after UI/BE refresh (2026-09-10) — Conditional Pass  
**Owner:** QA  
**Date:** 2026-09-10

---

## 1. Testing pyramid (theo giáo trình)

| Lớp | Mục tiêu gợi ý | Dùng cho (dự án này) |
|---|---|---|
| Unit | 60–70% test logic | Role rules (QT-1..4), validators asset/request, WO create rules, IoT mapping BR-13, prediction BR-10 |
| Integration | 15–25% | API + auth JWT, create asset/request/WO, IoT ingest + get data, prediction generate/get, **(gap)** create mapping auto DeviceId |
| E2E | 5–15% | Login theo role → critical journeys (FM queue→WO→Complete; Admin IoT map; Requester YC) — hiện **Manual** |
| Non-functional | bổ sung | RBAC 401/403, secrets/.env.example, latency smoke local, keyboard form cơ bản, audit user create/update |

**Hiện trạng:** Unit + Integration automated (**65 passed / 0 failed**, fresh 2026-09-10).  
**Manual suite sau refactor:** **Pass 40 / Fail 10 (~20%)** — xem `testcase.md`.  
E2E UI automated (Playwright) → **Final** (BUG-006).

---

## 2. Coverage theo layer (cập nhật)

| Layer | Coverage |
|---|---|
| Unit | User QT rules; asset type/status; request description; WO duplicate/asset; IoT unmapped ingest; prediction no IoT / BR-10 |
| Integration | login; assets; requests; work-orders; iot ingest + API key; iot-data; prediction; users PUT QT; roles/permissions |
| Integration **gap** | `POST /api/iot-mappings` chỉ `{ assetId }` (auto `SENSOR_{id}`) — **BUG-015 / TC-60 Fail** |
| E2E manual | FM queue phân công; Admin Users/IoT tách; mapping unmapped-only; requester vị trí→thiết bị; ẩn Assets requester |
| Non-functional | NFR-01/02/07/08 Must; NFR-05 Partial Fail; NFR-06 Deferred |

---

## 3. Mười test cases mẫu (format Output #26)

| ID | Case | Trace | Expected | Mode |
|---|---|---|---|---|
| TC-01 | Login đúng `manager` / `Due@2026` | US-01-01 AC1 · REQ-01 | 200 JWT + FacilityManager | Manual |
| TC-02 | Login sai password | US-01-01 AC2 | 401 + thông báo lỗi | Manual |
| TC-03 | FM tạo asset hợp lệ | US-02-01 | 201 + persisted | Automated |
| TC-04 | Requester POST asset | NFR-01 | 403 | Automated |
| TC-05 | Requester tạo request | US-03-01 AC1 | 201 Submitted | Automated |
| TC-06 | FM tạo WO lần 2 cùng request | US-04-01 | 409 | Automated |
| TC-07 | PUT gán Admin cho Technician | US-01-03 QT-3 | 400 | Automated |
| TC-08 | PUT sửa tài khoản Admin | US-01-03 QT-4 | 403 | Automated |
| TC-09 | Prediction hợp lệ không tạo WO | US-06-01 · BR-10 | Risk lưu; không side-effect WO/Status | Automated |
| TC-10 | Requester GET prediction | NFR-01 | 403 | Automated |

> Bộ đầy đủ + Result vòng mới: `testcase.md` (50 case mapped → 40 Pass / 10 Fail).

---

## 4. E2E Scenario mẫu (Output #27)

**Scenario A — FM xử lý từ hàng chờ phân công Work order:**

```
Given Facility Manager đã đăng nhập
  And tồn tại Request Status = Chờ phân công chưa có WO
When FM chọn kỹ thuật viên và bấm Phân công trên hàng chờ
Then hệ thống tạo Work Order Status = Đã phân công
  And Request chuyển Đang xử lý (đồng bộ)
  And không tạo được WO thứ hai cho cùng Request
When Technician tech1 đăng nhập và chọn Bắt đầu
Then WO Status = Đang xử lý
  And Request vẫn Đang xử lý
When Technician nhập kết quả và bấm Hoàn thành
Then WO Status = Hoàn thành
  And có lịch sử bảo trì
  And AI Prediction (nếu có) không tự tạo thêm WO
```

**Scenario B — Admin IoT mapping:**

```
Given Admin đã đăng nhập
  And còn tài sản chưa map
When Admin chọn tài sản chưa map và Tạo mapping (không nhập Device ID)
Then hệ thống tạo Device ID dạng SENSOR_{assetId}
  And tài sản biến mất khỏi dropdown “chưa map”
When Admin thử map lại cùng asset
Then BR-13
```

## 5. Completion gate

Sau mỗi vòng sửa bug/review: chạy lại **fresh**

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

Không dùng kết quả test cũ để khẳng định bản mới pass (Output #25).  
Đồng thời cập nhật cột **Result** trong `testcase.md` — **không** suy ra Pass từ riêng `65/0` automated.
