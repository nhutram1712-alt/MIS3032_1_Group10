# Story Spec - US-04-02 — Phân công Work Order

**Story ID:** US-04-02
**Requirement IDs:** REQ-15, BR-07, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Facility Manager** thực hiện phân công Work Order cho Technician để mỗi công việc bảo trì có người chịu trách nhiệm thực hiện.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`FacilityManager`** và có JWT Token hợp lệ.
- Work Order đã được tạo trong bảng `WORK_ORDERS` với Status `Assigned` hoặc chưa có Technician.
- Tài khoản Technician cần phân công đã tồn tại trong bảng `USERS` với Role `Technician`.

## HAPPY PATH
1. Facility Manager truy cập trang Quản lý Work Order.
2. Facility Manager chọn Work Order cần phân công và click "Phân công Technician".
3. Hệ thống hiển thị danh sách Technician có sẵn (`GET /api/users?role=Technician`).
4. Facility Manager chọn Technician phù hợp.
5. Client gửi `PATCH /api/work-orders/{id}` với payload `{ "technicianId": 7 }`.
6. Server xác thực JWT Token và Role `FacilityManager`.
7. Server kiểm tra `WorkOrderID` tồn tại và `TechnicianID` hợp lệ (có Role `Technician`).
8. Server cập nhật `TechnicianID` trong bảng `WORK_ORDERS`, đảm bảo Status là `Assigned`.
9. Server trả về `200 OK`.
10. Technician được phân công có thể thấy Work Order trong danh sách của mình (US-04-04).

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `technicianId` trong payload không tồn tại trong hệ thống, hoặc `UserID` được chỉ định không có Role `Technician`.
- **Lỗi 404 Not Found:** Xảy ra khi `WorkOrderID` trong URL không tồn tại.
- **Lỗi 403 Forbidden:** User không có Role `FacilityManager`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `WORK_ORDERS` — đọc theo `OrderID` để kiểm tra tồn tại và Status hiện tại.
- **Read:** Bảng `USERS` — đọc để xác minh `TechnicianID` hợp lệ và có Role `Technician`; đọc danh sách Technician để hiển thị lựa chọn.
- **Write:** Bảng `WORK_ORDERS` — Update cột `TechnicianID` theo `OrderID`.

## API CONTRACT

### Phân công Technician cho Work Order
- **Method:** `PATCH`
- **Endpoint:** `/api/work-orders/{id}`
- **Auth Level:** FacilityManager
- **Request Payload:**
```json
{
  "technicianId": 7
}
```
- **Response:**
  - `200 OK`: Phân công thành công
  - `400 Bad Request`: TechnicianID không hợp lệ
  - `404 Not Found`: WorkOrderID không tồn tại
  - `403 Forbidden`: Không đủ quyền

### Xem danh sách Technician (hỗ trợ UI chọn Technician)
- **Method:** `GET`
- **Endpoint:** `/api/users?role=Technician`
- **Auth Level:** FacilityManager
- **Response:** `200 OK: [{ "userId": 7, "username": "tech_tran", "role": "Technician" }]`

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `FacilityManager`.
- Chỉ Facility Manager mới được phép phân công Technician cho Work Order.

## VALIDATION/BUSINESS RULES
- **BR-07 (Technician chỉ cập nhật WO được phân công cho mình):** Sau khi Facility Manager phân công, hệ thống ghi nhận `TechnicianID` vào Work Order. Technician chỉ được thao tác trên Work Order có `TechnicianID` khớp với `UserID` của họ.
- **Validation TechnicianID:** `TechnicianID` phải là `UserID` tồn tại trong hệ thống với Role `Technician`. Không được phân công cho Requester, FacilityManager, hay Admin.
- **NFR-03 (Tính nhất quán dữ liệu):** Khi phân công, Work Order Status phải là `Assigned`. Không phân công lại cho Work Order đã `Completed` hoặc `Cancelled`.
- **NFR-04 (Lưu timestamp):** `CreatedAt` của Work Order đã có. Khuyến nghị log thời điểm phân công vào Observability.
- **REQ-15:** Facility Manager có thể phân công Work Order cho Technician — yêu cầu Must.

## OBSERVABILITY/LOGGING
- Ghi log action `WorkOrderAssigned` bằng Serilog, bao gồm:
  - `UserID` (Facility Manager thực hiện)
  - `OrderID`
  - `TechnicianID` được phân công
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Phân công với `TechnicianID` không tồn tại → 400.
  - Phân công với `UserID` không có Role `Technician` → 400.
  - Phân công Work Order đã `Completed` → 400 (không hợp lệ).
  - Phân công với `WorkOrderID` không tồn tại → 404.
  - Thực hiện bởi Role `Technician` → 403.
  - Phân công hợp lệ → `TechnicianID` được cập nhật đúng trong bảng `WORK_ORDERS`.
- **Integration Test:**
  - FM tạo Work Order → FM phân công Technician.
  - Technician đăng nhập → GET danh sách Work Order → thấy Work Order vừa được phân công.
  - Technician khác không thấy Work Order đó trong danh sách của họ.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- API cập nhật đúng `TechnicianID` trong bảng `WORK_ORDERS`.
- Validation TechnicianID phải có Role `Technician` hoạt động chính xác.
- Commit có chứa mã Story ID `US-04-02`.
