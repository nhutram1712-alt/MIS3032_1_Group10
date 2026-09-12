# Decision Log

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.0
> **Vị trí file:** `02-vault/decision-log.md`

---

## 1. Mục đích

File này là nơi **duy nhất** ghi lại mọi quyết định chính thức của dự án — không để quyết định "trôi" trong chat, trong Interview, hay trong đầu 1 người.

Cả `business-rules.md` và `requirements.md` đều đã trỏ tới file này ở mục "Related Documents" — nghĩa là 2 file đó **giả định** file này tồn tại và có nội dung. Dưới đây là nội dung chính thức, tổng hợp từ `Interview.md` (Decision 1-6, đã CONFIRMED) và từ quá trình chạy `vault-qa-benchmark.md` (DEC-07, DEC-08).

## 2. Quy ước

| Trường | Ý nghĩa |
|---|---|
| **Decision ID** | Mã duy nhất, không đổi khi nội dung được làm rõ thêm |
| **Related** | REQ/BR/NFR/OQ liên quan |
| **Status** | CONFIRMED / SUPERSEDED / PENDING |

---

## 3. Danh sách quyết định

### DEC-01 — Phạm vi Asset và quyền xem Asset

- **Ngày:** 2026-09-01
- **Nguồn:** `Interview.md` Decision 1
- **Liên quan:** Q-01, Q-18 · REQ-02, REQ-03, REQ-06, REQ-07, REQ-08 · BR-01→BR-04 · NFR-01, NFR-04, NFR-07
- **Quyết định:** Requester chỉ xem Asset thuộc phòng/khu vực được phép sử dụng, không xem toàn bộ hệ thống. Asset MVP chỉ quản lý Asset ID, Name, Type, Location, Status. Phạm vi Asset: Wi-Fi, Air Conditioner, Projector, Light, Fan.
- **Lý do:** Giới hạn theo khu vực đảm bảo phân quyền đúng và giữ MVP đơn giản.
- **Status:** CONFIRMED

### DEC-02 — Maintenance Request

- **Ngày:** 2026-09-01
- **Nguồn:** `Interview.md` Decision 2
- **Liên quan:** Q-02, Q-03, Q-05, Q-06, Q-08 · REQ-04, REQ-05, REQ-13, REQ-14, REQ-22, REQ-23 · BR-05, BR-06, BR-14, BR-18
- **Quyết định:** Ảnh/video không bắt buộc. Request dùng 6 trạng thái: Submitted → Pending → In Progress → Resolved → Closed, và có thể Rejected. Facility Manager không cần approve riêng, có thể tạo WO trực tiếp. 1 Request chỉ tạo tối đa 1 WO. Request chỉ Closed sau khi FM xác nhận kết quả.
- **Lý do:** Giảm bước trung gian nhưng vẫn giữ FM kiểm soát kết quả cuối.
- **Status:** CONFIRMED

### DEC-03 — Work Order và Technician

- **Ngày:** 2026-09-01
- **Nguồn:** `Interview.md` Decision 3
- **Liên quan:** Q-04, Q-07 · REQ-14→REQ-17, REQ-20→REQ-22 · BR-06→BR-09, BR-17
- **Quyết định:** Work Order chỉ có 4 trạng thái chính thức: Assigned → In Progress → Completed → Cancelled. Technician có thể **từ chối** Work Order kèm lý do, nhưng "Rejected" là một **action**, KHÔNG phải trạng thái chính thức của WO.
- **Lý do:** Tách rõ trách nhiệm FM/Technician, đồng thời xử lý được trường hợp Technician không thể thực hiện.
- **Status:** CONFIRMED
- **Lưu ý:** `customer-brief.md` có ghi chú cũ nghi ngờ về việc thêm trạng thái Rejected — ghi chú này đã lỗi thời kể từ quyết định này (xem `source-priority.md` mục 5, Ví dụ 2).

### DEC-04 — IoT Monitoring và Alert

- **Ngày:** 2026-09-01
- **Nguồn:** `Interview.md` Decision 4
- **Liên quan:** Q-09, Q-10, Q-15, Q-16, Q-19 · REQ-09, REQ-10, REQ-26→REQ-28, REQ-30 · BR-12, BR-13
- **Quyết định:** IoT Data thu thập 5 phút/lần. Mỗi Asset chỉ mapping 1 IoT Device/Sensor. IoT Alert dùng threshold cấu hình đơn giản theo Asset Type (chưa cần anomaly detection phức tạp). FM nhận Notification khi có IoT Alert nghiêm trọng.
- **Lý do:** Đủ để chứng minh khả năng giám sát IoT trong MVP mà không tăng độ phức tạp kỹ thuật.
- **Status:** CONFIRMED
- **Lưu ý còn mở:** Giá trị threshold cụ thể theo từng Asset Type **chưa được chốt** — vẫn là Open Question thật sự (Q-10), không được tự suy đoán số liệu (xem `vault-qa-benchmark.md` câu D1).

### DEC-05 — AI Predictive Maintenance

- **Ngày:** 2026-09-01
- **Nguồn:** `Interview.md` Decision 5
- **Liên quan:** Q-11→Q-14, Q-19 · REQ-11, REQ-12, REQ-29, REQ-30 · BR-10, BR-11, BR-15
- **Quyết định:** AI Prediction chỉ dự đoán khả năng cần bảo trì trong **7 ngày tới**, dựa trên IoT Data + Maintenance History. Kết quả hiển thị Maintenance Risk: Low/Medium/High. Có thể dùng dữ liệu mẫu/giả lập nếu thiếu dữ liệu thật. AI chỉ hỗ trợ — FM quyết định cuối. FM nhận Notification khi Risk = High.
- **Lý do:** Đơn giản, dễ giải thích, dễ kiểm chứng trong phạm vi MVP.
- **Status:** CONFIRMED

### DEC-06 — Authentication và phân quyền

- **Ngày:** 2026-09-01
- **Nguồn:** `Interview.md` Decision 6
- **Liên quan:** Q-20 · REQ-01, REQ-24, REQ-25 · NFR-01, NFR-02
- **Quyết định:** MVP dùng tài khoản riêng của hệ thống, không tích hợp SSO. Áp dụng RBAC với 4 role: Requester, Technician, Facility Manager, Admin.
- **Lý do:** Giảm phụ thuộc hệ thống bên ngoài, phù hợp phạm vi MVP.
- **Status:** CONFIRMED

---

### DEC-07 — Cập nhật Status của Open Questions đã được giải quyết

- **Ngày:** 2026-09-09
- **Nguồn:** Phát hiện từ `vault-qa-benchmark.md` (Round 1, câu C1 & C2)
- **Liên quan:** Q-01, Q-02, Q-03, Q-04, Q-05, Q-06, Q-07, Q-08, Q-19, Q-20
- **Quyết định:** 10 Open Question ở trên thực chất **đã được trả lời chính thức** thông qua DEC-01 → DEC-06 và đã phản ánh vào `business-rules.md`/`requirements.md`, nhưng cột **Status** trong `open-questions.md` vẫn ghi sai là "Open". Quyết định: đổi Status của 10 câu này thành **"Resolved"**, kèm ghi chú "→ xem DEC-0x tương ứng".
- **Lý do:** Nếu không sửa, người đọc/AI sau này dễ hiểu nhầm là các nội dung này chưa có câu trả lời, dẫn tới trả lời sai khi tra Vault (đã xảy ra thật ở Round 1 của benchmark).
- **Status:** CONFIRMED — cần BA/AI-Vault thực hiện sửa trực tiếp trên `open-questions.md`.

### DEC-08 — Áp dụng chính thức Source Priority cho toàn bộ Vault

- **Ngày:** 2026-09-09
- **Nguồn:** `source-priority.md` v1.0
- **Liên quan:** Toàn bộ Vault
- **Quyết định:** Từ thời điểm này, mọi xung đột thông tin giữa các file trong Vault phải được giải quyết theo đúng thứ tự ưu tiên trong `source-priority.md` (business-rules.md/requirements.md → decision-log.md → Interview.md → PRD/MVP-scope → use-case/story/AC → glossary → discovery docs → open-questions.md → AI output).
- **Lý do:** Tránh tình trạng mỗi người/mỗi lần hỏi AI lại chọn nguồn khác nhau khi có mâu thuẫn (đã xảy ra ở Round 1 benchmark, sửa ở Round 2).
- **Status:** CONFIRMED

### DEC-09 — Sửa lỗi nội dung sai trong BR-14 và BR-15

- **Ngày:** 2026-09-12
- **Nguồn:** Phát hiện trong lúc AI/Vault rà soát `business-rules.md` khi cập nhật `source-priority.md` v1.1
- **Liên quan:** BR-14, BR-15, REQ-05, REQ-12, Interview.md Decision 2, DEC-02
- **Vấn đề phát hiện:** `BR-14` hiện ghi *"Maintenance Request sử dụng Hoạt động, Cảnh báo, Bảo trì, Ngừng dùng"* — đây là 4 giá trị của **Asset Status** (Operational, Warning, Maintenance, Out of Service theo `glossary.md`), bị nhầm với trạng thái của **Maintenance Request**. `BR-15` cũng đổi "Risk = High" thành "Risk = Cao", không khớp giá trị chuẩn Low/Medium/High dùng trong `requirements.md` REQ-12.
- **Quyết định:** Khôi phục nội dung đúng theo `requirements.md` (rank 1) và `Interview.md` Decision 2 (rank 3, CONFIRMED):
  - `BR-14` (sửa lại): *"Maintenance Request sử dụng Submitted, Pending, In Progress, Resolved, Closed và Rejected."*
  - `BR-15` (sửa lại): *"Facility Manager có thể ưu tiên Asset có Maintenance Risk = High."*
- **Lý do:** BR-14/BR-15 hiện đang mâu thuẫn trực tiếp với REQ-05/REQ-12 và Interview Decision 2 (đều có rank cao hơn theo `source-priority.md`). Nếu không sửa, Engineering/QA có thể code/test sai lifecycle của Maintenance Request.
- **Status:** CONFIRMED — cần BA/AI-Vault thực hiện sửa trực tiếp trên `business-rules.md`.

---

## 4. Bảng tổng hợp (bảng đã có — chỉ cần thêm dòng cuối)

| Decision | Chủ đề | Status |
|---|---|---|
| DEC-01 | Asset & quyền xem Asset | CONFIRMED |
| DEC-02 | Maintenance Request | CONFIRMED |
| DEC-03 | Work Order & Technician | CONFIRMED |
| DEC-04 | IoT Monitoring & Alert | CONFIRMED |
| DEC-05 | AI Predictive Maintenance | CONFIRMED |
| DEC-06 | Authentication & Access Control | CONFIRMED |
| DEC-07 | Cập nhật Status Open Questions đã Resolved | CONFIRMED |
| DEC-08 | Áp dụng Source Priority chính thức | CONFIRMED |
| DEC-09 | Sửa lỗi nội dung sai trong BR-14 và BR-15 | CONFIRMED |



## 5. Related Documents

- `Interview.md` — nguồn gốc DEC-01 → DEC-06
- `vault-qa-benchmark.md` — nguồn gốc DEC-07, DEC-08
- `source-priority.md` — quy tắc được chính thức hóa ở DEC-08
- `open-questions.md` — cần cập nhật theo DEC-07
