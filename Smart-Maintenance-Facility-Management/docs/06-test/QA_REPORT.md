# QA_REPORT

**Project:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**QA Owner:** Role QA/Release  
**Date:** 2026-09-10  
**Phase:** Pre-release QA package (**không gồm Release deploy**)

> Cấu trúc theo checklist Nhóm 10 / giáo trình QA: **scope · environment · result · known issues · risk · sign-off** (release blockers = 0 về chức năng).

---

## 1. Scope

**In scope**
- EPIC-01 → EPIC-06 đã implement
- Unit/Integration automated + manual E2E scenarios
- Bug reports (Output #28), Code review (Output #25), Test strategy/cases (Output #26–#27)
- Security/NFR smoke (Must)

**Out of scope (bài cuối — Output #29..#31)**
- Release checklist trên URL thật
- README/Runbook deploy, Release Notes production
- Docker/staging/production tag

---

## 2. Environment

| Component | Detail |
|---|---|
| Backend | ASP.NET Core 9 · `http://localhost:5031` · Development InMemory |
| Frontend | React + Vite · `http://localhost:5173` |
| Accounts | `admin` / `manager` / `requester` / `tech1` … · password `Due@2026` |
| Browser | Chrome/Edge |

---

## 3. Result

| Metric | Value |
|---|---|
| Automated tests | **65 passed / 0 failed** |
| Core TC mẫu (TC-01..10) | **10/10 Pass** |
| Extended TC (TC-11..47) | **Pass** (manual + auto map) |
| Critical/High bugs open | **0** |
| Code review High open | **0** |

**Artifacts**
- `test-strategy.md` (Output #26 + E2E #27)
- `testcase.md` (bảng ID\|Case\|Trace\|Expected\|Mode)
- `qa-verification.md`
- `bug-log.md` (Output #28)
- `code-review.md` (Output #25)
- `security-nfr.md`

---

## 4. Known issues

| ID | Severity | Status | Note |
|---|---|---|---|
| BUG-006 | Medium | Deferred | Chưa Playwright E2E FE |
| Review: audit WO transitions | Medium | Open Should | Final |
| NFR-05/06 | Should | Partial | IoT interval config / scale chưa chứng minh |

Không có known issue Critical/High mở.

---

## 5. Risk

| Risk | Level | Mitigation |
|---|---|---|
| InMemory mất data khi restart BE | Low | Seed cố định; ghi README |
| UI regression vì thiếu E2E auto | Medium | Manual scenario + Final Playwright |
| Chưa deploy thật | — | Release phase (No-Go production) |

**Overall (local demo / QA package):** Low–Medium — chấp nhận được.  
**Production release:** **No-Go** đến khi hoàn thành Output #29–#31.

---

## 6. Sign-off

| Role | Decision | Date |
|---|---|---|
| QA | **PASS — QA package (excluding Release)** | 2026-09-10 |
| Engineering | Pending counter-sign | |
| Product/BA | Pending counter-sign | |

**Release blockers (functional):** 0  
**Release blockers (ops/deploy):** N/A — chưa làm Release
