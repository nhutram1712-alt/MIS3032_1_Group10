# AI Usage Log

> **Project:** Smart Maintenance & Facility Management  
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng  
> **Version:** 2.0  
> **Last updated:** 2026-09-12  
> **Owner:** AI/Vault  
> **File:** `docs/logs/ai-usage-log.md`

---

## 1. Mục đích

Log này ghi lại các lần dùng AI có evidence kiểm tra được trong bộ tài liệu hiện tại. Không ghi các entry mô phỏng như thể chúng đã xảy ra thật.

Mỗi entry theo schema của giáo trình:

`ID | Date | Task/Story | Goal | Context/Input | AI/Skill | Output | Verification performed | Human decision | Time/quality impact | Artifact link`

> **Nguyên tắc:** nếu artifact không ghi tên tool, ngày hoặc sign-off của người thật thì log ghi rõ `không có bằng chứng`, không tự điền.

---

## 2. Log entries

| ID | Date | Task/Story | Goal | Context/Input | AI/Skill | Output | Verification performed | Human decision | Time/quality impact | Artifact link |
|---|---|---|---|---|---|---|---|---|---|---|
| AI-001 | Không ghi trong artifact | Requirement Review | Phát hiện gap, contradiction, requirement chưa testable và câu hỏi cần xác nhận. | `requirements.md`, `business-rules.md`, các open questions/context liên quan. | **AI-assisted Requirement Reviewer**; tool/model không được ghi trong artifact. | Bảng review REQ/NFR/BR, contradiction/gap summary, testability review, 8 câu hỏi ưu tiên cao. | Artifact tự ghi `Draft / Pending Review`; các đề xuất không được tự động coi là requirement. Đối chiếu lại với `requirements.md`, `Interview.md`, `decision-log.md` khi dùng. | **Pending human sign-off** — chỉ dùng như review note cho đến khi BA/Stakeholder xác nhận. | Chất lượng: giúp tập trung các điểm mơ hồ và rủi ro; không có số phút đáng tin cậy trong artifact. | `docs/logs/requirement-review.md` |
| AI-002 | 2026-09-12 | Vault Q&A setup | Thiết lập AI chỉ trả lời từ Vault, bắt buộc trích nguồn, source priority và `KHÔNG ĐỦ DỮ LIỆU`. | 7 file: `business-rules.md`, `requirements.md`, `open-questions.md`, `decision-log.md`, `glossary.md`, `Interview.md`, `source-priority.md`. | ChatGPT; skill: grounded Vault Q&A. Prompt đầy đủ nằm trong file chat Word và benchmark. | AI xác nhận context, nêu source-priority, nhận diện `DEC-09` và `DEC-07` trước khi trả lời. | So sánh với `source-priority.md`, `decision-log.md`; nội dung phù hợp với rank nguồn hiện tại. | Người dùng đã yêu cầu dùng chính bộ tài liệu này làm cơ sở cho 2 artifact; **chưa có sign-off riêng cho từng claim**. | Chất lượng: giảm rủi ro trả lời dựa trên trí nhớ ngoài Vault. | `Đoạn chat AI 20 câu hỏi.docx`; `docs/02-vault/vault-qa-benchmark.md` |
| AI-003 | 2026-09-12 | Q&A — Asset Type / Asset Status | Kiểm tra câu factual và conflict source priority. | `BR-03`, `CON-02`, glossary, Interview Decision 1; `BR-16`, `REQ-08`, Q-17. | ChatGPT; grounded Q&A. | Trả lời 5 Asset Type; Facility Manager là người duy nhất đổi thủ công Asset Status; chỉ ra Q-17 đang Open nhưng rank thấp hơn. | Đối chiếu trực tiếp `BR-03`, `CON-02`, `BR-16`, `REQ-08`, `source-priority.md`. | Chấp nhận làm evidence benchmark; cần nhóm review trước khi commit nếu muốn dùng làm bằng chứng chính thức. | Chất lượng: phát hiện thêm inconsistency Q-17, tránh coi Open Question là source-of-truth. | `Đoạn chat AI 20 câu hỏi.docx`; benchmark A1, A2/C4 |
| AI-004 | 2026-09-12 | Q&A — Unknown / scope boundary | Kiểm tra AI có biết dừng khi Vault thiếu dữ liệu. | `BR-13`, `DEC-04`, Q-16; `DEC-01`, `REQ-06`, Q-18; `REQ-09`, `NFR-07` và context 7 file. | ChatGPT; grounded Q&A với rule `KHÔNG ĐỦ DỮ LIỆU`. | Trả lời `KHÔNG ĐỦ DỮ LIỆU` cho sau-MVP multi-sensor, nhu cầu field mở rộng và UI Dashboard cụ thể trong context 7 file. | Đối chiếu phạm vi MVP và các Open Question; không dùng design docs ngoài context để “sửa” câu trả lời chat gốc. | Chấp nhận làm evidence về unknown-handling; không suy diễn thêm. | Chất lượng: tránh bịa roadmap/UI hoặc biến assumption thành fact. | `Đoạn chat AI 20 câu hỏi.docx`; benchmark D3, D4, E3 |
| AI-005 | 2026-09-12 | Full-repo Vault consistency audit | Kiểm tra benchmark và 7-file Vault có nhất quán với toàn bộ ZIP project hay không. | Toàn bộ `docs/` trong `MIS3032_1_Group10-main.zip`, đặc biệt source priority, vault, design, technical, test/release. | ChatGPT (GPT-5.6 Sol); skill: source consistency audit. | Phát hiện 5 rủi ro chính: BR-14/15 chưa sửa vật lý; nhiều Open Question đã có decision nhưng còn Open; glossary multi-sensor mâu thuẫn MVP; technical threshold số cụ thể trong khi Q-10 chưa baseline; design có assumption `Rejected` cho WO. | Mỗi finding được kiểm tra lại với `source-priority.md`, `DEC-03/04/05/09`, `BR-13/17`, `REQ-05/12`. | **Pending team/BA sign-off** cho các thay đổi nguồn; không tự sửa các source file ngoài 2 file người dùng yêu cầu. | Chất lượng: phát hiện defect có thể làm Engineering/QA hiểu sai business rule. | `docs/02-vault/vault-qa-benchmark.md` §8 |
| AI-006 | 2026-09-12 | Vault Q&A Benchmark — full run | Hoàn thiện đủ 20 câu theo chuẩn factual/cross-file/conflict/unknown/edge-case và đo accuracy. | 7-file benchmark context + prompt strict + source priority; evidence Word được dùng cho 5 câu phục hồi. | ChatGPT (GPT-5.6 Sol); skill: benchmark/evaluation. | 20 câu được kiểm tra; kết quả verification hiện tại: 20 Correct, 0 Partial, 0 Wrong, 0 Unsupported; Accuracy 100%. | Đối chiếu từng expected answer với ID nguồn; phân biệt rõ 5/20 câu có evidence trực tiếp trong Word và 15/20 câu được hoàn thiện ở lượt verification mới. | Chấp nhận làm bản benchmark cập nhật; **không tuyên bố file Word ban đầu đã chứa đủ 20 câu**. | Chất lượng: đạt target >=80%; tăng tính auditability nhờ tách evidence cũ và run mới. | `docs/02-vault/vault-qa-benchmark.md` |
| AI-007 | 2026-09-12 | Documentation cleanup | Thay hai artifact mô phỏng bằng bản grounded, có giới hạn evidence rõ ràng. | Giáo trình 2026 + target files cũ + ZIP project + file chat Word. | ChatGPT (GPT-5.6 Sol); skill: technical documentation. | Viết lại `vault-qa-benchmark.md` và `ai-usage-log.md` theo đúng schema, không giữ claim Round 1/2/3 mô phỏng như sự kiện thật. | Kiểm tra file path, heading, bảng Markdown, ID nguồn và các claim quan trọng với repo. | Theo yêu cầu trực tiếp của người dùng: xuất đúng 2 file; các thay đổi khác trong repo không được thực hiện. | Chất lượng: evidence rõ hơn, tránh fabrication trong báo cáo môn học. | Hai file đầu ra hiện tại |

---

## 3. Tổng kết

| Chỉ số | Kết quả |
|---|---|
| Entries có evidence trong bộ tài liệu/lượt làm hiện tại | 7 |
| Entry dùng claim mô phỏng không có evidence | 0 |
| Full Vault benchmark hiện tại | 20/20 Correct = 100% |
| Evidence trực tiếp phục hồi từ file chat Word | 5/20 câu benchmark |
| Defect tài liệu phát hiện trong full-repo audit | 5 nhóm chính |
| Human sign-off còn cần | Các thay đổi nguồn ngoài 2 artifact này và các claim chưa có người thật xác nhận |

## 4. Những quyết định AI đã **không** tự thực hiện

- Không sửa `business-rules.md` dù `DEC-09` cho thấy BR-14/BR-15 đang sai; chỉ ghi action cần BA/AI-Vault thực hiện.
- Không tự đổi Status của Q-11/Q-12/Q-13/Q-15/Q-16/Q-17; chỉ báo inconsistency và đề xuất review.
- Không lấy threshold số trong technical docs làm business truth khi Q-10 chưa có quyết định nghiệp vụ tương ứng.
- Không biến assumption trong design/prototype thành business rule mới.
- Không tuyên bố 20/20 câu đã có trong file chat Word khi chỉ phục hồi được 5 cặp Q&A rõ ràng.

## 5. Related Documents

- `docs/02-vault/vault-qa-benchmark.md`
- `docs/logs/requirement-review.md`
- `docs/source-priority.md`
- `docs/02-vault/decision-log.md`
- `docs/02-vault/open-questions.md`
- `docs/02-vault/business-rules.md`
