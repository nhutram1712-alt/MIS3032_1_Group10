# Open Questions
### 4.1. Purpose
Tài liệu này quản lý các vấn đề chưa được xác nhận rõ ràng và cần stakeholder/Business Owner/đội dự án quyết định trước khi hoàn thiện requirement hoặc thiết kế hệ thống.

Open Question không phải là Requirement. Khi một câu hỏi được giải quyết, kết quả sẽ được dùng để cập nhật Requirement/Business Rule liên quan nhưng không thay đổi ID đã tồn tại.

---

### 4.2. Open Questions List

| ID | Open Question | Related Requirement | Priority | Status |
| --- | --- | --- | --- | --- |
| **Q-01** | Requester được xem Asset của phòng/khu vực mình phụ trách hay được xem toàn bộ Asset trong hệ thống? | REQ-02 | High | Open |
| **Q-02** | Maintenance Request có bắt buộc đính kèm hình ảnh/video không? | REQ-04 | Medium | Open |
| **Q-03** | Maintenance Request cần những trạng thái chính thức nào? | REQ-05, BR-14 | High | Open |
| **Q-04** | Work Order cần những trạng thái chính thức nào? | REQ-16, REQ-20 | High | Open |
| **Q-05** | Facility Manager có cần approve Maintenance Request trước khi tạo Work Order không? | REQ-13, REQ-14 | High | Open |
| **Q-06** | Một Maintenance Request có thể tạo nhiều Work Order hay chỉ một Work Order? | REQ-14 | Medium | Open |
| **Q-07** | Technician có được từ chối Work Order hoặc yêu cầu Facility Manager phân công lại không? | REQ-17, REQ-20 | Medium | Open |
| **Q-08** | Sau khi Technician hoàn thành Work Order, Facility Manager có cần xác nhận trước khi đóng Request không? | REQ-22, REQ-23 | High | Open |
| **Q-09** | IoT Alert được tạo dựa trên threshold cố định, AI anomaly detection hay kết hợp cả hai? | REQ-10, BR-12 | High | Open |
| **Q-10** | Threshold bất thường cụ thể của từng Asset Type là bao nhiêu? | REQ-10 | High | Open |
| **Q-11** | AI Prediction sẽ dự đoán khả năng hỏng hóc, nhu cầu bảo trì hay cả hai? | REQ-11 | High | Open |
| **Q-12** | AI Prediction sử dụng prediction horizon bao nhiêu ngày, ví dụ 7/30/90 ngày? | REQ-11 | Medium | Open |
| **Q-13** | Maintenance Risk được hiển thị dưới dạng Low/Medium/High, phần trăm rủi ro hay kết hợp cả hai? | REQ-12 | Medium | Open |
| **Q-14** | Trường Đại học Kinh tế – Đại học Đà Nẵng hiện có đủ Maintenance History để huấn luyện và đánh giá AI model không? | REQ-23, REQ-29, ASM-05 | High | Open |
| **Q-15** | IoT Data được thu thập real-time hay theo chu kỳ? Nếu theo chu kỳ thì interval bao lâu? | REQ-28, NFR-05 | High | Open |
| **Q-16** | Một Asset có thể được mapping với nhiều IoT Device/Sensor không? | REQ-26, REQ-27, BR-13 | Medium | Open |
| **Q-17** | Ai có quyền thay đổi thủ công Asset Status? | REQ-07, REQ-08 | Medium | Open |
| **Q-18** | Asset có cần quản lý thêm Manufacturer, Model, Installation Date, Warranty hoặc Purchase Date không? | REQ-06, REQ-07 | Medium | Open |
| **Q-19** | Facility Manager có nhận Notification khi xuất hiện IoT Alert nghiêm trọng hoặc AI Prediction có Maintenance Risk cao không? | REQ-10, REQ-11, REQ-12 | Medium | Open |
| **Q-20** | Hệ thống sử dụng tài khoản riêng hay tích hợp với hệ thống Identity/SSO hiện có của Trường Đại học Kinh tế – Đại học Đà Nẵng? | REQ-01, NFR-01 | High | Open |

---

### 4.3. Priority Definition

| Priority | Definition |
| --- | --- |
| **High** | Có thể ảnh hưởng trực tiếp đến business flow, architecture hoặc MVP scope |
| **Medium** | Cần xác nhận để hoàn thiện behavior/UI nhưng chưa chặn toàn bộ MVP |
| **Low** | Có thể quyết định ở giai đoạn refinement hoặc implementation |

---

### 4.4. Status Definition

| Status | Definition |
| --- | --- |
| **Open** | Chưa có quyết định chính thức |
| **In Discussion** | Đang trao đổi với stakeholder |
| **Resolved** | Đã có quyết định |
| **Rejected** | Stakeholder xác nhận không cần xử lý |
| **Converted** | Kết quả đã được chuyển thành Requirement/Business Rule/Constraint |
