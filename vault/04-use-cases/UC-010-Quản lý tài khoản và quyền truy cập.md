# **UC-010 — Quản lý tài khoản và quyền truy cập**

## **Related Requirements**

* FR-010 — Quản lý tài khoản và quyền truy cập.  
* FR-001 — Đăng nhập hệ thống.  
* NFR-001 — Phạm vi sử dụng nội bộ.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-006 — Khả năng truy vết và nhật ký.

## **Related Business Rules**

* BR-011 — Mô hình vai trò và quyền truy cập.

## **Primary Actor**

Administrator.

## **Supporting Actor**

Dịch vụ quản lý tài khoản người dùng nội bộ của nhà trường.

## **Preconditions**

1. Administrator đã đăng nhập thành công vào hệ thống.  
2. Administrator có quyền quản lý tài khoản và phân quyền.  
3. Người dùng cần quản lý thuộc phạm vi sử dụng nội bộ của nhà trường.

## **Trigger**

Administrator truy cập chức năng quản lý tài khoản và thực hiện thao tác xem, thêm, cập nhật tài khoản hoặc thay đổi vai trò người dùng.

## **Main Flow**

1. Administrator truy cập chức năng quản lý tài khoản.  
2. Hệ thống hiển thị danh sách tài khoản người dùng trong phạm vi hệ thống.  
3. Administrator tìm kiếm hoặc chọn tài khoản cần quản lý.  
4. Hệ thống hiển thị thông tin tài khoản và vai trò hiện tại.  
5. Administrator thực hiện thao tác quản lý tài khoản hoặc phân quyền.  
6. Hệ thống kiểm tra quyền của Administrator.  
7. Hệ thống kiểm tra vai trò được gán có thuộc một trong bốn vai trò hợp lệ hay không:  
   * Customer.  
   * Technician.  
   * Facility Manager.  
   * Administrator.  
8. Hệ thống lưu thông tin tài khoản hoặc thay đổi quyền truy cập.  
9. Hệ thống cập nhật quyền sử dụng chức năng của tài khoản theo BR-011.  
10. Hệ thống ghi nhận thay đổi tài khoản và quyền truy cập vào nhật ký theo NFR-006.  
11. Khi người dùng đăng nhập, hệ thống áp dụng quyền tương ứng với vai trò đã được Administrator gán.

Các trường thông tin tài khoản, giao diện quản lý, cơ chế tạo tài khoản và quy trình khóa/mở khóa tài khoản chưa được định nghĩa chi tiết.

## **Alternative Flows**

### **AF-01 — Thay đổi vai trò người dùng**

1. Administrator chọn một tài khoản.  
2. Administrator chọn vai trò mới cho tài khoản.  
3. Hệ thống kiểm tra vai trò mới thuộc danh sách vai trò được phép.  
4. Hệ thống lưu vai trò mới.  
5. Hệ thống áp dụng quyền tương ứng cho tài khoản.  
6. Hệ thống ghi nhật ký thay đổi quyền.

### **AF-02 — Cập nhật thông tin tài khoản**

1. Administrator chọn tài khoản cần cập nhật.  
2. Administrator chỉnh sửa thông tin được phép thay đổi.  
3. Hệ thống kiểm tra dữ liệu.  
4. Hệ thống lưu thông tin mới.  
5. Hệ thống ghi nhận thay đổi vào nhật ký.

## **Exception Flows**

### **EF-01 — Vai trò không hợp lệ**

1. Administrator cố gắng gán một vai trò không thuộc bốn vai trò được phép.  
2. Hệ thống từ chối thay đổi.  
3. Vai trò hiện tại của tài khoản được giữ nguyên.

### **EF-02 — Tài khoản không thuộc phạm vi hệ thống**

1. Administrator thực hiện thao tác với tài khoản không thuộc phạm vi người dùng nội bộ của nhà trường.  
2. Hệ thống từ chối thao tác.  
3. Tài khoản không được cấp quyền truy cập hệ thống.

### **EF-03 — Administrator không có quyền thực hiện thao tác**

1. Hệ thống kiểm tra quyền của người thực hiện.  
2. Nếu người dùng không có quyền Administrator, hệ thống từ chối thao tác.  
3. Không có thay đổi nào được thực hiện đối với tài khoản hoặc quyền truy cập.

### **EF-04 — Dữ liệu tài khoản không hợp lệ**

1. Hệ thống kiểm tra dữ liệu tài khoản được nhập hoặc cập nhật.  
2. Nếu dữ liệu không hợp lệ, hệ thống từ chối lưu.  
3. Thông tin tài khoản hiện tại được giữ nguyên.

Các quy tắc validation, thông báo lỗi và cơ chế xử lý tài khoản bị khóa chưa được định nghĩa cụ thể.

## **Data Requirements**

### **Confirmed data and controls**

* Tài khoản người dùng nội bộ.  
* Vai trò người dùng.  
* Bốn vai trò: Customer, Technician, Facility Manager và Administrator.  
* Quyền truy cập tương ứng với từng vai trò.  
* Nhật ký thay đổi tài khoản và quyền truy cập.  
* Administrator là người quản lý tài khoản và phân quyền.  
* Quyền truy cập phải tuân thủ BR-011 và NFR-005.

### **Undefined data**

* Các trường thông tin cụ thể của tài khoản.  
* Mã định danh tài khoản.  
* Email, username hoặc mã sinh viên/mã cán bộ sử dụng để định danh.  
* Trạng thái tài khoản như Active/Inactive/Locked.  
* Cơ chế tạo, vô hiệu hóa hoặc mở khóa tài khoản.  
* Quy tắc validation thông tin tài khoản.  
* Cơ chế đồng bộ tài khoản với hệ thống quản lý tài khoản của nhà trường.

## **Postconditions**

### **Success**

1. Thông tin tài khoản được tạo hoặc cập nhật thành công.  
2. Vai trò và quyền truy cập của tài khoản được cập nhật đúng theo BR-011.  
3. Thay đổi tài khoản hoặc quyền được ghi nhận trong nhật ký hệ thống.  
4. Người dùng được áp dụng quyền mới khi sử dụng hệ thống theo cơ chế xác thực được quy định.

### **Failure**

1. Tài khoản hoặc quyền truy cập không được thay đổi.  
2. Hệ thống ghi nhận hoặc thông báo lỗi phù hợp nếu thao tác thất bại.

## **Open Questions**

1. Administrator có được phép tạo tài khoản mới hay chỉ quản lý các tài khoản do nhà trường cấp?  
2. Những trường thông tin nào của tài khoản cần được quản lý?  
3. Tài khoản có các trạng thái Active, Inactive hoặc Locked hay không?  
4. Administrator có được khóa hoặc mở khóa tài khoản không?  
5. Khi thay đổi vai trò, quyền mới có hiệu lực ngay lập tức hay từ lần đăng nhập tiếp theo?  
6. Có giới hạn số lượng Administrator hay không?  
7. Administrator có được thay đổi vai trò của một Administrator khác hay không?  
8. Có cho phép một tài khoản có nhiều vai trò cùng lúc hay mỗi tài khoản chỉ có một vai trò?  
9. Hệ thống có đồng bộ tài khoản với hệ thống quản lý người dùng nội bộ của nhà trường hay không?  
10. Khi tài khoản bị vô hiệu hóa, các phiên đăng nhập đang hoạt động của tài khoản đó được xử lý như thế nào?  
11. Hệ thống cần lưu những thông tin nào trong nhật ký khi thay đổi tài khoản hoặc quyền?  
12. Quy tắc validation cụ thể đối với thông tin tài khoản là gì?

