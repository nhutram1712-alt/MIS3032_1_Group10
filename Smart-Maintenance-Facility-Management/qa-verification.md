# QA Verification

**Project:** Smart Maintenance & Facility Management  
**Purpose:** Evidence “fresh verification” (giáo trình: không dùng kết quả test cũ sau khi sửa code)  
**Date:** 2026-09-10

---

## 1. Fresh automated run

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

```text
Passed!  Failed: 0, Passed: 65, Skipped: 0, Total: 65
```

---

## 2. Map mẫu giáo trình → evidence

| Output | Artifact | Verified |
|---|---|---|
| #25 Code Review | `code-review.md` + fresh 65 pass | ✅ |
| #26 Test Strategy + 10 TC | `test-strategy.md` · `testcase.md` TC-01..10 | ✅ |
| #27 E2E Scenario | `test-strategy.md` §4 | ✅ (manual) |
| #28 Bug Report | `bug-log.md` BUG-001..006 | ✅ |
| #29–#31 Release | `docs/07-release/` | ⏸ Deferred |

---

## 3. API spot-check (sau fix QT rules)

| Check | Result |
|---|---|
| QT-4 Admin immutable | 403 |
| QT-2 Requester role change | 400 |
| QT-3 Assign Admin via PUT | 400 |
| QT-1 Tech → FacilityManager | 200 |
| GET /api/roles/permissions | 200 |

---

## 4. UI role matrix

| Role | Account | Result |
|---|---|---|
| Admin | `admin` | Pass |
| FacilityManager | `manager` | Pass |
| Technician | `tech1` | Pass |
| Requester | `requester` | Pass |

---

## 5. Explicit unverified (Final)

- Staging/production URL smoke (Output #29)
- Playwright E2E automated
- npm audit formal

**Sign-off:** Verified for QA package (pre-release) — 2026-09-10
