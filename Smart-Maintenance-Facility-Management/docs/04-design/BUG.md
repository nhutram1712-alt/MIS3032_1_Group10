# Bug Log — UI/UX (Smart Campus Facility Management)

## 1. Danh sách bug

| ID | Severity | Mô tả | Các bước tái hiện | Expected | Actual | Evidence | Status |
| --- | --- | --- | --- | --- | --- | --- | --- |
| BUG-UX-01 | Major | Màu brand không đồng bộ giữa 3 nguồn | So sánh sidebar prototype, Figma, DESIGN.md | Cùng một mã #23704D | Prototype #1E3B2C, DESIGN.md #2F5C41, Figma #23704D | Ảnh swatch Figma + dòng CSS --brand [link commit] | Fixed ở prototype (--brand:#23704D, hover #1B573C). DESIGN.md và các docs: Open |
| BUG-UX-02 | Major | Nút "Từ chối" ở khối "Phân công yêu cầu mới" (FM) không có hành vi | Vai trò FM → Work Order → bấm "Từ chối" | Từ chối yêu cầu đã chọn, có xác nhận/lý do | Không có phản ứng gì (thiếu handler) | [ảnh/video ngắn] | Open |
| BUG-UX-03 | Critical | Thiếu UI xác nhận kết quả và đóng yêu cầu (BR-18); Task 2b thất bại | Vai trò FM → tìm cách đóng yêu cầu sau khi WO hoàn thành | Có bước xác nhận rồi mới Closed | Không có thao tác nào để đóng | Prototype.docx, Task 2b | Fixed ở thiết kế (Modal-Confirm trong Figma). Implement: Open |
| BUG-UX-04 | Minor | Ghi chú trên UI ghi sai mã quyết định (DEC-01, đúng là DEC-09) | Technician → Work Order, cuộn xuống dưới bảng | Mã khớp Decision Log | Ghi "DEC-01" | Ảnh chụp ghi chú | Closed (đã gỡ ghi chú ở prototype_v3.html) |
| BUG-UX-05 | Major | Flow 1: chọn "Vị trí" chưa hiển thị đầy đủ danh sách vị trí trong trường | Requester → Yêu cầu sự cố → bấm ô Vị trí | Hiện danh sách các vị trí có trong hệ thống để chọn | Danh sách thiếu/không đủ | [ảnh trước/sau] | Fixed: dropdown hiển thị đầy đủ danh sách vị trí trong hệ thống để chọn trực tiếp. [bổ sung evidence] |
| BUG-UX-06 | Major | Q-24: IoT Alerts/AI Risks không liên kết tới chi tiết thiết bị | FM → IoT Alerts → bấm tên thiết bị | Mở chi tiết thiết bị đó | Chỉ là chữ, không thao tác được |  | Fixed: bấm tên thiết bị chuyển sang trang Tài sản → Chi tiết của đúng thiết bị. [bổ sung evidence] |
| BUG-UX-07 | Minor | Flow 1: người dùng không chắc đã gửi thành công (toast ngắn) | Gửi một yêu cầu, quan sát phản hồi | Phản hồi đủ rõ để biết đã gửi | Thanh Thảo phải cuộn xuống bảng để kiểm tra | Prototype.docx, Flow 1 | Open (đề xuất: highlight dòng mới 2 giây, xem ux-copy-table.md) |
| BUG-UX-08 | Major | Thiếu state loading và error trên hầu hết màn hình | Xem state matrix trong screen-inventory.md | Có đủ default/loading/empty/error/confirmation/permission | Chỉ có default, một phần empty/confirmation | screen-inventory.md | Open (known issue, đã đặc tả copy, chưa dựng) |
| BUG-UX-09 | Minor | Nhãn nút "Từ chối" mơ hồ khi đặt cạnh "Phân công" (Q-22) | FM → Work Order, xem khối phân công | Nhãn nói rõ phạm vi hành động | "Từ chối" không rõ từ chối gì | Prototype.docx, Q-22 | Fixed ở thiết kế (đổi thành "Từ chối yêu cầu"). Prototype: Open |
| BUG-UX-10 | Minor | Prototype.docx ghi "Tuấn" thay vì "Trầm" ở bảng tỷ lệ hoàn thành | Mở bảng completion rate | Cùng tên "Trầm" như bảng tester | Header cột ghi "Tuấn" | Ảnh trang tài liệu | Open (sửa tay trong file Word) |

## 2. Regression test cho các bug đã fix

| Bug | Test | Kết quả mong đợi |
| --- | --- | --- |
| BUG-UX-01 | Mở prototype_v3.html, kiểm tra sidebar và nút primary bằng DevTools; tìm #1E3B2C trong file | Màu = #23704D, hover = #1B573C, không còn mã cũ |
| BUG-UX-05 | Chọn vai trò Technician → Work Order, cuộn hết trang | Không còn khối ghi chú OQ-01/DEC-01 |
| BUG-UX-06 | Requester → bấm ô Vị trí, đếm số lựa chọn so với dữ liệu | Số lựa chọn khớp danh sách vị trí trong dữ liệu |
| BUG-UX-07 | FM → IoT Alerts và AI Risks → bấm từng tên thiết bị | Mỗi dòng mở trang Tài sản → Chi tiết của đúng thiết bị tương ứng |

## 3. Tổng kết

- Tổng: 10 bug. Critical 2, Major 5, Minor 4.
- Đã fix hoàn toàn: 2 (BUG-UX-01 ở prototype, BUG-UX-05). Fix ở thiết kế, chờ code: 3. Đã fix, chờ bổ sung evidence (ảnh trước/sau): 2. Còn open: 4.
- Cập nhật lại bảng này sau mỗi lần fix, kèm link commit/PR.
