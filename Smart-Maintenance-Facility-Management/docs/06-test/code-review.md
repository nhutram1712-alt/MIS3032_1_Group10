# Code Review

**Project:** Smart Maintenance & Facility Management  
**Format:** theo giáo trình Output #25 — Code Review mẫu  
**Scope:** Auth/Users QT · Assets · Requests/WO queue · Admin Users+IoT split · IoT auto DeviceId · AI  
**Date:** 2026-09-10  
**Reviewers:** QA (+ Engineering peer)

> Mẫu giáo trình: bảng **Severity | Finding | Action** + completion gate chạy lại fresh tests.

---

## Findings

| Severity | Finding | Action |
|---|---|---|
| High | `PUT /api/users/{id}` privilege escalation (QT) — lịch sử | Enforce QT-1..4 + FE rotate Tech↔FM. **Done.** |
| High | Overview Admin = 0 | Fix load Admin cards. **Done.** |
| Medium | Work Order UX stack dọc / chỉ ID KT | Redesign + queue phân công. **Done.** |
| Medium | Seed WO/assets mỏng | Mở rộng seed + 7 unmapped IoT. **Done.** |
| Medium | `/admin/*` thiếu `RequireRoles` — defense in depth kém | Bọc guard trong `App.tsx`. **Open — BUG-011.** |
| Medium | Create IoT mapping đổi contract (auto DeviceId) nhưng **không** thêm regression test | Thêm integration test POST chỉ `assetId`. **Open — BUG-015.** |
| Medium | Admin IoT thiếu Unmap + Update UI dù BR-13 / PUT API | Bổ sung FE (+ DELETE nếu cần). **Open — BUG-009/010.** |
| Medium | Route `/alerts` `/predictions` mở cho mọi role đã login | `RequireRoles` FM/Tech. **Open — BUG-013.** |
| Medium | Chưa Playwright E2E | Final. **Open — BUG-006.** |
| Medium | Audit WO transitions còn mỏng | Structured log Final. **Open (Should).** |
| Low | Alerts/Predictions copy English (`power_status`, `High`) | i18n map. **Open — BUG-007/008.** |
| Low | WO pill copy Anh–Việt lẫn | Chuẩn hóa. **Open — BUG-014.** |
| Low | Asset Detail nút status lệch | **Done.** |

---

## Module notes (ngắn)

| Module | Spec / Story | Review focus | Kết luận |
|---|---|---|---|
| Users + Admin Users page | US-01-03/04 | QT rules; bỏ matrix UI (MVP) | Pass (API permissions giữ) |
| Admin IoT | US-05-01/02 | Unmapped-only; auto DeviceId; thiếu Unmap/Edit | **Conditional** |
| Assets | US-02-* | Requester ẩn catalog | Pass |
| Requests / WO queue | US-03/04 · BR-07/09/14 | Sync status; Submitted-only create | Pass core; copy UX Fail nhỏ |
| IoT ingest/alerts | US-05 · BR-13 | API key; metric label UX | Ingest Pass; Alerts label Fail |
| AI Prediction | US-06 · BR-10/11 | No WO side-effect; tên asset không `#id` | Pass core; risk label Fail |

---

## Completion gate (bắt buộc theo giáo trình)

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

**Fresh result (2026-09-10):** `Passed 65 / Failed 0`  
**Manual Result đồng bộ:** `Pass 40 / Fail 10` (`testcase.md`) — **không** kết luận 100% Pass.

---

## Sign-off

| Role | Decision | Date |
|---|---|---|
| QA | **Conditional Pass** — không High/Critical mở; còn Medium Fail ~20% case | 2026-09-10 |
