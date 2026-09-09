# AI Predictive Maintenance Requirements

Tài liệu này định nghĩa các yêu cầu kỹ thuật cho module Trí tuệ Nhân tạo (AI) phục vụ dự đoán nhu cầu bảo trì.

---

## 1. Mục tiêu mô hình (Model Objective)

- **Bài toán:** Phân loại đa lớp (Multi-class Classification) hoặc Dự đoán xác suất (Probability Prediction).
- **Mục tiêu:** Dự đoán khả năng một Asset (Tài sản) cụ thể sẽ cần bảo trì/gặp sự cố trong khoảng thời gian **7 ngày tiếp theo**.
- **Vai trò:** AI chỉ đóng vai trò **hỗ trợ ra quyết định (Decision Support)**, không tự động tạo Work Order hay thay đổi trạng thái tài sản. Facility Manager là người quyết định cuối cùng (BR-10, CON-07).

---

## 2. Đầu vào Dữ liệu (Data Inputs)

Mô hình AI yêu cầu 2 nguồn dữ liệu chính từ SQL Server Database:

1. **IoT Time-series Data:** Dữ liệu nhiệt độ, độ ẩm, cường độ tín hiệu, trạng thái nguồn,... thu thập mỗi 5 phút/lần từ bảng `IOT_DATA`.
2. **Maintenance History:** Lịch sử các lần sửa chữa, kết quả bảo trì từ bảng `MAINTENANCE_HISTORY`.

> *(Ghi chú MVP — BR-11, ASM-05): Trong giai đoạn MVP, do dữ liệu lịch sử thực tế chưa đủ lớn, nhóm được phép sử dụng bộ dữ liệu mẫu (Mock/Synthetic Dataset) để huấn luyện và chứng minh tính khả thi của mô hình. Khi dùng dữ liệu mẫu, trường `basedOnSampleData` trong response phải là `true`.*

---

## 3. Đầu ra của Mô hình (Model Outputs)

Mô hình phải trả về một chỉ số **Maintenance Risk** (Mức độ rủi ro bảo trì) được phân loại thành 3 mức:

| RiskLevel | Ý nghĩa | Hành động gợi ý |
|-----------|---------|-----------------|
| `Low` | Hoạt động bình thường | Không cần can thiệp |
| `Medium` | Có dấu hiệu hao mòn | Đưa vào kế hoạch kiểm tra định kỳ |
| `High` | Rủi ro hỏng hóc lớn trong 7 ngày tới | Tạo Notification khẩn cấp cho Facility Manager (REQ-30) |

> ⚠️ Backend phải từ chối lưu bất kỳ giá trị `RiskLevel` nào **không thuộc** `Low`, `Medium`, `High`. Ghi log lỗi nếu AI Service trả về giá trị ngoài 3 mức này.

---

## 4. Kiến trúc Tích hợp (Integration Architecture)

- AI Model được đóng gói thành một **Microservice độc lập** (Python FastAPI hoặc Flask) để dễ dàng scale và deploy riêng biệt.
- Backend C# ASP.NET Core gọi AI Service theo định kỳ qua **Cronjob** (ví dụ: mỗi đêm lúc 2:00 AM) để lấy chỉ số Risk mới nhất cho tất cả Asset có IoT Mapping.
- Kết quả được lưu vào bảng `AI_PREDICTIONS` trong SQL Server (không ghi đè trực tiếp lên bảng `ASSETS`).
- **Authentication Backend → AI Service:** Service-to-service API Key riêng, không lộ ra internet công khai.

---

## 5. JSON Contract — Backend ↔ AI Service (Internal)

> Đây là giao tiếp **nội bộ** giữa Backend C# và AI Microservice. Không expose ra Client.

### 5.1 Request: Backend → AI Service (POST /predict)

Backend gửi dữ liệu tổng hợp của một Asset để AI phân tích:

```json
POST http://ai-service/predict
Content-Type: application/json
X-Service-Key: <internal-api-key>

{
  "assetId": 1,
  "assetType": "Air Conditioner",
  "iotData": [
    {
      "metricType": "temperature",
      "readingValue": 38.5,
      "timestamp": "2026-09-09T14:00:00"
    },
    {
      "metricType": "temperature",
      "readingValue": 37.2,
      "timestamp": "2026-09-09T13:55:00"
    },
    {
      "metricType": "humidity",
      "readingValue": 75.0,
      "timestamp": "2026-09-09T14:00:00"
    }
  ],
  "maintenanceHistory": [
    {
      "completedAt": "2026-07-15T10:00:00",
      "result": "Thay lọc gió, vệ sinh dàn lạnh."
    },
    {
      "completedAt": "2026-04-01T09:00:00",
      "result": "Bổ sung gas lạnh."
    }
  ],
  "useSampleData": false
}
```

#### Giải thích request fields:

| Field | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| `assetId` | `integer` | ✅ | ID của Asset cần dự đoán |
| `assetType` | `string` | ✅ | Loại Asset để AI áp dụng model phù hợp. Giá trị: `Wi-Fi`, `Air Conditioner`, `Projector`, `Light`, `Fan` |
| `iotData` | `array` | ✅ | Mảng các điểm đo IoT gần nhất (gợi ý: 24–48h gần nhất, tối đa 1000 điểm) |
| `iotData[].metricType` | `string` | ✅ | Loại metric: `temperature`, `humidity`, `signal_strength`, `power_status` |
| `iotData[].readingValue` | `float` | ✅ | Giá trị đo được |
| `iotData[].timestamp` | `string (ISO 8601)` | ✅ | Thời điểm đo — server-side timestamp từ bảng `IOT_DATA` |
| `maintenanceHistory` | `array` | ✅ (có thể rỗng `[]`) | Lịch sử bảo trì từ bảng `MAINTENANCE_HISTORY`. Rỗng nếu Asset chưa có lịch sử |
| `maintenanceHistory[].completedAt` | `string (ISO 8601)` | ✅ | Thời điểm hoàn thành bảo trì |
| `maintenanceHistory[].result` | `string` | ✅ | Mô tả kết quả bảo trì |
| `useSampleData` | `boolean` | ✅ | `true` nếu Backend dùng dữ liệu giả lập/mẫu (MVP) thay vì dữ liệu thật |

---

### 5.2 Response: AI Service → Backend

```json
// 200 OK — Dự đoán thành công
{
  "assetId": 1,
  "riskLevel": "High",
  "confidence": 0.87,
  "predictedAt": "2026-09-09T22:00:00",
  "basedOnSampleData": false,
  "message": "Asset có xác suất 87% gặp sự cố trong 7 ngày tới."
}
```

```json
// 422 Unprocessable Entity — Không đủ dữ liệu để dự đoán
{
  "assetId": 1,
  "riskLevel": null,
  "confidence": null,
  "predictedAt": "2026-09-09T22:00:00",
  "basedOnSampleData": false,
  "message": "Không đủ dữ liệu IoT để tạo Prediction đáng tin cậy."
}
```

```json
// 500 Internal Server Error — Lỗi AI Service
{
  "error": "Model inference failed.",
  "detail": "Mô tả kỹ thuật lỗi nội bộ."
}
```

#### Giải thích response fields:

| Field | Kiểu | Mô tả |
|-------|------|-------|
| `assetId` | `integer` | Phản chiếu AssetID đầu vào — để Backend đối soát |
| `riskLevel` | `string` hoặc `null` | `"Low"`, `"Medium"`, `"High"`, hoặc `null` nếu không đủ dữ liệu |
| `confidence` | `float (0.0–1.0)` hoặc `null` | Độ tin cậy của dự đoán. `null` nếu không có Prediction |
| `predictedAt` | `string (ISO 8601)` | Thời điểm AI Service tạo Prediction — Backend sẽ ghi vào `AI_PREDICTIONS.PredictedAt` |
| `basedOnSampleData` | `boolean` | `true` nếu AI dùng dữ liệu mẫu. Backend ghi vào `AI_PREDICTIONS.BasedOnSampleData` |
| `message` | `string` | Mô tả kết quả hoặc lý do không tạo được Prediction |

---

## 6. Hành vi Backend sau khi nhận Response từ AI Service

```
AI Service Response
       ↓
Backend validate: riskLevel ∈ {Low, Medium, High, null}?
       ↓ Hợp lệ
INSERT INTO AI_PREDICTIONS (AssetID, RiskLevel, PredictedAt, BasedOnSampleData)
       ↓ riskLevel = "High"
Tạo Notification cho FacilityManager (REQ-30)
       ↓ riskLevel = null (không đủ dữ liệu)
Không lưu Prediction — ghi log warning "Insufficient data for AssetID=X"
       ↓ Lỗi / Timeout từ AI Service
Không lưu — ghi log error — giữ nguyên Prediction cũ (nếu có)
```

**Quy tắc Backend bắt buộc:**
- `riskLevel` ngoài 3 giá trị hợp lệ → **từ chối lưu, ghi log lỗi**.
- AI Service timeout (gợi ý ngưỡng: 30 giây) → **không lưu, retry ở lần Cronjob tiếp theo**.
- Backend **không tự đặt RiskLevel** khi AI Service lỗi — giữ giá trị cũ trong `AI_PREDICTIONS`.

---

## 7. JSON Contract — Backend → Client (External API)

> Đây là response mà Client (Frontend) nhận từ Backend khi xem AI Prediction. Tham chiếu `api-contract.md` Section 7.

### GET /api/assets/{id}/prediction → 200 OK

```json
{
  "assetId": 1,
  "assetName": "Air Conditioner P301",
  "assetType": "Air Conditioner",
  "assetLocation": "P301",
  "risk": "High",
  "predictedAt": "2026-09-09T22:00:00",
  "stale": false,
  "basedOnSampleData": false
}
```

| Field | Ý nghĩa | Nguồn dữ liệu |
|-------|---------|---------------|
| `risk` | RiskLevel — `Low`, `Medium`, `High` | `AI_PREDICTIONS.RiskLevel` |
| `predictedAt` | Thời điểm tạo Prediction (NFR-08) | `AI_PREDICTIONS.PredictedAt` |
| `stale` | `true` nếu `PredictedAt` > 24 giờ trước | Tính toán server-side |
| `basedOnSampleData` | `true` nếu AI dùng dữ liệu mẫu (BR-11) | `AI_PREDICTIONS.BasedOnSampleData` |

---

## 8. Lịch sử thay đổi (Change Log)

| Phiên bản | Ngày | Thay đổi |
|-----------|------|----------|
| v1.0 | ban đầu | Phiên bản gốc: mục tiêu mô hình, data inputs, 3 mức Risk, kiến trúc tích hợp |
| v1.1 | 2026-09-09 | **[Bổ sung từ Gap Analysis]** Thêm toàn bộ JSON Contract: (1) Backend → AI Service request schema với field-level types; (2) AI Service → Backend response với 3 trường hợp (success/insufficient/error); (3) Backend → Client external API response; bổ sung bảng xử lý hành vi Backend sau AI response; bổ sung note về `confidence` field cần thêm vào `AI_PREDICTIONS` schema |
