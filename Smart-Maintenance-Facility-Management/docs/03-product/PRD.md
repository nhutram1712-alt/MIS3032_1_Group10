# Product Requirements Document
# Product Requirements Document (PRD)

| Metadata | Details |
| --- | --- |
| **Project** | Smart Maintenance & Facility Management |
| **Organization** | Trường Đại học Kinh tế – Đại học Đà Nẵng |
| **Document** | Product Requirements Document |
| **Version** | 1.0 |
| **Status** | Draft / Baseline |
| **Role** | Business Analyst |

---

## 1. Product Overview

### 1.1. Product Name
**Smart Maintenance & Facility Management System**

### 1.2. Product Purpose
Hệ thống được xây dựng nhằm hỗ trợ Trường Đại học Kinh tế – Đại học Đà Nẵng quản lý Asset và hoạt động bảo trì cơ sở vật chất theo hướng tập trung, có khả năng theo dõi dữ liệu IoT và hỗ trợ dự đoán nhu cầu bảo trì bằng AI.

Hệ thống chuyển quy trình quản lý bảo trì từ việc tiếp nhận và xử lý thông tin phân tán sang một quy trình tập trung:
Maintenance Request ──> Facility Manager ──> Work Order ──> Technician ──> Maintenance Result ──> Maintenance History
Đồng thời, hệ thống hỗ trợ proactive/predictive maintenance:
IoT Device/Sensor ──> IoT Data ──> Threshold / Anomaly Detection ──> IoT Alert ──> AI Prediction ──> Maintenance Risk ──> Facility Manager ──> Work Order
## 2. Problem Statement

### 2.1. Current Problem
Hoạt động quản lý Asset và bảo trì cơ sở vật chất hiện tại gặp phải các vấn đề:
* Thông tin về Asset và trạng thái Asset khó được quản lý tập trung.
* Maintenance Request chưa được tiếp nhận và theo dõi theo một quy trình thống nhất.
* Việc phân công và theo dõi Work Order chưa được quản lý rõ ràng.
* Lịch sử bảo trì chưa được lưu trữ hệ thống để phục vụ việc theo dõi tình trạng Asset.
* Dữ liệu từ IoT Device/Sensor chưa được liên kết với Asset để hỗ trợ monitoring.
* Facility Manager thiếu thông tin từ IoT và AI để hỗ trợ xác định Asset có nguy cơ cần bảo trì.
* Khi có Alert hoặc Maintenance Risk cao, Facility Manager không nhận được thông báo kịp thời.

### 2.2. Problem to Solve
Làm thế nào để xây dựng một hệ thống tập trung giúp quản lý Asset, Maintenance Request, Work Order và Maintenance History, đồng thời sử dụng IoT và AI để hỗ trợ Facility Manager phát hiện vấn đề và dự đoán nhu cầu bảo trì?

---

## 3. Product Goals

### 3.1. Business Goals
* Tập trung hóa việc quản lý Asset và thông tin bảo trì.
* Chuẩn hóa quy trình từ Maintenance Request đến Work Order và Maintenance History.
* Cung cấp dữ liệu IoT để hỗ trợ theo dõi tình trạng Asset.
* Sử dụng AI để hỗ trợ dự đoán khả năng Asset cần bảo trì.
* Giúp Facility Manager ưu tiên xử lý các Asset có Maintenance Risk cao.
* Giảm sự phụ thuộc vào việc theo dõi bảo trì thủ công và thông tin phân tán.

### 3.2. User Goals
* **Requester:** Báo cáo vấn đề của Asset, theo dõi trạng thái Maintenance Request, xem Asset thuộc phòng/khu vực mình được phép sử dụng.
* **Technician:** Xem Work Order được phân công, xem thông tin Asset, xem IoT Alert và AI Prediction liên quan, cập nhật và hoàn thành công việc bảo trì.
* **Facility Manager:** Quản lý Asset, tiếp nhận/xử lý Maintenance Request, tạo và phân công Work Order, theo dõi hoạt động bảo trì, theo dõi IoT Data/Alert, xem AI Prediction/Maintenance Risk, nhận Notification khi có Alert nghiêm trọng hoặc Risk cao.
* **Admin:** Quản lý User Account, Role/quyền, và mapping giữa Asset với IoT Device/Sensor.

---

## 4. Target Users

| User | Vai trò trong hệ thống | Mục tiêu chính |
| --- | --- | --- |
| **Requester** | Người yêu cầu bảo trì | Báo cáo và theo dõi vấn đề |
| **Technician** | Nhân viên kỹ thuật | Thực hiện và cập nhật công việc bảo trì |
| **Facility Manager** | Quản lý cơ sở vật chất | Quản lý Asset và toàn bộ quy trình bảo trì |
| **Admin** | Quản trị hệ thống | Quản lý User, Role và IoT Mapping |

---

## 5. User Personas

### 5.1. Requester
* **Role:** Student / Lecturer / Staff
* **Needs:** 
  * Báo cáo Asset bị lỗi.
  * Biết Request đang được xử lý đến đâu.
  * Chỉ nhìn thấy Asset trong phạm vi phòng/khu vực mình được phép sử dụng.
* **Pain Points:** 
  * Khó biết Request đã được tiếp nhận hay chưa.
  * Khó theo dõi tiến độ xử lý nếu thông tin phân tán.

### 5.2. Technician
* **Role:** Maintenance Technician
* **Needs:** 
  * Nhận Work Order.
  * Biết Asset cần xử lý.
  * Biết tình trạng và thông tin liên quan đến Asset.
  * Xem Alert/Prediction để hỗ trợ kiểm tra.
  * Ghi nhận kết quả bảo trì.
* **Pain Points:** 
  * Cần thông tin đầy đủ trước khi thực hiện công việc.
  * Có thể cần từ chối/reassign Work Order nếu không phù hợp.

### 5.3. Facility Manager
* **Role:** Facility Manager
* **Needs:** 
  * Theo dõi Asset, quản lý Maintenance Request, phân công Technician, theo dõi Work Order.
  * Theo dõi IoT Alert, xem AI Prediction và Maintenance Risk để ưu tiên các Asset có Risk cao.
* **Pain Points:** 
  * Cần tổng hợp nhiều loại thông tin để đưa ra quyết định bảo trì.
  * Cần phát hiện sớm các Asset có dấu hiệu bất thường.

### 5.4. Admin
* **Role:** System Administrator
* **Needs:** 
  * Quản lý tài khoản, Role và quyền.
  * Mapping Asset với IoT Device/Sensor.

---

## 6. Jobs To Be Done (JTBD)

* **JTBD-01 — Requester:** Khi phát hiện Asset có vấn đề, tôi muốn gửi Maintenance Request và theo dõi trạng thái để biết vấn đề của mình đang được xử lý như thế nào.
* **JTBD-02 — Technician:** Khi được giao Work Order, tôi muốn xem đầy đủ thông tin Asset và các cảnh báo liên quan để có thể thực hiện bảo trì và ghi nhận kết quả.
* **JTBD-03 — Facility Manager:** Khi quản lý nhiều Asset, tôi muốn theo dõi Request, Work Order, IoT Alert và AI Prediction để có thể xác định và ưu tiên nhu cầu bảo trì.
* **JTBD-04 — Admin:** Khi quản lý hệ thống, tôi muốn quản lý User, Role và IoT Mapping để đảm bảo người dùng có đúng quyền và dữ liệu IoT được liên kết đúng Asset.

---

## 7. Product Scope

### 7.1. In Scope — MVP
* **Asset Management:** Thêm/Cập nhật/Xem Asset, Quản lý Asset Status, Quản lý 5 Asset Type (*Wi-Fi, Air Conditioner, Projector, Light, Fan*).
* **Maintenance Request:** Tạo, theo dõi, xử lý Request, quản lý lifecycle (không bắt buộc attachment).
* **Work Order:** Tạo WO, phân công Technician, theo dõi WO, Technician cập nhật WO / từ chối WO kèm lý do, hoàn thành WO và ghi nhận kết quả.
* **Maintenance History:** Lưu kết quả Work Order vào lịch sử bảo trì của Asset.
* **IoT:** Mapping Asset với IoT Device/Sensor, thu thập IoT Data (chu kỳ 5 phút/lần), Threshold theo Asset Type, IoT Alert.
* **AI:** Sử dụng IoT Data và Maintenance History để dự đoán khả năng cần bảo trì trong 7 ngày. Phân loại Maintenance Risk (*Low / Medium / High*). Có thể dùng dữ liệu mẫu/giả lập trong MVP.
* **Notification:** Thông báo Facility Manager khi có IoT Alert nghiêm trọng hoặc Maintenance Risk = High.
* **User & Access:** Login bằng tài khoản riêng, hỗ trợ 4 Role (*Requester, Technician, Facility Manager, Admin*).

### 7.2. Out of Scope
* Finance Management, Procurement, Inventory Management, Spare Parts Management, Supplier/Vendor Management, HR Management, Academic/Student Management, General University Administration.
* SSO (Single Sign-On).
* AI tự động ra quyết định bảo trì.
* Sản xuất/chế tạo phần cứng IoT (IoT hardware manufacturing).
* Quản lý các loại tài sản ngoài 5 Asset Type đã xác định.

---

## 8. Core Product Workflows

### 8.1. Reactive Maintenance

```mermaid
graph TD
    A[Requester: Create Maintenance Request] --> B[Facility Manager: Review / Process Request]
    B --> C[Create Work Order & Assign Technician]
    C --> D{Technician: Accept or Reject?}
    D -->|Accept| E[In Progress Maintenance]
    D -->|Reject| B
    E --> F[Record Maintenance Result & Complete]
    F --> G[Facility Manager: Confirm Result]
    G --> H[Maintenance Request = Closed]
    H --> I[Save to Maintenance History]
