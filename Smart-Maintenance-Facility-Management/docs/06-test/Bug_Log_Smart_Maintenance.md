# Bug Log – Smart Maintenance & Facility Management

**Nguồn:** `Nhom10_Test case.xlsx`  
**Nguyên tắc ghi nhận:** Chỉ ghi nhận các test case từng **Failed** ít nhất một lần.

---

## 1. Tổng quan

| KPI | Giá trị |
|---|---:|
| Tổng bug ghi nhận | 14 |
| Bug đang mở | 6 |
| Bug đã đóng | 8 |
| Tỷ lệ đóng | 57.14% |

### Severity của các bug đang mở

| Severity | Số lượng |
|---|---:|
| Blocker | 0 |
| Major | 0 |
| Minor | 3 |
| Cosmetic | 3 |

### Danh sách bug đang mở

| Bug ID | Test Case | Module | Bug Summary | Severity | Reporter |
|---|---|---|---|---|---|
| BUG-007 | TC_UC04.2_05 | Work Order | Trang Work Order còn lẫn tiếng Anh và viết tắt chưa thuần Việt | Cosmetic | Thanh Thảo |
| BUG-008 | TC_UC05.1_02 | IoT Alerts | IoT Alerts hiển thị raw metric và mức độ bằng tiếng Anh | Cosmetic | Trầm |
| BUG-010 | TC_UC05.2_03 | IoT Mapping | IoT Mapping thiếu thao tác gỡ liên kết hoặc sửa Device ID | Minor | Trầm |
| BUG-011 | TC_UC06.1_03 | AI Risks | AI Risks chưa Việt hóa các mức rủi ro High / Medium | Cosmetic | Thịnh |
| BUG-013 | TC_UC07.2_03 | Phân quyền | Facility Manager vẫn mở được trang Người dùng bằng URL trực tiếp | Minor | Thịnh |
| BUG-014 | TC_UC07.2_04 | Phân quyền | Requester vẫn mở được trang AI Risks bằng URL trực tiếp và thấy lỗi thô | Minor | Thịnh |

---

## 2. Chi tiết Bug Log

### BUG-001 — Lỗi ban đầu khi: Đăng nhập thất bại khi nhập sai mật khẩu

- **Test Case ID:** `TC_UC01.1_02`
- **Module:** Đăng nhập
- **Reporter / Tester:** Trinh
- **Detected Date:** 02/09/2026
- **Initial Result:** Failed
- **Retest Date:** 05/09/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Mở trang Đăng nhập
2. Nhập tên đăng nhập `manager`
3. Nhập mật khẩu sai
4. Nhấn **Đăng nhập**

**Expected Result**

Hệ thống không cho vào app. Hiện thông báo lỗi rõ. Vẫn ở trang Đăng nhập, các ô nhập còn đó để sửa lại.

**Actual Result**

Hiện dòng đỏ `Invalid username or password.`. Không vào được bên trong. Vẫn ở màn Đăng nhập.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 05/09/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-002 — Lỗi ban đầu khi: Đăng xuất từ nút dưới menu thì về lại trang Đăng nhập

- **Test Case ID:** `TC_UC01.2_01`
- **Module:** Đăng xuất
- **Reporter / Tester:** Trinh
- **Detected Date:** 08/09/2026
- **Initial Result:** Failed
- **Retest Date:** 12/09/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Nhấn **Đăng xuất**

**Expected Result**

Hệ thống đưa về trang Đăng nhập. Hai ô nhập trống. Không còn menu bên trái của app.

**Actual Result**

Về đúng form Đăng nhập Smart Campus, các ô trống, có pill demo.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 12/09/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-003 — Lỗi ban đầu khi: Thêm tài sản mới với tên, loại, vị trí, trạng thái hợp lệ

- **Test Case ID:** `TC_UC02.1_04`
- **Module:** Tài sản
- **Reporter / Tester:** Trinh
- **Detected Date:** 12/09/2026
- **Initial Result:** Failed
- **Retest Date:** 16/09/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Chọn menu **Tài sản**
3. Nhấn **Thêm tài sản mới**
4. Nhập tên thiết bị hợp lệ
5. Chọn loại **Điều hòa**
6. Nhập vị trí hợp lệ
7. Chọn trạng thái **Hoạt động**
8. Nhấn **Lưu tài sản**

**Expected Result**

Form đóng hoặc reset. Danh sách xuất hiện `Điều hòa test TC A305`, vị trí `Phòng A305`, trạng thái `Hoạt động`.

**Actual Result**

Tài sản mới xuất hiện trên list: `Điều hòa test TC A305 · Phòng A305 · Hoạt động`.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 16/09/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-004 — Lỗi ban đầu khi: Gửi yêu cầu sự cố khi đã chọn thiết bị và nhập mô tả

- **Test Case ID:** `TC_UC03.1_03`
- **Module:** Yêu cầu sự cố
- **Reporter / Tester:** Trinh
- **Detected Date:** 19/09/2026
- **Initial Result:** Failed
- **Retest Date:** 22/09/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Requester (`requester`)
2. Chọn menu **Yêu cầu sự cố**
3. Chọn vị trí
4. Chọn thiết bị
5. Nhập mô tả sự cố hợp lệ
6. Nhấn **Gửi yêu cầu**

**Expected Result**

Yêu cầu được lưu. Xuất hiện đầu/gần đầu bảng. Trạng thái `Chờ phân công`. Ô mô tả được xóa để gửi yêu cầu tiếp.

**Actual Result**

Yêu cầu mới xuất hiện trên bảng, trạng thái `Chờ phân công`. Form mô tả reset về `0/500`.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 22/09/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-005 — Lỗi ban đầu khi: Phân công một yêu cầu đang chờ cho kỹ thuật viên

- **Test Case ID:** `TC_UC04.1_02`
- **Module:** Work Order
- **Reporter / Tester:** Trinh
- **Detected Date:** 24/09/2026
- **Initial Result:** Failed
- **Retest Date:** 28/09/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Chọn menu **Work Order**
3. Chọn một yêu cầu chờ phân công
4. Chọn kỹ thuật viên
5. Nhấn **Phân công**

**Expected Result**

Tạo Work Order mới. Yêu cầu biến khỏi hàng đợi. WO hiện `Đã phân công`, đúng kỹ thuật viên.

**Actual Result**

Sau khi nhấn Phân công, tổng WO tăng và yêu cầu vừa chọn không còn ở đầu hàng đợi.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 28/09/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-006 — Lỗi ban đầu khi: Technician nhấn Bắt đầu để nhận việc

- **Test Case ID:** `TC_UC04.2_02`
- **Module:** Work Order
- **Reporter / Tester:** Trinh
- **Detected Date:** 26/09/2026
- **Initial Result:** Failed
- **Retest Date:** 29/09/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Technician (`tech1`)
2. Chọn menu **Work Order**
3. Chọn một Work Order đang `Đã phân công`
4. Nhấn **Bắt đầu**

**Expected Result**

Trạng thái đổi thành `Đang thực hiện`. Nút Bắt đầu biến mất. Hiện ô Kết quả bảo trì và nút Hoàn thành.

**Actual Result**

WO chuyển `Đang thực hiện`, xuất hiện ô kết quả và nút Hoàn thành.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 29/09/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-007 — Trang Work Order còn lẫn tiếng Anh và viết tắt chưa thuần Việt

- **Test Case ID:** `TC_UC04.2_05`
- **Module:** Work Order
- **Reporter / Tester:** Thanh Thảo
- **Detected Date:** 01/10/2026
- **Initial Result:** Failed
- **Retest Date:** 06/10/2026
- **Retest Result:** Failed
- **Status:** **Open**
- **Severity:** Cosmetic
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Chọn menu **Work Order**
3. Kiểm tra tiêu đề trang, menu và pill trạng thái

**Expected Result**

Mọi nhãn người dùng nhìn thấy đều tiếng Việt, ví dụ: `Phiếu công việc`, `Đã phân công`, `Đổi kỹ thuật viên`.

**Actual Result**

Pill trạng thái đã Việt (`Đã phân công`, `Đang thực hiện`) nhưng tiêu đề trang và menu vẫn `Work Order`, nút vẫn `Đổi KT`.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Retest vẫn Failed; lỗi còn mở tại thời điểm 09/10/2026.

---

### BUG-008 — IoT Alerts hiển thị raw metric và mức độ bằng tiếng Anh

- **Test Case ID:** `TC_UC05.1_02`
- **Module:** IoT Alerts
- **Reporter / Tester:** Trầm
- **Detected Date:** 02/10/2026
- **Initial Result:** Failed
- **Retest Date:** 07/10/2026
- **Retest Result:** Failed
- **Status:** **Open**
- **Severity:** Cosmetic
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Chọn menu **IoT Alerts**
3. Kiểm tra cột **Chỉ số** và **Mức độ**

**Expected Result**

CHỈ SỐ là nhãn tiếng Việt (`Trạng thái nguồn`, `Nhiệt độ`…). MỨC ĐỘ là `Cao / Trung bình / Thấp`.

**Actual Result**

Vẫn còn raw `power_status`, `temperature`; pill mức độ vẫn `High`.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Retest vẫn Failed; lỗi còn mở tại thời điểm 09/10/2026.

---

### BUG-009 — Lỗi ban đầu khi: Trang IoT Mapping chỉ cho chọn tài sản chưa gắn cảm biến

- **Test Case ID:** `TC_UC05.2_01`
- **Module:** IoT Mapping
- **Reporter / Tester:** Trầm
- **Detected Date:** 03/10/2026
- **Initial Result:** Failed
- **Retest Date:** 05/10/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Admin (`admin`)
2. Chọn menu **IoT**
3. Mở danh sách **Tài sản chưa map**

**Expected Result**

Dropdown không chứa tài sản đã map. Số Chưa map > 0 trên môi trường demo. Không có ô để tự gõ Device ID.

**Actual Result**

8 tài sản chưa map; dropdown chỉ máy chưa gắn; không có ô nhập Device ID.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 05/10/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-010 — IoT Mapping thiếu thao tác gỡ liên kết hoặc sửa Device ID

- **Test Case ID:** `TC_UC05.2_03`
- **Module:** IoT Mapping
- **Reporter / Tester:** Trầm
- **Detected Date:** 04/10/2026
- **Initial Result:** Failed
- **Retest Date:** 08/10/2026
- **Retest Result:** Failed
- **Status:** **Open**
- **Severity:** Minor
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Admin (`admin`)
2. Chọn menu **IoT**
3. Xem cột thao tác trên danh sách mapping

**Expected Result**

Mỗi dòng mapping có thao tác gỡ liên kết. Có chỗ sửa Device ID hoặc xóa rồi tạo lại rõ ràng.

**Actual Result**

Bảng chỉ có 3 cột `Tài sản / Device ID / Thời gian` — không có nút gỡ hay sửa.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Retest vẫn Failed; lỗi còn mở tại thời điểm 09/10/2026.

---

### BUG-011 — AI Risks chưa Việt hóa các mức rủi ro High / Medium

- **Test Case ID:** `TC_UC06.1_03`
- **Module:** AI Risks
- **Reporter / Tester:** Thịnh
- **Detected Date:** 05/10/2026
- **Initial Result:** Failed
- **Retest Date:** 09/10/2026
- **Retest Result:** Failed
- **Status:** **Open**
- **Severity:** Cosmetic
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Chọn menu **AI Risks**
3. Kiểm tra cột **Rủi ro**

**Expected Result**

Cột rủi ro tiếng Việt: `Cao` (đỏ), `Trung bình` (cam), `Thấp` (xanh/xám).

**Actual Result**

Cột vẫn `High` (đỏ) và `Medium` (cam).

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Retest vẫn Failed; lỗi còn mở tại thời điểm 09/10/2026.

---

### BUG-012 — Lỗi ban đầu khi: Không đổi được vai trò Admin và Người yêu cầu; chỉ đổi được Kỹ thuật viên ↔ Facility Manager

- **Test Case ID:** `TC_UC07.1_02`
- **Module:** Người dùng
- **Reporter / Tester:** Trinh
- **Detected Date:** 06/10/2026
- **Initial Result:** Failed
- **Retest Date:** 08/10/2026
- **Retest Result:** Pass
- **Status:** **Closed**
- **Severity:** Chưa phân loại
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Admin (`admin`)
2. Chọn menu **Người dùng**
3. Kiểm tra cột Vai trò của Admin, Requester, Technician và Facility Manager

**Expected Result**

Admin và Requester không có combo đổi role. Technician và Facility Manager có dropdown luân chuyển.

**Actual Result**

`admin` / `requester` hiện chữ cố định. `tech` và `manager` có select đổi role.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Đã retest ngày 08/10/2026 và kết quả Pass. File test case không lưu riêng mô tả trạng thái lỗi ban đầu sau khi đã sửa.

---

### BUG-013 — Facility Manager vẫn mở được trang Người dùng bằng URL trực tiếp

- **Test Case ID:** `TC_UC07.2_03`
- **Module:** Phân quyền
- **Reporter / Tester:** Thịnh
- **Detected Date:** 08/10/2026
- **Initial Result:** Failed
- **Retest Date:** 09/10/2026
- **Retest Result:** Failed
- **Status:** **Open**
- **Severity:** Minor
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Facility Manager (`manager`)
2. Trên thanh địa chỉ trình duyệt nhập `http://localhost:5173/admin/users`
3. Nhấn Enter

**Expected Result**

Người không phải Admin không xem được trang quản trị. Hệ thống đưa về trang được phép (Tổng quan), không để lại giao diện Người dùng.

**Actual Result**

Vẫn vào được trang `Người dùng`, chỉ hiện câu `Chỉ Admin mới được khu vực này.` — chưa redirect.

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Retest vẫn Failed; lỗi còn mở tại thời điểm 09/10/2026.

---

### BUG-014 — Requester vẫn mở được trang AI Risks bằng URL trực tiếp và thấy lỗi thô

- **Test Case ID:** `TC_UC07.2_04`
- **Module:** Phân quyền
- **Reporter / Tester:** Thịnh
- **Detected Date:** 09/10/2026
- **Initial Result:** Failed
- **Retest Date:** 09/10/2026
- **Retest Result:** Failed
- **Status:** **Open**
- **Severity:** Minor
- **Fix Owner:** Chưa gán

**Steps to Reproduce**

1. Đăng nhập bằng tài khoản Requester (`requester`)
2. Trên thanh địa chỉ trình duyệt nhập `http://localhost:5173/predictions`
3. Nhấn Enter

**Expected Result**

Requester không xem dashboard AI. Nên được đưa về trang được phép, hoặc trang trống có hướng dẫn tiếng Việt — không hiện lỗi thô tiếng Anh.

**Actual Result**

Vẫn vào Dashboard rủi ro bảo trì; hiện chữ đỏ `Forbidden` và `Chưa có prediction.`

**Evidence**

Chưa có evidence file được ghi trong workbook nguồn.

**Resolution / Regression Notes**

Retest vẫn Failed; lỗi còn mở tại thời điểm 09/10/2026.

---

## 3. Ghi chú quản lý Bug

- Bug chỉ được chuyển sang **Closed** sau khi đã được retest và kết quả là **Pass**.
- Các bug đang **Open** cần tiếp tục được theo dõi, gán Fix Owner và cập nhật evidence nếu có.
- Với các bug ảnh hưởng nghiệp vụ, nên liên kết thêm **Requirement ID / Business Rule ID / User Story ID** trong Traceability Matrix.
- Các lỗi mức **Cosmetic** chủ yếu ảnh hưởng tính nhất quán ngôn ngữ/giao diện; các lỗi mức **Minor** cần được đánh giá thêm về business impact trước khi final release.
