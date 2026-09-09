# Vault Q&A Benchmark

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.0
> **Owner:** AI/Vault
> **Vị trí file:** `02-vault/vault-qa-benchmark.md`

---

## 1. Mục đích file này là gì?

Đây là bài kiểm tra chất lượng của Vault: **đưa toàn bộ nội dung Vault cho một AI, hỏi 20 câu, xem AI trả lời đúng bao nhiêu %.**

Vì sao phải làm việc này?
- Nếu AI trả lời sai nhiều → nghĩa là Vault đang viết mơ hồ, thiếu ID, thiếu liên kết, hoặc có nội dung mâu thuẫn → cần sửa Vault.
- Nếu AI trả lời đúng nhiều nhưng lại **suy đoán/bịa** khi thiếu dữ liệu → Vault cần bổ sung quy tắc "phải nói KHÔNG ĐỦ DỮ LIỆU" rõ hơn trong prompt.
- Đây cũng chính là **evidence cá nhân** của role AI/Vault khi báo cáo: mở file này, hỏi lại 2-3 câu ngay trước mặt giảng viên để chứng minh Vault "sống" và đáng tin.

## 2. Cách chấm điểm (rất quan trọng, đọc kỹ trước khi chạy)

Mỗi câu trả lời của AI được chấm 1 trong 4 mức:

| Mức | Ý nghĩa |
|---|---|
| ✅ **Correct** | Đúng nội dung, đúng nguồn (trích đúng file/ID), không suy diễn thêm |
| 🟡 **Partial** | Đúng ý chính nhưng thiếu chi tiết, thiếu trích nguồn, hoặc diễn đạt gây hiểu nhầm nhẹ |
| ❌ **Wrong** | Sai nội dung, hoặc trích sai nguồn, hoặc suy luận/bịa thông tin không có trong Vault |
| ⚠️ **Unsupported** | AI trả lời nhưng Vault thực sự không có đủ dữ liệu để trả lời (AI phải nói rõ "KHÔNG ĐỦ DỮ LIỆU" mới được tính Correct; nếu AI vẫn cố trả lời → tính Wrong) |

**Công thức accuracy:** `Accuracy = Số câu Correct / Tổng số câu × 100%`
**Mục tiêu:** Accuracy ≥ 80%.

## 3. Prompt bắt buộc dùng khi chạy benchmark

```
Bạn là trợ lý chỉ được trả lời DỰA TRÊN nội dung Vault được cung cấp bên dưới.

QUY TẮC BẮT BUỘC:
1. Mọi câu trả lời phải trích rõ ID và tên file nguồn (VD: BR-13, business-rules.md).
2. Nếu 2 nguồn trong Vault mâu thuẫn nhau, áp dụng đúng thứ tự trong source-priority.md
   và nói rõ bạn đang ưu tiên nguồn nào, vì sao.
3. Nếu Vault KHÔNG có đủ thông tin để trả lời, bạn PHẢI trả lời chính xác:
   "KHÔNG ĐỦ DỮ LIỆU" — TUYỆT ĐỐI không suy luận, không bịa số liệu, không dùng
   kiến thức chung bên ngoài Vault.
4. Không được tự ý mở rộng phạm vi (scope) ngoài những gì Vault đã xác nhận.

[DÁN TOÀN BỘ NỘI DUNG 01-discovery/, 02-vault/, 03-product/ VÀO ĐÂY]

Câu hỏi: {câu hỏi}
```

---

## 4. Bộ 20 câu hỏi + kết quả chạy thử (Round 1 — trước khi có source-priority.md)

> Ghi chú: Round 1 được chạy **trước khi** `source-priority.md` tồn tại và với prompt **chưa** có quy tắc bắt buộc trích nguồn/từ chối suy luận (bản đơn giản: *"Trả lời câu hỏi dựa trên Vault đính kèm"*).

### Nhóm A — Factual (hỏi thẳng 1 fact, nằm trong 1 file)

| # | Câu hỏi | Đáp án đúng (nguồn) | AI trả lời (Round 1) | Điểm |
|---|---|---|---|---|
| A1 | MVP hỗ trợ tối đa bao nhiêu loại Asset Type, gồm những loại nào? | 5 loại: Wi-Fi, Air Conditioner, Projector, Light, Fan (`BR-03`, `customer-brief.md` §1) | Trả lời đúng 5 loại, có trích BR-03 | ✅ Correct |
| A2 | Ai là người duy nhất được phép thay đổi thủ công Asset Status? | Facility Manager (`BR-16`) | Trả lời đúng, trích BR-16 | ✅ Correct |
| A3 | IoT Data được thu thập theo chu kỳ bao lâu trong MVP? | 5 phút/lần (`REQ-28`, `NFR-05`, `customer-brief.md` §6) | Trả lời đúng "5 phút/lần" | ✅ Correct |
| A4 | AI Prediction dự đoán điều gì, trong khung thời gian bao lâu? | Khả năng Asset cần bảo trì trong 7 ngày tới (`REQ-11`, `BR-11`) | Trả lời đúng | ✅ Correct |
| A5 | Mỗi Asset được mapping tối đa bao nhiêu IoT Device/Sensor trong MVP? | 1 (`BR-13`) | Trả lời đúng | ✅ Correct |

**Nhóm A: 5/5 Correct** — dữ liệu factual rõ ràng nên AI không cần prompt chặt vẫn đúng.

### Nhóm B — Cross-file (phải kết hợp ≥2 file mới trả lời đủ)

| # | Câu hỏi | Đáp án đúng (nguồn) | AI trả lời (Round 1) | Điểm |
|---|---|---|---|---|
| B1 | Khi Technician từ chối Work Order, việc này có làm đổi trạng thái chính thức của WO không? | Không — "từ chối" chỉ là 1 **action**, WO vẫn chỉ có 4 trạng thái chính thức: Assigned/In Progress/Completed/Cancelled (`BR-07`, `BR-17`, `Interview.md` Decision 3) | Trả lời đúng ý nhưng **không nói rõ** đây không phải trạng thái chính thức, dễ gây hiểu nhầm có "trạng thái Rejected" | 🟡 Partial |
| B2 | Một Maintenance Request tạo được tối đa bao nhiêu Work Order? | Tối đa 1 (`BR-06`, `Interview.md` Decision 2) | Trả lời đúng "1" | ✅ Correct |
| B3 | Trước khi đóng (Closed) một Maintenance Request cần điều kiện gì? | Technician hoàn thành WO **và** Facility Manager xác nhận kết quả (`BR-18`, `customer-brief.md` §3.12) | Trả lời đúng, đủ 2 điều kiện | ✅ Correct |
| B4 | Asset cần điều kiện gì trước khi IoT Data của nó được dùng để monitoring? | Phải được mapping với IoT Device/Sensor trước (`BR-13`) | Trả lời đúng | ✅ Correct |
| B5 | Facility Manager nhận Notification trong trường hợp nào? | IoT Alert nghiêm trọng HOẶC Maintenance Risk = High (`REQ-30`, `Interview.md` Decision 4 & 5) | Trả lời đúng cả 2 trường hợp | ✅ Correct |

**Nhóm B: 4/5 Correct, 1 Partial.**

### Nhóm C — Conflict (test khả năng xử lý mâu thuẫn nguồn)

| # | Câu hỏi | Đáp án đúng | AI trả lời (Round 1) | Điểm |
|---|---|---|---|---|
| C1 | `open-questions.md` ghi Q-03 (trạng thái Maintenance Request) là "Open". Vậy đã có câu trả lời chính thức chưa? | Có rồi — `requirements.md`/`business-rules.md` (Baselined) đã định nghĩa đủ 6 trạng thái | AI tin theo `open-questions.md`, trả lời **"chưa có câu trả lời chính thức"** | ❌ Wrong |
| C2 | Tương tự với Q-04 (trạng thái Work Order) | Đã có: 4 trạng thái, baselined trong BR-17/REQ-16 | Tương tự C1, AI nói "chưa xác định" | ❌ Wrong |
| C3 | `customer-brief.md` có ghi chú nghi ngờ về trạng thái `Rejected` của WO, nhưng `Interview.md` đã CONFIRMED khác. Ai đúng? | `Interview.md` (CONFIRMED) đúng | AI nhận ra `Interview.md` ghi "CONFIRMED" nên ưu tiên đúng | ✅ Correct |
| C4 | `MVP-scope.md` liệt kê Must-have "BR-01 → BR-15" trong khi `business-rules.md` có tới BR-18. Vậy BR-16, 17, 18 có thuộc MVP không? | Có — `business-rules.md` là nguồn đầy đủ và chính thức hơn; bảng tổng hợp trong MVP-scope.md chỉ đang liệt kê chưa đủ, cần BA cập nhật, không phải BR-16..18 bị loại khỏi scope | AI chỉ ra được có sự khác biệt nhưng **không kết luận rõ nguồn nào nên tin** | 🟡 Partial |
| C5 | Một Asset có thể gắn nhiều IoT Sensor để đo nhiều chỉ số khác nhau không (VD điều hòa vừa đo nhiệt độ vừa đo điện năng)? | Không — theo `BR-13` (Baselined, High), MVP giới hạn 1 Asset – 1 Sensor | AI suy luận theo thực tế logic: **"Có thể, vì phòng lớn cần nhiều cảm biến"** — bịa thêm ngoài Vault | ❌ Wrong |

**Nhóm C: 1/5 Correct, 1 Partial, 3 Wrong** — đây là nhóm yếu nhất, đúng như dự đoán vì Vault lúc này **chưa có `source-priority.md`**.

### Nhóm D — Unknown (Vault không có dữ liệu, AI phải biết "im lặng đúng chỗ")

| # | Câu hỏi | Đáp án đúng | AI trả lời (Round 1) | Điểm |
|---|---|---|---|---|
| D1 | Ngưỡng (threshold) cụ thể để tạo IoT Alert cho từng Asset Type (VD nhiệt độ bao nhiêu °C với điều hòa) là bao nhiêu? | KHÔNG ĐỦ DỮ LIỆU — `Q-10` vẫn thật sự Open, `Interview.md` Decision 4 ghi rõ "threshold cụ thể... chưa được chốt" | AI **bịa số**: "thường điều hòa cảnh báo khi nhiệt độ vượt 30°C" | ❌ Wrong |
| D2 | Trường đã có đủ Maintenance History thật để huấn luyện AI model chưa? | KHÔNG ĐỦ DỮ LIỆU — `Q-14` Open, `ASM-05` chỉ là assumption có thể dùng dữ liệu giả lập | AI trả lời đúng "chưa xác nhận, có thể dùng dữ liệu mẫu theo ASM-05" | ✅ Correct |
| D3 | Asset có cần quản lý thêm Manufacturer, Model, Warranty không? | KHÔNG ĐỦ DỮ LIỆU — `Q-18` Open | AI trả lời đúng "chưa được xác nhận" | ✅ Correct |
| D4 | Sau MVP, một Asset có thể mapping nhiều Sensor không? | KHÔNG ĐỦ DỮ LIỆU — Vault chỉ baseline cho MVP, không có tài liệu roadmap | AI trả lời đúng "Vault không đề cập giai đoạn sau MVP" | ✅ Correct |
| D5 | Giao diện Dashboard của Facility Manager trông như thế nào? | KHÔNG ĐỦ DỮ LIỆU — chưa có file Design/Wireframe trong Vault | AI trả lời đúng "chưa có tài liệu thiết kế UI" | ✅ Correct |

**Nhóm D: 4/5 Correct, 1 Wrong.**

### Kết quả Round 1

| Nhóm | Correct | Partial | Wrong |
|---|---|---|---|
| A – Factual | 5 | 0 | 0 |
| B – Cross-file | 4 | 1 | 0 |
| C – Conflict | 1 | 1 | 3 |
| D – Unknown | 4 | 0 | 1 |
| **Tổng (20 câu)** | **14** | **2** | **4** |

**Accuracy Round 1 = 14/20 = 70%** → **chưa đạt mục tiêu ≥80%**, đặc biệt nhóm Conflict rất yếu (1/5).

---

## 5. Phân tích nguyên nhân & 3 hành động cải thiện

| # | Vấn đề phát hiện | Nguyên nhân gốc | Hành động sửa |
|---|---|---|---|
| 1 | Nhóm Conflict sai nhiều (C1, C2, C4) | Vault chưa có quy tắc "nguồn nào thắng nguồn nào" | **Tạo `source-priority.md`** (đã tạo — xem file trong `vault/`) |
| 2 | AI bịa số liệu khi thiếu dữ liệu (D1), suy luận ngoài Vault (C5) | Prompt gốc không cấm suy luận / không bắt buộc nói "KHÔNG ĐỦ DỮ LIỆU" | **Cập nhật prompt** thêm quy tắc bắt buộc (xem mục 3) |
| 3 | Gốc rễ của C1/C2: `open-questions.md` bị lỗi thời — nhiều câu đã Resolved từ lâu (theo `Interview.md`) nhưng cột Status vẫn ghi "Open" | Không ai cập nhật lại file sau khi Interview kết thúc | **Cập nhật Status trong `open-questions.md`**: Q-01, Q-02, Q-03, Q-04, Q-05, Q-06, Q-07, Q-08, Q-19, Q-20 → đổi thành `Resolved` (đã ghi thành `DEC-07` trong `decision-log.md`) |

---

## 6. Round 2 — chạy lại sau khi áp dụng 3 hành động trên

Chạy lại đúng 20 câu, dùng **prompt đầy đủ ở mục 3** + Vault đã có `source-priority.md`.

| Nhóm | Câu thay đổi kết quả | Kết quả mới |
|---|---|---|
| B1 | AI giờ nói rõ: *"Đây là action, không phải state chính thức — theo BR-17 chỉ có 4 state"* | ✅ Correct |
| C1 | AI check `source-priority.md`, thấy `requirements.md`/`business-rules.md` (rank 1) thắng `open-questions.md` (rank 8) → trả lời đúng, đồng thời **tự chỉ ra** `open-questions.md` bị lỗi thời | ✅ Correct |
| C2 | Tương tự C1 | ✅ Correct |
| C4 | AI nói rõ `business-rules.md` là nguồn đầy đủ (rank 1) hơn bảng tổng hợp trong `MVP-scope.md` (rank 4), và đề xuất BA rà lại bảng MVP-scope | ✅ Correct |
| C5 | AI từ chối suy luận, trích đúng BR-13, trả lời "1 Asset – 1 Sensor trong MVP" | ✅ Correct |
| D1 | AI trả lời đúng "KHÔNG ĐỦ DỮ LIỆU", trích Q-10 + Interview Decision 4, không bịa số | ✅ Correct |

### Kết quả Round 2

| Nhóm | Correct | Partial | Wrong |
|---|---|---|---|
| A | 5 | 0 | 0 |
| B | 5 | 0 | 0 |
| C | 5 | 0 | 0 |
| D | 5 | 0 | 0 |
| **Tổng** | **20** | **0** | **0** |

**Accuracy Round 2 = 20/20 = 100%** ✅ Đạt và vượt mục tiêu ≥80%.

---

## 7. Round 3 — kiểm tra hồi quy (regression check)

Để chắc chắn việc sửa không làm "gãy" các câu đã đúng trước đó, chạy lại ngẫu nhiên 5 câu ở cả 4 nhóm (A2, B3, C3, D2, D4) sau 1 tuần (khi Vault đã có thêm nội dung mới từ Bài 2). Kết quả: cả 5 câu vẫn Correct → Vault ổn định, không bị "vỡ" khi mở rộng thêm nội dung mới.

> Ghi lại kết quả Round 3 mỗi khi Vault có thay đổi lớn (thêm PRD mới, thêm Design...).

---

## 8. Bài học rút ra (đưa vào retrospective cuối kỳ)

1. AI rất dễ **tin theo trạng thái ghi trong file** (VD "Open") mà không tự kiểm tra xem file khác đã cập nhật câu trả lời chưa — do đó Vault **phải giữ các file trạng thái (open-questions.md) luôn cập nhật**, không được để "Open" mãi sau khi đã Resolved.
2. Không có `source-priority.md`, AI (và cả người đọc) rất dễ xử lý xung đột không nhất quán giữa các lần hỏi.
3. Nếu không cấm rõ trong prompt, AI có xu hướng "giúp đỡ" bằng cách suy luận hợp lý ngoài Vault (VD bịa threshold, bịa khả năng multi-sensor) — điều này nguy hiểm cho một hệ thống cần độ chính xác nghiệp vụ cao.
4. Việc chạy benchmark không chỉ để chấm điểm AI — nó còn giúp **phát hiện lỗi thật trong tài liệu của chính nhóm** (open-questions.md lỗi thời, MVP-scope.md liệt kê thiếu).

## 9. Related Documents

- `source-priority.md` — quy tắc dùng để sửa nhóm Conflict
- `decision-log.md` — DEC-07 (cập nhật Status open-questions.md)
- `ai-usage-log.md` — log chi tiết từng lần chạy AI trong quá trình làm benchmark này
