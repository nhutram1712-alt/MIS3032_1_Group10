# Đặc tả Xác thực và Phân quyền (Authentication & Authorization Specification)

> Project: Smart Maintenance & Facility Management  
> Status: Review Required — Open Findings  
> Version: 2.0 (BA review)  
> Last updated: 2026-10-10

---

## 1. Mục đích, phạm vi và tài liệu tham chiếu

### 1.1. Mục đích

Tài liệu này mô tả các yêu cầu nghiệp vụ về xác thực (Authentication) và kiểm soát quyền truy cập (Authorization) của hệ thống. Mục tiêu là xác định:
- Các Role của hệ thống.
- Những chức năng mà mỗi Role được phép hoặc không được phép thực hiện.
- Nguồn yêu cầu và quy tắc nghiệp vụ làm căn cứ.
- Acceptance Criteria dùng để kiểm chứng.
- Hiện trạng thực tế và các điểm còn thiếu cần quyết định thêm.

### 1.2. Phạm vi

Phạm vi của tài liệu bao gồm:
- Đăng nhập bằng tài khoản nội bộ của hệ thống.
- Phân quyền theo Role: Requester, Technician, Facility Manager, Admin.
- Quy tắc sở hữu dữ liệu (data ownership) cho Requester và Technician.
- Quy tắc đăng xuất và quản lý phiên đăng nhập.
- Xác thực API key cho IoT Gateway.

Không bao gồm:
- SSO/LDAP/Identity Provider bên ngoài.
- MFA / xác thực đa yếu tố.
- Xây dựng chi tiết kỹ thuật của controller/middleware/service.

### 1.3. Tài liệu nguồn

| Loại | Tài liệu | Vai trò |
|---|---|---|
| Requirements | `docs/02-vault/requirements.md` | Căn cứ yêu cầu chính 
| Business Rules | `docs/02-vault/business-rules.md` | Quy tắc nghiệp vụ bắt buộc |
| Decision Log | `docs/02-vault/decision-log.md` | Quyết định chính thức |
| Interview | `docs/02-vault/Interview.md` | Xác nhận stakeholder proxy |
| User Stories | `docs/03-product/user-stories.md` | Luồng người dùng 
| Acceptance Criteria | `docs/03-product/acceptance-criteria.md` | Điều kiện chấp nhận |
| API Contract | `docs/05-technical/api-contract.md` | Mô tả hành vi API |
| Security/NFR | `docs/06-test/security-nfr.md` | Bằng chứng kiểm tra bảo mật |
| Code review | `docs/06-test/code-review.md` | Nhận xét thiết kế và finding |

---

## 2. Authentication Requirements

### 2.1. Login

Yêu cầu chính thức:
- REQ-01: Hệ thống cho phép người dùng đăng nhập bằng tài khoản riêng của hệ thống.
- DEC-06: MVP sử dụng tài khoản riêng của hệ thống, không tích hợp SSO.

Hành vi nghiệp vụ mong đợi:
- Người dùng nhập username và password.
- Nếu thông tin hợp lệ, hệ thống xác thực và cho phép truy cập vào hệ thống.
- Nếu sai, hệ thống từ chối đăng nhập bằng thông báo lỗi rõ ràng.
- Người dùng chỉ được truy cập các chức năng phù hợp với Role của mình.

### 2.2. Session và Logout

Các yêu cầu liên quan:
- Khi người dùng đăng nhập thành công, hệ thống cấp phiên làm việc hợp lệ.
- Khi người dùng đăng xuất, token / phiên hiện tại phải bị vô hiệu hóa.
- Hệ thống phải ngăn các request với token đã bị thu hồi tiếp tục được xử lý.

Nguồn kiểm chứng:
- AC-US-01-01-01, AC-US-01-01-02, AC-US-01-01-03.
- `security-nfr.md`: SEC-03, SEC-04, SEC-09.

### 2.3. Password & Security Policy

Các quy tắc nghiệp vụ đã được xác định:
- Mật khẩu không được lưu plaintext.
- Dữ liệu xác thực phải được bảo vệ khỏi truy cập trái phép.
- Người dùng không được xem hoặc truyền lại mật khẩu của tài khoản khác.

Nguồn:
- NFR-02: "Hệ thống bảo vệ thông tin xác thực và dữ liệu người dùng khỏi truy cập trái phép."
- DEC-06.

### 2.4. IoT Gateway Authentication

Yêu cầu riêng đối với thiết bị IoT:
- Gateway/thiết bị không dùng JWT để xác thực.
- Gateway phải gửi dữ liệu với API Key hợp lệ.

Nguồn:
- REQ-28, REQ-29, BR-13.
- `security-nfr.md` SEC-05.

### 2.5. Current status

| Thành phần | Status | Ghi chú |
|---|---|---|
| Đăng nhập nội bộ | PASS | Có bằng chứng từ AC và test |
| Logout / blacklist token | PASS | Đã được kiểm tra |
| Password hash | PASS | Có hash, không lưu plaintext |
| IoT API key | PASS | Đã được kiểm tra |
| Session invalidation sau đổi Role/khóa tài khoản | PENDING DECISION | Chưa có bằng chứng thực thi rõ ràng |

---

## 3. Role và Permission Matrix

### 3.1. Bốn Role trong MVP

| Role | Vai trò nghiệp vụ | Chức năng chính |
|---|---|---|
| Requester | Người báo cáo sự cố | Tạo và theo dõi maintenance request |
| Technician | Người thực hiện bảo trì | Xem, nhận, thực hiện và hoàn thành work order |
| Facility Manager | Quản lý bảo trì | Quản lý asset, xử lý request, tạo phân công WO |
| Admin | Quản trị hệ thống | Quản lý tài khoản và quyền Role |

### 3.2. Role × Function Matrix

| Role | Chức năng nghiệp vụ | Được phép | Không được phép | Requirement / Rule ID | AC ID |
|---|---|---|---|---|---|
| Requester | Đăng nhập | Có | Không | REQ-01 | AC-US-01-01-01 |
| Requester | Tạo Maintenance Request | Có | Không | REQ-04, BR-05 | AC-US-03-01-01 |
| Requester | Xem Request của mình | Có | Xem Request của người khác | REQ-05, BR-05 | AC-US-03-02-01, AC-AUTHZ-04 |
| Requester | Xem Asset trong phạm vi được phép | Có | Xem Asset ngoài phạm vi | REQ-02, REQ-03, BR-04, DEC-01 | AC-US-01-02-01, AC-US-01-02-02 |
| Technician | Xem Work Order được giao | Có | Xem WO không được phân công | REQ-17, BR-07 | AC-US-04-04-01, AC-US-04-04-03 |
| Technician | Cập nhật Work Order | Có với WO thuộc quyền | Cập nhật WO của Technician khác | REQ-20, BR-07 | AC-US-04-05-01, AC-US-04-05-04 |
| Technician | Ghi nhận kết quả | Có | Không tự tạo WO | REQ-21, REQ-22 | AC-US-04-06-01, AC-US-04-06-02 |
| Facility Manager | Thêm/Sửa Asset | Có | Không | REQ-06, REQ-07 | AC-US-02-01-01, AC-US-02-02-01 |
| Facility Manager | Thay đổi Asset Status | Có | Không | REQ-08, BR-16 | AC-US-02-03-02 |
| Facility Manager | Xử lý Maintenance Request | Có | Không | REQ-13, BR-18 | AC-US-03-03-01, AC-US-03-04-01 |
| Facility Manager | Tạo & phân công Work Order | Có | Không | REQ-14, REQ-15 | AC-US-04-01-01, AC-US-04-02-01 |
| Facility Manager | Xem IoT Alert / Prediction | Có | Không | REQ-09, REQ-10, REQ-11 | AC-US-05-03, AC-US-06-01 |
| Admin | Quản lý User | Có | Không cho User khác | REQ-24 | AC-US-01-03-01, AC-US-01-03-02 |
| Admin | Quản lý Role / quyền | Có (theo ma trận cố định) | Không gán Admin cho người khác | REQ-25 | AC-US-01-04-01, AC-US-01-04-03 |
| Admin | Mapping IoT | Có | Không | REQ-26, REQ-27 | AC-US-05-01-01, AC-US-05-02-01 |

### 3.3. Quy tắc sở hữu dữ liệu (Data Ownership)

#### Quy tắc 1 — Requester ownership
- Requester chỉ được xem và theo dõi Request do chính mình tạo.
- Facility Manager được xem toàn bộ các Request.
- Nguồn: REQ-05, BR-05, AC-US-03-02-01.

#### Quy tắc 2 — Technician ownership
- Technician chỉ được thao tác với Work Order được phân công cho mình.
- Từ chối Work Order được xem như hành động kèm lý do, không phải trạng thái chính thức.
- Nguồn: REQ-20, BR-07, DEC-03, AC-US-04-05-04.

#### Quy tắc 3 — Requester Access to Asset
- Yêu cầu: Requester chỉ xem Asset thuộc phòng/khu vực được phép.
- Quy định này đã được xác nhận trong DEC-01 và Interview Decision 1.
- Tuy nhiên, cần xác nhận thực thi giao diện và dữ liệu ở tầng dữ liệu đầy đủ.

---

## 4. Authorization Scenarios and Acceptance Criteria

| AC ID | Given | When | Then | Source Requirement/Rule | Verification Evidence | Status |
|---|---|---|---|---|---|---|
| AC-US-01-01-01 | User có tài khoản hợp lệ | User nhập đúng username/password | Hệ thống xác thực và cho phép truy cập | REQ-01, DEC-06 | Test xác thực thực tế | PASS |
| AC-US-01-01-02 | User nhập sai thông tin | User thực hiện login | Hệ thống từ chối và trả lỗi | REQ-01 | Test login sai | PASS |
| AC-US-01-01-03 | User đã đăng nhập | User truy cập hệ thống | Chỉ thấy menu/chức năng theo role | REQ-01, NFR-07 | Kiểm tra menu theo role | PARTIALLY VERIFIED |
| AC-US-01-02-01 | Requester được phép sử dụng một phòng/khu vực | Requester xem danh sách Asset | Chỉ thấy Asset thuộc phạm vi được phép | REQ-02, DEC-01 | Yêu cầu xác nhận; còn thiếu bằng chứng đầy đủ | PARTIALLY VERIFIED |
| AC-US-01-02-02 | Asset ngoài phạm vi | Requester tìm hoặc truy cập Asset đó | Hệ thống không cho phép xem | REQ-02, BR-04 | Có logic kiểm soát nhưng chưa rõ ở mọi điểm truy cập | PARTIALLY VERIFIED |
| AC-US-01-03-01 | Admin đã đăng nhập | Admin truy cập chức năng user management | Hệ thống hiển thị danh sách user | REQ-24 | Test user list | PASS |
| AC-US-01-03-02 | Admin tạo user mới | Admin nhập đúng thông tin | Hệ thống tạo tài khoản mới | REQ-24 | Test tạo User | PASS |
| AC-US-01-03-03 | Dữ liệu user không hợp lệ | Admin tạo user | Hệ thống từ chối và báo lỗi | REQ-24 | Có xử lý validate | PASS |
| AC-US-01-04-01 | Admin đã đăng nhập | Admin truy cập quản lý role | Hệ thống hiển thị quyền hiện có | REQ-25 | Ma trận quyền cố định | PASS |
| AC-US-01-04-02 | Admin thay đổi quyền role | Admin thực hiện cập nhật | Hệ thống lưu cấu hình quyền mới | REQ-25, US-01-04 | Mâu thuẫn với DEC-06 và BR không có role matrix động | PENDING DECISION |
| AC-US-01-04-03 | User không có quyền | User cố truy cập chức năng đặc quyền | Hệ thống từ chối | REQ-25, NFR-01 | Có test 401/403 | PASS |
| AC-US-03-01-01 | Requester hợp lệ | Requester tạo request | Request được tạo với trạng thái Submitted | REQ-04, BR-05 | Test tạo request | PASS |
| AC-US-03-02-01 | Requester có request | Requester xem request | Hệ thống hiển thị trạng thái | REQ-05 | Test xem request | PASS |
| AC-US-03-03-01 | Có request ở trạng thái Submitted | Facility Manager xử lý request | Request được cập nhật để xử lý | REQ-13 | Test xử lý request | PASS |
| AC-US-03-04-01 | Work Order đã hoàn thành | Facility Manager xác nhận | Request có thể đóng | REQ-13, BR-18 | Test đóng request | PASS |
| AC-US-04-01-01 | Có request hoặc nhu cầu bảo trì | Facility Manager tạo WO | Hệ thống tạo WO và liên kết asset | REQ-14, BR-08 | Test tạo WO | PASS |
| AC-US-04-02-01 | WO tồn tại và Technician hợp lệ | Facility Manager phân công | Hệ thống gán WO cho Technician | REQ-15 | Test phân công | PASS |
| AC-US-04-04-01 | Technician có WO | Technician truy cập danh sách WO | Chỉ thấy WO được phân công cho mình | REQ-17, BR-07 | Test ownership | PASS |
| AC-US-04-05-01 | WO được phân công cho mình | Technician cập nhật state | Hệ thống lưu thay đổi | REQ-20, BR-07 | Test cập nhật WO | PASS |
| AC-US-04-05-04 | WO không thuộc quyền | Technician cố truy cập | Hệ thống từ chối | REQ-20, BR-07 | Test ownership 403 | PASS |
| AC-US-04-06-02 | Technician đã ghi nhận kết quả | Technician hoàn thành WO | WO chuyển sang Completed | REQ-22, BR-17 | Test hoàn thành WO | PASS |
| AC-US-05-01-01 | Asset và device hợp lệ | Admin mapping | Hệ thống lưu mapping | REQ-26 | Test mapping | PASS |
| AC-US-05-02-01 | Asset đã có mapping | Admin cập nhật mapping | Hệ thống cập nhật mapping mới | REQ-27 | Test cập nhật mapping | PASS |
| AC-US-05-03-01 | Asset đã có IoT data | FM xem IoT Data | Hệ thống hiển thị dữ liệu | REQ-09, REQ-28 | Test IoT data | PASS |
| AC-US-05-04-01 | IoT Data bất thường | Hệ thống nhận dữ liệu | Tạo IoT Alert | REQ-10 | Kiểm tra alert | PASS |
| AC-US-06-01-01 | Có dữ liệu đủ | Hệ thống tạo AI Prediction | Tạo Prediction cho asset | REQ-11, REQ-29 | Test prediction | PASS |
| AC-US-06-02-01 | Asset có Prediction hợp lệ | Facility Manager xem Prediction | Hệ thống hiển thị Maintenance Risk | REQ-12 | Test xem risk | PASS |

---

## 5. Findings và Business Decisions Required

### Finding A — REQ-02, quyền xem Asset của Requester

**Nội dung:**
Requester chỉ được xem Asset thuộc phòng/khu vực được phép sử dụng. Quy định này được xác nhận trong DEC-01 và Interview. Tuy nhiên, hiện trạng thực tế cần phân biệt rõ giữa:
- Hệ thống chặn request ở tầng API; và
- Giao diện người dùng có thực sự giới hạn đúng phạm vi hay không.

**Kết luận:**
- Yêu cầu đã xác định rõ.
- Chưa có bằng chứng đầy đủ trên UI rằng tất cả các điểm truy cập đều giới hạn đúng theo room/zone.
- Không tự đổi REQ-02 thành quyền xem toàn bộ Asset khi chưa có quyết định phê duyệt.

**Status:** PENDING DECISION

### Finding B — Mâu thuẫn US-01-04

**Nội dung:**
AC-US-01-04-02 mô tả việc Admin cập nhật quyền của một Role. Nhưng Story Spec và Decision Log gợi ý ma trận quyền là cố định. Không có mô tả rõ ràng về kiểu thay đổi quyền động hay quyền “xem” như một matix cố định.

**Tác động:**
- Dẫn đến mơ hồ trong cách hiểu quyền Admin.
- Có thể dẫn đến việc đánh giá nhầm là hệ thống hỗ trợ cấu hình quyền động khi thực tế không có.

**Status:** PENDING DECISION

### Finding C — Ma trận quyền và chức năng thực tế

**Nội dung:**
Ma trận quyền cần phân biệt rõ hai khía cạnh:
1. Quyền truy cập chức năng (có thể sử dụng chức năng nào).
2. Quy tắc sở hữu dữ liệu (có thể truy cập dữ liệu nào).

Ví dụ: Technician có quyền xem Work Order được giao nhưng không được xem WO của Technician khác. Đây là quyền thực thi theo ownership, không chỉ là Role.

**Kết luận:**
- Quy tắc ownership đã được xác định rõ trong BR-07 và các AC tương ứng.
- Tuy nhiên, cần kiểm tra tính nhất quán giữa product spec, backend và UI.

### Finding D — Frontend Route Guard

**Nội dung:**
Một người dùng có thể vào trực tiếp URL của route đặc quyền, nhưng việc tải được trang không đồng nghĩa với việc có thể thực hiện API đặc quyền. Đây là sự khác biệt giữa:
- Route protection ở frontend.
- Authorization enforcement ở backend.

**Kết luận:**
- Backend hiện có kiểm soát thực sự tốt.
- Frontend route guard chưa nhất quán ở một số màn hình, và đây là gap UX / bảo vệ route cần xử lý.
- Không suy luận rằng mọi phân quyền backend đều bị vượt qua nếu route UI chưa chặn.

### Finding E — Token sau khi thay đổi Role hoặc vô hiệu hóa tài khoản

**Nội dung:**
Có bằng chứng rằng hệ thống có cơ chế logout và blacklist token. Tuy nhiên, khi tài khoản bị khóa hoặc thay đổi Role, hành vi chi tiết về session hiện tại chưa được phát biểu rõ.

**Cần quyết định:**
- Khi role/account status thay đổi, token hiện hữu có bị thu hồi ngay không?
- Nếu không, sinh viên / người dùng sẽ vẫn giữ quyền cũ cho đến khi hết thời gian token.

**Status:** PENDING DECISION

---

## 6. Verification Status

| Khu vực | Status | Ghi chú |
|---|---|---|
| Login thành công / thất bại | PASS | Có bằng chứng test và AC |
| Authorization theo Role | PASS | 401/403 test đã chạy |
| Logout / blacklist | PASS | Có kiểm tra thực tế |
| Password hash | PASS | Có bằng chứng bảo mật |
| IoT API key | PASS | Đã kiểm tra |
| Quyền xem Asset của Requester | PARTIALLY VERIFIED | Cần xác nhận ở UI và dữ liệu toàn diện |
| Role matrix động | PENDING DECISION | Mâu thuẫn giữa AC và DEC-06 |
| Frontend route guard | PARTIALLY VERIFIED | API chặn tốt, route UI chưa được đồng bộ |
| Session invalidation sau đổi Role | PENDING DECISION | Chưa có xác nhận rõ |

---

## 7. Traceability and Summary

| Requirement | Business Rule / Decision | User Story | Acceptance Criteria | Evidence | Status |
|---|---|---|---|---|---|
| REQ-01 | DEC-06 | US-01-01 | AC-US-01-01-01,02,03 | Login tests | PASS |
| REQ-02 | BR-04, DEC-01 | US-01-02 | AC-US-01-02-01,02 | Yêu cầu + kiểm tra hệ thống | PARTIALLY VERIFIED |
| REQ-24 | DEC-06 | US-01-03 | AC-US-01-03-01,02,03 | User management tests | PASS |
| REQ-25 | DEC-06 | US-01-04 | AC-US-01-04-01,02,03 | Role management + QT guard | PENDING DECISION |
| REQ-05 | BR-05, BR-14, DEC-02 | US-03-02 | AC-US-03-02 | Request status tests | PASS |
| REQ-14 | BR-06, BR-08 | US-04-01 | AC-US-04-01 | WO creation tests | PASS |
| REQ-17, REQ-20 | BR-07, DEC-03 | US-04-04, US-04-05 | AC-US-04-04, AC-US-04-05 | Ownership tests | PASS |
| REQ-26, REQ-27 | BR-13 | US-05-01, US-05-02 | AC-US-05-01, AC-US-05-02 | Mapping tests | PASS |
| REQ-09, REQ-10 | BR-12, BR-13 | US-05-03, US-05-04 | AC-US-05-03, AC-US-05-04 | IoT alert tests | PASS |
| REQ-11, REQ-12 | BR-10, BR-11, BR-15 | US-06-01, US-06-02 | AC-US-06-01, AC-US-06-02 | Prediction tests | PASS |

### Tổng kết

- Hệ thống có các Role rõ ràng và phân quyền cơ bản tương ứng với yêu cầu MVP.
- Xác thực và phân quyền ở tầng backend có bằng chứng tốt.
- Các luồng chính (login, request, work order, IoT, AI) có test và AC tương ứng.
- Vẫn còn các điểm open cần BA/PO quyết định: REQ-02, US-01-04, session invalidation sau đổi Role/khóa tài khoản.
- Do đó, trạng thái phù hợp là: Review Required — Open Findings, không đánh dấu hoàn tất tuyệt đối.

---

## 8. Kết luận BA

Tài liệu này phản ánh trạng thái thực tế và không đánh dấu tất cả các điều kiện đã đạt chỉ vì có requirement hoặc story. Bản chất của AuthN/AuthZ trong MVP là: core behavior đã có bằng chứng, nhưng vẫn còn những điểm cần quyết định nghiệp vụ để giúp hệ thống trở nên rõ ràng hơn và dễ bảo vệ trong buổi đánh giá BA.

**Key take-away:**
- Backend security: strong evidence.
- Frontend access control: partial.
- Business rule clarity: good, but a few open decisions remain.
- Release status: conditional/needs BA confirmation, not “fully approved”.
