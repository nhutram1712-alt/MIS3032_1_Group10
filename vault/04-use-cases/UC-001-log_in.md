# **UC-001 Đăng nhập hệ thống**

## **Related Requirements**

* FR-001 — Đăng nhập hệ thống.  
* NFR-001 — Phạm vi sử dụng nội bộ.  
* NFR-003 — Thời gian phản hồi.  
* NFR-005 — Bảo mật và phân quyền.

## **Related Business Rules**

* BR-011 — Mô hình vai trò và quyền truy cập.

## **Primary Actor**

Customer (giảng viên, sinh viên, nhân viên), Technician, Facility Manager và Administrator.

## **Supporting Actor**

Dịch vụ xác thực / Quản lý tài khoản người dùng nội bộ của nhà trường.

## **Preconditions**

1. Người dùng có tài khoản hợp lệ do nhà trường cấp.  
2. Tài khoản thuộc một trong bốn vai trò được phép truy cập: Customer, Technician, Facility Manager hoặc Administrator.

## **Trigger**

Thao tác hoặc sự kiện cụ thể bắt đầu luồng xác thực chưa được định nghĩa chi tiết trong Vault.

## **Main Flow**

1. Người dùng truy cập hệ thống quản lý cơ sở vật chất nội bộ.  
2. Người dùng cung cấp thông tin đăng nhập theo tài khoản được cấp.  
3. Hệ thống xác thực danh tính người dùng thông qua kết nối mã hóa TLS.  
4. Hệ thống kiểm tra vai trò được gán cho tài khoản (Customer, Technician, Facility Manager hoặc Administrator).  
5. Hệ thống khởi tạo phiên làm việc hợp lệ và phân quyền chức năng tương ứng theo vai trò được quy định tại BR-011:  
   * Customer: chỉ được gửi báo sự cố và xem yêu cầu của chính mình.  
   * Technician: chỉ xem và cập nhật các Work Order được phân công.  
   * Facility Manager: quản lý tài sản, yêu cầu, Work Order, lịch trình bảo trì và xem cảnh báo AI/IoT.  
   * Administrator: quản lý tài khoản, vai trò và cấu hình hệ thống.  
6. Người dùng truy cập vào giao diện làm việc được phép theo vai trò.

Các bước nhập liệu chi tiết, cơ chế tạo phiên (session/token) và giao diện điều hướng sau khi đăng nhập thành công chưa được định nghĩa trong Vault.

## **Alternative Flows**

Không có luồng đăng nhập thay thế hoặc khôi phục tài khoản nào được xác nhận trong Vault.

## **Exception Flows**

### **EF-01 — Thông tin đăng nhập không hợp lệ**

1. Hệ thống từ chối cấp quyền truy cập phiên làm việc.  
2. Thông điệp hiển thị và phản hồi chi tiết cho người dùng khi nhập sai thông tin chưa được định nghĩa trong Vault.

### **EF-02 — Tài khoản không thuộc phạm vi hợp lệ hoặc bị khóa**

1. Người dùng không được cấp quyền truy cập vào hệ thống nội bộ.  
2. Cơ chế xử lý cụ thể khi tài khoản bị khóa hoặc dịch vụ xác thực lỗi chưa được định nghĩa trong Vault.

## **Data Requirements**

### **Confirmed data and controls**

* Tài khoản người dùng hợp lệ do nhà trường cấp.  
* Mô hình phân quyền gán một trong 4 vai trò: Customer, Technician, Facility Manager, Administrator.  
* Kết nối bảo mật TLS.

### **Undefined data**

* Các trường thông tin đăng nhập cụ thể (tên tài khoản, email, mật khẩu hoặc mã sinh viên/cán bộ).  
* Định danh phiên (Session ID / Token), thời gian tồn tại (lifetime), cơ chế hủy phiên và lưu trữ phiên.  
* Quy tắc kiểm tra tính hợp lệ dữ liệu đầu vào (Validation rules).

## **Postconditions**

### **Success**

1. Người dùng được xác thực thành công vào hệ thống nội bộ.  
2. Người dùng nhận đúng quyền hạn tương ứng với vai trò đã gán.

### **Failure**

1. Người dùng không được cấp quyền truy cập hệ thống.  
2. Không có hành vi trạng thái thất bại bổ sung nào được định nghĩa trong Vault.

## **Open Questions**

1. Thao tác hoặc sự kiện cụ thể nào bắt đầu luồng đăng nhập?  
2. Các bước đăng nhập bắt buộc và điều kiện xác định đăng nhập thành công là gì?  
3. Có yêu cầu luồng đăng nhập thay thế hoặc khôi phục tài khoản hay không?  
4. Hệ thống phải phản hồi thế nào khi sai thông tin đăng nhập, tài khoản không hợp lệ hoặc dịch vụ xác thực lỗi?  
5. Các trường đăng nhập và quy tắc kiểm tra dữ liệu đầu vào cụ thể là gì?  
6. Cần lưu những dữ liệu nào về tài khoản, vai trò và phiên đăng nhập?  
7. Xử lý thế nào với đăng nhập lặp lại, phiên hết hạn, tài khoản bị khóa hoặc người dùng đã đăng nhập?  
8. Có cho phép nhiều phiên đăng nhập đồng thời cho cùng một tài khoản hay không?

