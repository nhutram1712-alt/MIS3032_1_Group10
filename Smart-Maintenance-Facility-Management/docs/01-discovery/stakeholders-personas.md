# Stakeholders & Personas
### 2.1. Overview
Hệ thống Smart Maintenance & Facility Management được xây dựng cho Trường Đại học Kinh tế – Đại học Đà Nẵng, với mục tiêu hỗ trợ quản lý tài sản cơ sở vật chất, tiếp nhận và xử lý yêu cầu bảo trì, giám sát tình trạng thiết bị thông qua IoT và hỗ trợ dự đoán nhu cầu bảo trì bằng AI.

Bốn persona/stakeholder chính của hệ thống gồm:
* Requester
* Technician
* Facility Manager
* Admin

---

### 2.2. Persona 01 – Requester

#### Thông tin chung
| Thuộc tính | Mô tả |
| --- | --- |
| **Role** | Requester |
| **Người dùng thực tế** | Sinh viên, giảng viên, nhân viên Trường Đại học Kinh tế – ĐHĐN |
| **Mức độ sử dụng** | Thường xuyên khi phát sinh sự cố |
| **Mục tiêu chính** | Báo lỗi nhanh chóng và theo dõi quá trình xử lý |

#### Bối cảnh
Requester là những người trực tiếp sử dụng các phòng học, phòng làm việc và cơ sở vật chất của trường. Khi Wi-Fi, điều hòa, máy chiếu, đèn hoặc quạt gặp sự cố, họ cần thông báo cho bộ phận phụ trách để được xử lý.

#### Goals
* Đăng nhập và sử dụng hệ thống dễ dàng.
* Xem thông tin và trạng thái cơ bản của tài sản tại phòng/khu vực liên quan.
* Gửi yêu cầu báo lỗi khi phát hiện thiết bị gặp sự cố.
* Cung cấp thông tin cần thiết về sự cố.
* Theo dõi trạng thái xử lý yêu cầu đã gửi.
* Biết được yêu cầu đã được tiếp nhận, phân công và hoàn thành hay chưa.

#### Pain Points
* Không biết nên báo sự cố qua kênh nào.
* Khó biết yêu cầu đã được tiếp nhận hay chưa.
* Không biết tình trạng xử lý yêu cầu sau khi báo lỗi.
* Sự cố thiết bị có thể ảnh hưởng trực tiếp đến việc học tập hoặc làm việc.

#### Key Expectation
* Báo lỗi nhanh – dễ theo dõi – biết được sự cố đang được xử lý đến đâu.

---

### 2.3. Persona 02 – Technician

#### Thông tin chung
| Thuộc tính | Mô tả |
| --- | --- |
| **Role** | Technician |
| **Người dùng thực tế** | Kỹ thuật viên/bộ phận kỹ thuật phụ trách bảo trì tại trường |
| **Mức độ sử dụng** | Hằng ngày khi có Work Order |
| **Mục tiêu chính** | Xử lý Work Order chính xác và cập nhật kết quả bảo trì |

#### Bối cảnh
Technician chịu trách nhiệm kiểm tra và xử lý các sự cố hoặc nhiệm vụ bảo trì được Facility Manager phân công. Technician cần biết chính xác thiết bị nào cần xử lý, vị trí thiết bị, tình trạng hiện tại và các thông tin liên quan.

#### Goals
* Xem danh sách Work Order được giao.
* Xem thông tin chi tiết của tài sản cần xử lý.
* Biết vị trí của thiết bị.
* Xem cảnh báo IoT liên quan đến thiết bị.
* Xem AI prediction/risk liên quan đến thiết bị.
* Cập nhật quá trình kiểm tra và sửa chữa.
* Ghi nhận nguyên nhân và kết quả xử lý.
* Hoàn thành Work Order sau khi bảo trì/sửa chữa.
* Lưu lại thông tin bảo trì để phục vụ lịch sử tài sản.

#### Pain Points
* Có thể thiếu thông tin về tình trạng thiết bị trước khi đến kiểm tra.
* Khó ưu tiên xử lý nếu có nhiều yêu cầu.
* Thông tin sửa chữa/bảo trì cần được ghi nhận đầy đủ để phục vụ những lần xử lý sau.

#### Key Expectation
* Nhận đúng Work Order – có đủ thông tin thiết bị – xử lý hiệu quả – ghi nhận đầy đủ kết quả.

---

### 2.4. Persona 03 – Facility Manager

#### Thông tin chung
| Thuộc tính | Mô tả |
| --- | --- |
| **Role** | Facility Manager |
| **Người dùng thực tế** | Cán bộ/bộ phận phụ trách quản lý cơ sở vật chất của trường |
| **Mức độ sử dụng** | Hằng ngày |
| **Mục tiêu chính** | Quản lý tài sản và chủ động điều phối hoạt động bảo trì |

#### Bối cảnh
Facility Manager chịu trách nhiệm quản lý tình trạng tài sản và điều phối các hoạt động bảo trì. Đây là persona sử dụng nhiều nhất các chức năng IoT và AI của hệ thống.

Facility Manager cần theo dõi cả hai nguồn maintenance:
* **Reactive Maintenance:** phát sinh từ Maintenance Request của Requester.
* **Predictive Maintenance:** phát sinh từ IoT monitoring và AI prediction.

#### Goals
* Quản lý thông tin và trạng thái tài sản.
* Theo dõi tài sản theo phòng/khu vực.
* Theo dõi dữ liệu và cảnh báo từ IoT.
* Phát hiện các thiết bị có dấu hiệu bất thường.
* Xem AI prediction và maintenance risk của tài sản.
* Xác định thiết bị cần được kiểm tra/bảo trì sớm.
* Tiếp nhận và đánh giá Maintenance Request.
* Tạo Work Order.
* Phân công Technician phù hợp.
* Theo dõi tiến độ xử lý Work Order.
* Theo dõi lịch sử sửa chữa/bảo trì của tài sản.
* Sử dụng dữ liệu lịch sử để hỗ trợ lập kế hoạch bảo trì.

#### Pain Points
* Khó theo dõi tình trạng nhiều loại tài sản tại nhiều phòng/khu vực.
* Khó phát hiện thiết bị có dấu hiệu bất thường trước khi xảy ra sự cố.
* Maintenance Request và Work Order cần được quản lý tập trung.
* Khó đưa ra quyết định bảo trì chủ động nếu thiếu dữ liệu.
* Cần tổng hợp thông tin IoT, lịch sử bảo trì và tình trạng asset để đánh giá thiết bị.

#### Key Expectation
* Có đầy đủ dữ liệu để biết tài sản nào đang có vấn đề, tài sản nào có nguy cơ và cần hành động gì tiếp theo.

---

### 2.5. Persona 04 – Admin

#### Thông tin chung
| Thuộc tính | Mô tả |
| --- | --- |
| **Role** | Admin |
| **Người dùng thực tế** | Quản trị viên hệ thống |
| **Mức độ sử dụng** | Theo nhu cầu quản trị |
| **Mục tiêu chính** | Đảm bảo người dùng và cấu hình hệ thống hoạt động chính xác |

#### Bối cảnh
Admin không trực tiếp xử lý các hoạt động bảo trì. Vai trò của Admin tập trung vào quản trị hệ thống, người dùng, quyền truy cập và kết nối giữa tài sản với thiết bị IoT.

#### Goals
* Quản lý tài khoản người dùng.
* Tạo, cập nhật hoặc vô hiệu hóa tài khoản.
* Phân quyền người dùng theo role.
* Đảm bảo người dùng chỉ truy cập được chức năng phù hợp.
* Quản lý thông tin kết nối giữa Asset và IoT Device/Sensor.
* Cập nhật hoặc thay đổi mapping khi thiết bị IoT được thay thế.
* Kiểm tra trạng thái kết nối của IoT Device.

#### Pain Points
* Sai role có thể dẫn đến người dùng truy cập sai chức năng.
* Asset và IoT Device cần được mapping chính xác.
* Thiết bị IoT thay đổi hoặc được thay thế cần được cập nhật trên hệ thống.

#### Key Expectation
* Người dùng đúng quyền – tài sản được mapping đúng IoT Device – hệ thống vận hành ổn định.

---

### 2.6. Stakeholder–Goal Summary

| Persona | Primary Goal | Key System Needs |
| --- | --- | --- |
| **Requester** | Báo lỗi và theo dõi yêu cầu | Maintenance Request, Request Tracking |
| **Technician** | Xử lý và hoàn thành Work Order | Work Order, Asset Detail, IoT/AI Information |
| **Facility Manager** | Quản lý tài sản và chủ động bảo trì | Asset Management, IoT Monitoring, AI Prediction, Work Order |
| **Admin** | Quản trị người dùng và hệ thống | User Management, Role Management, IoT Mapping |

---

### 2.7. Priority of Stakeholders
Dựa trên mục tiêu của hệ thống, mức độ ưu tiên có thể xác định như sau:

**Primary Users**
* **Facility Manager** – người ra quyết định và điều phối hoạt động bảo trì.
* **Technician** – người trực tiếp thực hiện maintenance.
* **Requester** – nguồn phát sinh Reactive Maintenance Request.

**Supporting User**
* **Admin** – đảm bảo hệ thống và quyền truy cập hoạt động đúng.

*Lưu ý: Admin là role bắt buộc của hệ thống nhưng không phải người sử dụng các chức năng nghiệp vụ bảo trì chính. Do đó, Admin được xem là Supporting User/Stakeholder thay vì Primary Business User.*
