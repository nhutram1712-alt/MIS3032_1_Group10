# AI Usage Log

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Owner:** AI/Vault
> **Vị trí file gợi ý:** gốc repo `docs/AI_USAGE_LOG.md`, hoặc `02-vault/ai-usage-log.md` nếu nhóm muốn gom hết vào vault

---

## 1. Mục đích

File này ghi lại **mọi lần dùng AI có ảnh hưởng đến sản phẩm/tài liệu của dự án**: dùng AI để làm gì, AI trả lời gì, người đã kiểm tra lại ra sao, và quyết định cuối cùng là gì.

**Vì sao phải ghi:** để chứng minh nhóm dùng AI có kiểm soát (không copy-paste mù), và để bất kỳ ai trong nhóm cũng có thể xem lại "AI từng gợi ý gì, tại sao mình chấp nhận/từ chối".

## 2. Cách điền mỗi entry

Mỗi dòng log cần đủ 7 trường:

| Trường | Ý nghĩa |
|---|---|
| **Task** | Đang làm việc gì |
| **Input/Context** | Đưa gì cho AI (file nào, đoạn nào) |
| **AI Tool & Prompt** | Dùng AI nào, prompt tóm tắt |
| **Output** | AI trả lời gì (tóm tắt) |
| **Human Verification** | Người kiểm tra lại như thế nào, đúng/sai chỗ nào |
| **Decision** | Chấp nhận nguyên văn / sửa lại / từ chối |
| **Time** | Thời gian thực hiện |

---

## 3. Log Entries

### Entry 01 — Thiết kế cấu trúc thư mục Vault

- **Ngày:** 2026-09-08
- **Task:** Xác định cấu trúc thư mục cho Vault (`01-discovery`, `02-vault`, `03-product`...)
- **Input/Context:** Toàn bộ file nháp hiện có của nhóm (idea, business rules, requirements...) + yêu cầu giáo trình về Vault
- **AI Tool & Prompt:** Claude — "Đề xuất cấu trúc thư mục Vault phù hợp với tài liệu BA hiện có của nhóm, dựa trên chuẩn Vault của giáo trình"
- **Output:** AI đề xuất 3 nhóm thư mục theo giai đoạn: Discovery → Vault (domain knowledge) → Product
- **Human Verification:** Đối chiếu với các file thật đang có, xác nhận nhóm nào chứa file nào là hợp lý
- **Decision:** Chấp nhận cấu trúc, tạo trên GitHub
- **Time:** 20 phút

---

### Entry 02 — Soạn `source-priority.md`

- **Ngày:** 2026-09-08
- **Task:** Viết quy tắc ưu tiên nguồn cho Vault
- **Input/Context:** `business-rules.md`, `requirements.md`, `open-questions.md`, `Interview.md`, `customer-brief.md` (đọc toàn bộ để xác định tài liệu nào baseline hơn tài liệu nào)
- **AI Tool & Prompt:** Claude — "Dựa trên Status (Baselined/Draft) của các file trong Vault, xếp thứ tự ưu tiên nguồn và đưa ví dụ xung đột thật nếu tìm thấy"
- **Output:** AI đề xuất thứ tự 9 hạng ưu tiên, và **phát hiện** open-questions.md đang có nhiều dòng Status = "Open" dù đã được Interview.md xác nhận từ trước
- **Human Verification:** Kiểm tra lại từng dòng open-questions.md (Q-01 → Q-20) đối chiếu Interview.md Decision 1-6 — xác nhận đúng là 10/20 câu đã có Interview quyết định nhưng Status chưa cập nhật
- **Decision:** Chấp nhận cấu trúc source-priority.md; tạo thêm quyết định DEC-07 để sửa lỗi open-questions.md
- **Time:** 35 phút

---

### Entry 03 — Soạn bộ 20 câu hỏi Vault Q&A Benchmark

- **Ngày:** 2026-09-08
- **Task:** Tạo bộ câu hỏi kiểm tra chất lượng Vault (4 nhóm: Factual/Cross-file/Conflict/Unknown)
- **Input/Context:** Toàn bộ `02-vault/` và `03-product/`
- **AI Tool & Prompt:** Claude — "Tạo 20 câu hỏi kiểm tra Vault, chia đều 4 loại: factual, cross-file, conflict (dựa trên mâu thuẫn thật nếu có), và câu hỏi Vault không có dữ liệu"
- **Output:** 20 câu hỏi, trong đó nhóm Conflict dùng đúng 5 mâu thuẫn thật tìm thấy trong Vault (open-questions.md lỗi thời, ghi chú cũ trong customer-brief.md, thiếu sót trong MVP-scope.md)
- **Human Verification:** Đọc lại từng câu, xác nhận đáp án đúng bằng cách tự tra trong file gốc (không tin AI 100%)
- **Decision:** Chấp nhận toàn bộ 20 câu, dùng làm bộ benchmark chính thức
- **Time:** 40 phút

---

### Entry 04 — Chạy Benchmark Round 1 (trước khi có source-priority.md)

- **Ngày:** 2026-09-08
- **Task:** Chạy AI trả lời 20 câu hỏi, dùng prompt đơn giản (chưa có quy tắc trích nguồn/chống suy luận)
- **Input/Context:** Toàn bộ Vault, KHÔNG kèm source-priority.md (vì lúc này chưa tồn tại)
- **AI Tool & Prompt:** Claude — "Trả lời câu hỏi dựa trên Vault đính kèm" (prompt tối giản, cố ý để so sánh)
- **Output:** 14/20 Correct, 2 Partial, 4 Wrong → Accuracy 70%. Lỗi tập trung ở nhóm Conflict (chỉ 1/5 đúng) và 1 câu bịa số liệu (D1 – threshold IoT)
- **Human Verification:** Chấm từng câu bằng tay theo 4 mức Correct/Partial/Wrong/Unsupported, đối chiếu lại nguồn gốc
- **Decision:** Không đạt mục tiêu 80% → cần cải thiện Vault + prompt trước khi chấp nhận
- **Time:** 45 phút

---

### Entry 05 — Phân tích lỗi & xác định hành động sửa

- **Ngày:** 2026-09-08
- **Task:** Tìm nguyên nhân gốc của các câu sai ở Round 1
- **Input/Context:** Bảng kết quả Round 1
- **AI Tool & Prompt:** Claude — "Phân tích vì sao AI trả lời sai ở các câu Conflict và câu D1, đề xuất cách sửa Vault hoặc prompt"
- **Output:** AI chỉ ra 3 nguyên nhân gốc: (1) thiếu source-priority.md, (2) prompt không cấm suy luận ngoài Vault, (3) open-questions.md lỗi thời
- **Human Verification:** Đồng ý với cả 3 nguyên nhân sau khi tự kiểm tra lại
- **Decision:** Thực hiện cả 3 hành động sửa (đã ghi ở Entry 02, 06, và quyết định DEC-07)
- **Time:** 15 phút

---

### Entry 06 — Cập nhật prompt chuẩn + chạy Round 2

- **Ngày:** 2026-09-08
- **Task:** Viết lại prompt chuẩn (bắt buộc trích nguồn, áp dụng source-priority, cấm suy luận khi thiếu dữ liệu) và chạy lại 20 câu
- **Input/Context:** Vault đầy đủ + `source-priority.md` mới tạo
- **AI Tool & Prompt:** Claude — prompt đầy đủ (xem `vault-qa-benchmark.md` mục 3)
- **Output:** 20/20 Correct → Accuracy 100%. AI tự phát hiện và chỉ ra open-questions.md lỗi thời khi trả lời C1/C2
- **Human Verification:** Chấm lại toàn bộ 20 câu, xác nhận không còn suy luận ngoài Vault
- **Decision:** Chấp nhận kết quả, benchmark đạt mục tiêu
- **Time:** 30 phút

---

### Entry 07 — Cập nhật Status trong `open-questions.md`

- **Ngày:** 2026-09-09
- **Task:** Sửa lỗi gốc rễ phát hiện từ benchmark — cập nhật Status cho các câu đã Resolved
- **Input/Context:** `open-questions.md`, `Interview.md` (đối chiếu từng Decision 1-6 với từng Open Question liên quan)
- **AI Tool & Prompt:** Claude — "Đối chiếu từng Open Question với Interview.md, liệt kê chính xác câu nào nên đổi Status thành Resolved"
- **Output:** Danh sách 10 câu cần đổi: Q-01, Q-02, Q-03, Q-04, Q-05, Q-06, Q-07, Q-08, Q-19, Q-20
- **Human Verification:** Kiểm tra lại từng câu một lần nữa, xác nhận đúng 10/20 câu
- **Decision:** Ghi thành quyết định chính thức `DEC-07` trong `decision-log.md`; đề nghị BA cập nhật file `open-questions.md`
- **Time:** 20 phút

---

## 4. Tổng kết

| Chỉ số | Giá trị |
|---|---|
| Tổng số entries | 7 |
| Tổng thời gian dùng AI có kiểm soát | ~205 phút (~3.4 giờ) |
| Số lần AI output bị sửa/từ chối một phần | 1 (Round 1 benchmark không đạt, phải chạy lại) |
| Accuracy Vault Benchmark cuối cùng | 100% (Round 2), xác nhận ổn định ở Round 3 |

## 5. Related Documents

- `vault-qa-benchmark.md` — chi tiết Round 1/2/3
- `decision-log.md` — DEC-07 và các quyết định liên quan
- `source-priority.md` — sản phẩm của Entry 02
