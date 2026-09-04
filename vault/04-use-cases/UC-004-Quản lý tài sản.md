UC-004 — Quản lý tài sản  
Related Requirements

* FR-004 — Quản lý tài sản.  
* FR-005 — Giám sát thiết bị qua IoT.  
* FR-011 — Quản lý lịch trình bảo trì tài sản.  
* NFR-001 — Phạm vi sử dụng nội bộ.  
* NFR-003 — Thời gian phản hồi.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-006 — Khả năng truy vết và nhật ký.  
* NFR-007 — Độ tin cậy dữ liệu IoT.

Related Business Rules

* BR-001 — Định danh và thông tin tài sản.  
* BR-002 — Phạm vi và vị trí tài sản.  
* BR-006 — Dữ liệu IoT phải gắn với tài sản.  
* BR-010 — Lưu lịch sử bảo trì.  
* BR-011 — Mô hình vai trò và quyền truy cập.

Primary Actor

Facility Manager.

Preconditions

1. Facility Manager có tài khoản hợp lệ.  
2. Facility Manager đã đăng nhập hệ thống.  
3. Facility Manager có quyền quản lý tài sản.

Trigger

Facility Manager truy cập chức năng quản lý tài sản.

Main Flow

1. Facility Manager mở danh sách tài sản.  
2. Hệ thống hiển thị Asset ID, tên tài sản, loại, vị trí và trạng thái.  
3. Facility Manager có thể tạo mới hoặc cập nhật thông tin tài sản.  
4. Hệ thống kiểm tra Asset ID là duy nhất và thông tin tài sản hợp lệ.  
5. Hệ thống lưu thông tin tài sản.  
6. Facility Manager có thể xem lịch sử sự cố và bảo trì của tài sản.  
7. Nếu tài sản có IoT, hệ thống hiển thị dữ liệu IoT được gắn với Asset ID.

Alternative Flows

AF-01 — Tạo tài sản mới

1. Facility Manager nhập thông tin tài sản.  
2. Hệ thống kiểm tra dữ liệu.  
3. Hệ thống tạo tài sản nếu Asset ID chưa tồn tại.

AF-02 — Cập nhật tài sản

1. Facility Manager chọn tài sản cần cập nhật.  
2. Facility Manager thay đổi thông tin.  
3. Hệ thống lưu thay đổi và ghi nhật ký.

AF-03 — Tài sản không có kết nối IoT

1. Hệ thống không nhận được dữ liệu IoT.  
2. Hệ thống vẫn cho phép Facility Manager quản lý tài sản.  
3. Trạng thái dữ liệu IoT được hiển thị là không cập nhật.

Exception Flows

EF-01 — Asset ID bị trùng

1. Hệ thống phát hiện Asset ID đã tồn tại.  
2. Hệ thống từ chối tạo tài sản.

EF-02 — Thông tin tài sản không hợp lệ

1. Hệ thống phát hiện thiếu hoặc sai thông tin bắt buộc.  
2. Hệ thống từ chối lưu dữ liệu.

EF-03 — Dữ liệu IoT không xác định được tài sản

1. Hệ thống không xác định được Asset ID của dữ liệu IoT.  
2. Hệ thống không sử dụng dữ liệu đó cho phân tích AI.

Data Requirements

Input

* Asset ID.  
* Tên tài sản.  
* Loại tài sản.  
* Vị trí.  
* Trạng thái.

Data used

* Thông tin tài sản.  
* Dữ liệu IoT.  
* Lịch sử sự cố và bảo trì.

Postconditions

1. Thông tin tài sản được lưu hoặc cập nhật thành công.  
2. Asset ID của tài sản là duy nhất.  
3. Facility Manager có thể xem lịch sử sự cố và bảo trì.  
4. Dữ liệu IoT hợp lệ được liên kết với đúng tài sản.  
5. Các thay đổi quan trọng được ghi vào nhật ký hệ thống.

Open Questions

1. Asset ID do người dùng nhập hay hệ thống tự tạo?  
2. Danh sách trạng thái tài sản gồm những giá trị nào?  
3. Facility Manager có được xóa tài sản hay chỉ chuyển sang trạng thái không hoạt động?  
4. Một tài sản có thể có nhiều sensor IoT hay không?  
5. Có cần tìm kiếm và lọc danh sách tài sản hay không?

