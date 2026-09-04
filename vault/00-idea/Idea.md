# 1. Vấn đề

Các thiết bị cơ sở vật chất như Wifi, điều hòa và máy chiếu, đèn được sử dụng thường xuyên nhưng việc theo dõi tình trạng, báo hỏng và bảo trì còn rời rạc. Khi thiết bị xảy ra sự cố, bộ phận kỹ thuật thường chỉ biết sau khi người dùng phản ánh, nên việc xử lý có thể chậm và khó theo dõi lịch sử tài sản.

# 2. Ý tưởng sản phẩm

Xây dựng một hệ thống web đơn giản để quản lý tài sản của trường. IoT được tích hợp với các thiết bị cơ sở vật chất để ghi nhận dữ liệu hoạt động; AI sử dụng dữ liệu này cùng lịch sử bảo trì để dự đoán thiết bị có nguy cơ gặp sự cố hoặc cần bảo trì. Kết quả được hiển thị cho bộ phận quản lý để chủ động xử lý.

# 3. Quản lý tài sản

- Quản lý danh sách Wifi, điều hòa và máy chiếu, đèn theo mã tài sản, vị trí và trạng thái.

- Xem lịch sử sự cố và bảo trì của từng tài sản.

- Cập nhật trạng thái: Hoạt động - Cần kiểm tra - Đang sửa - Hoàn thành.

# 4. Tích hợp IoT

IoT có nhiệm vụ kết nối và giám sát các thiết bị cơ sở vật chất, gửi dữ liệu về hệ thống theo thời gian gần thực.

- Wifi: trạng thái kết nối, tín hiệu hoặc điểm truy cập bất thường.

- Điều hòa: nhiệt độ, thời gian hoạt động, trạng thái bật/tắt hoặc bất thường.

- Máy chiếu: thời gian sử dụng, trạng thái hoạt động và cảnh báo thiết bị.

# 5. Tích hợp AI

AI không tự sửa thiết bị mà hỗ trợ dự đoán và ra quyết định bảo trì.

- Phân tích dữ liệu IoT và lịch sử sửa chữa.

- Dự đoán thiết bị có nguy cơ xảy ra sự cố.

- Đề xuất thiết bị cần kiểm tra hoặc bảo trì sớm.

- Xếp mức ưu tiên để Facility Manager xử lý trước các trường hợp quan trọng.

# 6. Người dùng mục tiêu

- Requester: giảng viên, sinh viên hoặc nhân viên gửi yêu cầu khi phát hiện sự cố.

- Technician: nhân viên kỹ thuật thực hiện kiểm tra và sửa chữa.

- Facility Manager: quản lý tài sản, theo dõi cảnh báo IoT và dự đoán AI, phân công xử lý.

- Administrator: quản lý tài khoản và phân quyền hệ thống.

# 7. MVP Goal - Chức năng theo vai trò

Requester có thể:

- Đăng nhập.

- Chọn tài sản và gửi yêu cầu báo sự cố.

- Theo dõi trạng thái xử lý.

Technician có thể:

- Xem công việc được giao.

- Xem dữ liệu IoT và cảnh báo/dự đoán AI liên quan đến tài sản.

- Cập nhật kết quả kiểm tra hoặc sửa chữa.

Facility Manager có thể:

- Quản lý danh sách và trạng thái tài sản.

- Xem cảnh báo IoT và danh sách thiết bị AI dự đoán cần bảo trì.

- Tạo và phân công Work Order cho kỹ thuật viên.

Administrator có thể:

- Quản lý tài khoản và quyền truy cập.

# 8. Quy trình hoạt động đơn giản

IoT thu thập dữ liệu thiết bị -> AI phân tích và dự đoán -> hệ thống tạo cảnh báo -> Facility Manager kiểm tra và tạo Work Order -> Technician xử lý -> hệ thống lưu lịch sử bảo trì.

# 9. Ngoài phạm vi MVP

- AI tự động ra quyết định thay con người hoặc tự sửa thiết bị.

- Quản lý ngân sách, mua sắm và nhân sự toàn trường.

- Triển khai IoT cho toàn bộ tài sản ngay từ đầu; MVP chỉ cần thí điểm với Wifi, điều hòa và máy chiếu.

LINK FILE:
