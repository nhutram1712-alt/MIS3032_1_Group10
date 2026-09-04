

## **FR-001 \- Đăng nhập hệ thống**

\- **ID:** FR-001

\- **Tên:** Đăng nhập hệ thống

\- **Mô tả:** Hệ thống phải cho phép Customer, Technician, Facility Manager và Administrator đăng nhập bằng tài khoản được cấp để sử dụng các chức năng theo đúng quyền của vai trò.

\- **Đối tượng:** Customer; Technician; Facility Manager; Administrator

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-011

\- **Trạng thái:** CONFIRMED

## **FR-002 \- Gửi yêu cầu báo sự cố tài sản**

\- **ID:** FR-002

\- **Tên:** Gửi yêu cầu báo sự cố tài sản

\- **Mô tả:** Hệ thống phải cho phép Customer gồm giảng viên, sinh viên và nhân viên trong trường chọn một tài sản Wifi, điều hòa hoặc máy chiếu và gửi yêu cầu báo sự cố kèm mô tả vấn đề.

\- **Đối tượng:** Customer

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-003

\- **Trạng thái:** CONFIRMED

## **FR-003 \- Theo dõi trạng thái yêu cầu**

\- **ID:** FR-003

\- **Tên:** Theo dõi trạng thái yêu cầu

\- **Mô tả:** Hệ thống phải cho phép Customer xem các yêu cầu do mình đã gửi và theo dõi trạng thái xử lý của từng yêu cầu.

\- **Đối tượng:** Customer

\- **Mức ưu tiên:** Không quy định

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-003

\- **Trạng thái:** CONFIRMED

## **FR-004 \- Quản lý tài sản**

\- **ID:** FR-004

\- **Tên:** Quản lý tài sản

\- **Mô tả:** Hệ thống phải cho phép Facility Manager quản lý danh sách Wifi, điều hòa và máy chiếu theo Asset ID, loại tài sản, vị trí và trạng thái; đồng thời xem lịch sử sự cố và bảo trì của từng tài sản.

\- **Đối tượng:** Facility Manager

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-001; BR-002; BR-010

\- **Trạng thái:** CONFIRMED

## **FR-005 \- Giám sát thiết bị qua IoT**

\- **ID:** FR-005

\- **Tên:** Giám sát thiết bị qua IoT

\- **Mô tả:** Hệ thống phải tiếp nhận và hiển thị dữ liệu hoạt động từ IoT được gắn với tài sản. Khi dữ liệu cho thấy dấu hiệu bất thường, hệ thống phải tạo cảnh báo để Facility Manager theo dõi; Technician được xem dữ liệu liên quan đến tài sản trong Work Order được giao.

\- **Đối tượng:** System; Facility Manager; Technician

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-006; BR-007

\- **Trạng thái:** CONFIRMED

## **FR-006 \- AI dự đoán sự cố và nhu cầu bảo trì**

\- **ID:** FR-006

\- **Tên:** AI dự đoán sự cố và nhu cầu bảo trì

\- **Mô tả:** Hệ thống AI phải phân tích dữ liệu IoT kết hợp với lịch sử bảo trì để dự đoán thiết bị có nguy cơ xảy ra sự cố hoặc cần bảo trì sớm. Kết quả phải được hiển thị cho Facility Manager để hỗ trợ quyết định xử lý.

\- **Đối tượng:** AI System; Facility Manager

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-008; BR-009

\- **Trạng thái:** CONFIRMED

## **FR-007 \- Tạo và phân công Work Order**

\- **ID:** FR-007

\- **Tên:** Tạo và phân công Work Order

\- **Mô tả:** Hệ thống phải cho phép Facility Manager tạo Work Order cho một tài sản và phân công Technician xử lý. Work Order có thể được tạo từ yêu cầu của Customer, cảnh báo IoT hoặc dự đoán bảo trì của AI.

\- **Đối tượng:** Facility Manager

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-004; BR-005

\- **Trạng thái:** CONFIRMED

## **FR-008 \- Xem Work Order được giao**

\- **ID:** FR-008

\- **Tên:** Xem Work Order được giao

\- **Mô tả:** Hệ thống phải cho phép Technician xem danh sách Work Order được phân công, bao gồm thông tin tài sản, nội dung sự cố và các dữ liệu IoT hoặc dự đoán AI liên quan nếu có.

\- **Đối tượng:** Technician

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-004; BR-011

\- **Trạng thái:** CONFIRMED

## **FR-009 \- Cập nhật kết quả kiểm tra và sửa chữa**

\- **ID:** FR-009

\- **Tên:** Cập nhật kết quả kiểm tra và sửa chữa

\- **Mô tả:** Hệ thống phải cho phép Technician cập nhật tiến độ, kết quả kiểm tra hoặc sửa chữa của Work Order được giao. Khi công việc hoàn thành, thông tin xử lý phải được lưu vào lịch sử bảo trì của tài sản.

\- **Đối tượng:** Technician

\- **Mức ưu tiên:** Tối quan trọng

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-005; BR-010

\- **Trạng thái:** CONFIRMED

## **FR-010 \- Quản lý tài khoản và quyền truy cập**

\- **ID:** FR-010

\- **Tên:** Quản lý tài khoản và quyền truy cập

\- **Mô tả:** Hệ thống phải cho phép Administrator quản lý tài khoản người dùng và phân quyền truy cập theo bốn vai trò: Customer, Technician, Facility Manager và Administrator.

\- **Đối tượng:** Administrator

\- **Mức ưu tiên:** Không quy định

\- **Nguồn:** vault/00-idea/idea.md; vault/01-sources/customer-brief.md; BR-011

\- **Trạng thái:** CONFIRMED

## **FR-011 \- Quản lý lịch trình bảo trì tài sản**

\- **ID:** FR-011

\- **Tên:** Quản lý lịch trình bảo trì tài sản

\- **Mô tả:** Hệ thống phải cho phép Facility Manager tạo, xem và cập nhật lịch bảo trì cho từng tài sản Wifi, điều hòa và máy chiếu. Mỗi lịch bảo trì gồm tài sản, thời gian dự kiến và nội dung bảo trì. Lịch có thể được lập định kỳ hoặc dựa trên khuyến nghị bảo trì của AI, nhưng phải do Facility Manager xác nhận.

\- **Đối tượng:** Facility Manager

\- **Mức ưu tiên:** Cao

\- **Nguồn:** Quyết định của Project Owner; FR-004; FR-006; BR-008; BR-009

\- **Trạng thái:** CONFIRMED

