# Vault Q&A Benchmark

> **Project:** Smart Maintenance & Facility Management  
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng  
> **Version:** 2.0  
> **Date:** 2026-09-12  
> **Owner:** AI/Vault  
> **File:** `docs/02-vault/vault-qa-benchmark.md`

---

## 1. Mục tiêu

Benchmark này kiểm tra khả năng AI trả lời **chỉ dựa trên Project Vault**, có truy nguồn, biết xử lý xung đột và biết dừng đúng lúc khi Vault không đủ dữ liệu.

Theo giáo trình, bộ benchmark nên có **15–25 câu**, bao phủ factual, cross-file, conflict, unknown và edge-case; mỗi câu được chấm `Correct / Partial / Wrong / Unsupported`. Mục tiêu của nhóm là **Accuracy >= 80%**.

## 2. Phạm vi nguồn dùng cho benchmark

### 2.1. Context chính của lượt Q&A

Để khớp với file bằng chứng `Đoạn chat AI 20 câu hỏi.docx`, lượt benchmark dùng 7 file sau:

1. `docs/02-vault/business-rules.md`
2. `docs/02-vault/requirements.md`
3. `docs/02-vault/open-questions.md`
4. `docs/02-vault/decision-log.md`
5. `docs/02-vault/glossary.md`
6. `docs/02-vault/Interview.md`
7. `docs/source-priority.md`

### 2.2. Full-repo audit

ZIP project được dùng để kiểm tra tính nhất quán của benchmark với toàn bộ tài liệu hiện tại. Các file ở `03-product/`, `04-design/`, `05-technical/`, `06-test/`, `07-release/` **không được dùng để lấp chỗ trống của câu trả lời benchmark nếu chúng không nằm trong context 7 file ở trên**.

Điểm này đặc biệt quan trọng với câu hỏi UI Dashboard: full repo hiện có tài liệu design, nhưng lượt chat gốc chỉ được cung cấp 7 file nên câu trả lời đúng trong context đó vẫn là `KHÔNG ĐỦ DỮ LIỆU`.

## 3. Prompt chuẩn

```text
Bạn là trợ lý chỉ được trả lời DỰA TRÊN nội dung các file Project Vault được cung cấp.

QUY TẮC BẮT BUỘC:
1. Mọi câu trả lời phải trích rõ ID và tên file nguồn.
2. Nếu 2 nguồn mâu thuẫn, áp dụng đúng thứ tự trong source-priority.md và nói rõ
   đang ưu tiên nguồn nào, vì sao.
3. Nếu Vault không đủ thông tin để trả lời, PHẢI trả lời chính xác:
   KHÔNG ĐỦ DỮ LIỆU
   Tuyệt đối không suy luận, không bịa số liệu, không dùng kiến thức ngoài Vault.
4. Không tự mở rộng phạm vi ngoài những gì Vault đã xác nhận.
5. Open Question không tự động thắng Requirement/Business Rule đã baselined.
```

## 4. Quy tắc chấm

| Mức | Tiêu chí |
|---|---|
| **Correct** | Đúng nội dung, đúng nguồn/ID, xử lý đúng source priority, không suy diễn. Với câu unknown, trả lời đúng `KHÔNG ĐỦ DỮ LIỆU` cũng là Correct. |
| **Partial** | Đúng ý chính nhưng thiếu nguồn, thiếu điều kiện quan trọng hoặc diễn đạt dễ gây hiểu nhầm. |
| **Wrong** | Sai nội dung, chọn sai nguồn, tự bịa hoặc suy luận ngoài Vault. |
| **Unsupported** | Câu trả lời đưa ra claim mà Vault không hỗ trợ. Khi AI vẫn cố trả lời một câu unknown, chấm Wrong/Unsupported tùy mức độ. |

**Accuracy = số câu Correct / 20 × 100%.**

---

## 5. Bộ 20 câu benchmark và đáp án chuẩn

### A. Factual — 4 câu

| ID | Câu hỏi | Đáp án chuẩn | Nguồn chính |
|---|---|---|---|
| A1 | MVP hỗ trợ tối đa bao nhiêu Asset Type, gồm những loại nào? | 5 loại: Wi-Fi, Air Conditioner, Projector, Light, Fan. | `BR-03` — `business-rules.md`; `CON-02` — `requirements.md` |
| A2 | Ai là người duy nhất được phép thay đổi thủ công Asset Status? | Facility Manager. | `BR-16` — `business-rules.md`; `REQ-08` — `requirements.md` |
| A3 | IoT Data được thu thập theo chu kỳ bao lâu trong MVP? | 5 phút/lần. | `REQ-28`, `NFR-05` — `requirements.md`; `DEC-04` — `decision-log.md` |
| A4 | AI Prediction dự đoán điều gì và trong bao lâu? | Khả năng Asset cần bảo trì trong 7 ngày tiếp theo; kết quả Risk ở Low/Medium/High. | `REQ-11`, `REQ-12` — `requirements.md`; `DEC-05` — `decision-log.md` |

### B. Cross-file — 4 câu

| ID | Câu hỏi | Đáp án chuẩn | Nguồn chính |
|---|---|---|---|
| B1 | Technician từ chối Work Order có tạo trạng thái `Rejected` chính thức cho WO không? | Không. Từ chối là **action**; WO chỉ có Assigned, In Progress, Completed, Cancelled. | `BR-07`, `BR-17` — `business-rules.md`; `DEC-03` — `decision-log.md`; Interview Decision 3 |
| B2 | Maintenance Request được đóng khi nào? | Chỉ sau khi Technician hoàn thành WO và Facility Manager xác nhận kết quả. | `BR-18` — `business-rules.md`; `REQ-13`, `REQ-22`, `REQ-23` — `requirements.md`; `DEC-02` |
| B3 | Facility Manager được notification trong những trường hợp nào? | Khi có IoT Alert nghiêm trọng hoặc Maintenance Risk = High. | `REQ-30` — `requirements.md`; `DEC-04`, `DEC-05` — `decision-log.md` |
| B4 | MVP dùng SSO hay tài khoản riêng, và có bao nhiêu role? | Tài khoản riêng; không SSO; 4 role: Requester, Technician, Facility Manager, Admin. | `CON-03`, `CON-08` — `requirements.md`; `DEC-06` — `decision-log.md` |

### C. Conflict — 4 câu

| ID | Câu hỏi | Đáp án chuẩn / cách xử lý conflict | Nguồn ưu tiên |
|---|---|---|---|
| C1 | `BR-14` hiện ghi các giá trị giống Asset Status, trong khi `REQ-05` ghi 6 trạng thái Maintenance Request. Nội dung nào đúng? | Đúng là `Submitted, Pending, In Progress, Resolved, Closed, Rejected`. `DEC-09` xác nhận BR-14 đang sai và phải sửa. | `REQ-05` + `DEC-09`; áp dụng `source-priority.md` |
| C2 | `BR-15` ghi `Risk = Cao`, còn `REQ-12` dùng `Low/Medium/High`. Dùng giá trị nào? | Dùng `High`. `DEC-09` xác nhận BR-15 phải sửa lại thành `Maintenance Risk = High`. | `REQ-12` + `DEC-09` |
| C3 | `glossary.md` nói một Asset có thể có một hoặc nhiều Sensor, nhưng `BR-13` giới hạn 1 Asset–1 Sensor trong MVP. Theo MVP dùng quy tắc nào? | Tối đa 1 IoT Device/Sensor cho mỗi Asset trong MVP. Business Rule/Requirement rank cao hơn glossary. | `BR-13`; `source-priority.md` |
| C4 | `open-questions.md` vẫn để Q-17 = Open nhưng `BR-16`/`REQ-08` đã xác định người đổi Asset Status. Có câu trả lời chính thức chưa? | Có. Facility Manager là người duy nhất được đổi thủ công. Q-17 là trạng thái tài liệu bị lỗi thời. | `BR-16`, `REQ-08`; `source-priority.md` |

### D. Unknown — 4 câu

| ID | Câu hỏi | Đáp án chuẩn | Nguồn/logic |
|---|---|---|---|
| D1 | Threshold cụ thể để tạo IoT Alert cho từng Asset Type là bao nhiêu? | **KHÔNG ĐỦ DỮ LIỆU.** Context 7 file chỉ xác nhận alert dùng threshold cấu hình; Q-10 vẫn Open và Interview Decision 4 nói giá trị cụ thể chưa chốt. | `Q-10` — `open-questions.md`; `DEC-04`; Interview Decision 4 |
| D2 | Trường hiện có đủ Maintenance History thật để huấn luyện/đánh giá AI chưa? | **KHÔNG ĐỦ DỮ LIỆU.** Vault chỉ cho phép dùng dữ liệu mẫu/giả lập nếu thiếu dữ liệu thật. | `Q-14`; `ASM-05`; `DEC-05` |
| D3 | Asset có cần bổ sung Manufacturer, Model, Installation Date, Warranty hoặc Purchase Date không? | **KHÔNG ĐỦ DỮ LIỆU** cho quyết định mở rộng. MVP hiện chỉ xác nhận Asset ID, Name, Type, Location, Status. | `Q-18`; `DEC-01`; `REQ-06` |
| D4 | UI cụ thể của Dashboard Facility Manager trông như thế nào? | **KHÔNG ĐỦ DỮ LIỆU** trong context 7 file của lượt chat gốc. | `REQ-09`, `NFR-07` chỉ mô tả yêu cầu khái niệm; không có design file trong context lượt chat |

### E. Edge-case / Scope boundary — 4 câu

| ID | Câu hỏi | Đáp án chuẩn | Nguồn chính |
|---|---|---|---|
| E1 | Request chỉ nêu khu vực, chưa xác định Asset. Có thể tạo Work Order ngay không? | Chưa. Phải xác định Asset cụ thể trước khi tạo WO. | `BR-05`, `BR-08` — `business-rules.md` |
| E2 | Một Maintenance Request đã có Work Order rồi. Có thể tạo Work Order thứ hai trong MVP không? | Không. Tối đa 1 Work Order/Request trong MVP. | `BR-06`; `DEC-02` |
| E3 | Sau MVP, một Asset có thể mapping nhiều Sensor không? | **KHÔNG ĐỦ DỮ LIỆU.** Vault chỉ chốt giới hạn 1 Sensor trong MVP, không có roadmap cho sau MVP trong context benchmark. | `BR-13`; `DEC-04`; `Q-16` |
| E4 | AI có thể tự tạo Work Order hoặc tự quyết định bảo trì khi Risk = High không? | Không. AI chỉ hỗ trợ; Facility Manager quyết định cuối cùng. | `BR-10`; `CON-07`; `DEC-05` |

---

## 6. Kết quả chạy đầy đủ ngày 2026-09-12

Lượt kiểm tra đầy đủ dùng prompt ở mục 3, đối chiếu từng câu với 7 file context và `source-priority.md`.

| Nhóm | Correct | Partial | Wrong | Unsupported |
|---|---:|---:|---:|---:|
| A — Factual | 4 | 0 | 0 | 0 |
| B — Cross-file | 4 | 0 | 0 | 0 |
| C — Conflict | 4 | 0 | 0 | 0 |
| D — Unknown | 4 | 0 | 0 | 0 |
| E — Edge-case | 4 | 0 | 0 | 0 |
| **Tổng** | **20** | **0** | **0** | **0** |

**Accuracy = 20/20 = 100%** — đạt mục tiêu `>= 80%`.

> Kết quả 100% ở đây là **lượt verification mới ngày 2026-09-12 với prompt chặt**. Không được dùng kết quả này để khẳng định file Word bằng chứng ban đầu đã chứa đủ 20 lượt hỏi đáp.

## 7. Evidence từ file `Đoạn chat AI 20 câu hỏi.docx`

File Word cung cấp prompt chuẩn và **5 cặp Q&A có thể phục hồi rõ từ nội dung tài liệu**. Không thấy đủ 20 câu hỏi/đáp án trong phần text của file, nên benchmark không tự dựng lại 15 lượt còn thiếu rồi gán là “chat gốc”.

| Benchmark ID | Evidence phục hồi từ file Word | Đánh giá |
|---|---|---|
| A1 | AI trả lời đúng 5 Asset Type và trích `BR-03`, `CON-02`, glossary, Interview. | Correct |
| A2 / C4 | AI trả lời Facility Manager và tự phát hiện Q-17 vẫn Open; áp dụng source priority đúng. | Correct |
| E3 | AI trả lời `KHÔNG ĐỦ DỮ LIỆU` cho câu hỏi sau MVP về multi-sensor, không suy luận roadmap. | Correct |
| D3 | AI trả lời `KHÔNG ĐỦ DỮ LIỆU` cho việc có cần thêm Manufacturer/Model/Warranty, đồng thời nêu phạm vi MVP hiện tại. | Correct |
| D4 | AI trả lời `KHÔNG ĐỦ DỮ LIỆU` cho UI Dashboard khi context chỉ có 7 file Vault. | Correct |

**Evidence coverage của file Word: 5/20 câu.** 15 câu còn lại được hoàn thiện ở lượt verification mới trong tài liệu này.

---

## 8. Lỗi Vault phát hiện và hành động cải thiện

Benchmark với prompt chặt không còn trả lời sai, nhưng quá trình đối chiếu đã phát hiện các defect tài liệu thật cần xử lý. Đây là các cải thiện quan trọng hơn việc chỉ tối ưu prompt.

| # | Defect / rủi ro | Evidence | Hành động đề xuất |
|---:|---|---|---|
| 1 | `business-rules.md` vẫn chứa nội dung sai ở BR-14 và BR-15 dù `DEC-09` đã xác nhận nội dung đúng. | `BR-14`, `BR-15`, `DEC-09` | Sửa trực tiếp BR-14 thành 6 trạng thái Maintenance Request; BR-15 dùng `Risk = High`. |
| 2 | `open-questions.md` còn để Q-11, Q-12, Q-13, Q-15, Q-16, Q-17 = Open dù `DEC-04`/`DEC-05` hoặc requirement/business rule đã chốt nội dung. | `DEC-04`, `DEC-05`, `REQ-11/12/28`, `BR-13/16` | Rà lại trạng thái Open Question; chỉ giữ Open cho phần thực sự chưa chốt như Q-10, Q-14, Q-18. |
| 3 | `glossary.md` nói một Asset có thể có “một hoặc nhiều” Sensor, mâu thuẫn với BR-13 trong MVP. | `glossary.md` §3.3 vs `BR-13` | Sửa wording thành: “khái niệm tổng quát có thể nhiều; **MVP giới hạn 1 Asset–1 Sensor theo BR-13**”. |
| 4 | `05-technical/iot-requirements.md` có bảng threshold số cụ thể dù Q-10 trong nguồn nghiệp vụ vẫn Open. | Q-10 + `source-priority.md` rule cho technical docs | Gắn rõ các threshold này là **sample/implementation assumption**, hoặc tạo Decision/Requirement được stakeholder xác nhận trước khi coi là official. |
| 5 | Một số design/prototype note đề xuất/tạm dùng `Rejected` như state của Work Order, trái với `DEC-03`/`BR-17` (Rejected chỉ là action). | `Prototype_finding.md`, `HANDOFF.md` vs `DEC-03`, `BR-17` | Sửa design để không tạo state business mới nếu chưa có decision; dùng action/reassignment flow đúng baseline. |

## 9. Regression checklist

Chạy lại ít nhất các câu sau mỗi khi sửa Vault lớn:

- A2 — quyền đổi Asset Status.
- C1 — BR-14 vs REQ-05/DEC-09.
- C3 — cardinality Asset–Sensor.
- D1 — threshold cụ thể phải không bị bịa.
- D4 — chỉ được trả UI khi design files thực sự nằm trong context.
- E4 — AI không được tự động quyết định bảo trì.

## 10. Kết luận

- Bộ benchmark có **20 câu**, đủ 5 nhóm yêu cầu: factual, cross-file, conflict, unknown, edge-case.
- Lượt verification hiện tại đạt **20/20 = 100%** với prompt grounded và source priority.
- File chat Word cung cấp bằng chứng rõ cho **5/20** lượt; phần còn lại được hoàn thiện trong lượt verification mới, không giả mạo là nội dung có sẵn trong Word.
- Benchmark phát hiện ít nhất **5 điểm cần cải thiện Vault**, trong đó ưu tiên cao nhất là sửa BR-14/BR-15 và đồng bộ trạng thái Open Question.

## 11. Related Documents

- `docs/source-priority.md`
- `docs/02-vault/business-rules.md`
- `docs/02-vault/requirements.md`
- `docs/02-vault/open-questions.md`
- `docs/02-vault/decision-log.md`
- `docs/02-vault/Interview.md`
- `docs/02-vault/glossary.md`
- `docs/logs/ai-usage-log.md`
