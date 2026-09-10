# QA_REPORT

**Project:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**QA Owner:** Role QA/Release  
**Date:** 2026-09-10  
**Phase:** Pre-release QA package (**không gồm Release deploy**)  
**Build:** FE latest + BE `out19` (Admin Users/IoT split, IoT auto Device ID, WO queue, sync request status)

> Cấu trúc theo checklist Nhóm 10 / giáo trình QA: **scope · environment · result · known issues · risk · sign-off**.

---

## 1. Scope

**In scope**
- EPIC-01 → EPIC-06 + đợt UX mới (Admin tách trang, IoT mapping unmapped-only + auto ID, FM merge Requests→WO queue, ẩn Assets với Requester)
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
| Backend | ASP.NET Core 9 · `http://127.0.0.1:5031` · Development InMemory · publish `.run/out19` |
| Frontend | React + Vite · `http://127.0.0.1:5173` |
| Accounts | `admin` / `manager` / `requester` / `tech1`… · password `Due@2026` |
| Seed highlight | ~78 assets · ~71 IoT mapped · **7 chưa map** · WO queue Submitted |
| Browser | Chrome/Edge |

---

## 3. Result

| Metric | Value |
|---|---|
| Manual + mapped suite (TC-01..60) | **Pass 40 / Fail 10 (~20% Fail)** |
| Core TC mẫu (TC-01..10) | **10/10 Pass** |
| Automated BE (`dotnet test`) | **65 passed / 0 failed** (fresh) |
| Critical/High bugs **open** | **0** |
| Medium/Low bugs **open** | **9** (+ 1 Deferred E2E) |
| Code review High open | **0** |

**Artifacts**
- `test-strategy.md` (Output #26 + E2E #27)
- `testcase.md` (bảng ID\|Case\|Trace\|Expected\|Mode + Result)
- `qa-verification.md`
- `bug-log.md` (Output #28)
- `code-review.md` (Output #25)
- `security-nfr.md`

---

## 4. Known issues (open)

| ID | Severity | Status | Note |
|---|---|---|---|
| BUG-007 | Medium | Open | Alerts metric raw English |
| BUG-008 | Low | Open | Predictions risk chưa Việt |
| BUG-009 | Medium | Open | IoT Unmap UI thiếu |
| BUG-010 | Medium | Open | IoT Update Device ID UI thiếu |
| BUG-011 | Medium | Open | Admin route thiếu RequireRoles |
| BUG-012 | Low | Open | NFR-05 interval |
| BUG-013 | Medium | Open | Requester deep-link alerts/predictions |
| BUG-014 | Low | Open | WO copy Anh–Việt lẫn |
| BUG-015 | Medium | Open | Thiếu auto-test mapping auto DeviceId |
| BUG-006 | Medium | Deferred | Playwright E2E FE |

Không có known issue Critical/High mở.

---

## 5. Risk

| Risk | Level | Mitigation |
|---|---|---|
| ~20% Fail manual chủ yếu UX/gap | Medium | Triage BUG-007..015 trước demo Formal |
| InMemory mất data khi restart BE | Low | Seed cố định (kèm 7 unmapped) |
| UI regression vì thiếu E2E auto | Medium | Manual scenario + Final Playwright |
| Auto Device ID chưa có regression test | Medium | BUG-015 trước merge lớn tiếp theo |
| Chưa deploy thật | — | Release phase (No-Go production) |

**Overall (local demo / QA package):** Medium — **Conditional Pass**.  
**Production release:** **No-Go** đến khi hoàn thành Output #29–#31 + đóng P0 Fail.

---

## 6. Sign-off

| Role | Decision | Date |
|---|---|---|
| QA | **CONDITIONAL PASS** — core Must Pass; ~20% Fail còn mở (Medium/Low) | 2026-09-10 |
| Engineering | Pending counter-sign / fix BUG-007..015 | |
| Product/BA | Pending counter-sign | |

**Release blockers (Critical/High functional):** 0  
**Release blockers (ops/deploy):** N/A — chưa làm Release  
**Khuyến nghị:** Không claim “100% pass”; báo cáo đúng **40/50 Pass**.
