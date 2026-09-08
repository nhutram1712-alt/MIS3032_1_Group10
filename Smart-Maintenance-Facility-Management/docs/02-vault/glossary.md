# Glossary
### 3.1. Purpose
Glossary này định nghĩa các thuật ngữ chính được sử dụng trong dự án **Smart Maintenance & Facility Management System** tại Trường Đại học Kinh tế – Đại học Đà Nẵng.

Mục đích của Glossary là đảm bảo BA, Developer, Tester, AI và các thành viên trong nhóm sử dụng cùng một cách hiểu và cách gọi thuật ngữ trong toàn bộ tài liệu dự án.

---

### 3.2. Terminology

| # | Term | Vietnamese | Definition |
| --- | --- | --- | --- |
| 1 | **Asset** | Tài sản | Thiết bị hoặc tài sản cơ sở vật chất được nhà trường quản lý và theo dõi trong hệ thống, bao gồm Wi-Fi, điều hòa, máy chiếu, đèn và quạt. |
| 2 | **Asset ID** | Mã tài sản | Mã định danh duy nhất của mỗi Asset trong hệ thống, dùng để phân biệt và truy xuất thông tin của từng tài sản. |
| 3 | **Asset Type** | Loại tài sản | Nhóm phân loại của Asset, bao gồm Wi-Fi, Air Conditioner, Projector, Light và Fan trong phạm vi MVP. |
| 4 | **Asset Status** | Trạng thái tài sản | Trạng thái hoạt động hiện tại của Asset, ví dụ: Operational, Warning, Maintenance hoặc Out of Service. |
| 5 | **Requester** | Người yêu cầu | Sinh viên, giảng viên hoặc nhân viên tại Trường Đại học Kinh tế – ĐHĐN có quyền gửi Maintenance Request khi phát hiện sự cố tài sản. |
| 6 | **Technician** | Kỹ thuật viên | Người chịu trách nhiệm kiểm tra, sửa chữa hoặc bảo trì tài sản theo Work Order được phân công. |
| 7 | **Facility Manager** | Quản lý cơ sở vật chất | Người/bộ phận chịu trách nhiệm quản lý tài sản, theo dõi tình trạng thiết bị, tiếp nhận yêu cầu và điều phối hoạt động bảo trì. |
| 8 | **Admin** | Quản trị viên | Người quản trị hệ thống, chịu trách nhiệm quản lý tài khoản, phân quyền và cấu hình kết nối giữa Asset với IoT Device/Sensor. |
| 9 | **Maintenance** | Bảo trì | Hoạt động kiểm tra, bảo dưỡng hoặc sửa chữa nhằm duy trì hoặc khôi phục trạng thái hoạt động của Asset. |
| 10 | **Maintenance Request** | Yêu cầu bảo trì/báo lỗi | Yêu cầu được tạo bởi Requester để thông báo một Asset đang gặp sự cố hoặc cần được kiểm tra/bảo trì. |
| 11 | **Work Order (WO)** | Lệnh công việc bảo trì | Công việc bảo trì chính thức được Facility Manager tạo và phân công cho Technician để xử lý một Maintenance Request hoặc một nhu cầu bảo trì được xác định từ hệ thống. |
| 12 | **Maintenance History** | Lịch sử bảo trì | Tập hợp thông tin về các lần kiểm tra, sửa chữa hoặc bảo trì đã được thực hiện đối với một Asset. |
| 13 | **IoT (Internet of Things)** | Internet vạn vật | Công nghệ cho phép các thiết bị/sensor kết nối với hệ thống để thu thập và truyền dữ liệu về tình trạng hoặc hoạt động của Asset. |
| 14 | **IoT Device / Sensor** | Thiết bị/Sensor IoT | Thiết bị được kết nối với hệ thống để thu thập dữ liệu liên quan đến Asset, chẳng hạn nhiệt độ, trạng thái hoạt động, thời gian vận hành hoặc mức tiêu thụ điện. |
| 15 | **IoT Data** | Dữ liệu IoT | Dữ liệu được thu thập từ IoT Device/Sensor về tình trạng và hoạt động của Asset. |
| 16 | **IoT Alert** | Cảnh báo IoT | Cảnh báo được hệ thống tạo ra khi dữ liệu IoT cho thấy Asset có dấu hiệu bất thường theo các điều kiện đã được xác định. |
| 17 | **IoT Mapping** | Liên kết Asset – IoT | Quan hệ xác định IoT Device/Sensor nào được kết nối và sử dụng để theo dõi một Asset cụ thể. |
| 18 | **AI Prediction** | Dự đoán AI | Kết quả do mô hình AI tạo ra dựa trên dữ liệu IoT, thông tin Asset và lịch sử bảo trì nhằm đánh giá khả năng Asset gặp sự cố hoặc cần bảo trì. |
| 19 | **Maintenance Risk** | Rủi ro bảo trì | Mức độ rủi ro cho biết khả năng một Asset có thể gặp sự cố hoặc cần được kiểm tra/bảo trì trong một khoảng thời gian xác định. |
| 20 | **Predictive Maintenance** | Bảo trì dự đoán | Phương pháp sử dụng dữ liệu tình trạng và lịch sử hoạt động/bảo trì kết hợp với AI để dự đoán nhu cầu bảo trì trước khi Asset xảy ra sự cố nghiêm trọng. |

---

### 3.3. Standard Terminology Rules
Để đảm bảo tính nhất quán trong toàn bộ dự án, nhóm thống nhất các quy tắc sau:

1. **Asset**
   * Luôn sử dụng **Asset** khi đề cập đến tài sản được quản lý trong hệ thống.
   * Không sử dụng lẫn lộn: `Device`, `Equipment`, `Machine`, `Facility` nếu đang đề cập đến đối tượng được quản lý trong Asset Management.
   * *Ví dụ:* Projector P-001 là một Asset.

2. **IoT Device/Sensor**
   * IoT Device/Sensor là thiết bị dùng để thu thập dữ liệu và **không đồng nghĩa với Asset**.
   * *Ví dụ:*
     * Asset: `AC-001 – Air Conditioner`
     * IoT Sensor: `TEMP-001`
   * Một Asset có thể được liên kết với một hoặc nhiều IoT Device/Sensor tùy loại thiết bị.

3. **Maintenance Request vs Work Order**
   * Hai khái niệm này phải được phân biệt rõ:
     * **Maintenance Request:** Người dùng báo có vấn đề.
     * **Work Order:** Facility Manager chính thức giao công việc xử lý cho Technician.
   * **Luồng chuẩn:** `Requester` → `Maintenance Request` → `Facility Manager` → `Work Order` → `Technician`

4. **IoT Alert vs AI Prediction**
   * IoT Alert và AI Prediction không phải cùng một loại cảnh báo.
   * **IoT Alert:** Phát hiện dữ liệu hiện tại có dấu hiệu bất thường.
   * **AI Prediction:** Dựa trên dữ liệu để dự đoán khả năng xảy ra sự cố hoặc nhu cầu bảo trì trong tương lai.
   * *Ví dụ:* 
     * IoT phát hiện nhiệt độ máy chiếu cao bất thường → **IoT Alert**
     * AI phân tích nhiệt độ + thời gian sử dụng + lịch sử bảo trì và dự đoán máy chiếu có nguy cơ cần bảo trì trong 7 ngày → **AI Prediction**

5. **Predictive Maintenance**
   * Predictive Maintenance là mục tiêu nghiệp vụ của việc sử dụng IoT + AI trong project, không phải tên của một chức năng đơn lẻ.
   * **Luồng khái niệm:** `IoT Data` → `Condition Monitoring` → `AI Prediction` → `Maintenance Recommendation` → `Facility Manager Decision` → `Work Order`

---

### 3.4. Naming Convention
Nhóm thống nhất sử dụng **English terms** làm tên chính trong:
* Database
* API
* Source code
* Use Case
* User Story ID/Title
* UI technical specification
* AI/IoT documentation

Tiếng Việt được sử dụng trong phần giải thích nghiệp vụ và tài liệu trình bày.

*Ví dụ:*
* Asset Management – Quản lý tài sản
* Maintenance Request – Yêu cầu bảo trì
* Work Order – Lệnh công việc bảo trì
* Facility Manager – Quản lý cơ sở vật chất

Việc thống nhất terminology này giúp tránh việc cùng một đối tượng được gọi bằng nhiều tên khác nhau trong các tài liệu và trong quá trình phát triển hệ thống.
