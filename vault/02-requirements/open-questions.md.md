

## **Q-001 — Phương thức đăng nhập**

\- ID: Q-001

\- Tên: Phương thức đăng nhập

\- Mô tả: Người dùng đăng nhập hệ thống như thế nào?

\- Answer: Customer, Technician, Facility Manager và Administrator sử dụng tài khoản được nhà trường cấp để truy cập hệ thống.

\- Actor: Customer; Technician; Facility Manager; Administrator

\- Priority: Không quy định

\- Source: FR-001; NFR-001

\- Status: ANSWERED

## **Q-002 — Quyền của Customer khi báo sự cố**

\- ID: Q-002

\- Tên: Quyền của Customer khi báo sự cố

\- Mô tả: Giảng viên, sinh viên và nhân viên trong trường có thể thực hiện những thao tác nào?

\- Answer: Customer có thể chọn tài sản Wifi, điều hòa, máy chiếu, đèn hoặc quạt, gửi yêu cầu báo sự cố và theo dõi trạng thái xử lý của yêu cầu do mình tạo.

\- Actor: Customer

\- Priority: Không quy định

\- Source: FR-002; FR-003; BR-003

\- Status: ANSWERED

## **Q-003 — Quy trình xử lý và phân công bảo trì**

\- ID: Q-003

\- Tên: Quy trình xử lý và phân công bảo trì

\- Mô tả: Ai chịu trách nhiệm xử lý yêu cầu bảo trì sau khi phát hiện sự cố?

\- Answer: Facility Manager kiểm tra yêu cầu, tạo Work Order và phân công Technician xử lý. Technician cập nhật kết quả sau khi hoàn thành.

\- Actor: Facility Manager; Technician

\- Priority: Tối quan trọng

\- Source: FR-007; FR-008; FR-009; BR-004; BR-005

\- Status: ANSWERED

## **Q-004 — Vai trò của IoT và AI trong bảo trì**

\- ID: Q-004

\- Tên: Vai trò của IoT và AI trong bảo trì

\- Mô tả: Dữ liệu IoT và AI được sử dụng như thế nào trong hệ thống?

\- Answer: IoT thu thập dữ liệu hoạt động của thiết bị. AI phân tích dữ liệu IoT kết hợp lịch sử bảo trì để dự đoán thiết bị có nguy cơ sự cố hoặc cần bảo trì sớm. Facility Manager là người quyết định xử lý.

\- Actor: AI System; Facility Manager

\- Priority: Tối quan trọng

\- Source: FR-005; FR-006; BR-006; BR-008; BR-009

\- Status: ANSWERED

## **Q-005 — Lịch trình bảo trì tài sản**

\- ID: Q-005

\- Tên: Lịch trình bảo trì tài sản

\- Mô tả: Hệ thống có hỗ trợ lập lịch bảo trì định kỳ cho tài sản không?

\- Answer: Facility Manager có thể tạo và quản lý lịch trình bảo trì cho Wifi, điều hòa và máy chiếu, đèn, quạt. Lịch bảo trì có thể dựa trên kế hoạch định kỳ hoặc khuyến nghị từ AI.

\- Actor: Facility Manager

\- Priority: Cao

\- Source: FR-011; BR-010; NFR-010

\- Status: ANSWERED

