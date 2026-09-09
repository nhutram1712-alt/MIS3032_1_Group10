# Story Spec - US-05-04 — Tiếp nhận và xử lý IoT Alert

**Story ID:** US-05-04
**Requirement IDs:** REQ-10, REQ-30, BR-12, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện nhận và xem IoT Alert khi hệ thống phát hiện điều kiện bất thường từ dữ liệu IoT, để có thể kịp thời phản ứng với các vấn đề bảo trì tiềm ẩn.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Asset đã được mapping với IoT Device/Sensor (BR-13 — US-05-01 đã hoàn thành).
- Hệ thống đã cấu hình threshold bất thường theo từng `Asset Type` (Wi-Fi, Air Conditioner, Projector, Light, Fan).
- IoT Data đang được thu thập (mỗi 5 phút — REQ-28).

## HAPPY PATH

### H1 — Hệ thống tự động phát hiện bất thường và tạo IoT Alert
1. Hệ thống nhận IoT Data mới qua `POST /api/iot/ingest` từ Gateway (không phải user action).
2. Hệ thống tra cứu bảng `IOT_MAPPINGS` để xác định `AssetID` từ `DeviceID`.
3. Hệ thống so sánh `ReadingValue` với threshold đã cấu hình cho `Asset Type`.
4. Nếu vượt threshold → hệ thống tạo IoT Alert và lưu vào bảng `IOT_ALERTS` (hoặc đánh dấu trong `IOT_DATA`).
5. Nếu `RiskLevel = High` hoặc Alert nghiêm trọng → hệ thống gửi Notification cho Facility Manager (REQ-30).

### H2 — Facility Manager xem danh sách IoT Alert
1. Facility Manager truy cập trang IoT Monitoring / Dashboard.
2. Client gửi `GET /api/iot-alerts` kèm JWT Token.
3. Server trả về `200 OK` với danh sách IoT Alert gần nhất, bao gồm thông tin Asset và giá trị gây ra alert.
4. Facility Manager xem được: Asset bị ảnh hưởng, loại bất thường, giá trị đo được vs threshold, thời điểm phát hiện.

### H3 — Facility Manager xem chi tiết IoT Alert của một Asset
1. Facility Manager click vào một Alert cụ thể.
2. Client gửi `GET /api/assets/{id}/iot-data` để xem dữ liệu IoT gần đây của Asset.
3. Server trả về chuỗi dữ liệu IoT, Facility Manager nhận biết xu hướng bất thường.
4. Facility Manager quyết định hành động: tạo Maintenance Request hoặc Work Order, hoặc theo dõi thêm.

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager`.
- **Lỗi 404 Not Found:** Xảy ra khi `GET /api/assets/{id}/iot-data` nhưng Asset không tồn tại hoặc không có IoT Mapping.
- **Trường hợp không có Alert:** Nếu không có Alert nào, server trả về `200 OK` với array rỗng `[]`.
- **Alert từ Device chưa mapping:** Nếu IoT Gateway gửi data từ `DeviceID` chưa được mapping → hệ thống bỏ qua hoặc log warning, không tạo Alert vì không xác định được Asset.

## DATA READ/WRITE
- **Read:** Bảng `IOT_DATA` — đọc `ReadingValue` và `MetricType` để so sánh với threshold (logic tạo Alert).
- **Read:** Bảng `IOT_MAPPINGS` — đọc để resolve `DeviceID` → `AssetID`.
- **Read:** Bảng `ASSETS` — đọc `Type` để xác định threshold cấu hình theo Asset Type.
- **Write:** Bảng `IOT_ALERTS` (hoặc đánh dấu trong `IOT_DATA`) — ghi Alert khi phát hiện bất thường (thực hiện bởi background process/ingestion pipeline, không phải user action).
- **Read:** Bảng `IOT_ALERTS` — đọc để hiển thị danh sách Alert cho Facility Manager.

## API CONTRACT

### Ingest IoT Data (Gateway → Hệ thống, không phải user)
- **Method:** `POST`
- **Endpoint:** `/api/iot/ingest`
- **Auth Level:** Gateway (API Key — không dùng JWT)
- **Request Payload:**
```json
{
  "deviceId": "SENSOR-WIFI-P201",
  "metrics": {
    "temperature": 45.2,
    "signal_strength": -85
  }
}
```
- **Response:** `201 Created` | `400 Bad Request`

### Xem danh sách IoT Alert (Facility Manager)
- **Method:** `GET`
- **Endpoint:** `/api/iot-alerts`
- **Auth Level:** FacilityManager
- **Response:** `200 OK`:
```json
[
  {
    "alertId": 12,
    "assetId": 1,
    "assetName": "WiFi Router P201",
    "assetLocation": "P201",
    "metricType": "temperature",
    "readingValue": 45.2,
    "threshold": 40.0,
    "severity": "High",
    "detectedAt": "2026-09-09T14:25:00"
  }
]
```

### Xem IoT Data chi tiết của Asset
- **Method:** `GET`
- **Endpoint:** `/api/assets/{id}/iot-data`
- **Auth Level:** FacilityManager
- **Response:** `200 OK`:
```json
[
  { "metric": "temperature", "value": 45.2, "time": "2026-09-09T14:25:00" },
  { "metric": "temperature", "value": 44.8, "time": "2026-09-09T14:20:00" }
]
```
- `404 Not Found`: Asset không tồn tại hoặc không có IoT Mapping

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager` cho các endpoint xem Alert và IoT Data.
- Endpoint `POST /api/iot/ingest` sử dụng **API Key** (không dùng JWT) — dành riêng cho IoT Gateway, không phải người dùng.

## VALIDATION/BUSINESS RULES
- **BR-12 (IoT Alert được tạo dựa trên threshold theo Asset Type):** Threshold phải được cấu hình riêng cho từng `Asset Type` (Wi-Fi, Air Conditioner, Projector, Light, Fan). Ví dụ:
  - `Air Conditioner`: nhiệt độ > 35°C → Alert.
  - `Wi-Fi`: signal_strength < -80 dBm → Alert.
  - Threshold phải được đọc từ cấu hình server, không hardcode trong business logic.
- **BR-12 (Alert chỉ khi IoT Data vượt threshold):** Không tạo Alert cho mọi data point — chỉ khi giá trị vượt ngưỡng bất thường đã cấu hình.
- **BR-13 (Device phải mapping trước khi dùng):** Nếu `DeviceID` trong payload `POST /api/iot/ingest` không có mapping trong `IOT_MAPPINGS` → bỏ qua, không lưu vào `IOT_DATA`, không tạo Alert. Log warning để Admin biết có Device chưa được cấu hình.
- **NFR-04 (Lưu timestamp):** `DetectedAt` trong Alert và `Timestamp` trong `IOT_DATA` phải là server-side timestamp — không tin timestamp từ IoT Gateway.
- **REQ-10:** Hệ thống tự động tạo IoT Alert khi IoT Data vượt threshold — đây là yêu cầu Must.
- **REQ-30:** Hệ thống gửi Notification cho Facility Manager khi có IoT Alert nghiêm trọng hoặc `Maintenance Risk = High`. Notification có thể là in-app notification hoặc email (tuỳ implementation).

## OBSERVABILITY/LOGGING
- Ghi log action `IoTAlertCreated` bằng Serilog khi hệ thống phát hiện bất thường, bao gồm:
  - `AssetID`
  - `DeviceID`
  - `MetricType`
  - `ReadingValue` và `Threshold`
  - `Severity`
  - `Timestamp`
- Ghi log action `IoTAlertViewed` khi Facility Manager xem Alert:
  - `UserID` (Facility Manager)
  - `AlertID`
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - IoT Data vượt threshold → Alert được tạo (`IoTAlertCreated` logged).
  - IoT Data trong giới hạn bình thường → không tạo Alert.
  - IoT Data từ `DeviceID` chưa có mapping → bỏ qua, không lưu, log warning.
  - `GET /api/iot-alerts` bởi `FacilityManager` → 200 OK với danh sách Alert.
  - `GET /api/iot-alerts` bởi `Requester` → 403.
  - `GET /api/assets/{id}/iot-data` với Asset không có IoT Mapping → 404.
  - Alert với `Severity = High` → Notification được gửi (mock notification service).
- **Integration Test:**
  - Gửi `POST /api/iot/ingest` với data vượt threshold → Alert được tạo.
  - FM `GET /api/iot-alerts` → thấy Alert vừa tạo với đúng thông tin Asset.
  - FM `GET /api/assets/{id}/iot-data` → thấy chuỗi dữ liệu IoT gần đây.
  - Gửi data từ Device chưa mapping → Alert không được tạo.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Alert được tạo tự động khi IoT Data vượt threshold theo BR-12.
- Notification được gửi cho FM khi có Alert nghiêm trọng (REQ-30).
- Device chưa mapping không tạo Alert, được log warning.
- Commit có chứa mã Story ID `US-05-04`.
