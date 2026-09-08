# Acceptance Criteria
# Acceptance Criteria

## 1. Overview

### 1.1. Purpose

Tài liệu này định nghĩa **Acceptance Criteria (AC)** cho các User Stories của hệ thống **Smart Maintenance & Facility Management**.

Acceptance Criteria được sử dụng để:

* Xác định điều kiện để một User Story được xem là hoàn thành.
* Làm cơ sở cho kiểm thử chức năng.
* Đảm bảo User Story đáp ứng đúng Requirements và Business Rules đã được Baselined.
* Hỗ trợ truy vết giữa **User Story → Requirement → Business Rule → Acceptance Criteria → Use Case**.

### 1.2. Acceptance Criteria Format

Các Acceptance Criteria được viết theo cấu trúc:

> **Given** – Điều kiện ban đầu
> **When** – Hành động của người dùng/hệ thống
> **Then** – Kết quả mong đợi

Mỗi User Story có từ **2–6 Acceptance Criteria**, bao gồm happy path và các trường hợp lỗi hoặc điều kiện quan trọng.

---

# 2. EPIC-01 — Authentication & Access Control

## US-01-01 — Đăng nhập hệ thống

**User Story**

> Là một User, tôi muốn đăng nhập bằng tài khoản của hệ thống, để có thể truy cập và sử dụng các chức năng phù hợp với Role của mình.

**Requirement:** REQ-01
**Business Rule:** —
**NFR:** NFR-01, NFR-02

### Acceptance Criteria

**AC-US-01-01-01 — Đăng nhập thành công**

* **Given:** User có tài khoản hợp lệ.
* **When:** User nhập đúng thông tin đăng nhập và chọn đăng nhập.
* **Then:** Hệ thống xác thực tài khoản và cho phép User truy cập hệ thống.

**AC-US-01-01-02 — Sai thông tin đăng nhập**

* **Given:** User nhập sai thông tin đăng nhập.
* **When:** User thực hiện đăng nhập.
* **Then:** Hệ thống từ chối đăng nhập và hiển thị thông báo lỗi phù hợp.

**AC-US-01-01-03 — Truy cập theo Role**

* **Given:** User đăng nhập thành công.
* **When:** User truy cập hệ thống.
* **Then:** Hệ thống chỉ cung cấp các chức năng phù hợp với Role của User.

---

## US-01-02 — Xem Asset được phép truy cập

**User Story**

> Là một Requester, tôi muốn chỉ xem được các Asset thuộc phòng/khu vực tôi được phép sử dụng, để chỉ truy cập thông tin Asset liên quan.

**Requirement:** REQ-02
**Business Rule:** BR-04
**NFR:** NFR-01, NFR-07

### Acceptance Criteria

**AC-US-01-02-01 — Xem Asset được phép**

* **Given:** Requester được phép sử dụng một phòng/khu vực.
* **When:** Requester xem danh sách Asset.
* **Then:** Hệ thống hiển thị các Asset thuộc phòng/khu vực được phép.

**AC-US-01-02-02 — Không xem Asset ngoài phạm vi**

* **Given:** Asset thuộc phòng/khu vực Requester không được phép sử dụng.
* **When:** Requester tìm hoặc truy cập Asset đó.
* **Then:** Hệ thống không cho phép Requester xem thông tin Asset.

---

## US-01-03 — Quản lý tài khoản User

**User Story**

> Là một Admin, tôi muốn quản lý tài khoản User, để duy trì quyền truy cập vào hệ thống.

**Requirement:** REQ-24
**NFR:** NFR-01, NFR-02

### Acceptance Criteria

**AC-US-01-03-01 — Xem danh sách tài khoản**

* **Given:** Admin đã đăng nhập.
* **When:** Admin truy cập chức năng quản lý User.
* **Then:** Hệ thống hiển thị danh sách tài khoản User.

**AC-US-01-03-02 — Thêm tài khoản**

* **Given:** Admin nhập đầy đủ thông tin tài khoản hợp lệ.
* **When:** Admin tạo tài khoản.
* **Then:** Hệ thống tạo tài khoản User mới.

**AC-US-01-03-03 — Dữ liệu không hợp lệ**

* **Given:** Thông tin tài khoản không hợp lệ hoặc bị thiếu.
* **When:** Admin thực hiện tạo tài khoản.
* **Then:** Hệ thống không tạo tài khoản và thông báo trường cần sửa.

---

## US-01-04 — Quản lý quyền theo Role

**User Story**

> Là một Admin, tôi muốn quản lý quyền truy cập theo Role, để User chỉ có thể sử dụng các chức năng được phép.

**Requirement:** REQ-25
**NFR:** NFR-01, NFR-07

### Acceptance Criteria

**AC-US-01-04-01 — Xem quyền của Role**

* **Given:** Admin đã đăng nhập.
* **When:** Admin truy cập quản lý Role.
* **Then:** Hệ thống hiển thị các Role và quyền tương ứng.

**AC-US-01-04-02 — Cập nhật quyền**

* **Given:** Admin có quyền quản lý Role.
* **When:** Admin thay đổi quyền của một Role và lưu.
* **Then:** Hệ thống lưu cấu hình quyền mới.

**AC-US-01-04-03 — Kiểm soát truy cập**

* **Given:** User không có quyền đối với một chức năng.
* **When:** User cố gắng truy cập chức năng đó.
* **Then:** Hệ thống từ chối truy cập.

---

# 3. EPIC-02 — Asset Management

## US-02-01 — Thêm Asset

**User Story**

> Là một Facility Manager, tôi muốn thêm Asset với thông tin cơ bản, để Asset có thể được quản lý trên hệ thống.

**Requirement:** REQ-06
**Business Rules:** BR-01, BR-02, BR-03, BR-04
**NFR:** NFR-03, NFR-04, NFR-06

### Acceptance Criteria

**AC-US-02-01-01 — Thêm Asset hợp lệ**

* **Given:** Facility Manager nhập Asset ID, Name, Type, Location và Status hợp lệ.
* **When:** Facility Manager chọn thêm Asset.
* **Then:** Hệ thống tạo Asset mới và lưu thông tin.

**AC-US-02-01-02 — Asset ID bị trùng**

* **Given:** Asset ID đã tồn tại trong hệ thống.
* **When:** Facility Manager tạo Asset với Asset ID đó.
* **Then:** Hệ thống từ chối tạo Asset và thông báo Asset ID đã tồn tại.

**AC-US-02-01-03 — Asset Type không được hỗ trợ**

* **Given:** Asset Type không thuộc phạm vi MVP.
* **When:** Facility Manager tạo Asset.
* **Then:** Hệ thống từ chối dữ liệu và yêu cầu chọn Asset Type được hỗ trợ.

**AC-US-02-01-04 — Thiếu thông tin bắt buộc**

* **Given:** Một hoặc nhiều trường bắt buộc bị bỏ trống.
* **When:** Facility Manager lưu Asset.
* **Then:** Hệ thống không tạo Asset và thông báo các trường cần bổ sung.

---

## US-02-02 — Cập nhật thông tin Asset

**User Story**

> Là một Facility Manager, tôi muốn cập nhật thông tin Asset, để dữ liệu Asset luôn chính xác.

**Requirement:** REQ-07
**Business Rules:** BR-01, BR-02, BR-04
**NFR:** NFR-03, NFR-04, NFR-06

### Acceptance Criteria

**AC-US-02-02-01 — Cập nhật thành công**

* **Given:** Asset tồn tại trong hệ thống.
* **When:** Facility Manager chỉnh sửa thông tin hợp lệ và lưu.
* **Then:** Hệ thống cập nhật thông tin Asset.

**AC-US-02-02-02 — Không hợp lệ**

* **Given:** Thông tin cập nhật không đáp ứng Business Rules.
* **When:** Facility Manager lưu thay đổi.
* **Then:** Hệ thống từ chối thay đổi và hiển thị lỗi phù hợp.

**AC-US-02-02-03 — Asset không tồn tại**

* **Given:** Asset không tồn tại hoặc không còn khả dụng.
* **When:** Facility Manager cố gắng cập nhật Asset.
* **Then:** Hệ thống thông báo Asset không tồn tại.

---

## US-02-03 — Quản lý trạng thái Asset

**User Story**

> Là một Facility Manager, tôi muốn xem và thay đổi thủ công trạng thái Asset, để duy trì trạng thái vận hành hiện tại.

**Requirement:** REQ-08
**Business Rule:** BR-16
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-02-03-01 — Xem trạng thái**

* **Given:** Asset tồn tại.
* **When:** Facility Manager xem Asset.
* **Then:** Hệ thống hiển thị trạng thái hiện tại của Asset.

**AC-US-02-03-02 — Thay đổi trạng thái**

* **Given:** Facility Manager có quyền thay đổi Asset Status.
* **When:** Facility Manager chọn trạng thái mới hợp lệ và lưu.
* **Then:** Hệ thống cập nhật Asset Status.

**AC-US-02-03-03 — Không đủ quyền**

* **Given:** User không phải Facility Manager.
* **When:** User cố gắng thay đổi Asset Status.
* **Then:** Hệ thống từ chối thao tác.

---

## US-02-04 — Xem trạng thái Asset

**User Story**

> Là một Requester, tôi muốn xem trạng thái hiện tại của Asset được phép truy cập, để biết Asset có đang hoạt động hoặc gặp vấn đề hay không.

**Requirement:** REQ-03
**Business Rule:** BR-04
**NFR:** NFR-01, NFR-07

### Acceptance Criteria

**AC-US-02-04-01 — Xem trạng thái Asset**

* **Given:** Requester được phép truy cập Asset.
* **When:** Requester xem thông tin Asset.
* **Then:** Hệ thống hiển thị trạng thái hiện tại của Asset.

**AC-US-02-04-02 — Asset ngoài phạm vi**

* **Given:** Requester không được phép truy cập Asset.
* **When:** Requester cố gắng xem Asset.
* **Then:** Hệ thống từ chối truy cập thông tin Asset.

---

# 4. EPIC-03 — Maintenance Request

## US-03-01 — Tạo Maintenance Request

**User Story**

> Là một Requester, tôi muốn tạo Maintenance Request cho Asset hoặc khu vực bị ảnh hưởng, để báo cáo vấn đề cần bảo trì.

**Requirement:** REQ-04
**Business Rule:** BR-05
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-03-01-01 — Tạo Request với Asset**

* **Given:** Requester có quyền truy cập Asset.
* **When:** Requester nhập thông tin vấn đề và xác định Asset bị ảnh hưởng.
* **Then:** Hệ thống tạo Maintenance Request với trạng thái `Submitted`.

**AC-US-03-01-02 — Tạo Request theo khu vực**

* **Given:** Requester chỉ xác định được khu vực bị ảnh hưởng.
* **When:** Requester tạo Maintenance Request.
* **Then:** Hệ thống cho phép tạo Request theo khu vực.

**AC-US-03-01-03 — Không xác định Asset hoặc khu vực**

* **Given:** Requester không xác định Asset hoặc khu vực bị ảnh hưởng.
* **When:** Requester gửi Maintenance Request.
* **Then:** Hệ thống không cho phép tạo Request.

**AC-US-03-01-04 — Hình ảnh/video không bắt buộc**

* **Given:** Requester không có hình ảnh hoặc video.
* **When:** Requester gửi Maintenance Request với thông tin hợp lệ.
* **Then:** Hệ thống vẫn cho phép tạo Request.

---

## US-03-02 — Theo dõi Maintenance Request

**User Story**

> Là một Requester, tôi muốn theo dõi trạng thái Maintenance Request, để biết tiến độ xử lý yêu cầu.

**Requirement:** REQ-05
**Business Rule:** BR-14
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-03-02-01 — Xem trạng thái**

* **Given:** Requester có Maintenance Request.
* **When:** Requester xem Request.
* **Then:** Hệ thống hiển thị trạng thái hiện tại.

**AC-US-03-02-02 — Các trạng thái hợp lệ**

* **Given:** Maintenance Request đang được xử lý.
* **When:** Trạng thái Request thay đổi.
* **Then:** Hệ thống chỉ sử dụng các trạng thái `Submitted`, `Pending`, `In Progress`, `Resolved`, `Closed`, `Rejected`.

---

## US-03-03 — Tiếp nhận và xử lý Maintenance Request

**User Story**

> Là một Facility Manager, tôi muốn tiếp nhận và xử lý Maintenance Request, để các vấn đề được báo cáo có thể được giải quyết.

**Requirement:** REQ-13
**Business Rules:** BR-05, BR-14
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-03-03-01 — Tiếp nhận Request**

* **Given:** Có Maintenance Request ở trạng thái `Submitted`.
* **When:** Facility Manager tiếp nhận Request.
* **Then:** Hệ thống cập nhật Request sang trạng thái phù hợp để xử lý.

**AC-US-03-03-02 — Xử lý Request**

* **Given:** Facility Manager đang xử lý một Request hợp lệ.
* **When:** Facility Manager cập nhật tiến độ.
* **Then:** Hệ thống lưu trạng thái mới và timestamp tương ứng.

**AC-US-03-03-03 — Request không hợp lệ**

* **Given:** Request thiếu thông tin cần thiết để xử lý.
* **When:** Facility Manager tiếp nhận Request.
* **Then:** Hệ thống không cho phép chuyển Request sang bước xử lý tiếp theo và yêu cầu bổ sung thông tin.

---

## US-03-04 — Xác nhận và đóng Maintenance Request

**User Story**

> Là một Facility Manager, tôi muốn xác nhận kết quả bảo trì trước khi đóng Request, để chỉ những Request đã được kiểm tra mới được đóng.

**Requirement:** REQ-13
**Business Rule:** BR-18
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-03-04-01 — Xác nhận kết quả**

* **Given:** Technician đã hoàn thành Work Order và ghi nhận kết quả.
* **When:** Facility Manager kiểm tra và xác nhận kết quả.
* **Then:** Hệ thống cho phép đóng Maintenance Request.

**AC-US-03-04-02 — Chưa hoàn thành Work Order**

* **Given:** Work Order liên quan chưa ở trạng thái `Completed`.
* **When:** Facility Manager cố gắng đóng Request.
* **Then:** Hệ thống không cho phép đóng Request.

**AC-US-03-04-03 — Đóng Request**

* **Given:** Work Order đã `Completed` và Facility Manager xác nhận kết quả.
* **When:** Facility Manager đóng Request.
* **Then:** Request chuyển sang trạng thái `Closed`.

---

# 5. EPIC-04 — Work Order & Maintenance History

## US-04-01 — Tạo Work Order

**User Story**

> Là một Facility Manager, tôi muốn tạo Work Order từ Maintenance Request hoặc nhu cầu bảo trì được xác định, để công việc bảo trì được giao và theo dõi chính thức.

**Requirement:** REQ-14
**Business Rules:** BR-05, BR-06, BR-08
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-04-01-01 — Tạo Work Order**

* **Given:** Có Maintenance Request hợp lệ hoặc nhu cầu bảo trì đã được xác định.
* **When:** Facility Manager tạo Work Order.
* **Then:** Hệ thống tạo Work Order và liên kết với một Asset cụ thể.

**AC-US-04-01-02 — Một Request chỉ có một Work Order**

* **Given:** Maintenance Request đã có Work Order.
* **When:** Facility Manager cố gắng tạo thêm Work Order từ Request đó.
* **Then:** Hệ thống từ chối thao tác.

**AC-US-04-01-03 — Không xác định Asset**

* **Given:** Maintenance Request chỉ xác định khu vực và chưa xác định Asset.
* **When:** Facility Manager tạo Work Order.
* **Then:** Hệ thống không cho phép tạo Work Order cho đến khi Asset được xác định.

---

## US-04-02 — Phân công Work Order

**User Story**

> Là một Facility Manager, tôi muốn phân công Work Order cho Technician, để mỗi công việc có người chịu trách nhiệm.

**Requirement:** REQ-15
**Business Rule:** BR-07
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-04-02-01 — Phân công thành công**

* **Given:** Work Order tồn tại và Technician hợp lệ.
* **When:** Facility Manager phân công Work Order.
* **Then:** Hệ thống liên kết Work Order với Technician và cập nhật trạng thái `Assigned`.

**AC-US-04-02-02 — Technician không hợp lệ**

* **Given:** Người được chọn không có Role Technician.
* **When:** Facility Manager phân công Work Order.
* **Then:** Hệ thống không cho phép phân công.

---

## US-04-03 — Theo dõi Work Order

**User Story**

> Là một Facility Manager, tôi muốn theo dõi trạng thái Work Order, để giám sát tiến độ bảo trì.

**Requirement:** REQ-16
**Business Rule:** BR-17
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-04-03-01 — Xem Work Order**

* **Given:** Facility Manager có Work Order trong hệ thống.
* **When:** Facility Manager truy cập danh sách Work Order.
* **Then:** Hệ thống hiển thị Work Order và trạng thái hiện tại.

**AC-US-04-03-02 — Trạng thái Work Order**

* **Given:** Work Order đang trong lifecycle.
* **When:** Trạng thái thay đổi.
* **Then:** Hệ thống sử dụng các trạng thái `Assigned`, `In Progress`, `Completed`, `Cancelled`.

**AC-US-04-03-03 — Lưu timestamp**

* **Given:** Trạng thái Work Order thay đổi.
* **When:** Hệ thống cập nhật trạng thái.
* **Then:** Hệ thống lưu timestamp của thay đổi.

---

## US-04-04 — Xem Work Order được phân công

**User Story**

> Là một Technician, tôi muốn xem Work Order được phân công và thông tin Asset liên quan, để biết công việc cần thực hiện.

**Requirement:** REQ-17, REQ-18
**Business Rules:** BR-07, BR-08
**NFR:** NFR-01, NFR-07

### Acceptance Criteria

**AC-US-04-04-01 — Xem Work Order được giao**

* **Given:** Technician có Work Order được phân công.
* **When:** Technician truy cập danh sách Work Order.
* **Then:** Hệ thống hiển thị các Work Order được phân công cho Technician đó.

**AC-US-04-04-02 — Xem Asset liên quan**

* **Given:** Work Order được liên kết với Asset.
* **When:** Technician xem Work Order.
* **Then:** Hệ thống hiển thị thông tin Asset liên quan.

**AC-US-04-04-03 — Không xem Work Order của Technician khác**

* **Given:** Work Order được phân công cho Technician khác.
* **When:** Technician cố gắng truy cập Work Order.
* **Then:** Hệ thống từ chối quyền cập nhật và truy cập theo quyền được cấu hình.

---

## US-04-05 — Cập nhật hoặc từ chối Work Order

**User Story**

> Là một Technician, tôi muốn cập nhật hoặc từ chối Work Order được phân công kèm lý do, để Work Order phản ánh chính xác khả năng thực hiện công việc của tôi.

**Requirement:** REQ-20
**Business Rules:** BR-07, BR-17
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-04-05-01 — Cập nhật Work Order**

* **Given:** Work Order được phân công cho Technician.
* **When:** Technician cập nhật thông tin hoặc trạng thái hợp lệ.
* **Then:** Hệ thống lưu thay đổi vào Work Order.

**AC-US-04-05-02 — Từ chối Work Order**

* **Given:** Work Order được phân công cho Technician.
* **When:** Technician chọn từ chối và nhập lý do.
* **Then:** Hệ thống ghi nhận hành động từ chối và lý do.

**AC-US-04-05-03 — Từ chối không có lý do**

* **Given:** Technician chọn từ chối Work Order.
* **When:** Technician không nhập lý do.
* **Then:** Hệ thống không cho phép hoàn tất thao tác từ chối.

**AC-US-04-05-04 — Không phải Work Order được phân công**

* **Given:** Work Order không được phân công cho Technician hiện tại.
* **When:** Technician cố gắng cập nhật hoặc từ chối Work Order.
* **Then:** Hệ thống từ chối thao tác.

> **Open Question:** Cần xác nhận liệu `Rejected` có trở thành một Work Order Status chính thức hay chỉ là một action kèm lý do. Hiện tại AC không thêm `Rejected` vào danh sách status chính thức của BR-17.

---

## US-04-06 — Hoàn thành Work Order và lưu Maintenance History

**User Story**

> Là một Technician, tôi muốn ghi nhận kết quả bảo trì và hoàn thành Work Order, để kết quả được lưu vào lịch sử bảo trì của Asset.

**Requirement:** REQ-21, REQ-22, REQ-23
**Business Rules:** BR-09, BR-18
**NFR:** NFR-03, NFR-04

### Acceptance Criteria

**AC-US-04-06-01 — Ghi nhận kết quả**

* **Given:** Technician đang thực hiện Work Order được phân công.
* **When:** Technician nhập kết quả kiểm tra, sửa chữa hoặc bảo trì.
* **Then:** Hệ thống lưu kết quả vào Work Order.

**AC-US-04-06-02 — Hoàn thành Work Order**

* **Given:** Technician đã ghi nhận đầy đủ kết quả bảo trì.
* **When:** Technician chọn hoàn thành Work Order.
* **Then:** Work Order chuyển sang trạng thái `Completed`.

**AC-US-04-06-03 — Thiếu kết quả**

* **Given:** Technician chưa ghi nhận kết quả bảo trì.
* **When:** Technician cố gắng hoàn thành Work Order.
* **Then:** Hệ thống không cho phép hoàn thành.

**AC-US-04-06-04 — Lưu Maintenance History**

* **Given:** Work Order đã được hoàn thành.
* **When:** Hệ thống xác nhận Work Order `Completed`.
* **Then:** Kết quả bảo trì được lưu vào Maintenance History của Asset.

---

# 6. EPIC-05 — IoT Monitoring

## US-05-01 — Mapping Asset với IoT Device

**User Story**

> Là một Admin, tôi muốn mapping Asset với IoT Device/Sensor, để dữ liệu IoT được liên kết với đúng Asset.

**Requirement:** REQ-26
**Business Rule:** BR-13
**NFR:** NFR-06

### Acceptance Criteria

**AC-US-05-01-01 — Mapping thành công**

* **Given:** Asset và IoT Device/Sensor tồn tại.
* **When:** Admin thực hiện mapping.
* **Then:** Hệ thống lưu quan hệ giữa Asset và IoT Device/Sensor.

**AC-US-05-01-02 — Asset đã có Device**

* **Given:** Asset đã được mapping với một IoT Device/Sensor.
* **When:** Admin cố gắng mapping thêm một Device khác.
* **Then:** Hệ thống từ chối thao tác theo giới hạn MVP.

**AC-US-05-01-03 — Asset không tồn tại**

* **Given:** Asset không tồn tại.
* **When:** Admin thực hiện mapping.
* **Then:** Hệ thống không cho phép mapping.

---

## US-05-02 — Cập nhật IoT Mapping

**User Story**

> Là một Admin, tôi muốn cập nhật IoT Mapping, để duy trì cấu hình IoT khi có thay đổi.

**Requirement:** REQ-27
**Business Rule:** BR-13
**NFR:** NFR-06

### Acceptance Criteria

**AC-US-05-02-01 — Cập nhật Mapping**

* **Given:** Asset đã có IoT Mapping.
* **When:** Admin thay đổi Device/Sensor được mapping.
* **Then:** Hệ thống cập nhật mapping mới.

**AC-US-05-02-02 — Mapping không hợp lệ**

* **Given:** Device/Sensor không hợp lệ hoặc không tồn tại.
* **When:** Admin lưu mapping.
* **Then:** Hệ thống từ chối thay đổi.

**AC-US-05-02-03 — Duy trì một Device cho Asset**

* **Given:** Asset thuộc phạm vi MVP.
* **When:** Admin cập nhật mapping.
* **Then:** Asset chỉ có tối đa một IoT Device/Sensor được mapping.

---

## US-05-03 — Giám sát IoT Data

**User Story**

> Là một Facility Manager, tôi muốn xem IoT Data của Asset, để giám sát tình trạng hiện tại của Asset.

**Requirement:** REQ-09, REQ-28
**Business Rules:** BR-12, BR-13
**NFR:** NFR-04, NFR-05, NFR-06

### Acceptance Criteria

**AC-US-05-03-01 — Nhận IoT Data**

* **Given:** Asset đã được mapping với IoT Device/Sensor.
* **When:** IoT Device gửi dữ liệu.
* **Then:** Hệ thống nhận và lưu IoT Data.

**AC-US-05-03-02 — Chu kỳ thu thập mặc định**

* **Given:** Hệ thống đang sử dụng cấu hình mặc định của MVP.
* **When:** Hệ thống thu thập IoT Data.
* **Then:** Dữ liệu được thu thập/lưu trữ theo chu kỳ 5 phút/lần.

**AC-US-05-03-03 — Xem IoT Data**

* **Given:** Asset có IoT Data.
* **When:** Facility Manager xem Asset.
* **Then:** Hệ thống hiển thị dữ liệu IoT liên quan đến Asset.

**AC-US-05-03-04 — Asset chưa mapping**

* **Given:** Asset chưa được mapping với IoT Device/Sensor.
* **When:** Facility Manager xem IoT Data.
* **Then:** Hệ thống thông báo Asset chưa có dữ liệu IoT được liên kết.

---

## US-05-04 — Xử lý IoT Alert

**User Story**

> Là một Facility Manager, tôi muốn nhận IoT Alert khi phát hiện điều kiện bất thường, để có thể phản ứng với các vấn đề bảo trì tiềm ẩn.

**Requirement:** REQ-10, REQ-30
**Business Rule:** BR-12
**NFR:** NFR-04

### Acceptance Criteria

**AC-US-05-04-01 — Tạo IoT Alert**

* **Given:** IoT Data của Asset đáp ứng điều kiện threshold/bất thường đã cấu hình.
* **When:** Hệ thống nhận dữ liệu IoT.
* **Then:** Hệ thống tạo IoT Alert cho Asset tương ứng.

**AC-US-05-04-02 — Không tạo Alert khi bình thường**

* **Given:** IoT Data nằm trong điều kiện hoạt động bình thường.
* **When:** Hệ thống nhận dữ liệu.
* **Then:** Hệ thống không tạo IoT Alert bất thường.

**AC-US-05-04-03 — Alert nghiêm trọng**

* **Given:** IoT Alert được xác định là nghiêm trọng.
* **When:** Alert được tạo.
* **Then:** Hệ thống gửi Notification cho Facility Manager.

---

# 7. EPIC-06 — AI Predictive Maintenance

## US-06-01 — Dự đoán nhu cầu bảo trì bằng AI

**User Story**

> Là một Facility Manager, tôi muốn nhận AI Prediction về khả năng Asset cần bảo trì trong 7 ngày tiếp theo, để ưu tiên bảo trì phòng ngừa.

**Requirement:** REQ-11, REQ-29
**Business Rules:** BR-10, BR-11, BR-15
**NFR:** NFR-08

### Acceptance Criteria

**AC-US-06-01-01 — Tạo AI Prediction**

* **Given:** Hệ thống có IoT Data và Maintenance History phù hợp của Asset.
* **When:** Hệ thống thực hiện AI Prediction.
* **Then:** Hệ thống tạo Prediction về khả năng Asset cần bảo trì trong 7 ngày tiếp theo.

**AC-US-06-01-02 — Liên kết Prediction với Asset**

* **Given:** AI Prediction được tạo.
* **When:** Hệ thống lưu Prediction.
* **Then:** Prediction được liên kết với Asset và thời điểm Prediction.

**AC-US-06-01-03 — Thiếu dữ liệu**

* **Given:** Asset không có đủ dữ liệu phù hợp cho Prediction.
* **When:** Hệ thống thực hiện Prediction.
* **Then:** Hệ thống không tạo kết quả không đáng tin cậy và hiển thị trạng thái phù hợp.

**AC-US-06-01-04 — AI chỉ hỗ trợ quyết định**

* **Given:** AI Prediction cho thấy Asset có khả năng cần bảo trì.
* **When:** Facility Manager xem Prediction.
* **Then:** Hệ thống chỉ cung cấp Prediction và không tự động quyết định hoặc thực hiện bảo trì.

---

## US-06-02 — Xem Maintenance Risk

**User Story**

> Là một Facility Manager, tôi muốn xem Maintenance Risk của Asset ở mức Low, Medium hoặc High, để hiểu mức độ rủi ro được dự đoán.

**Requirement:** REQ-12
**Business Rules:** BR-10, BR-15
**NFR:** NFR-08

### Acceptance Criteria

**AC-US-06-02-01 — Hiển thị Risk**

* **Given:** Asset có AI Prediction hợp lệ.
* **When:** Facility Manager xem kết quả Prediction.
* **Then:** Hệ thống hiển thị Maintenance Risk.

**AC-US-06-02-02 — Ba mức Risk**

* **Given:** AI Prediction được tạo.
* **When:** Hệ thống xác định Maintenance Risk.
* **Then:** Risk được biểu diễn bằng một trong ba mức `Low`, `Medium`, `High`.

**AC-US-06-02-03 — High Risk**

* **Given:** Asset có Maintenance Risk = `High`.
* **When:** Facility Manager xem danh sách Asset.
* **Then:** Hệ thống cho phép Facility Manager nhận diện Asset có mức rủi ro High để ưu tiên xem xét.

---

## US-06-03 — Xem AI Prediction và IoT Alert

**User Story**

> Là một Technician, tôi muốn xem IoT Alert và AI Prediction liên quan đến Asset, để hiểu các vấn đề tiềm ẩn trước khi thực hiện bảo trì.

**Requirement:** REQ-19
**Business Rules:** BR-10, BR-11
**NFR:** NFR-08

### Acceptance Criteria

**AC-US-06-03-01 — Xem IoT Alert**

* **Given:** Asset của Work Order có IoT Alert.
* **When:** Technician xem Work Order/Asset.
* **Then:** Hệ thống hiển thị IoT Alert liên quan.

**AC-US-06-03-02 — Xem AI Prediction**

* **Given:** Asset của Work Order có AI Prediction.
* **When:** Technician xem Work Order/Asset.
* **Then:** Hệ thống hiển thị AI Prediction liên quan.

**AC-US-06-03-03 — Không có dữ liệu**

* **Given:** Asset không có IoT Alert hoặc AI Prediction.
* **When:** Technician xem thông tin Asset.
* **Then:** Hệ thống thông báo rằng không có dữ liệu tương ứng.

**AC-US-06-03-04 — AI chỉ mang tính hỗ trợ**

* **Given:** Technician xem AI Prediction.
* **When:** Technician sử dụng thông tin Prediction để thực hiện công việc.
* **Then:** Hệ thống chỉ cung cấp thông tin hỗ trợ và không tự động đưa ra quyết định bảo trì.

---

# 8. Acceptance Criteria Traceability Summary

| Epic                                       | User Stories |  Số AC |
| ------------------------------------------ | -----------: | -----: |
| EPIC-01 — Authentication & Access Control  |            4 |     12 |
| EPIC-02 — Asset Management                 |            4 |     13 |
| EPIC-03 — Maintenance Request              |            4 |     13 |
| EPIC-04 — Work Order & Maintenance History |            6 |     19 |
| EPIC-05 — IoT Monitoring                   |            4 |     14 |
| EPIC-06 — AI Predictive Maintenance        |            3 |     11 |
| **Tổng cộng**                              |       **25** | **82** |

---

# 9. Coverage by Requirement

| Requirement | User Story         | AC Coverage         |
| ----------- | ------------------ | ------------------- |
| REQ-01      | US-01-01           | AC-US-01-01-01 → 03 |
| REQ-02      | US-01-02           | AC-US-01-02-01 → 02 |
| REQ-03      | US-02-04           | AC-US-02-04-01 → 02 |
| REQ-04      | US-03-01           | AC-US-03-01-01 → 04 |
| REQ-05      | US-03-02           | AC-US-03-02-01 → 02 |
| REQ-06      | US-02-01           | AC-US-02-01-01 → 04 |
| REQ-07      | US-02-02           | AC-US-02-02-01 → 03 |
| REQ-08      | US-02-03           | AC-US-02-03-01 → 03 |
| REQ-09      | US-05-03           | AC-US-05-03-01 → 04 |
| REQ-10      | US-05-04           | AC-US-05-04-01 → 03 |
| REQ-11      | US-06-01           | AC-US-06-01-01 → 04 |
| REQ-12      | US-06-02           | AC-US-06-02-01 → 03 |
| REQ-13      | US-03-03, US-03-04 | AC tương ứng        |
| REQ-14      | US-04-01           | AC-US-04-01-01 → 03 |
| REQ-15      | US-04-02           | AC-US-04-02-01 → 02 |
| REQ-16      | US-04-03           | AC-US-04-03-01 → 03 |
| REQ-17      | US-04-04           | AC-US-04-04-01 → 03 |
| REQ-18      | US-04-04           | AC-US-04-04-02      |
| REQ-19      | US-06-03           | AC-US-06-03-01 → 04 |
| REQ-20      | US-04-05           | AC-US-04-05-01 → 04 |
| REQ-21      | US-04-06           | AC-US-04-06-01      |
| REQ-22      | US-04-06           | AC-US-04-06-02 → 03 |
| REQ-23      | US-04-06           | AC-US-04-06-04      |
| REQ-24      | US-01-03           | AC-US-01-03-01 → 03 |
| REQ-25      | US-01-04           | AC-US-01-04-01 → 03 |
| REQ-26      | US-05-01           | AC-US-05-01-01 → 03 |
| REQ-27      | US-05-02           | AC-US-05-02-01 → 03 |
| REQ-28      | US-05-03           | AC-US-05-03-02      |
| REQ-29      | US-06-01           | AC-US-06-01-01 → 03 |
| REQ-30      | US-05-04           | AC-US-05-04-03      |

---

# 10. Coverage by Business Rule

| Business Rule | Covered by                             |
| ------------- | -------------------------------------- |
| BR-01         | US-02-01, US-02-02                     |
| BR-02         | US-02-01, US-02-02                     |
| BR-03         | US-02-01                               |
| BR-04         | US-01-02, US-02-01, US-02-02, US-02-04 |
| BR-05         | US-03-01, US-03-03, US-04-01           |
| BR-06         | US-04-01                               |
| BR-07         | US-04-02, US-04-04, US-04-05           |
| BR-08         | US-04-01, US-04-04                     |
| BR-09         | US-04-06                               |
| BR-10         | US-06-01, US-06-02, US-06-03           |
| BR-11         | US-06-01, US-06-03                     |
| BR-12         | US-05-03, US-05-04                     |
| BR-13         | US-05-01, US-05-02, US-05-03           |
| BR-14         | US-03-02, US-03-03                     |
| BR-15         | US-06-01, US-06-02                     |
| BR-16         | US-02-03                               |
| BR-17         | US-04-03, US-04-05, US-04-06           |
| BR-18         | US-03-04, US-04-06                     |

---

# 11. Important Open Question

### OQ-01 — Work Order Rejection Status

**Current situation:**

* REQ-20 cho phép Technician từ chối Work Order kèm lý do.
* BR-07 cũng cho phép hành động từ chối.
* Tuy nhiên, BR-17 hiện chỉ định nghĩa Work Order Status:

  * `Assigned`
  * `In Progress`
  * `Completed`
  * `Cancelled`

**Question:**

> Khi Technician từ chối Work Order, `Rejected` có phải là một Work Order Status chính thức hay chỉ là một action được ghi nhận kèm lý do?

**Current handling:**

Trong Acceptance Criteria hiện tại, `Rejected` **chưa được thêm vào danh sách Status chính thức** để tránh tự ý thay đổi BR-17 khi chưa có quyết định của stakeholder.

---

# 12. Definition of Accepted

Một User Story được xem là **Accepted** khi:

* Tất cả Acceptance Criteria của User Story đều đạt.
* Không còn lỗi Critical/Blocker liên quan đến User Story.
* Các Requirement và Business Rule được mapping đầy đủ.
* Các trường hợp lỗi quan trọng đã được xử lý.
* Không vi phạm quyền truy cập theo Role.
* Dữ liệu nghiệp vụ được lưu nhất quán.
* Các Open Question có ảnh hưởng trực tiếp đến Story đã được giải quyết hoặc được stakeholder chấp thuận cách xử lý tạm thời.

