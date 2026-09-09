# Requirement Review Log

> [!NOTE]
> * **Project:** Smart Maintenance & Facility Management
> * **Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
> * **Document Type:** AI-Assisted Requirement Review
> * **Status:** Draft / Pending Review

---

## 📋 Table of Contents
1. [Mục đích (Purpose)](#1-mục-đích-purpose)
2. [Quy tắc Review (Review Rules)](#2-quy-tắc-review-review-rules)
3. [Nội dung Review Chi tiết](#3-nội-dung-review-chi-tiết)
   * [3.1 Functional Requirements](#31-functional-requirements)
   * [3.2 Non-Functional Requirements](#32-non-functional-requirements)
   * [3.3 Business Rules](#33-business-rules)
   * [3.4 Constraints](#34-constraints)
   * [3.5 Assumptions](#35-assumptions)
4. [Tóm tắt Mâu thuẫn & Lỗ hổng (Contradiction & Gap Summary)](#4-tóm-tắt-mâu-thuẫn--lỗ-hổng-contradiction--gap-summary)
5. [Đánh giá Khả năng Kiểm thử (Testability Review)](#5-đánh-giá-khả-năng-kiểm-thử-testability-review)
6. [Câu hỏi Ưu tiên Cao nhất (Highest Priority Questions)](#6-câu-hỏi-ưu-tiên-cao-nhất-highest-priority-questions)
7. [Kết luận Review (AI Review Conclusion)](#7-kết-luận-review-ai-review-conclusion)
8. [Đề xuất Phạm vi MVP (Proposed MVP Scope)](#8-đề-xuất-phạm-vi-mvp-proposed-mvp-scope)
9. [Hành động Cần thiết từ Con người (Human Decision Required)](#9-hành-động-cần-thiết-từ-con-người-human-decision-required)

---

## 1. Mục đích (Purpose)

Tài liệu này ghi nhận kết quả **AI-assisted review** đối với các yêu cầu của hệ thống **Smart Maintenance & Facility Management** cho Trường Đại học Kinh tế – Đại học Đà Nẵng.

**Mục tiêu review:**
- Phát hiện requirement còn mơ hồ hoặc chưa testable.
- Phát hiện thiếu dữ liệu cần xác nhận.
- Phát hiện contradiction hoặc dependency giữa các requirement.
- Xác định các requirement có rủi ro cao đối với MVP.
- Đề xuất MVP scope từ góc nhìn Business Analyst (BA).

> [!IMPORTANT]
> Các nhận xét và đề xuất trong file này **không tự động trở thành requirement**. BA, Stakeholder và Project Team là người quyết định có sửa requirement hay không.

---

## 2. Quy tắc Review (Review Rules)

1. **Không tự tạo Business Rule** nếu INPUT không đề cập.
2. Nếu thiếu thông tin $\rightarrow$ Ghi `UNKNOWN` và tạo/liên kết Open Question.
3. **Không thay đổi Requirement ID** trong quá trình review.
4. **Không tự chuyển AI recommendation** thành confirmed requirement.
5. Requirement phải có khả năng kiểm thử (testable) hoặc cần được làm rõ để có thể kiểm thử.
6. Contradiction phải được đánh dấu để BA/Stakeholder quyết định.
7. Scope recommendation chỉ là đề xuất, không phải requirement đã xác nhận.

---

## 3. Nội dung Review Chi tiết

### 3.1 Functional Requirements

| ID | loại | Yêu cầu | Nguồn | Priority | Rủi ro / Điểm mơ hồ | Câu hỏi cần xác nhận |
| :--- | :--- | :--- | :--- | :---: | :--- | :--- |
| **REQ-01** | Functional | Người dùng có thể đăng nhập vào hệ thống theo tài khoản được cấp. | SRC-USER | `High` | Chưa xác định authentication mechanism và hành vi khi đăng nhập sai. | Hệ thống dùng tài khoản riêng hay SSO? |
| **REQ-02** | Functional | Requester có thể xem các Asset thuộc phòng/khu vực liên quan. | SRC-USER | `High` | “Liên quan” chưa định nghĩa rõ phạm vi truy cập. | Requester chỉ xem Asset của phòng/khu vực mình sử dụng hay toàn bộ Asset? |
| **REQ-03** | Functional | Requester có thể xem trạng thái cơ bản của Asset. | SRC-USER | `Medium` | “Trạng thái cơ bản” chưa xác định gồm những status nào. | Asset Status gồm những giá trị nào? |
| **REQ-04** | Functional | Requester có thể tạo Maintenance Request khi phát hiện vấn đề liên quan đến Asset. | SRC-USER | `High` | Chưa xác định required fields, attachment và điều kiện tạo request. | Request cần những thông tin bắt buộc nào? Có bắt buộc hình ảnh/video không? |
| **REQ-05** | Functional | Requester có thể theo dõi trạng thái của Maintenance Request đã tạo. | SRC-USER | `High` | Phụ thuộc vào bộ status chưa được xác định. | Maintenance Request có những status nào? |
| **REQ-06** | Functional | Facility Manager có thể thêm Asset vào hệ thống. | SRC-USER | `High` | Chưa xác định mandatory fields. | Khi tạo Asset, những trường nào bắt buộc? |
| **REQ-07** | Functional | Facility Manager có thể cập nhật thông tin Asset. | SRC-USER | `High` | Chưa xác định trường nào được phép chỉnh sửa. | Facility Manager được sửa toàn bộ Asset information hay chỉ một số trường? |
| **REQ-08** | Functional | Facility Manager có thể theo dõi thông tin và trạng thái của Asset. | SRC-USER | `High` | “Theo dõi” chưa xác định cụ thể view/filter/search nào cần có. | Cần những thông tin và thao tác monitoring nào? |
| **REQ-09** | Functional | Facility Manager có thể xem dữ liệu IoT liên quan đến Asset. | SRC-USER | `High` | Chưa xác định loại IoT data và cách hiển thị. | Asset sẽ có những loại IoT data nào? |
| **REQ-10** | Functional | Hệ thống có thể tạo và hiển thị IoT Alert khi dữ liệu IoT thỏa mãn điều kiện bất thường đã được xác định. | SRC-USER, SRC-BA | `High` | Cơ chế xác định bất thường chưa rõ. Có dependency với Q-09/Q-10. | Alert dựa trên threshold, anomaly detection hay cả hai? Threshold do ai cấu hình? |
| **REQ-11** | Functional | Facility Manager có thể xem AI Prediction về khả năng Asset cần bảo trì hoặc có nguy cơ hỏng hóc. | SRC-USER | `High` | “Cần bảo trì” và “nguy cơ hỏng hóc” là hai outcome khác nhau. Prediction chưa có measurable output. | AI dự đoán failure, maintenance need hay cả hai? |
| **REQ-12** | Functional | Hệ thống có thể hiển thị Maintenance Risk do AI xác định cho Asset. | SRC-USER, SRC-BA | `High` | Chưa xác định format và calculation của Risk. | Risk là Low/Medium/High, percentage hay cả hai? |
| **REQ-13** | Functional | Facility Manager có thể xem và xử lý Maintenance Request. | SRC-USER, SRC-BA | `High` | “Xử lý” chưa xác định các action cụ thể. | Facility Manager có thể approve, reject, assign, close request hay những action nào? |
| **REQ-14** | Functional | Facility Manager có thể tạo Work Order từ Maintenance Request hoặc nhu cầu bảo trì được xác định. | SRC-USER, SRC-BA | `High` | “Nhu cầu bảo trì được xác định” có thể đến từ AI/IoT nhưng chưa định nghĩa rõ. | Một Maintenance Request có thể tạo nhiều WO không? AI Prediction có trực tiếp tạo WO không? |
| **REQ-15** | Functional | Facility Manager có thể phân công Work Order cho Technician. | SRC-USER | `High` | Chưa xác định điều kiện assignment và có thể reassign hay không. | Technician có thể được reassign WO không? |
| **REQ-16** | Functional | Facility Manager có thể theo dõi trạng thái của Work Order. | SRC-USER, SRC-BA | `High` | Work Order status chưa được định nghĩa. | WO có những status nào? |
| **REQ-17** | Functional | Technician có thể xem các Work Order được phân công cho mình. | SRC-USER | `High` | Phụ thuộc vào assignment rule. | Technician có thể xem lịch sử WO của mình không? |
| **REQ-18** | Functional | Technician có thể xem thông tin Asset liên quan đến Work Order. | SRC-USER | `High` | Chưa xác định mức độ thông tin được phép xem. | Technician được xem những Asset fields nào? |
| **REQ-19** | Functional | Technician có thể xem IoT Alert và AI Prediction liên quan đến Asset của Work Order. | SRC-USER | `High` | Phụ thuộc vào availability và permission của IoT/AI data. | Technician có được xem toàn bộ AI/IoT data hay chỉ alert/risk liên quan? |
| **REQ-20** | Functional | Technician có thể cập nhật trạng thái Work Order được phân công. | SRC-USER | `High` | Chưa xác định status transition. | Technician được chuyển WO qua những trạng thái nào? |
| **REQ-21** | Functional | Technician có thể ghi nhận kết quả kiểm tra, sửa chữa hoặc bảo trì. | SRC-USER | `High` | Chưa xác định required information của maintenance result. | Kết quả bảo trì cần lưu những trường nào? |
| **REQ-22** | Functional | Technician có thể hoàn thành Work Order sau khi xử lý công việc. | SRC-USER | `High` | Điều kiện “hoàn thành” chưa được định nghĩa. | Có bắt buộc nhập kết quả bảo trì trước khi Complete WO không? |
| **REQ-23** | Functional | Hệ thống lưu kết quả Work Order vào Maintenance History của Asset. | SRC-USER, SRC-BA | `High` | Chưa xác định thời điểm tạo history và dữ liệu được copy. | History được tạo khi Technician Complete hay khi Facility Manager xác nhận? |
| **REQ-24** | Functional | Admin có thể quản lý tài khoản người dùng. | SRC-USER | `Medium` | “Quản lý” quá rộng. | Admin có thể create, update, deactivate và reset password không? |
| **REQ-25** | Functional | Admin có thể quản lý Role và quyền truy cập của người dùng. | SRC-USER | `High` | Có thể xung đột với việc Role là fixed 4 role trong MVP. | Role có fixed 4 role hay Admin có thể tạo Role mới? |
| **REQ-26** | Functional | Admin có thể liên kết Asset với IoT Device/Sensor. | SRC-USER | `High` | Chưa xác định cardinality và mapping lifecycle. | Một Asset có thể có nhiều IoT Device/Sensor không? |
| **REQ-27** | Functional | Admin có thể cập nhật thông tin IoT Mapping giữa Asset và IoT Device/Sensor. | SRC-USER | `High` | Chưa xác định mapping fields và hành vi khi thay đổi device. | Khi thay IoT Device/Sensor, dữ liệu mapping cũ được xử lý thế nào? |
| **REQ-28** | Functional | Hệ thống có thể thu thập và lưu trữ IoT Data liên quan đến Asset. | SRC-USER | `High` | Chưa xác định protocol, frequency, data format và retention. | IoT Data được thu thập real-time hay periodic? Lưu trong bao lâu? |
| **REQ-29** | Functional | Hệ thống sử dụng IoT Data và Maintenance History làm nguồn dữ liệu cho AI Prediction. | SRC-USER | `High` | Chưa xác định minimum data quality/quantity và AI processing method. | Nếu Maintenance History không đủ thì AI Prediction hoạt động thế nào? |

---

### 3.2 Non-functional Requirements

| ID | Loại | Yêu cầu | Nguồn | Priority | Rủi ro / Điểm mơ hồ | Câu hỏi cần xác nhận |
| :--- | :--- | :--- | :--- | :---: | :--- | :--- |
| **NFR-01** | NFR | Hệ thống phải áp dụng phân quyền dựa trên Role. | SRC-BA | `High` | Chưa có permission matrix. | Mỗi Role được phép thực hiện action nào? |
| **NFR-02** | NFR | Thông tin xác thực và dữ liệu người dùng phải được bảo vệ khỏi truy cập trái phép. | SRC-BA | `High` | Chưa có security criteria cụ thể. | Có security standard hoặc authentication requirement nào cần tuân thủ không? |
| **NFR-03** | NFR | Dữ liệu giữa Maintenance Request, Work Order và Maintenance History phải đảm bảo tính nhất quán. | SRC-BA | `High` | “Consistency” chưa có test criteria cụ thể. | Những field/link nào bắt buộc phải nhất quán? |
| **NFR-04** | NFR | Các bản ghi nghiệp vụ quan trọng phải lưu thời gian tạo/cập nhật tương ứng. | SRC-BA | `Medium` | “Quan trọng” chưa được định nghĩa đầy đủ. | Những entity nào bắt buộc có created/updated timestamp? |
| **NFR-05** | NFR | Việc thu thập IoT Data phải được thực hiện theo khoảng thời gian có thể cấu hình. | SRC-BA | `High` | Chưa có interval mặc định hoặc range. | Interval tối thiểu/tối đa là bao nhiêu? Ai được cấu hình? |
| **NFR-06** | NFR | Hệ thống phải có khả năng mở rộng khi số lượng Asset và IoT Device/Sensor tăng lên. | SRC-BA | `Medium` | Không có measurable scalability target. | MVP cần support tối đa bao nhiêu Asset/IoT Device? |
| **NFR-07** | NFR | Giao diện phải hiển thị thông tin phù hợp với Role. | SRC-BA | `Medium` | Chưa có permission/UI matrix. | Role nào được xem dữ liệu nào? |
| **NFR-08** | NFR | Mỗi AI Prediction phải được liên kết chính xác với Asset và thời điểm prediction. | SRC-BA | `High` | “Chính xác” chưa có validation criteria. | Prediction record cần lưu thêm model/version hoặc confidence score không? |

---

### 3.3 Business Rules

| ID | Loại | Yêu cầu | Nguồn | Priority | Rủi ro / Điểm mơ hồ | Câu hỏi cần xác nhận |
| :--- | :--- | :--- | :--- | :---: | :--- | :--- |
| **BR-01** | BR | Mỗi Asset phải có một Asset ID duy nhất. | SRC-BA | `High` | Không có major gap. | Asset ID do hệ thống tự sinh hay người dùng nhập? |
| **BR-02** | BR | Mỗi Asset phải thuộc một Asset Type được hệ thống hỗ trợ. | SRC-USER | `High` | Phạm vi Asset Type được giới hạn bởi MVP. | Có cho phép Asset Type ngoài MVP trong database không? |
| **BR-03** | BR | MVP quản lý 5 Asset Type: Wi-Fi, Air Conditioner, Projector, Light và Fan. | SRC-USER | `High` | Không có contradiction. | Không cần xác nhận thêm nếu đây là scope đã chốt. |
| **BR-04** | BR | Asset phải được gắn với một Location/Room xác định. | SRC-USER | `High` | Chưa xác định Location hierarchy. | Location có cấu trúc Campus $\rightarrow$ Building $\rightarrow$ Floor $\rightarrow$ Room không? |
| **BR-05** | BR | Maintenance Request phải xác định Asset hoặc khu vực bị ảnh hưởng. | SRC-BA | `High` | “Asset hoặc khu vực” tạo hai cách xác định request. | Requester có bắt buộc chọn Asset không nếu không biết Asset ID? |
| **BR-06** | BR | Facility Manager có quyền tạo và phân công Work Order cho Technician. | SRC-USER, SRC-BA | `High` | Không có major gap. | Có giới hạn Technician theo khu vực/chuyên môn không? |
| **BR-07** | BR | Technician chỉ được cập nhật Work Order được phân công cho mình. | SRC-BA | `High` | Chưa rõ Facility Manager/Admin có ngoại lệ không. | Admin/Facility Manager có được cập nhật WO của Technician không? |
| **BR-08** | BR | Mỗi Work Order phải được liên kết với Asset liên quan. | SRC-BA | `High` | Có thể conflict với BR-05 nếu Request chỉ xác định khu vực. | Nếu Maintenance Request chỉ có khu vực, Asset nào được gắn cho WO? |
| **BR-09** | BR | Work Order đã hoàn thành phải được lưu vào Maintenance History của Asset. | SRC-BA | `High` | Phụ thuộc vào thời điểm “hoàn thành” được xác nhận. | Complete WO có tự động tạo History không? |
| **BR-10** | BR | AI chỉ đóng vai trò hỗ trợ ra quyết định; Facility Manager quyết định hành động bảo trì cuối cùng. | SRC-BA | `High` | Không có major gap. | Có cần lưu lại decision của Facility Manager đối với AI recommendation không? |
| **BR-11** | BR | AI Prediction phải sử dụng dữ liệu phù hợp, bao gồm IoT Data và Maintenance History khi dữ liệu khả dụng. | SRC-USER, SRC-BA | `High` | “Khi dữ liệu khả dụng” chưa định nghĩa minimum data requirement. | Khi chỉ có IoT Data hoặc chỉ có History, AI có được prediction không? |
| **BR-12** | BR | IoT Alert chỉ được tạo khi IoT Data thỏa mãn điều kiện bất thường đã xác định/cấu hình. | SRC-BA | `High` | Chưa xác định cách xác định abnormal condition. | Threshold/rule do ai định nghĩa và cấu hình? |
| **BR-13** | BR | IoT Device/Sensor phải được mapping với Asset trước khi dữ liệu được sử dụng cho monitoring Asset. | SRC-USER, SRC-BA | `High` | Không có major gap. | Có cho phép lưu raw IoT data trước khi mapping không? |
| **BR-14** | BR | Maintenance Request phải có trạng thái để theo dõi vòng đời xử lý. | SRC-BA | `High` | Status values và transitions chưa xác định. | Bộ status chính thức là gì? |
| **BR-15** | BR | Asset có Maintenance Risk cao có thể được Facility Manager ưu tiên xử lý. | SRC-BA | `Medium` | “Có thể” khiến rule không bắt buộc và khó test. | High Risk có bắt buộc tạo/ưu tiên maintenance hay chỉ là recommendation? |

---

### 3.4 Constraints

| ID | Loại | Yêu cầu | Nguồn | Priority | Rủi ro / Điểm mơ hồ | Câu hỏi cần xác nhận |
| :--- | :--- | :--- | :--- | :---: | :--- | :--- |
| **CON-01** | Constraint | Hệ thống được xây dựng cho Trường Đại học Kinh tế – Đại học Đà Nẵng. | SRC-USER | `High` | Không có major gap. | Không cần nếu scope đã xác nhận. |
| **CON-02** | Constraint | MVP chỉ quản lý 5 Asset Type. | SRC-USER | `High` | Không có major gap. | Không cần nếu đã chốt MVP. |
| **CON-03** | Constraint | MVP hỗ trợ tối thiểu 4 Role. | SRC-USER | `High` | “Tối thiểu” có thể cho phép thêm Role. | MVP có đúng 4 Role hay có thể thêm Role khác? |
| **CON-04** | Constraint | MVP bắt buộc có IoT. | SRC-USER | `High` | Chưa định nghĩa mức độ IoT tối thiểu để được xem là đáp ứng. | MVP cần tích hợp IoT thật hay prototype/simulation được chấp nhận? |
| **CON-05** | Constraint | MVP bắt buộc có AI phục vụ Predictive Maintenance. | SRC-USER | `High` | Chưa xác định mức độ AI tối thiểu. | Prototype AI prediction bằng model thật, historical dataset hay simulation có được chấp nhận? |
| **CON-06** | Constraint | MVP không bao gồm Finance, Procurement, Inventory/Spare Parts và các nghiệp vụ ngoài Facility Maintenance. | SRC-USER | `High` | Không có major gap. | Không cần nếu scope đã xác nhận. |
| **CON-07** | Constraint | AI không được tự động thay thế quyết định của Facility Manager. | SRC-BA | `High` | Cần đảm bảo implementation không tự động tạo maintenance action. | Có cần lưu decision của Facility Manager để audit không? |

---

### 3.5 Assumptions

| ID | Loại | Yêu cầu | Nguồn | Priority | Rủi ro / Điểm mơ hồ | Câu hỏi cần xác nhận |
| :--- | :--- | :--- | :--- | :---: | :--- | :--- |
| **ASM-01** | ASM | Người dùng có tài khoản hợp lệ để truy cập hệ thống. | SRC-BA | `High` | Nếu không có account provisioning thì REQ-01 chưa test được. | Account được tạo bởi Admin hay tích hợp hệ thống hiện có? |
| **ASM-02** | ASM | Nhà trường có hoặc có thể cung cấp dữ liệu cơ bản về Asset. | SRC-BA | `High` | Nếu dữ liệu không đầy đủ sẽ ảnh hưởng Asset Management. | Nhà trường hiện có Asset dataset nào? |
| **ASM-03** | ASM | Các Asset trong MVP có thể được kết nối với IoT Device/Sensor phù hợp. | SRC-BA | `High` | Khả năng triển khai IoT thực tế chưa được xác nhận. | 5 Asset Type có IoT integration khả thi trong MVP không? |
| **ASM-04** | ASM | Nền tảng IoT có thể cung cấp dữ liệu về tình trạng/hoạt động của Asset. | SRC-BA | `High` | Chưa xác định platform và data format. | IoT platform/device nào sẽ được sử dụng? |
| **ASM-05** | ASM | Maintenance History có thể được thu thập và lưu trữ đủ để phục vụ AI Prediction. | SRC-BA | `High` | Đây là dependency quan trọng nhất của AI. | Dữ liệu lịch sử hiện có đủ để train/validate model không? |
| **ASM-06** | ASM | Facility Manager có quyền truy cập dữ liệu Asset, IoT và AI cần thiết. | SRC-BA | `Medium` | Có thể conflict nếu permission matrix hạn chế một số dữ liệu. | Facility Manager cần xem dữ liệu nào? |
| **ASM-07** | ASM | Technician có thiết bị phù hợp để truy cập và cập nhật Work Order. | SRC-BA | `Medium` | Chưa xác định web/mobile/device requirement. | Technician sử dụng desktop, tablet hay mobile? |
| **ASM-08** | ASM | Hệ thống được triển khai trên hạ tầng/network được nhà trường cho phép. | SRC-BA | `Medium` | Chưa biết deployment environment. | Hệ thống triển khai on-premise hay cloud? |

---

## 4. Tóm tắt Mâu thuẫn & Lỗ hổng (Contradiction & Gap Summary)

> [!WARNING]
> Các điểm mâu thuẫn cần được họp làm rõ trước khi chuyển sang giai đoạn Thiết kế Chi tiết (Detailed Design).

* **C-01 — BR-05 vs BR-08 (Xung đột xác định Asset):**
  * `BR-05`: Maintenance Request có thể xác định Asset **hoặc khu vực**.
  * `BR-08`: Mỗi Work Order **bắt buộc** phải được liên kết với Asset.
  * 🛑 *Gap logic:* Nếu Requester chỉ báo lỗi theo khu vực (không có Asset ID), Work Order sẽ không thể khởi tạo nếu áp dụng `BR-08`.
  * *Hướng xử lý:* Facility Manager phải xác định Asset trước khi bấm tạo Work Order, HOẶC cho phép Work Order tồn tại theo Location trước khi gán Asset. *(Status: Open)*

* **C-02 — REQ-25 vs CON-03 (Phạm vi Quản lý Role):**
  * `CON-03`: MVP hỗ trợ tối thiểu 4 Role cố định.
  * `REQ-25`: Admin có thể quản lý Role và quyền truy cập.
  * 🛑 *Xung đột:* Nếu Admin có quyền tạo mới/xóa Role thì phạm vi 4 Role bị phá vỡ.
  * *Hướng xử lý:* Khóa cố định (fixed) 4 Role cho MVP; Admin chỉ được gán/hủy gán Role cho User. *(Status: Open)*

* **C-03 — REQ-11 vs REQ-12 (Đầu ra AI Prediction):**
  * `REQ-11`: AI dự đoán khả năng "cần bảo trì" hoặc "nguy cơ hỏng hóc".
  * `REQ-12`: AI hiển thị "Maintenance Risk".
  * 🛑 *Chưa rõ:* Prediction và Risk là 2 màn hình/output riêng biệt hay Risk chỉ là cách hiển thị mức độ của Prediction? *(Status: Gap)*

* **C-04 — REQ-29 vs ASM-05 (Rủi ro Phụ thuộc Dữ liệu AI):**
  * `REQ-29`: Yêu cầu AI dùng cả IoT Data + Maintenance History.
  * `ASM-05`: Giả định nhà trường *có thể* có đủ dữ liệu lịch sử.
  * 🛑 *High Risk:* Trường hợp hệ thống mới triển khai hoàn toàn chưa có Maintenance History, mô hình AI sẽ không thể hoạt động theo đúng `REQ-29`. *(Status: High Risk)*

* **C-05 — REQ-10 vs BR-12 (Cơ chế IoT Alert):**
  * `REQ-10` & `BR-12`: Cảnh báo khi dữ liệu IoT vượt threshold bất thường.
  * 🛑 *Chưa xác định:* Threshold tĩnh do người dùng tự nhập hay dùng AI Anomaly Detection? AI/User nào cấu hình? *(Status: High Risk)*

---

## 5. Đánh giá Khả năng Kiểm thử (Testability Review)

Các từ khóa mơ hồ (Vague Terms) sau đây cần phải cụ thể hóa bằng thông số/danh sách định lượng:

| ID | Non-testable / Vague Term | Vấn đề cần cụ thể hóa |
| :--- | :--- | :--- |
| **REQ-02** | *"phòng/khu vực liên quan"* | Chưa định nghĩa phạm vi truy cập (Access Scope). |
| **REQ-03** | *"trạng thái cơ bản"* | Chưa có danh sách status cụ thể của Asset. |
| **REQ-08** | *"theo dõi"* | Chưa rõ các tính năng UI (Filter, Search, Export, View detail). |
| **REQ-10** | *"điều kiện bất thường"* | Chưa có thông số ngưỡng (Threshold values / Rule). |
| **REQ-11** | *"có nguy cơ" / "cần bảo trì"* | Chưa định nghĩa thang đo và Output tiêu chuẩn. |
| **REQ-12** | *"Maintenance Risk"* | Chưa có Risk Scale (ví dụ: % hay Low/Medium/High). |
| **REQ-13** | *"xử lý"* | Chưa có danh sách Action (Approve, Reject, Reassign, Close). |
| **REQ-15** | *"phân công"* | Chưa có Assignment Rules (chuyên môn, ca trực). |
| **REQ-21** | *"ghi nhận kết quả"* | Chưa quy định các trường thông tin bắt buộc nhập khi hoàn tất. |
| **REQ-24** | *"quản lý tài khoản"* | Chưa chốt phạm vi CRUD / Action. |
| **REQ-25** | *"quản lý Role và quyền"* | Chưa có Permission Matrix chi tiết. |
| **REQ-28** | *"thu thập"* | Chưa có Tần suất (Frequency) và Định dạng dữ liệu (Data format). |
| **NFR-03** | *"đảm bảo tính nhất quán"* | Chưa có Tiêu chí kiểm thử tính toàn vẹn (Consistency test criteria). |
| **NFR-06** | *"có khả năng mở rộng"* | Chưa có con số tải chịu đựng cụ thể (Capacity Target). |
| **NFR-07** | *"phù hợp với Role"* | Chưa có UI/Permission Matrix theo từng vai trò. |
| **BR-15** | *"có thể ưu tiên"* | Từ khóa "có thể" không thể lập kịch bản Test tự động/chính xác. |

---

## 6. Câu hỏi Ưu tiên Cao nhất (Highest Priority Questions)

```txt
┌───┬───────────────────────────────────────────────────────────────────────────┬────────────────────────┐
│ # │ Câu hỏi cần giải quyết                                                    │ Requirement IDs liên quan│
├───┼───────────────────────────────────────────────────────────────────────────┼────────────────────────┤
│ 1 │ AI Prediction dự đoán chính xác điều gì? (Failure rate hay time-to-fail?) │ REQ-11, REQ-12, BR-11  │
│ 2 │ IoT Alert dựa vào Threshold tĩnh cố định hay AI Anomaly Detection?         │ REQ-10, BR-12          │
│ 3 │ Dữ liệu lịch sử (Maintenance History) có đủ để train AI không?            │ REQ-23, REQ-29, ASM-05 │
│ 4 │ IoT Data được đẩy theo Real-time hay Periodic (chu kỳ định kỳ)?           │ REQ-28, NFR-05         │
│ 5 │ Danh sách Status chính thức của Maintenance Request & Work Order là gì?   │ REQ-05, REQ-16, BR-14  │
│ 6 │ Nếu Request báo lỗi không có Asset ID, Work Order sẽ gán Asset thế nào?   │ BR-05, BR-08           │
│ 7 │ Một Asset được gắn tối đa bao nhiêu IoT Device/Sensor trong MVP?          │ REQ-26, REQ-27         │
│ 8 │ Phạm vi Role trong MVP là 4 Role Cố định hay cho phép tạo Role mới?       │ CON-03, REQ-25         │
└───┴───────────────────────────────────────────────────────────────────────────┴────────────────────────┘
