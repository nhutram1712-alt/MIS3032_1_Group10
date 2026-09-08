# MVP Scope

### 1. Objective
MVP của **Smart Maintenance & Facility Management System** tập trung xây dựng một hệ thống quản lý bảo trì cơ sở vật chất cho Trường Đại học Kinh tế – Đại học Đà Nẵng, kết hợp:
* Quản lý Asset
* Maintenance Request
* Work Order
* IoT Monitoring
* AI Predictive Maintenance
* Maintenance History
* Quản lý người dùng và phân quyền

MVP ưu tiên giải quyết hai luồng chính:
1. **Reactive Maintenance:**
   `Requester` → `Maintenance Request` → `Facility Manager` → `Work Order` → `Technician` → `Maintenance History`
2. **Predictive Maintenance:**
   `IoT Data` → `Monitoring/Alert` → `AI Prediction` → `Facility Manager` → `Work Order` → `Technician` → `Maintenance History`

---

### 2. Must Have
Các chức năng bắt buộc phải có trong MVP để hệ thống có thể vận hành và đáp ứng yêu cầu AI + IoT.

| ID | Requirement | Rationale |
| --- | --- | --- |
| **REQ-01** | Login | Cần xác thực người dùng |
| **REQ-04** | Tạo Maintenance Request | Chức năng cốt lõi của reactive maintenance |
| **REQ-05** | Theo dõi Maintenance Request | Cho phép Requester theo dõi xử lý |
| **REQ-06** | Thêm Asset | Quản lý tài sản |
| **REQ-07** | Cập nhật Asset | Duy trì thông tin tài sản |
| **REQ-08** | Theo dõi Asset | Quản lý tình trạng tài sản |
| **REQ-09** | Xem IoT Data | Thành phần IoT bắt buộc |
| **REQ-10** | IoT Alert | Phát hiện tình trạng bất thường |
| **REQ-11** | AI Prediction | Thành phần AI bắt buộc |
| **REQ-12** | Maintenance Risk | Hiển thị kết quả đánh giá rủi ro |
| **REQ-13** | Xử lý Maintenance Request | Luồng xử lý sự cố |
| **REQ-14** | Tạo Work Order | Chuyển nhu cầu bảo trì thành công việc |
| **REQ-15** | Assign Work Order | Phân công Technician |
| **REQ-16** | Theo dõi Work Order | Quản lý tiến độ |
| **REQ-17** | Technician xem WO | Thực hiện công việc |
| **REQ-18** | Technician xem Asset | Cung cấp context khi bảo trì |
| **REQ-19** | Technician xem IoT/AI information | Hỗ trợ Technician xử lý |
| **REQ-20** | Technician cập nhật WO | Cập nhật tiến độ |
| **REQ-21** | Ghi nhận kết quả bảo trì | Lưu kết quả xử lý |
| **REQ-22** | Hoàn thành WO | Kết thúc công việc |
| **REQ-23** | Lưu Maintenance History | Tạo dữ liệu lịch sử cho quản lý và AI |
| **REQ-26** | IoT Mapping | Liên kết Asset với IoT |
| **REQ-27** | Cập nhật IoT Mapping | Quản lý mapping |
| **REQ-28** | Thu thập/lưu IoT Data | Nguồn dữ liệu cho monitoring và AI |
| **REQ-29** | Sử dụng IoT Data + Maintenance History cho AI | Nền tảng Predictive Maintenance |
| **NFR-01** | Role-based access | Đảm bảo đúng quyền |
| **NFR-03** | Data consistency | Đảm bảo dữ liệu nghiệp vụ nhất quán |
| **NFR-04** | Timestamp | Theo dõi lịch sử nghiệp vụ |
| **BR-01 → BR-15** | Business Rules | Đảm bảo hệ thống tuân thủ nghiệp vụ |

---

### 3. Should Have
Các chức năng quan trọng nhưng có thể hoàn thiện sau khi luồng MVP cốt lõi đã ổn định.

| ID | Requirement | Rationale |
| --- | --- | --- |
| **REQ-02** | Requester xem Asset theo phòng/khu vực | Hỗ trợ trải nghiệm Requester tốt hơn |
| **REQ-03** | Requester xem trạng thái Asset | Hỗ trợ tự kiểm tra trước khi tạo Request |
| **REQ-24** | Admin quản lý user account | Cần cho vận hành thực tế nhưng có thể đơn giản hóa trong prototype |
| **REQ-25** | Admin quản lý Role/Access | Có thể sử dụng cấu hình mặc định trong prototype |
| **NFR-02** | Bảo vệ authentication/user data | Cần triển khai đầy đủ khi đưa vào production |
| **NFR-05** | Configurable IoT collection interval | Có thể dùng interval cố định trong MVP prototype |
| **NFR-06** | Scalability | Quan trọng cho production nhưng chưa phải trọng tâm prototype |
| **NFR-07** | Role-specific UI | Cần đảm bảo trải nghiệm theo Role |
| **NFR-08** | AI Prediction gắn Asset + prediction time | Cần cho quản lý kết quả AI |

---

### 4. Could Have
Các chức năng có giá trị bổ sung nhưng không cần thiết để chứng minh MVP.

| Potential Feature | Related Area | Reason |
| --- | --- | --- |
| Dashboard nâng cao | Facility Management | Có thể phát triển sau khi core flow hoàn thiện |
| Báo cáo thống kê nâng cao | Maintenance | Không cần thiết cho core maintenance flow |
| Export dữ liệu | Reporting | Không ảnh hưởng đến core MVP |
| Notification nâng cao | Alert/AI | Có thể bổ sung sau |
| Mobile optimization nâng cao cho Technician | Technician | Có thể phát triển sau prototype |
| AI recommendation nâng cao | AI | MVP chỉ cần Prediction + Risk |
| Anomaly detection nâng cao | IoT/AI | Có thể phát triển sau rule-based Alert |

---

### 5. Out of Scope
Các nội dung không thuộc phạm vi MVP.

| Item | Reason |
| --- | --- |
| Finance Management | Không thuộc Facility Maintenance MVP |
| Procurement | Không thuộc phạm vi |
| Inventory Management | Không thuộc phạm vi MVP |
| Spare Parts Management | Không thuộc phạm vi MVP |
| Asset Purchasing | Không thuộc phạm vi |
| Supplier/Vendor Management | Không thuộc phạm vi |
| HR Management | Không thuộc phạm vi |
| Academic Management | Không thuộc phạm vi |
| Student Management | Không thuộc phạm vi |
| General University Administration | Không thuộc phạm vi |
| AI tự động quyết định bảo trì | AI chỉ hỗ trợ quyết định |
| IoT Hardware Manufacturing | Hệ thống chỉ tích hợp/nhận dữ liệu IoT |
| Quản lý toàn bộ loại tài sản ngoài 5 Asset Type | MVP giới hạn 5 Asset Type |

---

### 6. MVP Boundary

#### In Scope
* **Asset Types:** Wi-Fi, Air Conditioner, Projector, Light, Fan
* **Roles:** Requester, Technician, Facility Manager, Admin
* **Core Capabilities Architecture:**

```mermaid
graph TD
    AM[Asset Management] --> IoT[IoT Monitoring]
    IoT --> Alert[IoT Alert]
    
    Req[Requester] -->|Maintenance Request| FM[Facility Manager]
    Alert --> FM
    
    FM --> WO[Work Order]
    WO --> Tech[Technician]
    Tech --> MH[Maintenance History]
    
    MH --> AI[AI Prediction]
    IoT --> AI
    AI --> Risk[Maintenance Risk]
    Risk --> FM
