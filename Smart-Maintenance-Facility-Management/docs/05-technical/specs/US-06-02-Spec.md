# Story Spec - US-06-02 — Xem Maintenance Risk

**Story ID:** US-06-02
**Requirement IDs:** REQ-12, BR-10, BR-15, NFR-08
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện xem mức độ Maintenance Risk của Asset ở ba mức Low, Medium hoặc High để hiểu mức độ rủi ro bảo trì được dự đoán bởi AI và chủ động ưu tiên xử lý.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Asset cần xem Risk đã tồn tại trong bảng `ASSETS`.
- AI Prediction cho Asset đã được thực hiện và lưu trong bảng `AI_PREDICTIONS` (liên kết với `AssetID` và có `PredictedAt`).
- Asset đã có IoT Mapping và Maintenance History (nguồn dữ liệu cho AI — REQ-29, BR-11).

## HAPPY PATH

### H1 — Xem Maintenance Risk của tất cả Asset (Dashboard)
1. Facility Manager truy cập trang AI Predictive Maintenance / Dashboard.
2. Client gửi `GET /api/assets/predictions` (hoặc `GET /api/predictions`) kèm JWT Token.
3. Server trả về `200 OK` với danh sách Asset kèm `RiskLevel` gần nhất từ bảng `AI_PREDICTIONS`.
4. Facility Manager thấy toàn cảnh Risk của từng Asset: `Low`, `Medium`, `High`.
5. Facility Manager **ưu tiên** các Asset có `RiskLevel = High` để lên kế hoạch bảo trì chủ động (BR-15).

### H2 — Xem Maintenance Risk của một Asset cụ thể
1. Facility Manager click vào một Asset cụ thể.
2. Client gửi `GET /api/assets/{id}/prediction` kèm JWT Token.
3. Server truy vấn bảng `AI_PREDICTIONS` lấy prediction gần nhất theo `AssetID`.
4. Server trả về `200 OK` với `{ "risk": "High", "predictedAt": "..." }`.
5. Facility Manager xem được Risk Level và thời điểm prediction được thực hiện.

## ALTERNATE/ERROR PATHS
- **Lỗi 401 Unauthorized:** Xảy ra khi JWT Token không hợp lệ hoặc đã hết hạn.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager`.
- **Lỗi 404 Not Found:** Xảy ra khi `AssetID` không tồn tại, hoặc Asset tồn tại nhưng **chưa có AI Prediction** nào được thực hiện (AI chưa chạy cho Asset này).
- **Trường hợp Prediction cũ:** Nếu `PredictedAt` quá lâu (ví dụ > 24 giờ), server nên trả về thêm cảnh báo `"stale": true` trong response để FM biết prediction có thể không còn chính xác.

## DATA READ/WRITE
- **Read:** Bảng `AI_PREDICTIONS` — đọc `PredictionID`, `AssetID`, `RiskLevel`, `PredictedAt` — lấy bản gần nhất theo `AssetID` (ORDER BY `PredictedAt` DESC LIMIT 1).
- **Read:** Bảng `ASSETS` — đọc thông tin Asset để ghép vào response (`Name`, `Type`, `Location`).
- **Write:** Không có thao tác ghi trong Story này (Facility Manager chỉ xem — AI system ghi prediction qua background job).

## API CONTRACT

### Xem Prediction của một Asset
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

### Xem Risk tổng quan tất cả Asset (Dashboard)
- **Method:** `GET`
- **Endpoint:** `/api/predictions`
- **Auth Level:** FacilityManager
- **Response:** `200 OK`:
```json
[
  {
    "assetId": 1,
    "assetName": "Air Conditioner P301",
    "location": "P301",
    "risk": "High",
    "predictedAt": "2026-09-09T06:00:00"
  },
  {
    "assetId": 2,
    "assetName": "WiFi Router P201",
    "location": "P201",
    "risk": "Low",
    "predictedAt": "2026-09-09T06:00:00"
  }
]
```

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager` (và `Technician` cho endpoint chi tiết Asset).
- **BR-10:** AI chỉ cung cấp thông tin hỗ trợ quyết định — Facility Manager là người quyết định hành động cuối cùng. Hệ thống không tự động tạo Work Order hay Maintenance Request từ AI Prediction.

## VALIDATION/BUSINESS RULES
- **BR-10 (AI chỉ hỗ trợ quyết định — NGUYÊN TẮC CỐT LÕI):** API trả về kết quả AI Prediction dưới dạng thông tin tham khảo. Hệ thống **không được tự động** thực hiện bất kỳ hành động bảo trì nào (không tự tạo WO, không tự thay đổi Asset Status). Toàn bộ quyết định thuộc về Facility Manager (CON-07).
- **BR-15 (FM ưu tiên Asset có Risk = High):** UI/UX nên sắp xếp Asset có `RiskLevel = High` lên đầu danh sách hoặc đánh dấu nổi bật để FM dễ nhận ra và ưu tiên. Server có thể hỗ trợ query param `?sort=risk_desc`.
- **NFR-08 (Mỗi AI Prediction phải liên kết với Asset và thời điểm Prediction):** Response bắt buộc phải có `AssetID` và `PredictedAt` trong mọi trường hợp. Đây là yêu cầu traceability — FM cần biết prediction này được tạo vào lúc nào để đánh giá độ tin cậy.
- **RiskLevel hợp lệ:** Chỉ có 3 giá trị được phép: `Low`, `Medium`, `High`. Không có giá trị nào khác.
- **REQ-12:** Hệ thống hiển thị Maintenance Risk ở 3 mức Low, Medium, High — yêu cầu Must.

## OBSERVABILITY/LOGGING
- Ghi log action `MaintenanceRiskViewed` bằng Serilog, bao gồm:
  - `UserID` (Facility Manager)
  - `AssetID` (hoặc "ALL" nếu xem tổng quan)
  - `RiskLevel` được trả về
  - `PredictedAt` của prediction
  - `Timestamp` của request

## TEST PLAN
- **Unit Test:**
  - `GET /api/assets/1/prediction` với Prediction đã tồn tại → 200 OK với `risk` và `predictedAt`.
  - `GET /api/assets/99/prediction` với AssetID không tồn tại → 404.
  - `GET /api/assets/1/prediction` khi Asset chưa có Prediction → 404.
  - `GET /api/predictions` bởi `FacilityManager` → 200 OK với toàn bộ Asset.
  - `GET /api/assets/1/prediction` bởi `Requester` → 403.
  - `RiskLevel` trong response chỉ là `Low`, `Medium`, hoặc `High` — không có giá trị khác.
  - Prediction cũ hơn 24 giờ → response có `"stale": true`.
  - `PredictedAt` luôn có trong response (NFR-08).
- **Integration Test:**
  - AI background job chạy và insert vào `AI_PREDICTIONS` → FM `GET /api/assets/1/prediction` thấy kết quả.
  - Asset có `RiskLevel = High` xuất hiện đầu danh sách khi sort.
  - FM xem prediction → hệ thống **không** tự tạo WO hay Request (BR-10).

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Response luôn có `AssetID`, `RiskLevel` và `PredictedAt` (NFR-08).
- Hệ thống không thực hiện hành động tự động từ AI Prediction (BR-10, CON-07).
- Commit có chứa mã Story ID `US-06-02`.
