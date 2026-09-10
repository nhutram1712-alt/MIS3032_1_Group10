# QA Verification

**Project:** Smart Maintenance & Facility Management  
**Purpose:** Evidence “fresh verification” (giáo trình: không dùng kết quả test cũ sau khi sửa code)  
**Date:** 2026-09-10  
**Build:** BE `.run/out19` + FE Vite current

---

## 1. Fresh automated run

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

```text
Passed!  Failed: 0, Passed: 65, Skipped: 0, Total: 65
Duration: ~14 s
```

---

## 2. Manual suite (sau cập nhật code)

| Metric | Value |
|---|---|
| Total TC executed (TC-01..60 mapped) | 50 |
| Pass | 40 |
| Fail | 10 (~20%) |
| Evidence bảng | `testcase.md` §C |
| Open bugs | `bug-log.md` BUG-006..015 |

---

## 3. Map mẫu giáo trình → evidence

| Output | Artifact | Verified |
|---|---|---|
| #25 Code Review | `code-review.md` + fresh 65 pass + manual Fail ghi nhận | ✅ Conditional |
| #26 Test Strategy + 10 TC | `test-strategy.md` · `testcase.md` TC-01..10 | ✅ |
| #27 E2E Scenario | `test-strategy.md` §4 (queue + IoT map) | ✅ manual / ⏸ no Playwright |
| #28 Bug Report | `bug-log.md` (Verified + Open) | ✅ |
| #29–#31 Release | `docs/07-release/` | ⏸ Deferred |

---

## 4. API spot-check (QT + IoT mapping mới)

| Check | Result |
|---|---|
| QT-4 Admin immutable | 403 |
| QT-2 Requester role change | 400 |
| QT-3 Assign Admin via PUT | 400 |
| QT-1 Tech → FacilityManager | 200 |
| GET /api/roles/permissions | 200 |
| Assets / mappings count (seed) | assets≈78 · mapped≈71 · unmapped≈7 |
| POST mapping chỉ `assetId` (manual UI) | 201 + `SENSOR_{id}` — **chưa có automated** (TC-60 Fail) |

---

## 5. UI role matrix (smoke)

| Role | Account | Result | Note |
|---|---|---|---|
| Admin | `admin` | Pass core | Fail: route guard (TC-17), Unmap/Edit IoT |
| FacilityManager | `manager` | Pass core | Fail: Alerts/AI copy i18n |
| Technician | `tech1` | Pass | — |
| Requester | `requester` | Pass core | Fail: deep-link alerts/predictions |

---

## 6. Explicit unverified (Final)

- Staging/production URL smoke (Output #29)
- Playwright E2E automated
- npm audit formal
- NFR-05/06 đầy đủ

**Sign-off:** Fresh automated OK · Manual **Conditional** (~20% Fail mở) — 2026-09-10
