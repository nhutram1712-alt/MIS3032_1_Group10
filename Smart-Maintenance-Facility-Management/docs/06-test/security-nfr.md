# Security & NFR Verification

**Project:** Smart Maintenance & Facility Management  
**Owner:** QA  
**Date:** 2026-09-10  
**Refs:** NFR-01..08, api-contract, Output #25/#26 non-functional row  
**Build:** sau cập nhật Admin/IoT/WO

> Không gồm hardening production (TLS/WAF/secret manager cloud) — thuộc Release.

---

## 1. Security

| ID | Control | Evidence | Result |
|---|---|---|---|
| SEC-01 | JWT bắt buộc (trừ login + IoT ingest) | Controllers + middleware | Pass |
| SEC-02 | RBAC Role (NFR-01) | `[Authorize(Roles)]` + TC-04/07/08/10 | Pass |
| SEC-03 | Password hash (NFR-02) | BCrypt; không trả PasswordHash | Pass |
| SEC-04 | Logout blacklist | Auth logout + jti | Pass |
| SEC-05 | IoT API Key | `X-API-Key` · TC-40 | Pass |
| SEC-06 | Không privilege escalation qua PUT users | QT-2/3/4 · TC-07/08/13 | Pass |
| SEC-07 | Secrets không commit | `.env.example` only | Pass |
| SEC-08 | Error JSON `{ "error": "..." }` | Exception middleware | Pass |
| SEC-09 | FE route guard Admin / Alerts / Predictions | Defense in depth | **Fail** (BUG-011, BUG-013) — API vẫn chặn |

---

## 2. NFR

| NFR | How verified | Result |
|---|---|---|
| NFR-01 RBAC | 401/403 automated + UI role matrix | Pass (API); Partial FE deep-link |
| NFR-02 Auth data | Hash + DTO | Pass |
| NFR-03 Consistency Request/WO/History | 409 duplicate WO; sync status; history after complete | Pass |
| NFR-04 Timestamps | createdAt / detectedAt / predictedAt | Pass |
| NFR-05 IoT interval | MVP default — chưa UI config | **Fail / Partial** (TC-59 · BUG-012) |
| NFR-06 Scale | Chưa load test | Deferred |
| NFR-07 UI theo Role | TC-12, TC-22, TC-53..56; i18n Fail TC-38/48/52 | **Partial** (~ Fail UX) |
| NFR-08 Prediction link Asset + time | TC-09, TC-51; tên không `#id` | Pass |

---

## 3. Non-functional smoke (theo Test Strategy mẫu)

| Check | Result |
|---|---|
| Basic latency local (login + list) | Pass smoke |
| Keyboard login/admin forms | Pass smoke |
| No secrets in frontend source | Pass spot-check |
| Audit events user create/update | Pass; WO audit = Should Final |
| Auto Device ID không lộ secret | Pass (pattern `SENSOR_{id}`) |

**Critical security open:** 0  
**Medium FE guard gaps:** 2 (BUG-011, BUG-013) — không bypass được API privileged
