# Retrospective — AI Usage & Vault (Role AI/Vault)

> **Project:** Smart Maintenance & Facility Management
> **Owner:** AI/Vault
> **Vị trí gợi ý:** `docs/logs/retrospective.md`
> **Quy ước:** mọi con số dưới đây phải truy được tới file/evidence. Ô ghi **[ĐIỀN]** là số liệu chỉ người làm mới biết (thời gian, case cá nhân) — **không được để AI điền hộ**; nếu không có số thật thì ghi "không đo".

---

## 1. Bảng metric

| Metric | Kết quả | Evidence | Ý nghĩa |
|---|---|---|---|
| Vault Q&A benchmark | 20/20 Correct (100%) ở lượt verification hiện tại | `02-vault/vault-qa-benchmark.md` | Đạt mục tiêu ≥ 80% |
| Phần benchmark có evidence trực tiếp từ chat gốc | 5/20 câu (A1, A2, D3, D4, D5) | `vault-qa-benchmark.md` §7 | 15 câu còn lại là lượt verification mới, không nhận là đã chạy từ đầu |
| Câu hỏi bịa/hallucination trong 5 câu có evidence | 0/5 (3 câu unknown đều trả "KHÔNG ĐỦ DỮ LIỆU") | `ai-usage-log.md` AI-004 | Rule "được nói không đủ dữ liệu" hoạt động |
| Lỗi Vault phát hiện qua benchmark/audit | 5 nhóm: BR-14/15, Open Question lỗi thời (có Q-17), glossary vs BR-13, threshold kỹ thuật vs Q-10, design coi Rejected là state của WO | `vault-qa-benchmark.md` §8, `ai-usage-log.md` AI-005 | Benchmark soi ra lỗi của chính tài liệu nhóm |
| Lỗi đã sửa xong trong nguồn | **[ĐIỀN: đếm lại sau khi BA sửa BR-14/15, Q-17...]** | `decision-log.md` DEC-09, DEC-10 | — |
| AI Feature eval | 26/29 PASS (90%); risk logic 15/15, guardrail 8/8, contract 3/6 | `ai-eval/evaluation-result.md` | Service đúng luật MVP; 3 khoảng lệch tài liệu/thiết kế |
| Giá trị AI sai ngoài {Low, Medium, High} | 0 | Eval C01–C02 | Output validation ổn |
| Thời gian tiết kiệm nhờ AI | **[ĐIỀN hoặc "không đo"]** | — | Giáo trình hỏi metric ngoài cảm giác "nhanh hơn" |
| Số vòng lặp AI/prompt cho 1 artifact | **[ĐIỀN]** | `ai-usage-log.md` | — |

## 2. Case AI sai và cách kiểm chứng (dùng cho Báo cáo lần 2)

**Case thật: bản benchmark mô phỏng.**
- **AI đã làm gì:** bản đầu của `vault-qa-benchmark.md` và `ai-usage-log.md` trình bày "Round 1 = 70%, Round 2 = 100%, Round 3 hồi quy" như kết quả chạy thật, trong khi đó là mô phỏng.
- **Phát hiện bằng cách nào:** đối chiếu với nguồn gốc evidence — chỉ phục hồi được 5 cặp hỏi–đáp thật từ file chat gốc (`ai-usage-log.md` AI-006, AI-007).
- **Xử lý:** thay bằng bản v2.0 tách rõ 5/20 có evidence trực tiếp và 15/20 là verification mới; gỡ claim mô phỏng.
- **Bài học:** AI có thể viết số liệu nghe hợp lý; phải hỏi "evidence nằm ở đâu?" trước khi nhận.

**Case phụ từ benchmark thật:** AI áp dụng đúng `source-priority.md` và chỉ ra `Q-17` còn Open dù `BR-16`/`REQ-08` đã trả lời — lỗi mà lần rà soát tay trước đó (DEC-07) đã bỏ sót.

**[ĐIỀN — case của riêng bạn nếu có thêm]:** AI đề xuất gì → bạn kiểm chứng thế nào → quyết định.

## 3. Đề xuất AI đã bị từ chối / không tự thực hiện

| Đề xuất / hành vi | Quyết định | Lý do |
|---|---|---|
| Kết quả benchmark mô phỏng Round 1/2/3 | Gỡ bỏ | Không có evidence chạy thật |
| Tự sửa `business-rules.md` (BR-14/15) | AI không tự sửa; ghi `DEC-09` để BA sửa | File thuộc quyền BA baseline |
| Dùng threshold 35°C/45°C trong tài liệu kỹ thuật làm business truth | Không chấp nhận | Q-10 còn Open → chỉ là assumption |
| Biến giả định "Rejected" trong prototype thành state WO | Không chấp nhận | Trái DEC-03 (Rejected là action) |
| Coi eval 90% là "AI dự đoán đúng 90%" | Không chấp nhận | Eval chỉ kiểm tra luật + guardrail, chưa có dữ liệu thật |

## 4. Keep / Improve / Stop / Next

- **Keep:** prompt bắt buộc trích ID + quy tắc "KHÔNG ĐỦ DỮ LIỆU"; `source-priority.md` làm trọng tài khi nguồn mâu thuẫn; ghi rõ giới hạn evidence trong log.
- **Improve:** cập nhật `open-questions.md` ngay khi có quyết định (lỗi Q-17 lặp lại lỗi DEC-07); thêm câu benchmark cho `04-design`, `05-technical`, `06-test`, `07-release`; chốt contract AI service trước khi viết tài liệu.
- **Stop:** để AI điền số liệu thời gian/hiệu quả; viết kết quả "đã chạy" khi chưa chạy.
- **Next experiment:** chạy `run_eval.py` trong CI; mở rộng eval set khi Q-10 có ngưỡng chính thức; thêm case dữ liệu mẫu để kiểm `basedOnSampleData`.

## 5. Bài học AI (đưa vào phút 5:30–6:00 của Báo cáo lần 2)

1. AI rất giỏi tổng hợp và soi mâu thuẫn giữa nhiều file, nhưng cần luật ưu tiên nguồn rõ ràng thì mới kết luận đúng.
2. Cho phép AI trả lời "không đủ dữ liệu" quan trọng hơn là ép nó luôn có đáp án.
3. Số liệu đẹp chưa đủ — phải truy được evidence, và phải phân biệt "kiểm tra luật" với "đo độ chính xác thật".
4. Điều sẽ làm khác: cập nhật trạng thái Open Question cùng lúc ghi Decision; chốt contract giữa tài liệu và code sớm hơn.
