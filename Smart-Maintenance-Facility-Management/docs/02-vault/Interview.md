# Interview

## 1. Interview Overview

* **Ngày:** 2026-09-01
* **Người tham gia:** Project Owner (mô phỏng)
* **Hình thức:** Mini-interview / Stakeholder Proxy
* **Mục đích:** Xác nhận các Open Questions liên quan đến phạm vi MVP, business flow, IoT và AI trước khi hoàn thiện Requirements và Business Rules.

> **Lưu ý:** Do phạm vi đồ án không có điều kiện phỏng vấn trực tiếp đầy đủ các stakeholder thực tế của Trường Đại học Kinh tế – Đại học Đà Nẵng, người tham gia được ghi nhận dưới dạng **Project Owner (mô phỏng)**. Các quyết định dưới đây được dùng làm baseline cho tài liệu phân tích và cần được xác thực lại nếu triển khai thực tế.

---

# 2. Interview Decisions

## Quyết định 1 - Phạm vi Asset và quyền xem Asset

**- Ngày:** 2026-09-01
**- Người tham gia:** Project Owner (mô phỏng)
**- Open Questions liên quan:** Q-01; Q-18
**- Yêu cầu chức năng liên quan:** REQ-02; REQ-03; REQ-06; REQ-07; REQ-08
**- Business Rule liên quan:** BR-01; BR-02; BR-03; BR-04
**- NFR liên quan:** NFR-01; NFR-04; NFR-07

**- Quyết định:** Requester chỉ được xem Asset thuộc các phòng/khu vực mình được phép sử dụng, không được xem toàn bộ Asset trong hệ thống. Trong MVP, Asset chỉ quản lý các thông tin cơ bản gồm **Asset ID, Name, Type, Location và Status**. Phạm vi Asset gồm **Wi-Fi, Air Conditioner, Projector, Light và Fan**.

**- Lý do:** Giới hạn quyền truy cập theo khu vực giúp bảo đảm phân quyền và tập trung vào phạm vi MVP. Chỉ quản lý các thông tin cơ bản giúp hệ thống đơn giản và phù hợp với mục tiêu của đồ án.

**- Trạng thái:** CONFIRMED

---

## Quyết định 2 - Maintenance Request

**- Ngày:** 2026-09-01
**- Người tham gia:** Project Owner (mô phỏng)
**- Open Questions liên quan:** Q-02; Q-03; Q-05; Q-06; Q-08
**- Yêu cầu chức năng liên quan:** REQ-04; REQ-05; REQ-13; REQ-14; REQ-22; REQ-23
**- Business Rule liên quan:** BR-05; BR-06; BR-14; BR-18
**- NFR liên quan:** NFR-03; NFR-04

**- Quyết định:** Requester có thể tạo Maintenance Request cho Asset hoặc khu vực bị ảnh hưởng. Hình ảnh/video là **không bắt buộc** và chỉ được đính kèm khi cần. Maintenance Request sử dụng các trạng thái **Submitted → Pending → In Progress → Resolved → Closed** và có thể có **Rejected** khi request không hợp lệ. Facility Manager không cần bước approve riêng mà tiếp nhận request và có thể tạo Work Order trực tiếp. Một Maintenance Request chỉ tạo **một Work Order trong MVP**. Sau khi Technician hoàn thành Work Order, Facility Manager phải xác nhận kết quả trước khi đóng Maintenance Request.

**- Lý do:** Quy trình giảm bước trung gian nhưng vẫn bảo đảm Facility Manager kiểm soát quá trình xử lý và kết quả cuối cùng.

**- Trạng thái:** CONFIRMED

---

## Quyết định 3 - Work Order và Technician

**- Ngày:** 2026-09-01
**- Người tham gia:** Project Owner (mô phỏng)
**- Open Questions liên quan:** Q-04; Q-07
**- Yêu cầu chức năng liên quan:** REQ-14; REQ-15; REQ-16; REQ-17; REQ-20; REQ-21; REQ-22
**- Business Rule liên quan:** BR-06; BR-07; BR-08; BR-09; BR-17
**- NFR liên quan:** NFR-03; NFR-04; NFR-07

**- Quyết định:** Work Order trong MVP gồm 4 trạng thái **Assigned → In Progress → Completed → Cancelled**. Facility Manager tạo và phân công Work Order cho Technician. Technician có thể cập nhật Work Order, ghi nhận kết quả kiểm tra/bảo trì và **từ chối Work Order kèm lý do** để Facility Manager xem xét phân công lại.

**- Lý do:** Phân tách rõ trách nhiệm giữa Facility Manager và Technician, đồng thời cho phép xử lý trường hợp Technician không thể thực hiện Work Order.

**- Trạng thái:** CONFIRMED

> **Lưu ý:** `Rejected` của Technician được xem là **action**, không được thêm thành trạng thái chính thức của Work Order. Nội dung này cần được giữ nhất quán với OQ-01 nếu quyết định chính thức về lifecycle được cập nhật sau này.

---

## Quyết định 4 - IoT Monitoring và Alert

**- Ngày:** 2026-09-01
**- Người tham gia:** Project Owner (mô phỏng)
**- Open Questions liên quan:** Q-09; Q-10; Q-15; Q-16; Q-19
**- Yêu cầu chức năng liên quan:** REQ-09; REQ-10; REQ-26; REQ-27; REQ-28; REQ-30
**- Business Rule liên quan:** BR-12; BR-13
**- NFR liên quan:** NFR-04; NFR-05; NFR-06

**- Quyết định:** IoT Data được thu thập theo chu kỳ **5 phút/lần** trong MVP. Mỗi Asset chỉ mapping với **một IoT Device/Sensor**. IoT Alert trong MVP sử dụng **threshold được cấu hình đơn giản theo từng Asset Type**. Hệ thống có thể kết hợp phát hiện bất thường đơn giản nhưng không yêu cầu mô hình anomaly detection phức tạp. Facility Manager nhận Notification khi xuất hiện **IoT Alert nghiêm trọng**.

**- Lý do:** Cách tiếp cận này đủ để chứng minh khả năng giám sát Asset bằng IoT trong MVP mà không làm tăng quá mức độ phức tạp kỹ thuật.

**- Trạng thái:** CONFIRMED

> **Lưu ý:** Giá trị threshold cụ thể và loại IoT Data cụ thể cho từng Asset Type chưa được chốt trong interview, do đó vẫn được quản lý dưới dạng Open Question và không được tự bổ sung vào Business Rules.

---

## Quyết định 5 - AI Predictive Maintenance

**- Ngày:** 2026-09-01
**- Người tham gia:** Project Owner (mô phỏng)
**- Open Questions liên quan:** Q-11; Q-12; Q-13; Q-14; Q-19
**- Yêu cầu chức năng liên quan:** REQ-11; REQ-12; REQ-29; REQ-30
**- Business Rule liên quan:** BR-10; BR-11; BR-15
**- NFR liên quan:** NFR-08

**- Quyết định:** AI Prediction chỉ dự đoán **khả năng Asset cần bảo trì trong 7 ngày tiếp theo**, dựa trên dữ liệu IoT và Maintenance History. Kết quả được hiển thị dưới dạng **Maintenance Risk: Low / Medium / High**. Nếu chưa có đủ Maintenance History thực tế, MVP có thể sử dụng **dữ liệu mẫu/giả lập** để chứng minh chức năng AI và không yêu cầu xây dựng mô hình AI phức tạp. AI chỉ đóng vai trò hỗ trợ; **Facility Manager là người quyết định việc xử lý**. Facility Manager nhận Notification khi Maintenance Risk = **High**.

**- Lý do:** Prediction horizon 7 ngày và ba mức Risk giúp chức năng AI đơn giản, dễ giải thích và dễ kiểm chứng trong phạm vi MVP, đồng thời vẫn thể hiện được vai trò của AI trong Predictive Maintenance.

**- Trạng thái:** CONFIRMED

---

## Quyết định 6 - Authentication và phân quyền

**- Ngày:** 2026-09-01
**- Người tham gia:** Project Owner (mô phỏng)
**- Open Questions liên quan:** Q-20
**- Yêu cầu chức năng liên quan:** REQ-01; REQ-24; REQ-25
**- Business Rule liên quan:** —
**- NFR liên quan:** NFR-01; NFR-02

**- Quyết định:** MVP sử dụng **tài khoản riêng của hệ thống**, không tích hợp với hệ thống Identity/SSO hiện có của Trường Đại học Kinh tế – Đại học Đà Nẵng. Hệ thống áp dụng Role-Based Access Control với 4 vai trò chính: **Requester, Technician, Facility Manager và Admin**.

**- Lý do:** Giảm phụ thuộc vào hệ thống authentication bên ngoài và phù hợp với phạm vi phân tích, thiết kế MVP.

**- Trạng thái:** CONFIRMED

---

# 3. Interview Decision Summary

| Decision   | Main Topic                      | Open Questions               | Status    |
| ---------- | ------------------------------- | ---------------------------- | --------- |
| Decision 1 | Asset & Asset Access            | Q-01, Q-18                   | CONFIRMED |
| Decision 2 | Maintenance Request             | Q-02, Q-03, Q-05, Q-06, Q-08 | CONFIRMED |
| Decision 3 | Work Order & Technician         | Q-04, Q-07                   | CONFIRMED |
| Decision 4 | IoT Monitoring & Alert          | Q-09, Q-10, Q-15, Q-16, Q-19 | CONFIRMED |
| Decision 5 | AI Predictive Maintenance       | Q-11, Q-12, Q-13, Q-14, Q-19 | CONFIRMED |
| Decision 6 | Authentication & Access Control | Q-20                         | CONFIRMED |

---

# 4. Impact on Project Baseline

Các quyết định trên được sử dụng để cập nhật và xác nhận các tài liệu liên quan:

```text
Interview
    ↓
Open Questions
    ↓
Confirmed Decisions
    ↓
Requirements / Business Rules
    ↓
PRD / Use Cases / User Stories
    ↓
Acceptance Criteria
    ↓
Traceability Matrix
```

Các quyết định đã xác nhận **không tạo Requirement hoặc Business Rule mới một cách độc lập**. Chúng được dùng để cập nhật hoặc xác nhận các ID đã tồn tại trong Requirements và Business Rules.

Các vấn đề chưa có đủ thông tin, đặc biệt là **IoT Data cụ thể, threshold cụ thể theo từng Asset Type và chi tiết AI Dataset**, tiếp tục được quản lý trong `open-questions.md`.
