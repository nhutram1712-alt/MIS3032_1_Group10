# Story Spec - US-05-02 — Cập nhật IoT Mapping

**Story ID:** US-05-02
**Requirement IDs:** REQ-27, BR-13, NFR-06
**Design link:** TODO
**Goal:** Cho phép **Admin** thực hiện cập nhật IoT Mapping để duy trì thông tin kết nối chính xác giữa Asset và IoT Device/Sensor khi thiết bị được thay thế hoặc di chuyển.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Admin`** và có JWT Token hợp lệ.
- IoT Mapping cần cập nhật đã tồn tại trong bảng `IOT_MAPPINGS` (có `MappingID` hợp lệ).
- IoT Device mới (nếu thay thế) đã tồn tại trong bảng `IOT_DEVICES` và **chưa được mapping** với Asset khác.

## HAPPY PATH

### H1 — Thay thế IoT Device cho Asset (đổi Device)
1. Admin truy cập trang Quản lý IoT Mapping.
2. Admin tìm mapping hiện tại của Asset cần thay đổi thiết bị.
3. Admin chọn IoT Device mới để thay thế.
4. Client gửi `PUT /api/iot-mappings/{id}` với payload `{ "deviceId": "SENSOR-WIFI-P201-V2" }`.
5. Server xác thực JWT Token và Role `Admin`.
6. Server kiểm tra `MappingID` tồn tại trong `IOT_MAPPINGS`.
7. Server kiểm tra `DeviceID` mới tồn tại và **chưa được mapping** với Asset khác.
8. Server update `DeviceID` trong bảng `IOT_MAPPINGS`.
9. Server trả về `200 OK`.
10. Từ thời điểm này, dữ liệu IoT từ Device mới sẽ được liên kết với Asset.

### H2 — Xem danh sách IoT Mapping hiện tại
1. Admin truy cập trang Quản lý IoT Mapping.
2. Client gửi `GET /api/iot-mappings`.
3. Server trả về `200 OK` với danh sách tất cả mapping hiện tại (AssetID ↔ DeviceID).

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request — Device đã được dùng:** Xảy ra khi `DeviceID` mới trong payload đã được mapping với Asset khác (vi phạm BR-13: mỗi Device chỉ thuộc một Asset).
- **Lỗi 400 Bad Request — Payload không hợp lệ:** Xảy ra khi `deviceId` trong payload để trống.
- **Lỗi 404 Not Found:** Xảy ra khi `MappingID` trong URL không tồn tại trong `IOT_MAPPINGS`, hoặc `DeviceID` mới không tồn tại trong `IOT_DEVICES`.
- **Lỗi 403 Forbidden:** User không có Role `Admin`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `IOT_MAPPINGS` — đọc theo `MappingID` để kiểm tra tồn tại và lấy `AssetID` hiện tại.
- **Read:** Bảng `IOT_DEVICES` — kiểm tra `DeviceID` mới tồn tại.
- **Read:** Bảng `IOT_MAPPINGS` — kiểm tra `DeviceID` mới chưa được dùng ở mapping khác.
- **Write:** Bảng `IOT_MAPPINGS` — Update cột `DeviceID` theo `MappingID`.

## API CONTRACT

### Cập nhật IoT Mapping
- **Method:** `PUT`
- **Endpoint:** `/api/iot-mappings/{id}`
- **Auth Level:** Admin
- **Request Payload:**
```json
{
  "deviceId": "SENSOR-WIFI-P201-V2"
}
```
- **Response:**
  - `200 OK`:
```json
{
  "mappingId": 5,
  "assetId": 1,
  "assetName": "WiFi Router P201",
  "deviceId": "SENSOR-WIFI-P201-V2",
  "updatedAt": "2026-09-09T11:00:00"
}
```
  - `400 Bad Request`: DeviceID đã được dùng hoặc thiếu field
  - `404 Not Found`: MappingID hoặc DeviceID không tồn tại
  - `403 Forbidden`: Không đủ quyền

### Xem danh sách IoT Mapping
- **Method:** `GET`
- **Endpoint:** `/api/iot-mappings`
- **Auth Level:** Admin
- **Response:** `200 OK`:
```json
[
  {
    "mappingId": 5,
    "assetId": 1,
    "assetName": "WiFi Router P201",
    "assetLocation": "P201",
    "deviceId": "SENSOR-WIFI-P201-V2",
    "createdAt": "2026-09-09T10:00:00"
  }
]
```

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Admin`.
- Chỉ Admin mới được phép chỉnh sửa IoT Mapping.

## VALIDATION/BUSINESS RULES
- **BR-13 (Một Asset chỉ có một IoT Device/Sensor — MVP):** Khi cập nhật mapping, `DeviceID` mới phải là thiết bị **chưa được mapping** với bất kỳ Asset nào khác. Server phải query `IOT_MAPPINGS` để kiểm tra trước khi update.
- **BR-13 (Device phải mapping trước khi dùng):** Sau khi cập nhật, dữ liệu monitoring của Asset sẽ chuyển sang Device mới. Dữ liệu lịch sử từ Device cũ vẫn được giữ nguyên trong `IOT_DATA` (không xóa dữ liệu lịch sử).
- **NFR-06 (Khả năng mở rộng):** Thiết kế API cho phép mở rộng payload (thêm trường như `activeFrom`, `notes`) mà không breaking.
- **REQ-27:** Admin có thể cập nhật IoT Mapping — yêu cầu Must.

## OBSERVABILITY/LOGGING
- Ghi log action `IoTMappingUpdated` bằng Serilog, bao gồm:
  - `AdminUserID` (ai thực hiện)
  - `MappingID`
  - `AssetID`
  - `OldDeviceID` → `NewDeviceID`
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Cập nhật mapping với `DeviceID` mới hợp lệ (chưa được dùng) → 200 OK, record được update.
  - Cập nhật mapping với `DeviceID` đã được mapping với Asset khác → 400 (BR-13).
  - Cập nhật với `MappingID` không tồn tại → 404.
  - Cập nhật với `DeviceID` không tồn tại trong `IOT_DEVICES` → 404.
  - `deviceId` trong payload trống → 400.
  - Thực hiện bởi Role không phải `Admin` → 403.
  - Dữ liệu lịch sử `IOT_DATA` của Device cũ không bị xóa sau khi cập nhật mapping.
- **Integration Test:**
  - Admin cập nhật mapping Asset 1 từ Device A sang Device B.
  - `GET /api/assets/1/iot-data` → dữ liệu mới từ Device B.
  - Lịch sử `IOT_DATA` từ Device A vẫn còn nguyên trong DB.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- BR-13 được enforce: DeviceID mới không được trùng với mapping của Asset khác.
- Dữ liệu lịch sử IoT không bị mất khi cập nhật mapping.
- Commit có chứa mã Story ID `US-05-02`.
