# Customer Brief

## 1. Nhu cầu hệ thống

Trường Đại học Kinh tế - Đại học Đà Nẵng cần một hệ thống nội bộ để quản lý tài sản và hỗ trợ bảo trì thông minh. Giai đoạn đầu tập trung vào ba nhóm tài sản: Wifi, điều hòa và máy chiếu, đèn, quạt. Hệ thống tích hợp IoT để thu thập dữ liệu thiết bị và AI để dự đoán thiết bị có nguy cơ xảy ra sự cố hoặc cần bảo trì.

## 2. Khách hàng phải có thể

Khách hàng gồm giảng viên, sinh viên và nhân viên trong trường.

1. Đăng nhập hệ thống.
2. Xem danh sách tài sản và trạng thái cơ bản của thiết bị tại khu vực/phòng liên quan.
3. Gửi yêu cầu báo lỗi khi Wifi, điều hòa, máy chiếu, đèn, quạt gặp sự cố.
4. Theo dõi trạng thái xử lý yêu cầu đã gửi.

## 3. Bộ phận quản lý cơ sở vật chất phải có thể

5. Quản lý tài sản: thêm, cập nhật và theo dõi thông tin Wifi, điều hòa và máy chiếu, đèn, quạt.
6. Xem dữ liệu/cảnh báo IoT của thiết bị.
7. Xem dự đoán của AI về thiết bị có nguy cơ hỏng hoặc cần bảo trì.
8. Tạo Work Order và phân công kỹ thuật viên xử lý.

## 4. Kỹ thuật viên phải có thể

9. Xem các Work Order được giao.
10. Xem thông tin thiết bị, cảnh báo IoT và dự đoán AI liên quan.
11. Cập nhật kết quả sửa chữa/bảo trì và hoàn thành Work Order.

## 5. Quản trị viên phải có thể

12. Quản lý tài khoản người dùng và phân quyền.
13. Quản lý kết nối giữa tài sản và thiết bị/cảm biến IoT.

## 6. Tích hợp IoT

IoT được tích hợp với các thiết bị cơ sở vật chất để ghi nhận dữ liệu hoạt động và trạng thái thiết bị. Khi phát hiện dấu hiệu bất thường, hệ thống tạo cảnh báo để bộ phận quản lý theo dõi.

- **Wifi:** trạng thái kết nối, chất lượng tín hiệu hoặc tình trạng Access Point.
- **Điều hòa:** nhiệt độ, thời gian hoạt động và trạng thái vận hành.
- **Máy chiếu:** thời gian sử dụng, nhiệt độ hoặc trạng thái hoạt động.
- **Đèn:** trạng thái bật/tắt, thời gian hoạt động, mức tiêu thụ điện năng hoặc tình trạng hoạt động bất thường.
- **Quạt:** trạng thái bật/tắt, tốc độ hoạt động, thời gian vận hành, các dấu hiệu bất thường như quá nhiệt hoặc hoạt động không ổn định.

## 7. Tích hợp AI

AI sử dụng dữ liệu IoT kết hợp với lịch sử sửa chữa/bảo trì để đánh giá tình trạng tài sản. Mục tiêu chính là dự đoán thiết bị nào có khả năng xảy ra sự cố hoặc cần được bảo trì sớm, giúp nhà trường chủ động xử lý trước khi thiết bị hỏng hoàn toàn.

## 8. Thông tin của mỗi tài sản

- Mã tài sản (Asset ID).
- Tên và loại tài sản.
- Vị trí/phòng.
- Trạng thái hoạt động.
- Thông tin/cảnh báo IoT.
- Mức rủi ro hoặc khuyến nghị bảo trì từ AI.
- Lịch sử sửa chữa và bảo trì.

## 9. Phạm vi giai đoạn đầu

Phiên bản đầu ưu tiên hệ thống đơn giản, tập trung vào quản lý tài sản, báo lỗi, Work Order, dữ liệu IoT và dự đoán bảo trì bằng AI. Chưa mở rộng sang quản lý tài chính, mua sắm, kho linh kiện hoặc các nghiệp vụ quản trị toàn trường.

**Luồng chính:**

IoT thu thập dữ liệu → AI dự đoán cảnh báo/bảo trì → Work Order → kỹ thuật viên xử lý → lưu lịch sử bảo trì.
