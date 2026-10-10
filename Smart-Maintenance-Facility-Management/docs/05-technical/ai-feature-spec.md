# AI Feature Spec — AI Predictive Maintenance (Maintenance Risk 7 ngày)

> **Project:** Smart Maintenance & Facility Management — Trường Đại học Kinh tế, ĐHĐN
> **Story liên quan:** US-06-01, US-06-02, US-06-03
> **Requirement liên quan:** REQ-11, REQ-12, REQ-19, REQ-29, REQ-30, NFR-08
> **Business Rule / Constraint:** BR-10, BR-11, BR-15, CON-05, CON-07
> **Owner:** AI/Vault (phối hợp Engineering)
> **Vị trí gợi ý:** `docs/05-technical/ai-feature-spec.md` (eval nằm ở `docs/05-technical/ai-eval/`)
> **Nguồn chi tiết kỹ thuật:** `ai-requirements.md`, `src/ai/main.py`, `PredictionService.cs` — file này là bản spec AI Feature theo format giáo trình, **không thay thế** `ai-requirements.md`.

---

## 1. Business value

| Mục | Nội dung |
|---|---|
| Người dùng | Facility Manager (chính), Technician (xem tham khảo) |
| Vấn đề | Bảo trì hiện mang tính phản ứng; không biết Asset nào sắp hỏng (`problem-statement.md`). |
| Giá trị | Cho Facility Manager danh sách Asset có nguy cơ cần bảo trì trong **7 ngày tới** để ưu tiên kiểm tra sớm, tạo Work Order chủ động. |
| Không phải | Không phải chatbot; không tự tạo Work Order; không tự đổi Asset Status (BR-10, CON-07). |

## 2. Phạm vi

**Trong MVP:** dự đoán nhu cầu bảo trì 7 ngày, trả mức Low/Medium/High, lưu theo Asset + thời điểm (NFR-08), thông báo khi High (REQ-30).

**Ngoài MVP:** huấn luyện mô hình ML thật, dự đoán 30/90 ngày, phần trăm rủi ro, giải thích nguyên nhân chi tiết. Các câu hỏi Q-11, Q-12, Q-13 trong `open-questions.md` vẫn ghi Open; MVP chọn theo `Interview.md` Decision 5.

## 3. Context / Input

| Nguồn | Chi tiết | Ghi chú |
|---|---|---|
| IoT Data | Tối đa 288 reading gần nhất của thiết bị đã mapping với Asset (≈ 24 giờ nếu 5 phút/lần) | Chỉ Asset đã có IoT Mapping (BR-13) |
| Maintenance History | Số lần bảo trì của Asset (`historyCount`) | Chỉ dùng số đếm trong MVP |
| Khoảng dự đoán | `horizonDays = 7` cố định | DEC-05 |

Các metric AI đang sử dụng: `temperature`, `power_status`. Metric khác bị bỏ qua (xem Eval C05–C06).

## 4. Structured output

```json
{ "risk": "Low | Medium | High", "horizonDays": 7 }
```

Backend lưu vào `AI_PREDICTIONS` kèm `AssetId`, `PredictedAt`, `BasedOnSampleData`. Backend **từ chối lưu** nếu `risk` ngoài 3 giá trị (`PredictionService`, có test `Generate_InvalidRisk_DoesNotPersist`).

## 5. Quy tắc chấm điểm MVP (rule-based — **assumption**)

> Ngưỡng dưới đây nằm trong `src/ai/main.py`, **chưa được baseline trong business rule** (Q-10 còn Open). Phải coi là assumption của nhóm, không phải business truth.

| Thứ tự | Điều kiện | Kết quả |
|---|---|---|
| 1 | `power_status` nhỏ nhất ≤ 0 **hoặc** nhiệt độ lớn nhất ≥ 35°C | High |
| 2 | nhiệt độ lớn nhất ≥ 30°C **hoặc** `historyCount` ≥ 3 | Medium |
| 3 | còn lại | Low |

## 6. Validation

| Lớp | Kiểm tra | Kết quả khi vi phạm |
|---|---|---|
| AI Service | API key hợp lệ | 401 |
| AI Service | `horizonDays == 7` | 400 |
| AI Service | Có ít nhất 1 IoT reading | 422 |
| AI Service | Schema (assetId, kiểu số của value) | 422 |
| Backend | `risk ∈ {Low, Medium, High}` | Không lưu, ghi log lỗi |
| Backend | Asset phải có IoT Mapping và có reading | Không lưu, ghi log warning |

## 7. Fallback & độ tin cậy

| Tình huống | Hành vi |
|---|---|
| Không đủ dữ liệu | Không tạo Prediction; không bịa kết quả; API đọc trả 404 (`GetPrediction_InsufficientData_Returns404`) |
| AI Service timeout / lỗi | Backend thử lại 1 lần; vẫn lỗi thì **giữ Prediction cũ**, ghi log (`Generate_Timeout_KeepsOldRisk`) |
| AI trả giá trị lạ | Từ chối lưu |
| Dữ liệu mẫu | Khi dùng dữ liệu mẫu/giả lập, `basedOnSampleData` phải = `true` (BR-11, ASM-05) — **hiện chưa được thực thi, xem §10** |

## 8. Guardrail — AI không được tự quyết định

- Không tạo Work Order, không đổi Asset Status (test `Generate_Valid_SavesPredictionLinkedToAsset_DoesNotCreateWorkOrder_DoesNotChangeStatus`; eval C02).
- Chỉ Facility Manager quyết định xử lý; High chỉ tạo Notification (REQ-30).
- Trên UI phải ghi rõ AI chỉ hỗ trợ (TC_UC06.1_01 Pass).

## 9. Đánh giá (eval)

- **Eval set:** `ai-eval/eval-set.csv` — 29 case (15 risk logic, 8 guardrail/fallback, 6 output/contract).
- **Cách chạy lại:** `python ai-eval/run_eval.py` (không cần mở server).
- **Kết quả:** xem `ai-eval/evaluation-result.md` — 26/29 PASS.
- **Giới hạn:** eval kiểm tra service chạy đúng luật + guardrail. **Không chứng minh độ chính xác dự đoán ngoài thực tế** vì chưa có dữ liệu thật gắn nhãn (Q-14 Open, ASM-05).

## 10. Khoảng lệch đã phát hiện (cần Engineering/BA xử lý)

| # | Phát hiện | Bằng chứng |
|---|---|---|
| G1 | Response thực tế chỉ có `risk`, `horizonDays`; `ai-requirements.md §5.2` mô tả `riskLevel`, `confidence`, `predictedAt`, `basedOnSampleData`, `message` | Eval C03 |
| G2 | Tài liệu ghi header `X-Service-Key`, code dùng `X-Api-Key` | Eval C04 |
| G3 | Service không trả `basedOnSampleData` → backend luôn lưu `false` dù dữ liệu MVP là mẫu/rule-based | `AiPredictResult`, G1 |
| G4 | Chỉ có metric lạ (vd humidity) vẫn trả `Low` thay vì "không đủ dữ liệu" → có thể gây yên tâm sai | Eval C05 |
| G5 | Ngưỡng 30/35°C hard-code trong `main.py`, trong khi `iot-requirements.md` ghi threshold "cấu hình ở server, không hardcode"; API key mặc định nằm trong code (chỉ chấp nhận cho DEV) | `main.py` |
| G6 | Chưa có test tự động riêng cho service Python (`src/ai`); eval ở đây là bộ kiểm thử đầu tiên | `src/ai` không có `test_*` |
| G7 | Test `TC_UC06.1_03` Failed: UI hiện High/Medium thay vì Cao/Trung bình/Thấp — cần quyết định nhãn (xem đề xuất DEC-11) | `test case.xlsx` |

## 11. Traceability

| REQ | Story | Thành phần | Test/Eval |
|---|---|---|---|
| REQ-11 | US-06-01 | `src/ai/main.py`, `PredictionService` | Eval A01–A15, B02–B03; `PredictionServiceTests` |
| REQ-12 | US-06-02 | `PredictionsPage.tsx` | TC_UC06.1_01–04 |
| REQ-19 | US-06-03 | `AssetDetailPage.tsx` | `GetPrediction_Technician_Allowed` |
| REQ-29 | US-06-01 | `PredictionService` (đọc IoT + History) | Eval B01, `Generate_NoIotData_DoesNotSave` |
| REQ-30 | US-06-01 | `PredictionService` (Notification khi High) | `PredictionApiTests` |
| NFR-08 | US-06-01 | `AI_PREDICTIONS` | `Generate_Valid_Saves...` |
