# **UC-002 Gửi yêu cầu báo sự cố tài sản**

## **Related Requirements**

* FR-002 — Gửi yêu cầu báo sự cố tài sản.  
* NFR-001 — Phạm vi sử dụng nội bộ.  
* NFR-003 — Thời gian phản hồi.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-007 — Độ tin cậy dữ liệu IoT.

## **Related Business Rules**

* BR-001 — Định danh và thông tin tài sản.  
* BR-002 — Phạm vi và vị trí tài sản.  
* BR-003 — Yêu cầu báo sự cố của Customer.  
* BR-011 — Mô hình vai trò và quyền truy cập.

## **Primary Actor**

Customer (giảng viên, sinh viên và nhân viên trong trường).

## **Supporting Actor**

Không có.

## **Preconditions**

1. Customer đã đăng nhập thành công vào hệ thống (FR-001, BR-011, NFR-001).  
2. Tài sản gặp sự cố thuộc nhóm tài sản đang được quản lý trong phạm vi hệ thống (Wifi, điều hòa, máy chiếu, đèn, quạt) và đã có Asset ID duy nhất kèm vị trí cụ thể (BR-001, BR-002).

## **Trigger**

Customer phát hiện tài sản cơ sở vật chất gặp sự cố và thực hiện gửi yêu cầu báo lỗi (FR-002, BR-003).

## **Main Flow**

1. Customer chọn chức năng gửi báo sự cố trên hệ thống.  
2. Customer chọn một tài sản cụ thể gặp sự cố theo Asset ID, tên hoặc vị trí (FR-002, BR-003).  
3. Customer nhập thông tin mô tả chi tiết về sự cố gặp phải (FR-002, BR-003).  
4. Customer xác nhận gửi yêu cầu báo sự cố.  
5. Hệ thống kiểm tra tính hợp lệ của tài sản và mô tả sự cố (BR-003).  
6. Hệ thống tạo và lưu trữ bản ghi yêu cầu báo sự cố mới gắn với tài sản đã chọn và liên kết với Customer tạo yêu cầu (FR-002, BR-003).  
7. Hệ thống hiển thị thông báo gửi thành công và ghi nhận yêu cầu như một nguồn phát sinh hợp lệ để Facility Manager có thể tạo Work Order sau này (FR-007, BR-004).  
8. Customer có thể theo dõi yêu cầu này qua UC-003 (FR-003).

Giao diện chi tiết, biểu mẫu nhập liệu và màn hình xác nhận sau khi gửi chưa được định nghĩa chi tiết trong Vault.

## **Alternative Flows**

### **AF-01 — Báo sự cố khi IoT của tài sản mất kết nối**

1. Thiết bị/cảm biến IoT gắn với tài sản đang mất kết nối hoặc dữ liệu không được cập nhật (NFR-007, BR-006).  
2. Hệ thống vẫn cho phép Customer tạo và gửi yêu cầu báo sự cố thủ công bình thường mà không bị gián đoạn (NFR-007, BR-006).

Chưa có luồng thay thế cho phép lưu nháp, chỉnh sửa hoặc hủy yêu cầu trước khi gửi được xác nhận trong Vault.

## **Exception Flows**

### **EF-01 — Dữ liệu yêu cầu không hợp lệ hoặc thiếu thông tin bắt buộc**

1. Customer không chọn tài sản cụ thể hoặc để trống phần mô tả vấn đề (BR-003).  
2. Hệ thống từ chối lưu yêu cầu báo sự cố.  
3. Thông báo lỗi chi tiết trên giao diện chưa được định nghĩa trong Vault.

### **EF-02 — Tài sản không tồn tại hoặc lỗi lưu trữ**

1. Tài sản được chọn không tồn tại trong hệ thống hoặc việc lưu dữ liệu gặp lỗi kỹ thuật.  
2. Hệ thống không tạo yêu cầu báo sự cố.  
3. Hành vi phản hồi cụ thể cho người dùng chưa được định nghĩa trong Vault.

## **Data Requirements**

### **Confirmed data and controls**

* Asset ID của tài sản gặp sự cố (BR-001, BR-003).  
* Mô tả nội dung sự cố (BR-003).  
* Thông tin định danh của Customer tạo yêu cầu (BR-003, BR-011).  
* Kết nối mã hóa an toàn TLS (NFR-005).

### **Undefined data**

* Trạng thái khởi tạo mặc định của bản ghi yêu cầu (ngoài trạng thái của Work Order).  
* Các trường dữ liệu bổ sung: mức độ nghiêm trọng/ưu tiên do người dùng tự chọn, tệp/hình ảnh đính kèm minh họa sự cố, số điện thoại liên hệ.  
* Thời gian lưu trữ cụ thể của bản ghi yêu cầu báo sự cố (ngoài quy định lưu trữ lịch sử bảo trì Work Order).

## **Postconditions**

### **Success**

1. Yêu cầu báo sự cố được ghi nhận vào hệ thống và liên kết với đúng tài sản.  
2. Bản ghi yêu cầu sẵn sàng để Facility Manager xem xét làm nguồn phát sinh lập Work Order (BR-004).  
3. Customer có thể tra cứu và theo dõi trạng thái yêu cầu của chính mình (BR-003, FR-003).

### **Failure**

1. Yêu cầu báo sự cố không được tạo trong hệ thống.  
2. Không có hành vi trạng thái thất bại bổ sung nào được định nghĩa trong Vault.

## **Open Questions**

1. Có cho phép Customer lưu nháp, sửa hoặc hủy yêu cầu trước khi gửi hay không?  
2. Hệ thống phải xử lý thế nào khi tài sản không tồn tại, dữ liệu thiếu hoặc việc lưu yêu cầu thất bại?  
3. Ngoài Asset ID và mô tả vấn đề, các trường bắt buộc khác như thời gian, mức độ ưu tiên hoặc hình ảnh đính kèm có được bổ sung không?  
4. Xử lý thế nào khi nhiều Customer cùng báo một sự cố cho cùng một tài sản hoặc gửi yêu cầu trùng lặp?  
5. Cách thức xử lý và đồng bộ khi nhiều yêu cầu được gửi đồng thời cho cùng một tài sản chưa được quy định?

