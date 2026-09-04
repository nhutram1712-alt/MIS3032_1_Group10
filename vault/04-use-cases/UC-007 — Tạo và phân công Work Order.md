## **UC-007 — Tạo và phân công Work Order**

**Related Requirements**

* FR-007 — Tạo và phân công Work Order.  
* FR-002 — Gửi yêu cầu báo sự cố tài sản.  
* FR-005 — Giám sát thiết bị qua IoT.  
* FR-006 — AI dự đoán sự cố và nhu cầu bảo trì.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-006 — Khả năng truy vết và nhật ký.

**Related Business Rules**

* BR-004 — Tạo và phân công Work Order.  
* BR-005 — Vòng đời Work Order.  
* BR-011 — Mô hình vai trò và quyền truy cập.

**Primary Actor**  
 Facility Manager.

Technician là người được phân công xử lý; System cung cấp nguồn phát sinh (yêu cầu Customer, cảnh báo IoT, hoặc dự đoán AI).

**Preconditions**

1. Facility Manager đã đăng nhập hệ thống.  
2. Facility Manager có quyền tạo và phân công Work Order.  
3. Tồn tại ít nhất một nguồn phát sinh hợp lệ: yêu cầu báo sự cố của Customer, cảnh báo IoT, hoặc dự đoán bảo trì của AI.  
4. Tài sản liên quan đến nguồn phát sinh đã tồn tại trong hệ thống.

**Trigger**  
 Facility Manager kiểm tra một yêu cầu, cảnh báo, hoặc dự đoán và quyết định tạo Work Order để xử lý.

**Main Flow**

1. Facility Manager mở danh sách yêu cầu/cảnh báo/dự đoán cần xử lý.  
2. Facility Manager chọn một nguồn phát sinh cụ thể.  
3. Hệ thống hiển thị thông tin tài sản liên quan đến nguồn phát sinh đó.  
4. Facility Manager chọn Technician để phân công.  
5. Hệ thống kiểm tra tài sản và Technician hợp lệ.  
6. Hệ thống tạo Work Order với trạng thái NEW, gắn với tài sản và Technician được phân công.  
7. Hệ thống ghi nhật ký hành động tạo/phân công.  
8. Work Order xuất hiện trong danh sách của Facility Manager và trong danh sách công việc của Technician.

**Alternative Flows**

**AF-01 — Tạo Work Order nhưng chưa phân công Technician ngay**

1. Facility Manager tạo Work Order và xác nhận tài sản liên quan.  
2. Facility Manager chưa chọn Technician tại thời điểm tạo.  
3. Hệ thống lưu Work Order ở trạng thái NEW, chưa có Technician được gán.  
4. Facility Manager có thể quay lại phân công Technician sau.

**AF-02 — Tạo Work Order từ cảnh báo IoT hoặc dự đoán AI**

1. Facility Manager chọn một cảnh báo IoT hoặc một kết quả dự đoán AI làm nguồn phát sinh thay vì yêu cầu Customer.  
2. Hệ thống hiển thị dữ liệu IoT/mức rủi ro AI liên quan đến tài sản.  
3. Facility Manager xác nhận tạo Work Order dựa trên nguồn phát sinh đó.  
4. Hệ thống tiếp tục từ bước 4 của Main Flow.

**AF-03 — Nhiều yêu cầu Customer cho cùng một sự cố**

1. Hệ thống hiển thị các yêu cầu Customer liên quan đến cùng một tài sản.  
2. Facility Manager xem xét và chọn tạo một Work Order duy nhất để xử lý các yêu cầu liên quan.  
3. Hệ thống liên kết Work Order với tài sản đó.

**Exception Flows**

**EF-01 — Tài sản không tồn tại**

1. Facility Manager chọn nguồn phát sinh có Asset ID không tồn tại hoặc không còn hợp lệ.  
2. Hệ thống từ chối tạo Work Order.  
3. Facility Manager được thông báo tài sản không hợp lệ.

**EF-02 — Technician không tồn tại hoặc không hợp lệ**

1. Facility Manager chọn một Technician không tồn tại hoặc không có quyền Technician trong hệ thống.  
2. Hệ thống từ chối gán Technician đó.  
3. Work Order không được phân công cho đến khi chọn Technician hợp lệ.

**EF-03 — Facility Manager không có quyền**

1. Hệ thống kiểm tra quyền của người thực hiện thao tác.  
2. Nếu người dùng không có vai trò Facility Manager, hệ thống từ chối thao tác.  
3. Không có Work Order nào được tạo hoặc phân công.

**EF-04 — Lưu Work Order thất bại**

1. Hệ thống gặp lỗi khi lưu Work Order sau khi dữ liệu đã được xác nhận hợp lệ.  
2. Hệ thống không tạo Work Order.  
3. Facility Manager được thông báo thao tác không thành công và có thể thử lại.

Thông điệp lỗi cụ thể và cơ chế thử lại chưa được xác định trong Vault.

**Data Requirements**

*Input*

* Nguồn phát sinh (yêu cầu Customer / cảnh báo IoT / dự đoán AI).  
* Asset ID liên quan.  
* Technician được phân công.

*Data used*

* Thông tin tài sản.  
* Trạng thái Work Order (NEW).  
* Dữ liệu IoT/AI liên quan (khi nguồn phát sinh là cảnh báo IoT hoặc dự đoán AI).

**Postconditions**

*Success*

1. Work Order được tạo ở trạng thái NEW, gắn với đúng một tài sản.  
2. Technician được phân công (trừ trường hợp AF-01).  
3. Hành động tạo/phân công được ghi vào nhật ký hệ thống.

*Failure*

1. Không có Work Order nào được tạo hoặc phân công.  
2. Dữ liệu nguồn phát sinh (yêu cầu/cảnh báo/dự đoán) không bị thay đổi.

**Open Questions**

1. Cách xử lý khi chưa có Technician phù hợp trong danh sách?  
2. Có cho phép hủy Work Order sau khi tạo hay không?  
3. Có cho phép đổi Technician sau khi đã phân công hay không?  
4. Cách xử lý khi hai Facility Manager đồng thời tạo Work Order cho cùng một sự cố?  
5. Ngoài tài sản, nguồn phát sinh, Technician và trạng thái, các trường bắt buộc khác của Work Order là gì?  
* 

