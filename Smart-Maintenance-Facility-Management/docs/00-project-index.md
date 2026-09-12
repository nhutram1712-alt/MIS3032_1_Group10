# Project Index

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Owner:** AI/Vault
> **Version:** 1.0
> **Cập nhật lần cuối:** 2026-09-12

---

## 1. Mục đích

File này là "bản đồ" của toàn bộ repo: liệt kê **mọi file/thư mục hiện có**, dùng để làm gì, và trạng thái (current / draft / trống / cần sửa). Đây là điểm bắt đầu cho bất kỳ ai (người hoặc AI) muốn tra cứu Vault.

**Quy ước trạng thái:**

| Trạng thái | Ý nghĩa |
|---|---|
| ✅ Current | Nội dung đầy đủ, đã baseline, dùng được ngay |
| 🟡 Draft | Có nội dung nhưng chưa baseline/còn thay đổi |
| ⚠️ Cần sửa | Đã phát hiện lỗi/mâu thuẫn, đang chờ xử lý |
| ❌ Trống | Chỉ có khung/template, chưa có nội dung thật |

---

## 2. `docs/01-discovery/` — Bối cảnh & người dùng

| File | Mô tả | Trạng thái |
|---|---|---|
| `problem-statement.md` | Vấn đề cốt lõi cần giải quyết | ✅ Current |
| `stakeholders-personas.md` | 4 persona: Requester, Technician, Facility Manager, Admin | ✅ Current |

## 3. `docs/02-vault/` — Nguồn sự thật chính (business rule, requirement, quyết định)

| File | Mô tả | Trạng thái |
|---|---|---|
| `Interview.md` | 6 quyết định CONFIRMED từ phỏng vấn mô phỏng | ✅ Current |
| `business-rules.md` | 18 Business Rules (BR-01→BR-18) | ⚠️ **Cần sửa** — BR-14, BR-15 hiện sai nội dung (xem `decision-log.md` DEC-09) |
| `customer-brief.md` | Mô tả nhu cầu hệ thống ở mức business | ✅ Current |
| `decision-log.md` | DEC-01→DEC-09 — mọi quyết định chính thức | ✅ Current |
| `glossary.md` | 20 thuật ngữ domain | ✅ Current |
| `open-questions.md` | 20 câu hỏi mở, 12/20 đã Resolved | ✅ Current |
| `requirements.md` | 30 FR, 8 NFR, 8 Constraint, 8 Assumption | ✅ Current |
| `vault-qa-benchmark.md` | 20 câu hỏi kiểm tra chất lượng Vault | 🟡 Draft — kết quả Round 1/2 hiện là **mô phỏng**, cần chạy thật với AI và thay bằng kết quả thật |

## 4. `docs/03-product/` — Sản phẩm & phạm vi

| File | Mô tả | Trạng thái |
|---|---|---|
| `MVP-scope.md` | Must/Should/Could/Out of Scope | ✅ Current |
| `PRD.md` | Product Requirements Document | ✅ Current |
| `acceptance-criteria.md` | AC cho các User Story | ✅ Current |
| `use-cases.md` | Use case chi tiết | ✅ Current |
| `user-stories.md` | User Story theo Epic (US-01→US-06) | ✅ Current |

## 5. `docs/04-design/` — Thiết kế UX/UI

| File | Mô tả | Trạng thái |
|---|---|---|
| `Design.md` | Design system | ✅ Current (do UX/UI phụ trách) |
| `HANDOFF.md` | Handoff cho Engineering | ✅ Current |
| `Prototype_finding.md` | Kết quả usability test | ✅ Current |
| `Screen_Inventory.md` | Danh sách màn hình + state | ✅ Current |
| `UX_Copy_Table.md` | Bảng UX copy | ✅ Current |
| `wireframes/` | Wireframe/prototype brief | ✅ Current |

## 6. `docs/05-technical/` — Kiến trúc & kỹ thuật

| File | Mô tả | Trạng thái |
|---|---|---|
| `system-architecture.md` | Kiến trúc hệ thống | ✅ Current (do Engineering phụ trách) |
| `api-contract.md` | API contract | ✅ Current |
| `data-requirements.md` | Data model | ✅ Current |
| `iot-requirements.md` | Yêu cầu kỹ thuật IoT | ✅ Current |
| `ai-requirements.md` | Yêu cầu kỹ thuật AI Prediction | ✅ Current |
| `specs/US-XX-XX-Spec.md` (23 file) | Story Spec chi tiết cho từng User Story | ✅ Current |

## 7. `docs/06-test/` — Kiểm thử

| File | Mô tả | Trạng thái |
|---|---|---|
| `test-strategy.md` | Chiến lược test | ✅ Current (do QA/Release phụ trách) |
| `testcase.md` | Test case chi tiết | ✅ Current |
| `bug-log.md` | Nhật ký bug | ✅ Current |
| `code-review.md` | Kết quả code review | ✅ Current |
| `security-nfr.md` | Kiểm tra Security/NFR | ✅ Current |
| `QA_REPORT.md` | Báo cáo QA tổng hợp | ✅ Current |
| `qa-verification.md` | Xác nhận QA | ✅ Current |

## 8. `docs/07-release/` — Phát hành

| File | Mô tả | Trạng thái |
|---|---|---|
| `release-notes.md` | Release notes | ✅ Current (do QA/Release phụ trách) |

## 9. `docs/logs/` — Nhật ký

| File/Folder | Mô tả | Trạng thái |
|---|---|---|
| `ai-usage-log.md` | Nhật ký dùng AI | 🟡 Draft — 7 entry hiện là **mô phỏng**, cần thay bằng log thật của cả nhóm |
| `requirement-review.md` | Kết quả AI-assisted requirement review | ✅ Current (do BA phụ trách, dùng prompt "Requirement Reviewer") |
| `interview-notes/` | Ghi chép phỏng vấn thật | ❌ Trống — chỉ có README hướng dẫn định dạng, chưa có bản ghi nào |
| `meeting-notes/` | Biên bản họp | ❌ Trống — chỉ có README hướng dẫn định dạng, chưa có biên bản nào |

## 10. Meta files (gốc `docs/` và gốc repo)

| File | Mô tả | Trạng thái |
|---|---|---|
| `docs/source-priority.md` | Quy tắc ưu tiên nguồn khi Vault xung đột | ✅ Current — v1.1, đã cập nhật bao phủ toàn bộ `04-design/05-technical/06-test/07-release` |
| `docs/00-project-index.md` | Chính là file này | ✅ Current |
| `README.md` (gốc) | Giới thiệu repo | ✅ Current |

## 11. `src/` và `tests/` — Mã nguồn (không thuộc Vault, chỉ liệt kê để tham chiếu)

| Thư mục | Mô tả |
|---|---|
| `src/backend/` | ASP.NET Core API (Controllers, Application, Domain, Infrastructure, Tests) |
| `src/frontend/` | React + TypeScript + Vite |
| `src/ai/` | AI microservice (Python) cho AI Prediction |
| `tests/` | Test scaffold cấp repo |

---

## 12. Việc còn tồn đọng (theo dõi bởi AI/Vault)

| # | Việc | Ưu tiên |
|---|---|---|
| 1 | Sửa `BR-14`, `BR-15` trong `business-rules.md` theo `DEC-09` | 🔴 Cao |
| 2 | Chạy `vault-qa-benchmark.md` thật, thay kết quả mô phỏng | 🔴 Cao |
| 3 | Bổ sung entry thật vào `ai-usage-log.md` | 🟡 Trung bình |
| 4 | Bổ sung câu hỏi benchmark mới cho nội dung `04-design/05-technical/06-test/07-release` (Vault đã mở rộng nhiều so với Bài 1) | 🟡 Trung bình |
| 5 | Khuyến khích BA/PM điền `logs/interview-notes/` và `logs/meeting-notes/` nếu có phỏng vấn/họp thật | 🟢 Thấp |

## 13. Related Documents

- `source-priority.md` — quy tắc ưu tiên nguồn
- `decision-log.md` — mọi quyết định chính thức
- `vault-qa-benchmark.md` — kết quả kiểm tra chất lượng Vault
