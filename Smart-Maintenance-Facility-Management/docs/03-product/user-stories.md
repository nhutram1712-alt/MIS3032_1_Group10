# User Stories

**Dự án:** Smart Maintenance & Facility Management
**Đơn vị:** Trường Đại học Kinh tế – Đại học Đà Nẵng
**Phiên bản:** 1.0
**Trạng thái:** Draft – Chờ xây dựng Acceptance Criteria
**Vai trò:** Business Analyst

---

# 1. Tổng quan

Tài liệu này phân rã các Functional Requirements đã được xác nhận thành các **Epic** và **User Story** theo giá trị đối với người dùng và nghiệp vụ.

Các User Story được xây dựng dựa trên **Requirements v1.0 – Baselined** và được mapping với các **Business Rules** liên quan.

Tài liệu này **không bổ sung phạm vi sản phẩm mới** ngoài các Requirements và Business Rules đã được xác nhận.

## 1.1. Format User Story

Mỗi User Story sử dụng cấu trúc:

> **As a [Role], I want [Action], so that [Benefit].**

Trong tài liệu tiếng Việt:

> **Với vai trò là [Role], tôi muốn [Action], để [Benefit].**

## 1.2. Nguyên tắc Traceability

Mỗi User Story được liên kết với:

* Functional Requirement (REQ)
* Business Rule (BR)
* Non-Functional Requirement (NFR), khi có liên quan
* Design, khi Design đã được xác định

---

# 2. Tổng quan các Epic

| Epic ID  | Epic                             | Mục tiêu                                                                 | Số User Story |
| -------- | -------------------------------- | ------------------------------------------------------------------------ | ------------: |
| EPIC-01  | Authentication & Access Control  | Đăng nhập và kiểm soát quyền truy cập theo Role.                         |             4 |
| EPIC-02  | Asset Management                 | Quản lý thông tin và trạng thái Asset.                                   |             4 |
| EPIC-03  | Maintenance Request              | Báo cáo, theo dõi, xử lý và đóng Maintenance Request.                    |             4 |
| EPIC-04  | Work Order & Maintenance History | Tạo, phân công, thực hiện, hoàn thành Work Order và lưu lịch sử bảo trì. |             6 |
| EPIC-05  | IoT Monitoring                   | Kết nối Asset với IoT Device/Sensor, thu thập dữ liệu và cảnh báo.       |             4 |
| EPIC-06  | AI Predictive Maintenance        | Dự đoán rủi ro bảo trì và hỗ trợ quyết định bảo trì chủ động.            |             3 |
| **Tổng** |                                  |                                                                          |        **25** |

---

# 3. EPIC-01 — Authentication & Access Control

**Mục tiêu:** Cho phép người dùng truy cập hệ thống và đảm bảo người dùng chỉ được thực hiện các chức năng phù hợp với Role.

## US-01-01 — Đăng nhập hệ thống

**User Story**

> Với vai trò là **User**, tôi muốn đăng nhập bằng tài khoản của hệ thống, để có thể truy cập và sử dụng hệ thống.

**Priority:** Must

**Requirement Mapping:**

* REQ-01

**Business Rule Mapping:**

* Không có

**NFR Mapping:**

* NFR-01
* NFR-02

**Design:** Chưa thiết kế

---

## US-01-02 — Xem Asset được phép truy cập

**User Story**

> Với vai trò là **Requester**, tôi muốn chỉ xem được các Asset thuộc phòng/khu vực mà tôi được phép sử dụng, để chỉ tiếp cận thông tin Asset liên quan.

**Priority:** Should

**Requirement Mapping:**

* REQ-02

**Business Rule Mapping:**

* BR-04

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Chưa thiết kế

---

## US-01-03 — Quản lý tài khoản người dùng

**User Story**

> Với vai trò là **Admin**, tôi muốn quản lý tài khoản người dùng, để duy trì quyền truy cập vào hệ thống.

**Priority:** Should

**Requirement Mapping:**

* REQ-24

**Business Rule Mapping:**

* Không có

**NFR Mapping:**

* NFR-01
* NFR-02

**Design:** Chưa thiết kế

---

## US-01-04 — Quản lý quyền theo Role

**User Story**

> Với vai trò là **Admin**, tôi muốn quản lý quyền truy cập theo Role, để đảm bảo mỗi người dùng chỉ được truy cập các chức năng được phép.

**Priority:** Should

**Requirement Mapping:**

* REQ-25

**Business Rule Mapping:**

* Không có

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Chưa thiết kế

---

# 4. EPIC-02 — Asset Management

**Mục tiêu:** Cho phép Facility Manager quản lý thông tin và trạng thái của Asset.

## US-02-01 — Thêm Asset

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn thêm Asset với các thông tin cơ bản, để Asset có thể được quản lý trên hệ thống.

**Priority:** Must

**Requirement Mapping:**

* REQ-06

**Business Rule Mapping:**

* BR-01
* BR-02
* BR-03
* BR-04

**NFR Mapping:**

* NFR-03
* NFR-04
* NFR-06

**Design:** Chưa thiết kế

---

## US-02-02 — Cập nhật thông tin Asset

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn cập nhật thông tin Asset, để đảm bảo dữ liệu Asset luôn chính xác.

**Priority:** Must

**Requirement Mapping:**

* REQ-07

**Business Rule Mapping:**

* BR-01
* BR-02
* BR-04

**NFR Mapping:**

* NFR-03
* NFR-04
* NFR-06

**Design:** Chưa thiết kế

---

## US-02-03 — Quản lý trạng thái Asset

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn xem thông tin Asset và thay đổi thủ công Asset Status, để duy trì trạng thái hiện tại của Asset.

**Priority:** Must

**Requirement Mapping:**

* REQ-08

**Business Rule Mapping:**

* BR-16

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-02-04 — Xem trạng thái Asset

**User Story**

> Với vai trò là **Requester**, tôi muốn xem trạng thái hiện tại của Asset mà tôi được phép truy cập, để biết Asset đang hoạt động hay gặp vấn đề.

**Priority:** Should

**Requirement Mapping:**

* REQ-03

**Business Rule Mapping:**

* BR-04

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Chưa thiết kế

---

# 5. EPIC-03 — Maintenance Request

**Mục tiêu:** Cho phép Requester báo cáo sự cố và Facility Manager tiếp nhận, xử lý Maintenance Request.

## US-03-01 — Tạo Maintenance Request

**User Story**

> Với vai trò là **Requester**, tôi muốn tạo Maintenance Request cho một Asset hoặc khu vực bị ảnh hưởng, để báo cáo vấn đề cần được bảo trì.

**Priority:** Must

**Requirement Mapping:**

* REQ-04

**Business Rule Mapping:**

* BR-05

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-03-02 — Theo dõi Maintenance Request

**User Story**

> Với vai trò là **Requester**, tôi muốn theo dõi trạng thái của Maintenance Request, để biết tiến độ xử lý yêu cầu của mình.

**Priority:** Must

**Requirement Mapping:**

* REQ-05

**Business Rule Mapping:**

* BR-14

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-03-03 — Tiếp nhận và xử lý Maintenance Request

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn tiếp nhận và xử lý Maintenance Request, để các vấn đề được báo cáo có thể được giải quyết.

**Priority:** Must

**Requirement Mapping:**

* REQ-13

**Business Rule Mapping:**

* BR-05
* BR-14

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-03-04 — Xác nhận và đóng Maintenance Request

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn xác nhận kết quả bảo trì trước khi đóng Maintenance Request, để đảm bảo yêu cầu chỉ được đóng sau khi kết quả đã được kiểm tra.

**Priority:** Must

**Requirement Mapping:**

* REQ-13

**Business Rule Mapping:**

* BR-18

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

# 6. EPIC-04 — Work Order & Maintenance History

**Mục tiêu:** Quản lý quá trình thực hiện công việc bảo trì từ khi tạo Work Order đến khi hoàn thành và lưu lịch sử bảo trì.

## US-04-01 — Tạo Work Order

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn tạo Work Order từ Maintenance Request hoặc nhu cầu bảo trì đã được xác định, để công việc bảo trì được ghi nhận và theo dõi chính thức.

**Priority:** Must

**Requirement Mapping:**

* REQ-14

**Business Rule Mapping:**

* BR-05
* BR-06
* BR-08

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-04-02 — Phân công Work Order

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn phân công Work Order cho Technician, để mỗi công việc bảo trì có người chịu trách nhiệm thực hiện.

**Priority:** Must

**Requirement Mapping:**

* REQ-15

**Business Rule Mapping:**

* BR-07

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-04-03 — Theo dõi Work Order

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn theo dõi trạng thái Work Order, để giám sát tiến độ thực hiện công việc bảo trì.

**Priority:** Must

**Requirement Mapping:**

* REQ-16

**Business Rule Mapping:**

* BR-17

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

## US-04-04 — Xem Work Order được phân công

**User Story**

> Với vai trò là **Technician**, tôi muốn xem các Work Order được phân công cho mình và thông tin Asset liên quan, để biết công việc cần thực hiện và Asset cần bảo trì.

**Priority:** Must

**Requirement Mapping:**

* REQ-17
* REQ-18

**Business Rule Mapping:**

* BR-07
* BR-08

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Chưa thiết kế

---

## US-04-05 — Cập nhật hoặc từ chối Work Order

**User Story**

> Với vai trò là **Technician**, tôi muốn cập nhật hoặc từ chối Work Order được phân công kèm lý do, để phản ánh chính xác khả năng thực hiện công việc của mình.

**Priority:** Must

**Requirement Mapping:**

* REQ-20

**Business Rule Mapping:**

* BR-07
* BR-17

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

### Open Question

Requirements hiện tại cho phép Technician **từ chối Work Order** nhưng danh sách Work Order Status chỉ bao gồm:

* Assigned
* In Progress
* Completed
* Cancelled

Cần xác nhận:

1. `Reject` chỉ là một action của Technician và Work Order vẫn giữ trạng thái `Assigned`; hoặc
2. `Rejected` sẽ được bổ sung thành một Work Order Status chính thức.

**Trạng thái:** Open

---

## US-04-06 — Hoàn thành Work Order và lưu lịch sử bảo trì

**User Story**

> Với vai trò là **Technician**, tôi muốn ghi nhận kết quả kiểm tra, sửa chữa hoặc bảo trì và hoàn thành Work Order, để kết quả bảo trì được lưu lại trong lịch sử của Asset.

**Priority:** Must

**Requirement Mapping:**

* REQ-21
* REQ-22
* REQ-23

**Business Rule Mapping:**

* BR-09
* BR-18

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Chưa thiết kế

---

# 7. EPIC-05 — IoT Monitoring

**Mục tiêu:** Kết nối Asset với IoT Device/Sensor, thu thập dữ liệu, giám sát tình trạng và tạo cảnh báo khi phát hiện bất thường.

## US-05-01 — Mapping Asset với IoT Device/Sensor

**User Story**

> Với vai trò là **Admin**, tôi muốn mapping Asset với một IoT Device/Sensor, để dữ liệu IoT được liên kết với đúng Asset.

**Priority:** Must

**Requirement Mapping:**

* REQ-26

**Business Rule Mapping:**

* BR-13

**NFR Mapping:**

* NFR-06

**Design:** Chưa thiết kế

---

## US-05-02 — Cập nhật IoT Mapping

**User Story**

> Với vai trò là **Admin**, tôi muốn cập nhật IoT Mapping, để duy trì thông tin kết nối giữa Asset và IoT Device/Sensor.

**Priority:** Must

**Requirement Mapping:**

* REQ-27

**Business Rule Mapping:**

* BR-13

**NFR Mapping:**

* NFR-06

**Design:** Chưa thiết kế

---

## US-05-03 — Giám sát IoT Data

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn xem IoT Data của Asset, để theo dõi tình trạng hiện tại của Asset.

**Priority:** Must

**Requirement Mapping:**

* REQ-09
* REQ-28

**Business Rule Mapping:**

* BR-12
* BR-13

**NFR Mapping:**

* NFR-04
* NFR-05
* NFR-06

**Design:** Chưa thiết kế

---

## US-05-04 — Tiếp nhận và xử lý IoT Alert

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn nhận IoT Alert khi phát hiện điều kiện bất thường, để có thể kịp thời phản ứng với các vấn đề bảo trì tiềm ẩn.

**Priority:** Must

**Requirement Mapping:**

* REQ-10
* REQ-30

**Business Rule Mapping:**

* BR-12

**NFR Mapping:**

* NFR-04

**Design:** Chưa thiết kế

---

# 8. EPIC-06 — AI Predictive Maintenance

**Mục tiêu:** Sử dụng IoT Data và Maintenance History để dự đoán khả năng cần bảo trì và hỗ trợ Facility Manager ưu tiên công việc.

## US-06-01 — Dự đoán nhu cầu bảo trì bằng AI

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn nhận dự đoán bằng AI về khả năng Asset cần bảo trì trong 7 ngày tiếp theo, để có thể chủ động ưu tiên công việc bảo trì.

**Priority:** Must

**Requirement Mapping:**

* REQ-11
* REQ-29

**Business Rule Mapping:**

* BR-10
* BR-11
* BR-15

**NFR Mapping:**

* NFR-08

**Design:** Chưa thiết kế

---

## US-06-02 — Xem Maintenance Risk

**User Story**

> Với vai trò là **Facility Manager**, tôi muốn xem Maintenance Risk của Asset ở ba mức Low, Medium hoặc High, để hiểu mức độ rủi ro bảo trì được dự đoán.

**Priority:** Must

**Requirement Mapping:**

* REQ-12

**Business Rule Mapping:**

* BR-10
* BR-15

**NFR Mapping:**

* NFR-08

**Design:** Chưa thiết kế

---

## US-06-03 — Xem AI Prediction và IoT Alert

**User Story**

> Với vai trò là **Technician**, tôi muốn xem IoT Alert và AI Prediction liên quan đến Asset được giao, để hiểu các vấn đề tiềm ẩn trước khi thực hiện bảo trì.

**Priority:** Must

**Requirement Mapping:**

* REQ-19

**Business Rule Mapping:**

* BR-10
* BR-11

**NFR Mapping:**

* NFR-08

**Design:** Chưa thiết kế

---

# 9. Requirement Traceability Summary

## 9.1. Functional Requirements → User Stories

| Requirement | User Story         |
| ----------- | ------------------ |
| REQ-01      | US-01-01           |
| REQ-02      | US-01-02           |
| REQ-03      | US-02-04           |
| REQ-04      | US-03-01           |
| REQ-05      | US-03-02           |
| REQ-06      | US-02-01           |
| REQ-07      | US-02-02           |
| REQ-08      | US-02-03           |
| REQ-09      | US-05-03           |
| REQ-10      | US-05-04           |
| REQ-11      | US-06-01           |
| REQ-12      | US-06-02           |
| REQ-13      | US-03-03, US-03-04 |
| REQ-14      | US-04-01           |
| REQ-15      | US-04-02           |
| REQ-16      | US-04-03           |
| REQ-17      | US-04-04           |
| REQ-18      | US-04-04           |
| REQ-19      | US-06-03           |
| REQ-20      | US-04-05           |
| REQ-21      | US-04-06           |
| REQ-22      | US-04-06           |
| REQ-23      | US-04-06           |
| REQ-24      | US-01-03           |
| REQ-25      | US-01-04           |
| REQ-26      | US-05-01           |
| REQ-27      | US-05-02           |
| REQ-28      | US-05-03           |
| REQ-29      | US-06-01           |
| REQ-30      | US-05-04           |

**Coverage: 30/30 Functional Requirements được mapping.**

---

# 10. Business Rule Traceability

| Business Rule | User Story                             |
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
| BR-17         | US-04-03, US-04-05                     |
| BR-18         | US-03-04, US-04-06                     |

**Coverage: 18/18 Business Rules được mapping.**

---

# 11. Non-Functional Requirement Mapping

NFR được xem là các yêu cầu xuyên suốt hệ thống, không nhất thiết phải chuyển thành User Story độc lập.

| NFR    | Epic / User Story liên quan        |
| ------ | ---------------------------------- |
| NFR-01 | EPIC-01 và các Story có phân quyền |
| NFR-02 | US-01-01, US-01-03                 |
| NFR-03 | EPIC-03, EPIC-04                   |
| NFR-04 | EPIC-02, EPIC-03, EPIC-04, EPIC-05 |
| NFR-05 | US-05-03                           |
| NFR-06 | EPIC-02, EPIC-05                   |
| NFR-07 | EPIC-01 và các giao diện theo Role |
| NFR-08 | EPIC-06                            |

---

# 12. Out of Scope

Các nội dung sau **không thuộc phạm vi MVP**:

* Quản lý tài chính (Finance)
* Quản lý mua sắm (Procurement)
* Quản lý kho (Inventory)
* Quản lý phụ tùng thay thế (Spare Parts)
* Quản lý nhà cung cấp (Supplier/Vendor)
* Quản lý nhân sự (HR)
* Quản lý đào tạo/học vụ (Academic Management)
* Quản lý sinh viên (Student Management)
* Quản trị đại học tổng thể (General University Administration)
* AI tự động quyết định hoặc tự động thực hiện bảo trì
* Tích hợp SSO

---

# 13. Definition of Ready

Một User Story được xem là **Ready for Sprint** khi đáp ứng các điều kiện sau:

* [ ] User Story sử dụng đúng format `As a / I want / so that`.
* [ ] Role của người dùng được xác định rõ.
* [ ] Giá trị đối với người dùng/nghiệp vụ được xác định rõ.
* [ ] Có ít nhất một Requirement ID được mapping.
* [ ] Business Rule liên quan đã được mapping.
* [ ] Acceptance Criteria được viết theo format Given/When/Then.
* [ ] Happy path đã được bao phủ.
* [ ] Các trường hợp validation/error quan trọng đã được bao phủ.
* [ ] Các dependency liên quan đã được xác định.
* [ ] Design đã có hoặc được xác định là chưa cần thiết.
* [ ] Không có Open Question chưa được giải quyết làm cản trở việc thực hiện Story.
* [ ] Story đủ nhỏ để có thể hoàn thành trong khoảng 1–3 ngày.

---

# 14. Open Questions

## OQ-01 — Work Order Rejection Status

REQ-20 và BR-07 cho phép Technician **từ chối Work Order kèm lý do**.

Tuy nhiên, BR-17 và REQ-16 hiện chỉ định nghĩa các Work Order Status:

* Assigned
* In Progress
* Completed
* Cancelled

Cần xác nhận một trong hai phương án:

**Phương án 1:** `Reject` chỉ là một action của Technician, Work Order vẫn giữ trạng thái `Assigned`.

**Phương án 2:** Bổ sung `Rejected` thành một Work Order Status chính thức.

**Trạng thái:** Open

---

# 15. Next Step

Sau khi User Stories được review và xác nhận, bước tiếp theo là xây dựng:

**Acceptance Criteria**

Mỗi User Story cần có khoảng **2–6 Acceptance Criteria** theo format:

```text
Given [điều kiện ban đầu]
When [hành động]
Then [kết quả mong đợi]
```

Acceptance Criteria cần bao phủ:

* Happy path
* Validation
* Error/Exception cases quan trọng
* Business Rules liên quan

Sau khi hoàn thành Acceptance Criteria, các Requirement → User Story → Acceptance Criteria sẽ được liên kết trong **Traceability Matrix**.
