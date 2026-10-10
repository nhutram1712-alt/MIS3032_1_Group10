# Test case — Smart Maintenance & Facility Management

Cấu trúc giữ như file Excel: header dự án → thống kê Hoàn thành/Lỗi → bảng cột Module | ID | Kịch bản | Quy trình | Mong đợi | Thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi.

## 0. Tổng hợp

**Tên dự án:** Smart Maintenance & Facility Management (DUE Smart Campus)  
**Môi trường test:** FE http://localhost:5173  ·  BE http://127.0.0.1:5031  ·  mật khẩu demo Due@2026  
**Thời gian test:** 01/09/2026 – 09/10/2026 (gồm vòng test đầu và vòng kiểm tra lại các case Failed)  
**Tài khoản dùng khi test:** manager / Due@2026 (Facility Manager) · tech1 / Due@2026 (Technician) · requester / Due@2026 (Requester) · admin / Due@2026 (Admin)  

| Sheet / Module | Hoàn thành | Lỗi | Chưa kiểm tra | Tổng | Người phụ trách chính |
|---|---:|---:|---:|---:|---|
| 1. Đăng nhập | 11 | 0 | 0 | 11 | Trinh |
| 2. Tài sản | 9 | 0 | 0 | 9 | Thu Thảo, Trinh |
| 3. Yêu cầu sự cố | 6 | 0 | 0 | 6 | Trinh |
| 4. Work Order | 8 | 1 | 0 | 9 | Trinh, Thanh Thảo |
| 5. IoT Alerts | 4 | 2 | 0 | 6 | Trầm, Thịnh |
| 6. AI Risks | 3 | 1 | 0 | 4 | Trầm, Thịnh |
| 7. Quản trị | 6 | 2 | 0 | 8 | Trinh, Thu Thảo, Thanh Thảo, Thịnh |
| **TỔNG** | **47** | **6** | **0** | **53** |  |

| Người phụ trách | Số case | Tỷ lệ | Pass cuối | Failed cuối |
|---|---:|---:|---:|---:|
| Trinh | 26 | 49% | 26 | 0 |
| Thu Thảo | 7 | 13% | 7 | 0 |
| Thanh Thảo | 7 | 13% | 6 | 1 |
| Trầm | 7 | 13% | 5 | 2 |
| Thịnh | 6 | 11% | 3 | 3 |

**Lỗi còn mở (tính đến 09/10/2026)**

- TC_UC04.2_05 — Tiêu đề trang và menu vẫn ghi Work Order (chưa thuần Việt). Pill trạng thái đã là tiếng Việt.
- TC_UC05.1_02 — Cột chỉ số còn raw power_status / temperature; mức độ còn High.
- TC_UC05.2_03 — Trang IoT Mapping không có nút gỡ liên kết (Unmap) hoặc sửa Device ID.
- TC_UC06.1_03 — Mức rủi ro AI còn High / Medium, chưa đổi thành Cao / Trung bình.
- TC_UC07.2_03 — Tài khoản Facility Manager vẫn mở được trang Người dùng nếu nhập địa chỉ trực tiếp.

## 1. Đăng nhập

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | Đăng nhập & phân quyền |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 11 | 0 | 0 | 11 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC01.1 MODULE Đăng nhập** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Đăng nhập | TC_UC01.1_01 | Đăng nhập thành công bằng tài khoản Facility Manager | 1. Mở trang Đăng nhập<br>2. Chọn tài khoản Facility Manager (manager)<br>3. Nhấn "Đăng nhập" | Vào được hệ thống. Góc trái hiện Nguyễn Văn Quản / Facility Manager. Trang mặc định là Tổng quan (Smart Campus Facility Management). Menu có: Tổng quan, Tài sản, Work Order, IoT Alerts, AI Risks. | Đăng nhập thành công. Dashboard Tổng quan hiện 78 tài sản, 25 Work Order. Menu và user chip đúng như mong đợi. | TC_UC01.1_01.png | Trinh | 02/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_02 | Đăng nhập thất bại khi nhập sai mật khẩu | 1. Mở trang Đăng nhập<br>2. Nhập tên đăng nhập manager<br>3. Nhập mật khẩu sai<br>4. Nhấn "Đăng nhập" | Hệ thống không cho vào app. Hiện thông báo lỗi rõ. Vẫn ở trang Đăng nhập, các ô nhập còn đó để sửa lại. | Hiện dòng đỏ «Invalid username or password.». Không vào được bên trong. Vẫn ở màn Đăng nhập. | TC_UC01.1_02.png | Trinh | 02/09/2026 | Failed | 05/09/2026 | Pass | Pass |  |
| Đăng nhập | TC_UC01.1_03 | Bỏ trống tên đăng nhập thì không cho submit | 1. Mở trang Đăng nhập<br>2. Để trống tên đăng nhập và mật khẩu<br>3. Nhấn "Đăng nhập" | Không gọi đăng nhập. Ô Tên đăng nhập bị đánh dấu bắt buộc. Người dùng không vào được hệ thống. | Trình duyệt chặn submit, hiện «Please fill out this field.» trên ô Tên đăng nhập. | TC_UC01.1_03.png | Trinh | 03/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_04 | Có tên đăng nhập nhưng bỏ trống mật khẩu thì không cho submit | 1. Mở trang Đăng nhập<br>2. Nhập tên đăng nhập manager<br>3. Để trống mật khẩu<br>4. Nhấn "Đăng nhập" | Không đăng nhập được. Ô Mật khẩu báo phải nhập. | Trình duyệt chặn submit trên ô Mật khẩu (trường required). Không vào app. | TC_UC01.1_03.png | Trinh | 03/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_05 | Nhấn pill demo thì hệ thống tự điền đúng tài khoản và mật khẩu | 1. Mở trang Đăng nhập<br>2. Nhấn pill Requester: requester<br>3. Nhấn pill Admin: admin<br>4. Kiểm tra ô tên đăng nhập và mật khẩu đã đổi theo pill | Mỗi lần nhấn pill, ô Tên đăng nhập = tên pill, ô Mật khẩu = Due@2026. Không tự đăng nhập cho đến khi nhấn nút Đăng nhập. | Pill điền đúng username tương ứng và mật khẩu Due@2026. Vẫn ở trang Đăng nhập. | TC_UC01.1_01_login.png | Trinh | 04/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_06 | Menu Facility Manager đủ chức năng vận hành, không có mục quản trị | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Quan sát các mục menu bên trái | Menu đúng 5 mục: Tổng quan, Tài sản, Work Order, IoT Alerts, AI Risks. Không thấy menu quản trị người dùng / mapping IoT. | Menu đúng 5 mục vận hành. User chip: Nguyễn Văn Quản · Facility Manager. | TC_UC01.1_04.png | Trinh | 04/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_07 | Menu Requester chỉ còn Tổng quan và Yêu cầu sự cố | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Quan sát các mục menu bên trái | Chỉ 2 mục: Tổng quan, Yêu cầu sự cố. Góc trái hiện Nguyễn Thị Yêu Cầu / Requester. | Menu đúng 2 mục. User: Nguyễn Thị Yêu Cầu · Requester. | TC_UC01.1_05.png | Trinh | 05/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_08 | Menu Technician có Work Order, Tài sản & AI, IoT Alerts | 1. Đăng nhập bằng tài khoản Technician (tech1)<br>2. Quan sát các mục menu bên trái | Đúng 4 mục menu kỹ thuật. User: Trần Văn Kỹ / Technician. | Menu đúng 4 mục. User: Trần Văn Kỹ · Technician. | TC_UC01.1_06.png | Trinh | 05/09/2026 | Pass |  |  | Pass |  |
| Đăng nhập | TC_UC01.1_09 | Đăng nhập Admin thì vào thẳng trang Người dùng, menu chỉ chức năng quản trị | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Quan sát trang mặc định và menu bên trái | Sau đăng nhập, trang chính là Người dùng (không phải dashboard vận hành). Menu 3 mục. User: Lê Thị Quản Trị / Admin. | Vào thẳng Người dùng. Menu 3 mục. User: Lê Thị Quản Trị · Admin. | TC_UC01.1_07.png | Trinh | 08/09/2026 | Pass |  |  | Pass |  |
| **UC01.2 MODULE Đăng xuất** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Đăng xuất | TC_UC01.2_01 | Đăng xuất từ nút dưới menu thì về lại trang Đăng nhập | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Nhấn "Đăng xuất" | Hệ thống đưa về trang Đăng nhập. Hai ô nhập trống. Không còn menu bên trái của app. | Về đúng form Đăng nhập Smart Campus, các ô trống, có pill demo. | TC_UC01.2_01.png | Trinh | 08/09/2026 | Failed | 12/09/2026 | Pass | Pass |  |
| Đăng xuất | TC_UC01.2_02 | Sau khi đăng xuất, bấm Quay lại trình duyệt cũng không vào được trang đã login | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Nhấn "Đăng xuất"<br>3. Nhấn nút quay lại trên trình duyệt | Không xem được dữ liệu nội bộ. Hệ thống yêu cầu đăng nhập lại. | Sau đăng xuất, không còn session. Mở lại trang nội bộ thì bị đưa về Đăng nhập. | TC_UC01.2_02.png | Trinh | 09/09/2026 | Pass |  |  | Pass |  |

## 2. Tài sản

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | Tài sản |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 9 | 0 | 0 | 9 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC02.1 MODULE Danh mục tài sản** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Tài sản | TC_UC02.1_01 | Facility Manager mở danh mục cơ sở vật chất từ menu | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Tài sản" | Mở trang «Danh mục cơ sở vật chất». Có bộ lọc, tab loại (Tất cả, Điều hòa, Máy chiếu…), bảng thiết bị (Tên, Vị trí, Trạng thái, Rủi ro) và nút thêm mới. | Trang danh mục mở đúng, có lọc + tab loại + danh sách thiết bị + nút Thêm tài sản mới. | TC_UC02.1_01.png | Trinh | 10/09/2026 | Pass |  |  | Pass |  |
| Tài sản | TC_UC02.1_02 | Lọc danh sách theo loại Điều hòa | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Tài sản"<br>3. Nhấn tab "Điều hòa" | Tab Điều hòa được chọn (tô đậm). Bảng chỉ còn thiết bị loại điều hòa. Số dòng khớp số ghi trên tab. | Tab Điều hòa active. Danh sách chỉ điều hòa (14 thiết bị), các loại khác biến mất. | TC_UC02.1_02.png | Thu Thảo | 11/09/2026 | Pass |  |  | Pass |  |
| Tài sản | TC_UC02.1_03 | Lọc theo khu vực Phòng học / phòng họp | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Tài sản"<br>3. Chọn khu vực "Phòng học / phòng họp" | Danh sách thu hẹp đúng khu vực vừa chọn. Có thể kết hợp tiếp với tab loại. | Bộ lọc khu vực hoạt động; danh sách chỉ còn thiết bị thuộc phòng học / phòng họp. | TC_UC02.1_01.png | Thu Thảo | 11/09/2026 | Pass |  |  | Pass |  |
| Tài sản | TC_UC02.1_04 | Thêm tài sản mới với tên, loại, vị trí, trạng thái hợp lệ | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Tài sản"<br>3. Nhấn "Thêm tài sản mới"<br>4. Nhập tên thiết bị hợp lệ<br>5. Chọn loại Điều hòa<br>6. Nhập vị trí hợp lệ<br>7. Chọn trạng thái Hoạt động<br>8. Nhấn "Lưu tài sản" | Form đóng hoặc reset. Danh sách xuất hiện «Điều hòa test TC A305», vị trí Phòng A305, trạng thái Hoạt động. | Tài sản mới xuất hiện trên list: Điều hòa test TC A305 · Phòng A305 · Hoạt động. | TC_UC02.1_03.png | Trinh | 12/09/2026 | Failed | 16/09/2026 | Pass | Pass |  |
| Tài sản | TC_UC02.1_05 | Đóng form thêm tài sản thì không lưu bản nháp | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Tài sản"<br>3. Nhấn "Thêm tài sản mới"<br>4. Nhập tên thiết bị bất kỳ<br>5. Nhấn "Đóng" | Form biến mất. Không phát sinh tài sản mới trên danh sách. | Form đóng. Không có tài sản tên «Tài sản hủy test» được lưu. | TC_UC02.1_03_form.png | Thu Thảo | 15/09/2026 | Pass |  |  | Pass |  |
| Tài sản | TC_UC02.1_06 | Mở chi tiết tài sản từ cột Chi tiết / tên thiết bị | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Tài sản"<br>3. Chọn một tài sản trên danh sách<br>4. Nhấn "Chi tiết" | Mở trang chi tiết đúng thiết bị vừa chọn. Facility Manager thấy form sửa tên/loại/vị trí và form đổi trạng thái. | Mở được trang chi tiết: có form Cập nhật thông tin và Đổi trạng thái. | TC_UC02.1_04.png | Trinh | 15/09/2026 | Pass |  |  | Pass |  |
| Tài sản | TC_UC02.1_07 | Cập nhật vị trí tài sản trên trang chi tiết rồi lưu | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Mở chi tiết một tài sản<br>3. Sửa vị trí hợp lệ<br>4. Nhấn "Lưu thông tin" | Hệ thống lưu thành công. Giá trị mới hiện lại khi mở chi tiết / danh sách. | Nút Lưu thông tin hoạt động; thông tin tài sản được giữ sau khi lưu. | TC_UC02.1_04.png | Thu Thảo | 16/09/2026 | Pass |  |  | Pass |  |
| **UC02.2 MODULE Requester không được xem catalog** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Tài sản | TC_UC02.2_01 | Requester không nhìn thấy mục Tài sản trên menu | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Quan sát menu bên trái | Requester không có lối vào catalog tài sản từ giao diện. Chỉ thấy Tổng quan và Yêu cầu sự cố. | Menu requester không có Tài sản. Chỉ Tổng quan + Yêu cầu sự cố. | TC_UC02.2_01.png | Thu Thảo | 17/09/2026 | Pass |  |  | Pass |  |
| Tài sản | TC_UC02.2_02 | Requester tự mở trang danh mục tài sản bằng cách gõ địa chỉ trên thanh trình duyệt | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Trên thanh địa chỉ trình duyệt nhập http://localhost:5173/assets<br>3. Nhấn Enter | Người không có quyền không xem được danh mục tài sản. Hệ thống đưa về trang được phép (Tổng quan) và menu vẫn chỉ 2 mục. | Bị chuyển về Tổng quan. Không thấy bảng danh mục tài sản. | TC_UC02.2_01.png | Thu Thảo | 17/09/2026 | Pass |  |  | Pass |  |

## 3. Yêu cầu sự cố

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | Yêu cầu sự cố |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 6 | 0 | 0 | 6 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC03.1 MODULE Requester tạo và theo dõi yêu cầu** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Yêu cầu sự cố | TC_UC03.1_01 | Requester mở trang Yêu cầu sự cố và thấy form + danh sách của mình | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Chọn menu "Yêu cầu sự cố" | Trang «Yêu cầu sự cố» mở. Có chip thống kê, form chọn vị trí → thiết bị + mô tả, và bảng các yêu cầu đã gửi (cột Tài sản, Nội dung, Trạng thái, Thời gian). | Trang mở đúng. Có form báo cáo và bảng lịch sử (khoảng 21 yêu cầu trên môi trường demo). | TC_UC03.1_01.png | Trinh | 18/09/2026 | Pass |  |  | Pass |  |
| Yêu cầu sự cố | TC_UC03.1_02 | Chọn vị trí trước thì danh sách thiết bị chỉ còn máy ở vị trí đó | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Chọn menu "Yêu cầu sự cố"<br>3. Chọn vị trí<br>4. Mở danh sách thiết bị tại vị trí đó | Ô thiết bị chỉ còn máy thuộc đúng vị trí vừa chọn. Dòng Đã chọn hiện tên máy, loại, vị trí. | Chọn Bãi xe phía Nam → thiết bị «Đèn LED bãi xe»; dòng Đã chọn khớp vị trí. | TC_UC03.1_02_form.png | Trinh | 18/09/2026 | Pass |  |  | Pass |  |
| Yêu cầu sự cố | TC_UC03.1_03 | Gửi yêu cầu sự cố khi đã chọn thiết bị và nhập mô tả | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Chọn menu "Yêu cầu sự cố"<br>3. Chọn vị trí<br>4. Chọn thiết bị<br>5. Nhập mô tả sự cố hợp lệ<br>6. Nhấn "Gửi yêu cầu" | Yêu cầu được lưu. Xuất hiện đầu/gần đầu bảng. Trạng thái Chờ phân công. Ô mô tả được xóa để gửi yêu cầu tiếp. | YC mới xuất hiện trên bảng, trạng thái Chờ phân công. Form mô tả reset về 0/500. | TC_UC03.1_02.png | Trinh | 19/09/2026 | Failed | 22/09/2026 | Pass | Pass |  |
| Yêu cầu sự cố | TC_UC03.1_04 | Không gửi được yêu cầu khi chưa nhập mô tả sự cố | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Chọn menu "Yêu cầu sự cố"<br>3. Chọn vị trí và thiết bị<br>4. Để trống mô tả sự cố<br>5. Nhấn "Gửi yêu cầu" | Không tạo yêu cầu rỗng. Ô mô tả là bắt buộc. Bảng không thêm dòng mới. | Mô tả trống thì không gửi được (trường required). Không phát sinh YC rỗng. | TC_UC03.1_03.png | Trinh | 19/09/2026 | Pass |  |  | Pass |  |
| Yêu cầu sự cố | TC_UC03.1_05 | Lọc danh sách yêu cầu theo chip Chờ phân công | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Chọn menu "Yêu cầu sự cố"<br>3. Nhấn chip "Chờ phân công" | Bảng chỉ còn các yêu cầu đang Chờ phân công. Chip được tô active. | Chip Chờ phân công lọc đúng; các dòng còn lại đều là Chờ phân công. | TC_UC03.1_01.png | Trinh | 22/09/2026 | Pass |  |  | Pass |  |
| **UC03.2 MODULE Facility Manager không dùng trang Yêu cầu sự cố** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Yêu cầu sự cố | TC_UC03.2_01 | Facility Manager không có menu Yêu cầu sự cố — phân công nằm ở Work Order | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Quan sát menu bên trái<br>3. Chọn menu "Work Order" | FM không vào trang Yêu cầu sự cố từ menu. Việc lấy YC chờ và giao kỹ thuật nằm trên Work Order. | Menu FM không có Yêu cầu sự cố. Trang Work Order có form Phân công yêu cầu mới. | TC_UC03.2_01.png | Trinh | 23/09/2026 | Pass |  |  | Pass |  |

## 4. Work Order

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | Work Order |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 8 | 1 | 0 | 9 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC04.1 MODULE Facility Manager phân công** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Work Order | TC_UC04.1_01 | Facility Manager xem hàng đợi phân công và danh sách Work Order | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Work Order" | Trang Work Order mở đủ form phân công và bảng WO. Các cột: Tài sản, Kỹ thuật viên, Trạng thái, Thời gian, Thao tác. | Trang đúng: ~25 WO, hàng đợi chờ phân công, form giao việc cho Hoàng Minh Bảo Trì / kỹ thuật khác. | TC_UC04.1_01.png | Trinh | 24/09/2026 | Pass |  |  | Pass |  |
| Work Order | TC_UC04.1_02 | Phân công một yêu cầu đang chờ cho kỹ thuật viên | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Work Order"<br>3. Chọn một yêu cầu chờ phân công<br>4. Chọn kỹ thuật viên<br>5. Nhấn "Phân công" | Tạo Work Order mới. Yêu cầu biến khỏi hàng đợi. WO hiện Đã phân công, đúng kỹ thuật viên. | Sau khi nhấn Phân công, tổng WO tăng và YC vừa chọn không còn ở đầu hàng đợi. | TC_UC04.1_02.png | Trinh | 24/09/2026 | Failed | 28/09/2026 | Pass | Pass |  |
| Work Order | TC_UC04.1_03 | Đổi kỹ thuật viên trên Work Order đang Đã phân công | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Work Order"<br>3. Chọn một Work Order đang Đã phân công<br>4. Chọn kỹ thuật viên khác<br>5. Nhấn "Đổi KT" | Cột Kỹ thuật viên đổi sang người vừa chọn. Trạng thái vẫn Đã phân công. | Nút Đổi KT và dropdown kỹ thuật viên có trên từng WO Đã phân công. | TC_UC04.1_01.png | Thanh Thảo | 25/09/2026 | Pass |  |  | Pass |  |
| Work Order | TC_UC04.1_04 | Hủy một Work Order đang Đã phân công | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Work Order"<br>3. Chọn một Work Order đang Đã phân công<br>4. Nhấn "Hủy" | WO chuyển «Đã hủy» / «Đã kết thúc». Số Đã hủy tăng. Không còn nút Đổi KT trên dòng đó. | Cột thao tác có nút Hủy trên WO chưa kết thúc. Sau hủy, dòng chuyển trạng thái kết thúc. | TC_UC04.1_01.png | Thanh Thảo | 25/09/2026 | Pass |  |  | Pass |  |
| **UC04.2 MODULE Technician xử lý việc được giao** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Work Order | TC_UC04.2_01 | Technician chỉ thấy Work Order của mình và đúng nút theo trạng thái | 1. Đăng nhập bằng tài khoản Technician (tech1)<br>2. Chọn menu "Work Order" | Tech không thấy form phân công của FM. Thao tác đúng theo từng trạng thái như trên. | Trang kỹ thuật đúng: 9 WO, có Bắt đầu/Từ chối trên Đã phân công, Hoàn thành trên Đang thực hiện. | TC_UC04.2_01.png | Trinh | 26/09/2026 | Pass |  |  | Pass |  |
| Work Order | TC_UC04.2_02 | Technician nhấn Bắt đầu để nhận việc | 1. Đăng nhập bằng tài khoản Technician (tech1)<br>2. Chọn menu "Work Order"<br>3. Chọn một Work Order đang Đã phân công<br>4. Nhấn "Bắt đầu" | Trạng thái đổi thành Đang thực hiện. Nút Bắt đầu biến mất. Hiện ô Kết quả bảo trì và nút Hoàn thành. | WO chuyển Đang thực hiện, xuất hiện ô kết quả + nút Hoàn thành. | TC_UC04.2_02.png | Trinh | 26/09/2026 | Failed | 29/09/2026 | Pass | Pass |  |
| Work Order | TC_UC04.2_03 | Technician hoàn thành việc kèm kết quả bảo trì | 1. Đăng nhập bằng tài khoản Technician (tech1)<br>2. Chọn menu "Work Order"<br>3. Chọn một Work Order đang Đang thực hiện<br>4. Nhập kết quả bảo trì<br>5. Nhấn "Hoàn thành" | WO chuyển Hoàn thành / Đã kết thúc. Không còn nút sửa. Ô thống kê Hoàn thành tăng. | Dòng đang thực hiện có ô kết quả + Hoàn thành; sau khi gửi, WO vào nhóm đã kết thúc. | TC_UC04.2_02.png | Thanh Thảo | 29/09/2026 | Pass |  |  | Pass |  |
| Work Order | TC_UC04.2_04 | Không từ chối được Work Order khi chưa nhập lý do | 1. Đăng nhập bằng tài khoản Technician (tech1)<br>2. Chọn menu "Work Order"<br>3. Chọn một Work Order đang Đã phân công<br>4. Để trống lý do từ chối<br>5. Quan sát nút "Từ chối" | Nút Từ chối tắt khi lý do trống. Chỉ bấm được sau khi đã nhập lý do. | Nút Từ chối disabled khi ô lý do trống; nhập lý do thì bấm được. | TC_UC04.2_01.png | Thanh Thảo | 30/09/2026 | Pass |  |  | Pass |  |
| Work Order | TC_UC04.2_05 | Toàn bộ chữ trên trang Work Order phải là tiếng Việt, không lẫn tiếng Anh | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "Work Order"<br>3. Kiểm tra tiêu đề trang, menu và pill trạng thái | Mọi nhãn người dùng nhìn thấy đều tiếng Việt (ví dụ: Phiếu công việc, Đã phân công, Đổi kỹ thuật viên). | Pill trạng thái đã Việt (Đã phân công, Đang thực hiện) nhưng tiêu đề trang và menu vẫn «Work Order», nút vẫn «Đổi KT». | TC_UC04.2_03.png | Thanh Thảo | 01/10/2026 | Failed | 06/10/2026 | Failed | Failed | Cosmetic |

## 5. IoT Alerts

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | IoT |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 4 | 2 | 0 | 6 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC05.1 MODULE Cảnh báo thiết bị** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| IoT Alerts | TC_UC05.1_01 | Facility Manager xem danh sách cảnh báo vượt ngưỡng | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "IoT Alerts" | Có bảng cảnh báo. Mỗi dòng đủ giá trị đo, ngưỡng, mức độ, thời điểm. Click tên tài sản thì sang trang chi tiết. | Có 4 cảnh báo (quạt/đèn) kèm giá trị, ngưỡng, thời điểm. Link tài sản bấm được. | TC_UC05.1_01.png | Trầm | 02/10/2026 | Pass |  |  | Pass |  |
| IoT Alerts | TC_UC05.1_02 | Cột chỉ số và mức độ phải viết bằng lời người dùng hiểu (tiếng Việt) | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "IoT Alerts"<br>3. Kiểm tra cột Chỉ số và Mức độ | CHỈ SỐ là nhãn tiếng Việt (Trạng thái nguồn, Nhiệt độ…). MỨC ĐỘ là Cao / Trung bình / Thấp. | Vẫn còn raw `power_status`, `temperature`; pill mức độ vẫn `High`. | TC_UC05.1_02.png | Trầm | 02/10/2026 | Failed | 07/10/2026 | Failed | Failed | Cosmetic |
| IoT Alerts | TC_UC05.1_03 | Cảnh báo High đi kèm giá trị vượt hoặc chạm ngưỡng | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "IoT Alerts"<br>3. So sánh cột Giá trị với cột Ngưỡng | Dòng High có căn cứ: giá trị chạm/vượt ngưỡng đã cấu hình. Không có dòng rỗng. | Dòng Đèn LED hành lang: temperature 42.8 so với ngưỡng 40 — đúng là vượt ngưỡng. | TC_UC05.1_02.png | Thịnh | 03/10/2026 | Pass |  |  | Pass |  |
| **UC05.2 MODULE Admin liên kết cảm biến với tài sản** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| IoT Mapping | TC_UC05.2_01 | Trang IoT Mapping chỉ cho chọn tài sản chưa gắn cảm biến | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "IoT"<br>3. Mở danh sách "Tài sản chưa map" | Dropdown không chứa tài sản đã map. Số Chưa map > 0 trên môi trường demo. Không có ô để tự gõ Device ID. | 8 tài sản chưa map; dropdown chỉ máy chưa gắn; không có ô nhập Device ID. | TC_UC05.2_01.png | Trầm | 03/10/2026 | Failed | 05/10/2026 | Pass | Pass |  |
| IoT Mapping | TC_UC05.2_02 | Device ID được hệ thống tự sinh dạng SENSOR_số, không bắt admin nhập tay | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "IoT"<br>3. Kiểm tra form tạo liên kết và cột Device ID | Admin chỉ chọn tài sản rồi nhấn Tạo mapping. Mã cảm biến do hệ thống sinh. | Bảng hiện SENSOR_7, SENSOR_6, SENSOR_5… Form không có ô nhập Device ID. | TC_UC05.2_01.png | Thịnh | 03/10/2026 | Pass |  |  | Pass |  |
| IoT Mapping | TC_UC05.2_03 | Admin gỡ liên kết cảm biến hoặc sửa Device ID ngay trên giao diện | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "IoT"<br>3. Xem cột thao tác trên danh sách mapping | Mỗi dòng mapping có thao tác gỡ liên kết. Có chỗ sửa Device ID (hoặc xóa rồi tạo lại rõ ràng). | Bảng chỉ 3 cột Tài sản / Device ID / Thời gian — không có nút gỡ hay sửa. | TC_UC05.2_02.png | Trầm | 04/10/2026 | Failed | 08/10/2026 | Failed | Failed | Minor |

## 6. AI Risks

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | AI Risks |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 3 | 1 | 0 | 4 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC06.1 MODULE Dashboard dự đoán rủi ro** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| AI Risks | TC_UC06.1_01 | Facility Manager mở dashboard rủi ro bảo trì | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "AI Risks" | Có bảng dự đoán cho tài sản campus. Dòng gợi ý nói rõ AI chỉ hỗ trợ, không tự tạo Work Order. | Dashboard mở, có danh sách dự đoán. Hint ghi AI không tự tạo Work Order. | TC_UC06.1_01.png | Trầm | 04/10/2026 | Pass |  |  | Pass |  |
| AI Risks | TC_UC06.1_02 | Cột tài sản chỉ hiện tên máy, không gắn mã kiểu #12 phía trước | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "AI Risks"<br>3. Kiểm tra cột Tài sản | Tên thiết bị thuần (Quạt trần C102, Điều hòa LG Hội trường…). Vị trí nằm cột riêng. | Tên không prefix #id. Ví dụ: Quạt trần C102, Điều hòa LG Hội trường. | TC_UC06.1_02.png | Thịnh | 04/10/2026 | Pass |  |  | Pass |  |
| AI Risks | TC_UC06.1_03 | Mức rủi ro phải là Cao / Trung bình / Thấp | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Chọn menu "AI Risks"<br>3. Kiểm tra cột Rủi ro | Cột rủi ro tiếng Việt: Cao (đỏ), Trung bình (cam), Thấp (xanh/xám). | Cột vẫn High (đỏ) và Medium (cam). | TC_UC06.1_03.png | Thịnh | 05/10/2026 | Failed | 09/10/2026 | Failed | Failed | Cosmetic |
| AI Risks | TC_UC06.1_04 | Xem dashboard AI không làm phát sinh Work Order mới | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Ghi nhận số Tổng WO ở menu "Work Order"<br>3. Chọn menu "AI Risks"<br>4. Quay lại menu "Work Order" và so số Tổng WO | Số Work Order không tăng chỉ vì mở trang AI. AI không tự tạo phiếu. | Mở AI Risks không tạo WO mới. Hint trang cũng ghi không tự tạo Work Order. | TC_UC06.1_01.png | Trầm | 05/10/2026 | Pass |  |  | Pass |  |

## 7. Quản trị

| | |
|---|---|
| Tên dự án | Smart Maintenance & Facility Management (DUE Smart Campus) |
| Module Code | Quản trị |

| Hoàn thành | Lỗi | Chưa kiểm tra | Tổng số trường hợp thử nghiệm |
|---:|---:|---:|---:|
| 6 | 2 | 0 | 8 |

| Module | ID | Kịch bản thử nghiệm | Quy trình | Kết quả mong đợi | Kết quả thực tế | Minh chứng | Người phụ trách | Ngày kiểm tra | Kết quả lần 1 | Ngày kiểm tra lại | Kết quả lần 2 | Trạng thái | Mức độ lỗi |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **UC07.1 MODULE Người dùng** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Người dùng | TC_UC07.1_01 | Admin xem danh sách người dùng và số liệu theo vai trò | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "Người dùng" | Thấy đủ user demo (manager, tech1, requester, admin…). Có thể lọc theo chip vai trò. Có nút Thêm người dùng. | 7 user / 7 đang hoạt động. Bảng đủ 4 vai trò. Có nút Thêm người dùng. | TC_UC07.1_01.png | Trinh | 06/10/2026 | Pass |  |  | Pass |  |
| Người dùng | TC_UC07.1_02 | Không đổi được vai trò Admin và Người yêu cầu; chỉ đổi được Kỹ thuật viên ↔ Facility Manager | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "Người dùng"<br>3. Kiểm tra cột Vai trò của Admin, Requester, Technician và Facility Manager | Admin và Requester không có combo đổi role. Technician và Facility Manager có dropdown luân chuyển. | admin / requester hiện chữ cố định. tech và manager có select đổi role. | TC_UC07.1_02.png | Trinh | 06/10/2026 | Failed | 08/10/2026 | Pass | Pass |  |
| Người dùng | TC_UC07.1_03 | Mở form Thêm người dùng đủ các ô Username, Mật khẩu, Họ tên, Vai trò | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "Người dùng"<br>3. Nhấn "Thêm người dùng"<br>4. Nhấn "Đóng" | Form hiện ngay trên trang. Có đủ 4 trường và nút tạo/đóng. Ghi chú: Admin chỉ gán khi tạo mới. | Form mở đủ Username / Mật khẩu / Họ tên / Vai trò và nút Tạo tài khoản. | TC_UC07.1_03.png | Thu Thảo | 07/10/2026 | Pass |  |  | Pass |  |
| Người dùng | TC_UC07.1_04 | Lọc bảng user theo chip Kỹ thuật viên | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "Người dùng"<br>3. Nhấn chip "Kỹ thuật viên" | Bảng chỉ còn user vai trò Kỹ thuật viên. Chip đang chọn được tô. | Chip Kỹ thuật viên lọc đúng các tài khoản tech trên bảng. | TC_UC07.1_01.png | Trầm | 07/10/2026 | Pass |  |  | Pass |  |
| **UC07.2 MODULE Phân quyền trang quản trị** |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Phân quyền | TC_UC07.2_01 | Facility Manager không thấy menu Người dùng và IoT kiểu admin | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Quan sát menu bên trái | FM không có lối vào trang quản trị từ menu. | Menu FM: Tổng quan, Tài sản, Work Order, IoT Alerts, AI Risks — không có Người dùng / IoT admin. | TC_UC01.1_04.png | Thanh Thảo | 08/10/2026 | Pass |  |  | Pass |  |
| Phân quyền | TC_UC07.2_02 | Admin mở trang IoT Mapping từ menu | 1. Đăng nhập bằng tài khoản Admin (admin)<br>2. Chọn menu "IoT" | Trang IoT Mapping mở. Có thống kê đã map / chưa map và form tạo liên kết. | Trang IoT Mapping đúng: 71 đã map, 8 chưa map, form + bảng SENSOR_*. | TC_UC05.2_01.png | Thanh Thảo | 08/10/2026 | Pass |  |  | Pass |  |
| Phân quyền | TC_UC07.2_03 | Facility Manager bị chặn khi tự mở trang Người dùng bằng thanh địa chỉ trình duyệt | 1. Đăng nhập bằng tài khoản Facility Manager (manager)<br>2. Trên thanh địa chỉ trình duyệt nhập http://localhost:5173/admin/users<br>3. Nhấn Enter | Người không phải Admin không xem được trang quản trị. Hệ thống đưa về trang được phép (Tổng quan), không để lại giao diện Người dùng. | Vẫn vào được trang «Người dùng», chỉ hiện câu «Chỉ Admin mới được khu vực này.» — chưa redirect. | TC_UC07.2_01.png | Thịnh | 08/10/2026 | Failed | 09/10/2026 | Failed | Failed | Minor |
| Phân quyền | TC_UC07.2_04 | Requester bị chặn nhẹ nhàng khi tự mở trang dự đoán AI bằng thanh địa chỉ | 1. Đăng nhập bằng tài khoản Requester (requester)<br>2. Trên thanh địa chỉ trình duyệt nhập http://localhost:5173/predictions<br>3. Nhấn Enter | Requester không xem dashboard AI. Nên được đưa về trang được phép, hoặc trang trống có hướng dẫn tiếng Việt — không hiện lỗi thô tiếng Anh. | Vẫn vào Dashboard rủi ro bảo trì; hiện chữ đỏ «Forbidden» và «Chưa có prediction.» | TC_UC07.2_02.png | Thịnh | 09/10/2026 | Failed | 09/10/2026 | Failed | Failed | Minor |
