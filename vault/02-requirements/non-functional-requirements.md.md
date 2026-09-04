

# **NFR-001 \- Phạm vi sử dụng nội bộ**

\- **ID:** NFR-001

\- **Tên:** Phạm vi sử dụng nội bộ

\- **Mô tả:** Chỉ người dùng có tài khoản hợp lệ của nhà trường mới được truy cập hệ thống. Quyền sử dụng được giới hạn theo bốn vai trò: Customer, Technician, Facility Manager và Administrator.

\- **Đối tượng:** Customer; Technician; Facility Manager; Administrator

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/01-sources/customer-brief.md; FR-001; BR-011

\- **Trạng thái:** CONFIRMED

 

# **NFR-002 \- Quy mô sử dụng ban đầu**

\- **ID:** NFR-002

\- **Tên:** Quy mô sử dụng ban đầu

\- **Mô tả:** Trong giai đoạn MVP, hệ thống phải hỗ trợ tối thiểu 500 tài khoản người dùng, 100 tài sản Wifi/điều hòa/máy chiếu và 50 phiên truy cập hoạt động đồng thời mà không làm gián đoạn các chức năng chính.

\- **Đối tượng:** N/A \- toàn hệ thống

\- **Mức ưu tiên:** Cao

\- **Nguồn:** Quyết định của Project Owner cho phạm vi MVP và khả năng kiểm thử

\- **Trạng thái:** CONFIRMED

# **NFR-003 \- Thời gian phản hồi**

\- **ID:** NFR-003

\- **Tên:** Thời gian phản hồi

\- **Mô tả:** Ít nhất 95% thao tác đăng nhập, xem tài sản, gửi yêu cầu, xem Work Order và cập nhật trạng thái phải hoàn thành trong vòng 3 giây. Màn hình tổng hợp dữ liệu IoT và kết quả AI phải tải trong vòng 5 giây trong điều kiện vận hành bình thường.

\- **Đối tượng:** N/A \- toàn hệ thống

\- **Mức ưu tiên:** Cao

\- **Nguồn:** Quyết định của Project Owner cho khả năng kiểm thử; FR-002-FR-009

\- **Trạng thái:** CONFIRMED

 

# **NFR-004 \- Tính sẵn sàng**

\- **ID:** NFR-004

\- **Tên:** Tính sẵn sàng

\- **Mô tả:** Hệ thống phải đạt mức sẵn sàng tối thiểu 99% theo tháng trong thời gian phục vụ hoạt động học tập và làm việc của nhà trường. Các đợt bảo trì hệ thống phải được thông báo trước và không được làm mất dữ liệu đã ghi nhận.

\- **Đối tượng:** N/A \- toàn hệ thống

\- **Mức ưu tiên:** Cao

\- **Nguồn:** Quyết định của Project Owner cho phạm vi MVP

\- **Trạng thái:** CONFIRMED

 

# **NFR-005 \- Bảo mật và phân quyền**

\- **ID:** NFR-005

\- **Tên:** Bảo mật và phân quyền

\- **Mô tả:** Hệ thống phải xác thực người dùng, áp dụng phân quyền theo vai trò và sử dụng kết nối mã hóa TLS. Customer chỉ xem yêu cầu của mình; Technician chỉ xem Work Order được giao; Facility Manager quản lý tài sản và bảo trì; Administrator quản lý tài khoản và quyền truy cập.

\- **Đối tượng:** Customer; Technician; Facility Manager; Administrator

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** FR-001; FR-010; BR-003; BR-011

\- **Trạng thái:** CONFIRMED

 

# **NFR-006 \- Khả năng truy vết và nhật ký**

\- **ID:** NFR-006

\- **Tên:** Khả năng truy vết và nhật ký

\- **Mô tả:** Hệ thống phải ghi nhật ký các thay đổi quan trọng như cập nhật tài sản, tạo/phân công Work Order, thay đổi trạng thái Work Order, thay đổi quyền người dùng và kết quả cảnh báo AI/IoT. Nhật ký phải có thời gian, đối tượng liên quan và người thực hiện khi có.

\- **Đối tượng:** System; Technician; Facility Manager; Administrator

\- **Mức ưu tiên:** Cao

\- **Nguồn:** FR-004; FR-007-FR-010; BR-005; BR-010; BR-011

\- **Trạng thái:** CONFIRMED

 

# **NFR-007 \- Độ tin cậy dữ liệu IoT**

\- **ID:** NFR-007

\- **Tên:** Độ tin cậy dữ liệu IoT

\- **Mô tả:** Mỗi bản ghi IoT phải gắn với đúng Asset ID và thời điểm ghi nhận. Khi thiết bị IoT mất kết nối hoặc dữ liệu quá cũ, hệ thống phải hiển thị trạng thái dữ liệu không còn cập nhật thay vì xem đó là dữ liệu bình thường. Việc mất kết nối IoT không được ngăn Customer báo lỗi thủ công.

\- **Đối tượng:** System; Facility Manager; Technician

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** FR-005; BR-006; BR-007

\- **Trạng thái:** CONFIRMED

 

# **NFR-008 \- Tính minh bạch và an toàn của AI**

\- **ID:** NFR-008

\- **Tên:** Tính minh bạch và an toàn của AI

\- **Mô tả:** Kết quả AI phải hiển thị tối thiểu tài sản liên quan, mức rủi ro và khuyến nghị kiểm tra/bảo trì. AI chỉ hỗ trợ dự đoán và không được tự động tạo quyết định cuối cùng, tự phân công Technician hoặc tự đóng Work Order.

\- **Đối tượng:** AI System; Facility Manager

\- **Mức ưu tiên:**  Quan trọng

\- **Nguồn:** FR-006; BR-008; BR-009

\- **Trạng thái:** CONFIRMED

 

# **NFR-009 \- Khả năng khôi phục dữ liệu**

\- **ID:** NFR-009

\- **Tên:** Khả năng khôi phục dữ liệu

\- **Mô tả:** Dữ liệu tài sản, yêu cầu sự cố, Work Order và lịch sử bảo trì phải được sao lưu định kỳ. Mục tiêu điểm khôi phục (RPO) tối đa là 24 giờ và mục tiêu thời gian khôi phục (RTO) tối đa là 4 giờ sau sự cố nghiêm trọng.

\- **Đối tượng:** N/A \- toàn hệ thống

\- **Mức ưu tiên:** Cao

\- **Nguồn:** Quyết định của Project Owner cho khả năng khôi phục hệ thống

\- **Trạng thái:** CONFIRMED

# **NFR-010 \- Lưu trữ lịch sử bảo trì**

\- **ID:** NFR-010

\- **Tên:** Lưu trữ lịch sử bảo trì

\- **Mô tả:** Lịch sử sự cố và bảo trì của tài sản phải được lưu tối thiểu 2 năm để phục vụ tra cứu và làm dữ liệu tham khảo cho AI. Sau thời hạn này, dữ liệu có thể được lưu trữ dài hạn hoặc ẩn danh theo chính sách dữ liệu của nhà trường.

\- **Đối tượng:** N/A \- toàn hệ thống

\- **Mức ưu tiên:** Cao

\- **Nguồn:** FR-004; FR-009; BR-010; quyết định của Project Owner cho thời hạn lưu trữ

\- **Trạng thái:** CONFIRMED

