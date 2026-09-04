## **UC-008 — Xem Work Order được giao**

**Related Requirements**

* FR-008 — Xem Work Order được giao.  
* FR-005 — Giám sát thiết bị qua IoT.  
* FR-006 — AI dự đoán sự cố và nhu cầu bảo trì.  
* NFR-003 — Thời gian phản hồi.  
* NFR-005 — Bảo mật và phân quyền.

**Related Business Rules**

* BR-004 — Tạo và phân công Work Order.  
* BR-011 — Mô hình vai trò và quyền truy cập.

**Primary Actor**  
 Technician.

**Preconditions**

1. Technician đã đăng nhập hệ thống.  
2. Có ít nhất một Work Order đã được Facility Manager phân công cho Technician đó (đối với luồng chính; xem AF-01 khi chưa có).

**Trigger**  
 Technician truy cập chức năng xem Work Order được giao.

**Main Flow**

1. Technician mở danh sách Work Order được giao.  
2. Hệ thống truy xuất các Work Order đã được phân công cho Technician đó.  
3. Hệ thống hiển thị danh sách kèm thông tin tài sản và nội dung sự cố.  
4. Technician chọn một Work Order để xem chi tiết.  
5. Hệ thống hiển thị chi tiết Work Order, tài sản liên quan, và dữ liệu IoT/AI liên quan nếu có.  
6. Technician sử dụng thông tin này để chuẩn bị xử lý công việc (tiếp tục qua UC-009).

**Alternative Flows**

**AF-01 — Technician chưa có Work Order được giao**

1. Hệ thống không tìm thấy Work Order nào được phân công cho Technician.  
2. Hệ thống hiển thị danh sách rỗng.

**AF-02 — Work Order không có dữ liệu IoT/AI liên quan**

1. Technician mở chi tiết một Work Order mà tài sản liên quan không có dữ liệu IoT hoặc dự đoán AI.  
2. Hệ thống hiển thị thông tin Work Order và tài sản mà không kèm phần dữ liệu IoT/AI.

**AF-03 — Work Order có dữ liệu IoT/AI liên quan**

1. Technician mở chi tiết một Work Order mà tài sản liên quan có dữ liệu IoT hoặc kết quả dự đoán AI.  
2. Hệ thống hiển thị dữ liệu IoT gần nhất và/hoặc mức rủi ro, khuyến nghị của AI cùng với thông tin Work Order.

**Exception Flows**

**EF-01 — Không tải được danh sách Work Order**

1. Hệ thống gặp lỗi khi truy xuất dữ liệu Work Order.  
2. Hệ thống không hiển thị được danh sách.  
3. Technician được thông báo lỗi tải dữ liệu.

**EF-02 — Technician cố truy cập Work Order không được giao cho mình**

1. Technician cố mở một Work Order được phân công cho Technician khác.  
2. Hệ thống từ chối hiển thị Work Order đó.

**EF-03 — Work Order bị thay đổi trong lúc Technician đang xem**

1. Facility Manager thay đổi phân công hoặc trạng thái của Work Order trong khi Technician đang xem chi tiết.  
2. Hệ thống không đảm bảo dữ liệu Technician đang xem là bản mới nhất cho đến khi làm mới trang.

Cơ chế cập nhật thời gian thực và thông báo khi có thay đổi chưa được định nghĩa trong Vault.

**Data Requirements**

*Data used*

* Thông tin Work Order (tài sản, nội dung sự cố, trạng thái).  
* Dữ liệu IoT liên quan đến tài sản (nếu có).  
* Kết quả dự đoán AI liên quan đến tài sản (nếu có).

**Postconditions**

*Success*

1. Technician xem được danh sách Work Order được phân công cho mình.  
2. Technician xem được chi tiết Work Order kèm dữ liệu IoT/AI liên quan nếu có.

*Failure*

1. Technician không xem được danh sách hoặc chi tiết Work Order do lỗi hệ thống.  
2. Không có dữ liệu nào bị thay đổi do việc xem thất bại.

**Open Questions**

1. Hành động cụ thể nào (nút bấm, menu...) mở danh sách Work Order được giao?  
2. Có cần bộ lọc, sắp xếp, hoặc phân trang cho danh sách Work Order hay không?  
3. Danh sách có tự động làm mới khi Work Order thay đổi hay Technician phải tải lại thủ công?  
4. Cách xử lý khi Work Order bị chuyển cho Technician khác trong lúc Technician đang xem?  
   

