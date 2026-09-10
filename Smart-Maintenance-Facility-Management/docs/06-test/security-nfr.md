# Security & NFR Verification

**Project:** Smart Maintenance & Facility Management  
**Owner:** QA  
**Date:** 2026-09-10  
**Refs:** NFR-01..08 (`requirements.md`), api-contract, Output #25/#26 non-functional row

> Không gồm hardening production (TLS/WAF/secret manager cloud) — thuộc Release.

---

## 1. Security

| ID | Control | Evidence | Result |
|---|---|---|---|
| SEC-01 | JWT bắt buộc (trừ login + IoT ingest) | Controllers + middleware | Pass |
| SEC-02 | RBAC Role (NFR-01) | `[Authorize(Roles)]` + TC-04/07/08/10 | Pass |
| SEC-03 | Password hash (NFR-02) | BCrypt; không trả PasswordHash | Pass |
| SEC-04 | Logout blacklist | Auth logout + jti | Pass |
| SEC-05 | IoT API Key | `X-API-Key` · TC-36 | Pass |
| SEC-06 | Không privilege escalation qua PUT users | QT-2/3/4 · TC-07/08/13 | Pass |
| SEC-07 | Secrets không commit | `.env.example` only | Pass |
| SEC-08 | Error JSON `{ "error": "..." }` | Exception middleware | Pass |

---

## 2. NFR

| NFR | How verified | Result |
|---|---|---|
| NFR-01 RBAC | 401/403 automated + UI role matrix | Pass |
| NFR-02 Auth data | Hash + DTO | Pass |
| NFR-03 Consistency Request/WO/History | 409 duplicate WO; history after complete | Pass |
| NFR-04 Timestamps | createdAt / detectedAt / predictedAt | Pass |
| NFR-05 IoT interval | MVP default/sample — chưa UI config | Partial (Should) |
| NFR-06 Scale | Chưa load test | Deferred |
| NFR-07 UI theo Role | TC-12, TC-44..47 | Pass |
| NFR-08 Prediction link Asset + time | TC-09, TC-43 | Pass |

---

## 3. Non-functional smoke (theo Test Strategy mẫu)

| Check | Result |
|---|---|
| Basic latency local (login + list) | Pass smoke |
| Keyboard login/admin forms | Pass smoke |
| No secrets in frontend source | Pass spot-check |
| Audit events user create/update | Pass; WO audit = Should Final |

**Critical security open:** 0
