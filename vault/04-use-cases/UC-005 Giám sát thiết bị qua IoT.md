# **UC-005 Giám sát thiết bị qua IoT**

## **Related Requirements**

·       FR-005 — Giám sát thiết bị qua IoT.

·       NFR-003 — Thời gian phản hồi.

·       NFR-005 — Bảo mật và phân quyền.

·       NFR-006 — Khả năng truy vết và nhật ký.

·       NFR-007 — Độ tin cậy dữ liệu IoT.

## **Related Business Rules**

·       BR-006 — Dữ liệu IoT phải gắn với tài sản.

·       BR-007 — Cảnh báo bất thường từ IoT.

·       BR-011 — Mô hình vai trò và quyền truy cập.

## **Primary Actor**

Facility Manager.

Technician có thể xem dữ liệu IoT liên quan đến tài sản trong Work Order được giao; hệ thống IoT là nguồn cung cấp dữ liệu.

## **Preconditions**

1\.      Người dùng đã đăng nhập và có quyền phù hợp.

2\.      Tài sản Wifi, điều hòa hoặc máy chiếu đã tồn tại trong hệ thống và có Asset ID.

3\.      Thiết bị/cảm biến IoT đã được liên kết đúng với Asset ID tương ứng.

4\.      Kết nối từ thiết bị IoT đến hệ thống đang khả dụng hoặc hệ thống có thể nhận dữ liệu mới nhất đã lưu.

## **Trigger**

Thiết bị IoT gửi dữ liệu hoạt động mới hoặc Facility Manager mở màn hình giám sát thiết bị.

## **Main Flow**

5\.      Thiết bị IoT gửi dữ liệu hoạt động kèm Asset ID và thời điểm ghi nhận.

6\.      Hệ thống kiểm tra Asset ID và xác định tài sản tương ứng.

7\.      Hệ thống lưu và hiển thị dữ liệu hoạt động mới nhất của tài sản.

8\.      Hệ thống kiểm tra dữ liệu theo ngưỡng/cấu hình giám sát hiện có.

9\.      Nếu dữ liệu có dấu hiệu bất thường, hệ thống tạo cảnh báo cho Facility Manager.

10\.  Facility Manager xem cảnh báo và dữ liệu liên quan để quyết định bước xử lý tiếp theo.

11\.  Nếu tài sản đã có Work Order và Technician được phân công, Technician có thể xem dữ liệu IoT liên quan.

## **Alternative Flows**

### **AF-01 — Dữ liệu bình thường**

12\.  Dữ liệu IoT nằm trong ngưỡng bình thường.

13\.  Hệ thống cập nhật trạng thái thiết bị và không tạo cảnh báo.

### **AF-02 — IoT tạm thời mất kết nối**

14\.  Hệ thống không nhận được dữ liệu mới trong khoảng thời gian đã cấu hình.

15\.  Hệ thống hiển thị trạng thái dữ liệu không còn cập nhật hoặc cảnh báo mất kết nối.

16\.  Customer vẫn có thể báo lỗi thủ công cho tài sản.

## **Exception Flows**

### **EF-01 — Dữ liệu không có Asset ID hợp lệ**

17\.  Hệ thống không sử dụng bản ghi cho giám sát hoặc phân tích AI.

18\.  Hệ thống ghi nhận lỗi dữ liệu để kiểm tra.

Thông điệp lỗi chi tiết và cơ chế thử lại chưa được xác định trong tài liệu yêu cầu hiện tại.

## **Data Requirements**

### **Input**

·       Asset ID.

·       Thời điểm ghi nhận dữ liệu.

·       Giá trị/trạng thái hoạt động do thiết bị IoT gửi.

### **Stored/Displayed Data**

·       Asset ID và tài sản tương ứng.

·       Dữ liệu IoT mới nhất và thời điểm cập nhật.

·       Trạng thái bình thường/bất thường hoặc trạng thái mất kết nối.

·       Cảnh báo IoT nếu có.

## **Postconditions**

### **Success**

19\.  Dữ liệu IoT hợp lệ được gắn đúng với tài sản.

20\.  Facility Manager nhìn thấy trạng thái thiết bị mới nhất.

21\.  Cảnh báo được tạo khi phát hiện dấu hiệu bất thường.

### **Failure**

22\.  Dữ liệu không hợp lệ không được dùng cho phân tích AI.

23\.  Việc mất dữ liệu IoT không làm mất khả năng quản lý tài sản hoặc báo lỗi thủ công.

## **Open Questions**

24\.  Ngưỡng bất thường cụ thể cho từng loại tài sản được cấu hình như thế nào?

25\.  Bao lâu không nhận được dữ liệu thì được xem là mất kết nối?

26\.  Thông báo cảnh báo được gửi qua màn hình hệ thống hay thêm kênh khác?

