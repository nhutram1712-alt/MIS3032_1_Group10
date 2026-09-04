# **UC-006 AI dự đoán sự cố và nhu cầu bảo trì**

## **Related Requirements**

·       FR-006 — AI dự đoán sự cố và nhu cầu bảo trì.

·       NFR-003 — Thời gian phản hồi.

·       NFR-006 — Khả năng truy vết và nhật ký.

·       NFR-008 — Tính minh bạch và an toàn của AI.

·       NFR-010 — Lưu trữ lịch sử bảo trì.

## **Related Business Rules**

·       BR-006 — Dữ liệu IoT phải gắn với tài sản.

·       BR-008 — AI dự đoán sự cố và nhu cầu bảo trì.

·       BR-009 — AI chỉ hỗ trợ quyết định.

·       BR-010 — Lưu lịch sử bảo trì.

·       BR-011 — Mô hình vai trò và quyền truy cập.

## **Primary Actor**

Facility Manager.

AI System thực hiện phân tích; Facility Manager là người xem kết quả và quyết định có tạo Work Order hay lập lịch bảo trì hay không.

## **Preconditions**

27\.  Facility Manager đã đăng nhập và có quyền xem dữ liệu AI.

28\.  Tài sản cần phân tích đã tồn tại và có Asset ID.

29\.  Có dữ liệu IoT hợp lệ và/hoặc lịch sử bảo trì đủ để AI phân tích.

30\.  Dữ liệu đầu vào được liên kết đúng với tài sản.

## **Trigger**

AI được kích hoạt theo chu kỳ phân tích hoặc khi có dữ liệu IoT/lịch sử bảo trì mới đủ điều kiện phân tích.

## **Main Flow**

31\.  Hệ thống tập hợp dữ liệu IoT hợp lệ và lịch sử bảo trì của tài sản.

32\.  AI phân tích dữ liệu để ước lượng nguy cơ xảy ra sự cố hoặc nhu cầu bảo trì sớm.

33\.  AI tạo kết quả dự đoán gồm tài sản liên quan, mức rủi ro và khuyến nghị kiểm tra/bảo trì.

34\.  Hệ thống hiển thị kết quả cho Facility Manager.

35\.  Facility Manager xem kết quả dự đoán cùng dữ liệu liên quan.

36\.  Facility Manager quyết định bỏ qua, tiếp tục theo dõi, tạo Work Order hoặc đưa vào lịch bảo trì.

37\.  Hệ thống ghi nhận kết quả AI và hành động của Facility Manager để phục vụ truy vết.

## **Alternative Flows**

### **AF-01 — Mức rủi ro thấp**

38\.  AI đánh giá rủi ro thấp.

39\.  Hệ thống hiển thị kết quả để Facility Manager theo dõi và không tự động tạo Work Order.

### **AF-02 — Mức rủi ro cao**

40\.  AI đánh giá tài sản có nguy cơ cao hoặc cần bảo trì sớm.

41\.  Hệ thống làm nổi bật khuyến nghị.

42\.  Facility Manager xem xét và quyết định xử lý.

## **Exception Flows**

### **EF-01 — Dữ liệu đầu vào không đủ hoặc không hợp lệ**

43\.  AI không đưa ra dự đoán đáng tin cậy.

44\.  Hệ thống thông báo rằng chưa đủ dữ liệu hoặc dữ liệu không hợp lệ.

45\.  Không có Work Order nào được tạo tự động.

## **Data Requirements**

### **Input**

·       Asset ID.

·       Dữ liệu IoT hợp lệ của tài sản.

·       Lịch sử sự cố và bảo trì liên quan.

### **Output**

·       Tài sản liên quan.

·       Mức rủi ro dự đoán.

·       Khuyến nghị kiểm tra hoặc bảo trì.

·       Thời điểm tạo dự đoán.

## **Postconditions**

### **Success**

46\.  Facility Manager nhận được kết quả dự đoán có thể truy vết.

47\.  Kết quả AI không tự động thay thế quyết định của Facility Manager.

48\.  Nếu Facility Manager chọn xử lý, hành động tiếp theo được thực hiện qua chức năng Work Order hoặc lịch bảo trì.

### **Failure**

49\.  Không có quyết định tự động được thực hiện nếu AI không tạo được kết quả.

50\.  Dữ liệu gốc vẫn được giữ để có thể phân tích lại sau.

## **Open Questions**

51\.  Thang mức rủi ro của AI gồm những mức nào?

52\.  Bao nhiêu dữ liệu tối thiểu thì AI mới được phép đưa ra dự đoán?

53\.  Tần suất chạy dự đoán AI là theo thời gian cố định hay theo sự kiện dữ liệu?