# REQUIREMENT QA

**Smart Campus Property & Facility Management System**
**Trường Đại học Kinh tế - Đại học Đà Nẵng**

## Mục đích
Tài liệu này rà soát từng Functional Requirement đã được xác nhận để đánh giá mức độ sẵn sàng cho triển khai và kiểm thử. Bằng chứng được đối chiếu từ Functional Requirements, Business Rules, Non-functional Requirements và Open Questions của dự án.
- **ANSWERED** nghĩa là tài liệu hiện tại đã cung cấp câu trả lời. 
- **OPEN** nghĩa là thông tin triển khai/kiểm thử còn thiếu hoặc chưa rõ; nội dung trong ô là câu hỏi cần làm rõ.

---

## FR-001 — Đăng nhập hệ thống

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Các đối tượng là Customer, Technician, Facility Manager và Administrator (FR-001). |
| **Trigger** | OPEN | Thao tác hoặc sự kiện cụ thể nào bắt đầu luồng đăng nhập? |
| **Preconditions** | ANSWERED | Người dùng phải có tài khoản hợp lệ của nhà trường và thuộc một vai trò được phép truy cập (NFR-001, BR-011). |
| **Main flow** | OPEN | Các bước đăng nhập bắt buộc và điều kiện xác định đăng nhập thành công là gì? |
| **Alternative flow** | OPEN | Có yêu cầu luồng đăng nhập thay thế hoặc khôi phục tài khoản hay không? |
| **Error flow** | OPEN | Hệ thống phải phản hồi thế nào khi sai thông tin đăng nhập, tài khoản không hợp lệ hoặc dịch vụ xác thực lỗi? |
| **Validation** | OPEN | Các trường đăng nhập và quy tắc kiểm tra dữ liệu đầu vào cụ thể là gì? |
| **Business rules** | ANSWERED | Mô hình vai trò và phạm vi quyền truy cập được quy định tại BR-011. |
| **Permissions** | ANSWERED | Mỗi người dùng chỉ sử dụng các chức năng tương ứng với Customer, Technician, Facility Manager hoặc Administrator (BR-011). |
| **Data requirements** | OPEN | Cần lưu những dữ liệu nào về tài khoản, vai trò và phiên đăng nhập? |
| **Edge cases** | OPEN | Xử lý thế nào với đăng nhập lặp lại, phiên hết hạn, tài khoản bị khóa hoặc người dùng đã đăng nhập? |
| **Concurrency** | OPEN | Có cho phép nhiều phiên đăng nhập đồng thời cho cùng một tài khoản hay không? |
| **Security** | ANSWERED | Hệ thống phải xác thực người dùng, phân quyền theo vai trò và dùng kết nối mã hóa TLS (NFR-005). |

## FR-002 — Gửi yêu cầu báo sự cố tài sản

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Customer gồm giảng viên, sinh viên và nhân viên trong trường (FR-002). |
| **Trigger** | ANSWERED | Luồng bắt đầu khi Customer phát hiện tài sản có sự cố và muốn gửi yêu cầu (FR-002, BR-003). |
| **Preconditions** | ANSWERED | Customer phải đăng nhập và chọn một tài sản Wifi, điều hòa hoặc máy chiếu, đèn và quạt đang được quản lý (NFR-001, BR-002, BR-003). |
| **Main flow** | ANSWERED | Customer chọn tài sản, nhập mô tả vấn đề và gửi yêu cầu báo sự cố (FR-002, BR-003). |
| **Alternative flow** | OPEN | Có cho phép Customer lưu nháp, sửa hoặc hủy yêu cầu trước khi gửi hay không? |
| **Error flow** | OPEN | Hệ thống phải xử lý thế nào khi tài sản không tồn tại, dữ liệu thiếu hoặc việc lưu yêu cầu thất bại? |
| **Validation** | ANSWERED | Yêu cầu phải gắn với một tài sản cụ thể và có mô tả vấn đề (BR-003). |
| **Business rules** | ANSWERED | Customer phải chọn tài sản cụ thể; Customer chỉ được xem các yêu cầu do mình tạo (BR-003). |
| **Permissions** | ANSWERED | Customer được tạo yêu cầu báo sự cố và chỉ truy cập các yêu cầu của chính mình (BR-003, BR-011). |
| **Data requirements** | OPEN | Ngoài Asset ID và mô tả vấn đề, các trường bắt buộc khác như thời gian, mức độ ưu tiên hoặc tệp đính kèm chưa được xác định. |
| **Edge cases** | OPEN | Xử lý thế nào khi nhiều Customer cùng báo một sự cố cho cùng tài sản hoặc gửi yêu cầu trùng lặp? |
| **Concurrency** | OPEN | Cách xử lý nhiều yêu cầu được gửi đồng thời cho cùng một tài sản chưa được quy định. |
| **Security** | ANSWERED | Customer phải được xác thực và chỉ xem dữ liệu thuộc yêu cầu của mình (NFR-005, BR-003). |

## FR-003 — Theo dõi trạng thái yêu cầu

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Customer (FR-003). |
| **Trigger** | OPEN | Hành động cụ thể nào mở danh sách hoặc chi tiết yêu cầu của Customer? |
| **Preconditions** | ANSWERED | Customer phải đăng nhập và yêu cầu phải do chính Customer đó tạo (BR-003, NFR-001). |
| **Main flow** | ANSWERED | Hệ thống hiển thị các yêu cầu do Customer tạo và trạng thái xử lý tương ứng (FR-003). |
| **Alternative flow** | OPEN | Hệ thống hiển thị gì khi Customer chưa có yêu cầu nào? |
| **Error flow** | OPEN | Hệ thống xử lý thế nào khi không tải được dữ liệu hoặc yêu cầu không còn tồn tại? |
| **Validation** | OPEN | Các bộ lọc, sắp xếp hoặc tham số tra cứu trạng thái chưa được xác định. |
| **Business rules** | ANSWERED | Customer chỉ được xem các yêu cầu do chính mình tạo (BR-003). |
| **Permissions** | ANSWERED | Customer không được xem yêu cầu của Customer khác (BR-003, NFR-005). |
| **Data requirements** | OPEN | Các trường phải hiển thị ngoài nội dung yêu cầu và trạng thái xử lý chưa được xác định đầy đủ. |
| **Edge cases** | OPEN | Hiển thị thế nào khi yêu cầu đang thay đổi trạng thái đúng lúc Customer xem? |
| **Concurrency** | OPEN | Cách đồng bộ giao diện khi Technician hoặc Facility Manager cập nhật yêu cầu đồng thời chưa được quy định. |
| **Security** | ANSWERED | Quyền xem dữ liệu được giới hạn theo người tạo yêu cầu và vai trò (NFR-005). |

## FR-004 — Quản lý tài sản

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor chính là Facility Manager (FR-004). |
| **Trigger** | OPEN | Hành động cụ thể nào bắt đầu luồng thêm, cập nhật hoặc xem tài sản? |
| **Preconditions** | ANSWERED | Facility Manager phải đăng nhập và có quyền quản lý tài sản (BR-011, NFR-005). |
| **Main flow** | ANSWERED | Facility Manager quản lý danh sách Wifi, điều hòa, máy chiếu, đèn và quạt theo Asset ID, loại, vị trí, trạng thái và xem lịch sử bảo trì (FR-004). |
| **Alternative flow** | OPEN | Có cho phép vô hiệu hóa, khôi phục hoặc xóa tài sản trong MVP hay không? |
| **Error flow** | OPEN | Hệ thống xử lý thế nào khi Asset ID bị trùng, dữ liệu không hợp lệ hoặc lưu thất bại? |
| **Validation** | ANSWERED | Asset ID phải duy nhất; tài sản thuộc một trong năm nhóm MVP và phải gắn với vị trí cụ thể (BR-001, BR-002). |
| **Business rules** | ANSWERED | Định danh, thông tin tối thiểu, phạm vi tài sản và vị trí được quy định tại BR-001 và BR-002. |
| **Permissions** | ANSWERED | Facility Manager được quản lý tài sản; quyền truy cập tổng thể được kiểm soát theo BR-011 và NFR-005. |
| **Data requirements** | ANSWERED | Tối thiểu gồm Asset ID, tên, loại tài sản, vị trí, trạng thái và lịch sử sự cố/bảo trì (FR-004, BR-001, BR-010). |
| **Edge cases** | OPEN | Cách xử lý tài sản chuyển vị trí, ngừng sử dụng hoặc có dữ liệu lịch sử nhưng bị thay đổi thông tin chưa được quy định. |
| **Concurrency** | OPEN | Cách xử lý khi nhiều người cập nhật cùng một tài sản tại cùng thời điểm chưa được quy định. |
| **Security** | ANSWERED | Thao tác quản lý tài sản phải theo phân quyền và được ghi nhật ký khi có thay đổi quan trọng (NFR-005, NFR-006). |

## FR-005 — Giám sát thiết bị qua IoT

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Các actor là System, Facility Manager và Technician (FR-005). |
| **Trigger** | ANSWERED | Luồng được kích hoạt khi thiết bị IoT gửi dữ liệu hoạt động hoặc dữ liệu đạt điều kiện bất thường (FR-005, BR-007). |
| **Preconditions** | ANSWERED | Dữ liệu IoT phải gắn với đúng Asset ID và thời điểm ghi nhận; dữ liệu không xác định được tài sản không dùng cho AI (BR-006). |
| **Main flow** | ANSWERED | Hệ thống nhận và hiển thị dữ liệu IoT; khi phát hiện bất thường, tạo cảnh báo cho Facility Manager (FR-005, BR-007). |
| **Alternative flow** | ANSWERED | Khi IoT mất kết nối, tài sản vẫn được quản lý và Customer vẫn có thể báo lỗi thủ công; hệ thống phải hiển thị dữ liệu không còn cập nhật (BR-006, NFR-007). |
| **Error flow** | OPEN | Cách xử lý dữ liệu IoT sai định dạng, lỗi gateway hoặc lỗi lưu trữ chưa được quy định. |
| **Validation** | ANSWERED | Mỗi bản ghi IoT phải có Asset ID phù hợp và thời điểm ghi nhận; dữ liệu không gắn được tài sản không dùng cho AI (BR-006, NFR-007). |
| **Business rules** | ANSWERED | Quy tắc dữ liệu IoT và cảnh báo bất thường được quy định tại BR-006 và BR-007. |
| **Permissions** | ANSWERED | Facility Manager theo dõi cảnh báo; Technician chỉ xem dữ liệu liên quan đến Work Order được giao (FR-005, BR-011). |
| **Data requirements** | OPEN | Các chỉ số IoT cụ thể cho Wifi, điều hòa và máy chiếu chưa được xác định đầy đủ. |
| **Edge cases** | ANSWERED | Trường hợp mất kết nối hoặc dữ liệu quá cũ được đánh dấu không còn cập nhật và không ngăn báo lỗi thủ công (NFR-007). |
| **Concurrency** | OPEN | Cách xử lý lượng lớn bản ghi IoT đến đồng thời hoặc bản ghi đến không đúng thứ tự chưa được xác định. |
| **Security** | OPEN | Cơ chế xác thực thiết bị IoT, mã hóa kênh thiết bị và quản lý khóa/chứng chỉ chưa được xác định; NFR-005 mới quy định bảo mật truy cập người dùng. |

## FR-006 — AI dự đoán sự cố và nhu cầu bảo trì

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là AI System và Facility Manager (FR-006). |
| **Trigger** | OPEN | Thời điểm AI chạy dự đoán theo lịch, theo dữ liệu IoT mới hay theo yêu cầu của Facility Manager chưa được xác định. |
| **Preconditions** | OPEN | AI cần dữ liệu IoT và lịch sử bảo trì, nhưng lượng dữ liệu tối thiểu và tiêu chí đủ dữ liệu chưa được xác định (FR-006, BR-008). |
| **Main flow** | ANSWERED | AI phân tích dữ liệu IoT cùng lịch sử bảo trì và hiển thị tài sản, mức rủi ro, khuyến nghị cho Facility Manager (FR-006, BR-008, NFR-008). |
| **Alternative flow** | OPEN | Hệ thống phải làm gì khi dữ liệu không đủ để AI đưa ra dự đoán đáng tin cậy? |
| **Error flow** | OPEN | Cách xử lý khi mô hình AI lỗi, không phản hồi hoặc kết quả không hợp lệ chưa được quy định. |
| **Validation** | ANSWERED | Kết quả AI tối thiểu phải có tài sản liên quan, mức rủi ro và khuyến nghị kiểm tra/bảo trì (BR-008, NFR-008). |
| **Business rules** | ANSWERED | AI dùng dữ liệu IoT và lịch sử bảo trì để dự đoán; AI chỉ hỗ trợ quyết định và không tự tạo quyết định cuối cùng (BR-008, BR-009). |
| **Permissions** | ANSWERED | Facility Manager là người xem xét kết quả AI và quyết định xử lý (BR-009, NFR-008). |
| **Data requirements** | ANSWERED | Đầu vào gồm dữ liệu IoT và lịch sử bảo trì; đầu ra tối thiểu gồm tài sản, mức rủi ro và khuyến nghị (BR-008). |
| **Edge cases** | OPEN | Cách xử lý dữ liệu thiếu, nhiễu, bất thường hoặc dự đoán rủi ro thấp/cao sát ngưỡng chưa được xác định. |
| **Concurrency** | OPEN | Cách xử lý nhiều dự đoán đồng thời cho cùng tài sản hoặc dự đoán khi dữ liệu đang cập nhật chưa được xác định. |
| **Security** | ANSWERED | AI chỉ hỗ trợ quyết định, không tự phân công Technician hoặc tự đóng Work Order; quyền truy cập kết quả tuân theo phân quyền (NFR-005, NFR-008). |

## FR-007 — Tạo và phân công Work Order

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Facility Manager (FR-007). |
| **Trigger** | ANSWERED | Work Order có thể được tạo từ yêu cầu Customer, cảnh báo IoT hoặc dự đoán bảo trì của AI (FR-007, BR-004). |
| **Preconditions** | ANSWERED | Facility Manager phải đăng nhập; Work Order phải gắn với một tài sản và Technician được phân công phải là người dùng phù hợp (BR-004, BR-011). |
| **Main flow** | ANSWERED | Facility Manager chọn nguồn yêu cầu/cảnh báo, tạo Work Order cho tài sản và phân công Technician (FR-007, BR-004). |
| **Alternative flow** | OPEN | Cách xử lý khi chưa có Technician phù hợp hoặc Facility Manager muốn tạo Work Order nhưng chưa phân công ngay chưa được xác định. |
| **Error flow** | OPEN | Hệ thống phải xử lý thế nào khi tài sản/Technician không tồn tại hoặc việc tạo Work Order thất bại? |
| **Validation** | ANSWERED | Chỉ Facility Manager được tạo và phân công Work Order; mỗi Work Order phải gắn với một tài sản (BR-004). |
| **Business rules** | ANSWERED | Quy tắc tạo/phân công và vòng đời Work Order được quy định tại BR-004 và BR-005. |
| **Permissions** | ANSWERED | Chỉ Facility Manager được tạo Work Order và phân công Technician (BR-004, BR-011). |
| **Data requirements** | OPEN | Ngoài tài sản, nguồn phát sinh, Technician và trạng thái, các trường bắt buộc của Work Order chưa được xác định đầy đủ. |
| **Edge cases** | OPEN | Cách xử lý Work Order trùng cho cùng sự cố, thay đổi Technician hoặc hủy Work Order chưa được quy định. |
| **Concurrency** | OPEN | Cách xử lý hai Facility Manager đồng thời tạo hoặc phân công Work Order cho cùng sự cố chưa được quy định. |
| **Security** | ANSWERED | Việc tạo/phân công phải theo vai trò và được ghi nhật ký thay đổi quan trọng (NFR-005, NFR-006). |

## FR-008 — Xem Work Order được giao

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Technician (FR-008). |
| **Trigger** | OPEN | Hành động cụ thể nào mở danh sách Work Order được giao chưa được mô tả. |
| **Preconditions** | ANSWERED | Technician phải đăng nhập và Work Order phải được phân công cho Technician đó (BR-011, NFR-005). |
| **Main flow** | ANSWERED | Hệ thống hiển thị danh sách Work Order được giao cùng thông tin tài sản, sự cố và dữ liệu IoT/AI liên quan nếu có (FR-008). |
| **Alternative flow** | OPEN | Hệ thống hiển thị gì khi Technician chưa có Work Order được giao? |
| **Error flow** | OPEN | Cách xử lý khi không tải được Work Order hoặc dữ liệu tài sản liên quan bị thiếu chưa được quy định. |
| **Validation** | OPEN | Bộ lọc, sắp xếp, phân trang hoặc tiêu chí hiển thị Work Order chưa được xác định. |
| **Business rules** | ANSWERED | Technician chỉ được xem Work Order được phân công; việc phân công do Facility Manager thực hiện (BR-004, BR-011). |
| **Permissions** | ANSWERED | Technician chỉ truy cập Work Order được giao cho mình (BR-011, NFR-005). |
| **Data requirements** | ANSWERED | Danh sách tối thiểu gồm thông tin Work Order, tài sản, nội dung sự cố và dữ liệu IoT/AI liên quan nếu có (FR-008). |
| **Edge cases** | OPEN | Xử lý thế nào khi Work Order bị chuyển cho Technician khác trong lúc đang xem? |
| **Concurrency** | OPEN | Cách làm mới danh sách khi Work Order được phân công/thay đổi đồng thời chưa được quy định. |
| **Security** | ANSWERED | Quyền truy cập Work Order được giới hạn theo Technician được phân công (NFR-005, BR-011). |

## FR-009 — Cập nhật kết quả kiểm tra và sửa chữa

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Technician (FR-009). |
| **Trigger** | ANSWERED | Luồng bắt đầu khi Technician xử lý Work Order được giao và cập nhật tiến độ/kết quả (FR-009). |
| **Preconditions** | ANSWERED | Technician phải đăng nhập và Work Order phải được phân công cho Technician đó (BR-005, BR-011). |
| **Main flow** | ANSWERED | Technician cập nhật tiến độ, kết quả kiểm tra/sửa chữa; khi hoàn thành, thông tin xử lý được lưu vào lịch sử bảo trì (FR-009, BR-010). |
| **Alternative flow** | OPEN | Có cho phép Technician chuyển Work Order về trạng thái trước hoặc yêu cầu hỗ trợ/chuyển người xử lý hay không? |
| **Error flow** | OPEN | Hệ thống xử lý thế nào khi cập nhật trạng thái không hợp lệ hoặc việc lưu kết quả thất bại? |
| **Validation** | ANSWERED | Trạng thái Work Order theo NEW → IN_PROGRESS → COMPLETED → CLOSED; Technician chỉ chuyển nhiệm vụ được giao sang IN_PROGRESS và COMPLETED (BR-005). |
| **Business rules** | ANSWERED | Vòng đời Work Order và lưu lịch sử bảo trì được quy định tại BR-005 và BR-010. |
| **Permissions** | ANSWERED | Technician chỉ cập nhật Work Order được phân công; Facility Manager kiểm tra trước khi CLOSED (BR-005, BR-011). |
| **Data requirements** | ANSWERED | Khi đóng Work Order phải lưu Asset ID, sự cố, Technician, nội dung xử lý, thời gian và kết quả (BR-010). |
| **Edge cases** | OPEN | Cách xử lý cập nhật lặp, Work Order đã CLOSED hoặc kết quả sửa chữa chưa hoàn tất chưa được quy định. |
| **Concurrency** | OPEN | Cách xử lý khi Technician và Facility Manager cập nhật cùng Work Order tại cùng thời điểm chưa được xác định. |
| **Security** | ANSWERED | Thay đổi Work Order phải tuân theo phân quyền và được ghi nhật ký (NFR-005, NFR-006). |

## FR-010 — Quản lý tài khoản và quyền truy cập

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Administrator (FR-010). |
| **Trigger** | OPEN | Hành động cụ thể nào bắt đầu luồng tạo, sửa, khóa tài khoản hoặc thay đổi vai trò chưa được xác định. |
| **Preconditions** | ANSWERED | Người thực hiện phải đăng nhập với vai trò Administrator (BR-011, NFR-005). |
| **Main flow** | OPEN | Các bước quản lý vòng đời tài khoản và thay đổi vai trò chưa được mô tả chi tiết. |
| **Alternative flow** | OPEN | Cách xử lý yêu cầu thay đổi vai trò không làm thay đổi dữ liệu hiện tại hoặc tài khoản đã bị khóa chưa được quy định. |
| **Error flow** | OPEN | Cách xử lý tài khoản không tồn tại, vai trò không hợp lệ hoặc lỗi lưu thay đổi chưa được quy định. |
| **Validation** | OPEN | Quy tắc về tự thay đổi quyền, xóa Administrator cuối cùng hoặc điều kiện gán từng vai trò chưa được xác định. |
| **Business rules** | ANSWERED | Mô hình bốn vai trò và phạm vi quyền truy cập được quy định tại BR-011. |
| **Permissions** | ANSWERED | Administrator quản lý tài khoản, vai trò và cấu hình hệ thống (BR-011). |
| **Data requirements** | OPEN | Các trường tài khoản, trạng thái tài khoản, lịch sử vai trò và dữ liệu phiên chưa được xác định đầy đủ. |
| **Edge cases** | OPEN | Cách xử lý tự hạ quyền, xóa/khóa Administrator cuối cùng hoặc gán nhiều vai trò chưa được quy định. |
| **Concurrency** | OPEN | Cách xử lý nhiều Administrator đồng thời thay đổi quyền của cùng một tài khoản chưa được xác định. |
| **Security** | ANSWERED | Hệ thống phải xác thực, phân quyền theo vai trò và ghi nhật ký thay đổi quyền người dùng (NFR-005, NFR-006). |

## FR-011 — Quản lý lịch trình bảo trì tài sản

| Check | Status | Evidence or question |
| :--- | :--- | :--- |
| **Actor** | ANSWERED | Actor là Facility Manager (FR-011). |
| **Trigger** | ANSWERED | Facility Manager tạo/cập nhật lịch bảo trì định kỳ hoặc xem xét khuyến nghị bảo trì từ AI (FR-011). |
| **Preconditions** | ANSWERED | Facility Manager phải đăng nhập; tài sản phải tồn tại; nếu lịch dựa trên AI thì Facility Manager phải xác nhận (FR-011, BR-009). |
| **Main flow** | ANSWERED | Facility Manager tạo, xem hoặc cập nhật lịch với tài sản, thời gian dự kiến và nội dung bảo trì (FR-011). |
| **Alternative flow** | ANSWERED | Lịch có thể được tạo theo kế hoạch định kỳ hoặc dựa trên khuyến nghị bảo trì của AI, nhưng quyết định cuối cùng thuộc Facility Manager (FR-011, BR-009). |
| **Error flow** | OPEN | Cách xử lý khi tài sản không tồn tại, thời gian không hợp lệ hoặc lưu lịch thất bại chưa được quy định. |
| **Validation** | ANSWERED | Mỗi lịch phải có tài sản, thời gian dự kiến và nội dung bảo trì; lịch dựa trên AI phải được Facility Manager xác nhận (FR-011). |
| **Business rules** | OPEN | BR-008 và BR-009 điều chỉnh việc dùng khuyến nghị AI, nhưng chưa có Business Rule riêng cho chu kỳ, trùng lịch hoặc thay đổi lịch bảo trì. |
| **Permissions** | ANSWERED | Facility Manager được tạo, xem và cập nhật lịch trình bảo trì (FR-011, BR-011). |
| **Data requirements** | ANSWERED | Dữ liệu tối thiểu gồm tài sản, thời gian dự kiến và nội dung bảo trì (FR-011). |
| **Edge cases** | OPEN | Cách xử lý lịch trùng nhau, lịch quá hạn, đổi ngày hoặc tài sản ngừng sử dụng chưa được quy định. |
| **Concurrency** | OPEN | Cách xử lý khi hai người đồng thời cập nhật cùng một lịch bảo trì chưa được quy định. |
| **Security** | ANSWERED | Việc quản lý lịch phải theo quyền Facility Manager và các thay đổi quan trọng cần có khả năng truy vết (NFR-005, NFR-006). |
