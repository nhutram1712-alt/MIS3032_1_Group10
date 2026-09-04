## **US-001 — Đăng nhập hệ thống**

## Là một Người dùng (gồm Customer, Technician, Facility Manager và Administrator)

## Tôi muốn đăng nhập bằng tài khoản được nhà trường cấp

## Để tôi có thể truy cập hệ thống và sử dụng các chức năng theo đúng quyền hạn của vai trò.

* ## **Related Requirement:** FR-001

* ## **Related Business Rule:** BR-011

* ## **Related Use Case:** UC-001

* ## **Priority:** Tối quan trọng

* ## **Dependencies:** Tài khoản người dùng hợp lệ do nhà trường cấp; tài khoản thuộc một trong bốn vai trò được phép truy cập (Customer, Technician, Facility Manager, Administrator).

## **US-002 — Gửi yêu cầu báo sự cố tài sản**

## Là một Customer (giảng viên, sinh viên hoặc nhân viên)

## Tôi muốn chọn một tài sản cơ sở vật chất và gửi yêu cầu báo sự cố kèm mô tả vấn đề

## Để bộ phận quản lý cơ sở vật chất ghi nhận và tiến hành xử lý/bảo trì.

* ## **Related Requirement:** FR-002

* ## **Related Business Rule:** BR-001; BR-002; BR-003; BR-011

* ## **Related Use Case:** UC-002

* ## **Priority:** Tối quan trọng

* ## **Dependencies:** Đăng nhập hệ thống (US-001/FR-001); tài sản thuộc phạm vi quản lý của nhà trường (Wifi, điều hòa, máy chiếu, đèn, quạt) đã được định danh bằng Asset ID và có vị trí cụ thể.

## **US-003 — Theo dõi trạng thái yêu cầu**

Là một Customer, tôi muốn xem trạng thái các yêu cầu báo sự cố do mình gửi để tôi có thể theo dõi tiến độ xử lý mà không cần liên hệ trực tiếp với nhân viên quản lý cơ sở vật chất.

* **Related Requirement:** FR-003  
* **Related Business Rule:** BR-003; BR-004; BR-005; BR-011  
* **Related Use Case:** UC-003  
* **Priority:** Không quy định  
* **Dependencies:** US-002; dữ liệu yêu cầu do Customer tạo; trạng thái Work Order được quản lý theo quy trình bảo trì.

## **US-004 — Quản lý tài sản**

Là một Facility Manager, tôi muốn tạo, cập nhật và xem thông tin tài sản cùng lịch sử bảo trì để tôi có thể quản lý chính xác tình trạng và quá trình bảo trì của các tài sản trong trường.

* **Related Requirement:** FR-004  
* **Related Business Rule:** BR-001; BR-002; BR-006; BR-010; BR-011  
* **Related Use Case:** UC-004  
* **Priority:** Quan trọng  
* **Dependencies:** Dữ liệu tài sản; dữ liệu IoT được liên kết với Asset ID; lịch sử bảo trì được tạo từ các Work Order đã hoàn thành.

## **US-005 — Giám sát tình trạng thiết bị qua IoT**

Là một Facility Manager

Tôi muốn xem dữ liệu hoạt động và cảnh báo IoT của Wifi, điều hòa và máy chiếu

Để tôi có thể phát hiện sớm thiết bị có dấu hiệu bất thường và theo dõi tình trạng tài sản.

* **Related Requirement:** FR-005  
* **Related Business Rule:** BR-006; BR-007; BR-011  
* **Related Use Case:** UC-005  
* **Priority:** Tối quan trọng  
* **Dependencies:** Tài sản có Asset ID; thiết bị IoT được liên kết đúng với tài sản; người dùng đã đăng nhập và có quyền phù hợp.

## **US-006 — Nhận dự đoán sự cố và nhu cầu bảo trì từ AI**

Là một Facility Manager

Tôi muốn AI phân tích dữ liệu IoT và lịch sử bảo trì để dự đoán thiết bị có nguy cơ sự cố hoặc cần bảo trì

Để tôi có thể chủ động kiểm tra, lập Work Order hoặc lên lịch bảo trì trước khi sự cố nghiêm trọng xảy ra.

* **Related Requirement:** FR-006  
* **Related Business Rule:** BR-008; BR-009; BR-010; BR-011  
* **Related Use Case:** UC-006  
* **Priority:** Tối quan trọng  
* **Dependencies:** US-005; có dữ liệu IoT hợp lệ và/hoặc lịch sử bảo trì của tài sản; Facility Manager có quyền xem kết quả AI.

  ## **US-007 — Tạo và phân công Work Order**

> **Là một** Facility Manager  
>  **Tôi muốn** tạo Work Order cho một tài sản và phân công Technician xử lý  
>  **Để** sự cố hoặc nhu cầu bảo trì được xử lý bởi đúng người có trách nhiệm

* **Yêu cầu liên quan:** FR-007  
* **Business Rule liên quan:** BR-004; BR-005; BR-011  
* **Use Case liên quan:** UC-007  
* **Mức ưu tiên:** Tối quan trọng  
* **Phụ thuộc:** Đăng nhập hệ thống (FR-001); nguồn phát sinh hợp lệ từ yêu cầu Customer (FR-002), cảnh báo IoT (FR-005), hoặc dự đoán AI (FR-006); Technician hợp lệ trong hệ thống.

## **US-008 — Xem Work Order được giao**

> **Là một** Technician  
>  **Tôi muốn** xem danh sách Work Order được phân công cho mình cùng thông tin tài sản và sự cố liên quan  
>  **Để** tôi biết công việc cần xử lý và có đủ thông tin để thực hiện

* **Yêu cầu liên quan:** FR-008  
* **Business Rule liên quan:** BR-004; BR-011  
* **Use Case liên quan:** UC-008  
* **Mức ưu tiên:** Tối quan trọng  
* **Phụ thuộc:** Đăng nhập hệ thống (FR-001); Work Order đã được Facility Manager tạo và phân công (US-007).

## **US-009 — Cập nhật kết quả kiểm tra và sửa chữa**

Là một **Technician**. Tôi muốn cập nhật tiến độ, kết quả kiểm tra hoặc sửa chữa của Work Order được phân công, để hệ thống ghi nhận kết quả xử lý và lưu thông tin vào lịch sử bảo trì của tài sản**.**

* **Related Requirement:** FR-009  
* **Related Business Rule:** BR-005, BR-010, BR-011  
* **Related Use Case:** UC-009  
* **Priority:** Quan trọng  
* **Dependencies:** Đăng nhập hệ thống (FR-001), Technician đã được Facility Manager phân công Work Order, Work Order gắn với một tài sản cụ thể và đang ở trạng thái cho phép Technician cập nhật.

# **US-010 — Quản lý tài khoản và quyền truy cập**

## Là một **Administrator**. Tôi muốn quản lý tài khoản người dùng và phân quyền theo vai trò, để đảm bảo người dùng được cấp quyền truy cập và sử dụng các chức năng phù hợp với vai trò của mình trong hệ thống.

* **Related Requirement:** FR-010  
* **Related Business Rule:** BR-011  
* **Related Use Case:** UC-010  
* **Priority:** Không quy định  
* **Dependencies:** Đăng nhập hệ thống (US-001/FR-001), Administrator có quyền quản lý tài khoản và phân quyền, tài khoản người dùng thuộc phạm vi sử dụng nội bộ của nhà trường.

# **US-011 — Quản lý lịch trình bảo trì tài sản**

## Là một **Facility Manager.** Tôi muốn tạo, xem và cập nhật lịch bảo trì cho từng tài sản, để chủ động lập kế hoạch bảo trì định kỳ hoặc thực hiện bảo trì dựa trên khuyến nghị của AI.

* **Related Requirement:** FR-011  
* **Related Business Rule:** BR-008, BR-009, BR-010  
* **Related Use Case:** UC-011  
* **Priority:** Cao  
* **Dependencies:** Đăng nhập hệ thống (US-001/FR-001), tài sản đã được quản lý trong hệ thống, Facility Manager có quyền quản lý lịch bảo trì, nếu lịch dựa trên AI thì hệ thống có kết quả dự đoán hoặc khuyến nghị bảo trì liên quan đến tài sản.

