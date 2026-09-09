# IoT Technical Requirements

Tài liệu này xác định các yêu cầu kỹ thuật và kiến trúc tích hợp cho hệ thống IoT của dự án Smart Maintenance Facility Management.

## 1. Kiến trúc kết nối IoT (IoT Connectivity Architecture)
- **Giao thức (Protocol):** MQTT hoặc HTTP RESTful để truyền tải dữ liệu từ thiết bị (IoT Device/Sensor) về Server.
- **Tần suất thu thập (Polling/Push Interval):** Thu thập dữ liệu theo chu kỳ mặc định **5 phút/lần** (Hard-requirement từ MVP).
- **IoT Gateway/Broker:** Hệ thống sử dụng một lớp Gateway trung gian (ví dụ: EMQX hoặc AWS IoT Core) để hứng dữ liệu trước khi đẩy vào C# ASP.NET Core API qua Webhook hoặc Message Queue.

## 2. Ràng buộc Dữ liệu (Data Constraints)
- **Mapping (Liên kết):** Ràng buộc 1-1. Một Asset (Tài sản) chỉ được mapping với tối đa một IoT Device/Sensor trong giai đoạn MVP.
- **Payload Format:** Dữ liệu IoT đẩy về hệ thống Backend bắt buộc phải tuân thủ chuẩn JSON.
  ```json
  {
    "deviceId": "SENSOR_001",
    "timestamp": "2026-09-09T09:00:00Z",
    "metrics": {
      "temperature": 28.5,
      "humidity": 60.2,
      "power_status": 1
    }
  }
