# UX COPY TABLE

> **Nguyên tắc:** Rõ ràng, ngắn gọn, nhất quán. Error message luôn đi kèm hành động sửa lỗi cụ thể (không báo "có lỗi" chung chung).

---

## Flow 1 — Requester: Yêu cầu sự cố

| Loại | Trạng thái | Copy hiện tại / đề xuất |
| :--- | :--- | :--- |
| **CTA** | Nút gửi | `"Gửi yêu cầu"` *(giữ nguyên)* |
| **Error** | Chưa chọn thiết bị | `"Chọn vị trí và thiết bị trước khi gửi yêu cầu."` |
| **Error** | Thiếu mô tả | `"Mô tả ngắn để kỹ thuật viên hiểu đúng sự cố (tối thiểu 10 ký tự)."` |
| **Empty** | Chưa có yêu cầu nào | `"Bạn chưa gửi yêu cầu nào. Chọn thiết bị đang gặp sự cố ở trên để bắt đầu."` |
| **Confirmation** | Gửi thành công | **Toast:** `"Đã gửi yêu cầu — [Tên thiết bị]"` + highlight dòng mới trong bảng 2 giây *(khắc phục finding "im lặng cập nhật")* |
| **Loading** | Đang gửi | Nút chuyển `"Đang gửi…"` + disable |

---

## Flow 2 — Facility Manager: Work Order (điều phối)

| Loại | Trạng thái | Copy hiện tại / đề xuất |
| :--- | :--- | :--- |
| **CTA** | Phân công | `"Phân công"` *(giữ nguyên)* |
| **CTA** | Từ chối yêu cầu (mới) | Đổi từ `"Từ chối"` $ightarrow$ `"Từ chối yêu cầu"` để phân biệt với "Từ chối" của Technician *(khắc phục Q-22/DEC-12)* |
| **CTA** | Xác nhận & đóng yêu cầu (mới — modal) | `"Xác nhận kết quả & đóng yêu cầu"` |
| **Error** | Request đã có WO | `"Yêu cầu này đã có Work Order — không thể tạo thêm (mỗi yêu cầu chỉ có 1 Work Order)."` |
| **Error** | Chưa chọn kỹ thuật viên | `"Chọn kỹ thuật viên trước khi phân công."` |
| **Empty** | Không còn yêu cầu chờ phân công | `"Không có yêu cầu nào đang chờ phân công. Mọi việc đã được xử lý."` |
| **Confirmation** | Phân công thành công | **Toast:** `"Đã phân công [Tài sản] cho [Kỹ thuật viên]"` |
| **Confirmation** | Đóng yêu cầu thành công (mới) | **Toast:** `"Đã đóng yêu cầu — [Tên thiết bị]"` |
| **Modal Copy (mới)** | Nội dung modal xác nhận | **Tiêu đề:** `"Xác nhận kết quả bảo trì"`<br>**Mô tả:** `"Kỹ thuật viên đã báo hoàn thành: [kết quả bảo trì]. Xác nhận để đóng yêu cầu này?"`<br>**Button:** `"Đóng yêu cầu"` *(primary)* / `"Chưa xác nhận"` *(secondary)* |

---

## Flow 3 — Technician: Work Order (được giao)

| Loại | Trạng thái | Copy hiện tại / đề xuất |
| :--- | :--- | :--- |
| **CTA** | Bắt đầu | `"Bắt đầu"` *(giữ nguyên)* |
| **CTA** | Hoàn thành | `"Hoàn thành"` *(giữ nguyên)* |
| **CTA** | Từ chối | `"Từ chối"` *(của Technician, giữ nguyên vì không mơ hồ trong context này)* |
| **Error** | Từ chối thiếu lý do | `"Nhập lý do trước khi từ chối công việc."` |
| **Error** | Hoàn thành thiếu kết quả | `"Ghi kết quả bảo trì trước khi đánh dấu hoàn thành."` |
| **Trạng thái (mới)** | Sau khi từ chối | Badge `"Từ chối"` *(tách riêng khỏi "Đã hủy")* + ghi chú: `"Đã từ chối — chờ Facility Manager phân công lại."` Badge "Đã hủy" chỉ dùng khi WO bị hủy hẳn *(khắc phục OQ-01)*. |
| **Permission** | Xem WO không phải của mình | `"Bạn không có quyền thao tác trên Work Order này."` |
| **Empty** | Không có WO nào được giao | `"Bạn chưa được giao công việc nào."` |
| **Confirmation** | Hoàn thành thành công | **Inline:** Dòng chuyển badge `"Hoàn thành"` + disable action *(giữ nguyên inline, không chuyển trang)* |

---

## Flow 4 — Facility Manager / Technician: IoT Alerts & AI Risks

| Loại | Trạng thái | Copy hiện tại / đề xuất |
| :--- | :--- | :--- |
| **Empty** | Không có alert nào | `"Không có cảnh báo nào — tất cả thiết bị đang hoạt động bình thường."` |
| **Empty** | Không đủ dữ liệu để AI dự đoán | `"Chưa đủ dữ liệu để AI dự đoán rủi ro cho thiết bị này."` |
| **Error** | Không nhận được dữ liệu IoT mới | `"Không có dữ liệu mới từ cảm biến — hiển thị lần cập nhật gần nhất có thể không chính xác."` *(không hiển thị dữ liệu cũ như thật)* |
| **Label** | Ghi chú | `"Dữ liệu mẫu MVP"` *(giữ nguyên, minh bạch với người dùng đây là dữ liệu demo)* |

---

## Flow 5 — Admin: Người dùng / IoT Mapping

| Loại | Trạng thái | Copy hiện tại / đề xuất |
| :--- | :--- | :--- |
| **CTA** | Thêm người dùng / Tạo mapping | `"Thêm người dùng"` / `"Tạo mapping"` *(giữ nguyên)* |
| **CTA** | Vô hiệu hoá | `"Vô hiệu hoá"` *(giữ nguyên — riêng dòng Admin đang đăng nhập nên disable nút này, đã đúng trên prototype)* |
| **Error** | Trùng mapping / thiếu field | `"Thiết bị này đã được liên kết với cảm biến khác."` / `"Chọn tài sản trước khi tạo mapping."` |
| **Empty** | Không còn tài sản chưa map | `"Tất cả tài sản đã được liên kết cảm biến."` |
| **Confirmation** | Tạo mapping / thêm user thành công | **Toast:** `"Đã tạo liên kết — [Tên tài sản]"` / `"Đã thêm người dùng — [Tên]"` |

---

## Nguyên tắc chung khi viết copy mới ở Figma

1. **Xưng hô:** Luôn xưng *"Bạn"* với Requester/Technician (giọng thân thiện, hướng dẫn), giữ giọng trung tính/ngắn gọn với FM/Admin (thao tác nghiệp vụ).
2. **Cấu trúc Error Message:** `Mô tả vấn đề` + `Hành động cụ thể để sửa` (không dùng *"Đã có lỗi xảy ra"* chung chung).
3. **Tính nhất quán:** Không dùng 2 nhãn khác nhau cho cùng 1 khái niệm nghiệp vụ *(bài học từ OQ-01)* — trước khi đặt tên trạng thái/badge mới, kiểm tra bảng badge ở `DESIGN.md` mục 1.
