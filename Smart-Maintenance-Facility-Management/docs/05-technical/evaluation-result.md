# Evaluation Result — AI Prediction Service

> **Dịch vụ được đánh giá:** `src/ai/main.py` (FastAPI 0.1.0, rule-based MVP)
> **Eval set:** `eval-set.csv` — 29 case
> **Cách chạy:** `python run_eval.py` (gọi trực tiếp ứng dụng FastAPI bằng TestClient, không dùng mock)
> **Kết quả thô:** `eval-results.csv`
> **Ngày chạy:** 2026-10-10 (lần chạy trong phiên làm việc này, trên bản code trong `MIS3032_1_Group10-main.zip`)

> ⚠️ **Phạm vi kết luận:** eval này kiểm tra service có chạy đúng luật MVP và đúng guardrail hay không. Nó **không** đo độ chính xác dự đoán hỏng hóc ngoài thực tế (chưa có dữ liệu thật gắn nhãn — Q-14 Open, ASM-05). Expected của nhóm A dựa trên bảng luật ở `ai-feature-spec.md §5` (assumption, Q-10 Open).

## 1. Kết quả tổng hợp

| Nhóm | PASS | Tổng | Tỷ lệ |
|---|---|---|---|
| A — Risk logic (luật + biên) | 15 | 15 | 100% |
| B — Guardrail / Fallback | 8 | 8 | 100% |
| C — Output / Contract / Safety | 3 | 6 | 50% |
| **Tổng** | **26** | **29** | **90%** |

Không có lần AI trả giá trị ngoài {Low, Medium, High}; không có response nào chứa trường tạo Work Order/đổi Asset Status (C02 PASS).

## 2. Các case FAIL (3)

| Case | Mong đợi | Thực tế | Loại | Nhận định |
|---|---|---|---|---|
| **C03** | Response có `riskLevel, confidence, predictedAt, basedOnSampleData, message` (`ai-requirements §5.2`) | Chỉ có `risk`, `horizonDays` | Lệch tài liệu ↔ service | Tài liệu lỗi thời **hoặc** service chưa làm đủ; backend đang dùng contract thực tế (`risk`). Hệ quả: `basedOnSampleData` luôn `false` (G3). |
| **C04** | Chấp nhận header `X-Service-Key` (`ai-requirements §5.1`) | 401 | Lệch tài liệu ↔ service | Code, README và backend đều dùng `X-Api-Key` → sửa tài liệu cho khớp. |
| **C05** | Chỉ có metric lạ (humidity) → 422 "không đủ dữ liệu" | 200, `Low` | Lỗ hổng thiết kế | Không có tín hiệu dùng được mà vẫn trả Low có thể gây yên tâm sai. Mong đợi 422 là suy luận theo tinh thần spec, **cần BA/Engineering xác nhận**. |

## 3. Chi tiết toàn bộ case

| ID | Nhóm | Kịch bản | Mong đợi | Thực tế | Kết quả |
|---|---|---|---|---|---|
| A01 | Risk logic | 25°C, chưa sửa chữa | 200 Low | 200 Low | PASS |
| A02 | Risk logic | Biên 29.9°C | 200 Low | 200 Low | PASS |
| A03 | Risk logic | Biên 30°C | 200 Medium | 200 Medium | PASS |
| A04 | Risk logic | Biên 34.9°C | 200 Medium | 200 Medium | PASS |
| A05 | Risk logic | Biên 35°C | 200 High | 200 High | PASS |
| A06 | Risk logic | 42°C | 200 High | 200 High | PASS |
| A07 | Risk logic | 25°C, sửa 3 lần | 200 Medium | 200 Medium | PASS |
| A08 | Risk logic | 25°C, sửa 2 lần | 200 Low | 200 Low | PASS |
| A09 | Risk logic | Nguồn tắt | 200 High | 200 High | PASS |
| A10 | Risk logic | Nguồn bật | 200 Low | 200 Low | PASS |
| A11 | Risk logic | [25, 36, 26] lấy max | 200 High | 200 High | PASS |
| A12 | Risk logic | 31°C, sửa 5 lần | 200 Medium | 200 Medium | PASS |
| A13 | Risk logic | Chỉ power = 1 | 200 Low | 200 Low | PASS |
| A14 | Risk logic | −5°C | 200 Low | 200 Low | PASS |
| A15 | Risk logic | 36°C + nguồn bật | 200 High | 200 High | PASS |
| B01 | Guardrail | Không có reading | 422 | 422 | PASS |
| B02 | Guardrail | horizon 14 | 400 | 400 | PASS |
| B03 | Guardrail | horizon 1 | 400 | 400 | PASS |
| B04 | Guardrail | Key sai | 401 | 401 | PASS |
| B05 | Guardrail | Thiếu key | 401 | 401 | PASS |
| B06 | Guardrail | Thiếu assetId | 422 | 422 | PASS |
| B07 | Guardrail | value = "abc" | 422 | 422 | PASS |
| B08 | Guardrail | Body rỗng | 422 | 422 | PASS |
| C01 | Output | Output hợp lệ, horizon = 7 | 200 Medium | 200 Medium | PASS |
| C02 | Output | Không có trường tạo WO/đổi Status | 200 High | 200 High | PASS |
| C03 | Output | Đủ field theo §5.2 | 200 + 5 field | thiếu 5 field | **FAIL** |
| C04 | Output | Header X-Service-Key | 200 Low | 401 | **FAIL** |
| C05 | Output | Chỉ metric lạ | 422 | 200 Low | **FAIL** |
| C06 | Output | Metric lạ + 36°C | 200 High | 200 High | PASS |

## 4. Hành động đề xuất

| # | Việc | Người xử lý đề xuất | Mức |
|---|---|---|---|
| 1 | Chốt contract thật (giữ `risk` hay đổi sang `riskLevel`, có `confidence/basedOnSampleData` không) rồi sửa tài liệu hoặc service | Engineering + AI/Vault | Major |
| 2 | Sửa `ai-requirements.md` header thành `X-Api-Key` | Engineering | Minor |
| 3 | Thêm `basedOnSampleData` vào response để thực thi BR-11 | Engineering | Major |
| 4 | Quyết định hành vi khi chỉ có metric lạ (đề xuất 422) rồi thêm test | BA + Engineering | Major |
| 5 | Đưa ngưỡng ra cấu hình hoặc gắn nhãn assumption; sau khi Q-10 chốt thì cập nhật eval | BA + AI/Vault | Major |

## 5. Chạy lại

Sau khi sửa service hoặc đổi quyết định, chạy lại `python run_eval.py` và cập nhật bảng ở mục 1. Mỗi lần chạy lại nên ghi 1 entry vào `ai-usage-log.md`.
