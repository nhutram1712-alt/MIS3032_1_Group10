
# Story Spec

**Story ID:** US-05-03

**Requirement IDs:** REQ-09, REQ-28, BR-12, BR-13

**Design link:** TODO

**Goal:** Cho phép hệ thống nhận và lưu trữ dữ liệu IoT từ các Asset đã được mapping, đồng thời cho phép Facility Manager xem dữ liệu IoT hiện tại của một Asset để giám sát tình trạng hoạt động.

### Preconditions

- Asset đã được Admin mapping với một IoT Device/Sensor cụ thể (BR-13).
- IoT Device/Sensor có khả năng gửi dữ liệu qua Gateway (MQTT/HTTP) theo `iot-technical-requirements.md`.
- Người dùng xem dữ liệu đã đăng nhập với Role `FacilityManager`.

### Happy path

1. IoT Device gửi dữ liệu định kỳ (5 phút/lần) tới IoT Gateway theo payload JSON chuẩn (`deviceId`, `timestamp`, `metrics`).
2. Gateway đẩy dữ liệu về Backend qua Webhook/Message Queue.
3. Hệ thống xác định Asset tương ứng thông qua IoT Mapping (DeviceID → AssetID).
4. Hệ thống lưu dữ liệu vào bảng `IOT_DATA`.
5. Facility Manager truy cập chi tiết Asset.
6. Hệ thống hiển thị dữ liệu IoT gần nhất (và/hoặc lịch sử ngắn hạn) của Asset đó.

### Alternate/error paths

- Asset chưa được mapping với IoT Device: hệ thống không thể liên kết dữ liệu, ghi log cảnh báo cấu hình chưa hoàn tất (BR-13).
- Không nhận được dữ liệu mới trong một chu kỳ: hệ thống ghi nhận trạng thái "không có dữ liệu mới", không coi là lỗi hệ thống.
- Payload JSON không đúng định dạng chuẩn: hệ thống từ chối tại Webhook endpoint, trả lỗi 400 Bad Request.
- Facility Manager xem Asset chưa có IoT Data: hệ thống hiển thị thông báo "Asset chưa có dữ liệu IoT được liên kết" thay vì báo lỗi.

### Data read/write

- **Read:** Bảng `IOT_MAPPINGS` (xác định AssetID tương ứng với DeviceID gửi dữ liệu); Bảng `ASSETS` (khi FM xem thông tin Asset).
- **Write:** Bảng `IOT_DATA` (Tạo bản ghi mới: DeviceID, ReadingValue, Timestamp cho mỗi lần nhận dữ liệu).
- **Gap cần lưu ý:** Theo `iot-technical-requirements.md`, payload thực tế gồm nhiều metric cùng lúc (`temperature`, `humidity`, `power_status`), nhưng bảng `IOT_DATA` trong `data-requirements.md` hiện chỉ có một cột `ReadingValue` duy nhất. Cần làm rõ với BA/DBA: (a) tách mỗi metric thành một dòng riêng (thêm cột `MetricType`), hoặc (b) mở rộng bảng để lưu đủ 3 metric trên cùng một dòng. Story này tạm giả định phương án (a) cho tới khi có xác nhận chính thức.

### API contract

- **Ghi nhận dữ liệu (Device → Backend):** Chưa có endpoint chính thức trong `api-contract.md`. Đề xuất: `POST /api/iot/ingest` (Internal, gọi từ IoT Gateway), Payload theo chuẩn JSON của `iot-technical-requirements.md`.
- **Xem dữ liệu (FM):** Chưa có endpoint chính thức trong `api-contract.md`. Đề xuất: `GET /api/assets/{id}/iot-data`, Auth Level: `FacilityManager`, Response: `200 OK: [ { "metricType": "temperature", "value": 28.5, "timestamp": "..." } ]`.
- Cả hai endpoint trên cần được BA/Tech Lead xác nhận và bổ sung chính thức vào `api-contract.md` trước khi implement.

### Authorization

- Endpoint ingest dữ liệu: xác thực bằng API Key/Secret riêng của IoT Gateway (không dùng JWT người dùng).
- Endpoint xem dữ liệu: JWT Token, bắt buộc Role `FacilityManager` (có thể mở rộng cho Technician trong phạm vi Work Order theo REQ-19/US-06-03, không thuộc scope Story này).

### Validation/business rules

- Một Asset chỉ được mapping với tối đa một IoT Device/Sensor trong MVP (BR-13).
- Dữ liệu IoT chỉ được lưu và sử dụng cho monitoring khi Asset đã có Mapping hợp lệ (BR-13).
- Chu kỳ thu thập mặc định là 5 phút/lần (REQ-28, NFR-05).

### Observability/logging

- Ghi log mỗi lần nhận IoT Data thành công/thất bại bằng Serilog (DeviceID, AssetID, Timestamp, kết quả xử lý).
- Cảnh báo (warning log) khi không nhận được dữ liệu từ một Device đã mapping quá 1-2 chu kỳ liên tiếp — hỗ trợ phát hiện Device offline sớm, phục vụ vận hành.

### Test plan

- Unit Test: Validate payload JSON đúng chuẩn; từ chối payload thiếu `deviceId` hoặc `metrics`.
- Integration Test: Gửi IoT Data hợp lệ cho Asset đã mapping → assert dữ liệu được lưu đúng vào `IOT_DATA`.
- Integration Test: Gửi IoT Data cho DeviceID chưa mapping với Asset nào → assert dữ liệu bị từ chối/ghi log cảnh báo, không tạo bản ghi lạc.
- Integration Test: FM xem Asset chưa có IoT Data → assert hiển thị thông báo phù hợp, không lỗi 500.

### Definition of Done

- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Endpoint ingest và endpoint xem dữ liệu đã được thống nhất và cập nhật chính thức vào `api-contract.md`.
- Schema `IOT_DATA` (đơn metric hay đa metric) đã được BA/DBA chốt trước khi release.
- Commit có chứa mã Story ID.
