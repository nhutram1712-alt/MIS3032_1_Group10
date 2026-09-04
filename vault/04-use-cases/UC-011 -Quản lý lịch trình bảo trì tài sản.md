# **UC-011 — Quản lý lịch trình bảo trì tài sản**

## **Related Requirements**

* FR-011 — Quản lý lịch trình bảo trì tài sản.  
* FR-004 — Quản lý tài sản.  
* FR-006 — AI dự đoán sự cố và nhu cầu bảo trì.  
* NFR-003 — Thời gian phản hồi.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-006 — Khả năng truy vết và nhật ký.  
* NFR-010 — Lưu trữ lịch sử bảo trì.

## **Related Business Rules**

* BR-008 — AI dự đoán sự cố và nhu cầu bảo trì.  
* BR-009 — AI chỉ hỗ trợ quyết định.  
* BR-010 — Lưu lịch sử bảo trì.

## **Primary Actor**

Facility Manager.

## **Supporting Actor**

AI System; System.

## **Preconditions**

1. Facility Manager đã đăng nhập thành công vào hệ thống.  
2. Facility Manager có quyền quản lý tài sản và lịch trình bảo trì.  
3. Tài sản cần lập lịch đã tồn tại trong hệ thống.  
4. Nếu lịch bảo trì được đề xuất dựa trên AI, hệ thống đã có kết quả dự đoán hoặc khuyến nghị bảo trì liên quan đến tài sản.

## **Trigger**

Facility Manager truy cập chức năng quản lý lịch trình bảo trì và thực hiện thao tác tạo, xem hoặc cập nhật lịch bảo trì cho một tài sản.

## **Main Flow**

1. Facility Manager truy cập chức năng quản lý lịch trình bảo trì.  
2. Hệ thống hiển thị danh sách các lịch bảo trì hiện có.  
3. Facility Manager chọn một tài sản cần lập hoặc cập nhật lịch bảo trì.  
4. Hệ thống hiển thị thông tin tài sản và các thông tin liên quan.  
5. Facility Manager nhập hoặc lựa chọn thời gian dự kiến bảo trì.  
6. Facility Manager nhập nội dung bảo trì.  
7. Facility Manager xác định lịch bảo trì là lịch định kỳ hoặc dựa trên khuyến nghị bảo trì của AI.  
8. Nếu sử dụng khuyến nghị AI, hệ thống hiển thị mức rủi ro và khuyến nghị liên quan đến tài sản.  
9. Facility Manager xem xét thông tin và xác nhận lịch bảo trì.  
10. Hệ thống kiểm tra dữ liệu lịch bảo trì.  
11. Hệ thống lưu lịch bảo trì gắn với tài sản.  
12. Hệ thống ghi nhận thao tác tạo hoặc cập nhật lịch vào nhật ký hệ thống theo NFR-006.  
13. Lịch bảo trì được hiển thị trong danh sách lịch của Facility Manager.  
14. Khi đến thời gian dự kiến, lịch được sử dụng làm thông tin tham khảo cho hoạt động bảo trì tiếp theo.

AI chỉ cung cấp khuyến nghị; việc tạo hoặc xác nhận lịch bảo trì vẫn do Facility Manager quyết định theo BR-009.

## **Alternative Flows**

### **AF-01 — Tạo lịch bảo trì định kỳ**

1. Facility Manager chọn tài sản.  
2. Facility Manager chọn hình thức bảo trì định kỳ.  
3. Facility Manager nhập thời gian dự kiến và nội dung bảo trì.  
4. Hệ thống kiểm tra thông tin.  
5. Facility Manager xác nhận.  
6. Hệ thống lưu lịch bảo trì định kỳ cho tài sản.

### **AF-02 — Lập lịch dựa trên khuyến nghị AI**

1. Hệ thống có kết quả AI dự đoán tài sản có nguy cơ xảy ra sự cố hoặc cần bảo trì.  
2. Facility Manager xem mức rủi ro và khuyến nghị bảo trì.  
3. Facility Manager đánh giá khuyến nghị.  
4. Facility Manager quyết định lập lịch bảo trì.  
5. Facility Manager xác định thời gian và nội dung bảo trì.  
6. Facility Manager xác nhận lịch.  
7. Hệ thống lưu lịch bảo trì.

### **AF-03 — Cập nhật lịch bảo trì**

1. Facility Manager chọn lịch bảo trì đã tồn tại.  
2. Facility Manager thay đổi thời gian hoặc nội dung bảo trì.  
3. Hệ thống kiểm tra thông tin cập nhật.  
4. Facility Manager xác nhận.  
5. Hệ thống lưu lịch mới.  
6. Hệ thống ghi nhận thay đổi vào nhật ký.

### **AF-04 — Xem lịch bảo trì**

1. Facility Manager truy cập danh sách lịch bảo trì.  
2. Hệ thống hiển thị các lịch bảo trì đã tạo.  
3. Facility Manager chọn một lịch để xem chi tiết.  
4. Hệ thống hiển thị tài sản, thời gian dự kiến và nội dung bảo trì.

## **Exception Flows**

### **EF-01 — Tài sản không tồn tại hoặc không hợp lệ**

1. Facility Manager chọn tài sản không tồn tại hoặc không còn thuộc phạm vi quản lý.  
2. Hệ thống từ chối tạo lịch bảo trì.  
3. Lịch bảo trì không được lưu.

### **EF-02 — Thông tin lịch bảo trì không hợp lệ**

1. Hệ thống kiểm tra thời gian và nội dung bảo trì.  
2. Nếu dữ liệu không hợp lệ hoặc thiếu thông tin bắt buộc, hệ thống từ chối lưu.  
3. Facility Manager được yêu cầu điều chỉnh thông tin.

### **EF-03 — Facility Manager không có quyền**

1. Hệ thống kiểm tra quyền của người thực hiện.  
2. Nếu người dùng không có quyền Facility Manager, hệ thống từ chối thao tác.  
3. Không có lịch bảo trì nào được tạo hoặc cập nhật.

### **EF-04 — Khuyến nghị AI không khả dụng**

1. Facility Manager chọn lập lịch dựa trên khuyến nghị AI.  
2. Hệ thống không có kết quả dự đoán hoặc dữ liệu AI không khả dụng.  
3. Hệ thống không thể cung cấp khuyến nghị AI.  
4. Facility Manager có thể lập lịch theo kế hoạch định kỳ nếu nghiệp vụ cho phép.

### **EF-05 — Xung đột lịch bảo trì**

1. Hệ thống phát hiện lịch bảo trì trùng hoặc xung đột với một lịch khác của cùng tài sản.  
2. Hệ thống thông báo cho Facility Manager.  
3. Facility Manager điều chỉnh thời gian hoặc xác nhận xử lý theo quy tắc hệ thống.

Quy tắc cụ thể về xung đột lịch và thông báo lỗi chưa được định nghĩa trong Vault.

## **Data Requirements**

### **Confirmed data and controls**

* Asset ID.  
* Tài sản được lập lịch bảo trì.  
* Thời gian dự kiến bảo trì.  
* Nội dung bảo trì.  
* Lịch bảo trì định kỳ.  
* Khuyến nghị bảo trì từ AI.  
* Mức rủi ro do AI cung cấp.  
* Facility Manager là người xác nhận lịch.  
* Nhật ký tạo và cập nhật lịch bảo trì.  
* Lịch sử bảo trì của tài sản.

### **Undefined data**

* ID của lịch bảo trì.  
* Tần suất cụ thể đối với lịch định kỳ.  
* Thời gian bắt đầu và kết thúc của lịch.  
* Trạng thái lịch bảo trì.  
* Quy tắc xử lý lịch trùng hoặc xung đột.  
* Có cho phép hủy hoặc xóa lịch bảo trì hay không.  
* Có gửi thông báo trước thời gian bảo trì hay không.  
* Cơ chế liên kết lịch bảo trì với Work Order.  
* Quy tắc validation chi tiết.

## **Postconditions**

### **Success**

1. Lịch bảo trì được tạo hoặc cập nhật thành công cho tài sản.  
2. Lịch bao gồm tối thiểu tài sản, thời gian dự kiến và nội dung bảo trì.  
3. Nếu lịch dựa trên AI, Facility Manager là người xem xét và xác nhận cuối cùng.  
4. Thao tác tạo hoặc cập nhật lịch được ghi nhận trong nhật ký hệ thống.

### **Failure**

1. Lịch bảo trì không được tạo hoặc cập nhật.  
2. Dữ liệu lịch hiện tại vẫn được giữ nguyên.  
3. Facility Manager nhận được thông tin về nguyên nhân thao tác thất bại nếu hệ thống đã xác định được nguyên nhân.

## **Open Questions**

1. Lịch bảo trì định kỳ được thiết lập theo ngày, tuần, tháng hay chu kỳ riêng cho từng loại tài sản?  
2. Hệ thống có tự động tạo các lần bảo trì tiếp theo từ một lịch định kỳ hay Facility Manager phải tạo từng lịch?  
3. Lịch bảo trì có các trạng thái cụ thể như Planned, In Progress, Completed, Cancelled hay không?  
4. Facility Manager có được hủy hoặc xóa lịch bảo trì đã tạo hay không?  
5. Khi lịch bảo trì đến hạn, hệ thống có tự động tạo Work Order hay chỉ gửi thông báo cho Facility Manager?  
6. Lịch bảo trì dựa trên AI có bắt buộc phải gắn với một kết quả dự đoán cụ thể hay không?  
7. Nếu Facility Manager không đồng ý với khuyến nghị AI, hệ thống có cần lưu lại quyết định từ chối hay lý do hay không?  
8. Có cho phép nhiều lịch bảo trì cho cùng một tài sản trong cùng một khoảng thời gian hay không?  
9. Quy tắc phát hiện và xử lý xung đột lịch bảo trì là gì?  
10. Hệ thống có gửi thông báo hoặc nhắc lịch bảo trì cho Facility Manager và Technician hay không?  
11. Lịch bảo trì có được liên kết trực tiếp với Work Order không? Nếu có, khi nào liên kết được tạo?  
12. Khi hoàn thành bảo trì, hệ thống có tự động cập nhật lịch và tạo bản ghi lịch sử bảo trì theo BR-010 hay không?  
13. Có cần lưu người tạo, người cập nhật và thời điểm thay đổi lịch trong lịch sử không?  
14. Thời gian lưu trữ lịch bảo trì có tuân theo NFR-010 hay có quy định riêng?

