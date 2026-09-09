# IoT Technical Requirements

Tài liệu này xác định các yêu cầu kỹ thuật và kiến trúc tích hợp cho hệ thống IoT của dự án Smart Maintenance Facility Management.

---

## 1. Kiến trúc kết nối IoT (IoT Connectivity Architecture)

- **Giao thức (Protocol):** MQTT hoặc HTTP RESTful để truyền tải dữ liệu từ thiết bị (IoT Device/Sensor) về Server.
- **Tần suất thu thập (Polling/Push Interval):** Thu thập dữ liệu theo chu kỳ mặc định **5 phút/lần** (Hard-requirement từ MVP — REQ-28).
- **IoT Gateway/Broker:** Hệ thống sử dụng một lớp Gateway trung gian (ví dụ: EMQX hoặc AWS IoT Core) để hứng dữ liệu trước khi đẩy vào C# ASP.NET Core API qua Webhook hoặc Message Queue.
- **Authentication Gateway → Backend:** API Key trong HTTP header `X-API-Key`. Không dùng JWT người dùng cho luồng ingest.

---

## 2. Ràng buộc Dữ liệu (Data Constraints)

- **Mapping (Liên kết):** Ràng buộc 1-1. Một Asset (Tài sản) chỉ được mapping với tối đa một IoT Device/Sensor trong giai đoạn MVP (BR-13).
- **Dữ liệu bị từ chối:** Nếu `DeviceID` trong payload chưa có Mapping hợp lệ trong bảng `IOT_MAPPINGS`, Backend **bỏ qua payload, không lưu vào `IOT_DATA`**, ghi warning log để Admin biết Device chưa được cấu hình (BR-13).
- **Timestamp:** Server tự ghi `Timestamp` tại thời điểm nhận được request — **không sử dụng trường `timestamp` do Gateway gửi lên** để tránh lỗi đồng hồ thiết bị (NFR-04).

---

## 3. Chuẩn JSON Payload — Gateway → Backend (POST /api/iot/ingest)

> **Đây là payload chuẩn mà IoT Gateway phải tuân thủ khi gọi `POST /api/iot/ingest`.**

```json
{
  "deviceId": "SENSOR-WIFI-P201",
  "metrics": {
    "temperature": 28.5,
    "humidity": 60.2,
    "signal_strength": -72,
    "power_status": 1
  }
}
```

### Giải thích từng field:

| Field | Kiểu dữ liệu | Bắt buộc | Mô tả |
|-------|-------------|----------|-------|
| `deviceId` | `string` |  **Bắt buộc** | Mã định danh thiết bị IoT. Phải khớp với `DeviceID` trong bảng `IOT_DEVICES`. Ví dụ: `"SENSOR-WIFI-P201"` |
| `metrics` | `object` |  **Bắt buộc** | Object chứa một hoặc nhiều metric đo lường. Phải có ít nhất 1 key-value. |
| `metrics.temperature` | `float` |  Tùy Asset Type | Nhiệt độ đo được (°C). Dùng cho Asset Type: `Air Conditioner`, `Projector` |
| `metrics.humidity` | `float` |  Tùy Asset Type | Độ ẩm (%). Dùng cho Asset Type: `Air Conditioner` |
| `metrics.signal_strength` | `float` |  Tùy Asset Type | Cường độ tín hiệu (dBm, giá trị âm). Dùng cho Asset Type: `Wi-Fi` |
| `metrics.power_status` | `integer (0/1)` |  Tùy Asset Type | Trạng thái nguồn điện: `1` = Đang hoạt động, `0` = Tắt/Mất điện. Dùng cho: `Light`, `Fan`, `Projector` |

>  **Lưu ý quan trọng:**
> - Trường `timestamp` **không được gửi lên** từ Gateway và sẽ bị Backend bỏ qua nếu có. Server tự ghi thời điểm nhận.
> - `metrics` object phải có ít nhất 1 metric hợp lệ. Payload thiếu `deviceId` hoặc `metrics` rỗng → Backend trả về `400 Bad Request`.
> - Mỗi metric trong `metrics` object sẽ được tách thành **một dòng riêng** trong bảng `IOT_DATA` với cột `MetricType` tương ứng (phương án schema: 1 metric = 1 row).

### Response từ Backend:

```json
// 201 Created — Ingest thành công
{ "received": true, "deviceId": "SENSOR-WIFI-P201", "metricsCount": 2 }

// 400 Bad Request — Payload không hợp lệ
{ "error": "Payload không hợp lệ: thiếu 'deviceId' hoặc 'metrics' rỗng." }

// 200 OK (Warning) — DeviceID chưa mapping, dữ liệu bị bỏ qua
{ "received": false, "warning": "DeviceID không có Mapping hợp lệ. Dữ liệu bị bỏ qua." }
```

---

## 4. Cấu hình Threshold cảnh báo (Alert Threshold Configuration)

Backend tự động tạo `IOT_ALERTS` khi `ReadingValue` vượt ngưỡng theo `Asset Type` (BR-12). Threshold được cấu hình ở tầng server (không hardcode):

| Asset Type | Metric | Điều kiện Alert | Severity mặc định |
|------------|--------|-----------------|-------------------|
| `Air Conditioner` | `temperature` | > 35°C | `High` |
| `Air Conditioner` | `humidity` | > 80% | `Medium` |
| `Wi-Fi` | `signal_strength` | < -80 dBm | `High` |
| `Projector` | `temperature` | > 45°C | `High` |
| `Projector` | `power_status` | = 0 (mất điện bất ngờ) | `Medium` |
| `Light` | `power_status` | = 0 (mất điện bất ngờ) | `Low` |
| `Fan` | `power_status` | = 0 (mất điện bất ngờ) | `Low` |

> Khi `Severity = High` → hệ thống tạo Notification cho Facility Manager (REQ-30).

---

## 5. Luồng xử lý dữ liệu IoT (IoT Data Processing Flow)

```
IoT Device/Sensor
       ↓ (MQTT/HTTP, mỗi 5 phút)
IoT Gateway (EMQX / AWS IoT Core)
       ↓ (Webhook / Message Queue)
POST /api/iot/ingest [API Key Auth]
       ↓
Backend: Validate payload → Lookup IOT_MAPPINGS (DeviceID → AssetID)
       ↓
Lưu vào IOT_DATA (1 row per metric)
       ↓
So sánh với Threshold theo Asset Type
       ↓ (nếu vượt threshold)
Tạo IOT_ALERTS record → Notification cho FM (nếu Severity = High)
```

---

## 6. Lịch sử thay đổi (Change Log)

| Phiên bản | Ngày | Thay đổi |
|-----------|------|----------|
| v1.0 | ban đầu | Phiên bản gốc: định nghĩa cơ bản về protocol, mapping constraint, payload mẫu |
| v1.1 | 2026-09-09 | **[Bổ sung từ Gap Analysis]** Làm rõ: bỏ trường `timestamp` do Gateway gửi lên; bổ sung field-level schema với kiểu dữ liệu và ràng buộc bắt buộc; xác nhận strategy 1 metric = 1 row trong `IOT_DATA`; thêm response body từ Backend; bổ sung Threshold config table; bổ sung luồng xử lý end-to-end |
