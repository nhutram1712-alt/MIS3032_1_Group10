# Story Spec - US-06-03 — Xem AI Prediction và IoT Alert

**Story ID:** US-06-03
**Requirement IDs:** REQ-19, BR-10, BR-11, NFR-08
**Design link:** TODO
**Goal:** Cho phép **Technician** thực hiện xem IoT Alert và AI Prediction liên quan đến Asset được giao để hiểu các vấn đề tiềm ẩn trước khi thực hiện bảo trì, giúp chuẩn bị tốt hơn cho công việc thực tế.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Technician`** và có JWT Token hợp lệ.
- Technician đã được phân công ít nhất một Work Order có `AssetID` cụ thể (BR-08).
- Asset liên quan đã có AI Prediction trong bảng `AI_PREDICTIONS` (hoặc trả về 404 nếu chưa có).
- Asset liên quan đã có IoT Mapping và có dữ liệu IoT Alert (hoặc trả về danh sách rỗng nếu chưa có Alert).

## HAPPY PATH

### H1 — Xem AI Prediction của Asset được giao
1. Technician truy cập chi tiết Work Order được phân công.
2. Technician click "Xem AI Prediction" cho Asset liên quan đến Work Order.
3. Client gửi `GET /api/assets/{id}/prediction` kèm JWT Token.
4. Server xác thực JWT Token, xác nhận Role là `Technician`.
5. Server kiểm tra Work Order có `TechnicianID = JWT.UserID` và `AssetID` khớp với Asset được request (tùy chọn enforce — xem Validation).
6. Server truy vấn bảng `AI_PREDICTIONS` lấy Prediction gần nhất cho `AssetID`.
7. Server trả về `200 OK` với `{ "risk": "High", "predictedAt": "..." }`.
8. Technician hiểu mức độ rủi ro AI dự đoán để chuẩn bị dụng cụ và phương án xử lý phù hợp.

### H2 — Xem IoT Alert liên quan đến Asset
1. Trong màn hình chi tiết Work Order, Technician xem phần "Cảnh báo IoT gần đây".
2. Client gửi `GET /api/iot-alerts?assetId={id}` kèm JWT Token.
3. Server lọc Alert theo `AssetID` liên quan đến Work Order của Technician.
4. Server trả về `200 OK` với danh sách IoT Alert gần đây của Asset đó.
5. Technician xem được loại bất thường đã được phát hiện, giá trị cụ thể, và thời điểm xảy ra.
6. Technician sử dụng thông tin này để hiểu vấn đề trước khi đến hiện trường.

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `Technician`.
- **Lỗi 404 Not Found:** Xảy ra khi `AssetID` không tồn tại, hoặc Asset chưa có AI Prediction.
- **Trường hợp chưa có Alert:** Nếu Asset chưa có IoT Alert nào, server trả về `200 OK` với array rỗng `[]` — không phải lỗi.
- **Trường hợp chưa có Prediction:** Nếu AI chưa chạy cho Asset này, server trả về `404 Not Found` với message rõ ràng: `"Chưa có AI Prediction cho Asset này."`.

## DATA READ/WRITE
- **Read:** Bảng `AI_PREDICTIONS` — đọc `RiskLevel` và `PredictedAt` gần nhất theo `AssetID` (ORDER BY `PredictedAt` DESC LIMIT 1).
- **Read:** Bảng `IOT_ALERTS` (hoặc `IOT_DATA` được đánh dấu alert) — đọc Alert gần đây theo `AssetID`.
- **Read:** Bảng `WORK_ORDERS` — đọc để xác minh Technician có WO liên quan đến Asset (tùy chọn enforce ownership).
- **Write:** Không có thao tác ghi. Technician chỉ xem thông tin tham khảo.

## API CONTRACT

### Xem AI Prediction của Asset
- **Method:** `GET`
- **Endpoint:** `/api/assets/{id}/prediction`
- **Auth Level:** FacilityManager, Technician (JWT bắt buộc)
- **Request Payload:** None
- **Response:**
  - `200 OK`:
```json
{
  "assetId": 1,
  "assetName": "Air Conditioner P301",
  "assetType": "Air Conditioner",
  "assetLocation": "P301",
  "risk": "High",
  "predictedAt": "2026-09-09T06:00:00",
  "stale": false
}
```
  - `404 Not Found`: Asset không tồn tại hoặc chưa có Prediction

### Xem IoT Alert của Asset (Technician)
- **Method:** `GET`
- **Endpoint:** `/api/iot-alerts?assetId={id}`
- **Auth Level:** FacilityManager, Technician
- **Request Payload:** None
- **Response:**
  - `200 OK`:
```json
[
  {
    "alertId": 12,
    "assetId": 1,
    "assetName": "Air Conditioner P301",
    "metricType": "temperature",
    "readingValue": 45.2,
    "threshold": 35.0,
    "severity": "High",
    "detectedAt": "2026-09-09T14:25:00"
  }
]
```

**Lưu ý:** Endpoint `GET /api/assets/{id}/prediction` dùng chung với US-06-02. Sự khác biệt chỉ là Role được phép — cả FacilityManager và Technician đều có quyền.

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Technician`.
- **Tuỳ chọn enforce (cần xác nhận):** Server có thể kiểm tra thêm rằng Technician chỉ được xem AI Prediction/Alert của Asset **thuộc Work Order được phân công cho mình**. Nếu enforce, Technician không thể tra cứu Prediction/Alert của Asset không liên quan đến công việc của họ. MVP có thể bỏ qua enforce này nếu phức tạp — cần BA xác nhận.
- **BR-10:** AI Prediction chỉ là thông tin hỗ trợ — Technician xem để tham khảo, không tự động quyết định phương án bảo trì.

## VALIDATION/BUSINESS RULES
- **BR-10 (AI chỉ hỗ trợ quyết định):** Thông tin AI Prediction và IoT Alert hiển thị cho Technician là **thông tin tham khảo** để chuẩn bị và hiểu vấn đề. Hệ thống không tự động chỉ định phương án sửa chữa hay thay thế linh kiện từ AI. Toàn bộ quyết định thực hiện thuộc về Technician (và Facility Manager).
- **BR-11 (AI sử dụng IoT Data và Maintenance History phù hợp):** AI Prediction được tạo từ dữ liệu IoT (`IOT_DATA`) và lịch sử bảo trì (`MAINTENANCE_HISTORY`). Trong MVP, có thể sử dụng dữ liệu mẫu/giả lập. Technician nên được thông báo nếu Prediction dựa trên dữ liệu mẫu (`"basedOnSampleData": true`).
- **NFR-08 (Prediction phải liên kết với Asset và thời điểm):** Response phải luôn có `AssetID` và `PredictedAt`. Technician cần biết prediction được tạo khi nào để đánh giá mức độ liên quan với tình trạng hiện tại.
- **REQ-19:** Technician có thể xem IoT Alert và AI Prediction liên quan đến Asset — yêu cầu Must.

## OBSERVABILITY/LOGGING
- Ghi log action `TechnicianViewedAIPrediction` bằng Serilog, bao gồm:
  - `UserID` (Technician)
  - `AssetID`
  - `RiskLevel` được trả về
  - `PredictedAt` của prediction
  - `Timestamp`
- Ghi log action `TechnicianViewedIoTAlert` bằng Serilog, bao gồm:
  - `UserID` (Technician)
  - `AssetID`
  - Số lượng Alert được trả về
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - `GET /api/assets/1/prediction` bởi `Technician` với Prediction tồn tại → 200 OK với `risk` và `predictedAt`.
  - `GET /api/assets/1/prediction` với Asset chưa có Prediction → 404.
  - `GET /api/iot-alerts?assetId=1` bởi `Technician` → 200 OK với danh sách Alert.
  - `GET /api/iot-alerts?assetId=1` với Asset chưa có Alert → 200 OK với `[]`.
  - `GET /api/assets/1/prediction` bởi `Requester` → 403.
  - Response luôn có `AssetID` và `PredictedAt` (NFR-08).
  - Prediction dựa trên dữ liệu mẫu → `"basedOnSampleData": true` trong response (BR-11).
- **Integration Test:**
  - FM phân công WO cho Technician với AssetID = 1.
  - Technician `GET /api/assets/1/prediction` → nhận AI Prediction.
  - Technician `GET /api/iot-alerts?assetId=1` → nhận danh sách IoT Alert.
  - Technician không thực hiện hành động tự động từ AI info — chỉ đọc (BR-10).

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Technician xem được AI Prediction và IoT Alert cho Asset liên quan đến Work Order.
- Response luôn bao gồm `AssetID` và `PredictedAt` (NFR-08).
- Hệ thống không thực hiện hành động tự động từ AI (BR-10, CON-07).
- Commit có chứa mã Story ID `US-06-03`.

---

> **Lưu ý Implementation — BR-11 (Dữ liệu mẫu/giả lập cho MVP):**
> Trong MVP, nếu AI Prediction được tạo từ dữ liệu mẫu hoặc giả lập (không phải IoT Data và Maintenance History thật), response nên có thêm trường `"basedOnSampleData": true` để Technician biết mức độ tin cậy của prediction. Facility Manager và Technician vẫn được phép xem nhưng nên có cảnh báo phù hợp trên UI.
