# Source Priority

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.1 (cập nhật: bổ sung 04-design, 05-technical, 06-test, 07-release)
> **Status:** Baseline
> **Owner:** AI/Vault
> **Vị trí file:** `docs/source-priority.md`

---

## 1. Mục đích

File này trả lời một câu hỏi duy nhất nhưng rất quan trọng:

> **Khi 2 tài liệu trong Vault nói khác nhau về cùng một điều, ta tin tài liệu nào?**

## 2. Nguyên tắc chung

1. Tài liệu có **Status: Baselined/Confirmed** luôn thắng tài liệu **Draft**.
2. Tài liệu càng gần quyết định nghiệp vụ cuối cùng càng ưu tiên cao hơn tài liệu mang tính khởi tạo ý tưởng.
3. `open-questions.md` **không phải nguồn sự thật** — chỉ là nơi lưu câu hỏi.
4. AI (chat, prompt output) luôn có độ ưu tiên **thấp nhất**.
5. **Mới ở v1.1:** tài liệu kỹ thuật/thiết kế/test (`05-technical`, `04-design`, `06-test`) chỉ có giá trị cho **chi tiết triển khai**, không được dùng để suy ra hoặc thay đổi **business rule/requirement** — nếu code/design "tiện" làm khác đi so với `business-rules.md`, `business-rules.md` vẫn thắng và code/design phải sửa lại.

---

## 3. Thứ tự ưu tiên (cao → thấp)

| Hạng | Nguồn | Vị trí | Status | Vai trò |
|---|---|---|---|---|
| 1 | `business-rules.md` + `requirements.md` | `02-vault/` | **Baselined** | Nguồn sự thật chính thức cho **rule + scope chức năng** |
| 2 | `decision-log.md` | `02-vault/` | Confirmed | Quyết định chính thức phát sinh **sau** baseline |
| 3 | `Interview.md` | `02-vault/` | Confirmed | Bằng chứng gốc — giải thích vì sao business-rules/requirements được chốt như vậy |
| 4 | `PRD.md`, `MVP-scope.md` | `03-product/` | Baseline/Draft | Tài liệu sản phẩm, phải nhất quán với requirements.md |
| 5 | `use-cases.md`, `user-stories.md`, `acceptance-criteria.md` | `03-product/` | Draft | Chi tiết hành vi/luồng |
| 6 | **`system-architecture.md`, `api-contract.md`, `data-requirements.md`, `iot-requirements.md`, `ai-requirements.md`, `specs/US-XX-XX-Spec.md`** | `05-technical/` | Draft | **(Mới)** Chi tiết kỹ thuật triển khai — chỉ ràng buộc *cách làm*, không được override business rule |
| 7 | **`Design.md`, `HANDOFF.md`, `Screen_Inventory.md`, `UX_Copy_Table.md`, `Prototype_finding.md`, `wireframes/`** | `04-design/` | Draft | **(Mới)** Thiết kế UI/UX — minh họa hành vi, không tự tạo business rule mới |
| 8 | **`test-strategy.md`, `testcase.md`, `QA_REPORT.md`, `security-nfr.md`, `code-review.md`, `bug-log.md`, `qa-verification.md`** | `06-test/` | Draft | **(Mới)** Bằng chứng kiểm thử — dùng để xác nhận implementation đúng/sai so với hạng 1-3, không phải nguồn định nghĩa rule |
| 9 | **`release-notes.md`** | `07-release/` | Draft | **(Mới)** Ghi nhận đã release gì — mô tả trạng thái thực tế, không phải nguồn yêu cầu |
| 10 | `glossary.md` | `02-vault/` | Baseline | Ưu tiên tuyệt đối riêng cho định nghĩa thuật ngữ |
| 11 | `customer-brief.md`, `problem-statement.md`, `stakeholders-personas.md` | `01-discovery/`, `02-vault/` | Baseline (context) | Bối cảnh ban đầu |
| 12 | `open-questions.md` | `02-vault/` | — | Không phải nguồn sự thật |
| 13 | AI chat / prompt output | — | — | Thấp nhất |

---

## 4. Quy tắc xử lý khi phát hiện xung đột

1. Xác định rank của từng nguồn theo bảng mục 3.
2. Nguồn rank thấp hơn (số nhỏ hơn) thắng.
3. Nếu cùng rank → ưu tiên theo ngày cập nhật gần nhất hoặc Confidence = High.
4. Nếu không giải quyết được → ghi vào `open-questions.md`, báo BA.
5. Mọi xung đột đã giải quyết → ghi vào `decision-log.md`.

**Quy tắc riêng cho tầng kỹ thuật/thiết kế/test (hạng 6-9, mới ở v1.1):** nếu code (`src/`) hoặc test (`06-test/`) phát hiện business rule không rõ ràng để implement/test được, đây **không phải** lý do để code tự quyết định theo cách thuận tiện nhất — phải tạo Open Question, báo BA/Facility Manager xác nhận trước, đúng như nguyên tắc CON-07 (`AI không tự động quyết định hoặc thực hiện bảo trì`) áp dụng tương tự cho quyết định kỹ thuật ảnh hưởng nghiệp vụ.

## 5. Ví dụ xung đột thật đã phát hiện trong Vault

### Ví dụ 1 — `open-questions.md` từng bị lỗi thời (đã xử lý ở DEC-07)

Đã giải quyết — xem `decision-log.md` DEC-07.

### Ví dụ 2 — Ghi chú cũ trong `customer-brief.md` về trạng thái Rejected (đã xử lý ở DEC-03/DEC-08)

Đã giải quyết.

### Ví dụ 3 — ⚠️ MỚI, CHƯA XỬ LÝ: `business-rules.md` BR-14 và BR-15 bị sai nội dung

`BR-14` hiện ghi: *"Maintenance Request sử dụng Hoạt động, Cảnh báo, Bảo trì, Ngừng dùng."* — đây thực chất là 4 giá trị của **Asset Status** (Operational, Warning, Maintenance, Out of Service theo `glossary.md`), **không phải** trạng thái của Maintenance Request.

Trạng thái đúng của Maintenance Request (theo `requirements.md` REQ-05, `Interview.md` Decision 2, `decision-log.md` DEC-02 — đều rank 1-3, cao hơn business-rules.md nếu có xung đột nội bộ) là: **Submitted, Pending, In Progress, Resolved, Closed, Rejected**.

`BR-15` cũng đổi "Risk = High" thành "Risk = Cao", không khớp giá trị chuẩn Low/Medium/High dùng xuyên suốt `requirements.md` REQ-12, `customer-brief.md` §7, `glossary.md`.

→ Đã ghi thành `DEC-09` trong `decision-log.md`, cần BA sửa trực tiếp `business-rules.md`.

## 6. Khi nào cần cập nhật file này

- Khi có tài liệu mới thêm vào Vault (đã xảy ra ở v1.1 khi thêm `04-design/05-technical/06-test/07-release`).
- Khi một tài liệu đổi Status.
- Khi phát hiện thêm 1 case xung đột mới trong lúc chạy Vault Q&A Benchmark.

## 7. Related Documents

- `decision-log.md` — DEC-09 (lỗi BR-14/BR-15 mới phát hiện)
- `vault-qa-benchmark.md` — nơi phát hiện xung đột thông qua test AI
- `business-rules.md`, `requirements.md` — nguồn sự thật rank 1
- `00-project-index.md` — bản đồ toàn bộ Vault đã mở rộng
