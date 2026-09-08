# Problem Statement

## 1. Problem Statement

Sinh viên, giảng viên và nhân viên tại **Trường Đại học Kinh tế – Đại học Đà Nẵng** gặp khó khăn trong việc báo cáo, theo dõi và xử lý các sự cố liên quan đến tài sản cơ sở vật chất.

Hiện tại, hoạt động quản lý và bảo trì các thiết bị như **Wi-Fi, điều hòa, máy chiếu, đèn và quạt** chưa được tập trung và chưa có khả năng giám sát tình trạng thiết bị một cách chủ động. Điều này có thể dẫn đến chậm trễ trong xử lý sự cố, khó khăn trong việc theo dõi tình trạng tài sản và tăng nguy cơ thiết bị hỏng đột xuất.

## 2. Problem Context

Tại **Trường Đại học Kinh tế – Đại học Đà Nẵng**, các thiết bị cơ sở vật chất được sử dụng thường xuyên tại phòng học, phòng làm việc và các khu vực phục vụ hoạt động giảng dạy, học tập và vận hành của nhà trường.

Khi thiết bị gặp sự cố, **sinh viên, giảng viên hoặc nhân viên** cần thông báo cho bộ phận phụ trách để được kiểm tra và xử lý. Tuy nhiên, quá trình quản lý tài sản và bảo trì còn gặp một số hạn chế trong việc tập trung thông tin, theo dõi tình trạng thiết bị và phát hiện sớm các dấu hiệu bất thường.

### Key Problems

* **Thiếu kênh báo cáo tập trung:** Người dùng chưa có một kênh thống nhất để gửi yêu cầu báo lỗi và theo dõi tiến độ xử lý.
* **Khó theo dõi tình trạng tài sản:** Facility Manager gặp khó khăn trong việc theo dõi trạng thái hoạt động của tài sản tại nhiều phòng và khu vực.
* **Bảo trì còn mang tính phản ứng:** Việc bảo trì chủ yếu được thực hiện sau khi thiết bị xảy ra sự cố thay vì chủ động xác định thiết bị có nguy cơ.
* **Dữ liệu tài sản chưa được khai thác hiệu quả:** Dữ liệu về tình trạng hoạt động và lịch sử sửa chữa/bảo trì chưa được sử dụng hiệu quả để đánh giá tình trạng tài sản.
* **Khó phối hợp xử lý sự cố:** Việc tiếp nhận, phân công và theo dõi yêu cầu cần được quản lý tập trung để **Facility Manager** và **Technician** có thể phối hợp hiệu quả.

## 3. Business Impact

Các vấn đề trên có thể tạo ra những tác động sau đối với hoạt động của nhà trường:

* **Gián đoạn hoạt động giảng dạy và học tập** khi Wi-Fi, máy chiếu, điều hòa, đèn hoặc quạt gặp sự cố.
* **Tăng thời gian xử lý sự cố** do quá trình báo lỗi, tiếp nhận và theo dõi yêu cầu chưa được quản lý tập trung.
* **Tăng nguy cơ thiết bị hỏng đột xuất** do các dấu hiệu bất thường không được phát hiện và theo dõi sớm.
* **Khó lập kế hoạch bảo trì chủ động** do thiếu dữ liệu tổng hợp về tình trạng và lịch sử hoạt động của tài sản.
* **Khó theo dõi lịch sử bảo trì và sửa chữa** của từng tài sản, gây hạn chế trong việc đánh giá tình trạng thiết bị.
* **Tăng khối lượng công việc** của Facility Manager và Technician khi số lượng tài sản và yêu cầu bảo trì tăng lên.

## 4. Problem Summary

Vấn đề cốt lõi cần giải quyết là:

> **Nhà trường thiếu một cơ chế quản lý tập trung kết nối thông tin tài sản, yêu cầu bảo trì, dữ liệu tình trạng thiết bị và lịch sử bảo trì. Điều này khiến việc xử lý sự cố còn mang tính phản ứng và hạn chế khả năng chủ động xác định các thiết bị có nguy cơ cần bảo trì.**

Vấn đề này là cơ sở để xây dựng hệ thống **Smart Maintenance & Facility Management**.

Hệ thống định hướng sử dụng:

* **IoT** để thu thập và cập nhật dữ liệu hoạt động của thiết bị.
* **AI** để hỗ trợ phân tích dữ liệu, đánh giá rủi ro và xác định các thiết bị có khả năng cần bảo trì.
* **Centralized Maintenance Management** để quản lý tập trung tài sản, yêu cầu bảo trì, quá trình xử lý và lịch sử bảo trì.
