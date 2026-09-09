# AI Predictive Maintenance Requirements

Tài liệu này định nghĩa các yêu cầu kỹ thuật cho module Trí tuệ Nhân tạo (AI) phục vụ dự đoán nhu cầu bảo trì.

## 1. Mục tiêu mô hình (Model Objective)
- **Bài toán:** Phân loại đa lớp (Multi-class Classification) hoặc Dự đoán xác suất (Probability Prediction).
- **Mục tiêu:** Dự đoán khả năng một Asset (Tài sản) cụ thể sẽ cần bảo trì/gặp sự cố trong khoảng thời gian **7 ngày tiếp theo**.
- **Vai trò:** AI chỉ đóng vai trò **hỗ trợ ra quyết định (Decision Support)**, không tự động tạo Work Order hay thay đổi trạng thái tài sản. Facility Manager là người quyết định cuối cùng.

## 2. Đầu vào Dữ liệu (Data Inputs)
Mô hình AI yêu cầu 2 nguồn dữ liệu chính từ SQL Server Database:
1. **IoT Time-series Data:** Dữ liệu nhiệt độ, độ ẩm, tình trạng nguồn điện,... thu thập mỗi 5 phút/lần.
2. **Maintenance History:** Lịch sử các lần sửa chữa, lỗi thường gặp, và tuổi thọ linh kiện từ bảng `MAINTENANCE_HISTORY`.

*(Ghi chú MVP: Trong giai đoạn MVP, do dữ liệu lịch sử thực tế chưa đủ lớn, nhóm được phép sử dụng bộ dữ liệu mẫu (Mock/Synthetic Dataset) để huấn luyện và chứng minh tính khả thi của mô hình).*

## 3. Đầu ra của Mô hình (Model Outputs)
- Mô hình phải trả về một chỉ số **Maintenance Risk** (Mức độ rủi ro bảo trì) được phân loại thành 3 mức:
  - `Low` (Thấp): Hoạt động bình thường.
  - `Medium` (Trung bình): Có dấu hiệu hao mòn, cần đưa vào kế hoạch kiểm tra định kỳ.
  - `High` (Cao): Rủi ro hỏng hóc lớn trong 7 ngày tới, cần tạo Notification khẩn cấp cho Facility Manager.

## 4. Kiến trúc Tích hợp (Integration Architecture)
- AI Model sẽ được đóng gói thành một Microservice độc lập (sử dụng Python FastAPI/Flask) để dễ dàng scale.
- Backend C# ASP.NET Core sẽ gọi API của AI Service theo định kỳ (ví dụ: Cronjob chạy mỗi đêm) để lấy chỉ số Risk mới nhất cập nhật vào bảng Asset.
