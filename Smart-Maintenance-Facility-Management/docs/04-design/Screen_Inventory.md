# Screen Inventory + State Matrix — Smart Campus Facility Management

> **Nguồn:** Prototype HTML/React đã build ở Giai đoạn 3 (18 screenshot) + usability findings (OQ-01, Q-22, DEC-09/10/11/12).  
> **Mục đích:** Input bắt buộc cho Bước 1 của Giai đoạn 6 — *"Từ prototype findings, xác định screen inventory và states cần thiết"* trước khi dựng Figma.

---

## 1. Danh sách màn hình theo vai trò (Screen Inventory)

| STT | Role | Screen | Route đề xuất |
| :---: | :--- | :--- | :--- |
| 1 | **Shared** | Đăng nhập | `/login` |
| 2 | **Requester** | Tổng quan | `/requester` |
| 3 | **Requester** | Yêu cầu sự cố | `/requester/requests` |
| 4 | **Facility Manager** | Tổng quan | `/fm` |
| 5 | **Facility Manager** | Tài sản (Danh mục cơ sở vật chất) | `/fm/assets` |
| 6 | **Facility Manager** | Work Order (điều phối) | `/fm/work-orders` |
| 7 | **Facility Manager** | IoT Alerts | `/fm/iot-alerts` |
| 8 | **Facility Manager** | AI Risks | `/fm/ai-risks` |
| 9 | **Technician** | Tổng quan | `/tech` |
| 10 | **Technician** | Work Order (được giao) | `/tech/work-orders` |
| 11 | **Technician** | Tài sản & AI (view rút gọn) | `/tech/assets` |
| 12 | **Technician** | IoT Alerts (view rút gọn) | `/tech/iot-alerts` |
| 13 | **Admin** | Tổng quan | `/admin` |
| 14 | **Admin** | Người dùng | `/admin/users` |
| 15 | **Admin** | IoT Mapping | `/admin/iot-mapping` |

---

## 2. State Matrix — States bắt buộc theo brief Giai đoạn 3

> **Chú thích ký hiệu:**  
> - **`X`**: Đã có trên prototype  
> - **`O`**: Chưa có, cần thiết kế mới ở Figma  
> - **`-`**: Không áp dụng / Không yêu cầu  

| Screen | default | loading | empty | error | confirmation/success | permission |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: |
| **Yêu cầu sự cố (Requester)** | X | O | O | O | — | — |
| **Work Order (FM)** | X | O | O | O | — | — |
| **Work Order (Technician)** | X | O | O | O | X | O |
| **IoT Alerts / AI Risks** | X | O | O | O | — | O |
| **Tài sản (FM)** | X | O | O | O | X | — |
| **Người dùng / IoT Mapping (Admin)** | X | O | O | O | X | — |
