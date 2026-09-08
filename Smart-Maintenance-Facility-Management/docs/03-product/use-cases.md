# Use Cases
# Use Cases

## 1. Overview

### 1.1. Purpose

Tài liệu này mô tả các **Use Case (UC)** chính của hệ thống **Smart Maintenance & Facility Management**.

Use Case được xây dựng dựa trên:

* Baseline Requirements `REQ-01 → REQ-30`
* Business Rules `BR-01 → BR-18`
* User Stories `US-01-01 → US-06-03`
* Acceptance Criteria đã được xác định.

Mục tiêu của Use Case là mô tả **tương tác giữa Actor và hệ thống**, bao gồm:

* Actor thực hiện hành động gì.
* Điều kiện để bắt đầu Use Case.
* Luồng chính.
* Các luồng thay thế và ngoại lệ.
* Kết quả sau khi Use Case hoàn thành.

---

# 2. Actors

| Actor                     | Mô tả                                                                                          |
| ------------------------- | ---------------------------------------------------------------------------------------------- |
| **Requester**             | Người sử dụng phòng/khu vực, có thể báo cáo sự cố và theo dõi Maintenance Request.             |
| **Technician**            | Nhân viên kỹ thuật thực hiện các công việc bảo trì được phân công.                             |
| **Facility Manager**      | Người quản lý cơ sở vật chất, tiếp nhận Request, quản lý Asset, Work Order và giám sát IoT/AI. |
| **Admin**                 | Quản trị tài khoản, Role và cấu hình IoT Mapping.                                              |
| **IoT Device/Sensor**     | Thiết bị cung cấp dữ liệu về tình trạng Asset cho hệ thống.                                    |
| **AI Prediction Service** | Thành phần AI phân tích dữ liệu và đưa ra dự đoán nhu cầu bảo trì.                             |

---

# 3. Use Case Diagram Overview

Hệ thống được chia thành 6 nhóm nghiệp vụ:

1. Authentication & Access Control
2. Asset Management
3. Maintenance Request
4. Work Order & Maintenance History
5. IoT Monitoring
6. AI Predictive Maintenance

### Use Case List

| UC ID | Use Case                                  | Primary Actor                 |
| ----- | ----------------------------------------- | ----------------------------- |
| UC-01 | Đăng nhập hệ thống                        | User                          |
| UC-02 | Quản lý tài khoản và Role                 | Admin                         |
| UC-03 | Xem Asset và trạng thái Asset             | Requester                     |
| UC-04 | Quản lý Asset                             | Facility Manager              |
| UC-05 | Tạo Maintenance Request                   | Requester                     |
| UC-06 | Theo dõi Maintenance Request              | Requester                     |
| UC-07 | Tiếp nhận và xử lý Maintenance Request    | Facility Manager              |
| UC-08 | Tạo và phân công Work Order               | Facility Manager              |
| UC-09 | Thực hiện Work Order                      | Technician                    |
| UC-10 | Theo dõi và hoàn tất Work Order           | Facility Manager / Technician |
| UC-11 | Quản lý IoT Mapping                       | Admin                         |
| UC-12 | Giám sát IoT Data                         | Facility Manager              |
| UC-13 | Xử lý IoT Alert                           | Facility Manager              |
| UC-14 | Dự đoán nhu cầu bảo trì bằng AI           | Facility Manager              |
| UC-15 | Xem Maintenance Risk và AI Prediction     | Facility Manager / Technician |
| UC-16 | Gửi Notification về cảnh báo nghiêm trọng | System                        |

---

# 4. Detailed Use Cases

# UC-01 — Đăng nhập hệ thống

**Primary Actor:** User
**Supporting Actor:** System

**Related User Stories:**

* US-01-01

**Related Requirements:**

* REQ-01

**Related NFR:**

* NFR-01, NFR-02

### Preconditions

* User đã có tài khoản hợp lệ.
* Hệ thống đang hoạt động.

### Main Flow

1. User truy cập màn hình đăng nhập.
2. User nhập thông tin tài khoản.
3. User chọn **Login**.
4. Hệ thống kiểm tra thông tin đăng nhập.
5. Hệ thống xác thực tài khoản.
6. Hệ thống xác định Role của User.
7. Hệ thống cho phép User truy cập các chức năng phù hợp với Role.

### Alternative / Exception Flows

**A1 — Sai thông tin đăng nhập**

1. Hệ thống xác định thông tin đăng nhập không hợp lệ.
2. Hệ thống từ chối đăng nhập.
3. Hệ thống hiển thị thông báo lỗi.

**A2 — Tài khoản không được phép truy cập**

1. Hệ thống xác định tài khoản không có quyền truy cập.
2. Hệ thống từ chối truy cập.

### Postconditions

* User đăng nhập thành công và được cấp quyền truy cập theo Role.

---

# UC-02 — Quản lý tài khoản và Role

**Primary Actor:** Admin

**Related User Stories:**

* US-01-03
* US-01-04

**Related Requirements:**

* REQ-24
* REQ-25

**Related NFR:**

* NFR-01, NFR-02, NFR-07

### Preconditions

* Admin đã đăng nhập.
* Admin có quyền quản lý User và Role.

### Main Flow

1. Admin truy cập chức năng quản lý User/Role.
2. Hệ thống hiển thị danh sách User và Role.
3. Admin thực hiện một trong các thao tác:

   * Xem tài khoản.
   * Tạo tài khoản.
   * Cập nhật tài khoản.
   * Cấu hình quyền theo Role.
4. Admin lưu thay đổi.
5. Hệ thống kiểm tra dữ liệu.
6. Hệ thống lưu thay đổi.
7. Hệ thống áp dụng quyền mới cho User.

### Alternative / Exception Flows

**A1 — Thông tin tài khoản không hợp lệ**

* Hệ thống không lưu dữ liệu và hiển thị lỗi.

**A2 — Admin cố gắng truy cập chức năng không được phép**

* Hệ thống từ chối thao tác.

### Postconditions

* Tài khoản hoặc cấu hình Role được cập nhật thành công.

---

# UC-03 — Xem Asset và trạng thái Asset

**Primary Actor:** Requester

**Related User Stories:**

* US-01-02
* US-02-04

**Related Requirements:**

* REQ-02
* REQ-03

**Related Business Rules:**

* BR-04

**Related NFR:**

* NFR-01, NFR-07

### Preconditions

* Requester đã đăng nhập.
* Requester có quyền truy cập phòng/khu vực.

### Main Flow

1. Requester truy cập danh sách Asset.
2. Hệ thống xác định phạm vi phòng/khu vực Requester được phép sử dụng.
3. Hệ thống hiển thị các Asset thuộc phạm vi được phép.
4. Requester chọn một Asset.
5. Hệ thống hiển thị thông tin Asset.
6. Hệ thống hiển thị trạng thái hiện tại của Asset.

### Alternative / Exception Flows

**A1 — Asset ngoài phạm vi**

* Hệ thống không hiển thị hoặc từ chối truy cập Asset ngoài phạm vi được phép.

**A2 — Asset không tồn tại**

* Hệ thống thông báo Asset không tồn tại.

### Postconditions

* Requester xem được thông tin và trạng thái của Asset được phép truy cập.

---

# UC-04 — Quản lý Asset

**Primary Actor:** Facility Manager

**Related User Stories:**

* US-02-01
* US-02-02
* US-02-03

**Related Requirements:**

* REQ-06
* REQ-07
* REQ-08

**Related Business Rules:**

* BR-01
* BR-02
* BR-03
* BR-04
* BR-16

**Related NFR:**

* NFR-03, NFR-04, NFR-06

### Preconditions

* Facility Manager đã đăng nhập.

### Main Flow

1. Facility Manager truy cập quản lý Asset.
2. Hệ thống hiển thị danh sách Asset.
3. Facility Manager chọn:

   * Thêm Asset.
   * Cập nhật Asset.
   * Xem Asset.
   * Thay đổi Asset Status.
4. Hệ thống kiểm tra dữ liệu.
5. Hệ thống lưu thay đổi.
6. Hệ thống cập nhật thông tin Asset.

### Alternative / Exception Flows

**A1 — Asset ID bị trùng**

* Hệ thống từ chối tạo Asset.

**A2 — Asset Type không được hỗ trợ**

* Hệ thống yêu cầu chọn Asset Type thuộc phạm vi MVP.

**A3 — Thiếu thông tin bắt buộc**

* Hệ thống yêu cầu bổ sung thông tin.

**A4 — User không phải Facility Manager**

* Hệ thống từ chối thay đổi Asset Status.

### Postconditions

* Asset được tạo/cập nhật thành công hoặc trạng thái Asset được thay đổi.

---

# UC-05 — Tạo Maintenance Request

**Primary Actor:** Requester

**Related User Stories:**

* US-03-01

**Related Requirements:**

* REQ-04

**Related Business Rules:**

* BR-05

**Related NFR:**

* NFR-03, NFR-04

### Preconditions

* Requester đã đăng nhập.
* Requester có quyền sử dụng phòng/khu vực liên quan.

### Main Flow

1. Requester chọn chức năng tạo Maintenance Request.
2. Requester mô tả vấn đề.
3. Requester xác định Asset hoặc khu vực bị ảnh hưởng.
4. Requester có thể đính kèm hình ảnh/video nếu có.
5. Requester gửi Request.
6. Hệ thống kiểm tra thông tin.
7. Hệ thống tạo Maintenance Request.
8. Hệ thống đặt trạng thái `Submitted`.
9. Hệ thống lưu timestamp.

### Alternative / Exception Flows

**A1 — Không xác định Asset hoặc khu vực**

* Hệ thống không cho phép gửi Request.

**A2 — Không có hình ảnh/video**

* Hệ thống vẫn cho phép gửi Request vì hình ảnh/video không bắt buộc.

**A3 — Thông tin không hợp lệ**

* Hệ thống hiển thị lỗi và yêu cầu Requester chỉnh sửa.

### Postconditions

* Maintenance Request được tạo với trạng thái `Submitted`.

---

# UC-06 — Theo dõi Maintenance Request

**Primary Actor:** Requester

**Related User Stories:**

* US-03-02

**Related Requirements:**

* REQ-05

**Related Business Rules:**

* BR-14

**Related NFR:**

* NFR-03, NFR-04

### Preconditions

* Requester đã đăng nhập.
* Requester có Maintenance Request.

### Main Flow

1. Requester truy cập danh sách Maintenance Request của mình.
2. Hệ thống hiển thị các Request.
3. Requester chọn một Request.
4. Hệ thống hiển thị thông tin Request.
5. Hệ thống hiển thị trạng thái hiện tại.

### Business Status

Maintenance Request sử dụng các trạng thái:

`Submitted → Pending → In Progress → Resolved → Closed`

hoặc:

`Submitted → Rejected`

### Alternative / Exception Flows

**A1 — Không có Request**

* Hệ thống hiển thị danh sách trống.

### Postconditions

* Requester biết được trạng thái hiện tại của Maintenance Request.

---

# UC-07 — Tiếp nhận và xử lý Maintenance Request

**Primary Actor:** Facility Manager

**Related User Stories:**

* US-03-03
* US-03-04

**Related Requirements:**

* REQ-13

**Related Business Rules:**

* BR-05
* BR-14
* BR-18

**Related NFR:**

* NFR-03, NFR-04

### Preconditions

* Facility Manager đã đăng nhập.
* Có Maintenance Request cần xử lý.

### Main Flow

1. Facility Manager xem danh sách Maintenance Request.
2. Facility Manager chọn một Request.
3. Hệ thống hiển thị thông tin Request.
4. Facility Manager tiếp nhận Request.
5. Facility Manager kiểm tra vấn đề.
6. Facility Manager xác định hướng xử lý.
7. Nếu cần Technician thực hiện bảo trì, Facility Manager tạo Work Order.
8. Request được cập nhật theo tiến độ xử lý.
9. Sau khi bảo trì hoàn thành, Facility Manager kiểm tra kết quả.
10. Facility Manager xác nhận kết quả.
11. Hệ thống chuyển Request sang `Closed`.

### Alternative / Exception Flows

**A1 — Request thiếu thông tin**

* Facility Manager yêu cầu bổ sung thông tin hoặc xử lý theo trạng thái phù hợp.

**A2 — Work Order chưa hoàn thành**

* Facility Manager không thể đóng Request.

**A3 — Kết quả bảo trì không đạt**

* Facility Manager không xác nhận và Request tiếp tục được xử lý.

### Postconditions

* Request được xử lý hoặc chuyển sang trạng thái `Closed` sau khi kết quả được xác nhận.

---

# UC-08 — Tạo và phân công Work Order

**Primary Actor:** Facility Manager

**Related User Stories:**

* US-04-01
* US-04-02

**Related Requirements:**

* REQ-14
* REQ-15

**Related Business Rules:**

* BR-05
* BR-06
* BR-07
* BR-08

**Related NFR:**

* NFR-03, NFR-04

### Preconditions

* Facility Manager đã đăng nhập.
* Có Maintenance Request hoặc nhu cầu bảo trì đã được xác định.
* Asset liên quan đã được xác định.

### Main Flow

1. Facility Manager chọn Maintenance Request hoặc nhu cầu bảo trì.
2. Facility Manager chọn tạo Work Order.
3. Hệ thống kiểm tra Asset liên quan.
4. Hệ thống kiểm tra Request chưa có Work Order.
5. Hệ thống tạo Work Order.
6. Facility Manager chọn Technician.
7. Facility Manager phân công Work Order.
8. Hệ thống liên kết Work Order với Technician.
9. Hệ thống chuyển trạng thái Work Order sang `Assigned`.
10. Hệ thống lưu timestamp.

### Alternative / Exception Flows

**A1 — Request đã có Work Order**

* Hệ thống không cho phép tạo thêm Work Order từ Request đó.

**A2 — Chưa xác định Asset**

* Hệ thống yêu cầu xác định Asset trước khi tạo Work Order.

**A3 — Người được chọn không phải Technician**

* Hệ thống không cho phép phân công.

### Postconditions

* Work Order được tạo và phân công cho Technician.

---

# UC-09 — Thực hiện Work Order

**Primary Actor:** Technician

**Related User Stories:**

* US-04-04
* US-04-05
* US-04-06
* US-06-03

**Related Requirements:**

* REQ-17
* REQ-18
* REQ-19
* REQ-20
* REQ-21
* REQ-22

**Related Business Rules:**

* BR-07
* BR-08
* BR-09
* BR-10
* BR-11
* BR-17

**Related NFR:**

* NFR-01, NFR-03, NFR-04, NFR-07, NFR-08

### Preconditions

* Technician đã đăng nhập.
* Work Order được phân công cho Technician.

### Main Flow

1. Technician xem danh sách Work Order được phân công.
2. Technician chọn Work Order.
3. Hệ thống hiển thị thông tin Asset.
4. Hệ thống hiển thị IoT Alert và AI Prediction nếu có.
5. Technician bắt đầu thực hiện công việc.
6. Work Order chuyển sang `In Progress`.
7. Technician kiểm tra Asset.
8. Technician thực hiện sửa chữa/bảo trì.
9. Technician ghi nhận kết quả.
10. Technician hoàn thành Work Order.
11. Hệ thống chuyển Work Order sang `Completed`.
12. Hệ thống lưu kết quả vào Maintenance History.

### Alternative / Exception Flows

**A1 — Technician từ chối Work Order**

1. Technician chọn từ chối Work Order.
2. Technician nhập lý do.
3. Hệ thống kiểm tra lý do.
4. Hệ thống ghi nhận hành động từ chối và lý do.

**A2 — Không nhập lý do**

* Hệ thống không cho phép hoàn tất thao tác từ chối.

**A3 — Work Order không thuộc Technician**

* Hệ thống từ chối thao tác cập nhật.

**A4 — Chưa ghi nhận kết quả**

* Hệ thống không cho phép hoàn thành Work Order.

### Postconditions

* Work Order được hoàn thành và kết quả được lưu vào Maintenance History.

---

# UC-10 — Theo dõi và hoàn tất Work Order

**Primary Actor:** Facility Manager
**Supporting Actor:** Technician

**Related User Stories:**

* US-04-03
* US-04-06

**Related Requirements:**

* REQ-16
* REQ-21
* REQ-22
* REQ-23

**Related Business Rules:**

* BR-09
* BR-17
* BR-18

### Preconditions

* Work Order tồn tại.

### Main Flow

1. Facility Manager mở danh sách Work Order.
2. Hệ thống hiển thị Work Order và trạng thái.
3. Facility Manager theo dõi tiến độ.
4. Technician cập nhật Work Order.
5. Technician ghi nhận kết quả.
6. Technician hoàn thành Work Order.
7. Hệ thống cập nhật trạng thái `Completed`.
8. Hệ thống lưu Maintenance History.
9. Facility Manager kiểm tra kết quả.
10. Facility Manager xác nhận kết quả.
11. Maintenance Request liên quan được phép chuyển sang `Closed`.

### Alternative / Exception Flows

**A1 — Work Order bị Cancelled**

* Work Order chuyển sang `Cancelled` và không được tiếp tục thực hiện.

**A2 — Kết quả chưa được ghi nhận**

* Work Order không được chuyển sang `Completed`.

**A3 — Facility Manager chưa xác nhận**

* Maintenance Request chưa được chuyển sang `Closed`.

### Postconditions

* Work Order có trạng thái cuối cùng phù hợp.
* Maintenance History được cập nhật nếu Work Order hoàn thành.

---

# UC-11 — Quản lý IoT Mapping

**Primary Actor:** Admin

**Related User Stories:**

* US-05-01
* US-05-02

**Related Requirements:**

* REQ-26
* REQ-27

**Related Business Rule:**

* BR-13

**Related NFR:**

* NFR-06

### Preconditions

* Admin đã đăng nhập.
* Asset và IoT Device/Sensor tồn tại.

### Main Flow

1. Admin truy cập IoT Mapping.
2. Hệ thống hiển thị Asset và IoT Device/Sensor.
3. Admin chọn Asset.
4. Admin chọn IoT Device/Sensor.
5. Admin thực hiện mapping.
6. Hệ thống kiểm tra mapping.
7. Hệ thống lưu mapping.
8. Admin có thể cập nhật mapping khi cần.

### Alternative / Exception Flows

**A1 — Asset đã có Device**

* Hệ thống không cho phép mapping thêm Device khác trong MVP.

**A2 — Device không tồn tại**

* Hệ thống từ chối mapping.

**A3 — Asset không tồn tại**

* Hệ thống từ chối mapping.

### Postconditions

* Asset được liên kết với tối đa một IoT Device/Sensor trong MVP.

---

# UC-12 — Giám sát IoT Data

**Primary Actor:** Facility Manager
**Supporting Actor:** IoT Device/Sensor

**Related User Stories:**

* US-05-03

**Related Requirements:**

* REQ-09
* REQ-28

**Related Business Rules:**

* BR-12
* BR-13

**Related NFR:**

* NFR-04
* NFR-05
* NFR-06

### Preconditions

* Asset đã được mapping với IoT Device/Sensor.
* IoT Device/Sensor có thể gửi dữ liệu.

### Main Flow

1. IoT Device/Sensor gửi dữ liệu.
2. Hệ thống nhận IoT Data.
3. Hệ thống xác định Asset tương ứng thông qua IoT Mapping.
4. Hệ thống lưu IoT Data.
5. Hệ thống thực hiện thu thập theo chu kỳ cấu hình.
6. Facility Manager truy cập thông tin Asset.
7. Hệ thống hiển thị IoT Data của Asset.

### Alternative / Exception Flows

**A1 — Asset chưa mapping**

* Hệ thống không thể liên kết dữ liệu với Asset và thông báo cấu hình chưa hoàn tất.

**A2 — Không nhận được dữ liệu**

* Hệ thống ghi nhận trạng thái không có dữ liệu mới.

### Postconditions

* IoT Data được lưu và có thể được sử dụng cho monitoring và AI Prediction.

---

# UC-13 — Xử lý IoT Alert

**Primary Actor:** Facility Manager
**Supporting Actor:** System

**Related User Stories:**

* US-05-04

**Related Requirements:**

* REQ-10
* REQ-30

**Related Business Rule:**

* BR-12

**Related NFR:**

* NFR-04

### Preconditions

* Asset đã được mapping với IoT Device/Sensor.
* Hệ thống nhận được IoT Data.

### Main Flow

1. Hệ thống nhận IoT Data.
2. Hệ thống kiểm tra dữ liệu với threshold/điều kiện bất thường.
3. Hệ thống phát hiện điều kiện bất thường.
4. Hệ thống tạo IoT Alert.
5. Hệ thống liên kết Alert với Asset.
6. Facility Manager xem Alert.
7. Facility Manager đánh giá và quyết định hướng xử lý.

### Alternative / Exception Flows

**A1 — Dữ liệu trong giới hạn bình thường**

* Hệ thống không tạo Alert.

**A2 — Alert nghiêm trọng**

* Hệ thống tạo Alert và kích hoạt Notification cho Facility Manager.

### Postconditions

* IoT Alert được lưu và Facility Manager được thông báo nếu Alert nghiêm trọng.

---

# UC-14 — Dự đoán nhu cầu bảo trì bằng AI

**Primary Actor:** Facility Manager
**Supporting Actor:** AI Prediction Service

**Related User Stories:**

* US-06-01

**Related Requirements:**

* REQ-11
* REQ-29

**Related Business Rules:**

* BR-10
* BR-11
* BR-15

**Related NFR:**

* NFR-08

### Preconditions

* Asset có dữ liệu IoT phù hợp.
* Hệ thống có Maintenance History phù hợp hoặc dữ liệu mẫu/giả lập cho MVP.

### Main Flow

1. Hệ thống thu thập IoT Data và Maintenance History.
2. Hệ thống cung cấp dữ liệu phù hợp cho AI Prediction Service.
3. AI phân tích dữ liệu.
4. AI tạo Prediction về khả năng Asset cần bảo trì trong 7 ngày tiếp theo.
5. Hệ thống lưu Prediction.
6. Hệ thống liên kết Prediction với Asset.
7. Hệ thống lưu thời điểm Prediction.
8. Facility Manager xem kết quả.

### Alternative / Exception Flows

**A1 — Không đủ dữ liệu**

* Hệ thống không tạo Prediction đáng tin cậy và thông báo tình trạng thiếu dữ liệu.

**A2 — AI Prediction thất bại**

* Hệ thống không lưu kết quả không hợp lệ và ghi nhận lỗi xử lý.

### Postconditions

* AI Prediction được lưu cùng Asset và thời điểm Prediction.

---

# UC-15 — Xem Maintenance Risk và AI Prediction

**Primary Actor:** Facility Manager / Technician

**Related User Stories:**

* US-06-02
* US-06-03

**Related Requirements:**

* REQ-12
* REQ-19

**Related Business Rules:**

* BR-10
* BR-11
* BR-15

**Related NFR:**

* NFR-08

### Preconditions

* Asset có AI Prediction hợp lệ.

### Main Flow

1. Actor truy cập thông tin Asset.
2. Hệ thống kiểm tra AI Prediction.
3. Hệ thống hiển thị Maintenance Risk:

   * `Low`
   * `Medium`
   * `High`
4. Hệ thống hiển thị AI Prediction liên quan.
5. Nếu Actor là Technician, hệ thống chỉ hiển thị thông tin liên quan đến Asset trong Work Order được phân công.
6. Facility Manager sử dụng thông tin để hỗ trợ quyết định ưu tiên bảo trì.

### Alternative / Exception Flows

**A1 — Không có Prediction**

* Hệ thống thông báo chưa có AI Prediction.

**A2 — Risk = High**

* Hệ thống đánh dấu Asset có mức rủi ro cao để Facility Manager dễ nhận diện.

### Postconditions

* Actor xem được AI Prediction và Maintenance Risk phù hợp với quyền truy cập.

---

# UC-16 — Gửi Notification về cảnh báo nghiêm trọng

**Primary Actor:** System
**Recipient:** Facility Manager

**Related User Stories:**

* US-05-04
* US-06-01
* US-06-02

**Related Requirements:**

* REQ-30

### Preconditions

Một trong các điều kiện sau xảy ra:

* Có IoT Alert nghiêm trọng.
* Asset có Maintenance Risk = `High`.

### Main Flow

1. Hệ thống phát hiện điều kiện cần Notification.
2. Hệ thống xác định Facility Manager liên quan.
3. Hệ thống tạo Notification.
4. Hệ thống gửi Notification.
5. Facility Manager nhận và xem Notification.
6. Facility Manager xem Asset/Alert/AI Prediction liên quan.

### Alternative / Exception Flows

**A1 — Không phải Alert nghiêm trọng**

* Hệ thống không gửi Notification theo REQ-30.

**A2 — Risk không phải High**

* Hệ thống không gửi Notification theo điều kiện High Risk.

### Postconditions

* Facility Manager nhận được Notification khi điều kiện cảnh báo đáp ứng.

---

# 5. Use Case Relationships

## 5.1. Maintenance Request → Work Order

Luồng nghiệp vụ chính:

```text
Requester
   │
   ▼
UC-05 Tạo Maintenance Request
   │
   ▼
UC-06 Theo dõi Maintenance Request
   │
   ▼
UC-07 Facility Manager xử lý Request
   │
   ▼
UC-08 Tạo & phân công Work Order
   │
   ▼
UC-09 Technician thực hiện Work Order
   │
   ▼
UC-10 Theo dõi & hoàn tất Work Order
   │
   ▼
Maintenance History
   │
   ▼
UC-07 Xác nhận kết quả
   │
   ▼
Maintenance Request = Closed
```

---

## 5.2. IoT Monitoring → Alert

```text
IoT Device/Sensor
       │
       ▼
UC-12 Giám sát IoT Data
       │
       ▼
Kiểm tra Threshold
       │
       ├── Bình thường
       │
       └── Bất thường
              │
              ▼
       UC-13 IoT Alert
              │
              ▼
       UC-16 Notification
              │
              ▼
       Facility Manager
```

---

## 5.3. IoT + Maintenance History → AI Prediction

```text
IoT Data ─────────────┐
                      │
                      ▼
              UC-14 AI Prediction
                      ▲
                      │
Maintenance History ──┘
                      │
                      ▼
              Maintenance Risk
               │           │
               ▼           ▼
       Facility Manager  Technician
```

---

# 6. Use Case to User Story Mapping

| Use Case | User Stories                           |
| -------- | -------------------------------------- |
| UC-01    | US-01-01                               |
| UC-02    | US-01-03, US-01-04                     |
| UC-03    | US-01-02, US-02-04                     |
| UC-04    | US-02-01, US-02-02, US-02-03           |
| UC-05    | US-03-01                               |
| UC-06    | US-03-02                               |
| UC-07    | US-03-03, US-03-04                     |
| UC-08    | US-04-01, US-04-02                     |
| UC-09    | US-04-04, US-04-05, US-04-06, US-06-03 |
| UC-10    | US-04-03, US-04-06                     |
| UC-11    | US-05-01, US-05-02                     |
| UC-12    | US-05-03                               |
| UC-13    | US-05-04                               |
| UC-14    | US-06-01                               |
| UC-15    | US-06-02, US-06-03                     |
| UC-16    | US-05-04, US-06-01, US-06-02           |

---

# 7. Use Case to Requirement Mapping

| Use Case | Requirements                                   |
| -------- | ---------------------------------------------- |
| UC-01    | REQ-01                                         |
| UC-02    | REQ-24, REQ-25                                 |
| UC-03    | REQ-02, REQ-03                                 |
| UC-04    | REQ-06, REQ-07, REQ-08                         |
| UC-05    | REQ-04                                         |
| UC-06    | REQ-05                                         |
| UC-07    | REQ-13                                         |
| UC-08    | REQ-14, REQ-15                                 |
| UC-09    | REQ-17, REQ-18, REQ-19, REQ-20, REQ-21, REQ-22 |
| UC-10    | REQ-16, REQ-21, REQ-22, REQ-23                 |
| UC-11    | REQ-26, REQ-27                                 |
| UC-12    | REQ-09, REQ-28                                 |
| UC-13    | REQ-10, REQ-30                                 |
| UC-14    | REQ-11, REQ-29                                 |
| UC-15    | REQ-12, REQ-19                                 |
| UC-16    | REQ-30                                         |

---

# 8. Use Case to Business Rule Mapping

| Use Case | Business Rules                           |
| -------- | ---------------------------------------- |
| UC-03    | BR-04                                    |
| UC-04    | BR-01, BR-02, BR-03, BR-04, BR-16        |
| UC-05    | BR-05                                    |
| UC-06    | BR-14                                    |
| UC-07    | BR-05, BR-14, BR-18                      |
| UC-08    | BR-05, BR-06, BR-07, BR-08               |
| UC-09    | BR-07, BR-08, BR-09, BR-10, BR-11, BR-17 |
| UC-10    | BR-09, BR-17, BR-18                      |
| UC-11    | BR-13                                    |
| UC-12    | BR-12, BR-13                             |
| UC-13    | BR-12                                    |
| UC-14    | BR-10, BR-11, BR-15                      |
| UC-15    | BR-10, BR-11, BR-15                      |

---

# 9. Status Lifecycle

## 9.1. Maintenance Request

```text
Submitted
    │
    ▼
Pending
    │
    ▼
In Progress
    │
    ▼
Resolved
    │
    ▼
Closed
```

Alternative:

```text
Submitted
    │
    ▼
Rejected
```

**Business Rule:** Maintenance Request chỉ được `Closed` sau khi Technician hoàn thành Work Order và Facility Manager xác nhận kết quả.

---

## 9.2. Work Order

```text
Assigned
    │
    ▼
In Progress
    │
    ▼
Completed
```

Alternative:

```text
Assigned / In Progress
          │
          ▼
      Cancelled
```

> **Open Question OQ-01:** Cần xác nhận liệu hành động Technician từ chối Work Order có tạo status `Rejected` hay không. Hiện tại `Rejected` chưa được xem là Work Order Status chính thức.

---

# 10. Cross-Cutting Rules

Các quy tắc sau được áp dụng xuyên suốt các Use Case:

### Access Control

* User chỉ được thực hiện chức năng phù hợp với Role.
* Technician chỉ được cập nhật Work Order được phân công cho mình.
* Chỉ Facility Manager được thay đổi thủ công Asset Status.
* Admin quản lý User, Role và IoT Mapping.

### Data Consistency

* Maintenance Request, Work Order và Maintenance History phải nhất quán theo lifecycle.
* Các bản ghi nghiệp vụ quan trọng phải lưu timestamp.

### IoT

* Asset phải được mapping với IoT Device/Sensor trước khi sử dụng dữ liệu monitoring.
* Một Asset chỉ có một IoT Device/Sensor trong MVP.
* MVP mặc định thu thập IoT Data mỗi 5 phút.

### AI

* AI sử dụng IoT Data và Maintenance History phù hợp.
* AI Prediction hướng tới khả năng Asset cần bảo trì trong 7 ngày tiếp theo.
* Maintenance Risk gồm `Low`, `Medium`, `High`.
* AI chỉ hỗ trợ quyết định.
* Facility Manager là người quyết định cuối cùng.

---

# 11. Out of Scope

Các Use Case sau **không thuộc phạm vi MVP**:

* Finance Management.
* Procurement Management.
* Inventory Management.
* Spare Parts Management.
* Supplier/Vendor Management.
* HR Management.
* Academic Management.
* Student Management.
* General University Administration.
* Automatic maintenance execution by AI.
* SSO / University Single Sign-On.

---

# 12. Open Questions

| ID    | Open Question                                                                            | Affected Use Cases | Status  |
| ----- | ---------------------------------------------------------------------------------------- | ------------------ | ------- |
| OQ-01 | Technician từ chối Work Order có tạo Status `Rejected` hay chỉ ghi nhận action + reason? | UC-09, UC-10       | Pending |

---

# 13. Use Case Completeness Check

| Category                      |   Coverage |
| ----------------------------- | ---------: |
| User Stories                  |      25/25 |
| Functional Requirements       |      30/30 |
| Business Rules                |      18/18 |
| Main Roles                    |        4/4 |
| IoT                           |    Covered |
| AI                            |    Covered |
| Maintenance Request lifecycle |    Covered |
| Work Order lifecycle          |    Covered |
| Maintenance History           |    Covered |
| Open Questions                | Documented |

**Conclusion:** Bộ Use Case hiện tại bao phủ đầy đủ các nghiệp vụ chính của MVP và có thể được sử dụng làm cơ sở để xây dựng **Traceability Matrix**.

