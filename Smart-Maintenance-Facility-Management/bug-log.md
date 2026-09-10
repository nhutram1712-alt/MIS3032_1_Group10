# Bug Log

**Project:** Smart Maintenance & Facility Management  
**Format:** theo giáo trình Output #28 — Bug Report mẫu  
**Owner:** QA  
**Last updated:** 2026-09-10

Mỗi bug dùng đủ các mục: **Summary / Environment / Reproduction Steps / Expected vs Actual / Root Cause / Solution / Regression Risk / Test Plan / Evidence**.

---

## BUG-001 — Admin Overview hiển thị toàn 0

**Summary:**  
Màn Tổng quan khi đăng nhập Admin luôn hiện 0 tài sản / request / work order dù DB đã có seed.

**Environment:**  
Chrome · FE `localhost:5173` · BE Development InMemory · role Admin (`admin` / `Due@2026`).

**Reproduction Steps:**  
1. Đăng nhập `admin`.  
2. Mở **Tổng quan**.  
3. Quan sát 3 thẻ thống kê.

**Expected:**  
Admin thấy số liệu phù hợp quyền (ít nhất số tài sản / users / IoT mapping).  
**Actual:**  
Tất cả thẻ = 0 vì Overview không load assets cho Admin và dùng card Request/WO không thuộc quyền Admin.

**Root Cause:**  
Logic Overview bỏ qua Admin khi fetch assets; card set theo FacilityManager/Requester.

**Solution:**  
Load assets + users + IoT mappings cho Admin; đổi bộ card Tổng quan theo role Admin.

**Regression Risk:** Medium — đụng Overview multi-role.

**Test Plan:**  
- Manual: TC-44 Admin overview.  
- Spot-check API `GET /api/assets`, `GET /api/users`, `GET /api/iot-mappings` với token Admin.

**Evidence:**  
Screenshot trước/sau; commit OverviewPage.  
**Status:** Verified  
**Owner:** Engineering + QA

---

## BUG-002 — Seed Work Order gần như trống, khó demo/QA

**Summary:**  
Sau login Facility Manager, bảng Work Order chỉ có khoảng 1 bản ghi Cancelled, thiếu đa trạng thái để kiểm thử UI.

**Environment:**  
BE Development InMemory seed · role FacilityManager.

**Reproduction Steps:**  
1. Restart API (reseed).  
2. Login `manager`.  
3. Mở Work Order.

**Expected:**  
Nhiều WO Assigned / In Progress / Completed / Cancelled để cover thao tác.  
**Actual:**  
Dữ liệu tối giản (~1 WO).

**Root Cause:**  
`DbSeeder` chỉ seed mẫu hẹp.

**Solution:**  
Mở rộng seed: 15 assets, 10 requests, 8 work orders, thêm tech/requester.

**Regression Risk:** Low.

**Test Plan:**  
- Manual smoke TC-45.  
- Đếm `GET /api/work-orders` ≥ 8 sau restart.

**Evidence:**  
API count assets=15 requests=10 workOrders=8.  
**Status:** Verified  
**Owner:** Engineering

---

## BUG-003 — Admin có thể tự đổi role / gán Admin cho người khác (privilege escalation / self-lockout)

**Summary:**  
Trên UI Quản trị, Admin đổi được role của chính mình và có thể gán `Admin` cho user khác — vi phạm US-01-03/01-04 (QT-1..4).

**Environment:**  
Admin UI Users table · API `PUT /api/users/{id}`.

**Reproduction Steps:**  
1. Login `admin`.  
2. Mở Người dùng & IoT.  
3. Đổi role dòng `admin` sang Technician **hoặc** đổi `tech1` sang Admin.

**Expected:**  
- Target Admin → không sửa được (403).  
- Payload `role=Admin` → 400.  
- Chỉ luân chuyển Technician ↔ FacilityManager.  
**Actual:**  
Dropdown cho phép thao tác; BE chưa chặn đủ.

**Root Cause:**  
FE expose full role list; BE thiếu steel rules QT-2/3/4 và giới hạn QT-1.

**Solution:**  
BE enforce QT-1..4 + message đúng contract; FE chỉ rotate Tech↔FM; Admin/Requester hiển thị text; thêm `UserServiceTests`.

**Regression Risk:** High — authz.

**Test Plan:**  
- Automated: TC-07, TC-08, TC-13, TC-14.  
- Manual: TC-16.

**Evidence:**  
`UserServiceTests`; API 403/400; UI read-only.  
**Status:** Verified  
**Owner:** Engineering + QA

---

## BUG-004 — Nút Cập nhật status trên Asset Detail quá cao / lệch

**Summary:**  
Nút cập nhật status cao/rộng lệch so với nút Lưu thông tin, làm card Đổi trạng thái mất cân đối.

**Environment:**  
FM · Asset detail · Chrome.

**Reproduction Steps:**  
1. Login `manager`.  
2. Mở chi tiết một asset.  
3. Quan sát nút **Cập nhật status**.

**Expected:**  
Nút dài-thon, chiều cao gần với **Lưu thông tin**.  
**Actual:**  
Padding/chiều cao lệch.

**Root Cause:**  
CSS button full-grid stretch + padding lớn.

**Solution:**  
Class `btn-wide` / `btn-status` giảm padding dọc, giữ full width.

**Regression Risk:** Low.

**Test Plan:**  
Visual check TC-20.

**Evidence:**  
UI screenshot sau chỉnh.  
**Status:** Verified  
**Owner:** Engineering

---

## BUG-005 — Work Order actions xếp dọc, hàng cao lệch, chỉ hiện TechnicianId

**Summary:**  
Cột Thao tác xếp select + 2 nút dọc làm hàng cao không đều; cột kỹ thuật chỉ hiện số ID — UX kém chuyên nghiệp.

**Environment:**  
FM Work Order page.

**Reproduction Steps:**  
1. Login `manager`.  
2. Mở Work Order với nhiều bản ghi Assigned.  
3. Quan sát cột Thao tác / Kỹ thuật.

**Expected:**  
Action compact ngang; hiện tên technician; hàng cân hơn.  
**Actual:**  
Stack dọc, ID thô, lý do cancel làm hàng phình.

**Root Cause:**  
`action-stack` grid dọc; chưa resolve tên từ `/api/users?role=Technician`.

**Solution:**  
Redesign `WorkOrdersPage` (summary chips, inline actions, avatar/tên KT).

**Regression Risk:** Medium — đụng thao tác FM/Tech.

**Test Plan:**  
TC-29..33, TC-45, TC-46.

**Evidence:**  
UI sau redesign.  
**Status:** Verified  
**Owner:** Engineering + QA

---

## BUG-006 — Chưa có E2E frontend tự động (Playwright/Cypress)

**Summary:**  
Critical UI journeys chưa được bảo vệ bằng E2E automated; phụ thuộc manual.

**Environment:**  
Repo `src/frontend`.

**Reproduction Steps:**  
1. Kiểm tra `package.json` / thư mục e2e.  
2. Không thấy Playwright/Cypress suite.

**Expected:**  
Có E2E tối thiểu cho login + Request→WO (Final).  
**Actual:**  
Chỉ BE automated + manual UI.

**Root Cause:**  
Chưa nằm trong Definition of Done giai đoạn hiện tại (pre-release).

**Solution:**  
Deferred bài cuối — thêm Playwright theo scenario trong `test-strategy.md`.

**Regression Risk:** Medium nếu chỉ dựa manual lâu dài.

**Test Plan:**  
Final: automate Scenario Output #27.

**Evidence:**  
Ghi trong `test-strategy.md` / `QA_REPORT.md`.  
**Status:** Deferred (Final)  
**Owner:** QA + Engineering

---

## Summary

| Severity | Open | Verified | Deferred |
|---|---:|---:|---:|
| Critical | 0 | 1 (BUG-003) | 0 |
| High | 0 | 1 (BUG-001) | 0 |
| Medium | 0 | 2 | 1 (BUG-006) |
| Low | 0 | 1 | 0 |

**Release blockers (functional):** 0
