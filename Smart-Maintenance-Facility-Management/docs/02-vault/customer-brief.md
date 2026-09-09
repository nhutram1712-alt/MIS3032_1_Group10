# Customer Brief

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.0
> **Status:** Baseline
> **Scope:** MVP

## 1. Nhu cầu hệ thống

Trường Đại học Kinh tế – Đại học Đà Nẵng cần một hệ thống nội bộ để quản lý tài sản cơ sở vật chất và hỗ trợ hoạt động bảo trì.

Trong giai đoạn MVP, hệ thống tập trung vào **5 loại tài sản**:

* Wi-Fi
* Air Conditioner
* Projector
* Light
* Fan

Hệ thống hỗ trợ:

* Quản lý thông tin và trạng thái Asset.
* Tiếp nhận và theo dõi Maintenance Request.
* Tạo và phân công Work Order cho Technician.
* Theo dõi kết quả bảo trì và lưu Maintenance History.
* Tích hợp IoT để thu thập và giám sát dữ liệu của Asset.
* Sử dụng AI để dự đoán khả năng Asset cần bảo trì trong **7 ngày tiếp theo**.

AI chỉ đóng vai trò **hỗ trợ ra quyết định**; Facility Manager là người đưa ra quyết định cuối cùng.

---

## 2. Requester phải có thể

Requester là người sử dụng tài sản thuộc phạm vi được phép, có thể bao gồm giảng viên, sinh viên hoặc nhân viên trong trường.

Requester phải có thể:

1. Đăng nhập bằng tài khoản của hệ thống.
2. Xem các Asset thuộc phòng/khu vực mình được phép sử dụng.
3. Xem trạng thái hiện tại của Asset.
4. Tạo Maintenance Request để báo cáo vấn đề của Asset hoặc khu vực.
5. Theo dõi trạng thái Maintenance Request.

Maintenance Request có thể sử dụng các trạng thái:

* Submitted
* Pending
* In Progress
* Resolved
* Closed
* Rejected

Hình ảnh hoặc video có thể được đính kèm khi báo lỗi nhưng **không bắt buộc**.

---

## 3. Facility Manager phải có thể

Facility Manager phải có thể:

1. Thêm Asset với các thông tin cơ bản:

   * Asset ID
   * Name
   * Type
   * Location
   * Status
2. Cập nhật thông tin cơ bản của Asset.
3. Xem và thay đổi thủ công Asset Status.
4. Xem IoT Data của Asset.
5. Tiếp nhận và xử lý Maintenance Request.
6. Tạo Work Order từ Maintenance Request hoặc nhu cầu bảo trì được xác định.
7. Phân công Work Order cho Technician.
8. Theo dõi trạng thái Work Order.
9. Xem IoT Alert khi Asset có dữ liệu bất thường.
10. Xem AI Prediction và Maintenance Risk của Asset.
11. Ưu tiên Asset có Maintenance Risk = High.
12. Xác nhận kết quả bảo trì trước khi đóng Maintenance Request.

Work Order trong MVP sử dụng các trạng thái:

* Assigned
* In Progress
* Completed
* Cancelled

---

## 4. Technician phải có thể

Technician phải có thể:

1. Xem các Work Order được phân công cho mình.
2. Xem thông tin Asset liên quan đến Work Order.
3. Xem IoT Alert và AI Prediction liên quan đến Asset.
4. Cập nhật Work Order được phân công.
5. Từ chối Work Order kèm lý do khi cần.
6. Ghi nhận kết quả kiểm tra, sửa chữa hoặc bảo trì.
7. Hoàn thành Work Order sau khi thực hiện và ghi nhận kết quả.

Khi Work Order hoàn thành, kết quả được lưu vào **Maintenance History** của Asset.

> Trong MVP, hành động “từ chối Work Order” được xử lý như một hành động của Technician và chưa được xác định là một trạng thái Work Order riêng. Việc có thêm trạng thái `Rejected` là Open Question và cần được xác nhận trước khi thay đổi baseline.

---

## 5. Admin phải có thể

Admin phải có thể:

1. Quản lý tài khoản người dùng.
2. Quản lý quyền truy cập theo Role.
3. Mapping Asset với IoT Device/Sensor.
4. Cập nhật IoT Mapping.

Trong MVP, mỗi Asset chỉ được mapping với **một IoT Device/Sensor**.

---

## 6. Tích hợp IoT

IoT được sử dụng để thu thập và lưu trữ dữ liệu liên quan đến Asset.

Hệ thống:

* Thu thập IoT Data theo chu kỳ mặc định **5 phút/lần**.
* Cho phép Facility Manager xem IoT Data của Asset.
* Phát hiện dữ liệu đáp ứng điều kiện bất thường hoặc threshold được cấu hình.
* Tạo **IoT Alert** khi điều kiện cảnh báo được đáp ứng.
* Sử dụng IoT Data phù hợp làm nguồn cho AI Prediction.

Trong phạm vi MVP, hệ thống **không cố định một danh sách sensor/measurement cụ thể cho từng Asset Type**. Loại dữ liệu IoT cụ thể phụ thuộc vào Device/Sensor và cấu hình được sử dụng.

---

## 7. Tích hợp AI

AI sử dụng **IoT Data và Maintenance History phù hợp** để hỗ trợ đánh giá nhu cầu bảo trì của Asset.

Hệ thống cung cấp:

* **AI Prediction:** khả năng Asset cần bảo trì trong **7 ngày tiếp theo**.
* **Maintenance Risk:** Low, Medium hoặc High.

Kết quả AI được liên kết với Asset và thời điểm thực hiện Prediction.

AI được sử dụng để hỗ trợ Facility Manager xác định các Asset có nguy cơ cần được ưu tiên bảo trì.

> AI không tự động quyết định hoặc thực hiện hoạt động bảo trì. Quyết định cuối cùng thuộc về Facility Manager.

Trong MVP, AI có thể sử dụng **dữ liệu mẫu/giả lập** nếu dữ liệu thực tế chưa đủ.

---

## 8. Thông tin của Asset

Thông tin cơ bản của mỗi Asset gồm:

* Asset ID
* Name
* Type
* Location/Room
* Status

Ngoài thông tin cơ bản, hệ thống có thể liên kết Asset với:

* IoT Data
* IoT Alert
* AI Prediction
* Maintenance Risk
* Maintenance History

Mỗi Asset phải có Asset ID duy nhất và Location/Room.

---

## 9. Quy trình bảo trì tổng quát

Quy trình MVP được tổ chức theo chuỗi:

```text
Requester
    │
    │ Báo lỗi
    ▼
Maintenance Request
    │
    │ Facility Manager xử lý
    ▼
Work Order
    │
    │ Phân công
    ▼
Technician
    │
    │ Kiểm tra / sửa chữa / bảo trì
    ▼
Maintenance Result
    │
    ├──► Maintenance History
    │
    ▼
Facility Manager xác nhận
    │
    ▼
Maintenance Request → Closed
```

Song song với quy trình trên:

```text
IoT Device/Sensor
       │
       ▼
   IoT Data
       │
       ├──────────────► IoT Alert
       │
       ▼
 AI Prediction
       │
       ▼
Maintenance Risk
       │
       ▼
Facility Manager
```

AI và IoT cung cấp thông tin hỗ trợ cho hoạt động bảo trì nhưng không tự động thay thế quyết định của Facility Manager.

---

## 10. Phạm vi giai đoạn đầu

### In Scope

MVP tập trung vào:

* Authentication.
* User Account & Role Management.
* Asset Management.
* Maintenance Request.
* Work Order.
* Maintenance History.
* IoT Mapping.
* IoT Data Monitoring.
* IoT Alert.
* AI Predictive Maintenance.
* Maintenance Risk.
* Notification cho Facility Manager khi có IoT Alert nghiêm trọng hoặc Maintenance Risk = High.

### Out of Scope

MVP không bao gồm:

* Finance.
* Procurement.
* Inventory.
* Spare Parts Management.
* Supplier/Vendor Management.
* HR Management.
* Academic Management.
* Student Management.
* General University Administration.
* SSO.

---

## 11. Các vai trò chính

| Role                 | Trách nhiệm chính                                                                                                  |
| -------------------- | ------------------------------------------------------------------------------------------------------------------ |
| **Requester**        | Xem Asset được phép sử dụng, báo lỗi và theo dõi Maintenance Request                                               |
| **Facility Manager** | Quản lý Asset, xử lý Request, tạo/phân công Work Order, giám sát IoT và sử dụng AI Prediction để hỗ trợ quyết định |
| **Technician**       | Thực hiện Work Order, cập nhật và ghi nhận kết quả bảo trì                                                         |
| **Admin**            | Quản lý tài khoản, Role/Permission và IoT Mapping                                                                  |

---

## 12. Nguyên tắc phạm vi

Customer Brief này chỉ mô tả nhu cầu và phạm vi ở mức business/product context. Các chi tiết về:

* Functional Requirements,
* Non-Functional Requirements,
* Business Rules,
* Use Cases,
* User Stories,
* Acceptance Criteria

được quản lý trong các tài liệu tương ứng và phải được traceability với Customer Brief.


