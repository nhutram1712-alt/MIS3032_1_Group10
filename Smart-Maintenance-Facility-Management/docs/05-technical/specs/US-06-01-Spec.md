
# Story Spec -  US-06-01 — Dự đoán nhu cầu bảo trì bằng AI

**Story ID:** US-06-01

**Requirement IDs:** REQ-11, REQ-29, BR-10, BR-11, BR-15

**Design link:** TODO

**Goal:** Cho phép hệ thống tạo AI Prediction về khả năng một Asset cần bảo trì trong 7 ngày tiếp theo, dựa trên dữ liệu IoT và Maintenance History, để hỗ trợ Facility Manager chủ động ưu tiên bảo trì.

### Preconditions

- Asset đã có dữ liệu IoT phù hợp (đã mapping IoT Device và có dữ liệu được thu thập, xem US-05-03).
- Hệ thống có Maintenance History phù hợp cho Asset, hoặc sử dụng dữ liệu mẫu/giả lập theo ASM-05 nếu dữ liệu thực chưa đủ (được phép trong MVP).
- AI Prediction Service (Python FastAPI/Flask microservice) đã được triển khai và có thể nhận request từ Backend.

### Happy path

1. Cronjob Backend (chạy định kỳ, ví dụ mỗi đêm) tổng hợp IoT Data + Maintenance History phù hợp cho từng Asset.
2. Backend gọi API của AI Prediction Service, gửi dữ liệu đầu vào.
3. AI Service phân tích dữ liệu, trả về kết quả: khả năng Asset cần bảo trì trong 7 ngày tới, cùng Maintenance Risk (`Low`/`Medium`/`High`).
4. Backend nhận kết quả, lưu Prediction mới, liên kết với AssetID và thời điểm thực hiện Prediction (NFR-08).
5. Backend cập nhật chỉ số Maintenance Risk mới nhất vào Asset.
6. Nếu Risk = `High`, hệ thống tạo Notification cho Facility Manager (liên quan REQ-30/US-05-04, US-06-02).
7. Facility Manager xem kết quả Prediction khi truy cập Asset.

### Alternate/error paths

- Asset không có đủ dữ liệu IoT/Maintenance History phù hợp: hệ thống không tạo Prediction đáng tin cậy, hiển thị trạng thái "Chưa đủ dữ liệu để dự đoán" thay vì trả về Risk mặc định.
- AI Prediction Service trả lỗi hoặc timeout: Backend không lưu kết quả không hợp lệ, ghi nhận lỗi xử lý, giữ nguyên Risk cũ (nếu có) cho tới lần chạy tiếp theo.
- AI Service trả về giá trị Risk không thuộc 3 mức hợp lệ: Backend từ chối lưu, ghi log lỗi dữ liệu từ AI Service.

### Data read/write

- **Read:** Bảng `IOT_DATA` (dữ liệu time-series 5 phút/lần); Bảng `MAINTENANCE_HISTORY` (lịch sử sửa chữa, lỗi thường gặp — hoặc dữ liệu mẫu/giả lập trong MVP theo ASM-05).
- **Write:** Cập nhật trường Risk hiện tại trên bảng `ASSETS`.
- **Gap cần lưu ý:** Cần bổ sung bảng lưu kết quả Prediction (ví dụ `AI_PREDICTIONS`: PredictionID, AssetID FK, RiskLevel, PredictedAt) — bảng này **chưa có** trong `data-requirements.md` hiện tại, cần bổ sung trước khi implement để đảm bảo NFR-08 (Prediction phải liên kết Asset + thời điểm).

### API contract

- **Backend → AI Service:** Nội bộ, không thuộc `api-contract.md` (giao tiếp giữa 2 service, ví dụ `POST http://ai-service/predict`, Payload chứa IoT Data + Maintenance History tổng hợp theo Asset).
- **Client → Backend (xem kết quả):** Chưa có endpoint chính thức trong `api-contract.md`. Đề xuất: `GET /api/assets/{id}/prediction`, Auth Level: `FacilityManager`/`Technician`, Response: `200 OK: { "risk": "High", "predictedAt": "..." }`.
- Cần bổ sung chính thức các endpoint trên vào `api-contract.md` trước khi implement.

### Authorization

- Lệnh gọi AI Service từ Backend: xác thực nội bộ (service-to-service, ví dụ API Key riêng), không lộ ra ngoài internet công khai.
- Endpoint xem kết quả Prediction: JWT Token, Role `FacilityManager` (và `Technician` trong phạm vi Work Order theo REQ-19).

### Validation/business rules

- AI chỉ đóng vai trò hỗ trợ quyết định, không tự động tạo Work Order hay thay đổi Asset Status (BR-10, CON-07).
- Prediction chỉ dự đoán trong khung thời gian 7 ngày tới (BR-11).
- Kết quả Risk chỉ được nhận 1 trong 3 giá trị: `Low`, `Medium`, `High` (REQ-12).
- Mỗi Prediction phải được liên kết với đúng Asset và thời điểm thực hiện (NFR-08).
- Trong MVP, được phép dùng dữ liệu mẫu/giả lập nếu Maintenance History thực tế chưa đủ (BR-11, ASM-05).

### Observability/logging

- Ghi log mỗi lần gọi AI Service bằng Serilog (AssetID, thời gian gọi, thời gian phản hồi, kết quả Risk trả về, hoặc lỗi nếu có).
- Ghi log riêng khi Prediction bị từ chối do thiếu dữ liệu hoặc lỗi từ AI Service, phục vụ theo dõi chất lượng mô hình theo thời gian.

### Test plan

- Unit Test: Validate response từ AI Service — chỉ chấp nhận Risk thuộc `Low`/`Medium`/`High`.
- Integration Test: Mock AI Service trả kết quả hợp lệ → assert Prediction được lưu, liên kết đúng Asset + timestamp, Asset.Risk được cập nhật.
- Integration Test: Mock AI Service timeout/lỗi → assert hệ thống không lưu kết quả sai, ghi log lỗi, Risk cũ không bị ghi đè bằng giá trị rác.
- Integration Test: Asset không đủ dữ liệu đầu vào → assert hệ thống trả về trạng thái "Chưa đủ dữ liệu", không lưu kết quả không đáng tin cậy.

### Definition of Done

- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- Bảng lưu kết quả Prediction (ví dụ `AI_PREDICTIONS`) đã được bổ sung vào schema chính thức và được BA/DBA xác nhận.
- Endpoint xem kết quả Prediction đã được cập nhật vào `api-contract.md`.
- Cronjob gọi AI Service chạy ổn định, có cơ chế retry/log lỗi khi AI Service không phản hồi.
- Commit có chứa mã Story ID.
