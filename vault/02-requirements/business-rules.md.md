

## **BR-001 \- Định danh và thông tin tài sản**

\- **ID:** BR-001

\- **Tên:** Định danh và thông tin tài sản

\- **Mô tả:** Mỗi tài sản trong hệ thống phải có một Asset ID duy nhất. Thông tin tối thiểu gồm tên tài sản, loại tài sản, vị trí và trạng thái hiện tại. Hai tài sản không được sử dụng cùng một Asset ID.

\- **Đối tượng:** Facility Manager; Administrator

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/01-sources/customer-brief.md; vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-002 \- Phạm vi và vị trí tài sản**

\- **ID:** BR-002

\- **Tên:** Phạm vi và vị trí tài sản

\- **Mô tả:** Trong MVP, hệ thống quản lý các nhóm tài sản: Wifi, điều hòa, máy chiếu và đèn, quạt. Mỗi tài sản phải được gắn với một vị trí cụ thể trong trường như tòa nhà, tầng, phòng học hoặc khu vực sử dụng.

\- **Đối tượng:** Facility Manager; Administrator

\- **Mức ưu tiên:** Không quy định

\- **Nguồn:** vault/01-sources/customer-brief.md; vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-003 \- Yêu cầu báo sự cố của Customer**

\- **ID:** BR-003

\- **Tên:** Yêu cầu báo sự cố của Customer

\- **Mô tả:** Customer gồm giảng viên, sinh viên và nhân viên trong trường. Khi báo sự cố, Customer phải chọn một tài sản cụ thể và mô tả vấn đề. Customer chỉ được xem các yêu cầu do chính mình tạo.

\- **Đối tượng:** Customer

\- **Mức ưu tiên:** Không quy định

\- **Nguồn:** vault/01-sources/customer-brief.md

\- **Trạng thái:** CONFIRMED

## **BR-004 \- Tạo và phân công Work Order**

\- **ID:** BR-004

\- **Tên:** Tạo và phân công Work Order

\- **Mô tả:** Chỉ Facility Manager được tạo Work Order và phân công Technician Staff. Mỗi Work Order phải gắn với một tài sản và có thể được tạo từ yêu cầu của Customer, cảnh báo IoT hoặc dự đoán bảo trì của AI.

\- **Đối tượng:** Facility Manager

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/01-sources/customer-brief.md; vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-005 \- Vòng đời Work Order**

\- **ID:** BR-005

\- **Tên:** Vòng đời Work Order

\- **Mô tả:** Work Order sử dụng quy trình trạng thái đơn giản: NEW \-\> IN\_PROGRESS \-\> COMPLETED \-\> CLOSED. Technician Staff được chuyển nhiệm vụ được giao sang IN\_PROGRESS và COMPLETED; Facility Manager kiểm tra kết quả trước khi chuyển sang CLOSED.

\- **Đối tượng:** Technician Staff; Facility Manager

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/01-sources/customer-brief.md

\- **Trạng thái:** CONFIRMED

## **BR-006 \- Dữ liệu IoT phải gắn với tài sản**

\- **ID:** BR-006

\- **Tên:** Dữ liệu IoT phải gắn với tài sản

\- **Mô tả:** Mọi dữ liệu IoT dùng để giám sát thiết bị phải được gắn với đúng Asset ID và thời điểm ghi nhận. Dữ liệu không xác định được tài sản sẽ không được dùng cho phân tích AI. Khi IoT tạm thời mất kết nối, tài sản vẫn có thể được quản lý và báo lỗi thủ công.

\- **Đối tượng:** System; Facility Manager

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-007 \- Cảnh báo bất thường từ IoT**

\- **ID:** BR-007

\- **Tên:** Cảnh báo bất thường từ IoT

\- **Mô tả:** Khi dữ liệu IoT cho thấy thiết bị có dấu hiệu bất thường hoặc ngừng gửi dữ liệu theo ngưỡng đã cấu hình, hệ thống tạo cảnh báo để Facility Manager kiểm tra. Cảnh báo IoT không tự động đóng hoặc xác nhận sự cố.

\- **Đối tượng:** System; Facility Manager

\- **Mức ưu tiên:** Cao

\- **Nguồn:** vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-008 \- AI dự đoán sự cố và nhu cầu bảo trì**

\- **ID:** BR-008

\- **Tên:** AI dự đoán sự cố và nhu cầu bảo trì

\- **Mô tả:** AI sử dụng dữ liệu IoT cùng lịch sử bảo trì của tài sản để dự đoán thiết bị có nguy cơ xảy ra sự cố hoặc cần bảo trì. Kết quả tối thiểu gồm tài sản liên quan, mức rủi ro và khuyến nghị bảo trì.

\- **Đối tượng:** AI System; Facility Manager

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md

\- **Trạng thái:** CONFIRMED

## **BR-009 \- AI chỉ hỗ trợ quyết định**

\- **ID:** BR-009

\- **Tên:** AI chỉ hỗ trợ quyết định

\- **Mô tả:** Kết quả dự đoán của AI là thông tin hỗ trợ, không phải quyết định cuối cùng. Facility Manager phải xem xét cảnh báo hoặc dự đoán trước khi tạo Work Order. AI không tự phân công Technician Staff và không tự đóng Work Order.

\- **Đối tượng:** AI System; Facility Manager

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-010 \- Lưu lịch sử bảo trì**

\- **ID:** BR-010

\- **Tên:** Lưu lịch sử bảo trì

\- **Mô tả:** Khi Work Order được đóng, hệ thống phải lưu lịch sử bảo trì của tài sản, bao gồm Asset ID, sự cố, Technician Staff thực hiện, nội dung xử lý, thời gian và kết quả. Lịch sử này được dùng làm dữ liệu tham khảo cho các lần bảo trì sau và cho mô hình AI.

\- **Đối tượng:** Technician Staff; Facility Manager; System

\- **Mức ưu tiên:** Cao

\- **Nguồn:** vault/01-sources/customer-brief.md; vault/00-idea/idea.md

\- **Trạng thái:** CONFIRMED

## **BR-011 \- Mô hình vai trò và quyền truy cập**

\- **ID:** BR-011

\- **Tên:** Mô hình vai trò và quyền truy cập

\- **Mô tả:** Customer có thể đăng nhập, báo sự cố và xem yêu cầu của mình. Technician Staff chỉ xem và cập nhật Work Order được phân công. Facility Manager quản lý tài sản, yêu cầu, Work Order, lịch sử bảo trì và theo dõi cảnh báo AI/IoT. Administrator quản lý tài khoản, vai trò và cấu hình hệ thống.

\- **Đối tượng:** Customer; Technician Staff; Facility Manager; Administrator

\- **Mức ưu tiên:** Quan trọng

\- **Nguồn:** vault/01-sources/customer-brief.md

\- **Trạng thái:** CONFIRMED

