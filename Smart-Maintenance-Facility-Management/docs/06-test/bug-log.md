# Bug Log

**Project:** Smart Maintenance & Facility Management  
**Format:** theo giáo trình Output #28 — Bug Report mẫu  
**Owner:** QA  
**Last updated:** 2026-09-10 (sau vòng refactor Admin / IoT / WO queue)

Mỗi bug dùng đủ các mục: **Summary / Environment / Reproduction Steps / Expected vs Actual / Root Cause / Solution / Regression Risk / Test Plan / Evidence**.

---

## BUG-001 — Admin Overview hiển thị toàn 0

**Summary:** Màn Tổng quan Admin hiện 0 dù DB có seed.  
**Status:** Verified (đã fix trước đó)  
**Severity:** High · **Owner:** Engineering + QA

---

## BUG-002 — Seed Work Order gần như trống

**Summary:** Seed WO tối giản, khó demo lifecycle.  
**Status:** Verified (seed mở rộng + queue Submitted)  
**Severity:** Medium · **Owner:** Engineering

---

## BUG-003 — Privilege escalation qua PUT users

**Summary:** Admin đổi được role Admin/Requester / gán Admin.  
**Status:** Verified (QT-1..4 + tests)  
**Severity:** Critical · **Owner:** Engineering + QA

---

## BUG-004 — Nút Cập nhật status Asset Detail lệch layout

**Status:** Verified · **Severity:** Low

---

## BUG-005 — Work Order actions xếp dọc / chỉ hiện TechnicianId

**Status:** Verified (redesign + tên KT) · **Severity:** Medium

---

## BUG-006 — Chưa có E2E frontend tự động (Playwright/Cypress)

**Summary:**  
Critical UI journeys (login → queue phân công → Tech complete; Admin IoT map) chưa có E2E auto.

**Environment:** `src/frontend` · không có thư mục e2e.

**Reproduction Steps:**  
1. Kiểm tra `package.json` / e2e.  
2. Không thấy Playwright/Cypress.

**Expected:** E2E tối thiểu theo Output #27.  
**Actual:** Chỉ BE automated + manual UI.

**Root Cause:** Chưa vào DoD giai đoạn pre-release.  
**Solution:** Deferred Final — Playwright theo scenario `test-strategy.md`.  
**Regression Risk:** Medium.  
**Test Plan:** TC-58.  
**Status:** Open (Deferred Final) · **Severity:** Medium  
**Owner:** QA + Engineering

---

## BUG-007 — IoT Alerts hiện raw metric (`power_status`, `temperature`)

**Summary:**  
Cột Chỉ số trên màn IoT Alerts để nguyên snake_case tiếng Anh, không thân thiện người dùng VN.

**Environment:** FM · `/alerts` · Chrome.

**Reproduction Steps:**  
1. Login `manager`.  
2. Mở **IoT Alerts**.  
3. Xem cột Chỉ số.

**Expected:** Label kiểu “Trạng thái nguồn” / “Nhiệt độ”.  
**Actual:** `power_status`, `temperature`.

**Root Cause:** FE render `metricType` thô từ API, chưa map i18n.  
**Solution:** Bảng map metric → label Việt trên `AlertsPage`.  
**Regression Risk:** Low.  
**Test Plan:** TC-48.  
**Status:** Open · **Severity:** Medium  
**Owner:** Engineering

---

## BUG-008 — AI Risks hiện `High` / `Medium` chưa Việt hóa

**Summary:**  
Cột Rủi ro trên Predictions để nguyên tiếng Anh.

**Environment:** FM · `/predictions`.

**Reproduction Steps:**  
1. Login `manager` → **AI Risks**.  
2. Quan sát cột Rủi ro.

**Expected:** Cao / Trung bình / Thấp.  
**Actual:** `High` / `Medium`.

**Root Cause:** Chưa map `RiskLevels` sang copy UI.  
**Solution:** Map giống status pills các màn khác.  
**Regression Risk:** Low.  
**Test Plan:** TC-52.  
**Status:** Open · **Severity:** Low  
**Owner:** Engineering

---

## BUG-009 — Admin IoT thiếu Unmap / xóa mapping trên UI

**Summary:**  
Sau tạo liên kết, Admin không gỡ mapping trên UI (chỉ xem danh sách).

**Environment:** Admin · `/admin/iot`.

**Reproduction Steps:**  
1. Login `admin` → **IoT**.  
2. Tìm thao tác xóa/gỡ trên danh sách mapping.

**Expected:** Có Unmap (và BR-13 được tôn trọng khi map lại).  
**Actual:** Không có action gỡ.

**Root Cause:** MVP chỉ implement Create + List; thiếu DELETE API/UI.  
**Solution:** Thêm endpoint + nút xác nhận Unmap.  
**Regression Risk:** Medium (BR-13).  
**Test Plan:** TC-45.  
**Status:** Open · **Severity:** Medium  
**Owner:** Engineering

---

## BUG-010 — Admin IoT thiếu UI cập nhật Device ID (PUT đã có)

**Summary:**  
API `PUT /api/iot-mappings/{id}` tồn tại nhưng UI không cho sửa Device ID sau khi map (dù create đã auto-gen).

**Environment:** Admin · `/admin/iot`.

**Reproduction Steps:**  
1. Mở danh sách mapping.  
2. Không có edit Device ID.

**Expected:** Có sửa / đổi cảm biến theo US-05-02.  
**Actual:** Read-only list.

**Root Cause:** FE chưa wire `updateIotMapping`.  
**Solution:** Dialog sửa Device ID gọi PUT.  
**Regression Risk:** Medium.  
**Test Plan:** TC-46.  
**Status:** Open · **Severity:** Medium  
**Owner:** Engineering

---

## BUG-011 — Route `/admin/*` thiếu `RequireRoles` (chỉ check trong page)

**Summary:**  
Non-Admin vẫn vào được URL `/admin/users` / `/admin/iot`; page hiện thông báo nhưng không redirect sớm như Assets.

**Environment:** Login `manager` hoặc `requester` · gõ URL admin.

**Reproduction Steps:**  
1. Login `manager`.  
2. Mở `http://127.0.0.1:5173/admin/iot`.

**Expected:** Redirect `/` hoặc 403 shell (giống guard Assets).  
**Actual:** Vào được layout admin + message “Chỉ Admin…”.

**Root Cause:** `App.tsx` chưa bọc `RequireRoles(["Admin"])`.  
**Solution:** Thêm route guard.  
**Regression Risk:** Low–Medium (defense in depth; API đã chặn).  
**Test Plan:** TC-17.  
**Status:** Open · **Severity:** Medium  
**Owner:** Engineering

---

## BUG-012 — NFR-05 IoT sampling interval chưa cấu hình / chứng minh

**Summary:**  
Interval cảm biến dùng default seed/ingest; chưa config UI hoặc tài liệu đo được.

**Expected:** Config hoặc documented default có evidence.  
**Actual:** Partial / không verify được.  
**Status:** Open (Should / Final) · **Severity:** Low  
**Test Plan:** TC-59 · **Owner:** QA + Engineering

---

## BUG-013 — Requester deep-link `/alerts` hoặc `/predictions` không bị chặn route

**Summary:**  
Nav ẩn với Requester nhưng route vẫn mở; API 403 → trải nghiệm lỗi/thô.

**Environment:** `requester` · gõ URL.

**Reproduction Steps:**  
1. Login `requester`.  
2. Mở `/predictions` hoặc `/alerts`.

**Expected:** Redirect hoặc empty state rõ “không có quyền”.  
**Actual:** Trang render, lỗi API / bảng trống khó hiểu.

**Root Cause:** Route chưa `RequireRoles` cho FM/Tech.  
**Solution:** Guard + empty copy.  
**Regression Risk:** Low.  
**Test Plan:** TC-57.  
**Status:** Open · **Severity:** Medium  
**Owner:** Engineering

---

## BUG-014 — Copy status Work Order còn lẫn Anh–Việt

**Summary:**  
Một số chỗ (pill/cancel reason label) vẫn lẫn English với UI Việt sau redesign.

**Environment:** FM/Tech · Work Order.  
**Expected:** Copy Việt nhất quán.  
**Actual:** Còn sót English.  
**Status:** Open · **Severity:** Low  
**Test Plan:** TC-38 · **Owner:** Engineering

---

## BUG-015 — Thiếu automated test cho tạo IoT mapping chỉ `assetId` (auto Device ID)

**Summary:**  
Sau khi đổi Create mapping sang auto `SENSOR_{assetId}`, chưa có integration/unit case regression.

**Environment:** `SmartMaintenance.Tests`.  
**Expected:** Test POST `/api/iot-mappings` body `{ assetId }` → 201 + deviceId `SENSOR_*`.  
**Actual:** Không có case tương ứng (suite 65 vẫn green nhưng **không cover** thay đổi này).  
**Status:** Open · **Severity:** Medium  
**Test Plan:** TC-60 · **Owner:** QA + Engineering

---

## Summary

| Severity | Open | Verified | Deferred |
|---|---:|---:|---:|
| Critical | 0 | 1 (BUG-003) | 0 |
| High | 0 | 1 (BUG-001) | 0 |
| Medium | 6 | 2 | 1 (BUG-006) |
| Low | 3 | 1 | 0 |

**Open bugs (functional/UX):** 9 (+ 1 Deferred E2E)  
**Release blockers Critical/High:** 0  
**QA package:** Conditional Pass — ~20% case Fail còn mở (xem `testcase.md` / `QA_REPORT.md`)
