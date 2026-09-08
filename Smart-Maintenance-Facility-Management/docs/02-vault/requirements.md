# Requirements
> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.0
> **Status:** Baselined
> **Role:** Business Analyst

---

## 1. Quy ước

### 1.1. Requirement ID

Mỗi Requirement có một ID duy nhất.

- ID đã được tạo **không được thay đổi** khi nội dung được chỉnh sửa.
- Chỉ tạo ID mới khi xuất hiện một requirement thực sự mới.
- Các Open Question khi được giải quyết sẽ được dùng để cập nhật Requirement hoặc Business Rule liên quan, không tạo lại ID nếu nội dung đã tồn tại.

### 1.2. Source

| Source | Ý nghĩa |
|---|---|
| SRC-USER | Thông tin/yêu cầu được cung cấp hoặc xác nhận trực tiếp bởi stakeholder/user |
| SRC-BA | Requirement được BA chuẩn hóa, phân rã hoặc diễn giải từ thông tin đã có |
| SRC-BA-Q | BA xác định nội dung cần stakeholder xác nhận; sau khi được resolve sẽ cập nhật lại Requirement hoặc Business Rule |

### 1.3. Confidence

| Mức | Ý nghĩa |
|---|---|
| High | Được stakeholder xác nhận hoặc nêu rõ |
| Medium | BA chuẩn hóa/suy luận hợp lý từ context |
| Low | Phụ thuộc vào thông tin chưa được xác nhận |

---

## 2. Functional Requirements

| ID | Requirement | Source | Confidence | Priority |
|---|---|---|---|---|
| REQ-01 | Hệ thống cho phép người dùng đăng nhập bằng tài khoản riêng của hệ thống. | SRC-USER | High | Must |
| REQ-02 | Requester chỉ được xem Asset thuộc phòng/khu vực mình được phép sử dụng. | SRC-USER | High | Should |
| REQ-03 | Requester có thể xem trạng thái hiện tại của Asset thuộc phạm vi được phép. | SRC-USER | High | Should |
| REQ-04 | Requester có thể tạo Maintenance Request để báo cáo vấn đề của Asset/khu vực. Hình ảnh/video là không bắt buộc. | SRC-USER | High | Must |
| REQ-05 | Requester có thể theo dõi Maintenance Request với các trạng thái Submitted, Pending, In Progress, Resolved, Closed hoặc Rejected. | SRC-USER | High | Must |
| REQ-06 | Facility Manager có thể thêm Asset với Asset ID, Name, Type, Location và Status. | SRC-USER | High | Must |
| REQ-07 | Facility Manager có thể cập nhật thông tin cơ bản của Asset. | SRC-USER | High | Must |
| REQ-08 | Facility Manager có thể xem Asset và thay đổi thủ công Asset Status. | SRC-USER + SRC-BA | High | Must |
| REQ-09 | Facility Manager có thể xem IoT Data của Asset. | SRC-USER | High | Must |
| REQ-10 | Hệ thống tạo IoT Alert khi IoT Data vượt threshold/điều kiện bất thường đã cấu hình. | SRC-USER + SRC-BA | High | Must |
| REQ-11 | Hệ thống cung cấp AI Prediction về khả năng Asset cần bảo trì trong 7 ngày tiếp theo. | SRC-USER + SRC-BA | High | Must |
| REQ-12 | Hệ thống hiển thị Maintenance Risk ở 3 mức Low, Medium, High. | SRC-USER + SRC-BA | High | Must |
| REQ-13 | Facility Manager tiếp nhận/xử lý Maintenance Request và xác nhận kết quả trước khi đóng Request. | SRC-USER + SRC-BA | High | Must |
| REQ-14 | Facility Manager có thể tạo một Work Order từ Maintenance Request hoặc nhu cầu bảo trì được xác định. | SRC-USER + SRC-BA | High | Must |
| REQ-15 | Facility Manager có thể phân công Work Order cho Technician. | SRC-USER + SRC-BA | High | Must |
| REQ-16 | Facility Manager có thể theo dõi Work Order với trạng thái Assigned, In Progress, Completed hoặc Cancelled. | SRC-USER + SRC-BA | High | Must |
| REQ-17 | Technician có thể xem các Work Order được phân công cho mình. | SRC-USER | High | Must |
| REQ-18 | Technician có thể xem thông tin Asset liên quan đến Work Order. | SRC-USER + SRC-BA | High | Must |
| REQ-19 | Technician có thể xem IoT Alert và AI Prediction liên quan đến Asset. | SRC-USER | High | Must |
| REQ-20 | Technician có thể cập nhật Work Order được phân công và từ chối Work Order kèm lý do. | SRC-USER + SRC-BA | High | Must |
| REQ-21 | Technician có thể ghi nhận kết quả kiểm tra, sửa chữa hoặc bảo trì. | SRC-USER + SRC-BA | Medium | Must |
| REQ-22 | Technician có thể hoàn thành Work Order sau khi thực hiện và ghi nhận kết quả bảo trì. | SRC-USER + SRC-BA | High | Must |
| REQ-23 | Hệ thống lưu kết quả Work Order hoàn thành vào Maintenance History của Asset. | SRC-USER + SRC-BA | High | Must |
| REQ-24 | Admin có thể quản lý tài khoản người dùng. | SRC-USER + SRC-BA | Medium | Should |
| REQ-25 | Admin có thể quản lý quyền truy cập theo Role. | SRC-USER + SRC-BA | High | Should |
| REQ-26 | Admin có thể mapping một Asset với một IoT Device/Sensor. | SRC-USER + SRC-BA | High | Must |
| REQ-27 | Admin có thể cập nhật IoT Mapping. | SRC-USER + SRC-BA | High | Must |
| REQ-28 | Hệ thống thu thập và lưu trữ IoT Data theo chu kỳ 5 phút/lần. | SRC-USER + SRC-BA | High | Must |
| REQ-29 | Hệ thống sử dụng IoT Data và Maintenance History phù hợp làm nguồn cho AI Prediction. | SRC-USER + SRC-BA | High | Must |
| REQ-30 | Hệ thống gửi Notification cho Facility Manager khi có IoT Alert nghiêm trọng hoặc Maintenance Risk = High. | SRC-USER | High | Must |

---

## 3. Non-Functional Requirements

| ID | Requirement | Source | Confidence | Priority |
|---|---|---|---|---|
| NFR-01 | Hệ thống kiểm soát quyền truy cập dựa trên Role. | SRC-BA | Medium | Must |
| NFR-02 | Hệ thống bảo vệ thông tin xác thực và dữ liệu người dùng khỏi truy cập trái phép. | SRC-BA | Medium | Should |
| NFR-03 | Dữ liệu giữa Maintenance Request, Work Order và Maintenance History phải nhất quán theo lifecycle. | SRC-BA | Medium | Must |
| NFR-04 | Các bản ghi nghiệp vụ quan trọng phải lưu timestamp. | SRC-BA | Medium | Must |
| NFR-05 | Hệ thống hỗ trợ cấu hình chu kỳ thu thập IoT; MVP mặc định 5 phút/lần. | SRC-USER + SRC-BA | High | Should |
| NFR-06 | Hệ thống có khả năng mở rộng số lượng Asset và IoT Device/Sensor. | SRC-BA | Medium | Should |
| NFR-07 | Giao diện cung cấp chức năng và thông tin phù hợp với từng Role. | SRC-BA | Medium | Should |
| NFR-08 | Mỗi AI Prediction phải được liên kết với Asset và thời điểm Prediction. | SRC-BA | Medium | Should |

---

## 4. Constraints

| ID | Constraint |
|---|---|
| CON-01 | Hệ thống phục vụ Trường Đại học Kinh tế – Đại học Đà Nẵng. |
| CON-02 | MVP chỉ quản lý 5 Asset Type: Wi-Fi, Air Conditioner, Projector, Light, Fan. |
| CON-03 | MVP có 4 Role: Requester, Technician, Facility Manager, Admin. |
| CON-04 | MVP bắt buộc tích hợp IoT. |
| CON-05 | MVP bắt buộc có AI phục vụ predictive maintenance. |
| CON-06 | MVP không bao gồm Finance, Procurement, Inventory, Spare Parts, Supplier/Vendor, HR, Academic, Student Management và General University Administration. |
| CON-07 | AI không tự động quyết định hoặc thực hiện bảo trì. |
| CON-08 | MVP sử dụng tài khoản riêng của hệ thống, không tích hợp SSO. |

---

## 5. Assumptions

| ID | Assumption |
|---|---|
| ASM-01 | Người dùng có tài khoản hợp lệ. |
| ASM-02 | Nhà trường có thể cung cấp hoặc tạo Asset Data cho MVP. |
| ASM-03 | Asset MVP có thể kết nối IoT Device/Sensor phù hợp. |
| ASM-04 | Hệ thống có thể nhận IoT Data từ IoT Device/Sensor. |
| ASM-05 | Có thể thu thập hoặc tạo dữ liệu mẫu/giả lập Maintenance History cho AI MVP. |
| ASM-06 | Facility Manager có quyền truy cập dữ liệu cần thiết để quản lý bảo trì. |
| ASM-07 | Technician có thiết bị để truy cập và cập nhật Work Order. |
| ASM-08 | Hệ thống có thể triển khai trên infrastructure/network được nhà trường cho phép. |

---

## 6. Related Documents

- `business-rules.md` — Business Rules
- `open-questions.md` — Open Questions
- `decision-log.md` — Decision Log
- `PRD.md` — Product Requirements Document
- `MVP-scope.md` — MVP Scope
