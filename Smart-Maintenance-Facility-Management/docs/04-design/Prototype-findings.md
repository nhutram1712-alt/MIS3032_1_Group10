Prototype.brief
FLOW 1 — Requester: Tạo và theo dõi Maintenance Request
UC: UC-05 (Tạo Maintenance Request), UC-06 (Theo dõi Maintenance Request)
US: US-03-01, US-03-02
REQ: REQ-04, REQ-05, REQ-02, REQ-03
BR: BR-05 (Request phải xác định Asset hoặc khu vực), BR-14 (6 trạng thái lifecycle)
AC tham chiếu: AC-US-03-01-01→04, AC-US-03-02-01→02
Persona
Requester (sinh viên/giảng viên/nhân viên) — Persona 01. "Báo lỗi nhanh – dễ theo dõi – biết được sự cố đang được xử lý đến đâu."
Mục tiêu
Requester phát hiện điều hòa phòng học hỏng, báo lỗi trong dưới 1 phút, và biết chắc chắn yêu cầu đã được ghi nhận + theo dõi được tiến độ.
Screens bắt buộc
UC-03 — Danh sách Asset theo phòng/khu vực (REQ-02, REQ-03, BR-04)
UC-05 — Form tạo Maintenance Request (mô tả vấn đề + Asset/khu vực + ảnh không bắt buộc)
UC-05 — Xác nhận đã gửi (trạng thái khởi tạo = Submitted)
UC-06 — Danh sách Request của tôi (hiển thị đủ 6 trạng thái)
UC-06 — Chi tiết 1 Request
States bắt buộc
loading, empty (chưa có Asset được gán / chưa có Request nào — UC-06 A1)
error — theo AC-US-03-01-03: không xác định Asset hoặc khu vực → hệ thống KHÔNG cho gửi Request
confirmation, success
Business Rule quan trọng cần thể hiện đúng
BR-05: Request phải xác định Asset hoặc khu vực bị ảnh hưởng — nếu chỉ chọn khu vực (chưa chọn Asset cụ thể), vẫn được phép tạo Request (AC-US-03-01-02), nhưng phải xác định Asset trước khi tạo Work Order (ràng buộc này thuộc Flow 2, UC-08 A2).
BR-14 / AC-US-03-02-02: Đúng 6 trạng thái chính thức: Submitted → Pending → In Progress → Resolved → Closed, hoặc nhánh Submitted → Rejected.
Sample Data
Asset: AC-A203-01 — Điều hòa, Phòng A203, Air Conditioner
Request mẫu 1: REQ-1042 "Điều hòa không lạnh, có tiếng ồn lớn" — status: Pending
Request mẫu 2: REQ-1040 "Wi-Fi rớt liên tục khi đông người" — status: Resolved
Request mẫu 3: REQ-1039 "Hình ảnh máy chiếu mờ, chập chờn" — status: In Progress
Request mẫu 4: REQ-1021 "Rò rỉ nước ở dàn lạnh" — status: Closed
[ASSUMPTION] cần kiểm chứng qua test
Ảnh/video: hiển thị "Thêm ảnh (không bắt buộc)" — REQ-04 xác nhận không bắt buộc, nhưng liên quan Q-02 (đã Open) cần xem người dùng có kỳ vọng bắt buộc không.
Requester chỉ thấy Asset thuộc phòng/khu vực được gán (Q-01 vẫn Open — chưa xác nhận là "theo phòng phụ trách" hay "toàn bộ").
FLOW 2 — Facility Manager: Tiếp nhận Request → Tạo & phân công Work Order
UC: UC-07 (Tiếp nhận và xử lý Maintenance Request), UC-08 (Tạo và phân công Work Order)
US: US-03-03, US-03-04, US-04-01, US-04-02
REQ: REQ-13, REQ-14, REQ-15
BR: BR-05, BR-06 (1 Request = tối đa 1 WO), BR-07, BR-08, BR-18 (Request chỉ Closed sau khi FM xác nhận)
AC tham chiếu: AC-US-03-03-01→03, AC-US-03-04-01→03, AC-US-04-01-01→03, AC-US-04-02-01→02
Persona
Facility Manager — Persona 03. "Có đầy đủ dữ liệu để biết tài sản nào đang có vấn đề... và cần hành động gì tiếp theo."
Mục tiêu
FM nhận một Request mới, xem xét, tạo Work Order, phân công đúng Technician — và sau khi Work Order hoàn thành, xác nhận kết quả để đóng Request đúng theo BR-18.
Screens bắt buộc
UC-07 — Danh sách Maintenance Request chờ xử lý
UC-07 — Chi tiết Request + nút "Tạo Work Order"
UC-08 — Form tạo Work Order (chọn Technician)
UC-08 — Xác nhận đã tạo & phân công (trạng thái WO = Assigned)
UC-10 — Danh sách Work Order + trạng thái (Assigned/In Progress/Completed/Cancelled theo BR-17)
UC-07 (bước 9-11) / US-03-04 — Màn hình xác nhận kết quả & đóng Request (chỉ xuất hiện khi WO liên quan đã Completed → Request tự chuyển Resolved → FM xác nhận → Closed)
States bắt buộc
empty (không còn request chờ xử lý)
error — theo BR-06/AC-US-04-01-02: Request đã có Work Order rồi → từ chối tạo thêm
error — theo AC-US-04-01-03: chưa xác định Asset (chỉ có khu vực) → không cho tạo Work Order cho đến khi Asset được xác định
error — thiếu Technician khi phân công (AC-US-04-02-02: Technician không hợp lệ)
confirmation, success
[ASSUMPTION] cần kiểm chứng qua test
Prototype gộp "Tạo Work Order" và "Phân công Technician" thành 1 form duy nhất để thao tác nhanh — thực tế UC-08 mô tả 2 bước tách biệt (bước 5 tạo WO, bước 6-7 mới chọn & phân công Technician). Cần test xem FM có cảm thấy thiếu bước "xem lại trước khi phân công" không.
Không có bước "approve" riêng trước khi tạo Work Order (liên quan Q-05 — vẫn Open) — hành động "Tạo Work Order" gộp luôn vai trò duyệt.
FLOW 3 — Technician: Thực hiện Work Order
UC: UC-09 (Thực hiện Work Order), UC-10 (Theo dõi và hoàn tất Work Order — phần Technician)
US: US-04-04, US-04-05, US-04-06
REQ: REQ-17, REQ-18, REQ-19, REQ-20, REQ-21, REQ-22
BR: BR-07 (chỉ cập nhật WO của mình), BR-08, BR-09, BR-10, BR-11, BR-17
AC tham chiếu: AC-US-04-04-01→03, AC-US-04-05-01→04, AC-US-04-06-01→04
Persona
Technician — Persona 02. "Nhận đúng Work Order – có đủ thông tin thiết bị – xử lý hiệu quả – ghi nhận đầy đủ kết quả."
Mục tiêu
Technician mở Work Order được giao, xem đủ thông tin Asset + IoT Alert/AI Prediction liên quan (UC-09 bước 3-4), quyết định chấp nhận hoặc từ chối, cập nhật tiến độ và hoàn thành kèm ghi nhận kết quả.
Screens bắt buộc
UC-09 — Danh sách Work Order được phân công
UC-09 — Chi tiết Work Order (Asset info + IoT Alert + AI Prediction nếu có — theo UC-15 bước 5: Technician chỉ xem thông tin liên quan Asset trong WO được giao)
UC-09 (A1) — Từ chối kèm lý do bắt buộc
UC-09 — Form ghi nhận kết quả xử lý
UC-09/UC-10 — Xác nhận hoàn thành (WO chuyển Completed, lưu vào Maintenance History)
States bắt buộc
empty, loading
2 biến thể ở màn hình chi tiết: có IoT Alert/AI Prediction và không có (UC-15 A1: "Không có Prediction → hệ thống thông báo chưa có AI Prediction")
error — theo AC-US-04-05-03: từ chối mà không nhập lý do → không cho hoàn tất thao tác
error — theo AC-US-04-06-03: chưa ghi nhận kết quả → không cho hoàn thành Work Order
permission — theo AC-US-04-04-03: Technician không được xem/cập nhật WO không phải của mình
confirmation, success
[ASSUMPTION] khác cần kiểm chứng
Sau khi Technician "Hoàn thành" (Completed), Request liên quan tự chuyển Resolved — Technician chưa thấy được đây có phải bước cuối hay còn chờ FM xác nhận (Q-08 vẫn Open) — cần quan sát Technician có hiểu rõ điều này không.
FLOW 4 — Facility Manager: Giám sát IoT & AI Prediction / Maintenance Risk
UC: UC-12 (Giám sát IoT Data), UC-13 (Xử lý IoT Alert), UC-14 (Dự đoán bằng AI), UC-15 (Xem Maintenance Risk và AI Prediction)
US: US-05-03, US-05-04, US-06-01, US-06-02
REQ: REQ-09, REQ-10, REQ-11, REQ-12, REQ-28, REQ-29, REQ-30
BR: BR-10 (AI chỉ hỗ trợ quyết định), BR-11 (AI dùng IoT Data + Maintenance History), BR-12 (Alert theo threshold), BR-13 (Asset phải mapping IoT trước), BR-15 (FM ưu tiên Asset Risk = High)
AC tham chiếu: AC-US-05-03-01→04, AC-US-05-04-01→03, AC-US-06-01-01→04, AC-US-06-02-01→03
Persona
Facility Manager — đây là flow thể hiện rõ nhất giá trị AI/IoT của sản phẩm, và có nhiều Open Question nhất (5 câu).
Mục tiêu
FM mở dashboard, nhanh chóng nhận diện Asset nào có Maintenance Risk = High, hiểu lý do dựa trên dữ liệu gì, và có hành động tiếp theo rõ ràng (tạo Work Order chủ động — nối sang Flow 2).
Screens bắt buộc
UC-15 — Dashboard tổng quan: danh sách Asset kèm Maintenance Risk (Low/Medium/High)
UC-12 — Chi tiết IoT Data của 1 Asset
UC-14/UC-15 — Chi tiết AI Prediction (risk level + khuyến nghị)
UC-08 — Hành động: Tạo Work Order chủ động từ Prediction (nối sang Flow 2)
States bắt buộc
loading, empty (chưa có Asset nào có Alert/Risk)
error — theo UC-12 A2: không nhận được dữ liệu IoT mới → hiển thị "không có dữ liệu mới", KHÔNG hiển thị dữ liệu cũ như dữ liệu hiện tại
empty — theo UC-14 A1: không đủ dữ liệu → hệ thống không tạo Prediction đáng tin cậy, thông báo rõ tình trạng thiếu dữ liệu (không được tự bịa kết quả)
confirmation (dẫn sang Flow 2)
Business Rule quan trọng cần thể hiện đúng
BR-10 / AC-US-06-01-04: "AI chỉ cung cấp Prediction và không tự động quyết định hoặc thực hiện bảo trì" — prototype phải luôn có bước FM tự bấm "Tạo Work Order chủ động", AI không tự tạo.
UC-15 bước 5: Nếu actor là Technician (không phải FM) xem màn hình này, hệ thống chỉ hiển thị Asset liên quan đến Work Order được giao cho Technician đó — không hiển thị toàn bộ danh sách như FM.
[ASSUMPTION] cần kiểm chứng — đây là flow có nhiều Open Question nhất
Risk hiển thị dạng nhãn màu Low/Medium/High (REQ-12), KHÔNG hiển thị % cụ thể — Q-13 vẫn Open, cần test xem FM có muốn thấy con số % không.
Prediction horizon cố định "7 ngày" (REQ-11 xác nhận High Confidence) dù Q-12 vẫn hỏi liệu có thể là 30/90 ngày.
Ngưỡng bất thường cụ thể để tạo IoT Alert (Q-09, Q-10 vẫn Open) — prototype dùng số liệu giả định minh họa, không phải threshold thật.
Screen Flow
Flow 1 — UC-05, UC-06 — Requester: Tạo & theo dõi Maintenance Request
Flow 2 — UC-07, UC-08 — Facility Manager: Xử lý Request → Work Order → Đóng Request
Flow 3 — UC-09, UC-10 — Technician: Thực hiện Work Order
Flow 4 — UC-12, UC-13, UC-14, UC-15 — Facility Manager: IoT & AI Prediction
Usability Test Script
Chuẩn bị trước buổi test
Prototype đã chạy được (mở prototype.html bằng trình duyệt) cho cả 4 flow.
Đã tự kiểm tra đủ state theo checklist ở huong-dan-buoc-3-den-7.md Bước 4.
Chuẩn bị công cụ ghi lại: quay màn hình, hoặc ngồi cạnh ghi chú tay theo mẫu bên dưới.
Chọn 3 người test, cố gắng match với 1 trong các persona: Requester (dễ nhất, ai cũng đóng được vai sinh viên/GV), Facility Manager, Technician.
Câu mở đầu (đọc cho người test, giống nhau cho cả 3 người)
"Mình đang thử nghiệm một prototype cho hệ thống quản lý bảo trì cơ sở vật chất của trường, chưa phải sản phẩm hoàn chỉnh. Mình sẽ đưa cho bạn một tình huống, bạn cứ thao tác tự nhiên như đang dùng thật, kể cả nếu bạn không chắc phải bấm gì. Mình sẽ không hướng dẫn trong lúc bạn làm, chỉ quan sát thôi. Nếu bạn nghĩ thành tiếng trong lúc làm thì càng tốt."
Task 1 — Flow 1 (UC-05, UC-06): Requester tạo & theo dõi Request
Task đọc cho người test:
"Điều hòa ở phòng bạn đang học bị hỏng, không mát. Hãy báo lỗi, sau đó kiểm tra xem yêu cầu của bạn đã được gửi và đang ở trạng thái nào."
Quan sát cần ghi:
Có tìm đúng Asset "điều hòa" không, hay bị nhầm/lạc?
Có bị vướng ở phần "ảnh không bắt buộc" không (dừng lại, do dự)? → liên quan Q-02
Sau khi gửi, có tự tìm được màn hình xem trạng thái không, hay phải mò?
Có hiểu đúng ý nghĩa các trạng thái (Submitted/Pending/In Progress/Resolved/Closed) không?
Thời gian hoàn thành: _____ giây/phút.
Có hoàn thành task không: ☐ Có ☐ Không ☐ Hoàn thành một phần
Task 2 — Flow 2 (UC-07, UC-08): Facility Manager xử lý Request → Work Order
Task đọc cho người test:
"Có một yêu cầu báo lỗi mới vừa gửi tới. Hãy xử lý yêu cầu đó và phân công cho một kỹ thuật viên."
Quan sát cần ghi:
Có đọc kỹ nội dung Request trước khi tạo WO không, hay bấm ngay?
Có hiểu được "Tạo Work Order" cũng là hành động xử lý/duyệt Request không (không có bước approve riêng)? → liên quan Q-05
Có chọn đúng Technician không, có bị rối vì thiếu thông tin (chuyên môn, đang bận...) không?
Thời gian hoàn thành: _____
Có hoàn thành task không: ☐ Có ☐ Không ☐ Một phần
Task 2b (nối tiếp, sau khi Task 3 hoàn tất — có thể làm cùng người hoặc người khác):
"Kỹ thuật viên vừa báo đã hoàn thành công việc. Hãy kiểm tra và đóng yêu cầu này lại."
Có tìm đúng nút "Xác nhận kết quả & Đóng yêu cầu" không?
Có hiểu vì sao phải có bước xác nhận riêng (không tự động đóng) không? → liên quan BR-18, Q-08
Task 3 — Flow 3 (UC-09): Technician xử lý Work Order
Task đọc cho người test:
"Bạn vừa được giao một công việc sửa chữa. Hãy xem thông tin, xác nhận nhận việc, và sau khi sửa xong thì ghi nhận kết quả."
Quan sát cần ghi:
Có xem phần thông tin Asset + IoT Alert/AI Prediction trước khi quyết định chấp nhận không?
Có hiểu rõ khác biệt giữa "chấp nhận" và "từ chối" không?
Quan trọng nhất (liên quan OQ-01 chưa resolve): Sau khi ghi nhận kết quả xong, có hiểu công việc đã kết thúc hay còn chờ FM xác nhận không?
Thời gian hoàn thành: _____
Có hoàn thành task không: ☐ Có ☐ Không ☐ Một phần
Task 3b — Nhánh từ chối (test riêng, đưa Work Order khác):
"Bạn nhận thấy công việc này không đúng chuyên môn của mình. Hãy từ chối và cho biết lý do."
Sau khi từ chối, người test có hiểu Work Order vẫn hiển thị trạng thái Assigned (không đổi thành "Rejected" hay "Cancelled") không?
Họ có kỳ vọng điều gì sẽ xảy ra tiếp theo? (Ghi nguyên văn câu trả lời — đây là dữ liệu quan trọng nhất để giúp đề xuất hướng resolve OQ-01)
Task 4 — Flow 4 (UC-12→15): Facility Manager xem AI Prediction
Task đọc cho người test:
"Hãy tìm xem trong danh sách thiết bị, cái nào đang có nguy cơ hỏng cao nhất, và cho biết vì sao bạn nghĩ vậy."
Quan sát cần ghi:
Có nhận ra ngay Asset có Risk = High không (màu sắc/vị trí có đủ nổi bật)?
Có đọc và hiểu được lý do/khuyến nghị AI đưa ra không, hay chỉ nhìn nhãn Risk mà bỏ qua chi tiết?
Có mong muốn thấy con số % cụ thể thay vì chỉ Low/Medium/High không? → liên quan Q-13
Có tự tìm được nút "Tạo Work Order chủ động" không?
Thời gian hoàn thành: _____
Có hoàn thành task không: ☐ Có ☐ Không ☐ Một phần
Câu hỏi sau khi làm xong cả 4 task (không hỏi cảm nhận chủ quan, chỉ hỏi để làm rõ hành vi)
"Lúc nãy ở bước [X], bạn dừng lại khá lâu — bạn đang phân vân điều gì?"
"Có bước nào bạn làm mà không chắc chắn là đúng không?"
"Ở Task 3b, sau khi bạn từ chối công việc, bạn nghĩ điều gì sẽ xảy ra tiếp theo với công việc đó?"
"Nếu phải làm lại task [Y], bạn nghĩ có bấm giống vậy không?"
Usability Finding
Người test (mô phỏng)
Bảng Observation → Issue → Decision
Tỷ lệ hoàn thành task (Task completion rate)
(X = hoàn thành, 0 = không hoàn thành, /= hoàn thành một phần — mỗi người chỉ được giao 1-2 flow phù hợp vai trò đóng, theo đúng persona)
Top vấn đề cần xử lý ngay (ưu tiên theo mức độ nghiêm trọng)
[Nghiêm trọng nhất] OQ-01 — Technician hiểu nhầm trạng thái Work Order sau khi từ chối (chi tiết bên dưới).
Q-13 — Người dùng (vai Facility Manager) mong muốn thấy thêm thông tin định lượng (%) bên cạnh nhãn Risk, không chỉ Low/Medium/High.
Không phát hiện vấn đề nghiêm trọng nào ở Flow 1 và Flow 2 — cả 2 flow được hoàn thành mượt, không cần thay đổi lớn.
⚠️ Kết quả quan trọng nhất: OQ-01 — Work Order Rejection Status
Task 3b: Yêu cầu người test (đóng vai Technician) từ chối một Work Order và cho biết lý do, sau đó hỏi họ nghĩ điều gì sẽ xảy ra tiếp theo.
Người test (Thảo) kỳ vọng gì sau khi từ chối WO:
Sau khi bấm "Xác nhận từ chối" và thấy màn hình xác nhận ghi "Work Order vẫn giữ trạng thái Assigned", Tuấn nói: "Ủa sao vẫn Assigned? Tôi tưởng từ chối xong là xong việc của tôi rồi, không liên quan tới tôi nữa chứ?" — Tuấn quay lại danh sách Work Order và thấy công việc đó vẫn nằm trong danh sách được giao cho mình, tỏ ra bối rối, hỏi thêm: "Vậy giờ tôi có phải làm gì nữa không, hay Facility Manager sẽ tự thấy?"
Ghi chú thêm (do chỉ có 1 Technician trong nhóm test mô phỏng, không có Người test 2/3 cho task này):
Đây là hạn chế của bộ test mô phỏng — Task 3b lẽ ra nên được thực hiện bởi ít nhất 2-3 người đóng vai Technician để có dữ liệu đối chiếu. Khi triển khai thật, cần bổ sung thêm người test cho riêng task này.
Đề xuất hướng resolve OQ-01 (dựa trên quan sát ở trên):
☐ Phương án 1: Reject chỉ là action, WO giữ nguyên Assigned
☒ Phương án 2 (đề xuất): Bổ sung Rejected thành Work Order Status chính thức
Lý do đề xuất Phương án 2: Quan sát cho thấy giữ nguyên trạng thái Assigned sau khi từ chối gây hiểu nhầm — người dùng nghĩ trách nhiệm đã chuyển đi trong khi hệ thống vẫn coi công việc thuộc về họ. Việc này có rủi ro thực tế: Work Order có thể "bị treo" nếu Facility Manager không chủ động kiểm tra lại. Thêm trạng thái Rejected rõ ràng sẽ giúp cả Technician lẫn Facility Manager cùng nhìn thấy đúng tình trạng.
Lưu ý quan trọng: Đây là đề xuất dựa trên dữ liệu mô phỏng với cỡ mẫu rất nhỏ (1 người, 1 lần test) — cần được stakeholder/giảng viên xác nhận chính thức trước khi cập nhật BR-17, không tự động áp dụng.
Open Questions khác được resolve nhờ test này
Hạn chế của đợt test mô phỏng này (cần nêu rõ khi báo cáo)
Cỡ mẫu rất nhỏ: chỉ 1 người/vai trò, mỗi flow chỉ được test 1 lần — không đủ để kết luận thống kê, chỉ đủ để phát hiện tín hiệu ban đầu (early signal).
Task 3b (quan trọng nhất — OQ-01) chỉ có 1 lượt quan sát, nên đề xuất Phương án 2 cần được xác nhận thêm trước khi áp dụng chính thức.
Đây là dữ liệu mô phỏng, không phải người dùng thật của Trường Đại học Kinh tế – Đại học Đà Nẵng — khi triển khai thật, cần lặp lại quy trình này với đúng đối tượng (giảng viên/sinh viên/nhân viên kỹ thuật thật).
Decision Log
DEC-01 — Đề xuất hướng resolve OQ-01: bổ sung trạng thái Rejected cho Work Order
Ngày: 03/09/2026
Người tham gia: Project Owner (mô phỏng), dựa trên usability test mô phỏng với 1 người đóng vai Technician
Yêu cầu/Use Case liên quan: UC-09, UC-10
User Story liên quan: US-04-05
Business Rule liên quan: BR-17 (vòng đời Work Order)
Open Question liên quan: OQ-01 (ghi trong use-cases.md, user-stories.md, acceptance-criteria.md)
Vấn đề: Khi Technician từ chối Work Order, prototype hiện giữ nguyên status Assigned (theo đúng tinh thần chưa tự quyết định thay OQ-01). Usability test mô phỏng cho thấy người dùng hiểu nhầm: nghĩ rằng "từ chối" đồng nghĩa với "đã xong trách nhiệm", trong khi hệ thống vẫn coi Work Order thuộc về họ — có rủi ro Work Order bị "treo" nếu Facility Manager không chủ động kiểm tra lại.
Quyết định (đề xuất): Bổ sung Rejected thành một Work Order Status chính thức trong BR-17, với vòng đời cập nhật đề xuất: Assigned → In Progress → Completed/Cancelled, thêm nhánh Assigned → Rejected. Khi Work Order ở trạng thái Rejected, hệ thống cần đưa về hàng chờ để Facility Manager phân công lại (chưa xác định cơ chế cụ thể — cần Open Question mới nếu được duyệt).
Ảnh hưởng: BR-17, UC-09, UC-10, US-04-05, các AC liên quan đến trạng thái Work Order cần rà soát lại.
Trạng thái: Proposed — dựa trên dữ liệu mô phỏng cỡ mẫu rất nhỏ (n=1), cần stakeholder/giảng viên xác nhận chính thức và nên test lại với cỡ mẫu lớn hơn trước khi áp dụng.
DEC-02 — Xác nhận: ảnh đính kèm khi tạo Maintenance Request giữ nguyên là optional
Ngày: 03/09/2026
Người tham gia: Project Owner (mô phỏng), dựa trên usability test mô phỏng với 1 người đóng vai Requester
Yêu cầu liên quan: REQ-04
Open Question liên quan: Q-02
Vấn đề: Q-02 hỏi liệu Maintenance Request có bắt buộc đính kèm ảnh/video không.
Quyết định (đề xuất): Giữ nguyên theo REQ-04 hiện tại — ảnh/video không bắt buộc. Label giao diện "Thêm ảnh (không bắt buộc)" được xác nhận đủ rõ ràng qua usability test — người dùng chỉ dừng lại kiểm tra ngắn (dưới 5 giây) trước khi bỏ qua, không gây tắc nghẽn luồng.
Ảnh hưởng: Q-02 có thể chuyển Status từ Open sang Resolved trong open-questions.md.
Trạng thái: Proposed — cỡ mẫu nhỏ (n=1), nên xác nhận thêm nếu có điều kiện test với nhiều Requester hơn.
DEC-XX — Xác nhận: không cần bước "Approve" tách riêng trước khi tạo Work Order
Ngày: 03/09/2026
Người tham gia: Project Owner (mô phỏng), dựa trên usability test mô phỏng với 1 người đóng vai Facility Manager
Yêu cầu liên quan: REQ-13, REQ-14
Use Case liên quan: UC-07, UC-08
Open Question liên quan: Q-05
Vấn đề: Q-05 hỏi liệu Facility Manager có cần một bước "approve" Maintenance Request tách biệt trước khi tạo Work Order hay không.
Quyết định (đề xuất): Không cần bước approve riêng biệt. Usability test cho thấy Facility Manager tự nhiên đọc kỹ nội dung Request trước khi bấm "Tạo Work Order" — hành vi này đã đóng vai trò tương đương một bước xét duyệt không chính thức. Gộp chung hành động "xem xét" và "tạo Work Order" như thiết kế hiện tại của prototype là phù hợp.
Ảnh hưởng: Q-05 có thể chuyển Status từ Open sang Resolved. UC-07/UC-08 giữ nguyên không cần thêm bước mới.
Trạng thái: Proposed — cỡ mẫu nhỏ (n=1), nên xác nhận thêm với nhiều Facility Manager hơn, đặc biệt trong tình huống có nhiều Request dồn cùng lúc (áp lực xử lý nhanh có thể khiến FM bỏ qua bước đọc kỹ).
Open Questions

|  | Vai trò đóng | Vai trò | Ngày test | Ghi chú |
| 1 | Lê Thị Thanh Thảo | Requester | 03/09/2026 | Chưa dùng hệ thống quản lý bảo trì trước đây |
| 2 | Hồ Thị Thu Thảo | Facility Manager | 03/09/2026 | Chưa dùng hệ thống quản lý bảo trì trước đây |
| 3 | Đặng Như Trầm | Technician | 03/09/2026 | Chưa dùng hệ thống quản lý bảo trì trước đây |


|  | Flow (UC) | Người test | Observation | Issue | Decision |
| 1 | Flow 1 (UC-05) | Thảo | Dừng lại ~5 giây ở nút "Thêm ảnh (không bắt buộc)", nói: "để coi có bắt buộc không đã" trước khi bấm tiếp qua | Label đã ghi "(không bắt buộc)" nhưng người dùng vẫn có phản xạ kiểm tra kỹ trước khi bỏ qua — không phải lỗi nghiêm trọng | Giữ nguyên label hiện tại; không cần sửa vì hành vi "kiểm tra kỹ trước khi bỏ qua" là hợp lý, không gây tắc nghẽn (dưới 5s) |
| 2 | Flow 1 (UC-06) | Thảo | Sau khi gửi request, tự bấm đúng nút "Xem trạng thái yêu cầu" mà không cần gợi ý, xem đúng danh sách và hiểu các trạng thái mẫu hiển thị | Không có issue — hoàn thành mượt | Không cần thay đổi |
| 3 | Flow 2 (UC-07) | Thu Thảo | Đọc kỹ toàn bộ mô tả sự cố trước khi bấm "Tạo Work Order", mất khoảng 12 giây đọc | Không có issue — đúng hành vi mong muốn (không bấm ẩu) | Không cần thay đổi |
| 4 | Flow 2 (UC-08) | Thu Thảo | Khi thử tạo Work Order lần 2 cho cùng 1 request đã có WO, thấy thông báo lỗi ngay, nói: "à hiểu rồi, nó chặn vì đã có việc rồi" | Thông báo lỗi đủ rõ, người dùng tự suy luận đúng nguyên nhân (BR-06) mà không cần hỏi thêm | Không cần thay đổi |
| 5 | Flow 3 (UC-09) — Task 3 | Trầm | Trước khi bấm "Chấp nhận", có xem qua phần cảnh báo IoT hiển thị, đọc thành tiếng: "nhiệt độ cao bất thường lúc 14:32" | Thông tin IoT hữu ích, được người dùng chủ động tham khảo trước khi quyết định — đúng mục tiêu UC-09 bước 3-4 | Không cần thay đổi |
| 6 | Flow 3 (UC-09) — Task 3b (OQ-01) | Trầm | Xem mục riêng "Kết quả quan trọng nhất: OQ-01" bên dưới | Xem bên dưới | Xem bên dưới |
| 7 | Flow 4 (UC-15) | Thu Thảo | Nhận ra ngay Asset có nhãn "Risk: High" (màu rust/cam đậm) trong vòng 2 giây, không cần tìm | Màu sắc đủ nổi bật để phân biệt nhanh mức độ ưu tiên | Không cần thay đổi |
| 8 | Flow 4 (UC-14) | Thu Thảo | Sau khi đọc khuyến nghị AI, hỏi thêm: "vậy con số này AI tính ra bao nhiêu phần trăm chắc chắn?" dù không có trên màn hình | Người dùng có xu hướng muốn biết mức độ tin cậy cụ thể, không chỉ nhãn Low/Medium/High | Đề xuất resolve Q-13 theo hướng: cân nhắc hiển thị thêm % bên cạnh nhãn, hoặc ít nhất ghi chú "dựa trên bao nhiêu dữ liệu" để tăng độ tin cậy cảm nhận |


| Flow | Thanh Thảo | Thu Thảo | Tuấn | Tỷ lệ hoàn thành |
| Flow 1 — UC-05/06 | X | — | — | 1/1 |
| Flow 2 — UC-07/08 | — | X | — | 1/1 |
| Flow 3 — UC-09 | — | — | / (hoàn thành thao tác nhưng hiểu sai kết quả — xem OQ-01) | 1/1 (một phần) |
| Flow 4 — UC-12→15 | — | X | — | 1/1 |


| Open Question ID | Nội dung | Kết quả từ test | Cập nhật vào open-questions.md? |
| Q-01 | Requester xem Asset theo phòng hay toàn bộ? | Không phát sinh vấn đề trong test (Khôi chỉ thấy đúng 1 tài sản mẫu thuộc phòng của mình, không thắc mắc gì) — chưa đủ dữ liệu để kết luận chắc chắn | 0(giữ Open, cần test thêm với nhiều Asset hơn) |
| Q-02 | Ảnh có bắt buộc không? | Label "(không bắt buộc)" đã đủ rõ, người dùng chỉ dừng lại kiểm tra ngắn (~5s), không gây tắc nghẽn | X (đề xuất Resolved: giữ ảnh là optional, label hiện tại đã đạt) |
| Q-05 | FM có cần approve riêng trước khi tạo WO? | Lan (đóng vai FM) tự nhiên đọc kỹ nội dung trước khi tạo WO mà không cần có nút "Approve" tách riêng — hành vi "đọc kỹ rồi mới bấm Tạo" đã đóng vai trò tương đương approve | X (đề xuất Resolved: không cần bước approve riêng, gộp chung với hành động Tạo Work Order như hiện tại) |
| Q-08 | FM có cần xác nhận trước khi đóng Request? | Chưa được test trực tiếp trong lần mô phỏng này (thiếu thời gian) | 0 (giữ Open, cần test riêng bước "Xác nhận kết quả & Đóng yêu cầu") |
| Q-13 | Risk hiển thị Low/Medium/High hay kèm %? | Lan chủ động hỏi thêm về độ tin cậy/con số cụ thể dù không có trên UI | 0 (giữ Open nhưng nghiêng về hướng cần bổ sung thêm thông tin định lượng — cần test với nhiều Facility Manager hơn để chắc chắn) |


| ID | Open Question | Status hiện tại | Status đề xuất | Ghi chú cập nhật |
| Q-01 | Requester được xem Asset của phòng/khu vực mình phụ trách hay toàn bộ Asset trong hệ thống? | Open | Open (giữ nguyên) | Chưa đủ dữ liệu test — người test không gặp tình huống có nhiều Asset để bộc lộ vấn đề. Cần test lại với môi trường có nhiều phòng/Asset hơn. |
| Q-02 | Maintenance Request có bắt buộc đính kèm hình ảnh/video không? | Open | Resolved (đề xuất) | Usability test xác nhận label "(không bắt buộc)" đủ rõ, giữ nguyên theo REQ-04. Xem DEC-XX trong decision-log-additions.md. |
| Q-05 | Facility Manager có cần approve Maintenance Request trước khi tạo Work Order không? | Open | Resolved (đề xuất) | Usability test cho thấy hành vi đọc kỹ trước khi tạo WO đã thay thế vai trò approve riêng. Xem DEC-XX. |
| Q-08 | Sau khi Technician hoàn thành Work Order, Facility Manager có cần xác nhận trước khi đóng Request không? | Open | Open (giữ nguyên) | Chưa test trực tiếp bước này trong đợt mô phỏng — prototype đã có sẵn màn hình "Xác nhận kết quả & Đóng yêu cầu" theo giả định BR-18, nhưng chưa quan sát hành vi người dùng thật ở bước này. |
| Q-13 | Maintenance Risk hiển thị Low/Medium/High, % rủi ro hay kết hợp cả hai? | Open | Open (giữ nguyên, nghiêng về 1 hướng) | Facility Manager mô phỏng chủ động hỏi thêm về độ tin cậy/con số cụ thể — tín hiệu ban đầu nghiêng về hướng "kết hợp cả hai" (nhãn + %), nhưng cỡ mẫu quá nhỏ (n=1) để kết luận. |
