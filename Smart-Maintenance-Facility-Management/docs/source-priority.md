# Source Priority

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.0
> **Status:** Baseline
> **Owner:** AI/Vault
> **Vị trí file:** đặt ngang hàng với `01-discovery/`, `02-vault/`, `03-product/` (gốc vault)

---

## 1. Mục đích

File này trả lời một câu hỏi duy nhất nhưng rất quan trọng:

> **Khi 2 tài liệu trong Vault nói khác nhau về cùng một điều, ta tin tài liệu nào?**

Nếu không có quy tắc này, người (và AI) đọc Vault có thể lấy thông tin từ file cũ, file nháp, hoặc suy luận sai — dẫn đến requirement/business rule bị hiểu sai khi code hoặc test.

---

## 2. Nguyên tắc chung

1. Tài liệu có **Status: Baselined/Confirmed** luôn thắng tài liệu **Draft**.
2. Tài liệu càng **gần với quyết định cuối cùng của nghiệp vụ** (business rule, requirement đã chốt) càng có độ ưu tiên cao hơn tài liệu **mang tính khởi tạo ý tưởng** (customer brief, problem statement).
3. `open-questions.md` **không phải là nguồn sự thật** — nó chỉ là nơi lưu câu hỏi chưa/đã được giải quyết. Không được trích dẫn `open-questions.md` như một fact.
4. AI (chat, prompt output) luôn có độ ưu tiên **thấp nhất** — chỉ là ghi chú làm việc, phải được đối chiếu lại với Vault trước khi tin.

---

## 3. Thứ tự ưu tiên (cao → thấp)

| Hạng | Nguồn | Vị trí | Status | Vai trò |
|---|---|---|---|---|
| 1 | `business-rules.md` + `requirements.md` | `02-vault/` | **Baselined** | Nguồn sự thật chính thức cho **rule + scope chức năng** |
| 2 | `decision-log.md` | `02-vault/` | Confirmed | Quyết định chính thức phát sinh **sau** baseline (bổ sung/điều chỉnh baseline) |
| 3 | `Interview.md` | `02-vault/` | Confirmed | Bằng chứng gốc — giải thích **vì sao** business-rules/requirements được chốt như vậy |
| 4 | `PRD.md`, `MVP-scope.md` | `03-product/` | Baseline/Draft | Tài liệu sản phẩm, phải nhất quán với requirements.md |
| 5 | `use-cases.md`, `user-stories.md`, `acceptance-criteria.md` | `03-product/` | Draft | Chi tiết hành vi/luồng — ưu tiên cho **chi tiết UI/flow** mà requirements.md không mô tả tới mức đó |
| 6 | `glossary.md` | `02-vault/` | Baseline | Ưu tiên **tuyệt đối** riêng cho định nghĩa thuật ngữ (tên gọi, không phải scope) |
| 7 | `customer-brief.md`, `problem-statement.md`, `stakeholders-personas.md` | `01-discovery/`, `02-vault/` | Baseline (context) | Bối cảnh ban đầu — dùng khi không có tài liệu nào chi tiết hơn đề cập |
| 8 | `open-questions.md` | `02-vault/` | — | **Không phải nguồn sự thật.** Chỉ tra để biết điều gì **chưa chắc chắn** |
| 9 | AI chat / prompt output | — | — | Thấp nhất — chỉ là working note, luôn phải verify lại với (1)-(8) |

---

## 4. Quy tắc xử lý khi phát hiện xung đột

Khi 2 nguồn nói khác nhau về cùng 1 nội dung:

1. Xác định **rank** của từng nguồn theo bảng ở mục 3.
2. Nguồn có rank cao hơn (số nhỏ hơn) **thắng**.
3. Nếu 2 nguồn **cùng rank** (ví dụ business-rules.md vs requirements.md), ưu tiên theo:
   - Ngày cập nhật gần nhất, HOẶC
   - Nội dung nào có **Confidence = High** (theo cột Confidence trong business-rules.md/requirements.md).
4. Nếu vẫn không giải quyết được → **không tự suy luận/chọn đại**. Ghi lại thành một dòng mới trong `open-questions.md` và báo cho BA xử lý.
5. Mọi lần xung đột được phát hiện **và đã giải quyết** phải được ghi lại trong `decision-log.md` kèm lý do chọn nguồn nào.

---

## 5. Ví dụ xung đột thật đã phát hiện trong Vault (dùng làm case mẫu)

### Ví dụ 1 — `open-questions.md` bị lỗi thời

`open-questions.md` liệt kê **Q-03** ("Maintenance Request cần những trạng thái chính thức nào?") với **Status = Open**.

Nhưng `requirements.md` (REQ-05) và `business-rules.md` (BR-14) — cả hai đều **Baselined, Confidence: High** — đã định nghĩa rõ 6 trạng thái: `Submitted, Pending, In Progress, Resolved, Closed, Rejected`. `Interview.md` (Decision 2, CONFIRMED) cũng xác nhận điều này.

→ Theo Source Priority: **business-rules.md/requirements.md (rank 1) thắng open-questions.md (rank 8).** Câu trả lời đúng là **đã có 6 trạng thái chính thức**, không phải "chưa có câu trả lời". `open-questions.md` đang bị lỗi thời (Status lẽ ra phải là `Resolved`).

> Tình huống tương tự lặp lại ở **Q-01, Q-02, Q-04, Q-05, Q-06, Q-07, Q-08, Q-09 (một phần), Q-19, Q-20** — tất cả đều đã có Interview Decision CONFIRMED nhưng cột Status trong `open-questions.md` vẫn ghi "Open". Đây là phát hiện thật từ quá trình chạy Vault Q&A Benchmark (xem `vault-qa-benchmark.md` mục Round 1) và đã được ghi thành `decision-log.md` → **DEC-07**.

### Ví dụ 2 — Ghi chú cũ trong `customer-brief.md`

`customer-brief.md` có 1 dòng cảnh báo: *"việc có thêm trạng thái `Rejected` [cho Work Order] là Open Question và cần được xác nhận trước khi thay đổi baseline."*

Nhưng `Interview.md` (Decision 3, CONFIRMED) đã chốt: **"Rejected" là một action của Technician, KHÔNG phải trạng thái chính thức của Work Order** (Work Order chỉ có 4 trạng thái: `Assigned, In Progress, Completed, Cancelled` theo BR-17).

→ `Interview.md` (rank 3, Confirmed, mới hơn) thắng ghi chú cũ trong `customer-brief.md` (rank 7). Ghi chú trong customer-brief.md chỉ còn giá trị lịch sử, không còn là câu hỏi mở.

### Ví dụ 3 — AI không được suy luận ngoài Vault

Nếu hỏi *"Một Asset có thể gắn nhiều IoT Sensor không?"*, AI **không được** trả lời "Có thể, vì thực tế phòng lớn cần nhiều cảm biến" — đó là suy luận ngoài Vault. Câu trả lời đúng phải bám `BR-13` (Baselined, High): **trong MVP, một Asset chỉ mapping với một IoT Device/Sensor.**

---

## 6. Khi nào cần cập nhật file này

- Khi có tài liệu mới được thêm vào Vault (VD: Design/Wireframe, API Contract).
- Khi một tài liệu đổi Status (VD: PRD.md từ Draft → Baseline).
- Khi phát hiện thêm 1 case xung đột mới trong lúc chạy Vault Q&A Benchmark.

## 7. Related Documents

- `decision-log.md` — nơi ghi các xung đột đã được giải quyết chính thức
- `vault-qa-benchmark.md` — nơi phát hiện các xung đột thông qua việc test AI
- `business-rules.md`, `requirements.md` — nguồn sự thật rank 1
