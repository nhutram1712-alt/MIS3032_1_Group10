# **Acceptance Criteria**

## **US-001 — Đăng nhập hệ thống**

## Các tiêu chí dưới đây chỉ bao phủ các hành vi đã được xác nhận trong yêu cầu hệ thống. Chúng không định nghĩa cơ chế thử lại, khóa tài khoản tạm thời, số lần đăng nhập sai tối đa, giao diện điều hướng chi tiết hoặc thời gian hết hạn phiên đăng nhập vì các vấn đề này vẫn đang mở trong FR-001 và UC-001.

### **AC-US001-01 — Đăng nhập thành công với tài khoản nội bộ hợp lệ**

## **Given** người dùng sở hữu tài khoản hợp lệ do nhà trường cấp thuộc một trong bốn vai trò: Customer, Technician, Facility Manager hoặc Administrator

## **When** người dùng thực hiện đăng nhập vào hệ thống

## **Then** hệ thống xác thực thành công danh tính người dùng qua kết nối mã hóa TLS và cấp quyền truy cập phiên làm việc tương ứng.

* ## **FR:** FR-001

* ## **NFR:** NFR-001; NFR-005

* ## **BR:** BR-011

* ## **UC:** UC-001

* ## **US:** US-001

### **AC-US001-02 — Áp dụng phân quyền chuẩn xác theo vai trò sau đăng nhập**

## **Given** người dùng đã đăng nhập thành công vào hệ thống

## **When** người dùng bắt đầu phiên làm việc

## **Then** hệ thống giới hạn quyền hạn nghiêm ngặt theo đúng vai trò được quy định: Customer chỉ được gửi và theo dõi yêu cầu của mình; Technician chỉ được xem và cập nhật Work Order được giao; Facility Manager được quản lý tài sản, Work Order, lịch bảo trì và cảnh báo; Administrator chỉ thực hiện quản lý tài khoản và quyền truy cập.

* ## **FR:** FR-001

* ## **NFR:** NFR-005

* ## **BR:** BR-011

* ## **UC:** UC-001

* ## **US:** US-001

### **AC-US001-03 — Từ chối đăng nhập khi sai thông tin xác thực**

## **Given** người dùng truy cập trang đăng nhập của hệ thống

## **When** người dùng cung cấp thông tin tài khoản không chính xác

## **Then** hệ thống từ chối cấp quyền truy cập phiên làm việc.

* ## **FR:** FR-001

* ## **NFR:** NFR-005

* ## **BR:** BR-011

* ## **UC:** UC-001

* ## **US:** US-001

### **AC-US001-04 — Đảm bảo thời gian phản hồi khi đăng nhập**

## **Given** người dùng cung cấp thông tin tài khoản hợp lệ của nhà trường

## **When** người dùng gửi yêu cầu đăng nhập trong điều kiện vận hành bình thường

## **Then** thao tác đăng nhập và xác thực của hệ thống phải hoàn thành trong vòng 3 giây.

* ## **FR:** FR-001

* ## **NFR:** NFR-003

* ## **UC:** UC-001

* ## **US:** US-001

## **US-002 — Gửi yêu cầu báo sự cố tài sản**

## Các tiêu chí dưới đây chỉ bao phủ các hành vi đã được xác nhận. Chúng không định nghĩa bộ lọc danh sách tài sản, cơ chế đính kèm tệp/hình ảnh, lưu nháp, hoặc giải quyết xung đột khi nhiều người cùng gửi báo lỗi trùng lặp cho một tài sản vì các nội dung này chưa được xác định trong FR-002 và UC-002.

### **AC-US002-01 — Gửi yêu cầu báo sự cố hợp lệ cho tài sản**

## **Given** Customer đã đăng nhập thành công vào hệ thống và chọn một tài sản hợp lệ thuộc phạm vi quản lý (Wifi, điều hòa, máy chiếu, đèn, quạt) có Asset ID duy nhất và vị trí cụ thể

## **When** Customer nhập nội dung mô tả vấn đề và gửi yêu cầu

## **Then** hệ thống ghi nhận một yêu cầu báo sự cố mới liên kết với đúng tài sản đó và gắn với định danh của Customer tạo yêu cầu.

* ## **FR:** FR-002

* ## **BR:** BR-001; BR-002; BR-003

* ## **UC:** UC-002

* ## **US:** US-002

### **AC-US002-02 — Yêu cầu báo sự cố phải gắn với một tài sản cụ thể**

## **Given** Customer đã đăng nhập và đang tạo yêu cầu báo sự cố

## **When** Customer không chọn bất kỳ tài sản cụ thể nào và cố gắng gửi yêu cầu

## **Then** hệ thống từ chối tạo yêu cầu báo sự cố.

* ## **FR:** FR-002

* ## **BR:** BR-003

* ## **UC:** UC-002

* ## **US:** US-002

### **AC-US002-03 — Yêu cầu báo sự cố phải có nội dung mô tả vấn đề**

## **Given** Customer đã đăng nhập và đã chọn một tài sản cụ thể

## **When** Customer để trống nội dung mô tả vấn đề và gửi yêu cầu

## **Then** hệ thống từ chối tạo yêu cầu báo sự cố.

* ## **FR:** FR-002

* ## **BR:** BR-003

* ## **UC:** UC-002

* ## **US:** US-002

### **AC-US002-04 — Customer chỉ được xem yêu cầu do chính mình tạo**

## **Given** Customer đã đăng nhập và đã gửi yêu cầu báo sự cố vào hệ thống

## **When** Customer truy cập thông tin các yêu cầu báo sự cố

## **Then** hệ thống chỉ hiển thị các yêu cầu do chính Customer đó tạo và không hiển thị yêu cầu của người dùng khác.

* ## **FR:** FR-002; FR-003

* ## **BR:** BR-003; BR-011

* ## **NFR:** NFR-005

* ## **UC:** UC-002

* ## **US:** US-002

### **AC-US002-05 — Cho phép gửi báo lỗi thủ công khi IoT mất kết nối**

## **Given** thiết bị/cảm biến IoT gắn với tài sản đang tạm thời mất kết nối hoặc dữ liệu không còn cập nhật

## **When** Customer chọn tài sản đó và gửi yêu cầu báo sự cố thủ công

## **Then** hệ thống vẫn cho phép tạo và ghi nhận yêu cầu báo sự cố cho tài sản đó một cách bình thường.

* ## **FR:** FR-002; FR-005

* ## **BR:** BR-006

* ## **NFR:** NFR-007

* ## **UC:** UC-002

* ## **US:** US-002

### **AC-US002-06 — Yêu cầu báo sự cố là nguồn phát sinh hợp lệ để tạo Work Order**

## **Given** một yêu cầu báo sự cố từ Customer đã được hệ thống ghi nhận thành công cho một tài sản

## **When** Facility Manager kiểm tra các yêu cầu phát sinh để xử lý

## **Then** hệ thống chấp nhận yêu cầu của Customer là một nguồn phát sinh hợp lệ để tạo Work Order gắn với tài sản đó.

* ## **FR:** FR-002; FR-007

* ## **BR:** BR-004

* ## **UC:** UC-002; UC-007

* ## **US:** US-002; US-007

### **AC-US002-07 — Đảm bảo thời gian phản hồi khi gửi yêu cầu báo sự cố**

## **Given** Customer nhập đầy đủ thông tin tài sản và mô tả vấn đề hợp lệ

## **When** Customer xác nhận gửi yêu cầu báo sự cố trong điều kiện vận hành bình thường

## **Then** thao tác xử lý và lưu yêu cầu của hệ thống phải hoàn thành trong vòng 3 giây.

* ## **FR:** FR-002

* ## **NFR:** NFR-003

* ## **UC:** UC-002

* ## **US:** US-002

## **US-003 — Theo dõi trạng thái yêu cầu**

The criteria below cover only confirmed behavior. They do not define notification behavior, request status values, filtering, sorting, or detailed error messages because those matters remain open in UC-003.

### **AC-US003-01 — Hiển thị các yêu cầu do Customer tạo**

**Given** Customer đã đăng nhập và đã tạo một hoặc nhiều yêu cầu báo sự cố

**When** Customer truy cập chức năng theo dõi yêu cầu

**Then** hệ thống chỉ hiển thị các yêu cầu do chính Customer đó tạo.

* **FR:** FR-003  
* **BR:** BR-003; BR-011  
* **NFR:** NFR-005  
* **UC:** UC-003  
* **US:** US-003

### **AC-US003-02 — Hiển thị trạng thái hiện tại của yêu cầu**

**Given** Customer có một yêu cầu báo sự cố đã được lưu trong hệ thống

**When** Customer xem yêu cầu đó

**Then** hệ thống hiển thị thông tin tài sản, nội dung sự cố và trạng thái xử lý hiện tại của yêu cầu.

* **FR:** FR-003  
* **BR:** BR-003  
* **UC:** UC-003  
* **US:** US-003

### **AC-US003-03 — Hiển thị Work Order liên quan nếu có**

**Given** một yêu cầu báo sự cố của Customer đã được Facility Manager chuyển thành Work Order

**When** Customer xem yêu cầu đó

**Then** hệ thống hiển thị trạng thái xử lý của Work Order liên quan.

* **FR:** FR-003; FR-007  
* **BR:** BR-004; BR-005  
* **UC:** UC-003  
* **US:** US-003

### **AC-US003-04 — Không cho phép Customer xem yêu cầu của người khác**

**Given** Customer đã đăng nhập vào hệ thống

**When** Customer cố gắng truy cập một yêu cầu không do mình tạo

**Then** hệ thống từ chối truy cập và không hiển thị thông tin của yêu cầu đó.

* **FR:** FR-003  
* **BR:** BR-003; BR-011  
* **NFR:** NFR-005  
* **UC:** UC-003  
* **US:** US-003

## **US-004 — Quản lý tài sản**

The criteria below cover only confirmed behavior. They do not define asset deletion, Asset ID generation, detailed asset status values, search/filter behavior, or the number of IoT sensors per asset because those matters remain open in UC-004.

### **AC-US004-01 — Tạo tài sản với Asset ID duy nhất**

**Given** Facility Manager đã đăng nhập và có quyền quản lý tài sản

**When** Facility Manager nhập đầy đủ thông tin tài sản với một Asset ID chưa tồn tại và lưu tài sản

**Then** hệ thống tạo tài sản mới với Asset ID duy nhất, loại tài sản, vị trí và trạng thái được cung cấp.

* **FR:** FR-004  
* **BR:** BR-001; BR-002; BR-011  
* **NFR:** NFR-005  
* **UC:** UC-004  
* **US:** US-004

### **AC-US004-02 — Không cho phép trùng Asset ID**

**Given** một Asset ID đã tồn tại trong hệ thống

**When** Facility Manager tạo tài sản mới với Asset ID đó

**Then** hệ thống từ chối tạo tài sản mới.

* **FR:** FR-004  
* **BR:** BR-001  
* **UC:** UC-004  
* **US:** US-004

### **AC-US004-03 — Cập nhật thông tin tài sản**

**Given** tài sản đã tồn tại trong hệ thống

**When** Facility Manager cập nhật thông tin tài sản và lưu thay đổi

**Then** hệ thống cập nhật thông tin tài sản và ghi nhận thay đổi quan trọng vào nhật ký hệ thống.

* **FR:** FR-004  
* **BR:** BR-001; BR-002  
* **NFR:** NFR-006  
* **UC:** UC-004  
* **US:** US-004

### **AC-US004-04 — Xem thông tin và lịch sử tài sản**

**Given** Facility Manager đã đăng nhập và chọn một tài sản

**When** Facility Manager xem thông tin chi tiết tài sản

**Then** hệ thống hiển thị Asset ID, tên, loại, vị trí, trạng thái và lịch sử sự cố, bảo trì liên quan.

* **FR:** FR-004  
* **BR:** BR-001; BR-002; BR-010  
* **UC:** UC-004  
* **US:** US-004

### **AC-US004-05 — Liên kết dữ liệu IoT với tài sản**

**Given** một tài sản có dữ liệu IoT được ghi nhận

**When** hệ thống nhận dữ liệu IoT của tài sản

**Then** dữ liệu được liên kết với đúng Asset ID và dữ liệu không còn cập nhật được hiển thị là không còn cập nhật.

* **FR:** FR-005  
* **BR:** BR-006  
* **NFR:** NFR-007  
* **UC:** UC-004  
* **US:** US-004

## **US-005 — Giám sát tình trạng thiết bị qua IoT**

Các tiêu chí dưới đây chỉ bao phủ hành vi đã được xác nhận từ FR-005 và các Business Rules/NFR liên quan. Các ngưỡng bất thường chi tiết và cơ chế thông báo cụ thể vẫn là câu hỏi mở.

### **AC-US005-01 — Hiển thị dữ liệu IoT hợp lệ của tài sản**

**Given** Facility Manager đã đăng nhập và một thiết bị IoT đang gửi dữ liệu có Asset ID hợp lệ

**When** hệ thống nhận dữ liệu mới

**Then** dữ liệu được gắn đúng với tài sản và hiển thị cùng thời điểm cập nhật.

**FR:** FR-005

**BR:** BR-006; BR-007; BR-011

**UC:** UC-005

**US:** US-005

### **AC-US005-02 — Tạo cảnh báo khi dữ liệu bất thường**

**Given** dữ liệu IoT của một tài sản vượt ngưỡng bất thường đã cấu hình

**When** hệ thống xử lý bản ghi IoT mới

**Then** hệ thống tạo cảnh báo để Facility Manager theo dõi.

**FR:** FR-005

**BR:** BR-006; BR-007; BR-011

**UC:** UC-005

**US:** US-005

### **AC-US005-03 — Không dùng dữ liệu không xác định được tài sản**

**Given** một bản ghi IoT không có Asset ID hợp lệ

**When** hệ thống tiếp nhận bản ghi

**Then** bản ghi không được dùng cho phân tích AI và được ghi nhận là dữ liệu lỗi.

**FR:** FR-005

**BR:** BR-006; BR-007; BR-011

**UC:** UC-005

**US:** US-005

### **AC-US005-04 — Hiển thị trạng thái mất kết nối IoT**

**Given** thiết bị IoT không gửi dữ liệu mới trong khoảng thời gian cấu hình

**When** Facility Manager xem trạng thái tài sản

**Then** hệ thống hiển thị dữ liệu không còn cập nhật hoặc trạng thái mất kết nối thay vì coi dữ liệu cũ là bình thường.

**FR:** FR-005

**BR:** BR-006; BR-007; BR-011

**UC:** UC-005

**US:** US-005

### **AC-US005-05 — Customer vẫn có thể báo lỗi khi IoT mất kết nối**

**Given** IoT của một tài sản đang mất kết nối

**When** Customer phát hiện sự cố và gửi yêu cầu báo lỗi thủ công

**Then** hệ thống vẫn cho phép tạo yêu cầu báo sự cố cho tài sản đó.

**FR:** FR-005

**BR:** BR-006; BR-007; BR-011

**UC:** UC-005

**US:** US-005

### **AC-US005-06 — Technician chỉ xem dữ liệu IoT của tài sản được giao**

**Given** Technician có một Work Order được phân công cho một tài sản

**When** Technician mở Work Order

**Then** Technician có thể xem dữ liệu IoT liên quan đến tài sản đó và không được mở rộng quyền quản lý tài sản.

**FR:** FR-005

**BR:** BR-006; BR-007; BR-011

**UC:** UC-005

**US:** US-005

## **US-006 — Nhận dự đoán sự cố và nhu cầu bảo trì từ AI**

Các tiêu chí dưới đây xác nhận AI sử dụng dữ liệu IoT và lịch sử bảo trì để hỗ trợ Facility Manager; AI không tự động đưa ra quyết định cuối cùng.

### **AC-US006-01 — Tạo dự đoán từ dữ liệu hợp lệ**

**Given** một tài sản có dữ liệu IoT hợp lệ và lịch sử bảo trì sẵn có

**When** AI thực hiện phân tích

**Then** hệ thống tạo kết quả dự đoán cho đúng tài sản.

**FR:** FR-006

**BR:** BR-006; BR-008; BR-009; BR-010; BR-011

**UC:** UC-006

**US:** US-006

### **AC-US006-02 — Hiển thị thông tin tối thiểu của dự đoán**

**Given** AI đã tạo được một kết quả dự đoán

**When** Facility Manager mở kết quả

**Then** hệ thống hiển thị tối thiểu tài sản liên quan, mức rủi ro và khuyến nghị kiểm tra hoặc bảo trì.

**FR:** FR-006

**BR:** BR-006; BR-008; BR-009; BR-010; BR-011

**UC:** UC-006

**US:** US-006

### **AC-US006-03 — AI không tự động tạo quyết định cuối cùng**

**Given** AI đánh giá một tài sản có nguy cơ cao

**When** kết quả dự đoán được tạo

**Then** hệ thống không tự động phân công Technician hoặc tự đóng Work Order; Facility Manager phải xem xét và quyết định.

**FR:** FR-006

**BR:** BR-006; BR-008; BR-009; BR-010; BR-011

**UC:** UC-006

**US:** US-006

### **AC-US006-04 — Không dự đoán từ dữ liệu IoT không hợp lệ**

**Given** dữ liệu IoT không xác định được Asset ID hoặc được đánh dấu không hợp lệ

**When** AI chuẩn bị dữ liệu đầu vào

**Then** dữ liệu đó không được dùng để tạo dự đoán.

**FR:** FR-006

**BR:** BR-006; BR-008; BR-009; BR-010; BR-011

**UC:** UC-006

**US:** US-006

### **AC-US006-05 — Xử lý khi dữ liệu chưa đủ**

**Given** tài sản chưa có đủ dữ liệu để AI tạo dự đoán đáng tin cậy

**When** AI được kích hoạt

**Then** hệ thống thông báo chưa đủ dữ liệu và không thực hiện hành động bảo trì tự động.

**FR:** FR-006

**BR:** BR-006; BR-008; BR-009; BR-010; BR-011

**UC:** UC-006

**US:** US-006

### **AC-US006-06 — Facility Manager có thể dùng kết quả AI để chủ động bảo trì**

**Given** AI đưa ra khuyến nghị cần kiểm tra hoặc bảo trì sớm

**When** Facility Manager chấp nhận khuyến nghị

**Then** Facility Manager có thể tiếp tục tạo Work Order hoặc đưa tài sản vào lịch trình bảo trì bằng chức năng tương ứng.

**FR:** FR-006

**BR:** BR-006; BR-008; BR-009; BR-010; BR-011

**UC:** UC-006

**US:** US-006

## **Acceptance Criteria — US-007**

**AC-US007-01 — Tạo Work Order gắn với tài sản khi có nguồn phát sinh hợp lệ**  
 Given Facility Manager đã đăng nhập và có một yêu cầu Customer, cảnh báo IoT, hoặc dự đoán AI làm nguồn phát sinh hợp lệ cho một tài sản  
 When Facility Manager chọn nguồn phát sinh đó và xác nhận tạo Work Order  
 Then hệ thống tạo một Work Order mới gắn với tài sản liên quan

* FR: FR-007   
*  BR: BR-004   
*  UC: UC-007   
* US: US-007

**AC-US007-02 — Chỉ Facility Manager được tạo và phân công Work Order**  
 Given một người dùng đã đăng nhập nhưng không có vai trò Facility Manager  
 When người dùng đó cố gắng tạo hoặc phân công Work Order  
 Then hệ thống từ chối thao tác

*  FR: FR-007   
*  BR: BR-004; BR-011   
* UC: UC-007   
* US: US-007

**AC-US007-03 — Work Order được gắn với đúng một tài sản**  
 Given Facility Manager tạo Work Order từ một nguồn phát sinh hợp lệ  
 When Work Order được tạo thành công  
 Then Work Order chỉ liên kết với đúng một tài sản

* FR: FR-007   
* BR: BR-004   
*  UC: UC-007   
*  US: US-007

**AC-US007-04 — Work Order mới có trạng thái NEW**  
 Given Facility Manager vừa tạo Work Order thành công  
 When hệ thống lưu Work Order  
 Then trạng thái ban đầu của Work Order là NEW

*  FR: FR-007   
* BR: BR-005   
* UC: UC-007   
* US: US-007

**AC-US007-05 — Technician được phân công phải là người dùng hợp lệ**  
 Given Facility Manager đang phân công Technician cho Work Order  
 When Facility Manager chọn một Technician tồn tại và hợp lệ trong hệ thống  
 Then hệ thống chấp nhận việc phân công và gắn Technician đó với Work Order

* FR: FR-007   
*  BR: BR-004; BR-011   
*  UC: UC-007   
* US: US-007

**AC-US007-06 — Work Order có thể tạo từ ba nguồn phát sinh hợp lệ**  
 Given tồn tại một yêu cầu Customer, HOẶC một cảnh báo IoT, HOẶC một dự đoán bảo trì AI cho một tài sản  
 When Facility Manager chọn nguồn đó để tạo Work Order  
 Then hệ thống chấp nhận nguồn phát sinh và tạo Work Order tương ứng

*  FR: FR-007   
* BR: BR-004   
* UC: UC-007   
*  US: US-007

**AC-US007-07 — Hành động tạo và phân công Work Order được ghi nhật ký**  
 Given Facility Manager tạo và phân công Work Order thành công  
 When hệ thống lưu Work Order  
 Then hệ thống ghi nhật ký gồm người thực hiện, hành động và thời gian

*  FR: FR-007   
*  NFR: NFR-006   
*  UC: UC-007   
* US: US-007

## **Acceptance Criteria — US-008**

**AC-US008-01 — Technician xem được danh sách Work Order được phân công cho mình**  
 Given Technician đã đăng nhập và có ít nhất một Work Order được Facility Manager phân công cho mình  
 When Technician truy cập khu vực Work Order được giao  
 Then hệ thống hiển thị các Work Order đó

* FR: FR-008  
*  BR: BR-004; BR-011   
* UC: UC-008  
* US: US-008

**AC-US008-02 — Danh sách hiển thị thông tin tài sản và nội dung sự cố**  
 Given Technician có một Work Order được giao, gắn với một tài sản có mô tả sự cố  
 When Technician xem danh sách Work Order  
 Then Work Order đó hiển thị kèm thông tin tài sản liên quan và nội dung sự cố

* FR: FR-008  
* UC: UC-008   
*  US: US-008

**AC-US008-03 — Hiển thị dữ liệu IoT/AI liên quan khi có sẵn**  
 Given Work Order được giao gắn với một tài sản có dữ liệu IoT hoặc dự đoán AI liên quan  
 When Technician xem Work Order đó  
 Then hệ thống hiển thị dữ liệu IoT/AI liên quan cùng thông tin Work Order  
 

* FR: FR-008   
*  UC: UC-008   
*  US: US-008

**AC-US008-04 — Technician chỉ xem Work Order được phân công cho chính mình**  
 Given tồn tại một Work Order được phân công cho một Technician khác  
 When Technician xem danh sách Work Order được giao của mình  
 Then hệ thống không hiển thị Work Order của Technician khác

*  FR: FR-008   
*  BR: BR-011   
*  UC: UC-008   
* US: US-008

**AC-US008-05 — Việc phân công Work Order chỉ do Facility Manager thực hiện**  
 Given một Work Order xuất hiện trong danh sách của Technician  
 When Technician xem Work Order đó  
 Then Work Order chỉ có thể xuất hiện trong danh sách nếu đã được Facility Manager phân công (không có cơ chế tự-nhận việc)

* FR: FR-008   
*  BR: BR-004; BR-011   
*  UC: UC-008   
* US: US-008

### **US-009: Cập nhật kết quả kiểm tra và sửa chữa**

**AC-US009-01 — Technician cập nhật tiến độ công việc đang thực hiện**

* **Given** Technician đã đăng nhập và mở một Work Order được phân công đang ở trạng thái cho phép cập nhật.  
* **When** Technician cập nhật tình trạng công việc đang thực hiện (chưa hoàn thành) và lưu lại.  
* **Then** hệ thống lưu thông tin tiến độ, tiếp tục giữ Work Order ở trạng thái IN\_PROGRESS và cho phép Technician tiếp tục cập nhật sau đó.  
* **FR:** FR-009  
* **BR:** BR-005  
* **UC:** UC-009  
* **US:** US-009

**AC-US009-02 — Cập nhật kết quả hoàn thành và lưu lịch sử bảo trì**

* **Given** Technician đã thực hiện xong việc kiểm tra hoặc sửa chữa tài sản cho Work Order được giao.  
* **When** Technician cập nhật kết quả xử lý và chuyển trạng thái Work Order sang COMPLETED.  
* **Then** hệ thống cập nhật trạng thái Work Order, ghi nhận thời gian thực hiện, đồng thời lưu thông tin xử lý vào lịch sử bảo trì của tài sản và ghi nhật ký thay đổi.  
* **FR:** FR-009  
* **BR:** BR-005; BR-010  
* **NFR:** NFR-006; NFR-010  
* **UC:** UC-009  
* **US:** US-009

**AC-US009-03 — Từ chối cập nhật khi Technician không có quyền với Work Order**

* **Given** Technician đã đăng nhập vào hệ thống.  
* **When** Technician cố gắng thao tác cập nhật một Work Order không được Facility Manager phân công cho mình.  
* **Then** hệ thống từ chối thao tác cập nhật và không thay đổi thông tin của Work Order đó.  
* **FR:** FR-009  
* **BR:** BR-011  
* **NFR:** NFR-005  
* **UC:** UC-009  
* **US:** US-009

**AC-US009-04 — Báo lỗi khi dữ liệu cập nhật không hợp lệ**

* **Given** Technician đang tiến hành cập nhật kết quả kiểm tra/sửa chữa cho Work Order hợp lệ.  
* **When** Technician nhập dữ liệu không hợp lệ hoặc thiếu các thông tin bắt buộc và tiến hành lưu.  
* **Then** hệ thống từ chối lưu kết quả, không thay đổi thông tin hiện tại và yêu cầu Technician bổ sung hoặc điều chỉnh thông tin.  
* **UC:** UC-009  
* **US:** US-009

### **Acceptance Criteria — US-010: Quản lý tài khoản và quyền truy cập**

**AC-US010-01 — Xem danh sách tài khoản người dùng**

* **Given** Administrator đã đăng nhập thành công vào hệ thống.  
* **When** Administrator truy cập chức năng quản lý tài khoản.  
* **Then** hệ thống hiển thị danh sách tài khoản người dùng thuộc phạm vi sử dụng nội bộ của nhà trường.  
* **FR:** FR-010  
* **NFR:** NFR-001  
* **UC:** UC-010  
* **US:** US-010

**AC-US010-02 — Cập nhật vai trò người dùng hợp lệ**

* **Given** Administrator chọn một tài khoản trong danh sách để phân quyền.  
* **When** Administrator chọn một trong bốn vai trò hợp lệ (Customer, Technician, Facility Manager, Administrator) và lưu lại.  
* **Then** hệ thống lưu vai trò mới, cập nhật quyền sử dụng chức năng tương ứng cho tài khoản và ghi nhận thay đổi vào nhật ký hệ thống.  
* **FR:** FR-010  
* **BR:** BR-011  
* **NFR:** NFR-006  
* **UC:** UC-010  
* **US:** US-010

**AC-US010-03 — Từ chối gán vai trò không hợp lệ**

* **Given** Administrator đang thao tác phân quyền cho một tài khoản.  
* **When** Administrator cố gắng gán một vai trò không thuộc bốn vai trò được hệ thống cho phép.  
* **Then** hệ thống từ chối thay đổi và giữ nguyên vai trò hiện tại của tài khoản.  
* **FR:** FR-010  
* **BR:** BR-011  
* **UC:** UC-010  
* **US:** US-010

**AC-US010-04 — Chặn quyền quản lý từ người dùng không phải Administrator**

* **Given** Một người dùng không có vai trò Administrator đã đăng nhập.  
* **When** Người dùng này cố gắng truy cập chức năng quản lý tài khoản hoặc thực hiện thay đổi quyền truy cập.  
* **Then** hệ thống từ chối thao tác và không có bất kỳ thay đổi nào được thực hiện đối với các tài khoản.  
* **FR:** FR-010  
* **BR:** BR-011  
* **NFR:** NFR-005  
* **UC:** UC-010  
* **US:** US-010

### **Acceptance Criteria — US-011: Quản lý lịch trình bảo trì tài sản**

**AC-US011-01 — Tạo lịch bảo trì định kỳ hợp lệ**

* **Given** Facility Manager đã đăng nhập và chọn một tài sản hợp lệ cần lập lịch.  
* **When** Facility Manager chọn hình thức bảo trì định kỳ, nhập thời gian dự kiến cùng nội dung bảo trì, sau đó xác nhận.  
* **Then** hệ thống lưu lịch bảo trì định kỳ gắn với tài sản đó và ghi nhận thao tác tạo lịch vào nhật ký.  
* **FR:** FR-011  
* **NFR:** NFR-006  
* **UC:** UC-011  
* **US:** US-011

**AC-US011-02 — Tạo lịch bảo trì dựa trên khuyến nghị từ AI**

* **Given** Hệ thống hiển thị kết quả AI dự đoán một tài sản có nguy cơ sự cố hoặc cần bảo trì kèm mức rủi ro.  
* **When** Facility Manager đánh giá khuyến nghị AI, quyết định lập lịch, xác định thời gian/nội dung bảo trì và xác nhận.  
* **Then** hệ thống lưu lịch bảo trì theo quyết định của Facility Manager (do AI chỉ đóng vai trò hỗ trợ, không tự động xác nhận lịch).  
* **FR:** FR-011; FR-006  
* **BR:** BR-008; BR-009  
* **UC:** UC-011  
* **US:** US-011

**AC-US011-03 — Cập nhật thông tin lịch bảo trì đã tồn tại**

* **Given** Facility Manager chọn một lịch bảo trì đã được tạo trước đó.  
* **When** Facility Manager thay đổi thời gian dự kiến hoặc nội dung bảo trì và xác nhận.  
* **Then** hệ thống kiểm tra thông tin, lưu lịch mới cập nhật và ghi nhận hành động thay đổi này vào nhật ký.  
* **FR:** FR-011  
* **NFR:** NFR-006  
* **UC:** UC-011  
* **US:** US-011

**AC-US011-04 — Báo lỗi khi thông tin lịch bảo trì không hợp lệ**

* **Given** Facility Manager đang tiến hành tạo hoặc cập nhật lịch bảo trì cho tài sản.  
* **When** Facility Manager cung cấp thời gian bảo trì, nội dung không hợp lệ hoặc để trống thông tin bắt buộc.  
* **Then** hệ thống từ chối lưu lịch bảo trì, giữ nguyên dữ liệu hiện tại và yêu cầu điều chỉnh thông tin.  
* **FR:** FR-011  
* **UC:** UC-011  
* **US:** US-011

**AC-US011-05 — Cảnh báo xung đột lịch bảo trì**

* **Given** Facility Manager tiến hành tạo hoặc cập nhật thời gian bảo trì cho một tài sản.  
* **When** Thời gian bảo trì được chọn bị trùng hoặc xung đột với một lịch bảo trì khác đã tồn tại của chính tài sản đó.  
* **Then** hệ thống phát hiện xung đột, thông báo cho Facility Manager để điều chỉnh thời gian theo quy tắc của hệ thống.  
* **UC:** UC-011  
* **US:** US-011

