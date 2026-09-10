# Code Review

**Project:** Smart Maintenance & Facility Management  
**Format:** theo giáo trình Output #25 — Code Review mẫu  
**Scope:** PR/modules MVP (Auth, Users QT rules, Assets, Requests, WO, IoT, AI)  
**Date:** 2026-09-10  
**Reviewers:** QA (+ Engineering peer)

> Mẫu giáo trình: bảng **Severity | Finding | Action** + completion gate chạy lại fresh tests.

---

## Findings

| Severity | Finding | Action |
|---|---|---|
| High | `PUT /api/users/{id}` trước đây cho phép đổi role Admin/Requester và gán Admin — nguy cơ self-lockout / privilege escalation (US-01-03/01-04). | Enforce QT-1..4 trong `UserService`; FE chỉ rotate Technician↔FacilityManager; thêm unit tests. **Done.** |
| High | Overview Admin không fetch assets → số liệu 0, dễ hiểu nhầm hệ thống “không có data”. | Sửa `OverviewPage` load assets/users/mappings cho Admin. **Done.** |
| Medium | Work Order UI stack action dọc + chỉ hiện `technicianId` — khó thao tác/review UX. | Redesign inline actions + resolve tên technician. **Done.** |
| Medium | Seed WO quá ít — che khuất defect UI/lifecycle khi demo. | Mở rộng `DbSeeder` (15 assets / 10 requests / 8 WO). **Done.** |
| Medium | Chưa có test E2E frontend cho critical journey Request→WO. | Ghi nợ Final (Playwright); giữ manual E2E scenario trong `test-strategy.md`. **Open (Deferred).** |
| Medium | Audit/logging mới cover user create/update; thiếu audit WO status transitions. | Bổ sung structured log cho WO patch ở Final. **Open (Should).** |
| Low | Nút Cập nhật status Asset Detail cao lệch so với Lưu thông tin. | Giảm padding (`btn-status`). **Done.** |
| Low | Một số trang còn hard-code copy tiếng Anh/Việt lẫn (Cancelled pill). | Chuẩn hóa copy UX ở vòng polish Final. **Open (Could).** |

---

## Module notes (ngắn)

| Module | Spec / Story | Review focus | Kết luận |
|---|---|---|---|
| Users + Roles permissions | US-01-03/01-04 | AuthZ steel rules, error messages | Pass sau fix High |
| Assets | US-02-* | Validation type/status, FM-only writes | Pass |
| Requests / WO | US-03/04 · BR-07/09/14/17 | Lifecycle, 409 duplicate, tech ownership | Pass |
| IoT | US-05 · BR-13 | API key ingest, one mapping/asset | Pass |
| AI Prediction | US-06 · BR-10/11 | No WO side-effect, RBAC, timestamps | Pass |

---

## Completion gate (bắt buộc theo giáo trình)

Sau khi sửa review, chạy lại fresh — **không** dùng output kiểm tra cũ:

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

**Fresh result (2026-09-10):** `Passed 65 / Failed 0`

---

## Sign-off

| Role | Decision | Date |
|---|---|---|
| QA | **Conditional Pass** — không còn High/Critical mở; E2E FE + audit WO để Final | 2026-09-10 |
