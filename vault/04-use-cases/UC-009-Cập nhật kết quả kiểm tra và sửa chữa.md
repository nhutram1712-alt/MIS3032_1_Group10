# **UC-009 — Cập nhật kết quả kiểm tra và sửa chữa**

## **Related Requirements**

* FR-009 — Cập nhật kết quả kiểm tra và sửa chữa.  
* FR-008 — Xem Work Order được giao.  
* NFR-005 — Bảo mật và phân quyền.  
* NFR-006 — Khả năng truy vết và nhật ký.  
* NFR-010 — Lưu trữ lịch sử bảo trì.

## **Related Business Rules**

* BR-005 — Vòng đời Work Order.  
* BR-010 — Lưu lịch sử bảo trì.  
* BR-011 — Mô hình vai trò và quyền truy cập.

## **Primary Actor**

Technician (nhân viên kỹ thuật).

## **Supporting Actor**

System.

## **Preconditions**

1. Technician đã đăng nhập thành công vào hệ thống.  
2. Technician có Work Order được Facility Manager phân công.  
3. Work Order thuộc tài sản được quản lý trong hệ thống.  
4. Work Order đang ở trạng thái cho phép Technician cập nhật theo BR-005.

## **Trigger**

Technician thực hiện thao tác mở một Work Order được phân công và cập nhật tiến độ, kết quả kiểm tra hoặc sửa chữa.

## **Main Flow**

1. Technician truy cập danh sách Work Order được phân công.  
2. Technician chọn một Work Order cần kiểm tra hoặc xử lý.  
3. Hệ thống hiển thị thông tin Work Order, bao gồm tài sản liên quan, nội dung sự cố và các dữ liệu IoT hoặc dự đoán AI liên quan nếu có.  
4. Technician thực hiện kiểm tra hoặc sửa chữa tài sản.  
5. Technician cập nhật tiến độ và kết quả kiểm tra hoặc sửa chữa vào Work Order.  
6. Hệ thống kiểm tra quyền cập nhật của Technician đối với Work Order.  
7. Hệ thống lưu thông tin cập nhật và ghi nhận thời gian thực hiện.  
8. Khi công việc hoàn thành, Technician chuyển Work Order sang trạng thái COMPLETED theo vòng đời Work Order.  
9. Hệ thống lưu thông tin xử lý vào lịch sử bảo trì của tài sản theo BR-010.  
10. Hệ thống ghi nhật ký thay đổi Work Order theo NFR-006.  
11. Facility Manager có thể kiểm tra kết quả xử lý và thực hiện bước tiếp theo trong vòng đời Work Order.

Các trường thông tin chi tiết cần nhập khi cập nhật kết quả, giao diện nhập liệu và cơ chế xác nhận hoàn thành chưa được định nghĩa cụ thể.

## **Alternative Flows**

## **AF-01 — Technician cập nhật tiến độ nhưng chưa hoàn thành**

1. Technician mở Work Order được phân công.  
2. Technician cập nhật tình trạng công việc đang thực hiện.  
3. Hệ thống lưu thông tin cập nhật.  
4. Work Order tiếp tục ở trạng thái IN\_PROGRESS.  
5. Technician có thể tiếp tục cập nhật Work Order sau đó.

## **AF-02 — Technician kiểm tra nhưng chưa thể hoàn thành sửa chữa**

1. Technician kiểm tra tài sản.  
2. Technician ghi nhận kết quả kiểm tra và nguyên nhân hoặc tình trạng phát hiện.  
3. Hệ thống lưu kết quả kiểm tra.  
4. Work Order tiếp tục được xử lý theo trạng thái hiện tại.  
5. Technician tiếp tục cập nhật khi có kết quả xử lý cuối cùng.

## **Exception Flows**

## **EF-01 — Technician không có quyền cập nhật Work Order**

1. Hệ thống kiểm tra Technician hiện tại không được phân công Work Order.  
2. Hệ thống từ chối thao tác cập nhật.  
3. Thông tin Work Order không được thay đổi.

### **EF-02 — Work Order không ở trạng thái cho phép cập nhật**

1. Hệ thống kiểm tra trạng thái hiện tại của Work Order.  
2. Nếu trạng thái không cho phép Technician cập nhật, hệ thống từ chối thao tác.  
3. Hệ thống không thay đổi dữ liệu Work Order.

## **EF-03 — Dữ liệu cập nhật không hợp lệ**

1. Hệ thống kiểm tra dữ liệu Technician nhập vào.  
2. Nếu dữ liệu không hợp lệ hoặc thiếu thông tin bắt buộc, hệ thống không lưu kết quả.  
3. Hệ thống yêu cầu Technician bổ sung hoặc điều chỉnh thông tin.

Các thông báo lỗi cụ thể và quy tắc validation chi tiết chưa được định nghĩa trong Vault.

## **Data Requirements**

## **Confirmed data and controls**

* Work Order được phân công cho Technician.  
* Thông tin tài sản liên quan đến Work Order.  
* Trạng thái Work Order: NEW, IN\_PROGRESS, COMPLETED, CLOSED.  
* Kết quả kiểm tra hoặc sửa chữa.  
* Technician thực hiện xử lý.  
* Thời gian thực hiện.  
* Lịch sử bảo trì của tài sản.  
* Nhật ký thay đổi Work Order.  
* Quyền cập nhật theo vai trò Technician.

## **Undefined data**

* Các trường thông tin cụ thể khi Technician cập nhật kết quả.  
* Có bắt buộc nhập nguyên nhân sự cố, nội dung sửa chữa, linh kiện thay thế hoặc ghi chú hay không.  
* Có cho phép đính kèm hình ảnh/tài liệu minh chứng hay không.  
* Quy tắc validation chi tiết đối với dữ liệu cập nhật.  
* Cơ chế xử lý khi lưu dữ liệu thất bại.

## **Postconditions**

### **Success**

1. Kết quả kiểm tra hoặc sửa chữa được lưu vào Work Order.  
2. Trạng thái Work Order được cập nhật phù hợp với vòng đời được quy định.  
3. Khi Work Order hoàn thành và được đóng, thông tin xử lý được lưu vào lịch sử bảo trì của tài sản.  
4. Thay đổi được ghi nhận trong nhật ký hệ thống.

### **Failure**

1. Kết quả cập nhật không được lưu nếu Technician không có quyền hoặc dữ liệu không hợp lệ.  
2. Trạng thái và thông tin Work Order vẫn giữ nguyên trước khi thao tác thất bại.

## **Open Questions**

1. Những trường thông tin nào là bắt buộc khi Technician cập nhật kết quả kiểm tra và sửa chữa?  
2. Technician có bắt buộc nhập nguyên nhân sự cố và nội dung xử lý hay không?  
3. Có cho phép Technician đính kèm hình ảnh trước/sau khi sửa chữa hay không?  
4. Technician có được cập nhật trực tiếp từ NEW sang COMPLETED hay bắt buộc phải qua IN\_PROGRESS?  
5. Khi Technician chuyển Work Order sang COMPLETED, hệ thống có yêu cầu xác nhận lần cuối hay không?  
6. Ai được phép chuyển Work Order từ COMPLETED sang CLOSED và điều kiện xác nhận là gì?  
7. Nếu Technician phát hiện cần thêm vật tư, thiết bị hoặc xử lý khác, Work Order được xử lý như thế nào?  
8. Nếu Technician cập nhật sai kết quả, có được phép chỉnh sửa hoặc thu hồi thông tin đã cập nhật hay không?  
9. Hệ thống phản hồi thế nào khi việc lưu kết quả cập nhật thất bại?  
10. Thông tin nào trong kết quả sửa chữa được sử dụng làm dữ liệu đầu vào cho AI?

