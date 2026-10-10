# Hướng dẫn demo UI + Release Notes (phần UI/UX)

Phần 1 dán vào `docs/RUNBOOK.md` hoặc `README.md` (mục "Demo"). Phần 2 dán vào `RELEASE.md` / `CHANGELOG.md`. Mục tiêu của phần 1: một sinh viên khác đọc và tự chạy được demo, không cần hỏi tác giả.

## 1. Cách demo (4 flow)

**Chuẩn bị:** Mở `prototype_v3.html` bằng trình duyệt. Không có màn đăng nhập, đổi vai trò bằng dropdown "Vai trò" ở sidebar. Dữ liệu là dữ liệu mẫu MVP.

| Bước | Vai trò | Thao tác | Kết quả cần thấy |
|---|---|---|---|
| 1 | Requester | Menu "Yêu cầu sự cố" → chọn Vị trí → chọn Thiết bị → nhập mô tả → "Gửi yêu cầu" | Toast xác nhận, dòng mới nằm đầu bảng |
| 2 | Facility Manager | Work Order → khối "Phân công yêu cầu mới" → chọn yêu cầu và kỹ thuật viên → "Phân công" | Work Order mới xuất hiện, trạng thái "Đã phân công" |
| 3 | Technician | Work Order → "Bắt đầu" → nhập kết quả bảo trì → "Hoàn thành" | Trạng thái đổi sang "Đang thực hiện" rồi "Hoàn thành" |
| 4 | Technician | Dòng khác "Đã phân công" → nhập lý do → "Từ chối" | Dòng chuyển trạng thái. Lưu ý: prototype vẫn hiển thị "Đã hủy" (OQ-01), thiết kế đúng ở Figma là badge "Từ chối" |
| 5 | Facility Manager | Menu "IoT Alerts" rồi "AI Risks" | Bảng theo dõi, không có nút hành động (theo BR-10, AI chỉ hỗ trợ quyết định). Bấm tên thiết bị sẽ mở Tài sản → Chi tiết (theo bản prototype mới nhất của nhóm) |
| 6 | Facility Manager | Khối "Demo · Giả lập state" góc dưới phải → "empty" rồi "success" | Thấy state rỗng rồi khôi phục dữ liệu |

**Không bấm:** Nút "Từ chối" ở khối "Phân công yêu cầu mới" (chưa có hành vi, xem `BUG-UX-02`).

**Tài liệu thiết kế:** Figma [link] · `DESIGN.md` · `screen-inventory.md` · `ux-copy-table.md` · `HANDOFF.md`.

## 2. Release Notes — phần UI/UX (v1.0.0-final)

### Thay đổi

- Thống nhất màu thương hiệu `#23704D` (hover `#1B573C`) giữa Figma và prototype.
- Bộ design system trong Figma: Foundations (màu, chữ, khoảng cách, bo góc, đổ bóng) và Components (Button, Input, Dropdown, Badge, Card, Toast, Modal-Confirm, Empty state, Voice control).
- Bổ sung thiết kế badge trạng thái "Từ chối" (tách khỏi "Đã hủy") và Modal xác nhận đóng yêu cầu.
- Đổi nhãn nút FM thành "Từ chối yêu cầu" trong thiết kế.
- Dropdown "Vị trí" (Flow 1) hiển thị danh sách vị trí; bấm tên thiết bị ở IoT Alerts/AI Risks chuyển sang trang Tài sản → Chi tiết của đúng thiết bị.
- Gỡ ghi chú OQ-01 (ghi sai mã quyết định) khỏi màn hình Technician.

### Known issues

- Nút "Từ chối" khối phân công (FM) chưa có hành vi.
- Logic prototype/code chưa cập nhật theo thiết kế "Từ chối" và Modal-Confirm [xác nhận].
- Thiếu state loading/error, chưa có responsive mobile, chưa kiểm accessibility thực đo.
- Voice control là giả định, chưa dùng trong luồng nào của MVP.

### Ghi chú nâng cấp

Từ bản trước: cập nhật `DESIGN.md` cho khớp `#23704D` trước khi dev dùng làm nguồn token.
