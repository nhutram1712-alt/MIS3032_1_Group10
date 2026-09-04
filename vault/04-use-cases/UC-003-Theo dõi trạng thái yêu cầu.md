UC-003 — Theo dõi trạng thái yêu cầu  
Related Requirements

* FR-003 — Theo dõi trạng thái yêu cầu.  
* FR-002 — Gửi yêu cầu báo sự cố tài sản.  
* NFR-001 — Phạm vi sử dụng nội bộ.  
* NFR-003 — Thời gian phản hồi.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-006 — Khả năng truy vết và nhật ký.

Related Business Rules

* BR-003 — Yêu cầu báo sự cố của Customer.  
* BR-004 — Tạo và phân công Work Order.  
* BR-005 — Vòng đời Work Order.  
* BR-011 — Mô hình vai trò và quyền truy cập.

Primary Actor

Customer.

Customer gồm giảng viên, sinh viên và nhân viên trong trường.

Preconditions

1. Customer có tài khoản hợp lệ.  
2. Customer đã đăng nhập hệ thống.  
3. Customer đã tạo ít nhất một yêu cầu báo sự cố.

Trigger

Customer truy cập chức năng theo dõi yêu cầu.

Main Flow

1. Customer mở danh sách yêu cầu đã gửi.  
2. Hệ thống chỉ hiển thị các yêu cầu do Customer hiện tại tạo.  
3. Hệ thống hiển thị thông tin tài sản, nội dung sự cố và trạng thái hiện tại.  
4. Customer chọn một yêu cầu để xem chi tiết.  
5. Hệ thống hiển thị trạng thái xử lý và Work Order liên quan nếu có.  
6. Customer chỉ được xem thông tin và không được thay đổi trạng thái xử lý.

Alternative Flows

AF-01 — Không chọn yêu cầu cụ thể

1. Customer chỉ xem danh sách các yêu cầu đã gửi.  
2. Hệ thống hiển thị trạng thái hiện tại của từng yêu cầu.

AF-02 — Xem chi tiết yêu cầu

1. Customer chọn một yêu cầu.  
2. Hệ thống kiểm tra yêu cầu thuộc Customer hiện tại.  
3. Hệ thống hiển thị thông tin chi tiết và trạng thái xử lý.

Exception Flows

EF-01 — Không có yêu cầu

1. Hệ thống không tìm thấy yêu cầu nào của Customer.  
2. Hệ thống hiển thị danh sách rỗng.

EF-02 — Yêu cầu không thuộc Customer

1. Hệ thống phát hiện yêu cầu không thuộc Customer hiện tại.  
2. Hệ thống từ chối truy cập.

EF-03 — Yêu cầu không tồn tại

1. Hệ thống không tìm thấy yêu cầu được chọn.  
2. Hệ thống không hiển thị thông tin yêu cầu.

Data Requirements

Input

* Customer ID.  
* Request ID.

Data used and returned

* Request ID.  
* Asset ID và thông tin tài sản.  
* Nội dung sự cố.  
* Trạng thái yêu cầu.  
* Thời gian tạo.  
* Work Order liên quan nếu có.  
* Trạng thái Work Order nếu có.

Postconditions

1. Customer xem được trạng thái hiện tại của yêu cầu do mình tạo.  
2. Dữ liệu yêu cầu không bị thay đổi.  
3. Use case không tạo hoặc cập nhật Work Order.

Open Questions

1. Trạng thái của Customer Request gồm những giá trị nào?  
2. Customer có được xem tên Technician và chi tiết kết quả sửa chữa hay không?  
3. Có cần thông báo cho Customer khi trạng thái yêu cầu thay đổi hay không?  
4. Có cần tìm kiếm hoặc lọc danh sách yêu cầu hay không?

