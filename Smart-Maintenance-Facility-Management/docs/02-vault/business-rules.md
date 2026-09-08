# Business Rules
# Business Rules

> **Project:** Smart Maintenance & Facility Management
> **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> **Version:** 1.0
> **Status:** Baselined
> **Role:** Business Analyst

---

## 1. Mục đích

Business Rules xác định các quy tắc nghiệp vụ mà hệ thống phải tuân thủ trong quá trình quản lý Asset, Maintenance Request, Work Order, IoT Monitoring và AI Predictive Maintenance.

Business Rule khác với Functional Requirement:

- **Requirement** mô tả hệ thống phải cung cấp chức năng gì.
- **Business Rule** mô tả quy tắc, điều kiện hoặc ràng buộc nghiệp vụ mà chức năng đó phải tuân thủ.

---

## 2. Quy ước

### 2.1. Business Rule ID

Mỗi Business Rule có một ID duy nhất.

- ID đã được tạo không được thay đổi khi nội dung được chỉnh sửa.
- Chỉ tạo ID mới khi xuất hiện một business rule thực sự mới.
- Open Question khi được giải quyết có thể cập nhật Business Rule liên quan.
- Không tạo lại ID cho Business Rule đã tồn tại chỉ vì nội dung được làm rõ.

### 2.2. Source

| Source | Ý nghĩa |
|---|---|
| SRC-USER | Thông tin/yêu cầu được cung cấp hoặc xác nhận trực tiếp bởi stakeholder/user |
| SRC-BA | Business Rule được BA chuẩn hóa, phân rã hoặc diễn giải từ thông tin đã có |
| SRC-BA-Q | BA xác định nội dung cần stakeholder xác nhận; sau khi được resolve sẽ cập nhật lại Business Rule |

### 2.3. Confidence

| Mức | Ý nghĩa |
|---|---|
| High | Được stakeholder xác nhận hoặc nêu rõ |
| Medium | BA chuẩn hóa/suy luận hợp lý từ context |
| Low | Phụ thuộc vào thông tin chưa được xác nhận |

---

## 3. Business Rules

| ID | Business Rule | Source | Confidence |
|---|---|---|---|
| BR-01 | Mỗi Asset có một Asset ID duy nhất. | SRC-BA | Medium |
| BR-02 | Asset phải thuộc Asset Type được hệ thống hỗ trợ. | SRC-USER | High |
| BR-03 | MVP chỉ hỗ trợ Wi-Fi, Air Conditioner, Projector, Light và Fan. | SRC-USER | High |
| BR-04 | Mỗi Asset phải có Location/Room. | SRC-USER | High |
| BR-05 | Maintenance Request phải xác định Asset hoặc khu vực bị ảnh hưởng. Nếu chỉ xác định khu vực thì phải xác định Asset trước khi tạo WO. | SRC-BA | High |
| BR-06 | Một Maintenance Request chỉ tạo tối đa một Work Order trong MVP. | SRC-USER + SRC-BA | High |
| BR-07 | Technician chỉ được cập nhật WO được phân công cho mình; có thể từ chối kèm lý do. | SRC-USER + SRC-BA | High |
| BR-08 | Mỗi Work Order phải liên kết với một Asset cụ thể. | SRC-BA | Medium |
| BR-09 | WO hoàn thành phải có kết quả được lưu vào Maintenance History. | SRC-BA | Medium |
| BR-10 | AI chỉ hỗ trợ quyết định; Facility Manager quyết định cuối cùng. | SRC-BA | Medium |
| BR-11 | AI Prediction sử dụng IoT Data và Maintenance History phù hợp. Có thể sử dụng dữ liệu mẫu/giả lập trong MVP. | SRC-USER + SRC-BA | High |
| BR-12 | IoT Alert được tạo khi IoT Data đáp ứng điều kiện bất thường/threshold đã cấu hình theo Asset Type. | SRC-USER + SRC-BA | High |
| BR-13 | IoT Device/Sensor phải được mapping với Asset trước khi sử dụng dữ liệu để monitoring. Một Asset chỉ có một IoT Device/Sensor trong MVP. | SRC-USER + SRC-BA | High |
| BR-14 | Maintenance Request sử dụng Submitted, Pending, In Progress, Resolved, Closed và Rejected. | SRC-USER | High |
| BR-15 | Facility Manager có thể ưu tiên Asset có Maintenance Risk = High. | SRC-BA | Medium |
| BR-16 | Chỉ Facility Manager được thay đổi thủ công Asset Status. | SRC-USER | High |
| BR-17 | Work Order sử dụng Assigned, In Progress, Completed và Cancelled. | SRC-USER + SRC-BA | High |
| BR-18 | Maintenance Request chỉ được Closed sau khi Technician hoàn thành WO và Facility Manager xác nhận kết quả. | SRC-USER | High |

---

## 4. Relationship with Requirements

Business Rules không thay thế Functional Requirements.

Một chức năng có thể được thực hiện dựa trên một hoặc nhiều Business Rules.

Ví dụ:

**REQ-14**

> Facility Manager có thể tạo một Work Order từ Maintenance Request hoặc nhu cầu bảo trì được xác định.

Chức năng này phải tuân thủ:

- **BR-05:** Request phải xác định Asset trước khi tạo WO nếu ban đầu chỉ xác định khu vực.
- **BR-06:** Một Maintenance Request chỉ tạo tối đa một WO trong MVP.
- **BR-08:** WO phải liên kết với một Asset cụ thể.

---

### Ví dụ khác

**REQ-22**

> Technician có thể hoàn thành Work Order sau khi thực hiện và ghi nhận kết quả bảo trì.

Chức năng này phải tuân thủ:

- **BR-07:** Technician chỉ cập nhật WO được phân công.
- **BR-09:** WO hoàn thành phải có kết quả được lưu vào Maintenance History.
- **BR-17:** WO sử dụng lifecycle Assigned → In Progress → Completed/Cancelled.

---

### AI & IoT

**REQ-11 / REQ-12 / REQ-29**

được ràng buộc bởi:

- **BR-10:** AI chỉ hỗ trợ quyết định.
- **BR-11:** AI sử dụng IoT Data và Maintenance History phù hợp.
- **BR-12:** IoT Alert dựa trên threshold/điều kiện bất thường.
- **BR-13:** Asset phải được mapping với IoT Device/Sensor trước khi monitoring.
- **BR-15:** Facility Manager có thể ưu tiên Asset có Risk = High.

---

## 5. Related Documents

- `requirements.md` — Functional & Non-Functional Requirements
- `open-questions.md` — Open Questions
- `decision-log.md` — Decisions affecting Business Rules
- `PRD.md` — Product Requirements Document
- `MVP-scope.md` — MVP Scope
