# Bảng Kiểm Định Trí Tuệ Nhân Tạo (Vault Q&A Benchmark)

**Mục tiêu:** Kiểm thử khả năng đọc hiểu và trích xuất thông tin của AI dựa trên kho tài liệu Project Vault. Đảm bảo AI không bị "ảo giác" (hallucination) khi gặp dữ liệu chưa được định nghĩa.

## 1. Bảng 20 Câu Hỏi Đánh Giá (Benchmark Q&A)

| STT | Loại | Câu hỏi test AI (Prompt) | Expected Answer (Câu trả lời kỳ vọng) & Nguồn | AI Trả lời | Đánh giá |
| :--- | :--- | :--- | :--- | :--- | :--- |
| 1 | **Fact** | Trong giai đoạn MVP, hệ thống quản lý các nhóm tài sản nào? | Wifi, điều hòa, máy chiếu và đèn, quạt (BR-002). | *(Chờ kiểm chứng)* | |
| 2 | **Fact** | Work Order có những trạng thái nào trong vòng đời của nó? | NEW, IN_PROGRESS, COMPLETED, CLOSED (BR-005). | *(Chờ kiểm chứng)* | |
| 3 | **Fact** | Lịch sử bảo trì của tài sản phải được lưu trữ trong thời gian tối thiểu là bao lâu? | Tối thiểu 2 năm (NFR-010). | *(Chờ kiểm chứng)* | |
| 4 | **Fact** | Mục tiêu thời gian khôi phục dữ liệu (RTO) tối đa sau sự cố nghiêm trọng là bao lâu? | Tối đa 4 giờ (NFR-009). | *(Chờ kiểm chứng)* | |
| 5 | **Fact** | Có bao nhiêu vai trò người dùng trong hệ thống? | 4 vai trò: Customer, Technician, Facility Manager, Administrator (BR-011). | *(Chờ kiểm chứng)* | |
| 6 | **Rule** | Ai là người có quyền tạo và phân công Work Order? | Chỉ Facility Manager (BR-004 / FR-007). | *(Chờ kiểm chứng)* | |
| 7 | **Rule** | Giảng viên báo sự cố xong có được xem danh sách yêu cầu của sinh viên khác không? | Không. Customer chỉ được xem các yêu cầu do chính mình tạo (BR-003). | *(Chờ kiểm chứng)* | |
| 8 | **Rule** | Hệ thống AI có quyền tự động đóng Work Order khi phát hiện thiết bị đã bình thường không? | Không. AI chỉ hỗ trợ quyết định, không tự động đóng Work Order (BR-009). | *(Chờ kiểm chứng)* | |
| 9 | **Rule** | Cảnh báo từ thiết bị IoT có tự động xác nhận sự cố mà không cần con người không? | Không. Cảnh báo IoT không tự động đóng hoặc xác nhận sự cố (BR-007). | *(Chờ kiểm chứng)* | |
| 10 | **Rule** | Người dùng có vai trò Facility Manager có được phân quyền cho tài khoản khác không? | Không. Chỉ Administrator mới quản lý tài khoản và phân quyền (BR-011 / FR-010). | *(Chờ kiểm chứng)* | |
| 11 | **Edge** | Nếu thiết bị IoT bị mất kết nối mạng, sinh viên có thể báo lỗi thủ công được không? | Có. Tài sản vẫn có thể được báo lỗi thủ công (BR-006 / NFR-007). | *(Chờ kiểm chứng)* | |
| 12 | **Edge** | Chuyện gì xảy ra nếu Facility Manager tạo tài sản mới nhưng trùng Asset ID đã có? | Hệ thống từ chối tạo tài sản mới vì Asset ID phải duy nhất (BR-001 / AC-US004-02). | *(Chờ kiểm chứng)* | |
| 13 | **Edge** | Technician có được phép cập nhật kết quả sửa chữa cho Work Order của người khác không? | Hệ thống sẽ từ chối thao tác cập nhật của Technician đó (AC-US009-03). | *(Chờ kiểm chứng)* | |
| 14 | **Edge** | Khách hàng chọn tài sản nhưng để trống mô tả sự cố thì gửi yêu cầu được không? | Hệ thống từ chối tạo yêu cầu báo sự cố (AC-US002-03). | *(Chờ kiểm chứng)* | |
| 15 | **Edge** | Nếu Facility Manager tạo lịch bảo trì bị trùng thời gian với lịch đã có sẵn thì sao? | Hệ thống phát hiện xung đột và thông báo để điều chỉnh thời gian (AC-US011-05). | *(Chờ kiểm chứng)* | |
| 16 | **Unknown**| Ngân sách bảo trì tối đa cho mỗi chiếc máy chiếu trong một năm là bao nhiêu? | Chưa được định nghĩa / Không có thông tin trong tài liệu. | *(Chờ kiểm chứng)* | |
| 17 | **Unknown**| Hãng sản xuất của các cảm biến IoT dùng cho điều hòa là hãng nào? | Chưa được định nghĩa / Không có thông tin trong tài liệu. | *(Chờ kiểm chứng)* | |
| 18 | **Unknown**| Nếu Technician không hoàn thành sửa chữa đúng hạn (SLA) thì bị phạt bao nhiêu tiền? | Chưa được định nghĩa / Không có thông tin trong tài liệu. | *(Chờ kiểm chứng)* | |
| 19 | **Unknown**| Hệ thống cho phép có tối đa bao nhiêu tài khoản Administrator cùng hoạt động? | Chưa được định nghĩa (Open Question mục US-010). | *(Chờ kiểm chứng)* | |
| 20 | **Unknown**| Khi Technician chuyển Work Order sang COMPLETED, có bắt buộc đính kèm ảnh không? | Chưa được định nghĩa (Undefined data US-009). | *(Chờ kiểm chứng)* | |

---

