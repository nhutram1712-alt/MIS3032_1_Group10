# Story Spec - US-05-01 — Mapping Asset với IoT Device/Sensor

**Story ID:** US-05-01
**Requirement IDs:** REQ-26, BR-13, NFR-06
**Design link:** TODO
**Goal:** Cho phép **Admin** thực hiện mapping một Asset với một IoT Device/Sensor để dữ liệu IoT được liên kết đúng với Asset, tạo nền tảng cho việc monitoring và cảnh báo.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Admin`** và có JWT Token hợp lệ.
- Asset cần mapping đã tồn tại trong bảng `ASSETS` với `AssetID` hợp lệ.
- IoT Device/Sensor cần mapping đã tồn tại trong bảng `IOT_DEVICES` với `DeviceID` hợp lệ.
- Asset **chưa có** IoT Mapping hiện tại (mỗi Asset chỉ có một IoT Device/Sensor trong MVP — BR-13).

## HAPPY PATH
1. Admin truy cập trang Quản lý IoT Mapping.
2. Admin xem danh sách Asset chưa được mapping với IoT Device.
3. Admin chọn Asset cần mapping và chọn IoT Device/Sensor tương ứng từ danh sách thiết bị chưa được dùng.
4. Admin xác nhận tạo mapping.
5. Client gửi `POST /api/iot-mappings` với payload `{ "assetId": 1, "deviceId": "SENSOR-001" }`.
6. Server xác thực JWT Token và Role `Admin`.
7. Server kiểm tra `AssetID` tồn tại trong bảng `ASSETS`.
8. Server kiểm tra `DeviceID` tồn tại trong bảng `IOT_DEVICES`.
9. Server kiểm tra Asset **chưa có** mapping nào trong bảng `IOT_MAPPINGS` (enforce BR-13: 1 Asset — 1 Device).
10. Server kiểm tra IoT Device **chưa được mapping** với Asset khác (tránh dùng chung thiết bị).
11. Server insert record mới vào bảng `IOT_MAPPINGS`.
12. Server trả về `201 Created`.
13. Từ thời điểm này, dữ liệu từ `DeviceID` sẽ được liên kết với `AssetID` khi monitoring.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request — Vi phạm BR-13 (1 Asset 1 Device):** Xảy ra khi Asset đã có IoT Mapping tồn tại. Server trả về thông báo: `"Asset đã được mapping với một IoT Device. Hãy cập nhật Mapping hiện có thay vì tạo mới."`.
- **Lỗi 400 Bad Request — Device đã được dùng:** Xảy ra khi `DeviceID` đã được mapping với Asset khác.
- **Lỗi 400 Bad Request — Payload không hợp lệ:** Xảy ra khi thiếu `assetId` hoặc `deviceId`.
- **Lỗi 404 Not Found:** Xảy ra khi `AssetID` không tồn tại trong `ASSETS` hoặc `DeviceID` không tồn tại trong `IOT_DEVICES`.
- **Lỗi 403 Forbidden:** User không có Role `Admin`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `ASSETS` — kiểm tra `AssetID` tồn tại.
- **Read:** Bảng `IOT_DEVICES` — kiểm tra `DeviceID` tồn tại và chưa được mapping.
- **Read:** Bảng `IOT_MAPPINGS` — kiểm tra Asset chưa có mapping hiện tại (enforce BR-13).
- **Write:** Bảng `IOT_MAPPINGS` — Insert record mới với `AssetID`, `DeviceID`, `CreatedAt`.

## API CONTRACT
- **Method:** `POST`
- **Endpoint:** `/api/iot-mappings`
- **Auth Level:** Admin (JWT bắt buộc)
- **Request Payload:**
```json
{
  "assetId": 1,
  "deviceId": "SENSOR-WIFI-P201"
}
```
- **Response:**
  - `201 Created`:
```json
{
  "mappingId": 5,
  "assetId": 1,
  "assetName": "WiFi Router P201",
  "deviceId": "SENSOR-WIFI-P201",
  "createdAt": "2026-09-09T10:00:00"
}
```
  - `400 Bad Request`: Asset đã có mapping, Device đã được dùng, hoặc thiếu field
  - `404 Not Found`: AssetID hoặc DeviceID không tồn tại
  - `403 Forbidden`: Không đủ quyền

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Admin`.
- Chỉ Admin mới được phép tạo và quản lý IoT Mapping — Facility Manager, Technician, Requester không có quyền này.

## VALIDATION/BUSINESS RULES
- **BR-13 (Một Asset chỉ có một IoT Device/Sensor trong MVP — QUAN TRỌNG):** Server phải kiểm tra bảng `IOT_MAPPINGS` trước khi insert. Nếu `AssetID` đã có mapping → trả về `400 Bad Request`. Đây là ràng buộc cứng trong MVP, không có ngoại lệ.
- **BR-13 (IoT Device phải mapping trước khi dùng):** Sau khi mapping được tạo, dữ liệu từ Device mới có thể được sử dụng cho monitoring Asset. Trước khi có mapping, dữ liệu IoT từ Device không có ngữ cảnh Asset.
- **NFR-06 (Khả năng mở rộng):** Thiết kế bảng `IOT_MAPPINGS` phải cho phép thêm trường mới (ví dụ: `MappingType`, `ActiveFrom`) mà không breaking schema hiện tại.
- **REQ-26:** Admin có thể mapping một Asset với một IoT Device/Sensor — yêu cầu Must.

## OBSERVABILITY/LOGGING
- Ghi log action `IoTMappingCreated` bằng Serilog, bao gồm:
  - `AdminUserID` (ai thực hiện)
  - `AssetID`
  - `DeviceID`
  - `MappingID` vừa tạo
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Tạo mapping hợp lệ (Asset và Device chưa được dùng) → 201 Created, record được insert.
  - Tạo mapping với Asset đã có mapping → 400 (BR-13).
  - Tạo mapping với Device đã được mapping với Asset khác → 400.
  - `AssetID` không tồn tại → 404.
  - `DeviceID` không tồn tại → 404.
  - Thiếu `assetId` hoặc `deviceId` → 400.
  - Thực hiện bởi Role không phải `Admin` → 403.
- **Integration Test:**
  - Admin tạo mapping Asset 1 ↔ Device A → 201 Created.
  - Admin thử tạo thêm mapping Asset 1 ↔ Device B → 400 (Asset đã có mapping).
  - Sau khi mapping, `GET /api/assets/1/iot-data` bắt đầu trả về dữ liệu từ Device A.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- BR-13 được enforce: mỗi Asset tối đa một IoT Device trong MVP.
- Dữ liệu IoT chỉ được liên kết với Asset sau khi mapping được tạo.
- Commit có chứa mã Story ID `US-05-01`.
