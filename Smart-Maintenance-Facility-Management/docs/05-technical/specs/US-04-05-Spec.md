# Story Spec - US-04-05 — Cập nhật hoặc từ chối Work Order

**Story ID:** US-04-05
**Requirement IDs:** REQ-20, BR-07, BR-17, NFR-03, NFR-04
**Design link:** TODO
**Goal:** Cho phép **Technician** thực hiện cập nhật trạng thái hoặc từ chối Work Order được phân công kèm lý do để phản ánh chính xác khả năng và tiến độ thực hiện công việc của mình.

## PRECONDITIONS
- Người dùng đã đăng nhập với Role **`Technician`** và có JWT Token hợp lệ.
- Work Order cần thao tác đã được phân công cho Technician này (có `TechnicianID` khớp) với Status `Assigned` hoặc `In Progress`.

## HAPPY PATH

### H1 — Bắt đầu thực hiện Work Order (Assigned → In Progress)
1. Technician xem Work Order có Status `Assigned` trong danh sách.
2. Technician click "Bắt đầu thực hiện".
3. Client gửi `PATCH /api/work-orders/{id}` với `{ "status": "In Progress" }`.
4. Server xác thực JWT Token, Role `Technician`, và `TechnicianID` trong WO khớp với UserID.
5. Server cập nhật `Status` = `In Progress` trong bảng `WORK_ORDERS`.
6. Server trả về `200 OK`.

### H2 — Từ chối Work Order kèm lý do
1. Technician nhận Work Order nhưng không thể thực hiện (không đủ dụng cụ, không đúng chuyên môn, v.v.).
2. Technician click "Từ chối", nhập lý do từ chối vào trường `rejectionReason`.
3. Client gửi `PATCH /api/work-orders/{id}` với `{ "status": "Assigned", "rejectionReason": "Không có dụng cụ phù hợp." }`.

   > **Open Question (từ user-stories.md):** Hiện tại chưa xác định Reject là action giữ Status `Assigned` hay thêm Status `Rejected` mới. Tạm thời xử lý: Technician từ chối → WO vẫn giữ Status `Assigned` + ghi `RejectionReason` để FM biết cần tái phân công.

4. Server cập nhật `RejectionReason` trong bảng `WORK_ORDERS`, Status giữ nguyên `Assigned`.
5. Server trả về `200 OK`.
6. Facility Manager thấy `RejectionReason` và quyết định phân công lại hoặc hủy WO.

## ALTERNATE/ERROR PATHS
- **Lỗi 400 Bad Request:** Xảy ra khi `status` không hợp lệ (không thuộc `Assigned`, `In Progress`); hoặc cố chuyển sang trạng thái không được phép với Role `Technician` (ví dụ: `Cancelled`, `Completed` trực tiếp từ `Assigned`); hoặc `rejectionReason` trống khi action từ chối.
- **Lỗi 404 Not Found:** Xảy ra khi `WorkOrderID` không tồn tại.
- **Lỗi 403 Forbidden — Vi phạm BR-07:** Technician cố cập nhật Work Order **không được phân công** cho mình (`TechnicianID` không khớp với `UserID`).
- **Lỗi 403 Forbidden:** User không có Role `Technician`.
- **Lỗi 401 Unauthorized:** JWT Token không hợp lệ hoặc hết hạn.

## DATA READ/WRITE
- **Read:** Bảng `WORK_ORDERS` — đọc theo `OrderID` để kiểm tra `TechnicianID`, `Status` hiện tại trước khi update.
- **Write:** Bảng `WORK_ORDERS` — Update cột `Status` (khi bắt đầu thực hiện); Update cột `RejectionReason` (khi từ chối); cả hai thao tác theo `OrderID`.

## API CONTRACT
- **Method:** `PATCH`
- **Endpoint:** `/api/work-orders/{id}`
- **Auth Level:** Technician (JWT bắt buộc)

### Bắt đầu thực hiện
- **Request Payload:**
```json
{
  "status": "In Progress"
}
```
- **Response:** `200 OK` | `400 Bad Request` | `403 Forbidden` | `404 Not Found`

### Từ chối Work Order
- **Request Payload:**
```json
{
  "status": "Assigned",
  "rejectionReason": "Không có dụng cụ phù hợp để sửa chữa loại này."
}
```
- **Response:** `200 OK` | `400 Bad Request` | `403 Forbidden` | `404 Not Found`

**Lưu ý:** Endpoint `PATCH /api/work-orders/{id}` dùng chung cho cả FM (phân công, hủy) và Technician (cập nhật, từ chối). Server phải phân biệt hành động dựa trên Role và payload.

## AUTHORIZATION
- Kiểm tra JWT Token, bắt buộc Role phải là `Technician`.
- **BR-07 (QUAN TRỌNG):** Server phải kiểm tra `WORK_ORDERS.TechnicianID = JWT.UserID` trước khi cho phép cập nhật. Nếu không khớp → `403 Forbidden` với message: `"Bạn không được phép cập nhật Work Order không được phân công cho mình."`.

## VALIDATION/BUSINESS RULES
- **BR-07 (Technician chỉ cập nhật WO được phân công cho mình):** Đây là Business Rule cốt lõi. Backend **phải enforce** kiểm tra ownership trước bất kỳ thao tác ghi nào. Không được dựa vào client-side filtering.
- **BR-17 (Work Order Status lifecycle từ phía Technician):** Technician chỉ được thực hiện các transition sau:
  - `Assigned` → `In Progress` (bắt đầu thực hiện)
  - `Assigned` → `Assigned` + ghi `RejectionReason` (từ chối — giữ nguyên Status)
  - `In Progress` → `Completed` (hoàn thành — xem US-04-06)
  - Technician **không được phép**: `Assigned` → `Cancelled`, `In Progress` → `Cancelled` (chỉ FM hủy).
- **Validation `rejectionReason`:** Khi action từ chối, trường `rejectionReason` không được để trống. Server trả về `400` nếu thiếu lý do.
- **NFR-03 (Tính nhất quán dữ liệu):** Sau khi Technician từ chối (`RejectionReason` được ghi), Maintenance Request liên quan vẫn giữ nguyên Status — FM chịu trách nhiệm quyết định bước tiếp theo.
- **NFR-04 (Lưu timestamp):** Khuyến nghị log thời điểm Technician bắt đầu thực hiện hoặc từ chối WO.

## OBSERVABILITY/LOGGING
- Ghi log action `WorkOrderUpdated` và `WorkOrderRejected` bằng Serilog, bao gồm:
  - `UserID` (Technician thực hiện)
  - `OrderID`
  - `OldStatus` → `NewStatus`
  - `RejectionReason` (nếu có)
  - `Timestamp`

## TEST PLAN
- **Unit Test:**
  - Technician A cập nhật WO của Technician A → 200 OK.
  - Technician A cập nhật WO của Technician B → 403 (vi phạm BR-07).
  - Chuyển `Assigned` → `In Progress` → 200 OK, Status được update.
  - Từ chối WO với `rejectionReason` hợp lệ → 200 OK, `RejectionReason` được ghi.
  - Từ chối WO với `rejectionReason` trống → 400.
  - Technician thử chuyển WO sang `Cancelled` → 400 (không được phép).
  - Chuyển WO đã `Completed` sang bất kỳ Status nào → 400 (trạng thái cuối).
  - Thao tác bởi Role `FacilityManager` qua endpoint Technician path → kiểm tra logic phân biệt.
- **Integration Test:**
  - FM phân công WO cho Technician → Technician PATCH `In Progress` → FM GET thấy Status `In Progress`.
  - Technician từ chối WO → FM GET chi tiết WO thấy `RejectionReason`.
  - Technician B thử cập nhật WO của Technician A → 403.

## DEFINITION OF DONE
- Code hoàn tất, pass toàn bộ Unit/Integration Test.
- BR-07 được enforce: Technician chỉ cập nhật được WO của mình.
- `RejectionReason` được lưu đúng khi Technician từ chối.
- Commit có chứa mã Story ID `US-04-05`.

---

> **Open Question (chưa resolved):** Tham chiếu user-stories.md — cần xác nhận action "Reject" của Technician:
> 1. WO giữ Status `Assigned` + ghi `RejectionReason` (FM tái phân công), **HOẶC**
> 2. Bổ sung Status `Rejected` chính thức vào Work Order lifecycle.
>
> **Tạm thời:** Spec này triển khai theo phương án 1. Cần cập nhật nếu stakeholder chọn phương án 2.
