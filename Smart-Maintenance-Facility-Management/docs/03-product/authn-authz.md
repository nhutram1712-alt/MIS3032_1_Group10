# Đặc tả Xác thực và Phân quyền (Authentication & Authorization Specification)

> **Dự án:** Smart Maintenance & Facility Management  
> **Tổ chức:** Trường Đại học Kinh tế – Đại học Đà Nẵng (DUE)  
> **Phiên bản:** 1.1  
> **Trạng thái:** Review Required — As-built Security Review with Open Findings (Draft — chờ BA sign-off)  
> **Ngày soạn:** 10/10/2026  
> **Commit tham chiếu:** `ecac1c9` (GitHub HEAD: `ecac1c9c88a631ba94ed464b334c410d90cf001d`)  
> **Vai trò thực hiện:** Business Analyst & Technical Documentation Reviewer  
> **Vị trí file:** `docs/03-product/authn-authz.md`

---

## 1. Mục đích, phạm vi và tham chiếu

### 1.1. Mục đích
Tài liệu này định nghĩa chi tiết cơ chế Xác thực (Authentication - AuthN) và Phân quyền (Authorization - AuthZ) của hệ thống **Smart Maintenance & Facility Management**. Tài liệu xác lập các quy tắc phân quyền theo vai trò (Role-Based Access Control - RBAC) và quy tắc sở hữu dữ liệu (Data Ownership) dựa trên **mã nguồn thực tế (As-built behavior)** của hệ thống, đồng thời đối chiếu với các yêu cầu nghiệp vụ (Requirements, Business Rules) đã được baselined để chỉ ra:
1. **Requirement:** Yêu cầu nghiệp vụ chính thức từ tài liệu nguồn.
2. **As-built Behavior:** Hành vi thực tế quan sát được từ mã nguồn và kiểm thử.
3. **Open Findings / Decision Required:** Các điểm mâu thuẫn, khoảng cách nghiệp vụ (gap) cần BA và Product Owner chính thức ra quyết định.

### 1.2. Phạm vi
- **Backend:** Toàn bộ 10 API Controller (28 action endpoints), Middleware xác thực, In-Memory Token Blacklist, Filter bảo mật và Application Layer trong các dự án `SmartMaintenance.Api`, `SmartMaintenance.Application`, `SmartMaintenance.Infrastructure`.
- **Frontend:** Hệ thống Route Guard (`RequireAuth`, `RequireRoles`) và phân chia Navigation Menu theo vai trò người dùng trong `src/frontend/src/App.tsx`.
- **Thiết bị ngoại vi:** Cơ chế xác thực qua API Key dành cho IoT Gateway khi gửi dữ liệu đo đạc (telemetry/ingest).

### 1.3. Tài liệu tham chiếu
- **Requirements (Trích nguyên văn từ `docs/02-vault/requirements.md`):** 
  - `REQ-01`: Hệ thống cho phép người dùng đăng nhập bằng tài khoản riêng của hệ thống.
  - `REQ-02`: Requester chỉ được xem Asset thuộc phòng/khu vực mình được phép sử dụng.
  - `REQ-03`: Requester có thể xem trạng thái hiện tại của Asset thuộc phạm vi được phép.
  - `REQ-04`: Requester có thể tạo Maintenance Request để báo cáo vấn đề của Asset/khu vực. Hình ảnh/video là không bắt buộc.
  - `REQ-05`: Requester có thể theo dõi Maintenance Request với các trạng thái Submitted, Pending, In Progress, Resolved, Closed hoặc Rejected.
  - `REQ-06`: Facility Manager có thể thêm Asset với Asset ID, Name, Type, Location và Status.
  - `REQ-07`: Facility Manager có thể cập nhật thông tin cơ bản của Asset.
  - `REQ-08`: Facility Manager có thể xem Asset và thay đổi thủ công Asset Status.
  - `REQ-09`: Facility Manager có thể xem IoT Data của Asset.
  - `REQ-10`: Hệ thống tạo IoT Alert khi IoT Data vượt threshold/điều kiện bất thường đã cấu hình (hành vi hệ thống).
  - `REQ-11`: Hệ thống cung cấp AI Prediction về khả năng Asset cần bảo trì trong 7 ngày tiếp theo (hành vi hệ thống).
  - `REQ-12`: Hệ thống hiển thị Maintenance Risk ở 3 mức Low, Medium, High (hành vi hệ thống).
  - `REQ-13`: Facility Manager tiếp nhận/xử lý Maintenance Request và xác nhận kết quả trước khi đóng Request.
  - `REQ-14`: Facility Manager có thể tạo một Work Order từ Maintenance Request hoặc nhu cầu bảo trì được xác định.
  - `REQ-15`: Facility Manager có thể phân công Work Order cho Technician.
  - `REQ-16`: Facility Manager có thể theo dõi Work Order với trạng thái Assigned, In Progress, Completed hoặc Cancelled.
  - `REQ-17`: Technician có thể xem các Work Order được phân công cho mình.
  - `REQ-18`: Technician có thể xem thông tin Asset liên quan đến Work Order.
  - `REQ-19`: Technician có thể xem IoT Alert và AI Prediction liên quan đến Asset.
  - `REQ-20`: Technician có thể cập nhật Work Order được phân công và từ chối Work Order kèm lý do.
  - `REQ-21`: Technician có thể ghi nhận kết quả kiểm tra, sửa chữa hoặc bảo trì.
  - `REQ-22`: Technician có thể hoàn thành Work Order sau khi thực hiện và ghi nhận kết quả bảo trì.
  - `REQ-23`: Hệ thống lưu kết quả Work Order hoàn thành vào Maintenance History của Asset (hành vi hệ thống).
  - `REQ-24`: Admin có thể quản lý tài khoản người dùng.
  - `REQ-25`: Admin có thể quản lý quyền truy cập theo Role.
  - `REQ-26`: Admin có thể mapping một Asset với một IoT Device/Sensor.
  - `REQ-27`: Admin có thể cập nhật IoT Mapping.
  - `REQ-28`: Hệ thống thu thập và lưu trữ IoT Data theo chu kỳ 5 phút/lần.
- **Non-Functional Requirements (`docs/02-vault/requirements.md`):** 
  - `NFR-01`: Hệ thống kiểm soát quyền truy cập dựa trên Role.
  - `NFR-02`: Hệ thống bảo vệ thông tin xác thực và dữ liệu người dùng khỏi truy cập trái phép.
  - `NFR-07`: Giao diện cung cấp chức năng và thông tin phù hợp với từng Role.
- **Ràng buộc hệ thống (Constraints):** 
  - `CON-03`: MVP chỉ có 4 Role cố định: `Requester`, `Technician`, `FacilityManager`, `Admin`.
  - `CON-08`: MVP sử dụng tài khoản riêng của hệ thống, không tích hợp SSO.
- **Business Rules (`docs/02-vault/business-rules.md`):** 
  - `BR-04`: Mỗi Asset phải có Location/Room (điều kiện dữ liệu tiên quyết).
  - `BR-05`: Maintenance Request phải xác định Asset hoặc khu vực bị ảnh hưởng trước khi tạo Work Order.
  - `BR-06`: Một Maintenance Request chỉ tạo tối đa một Work Order trong MVP.
  - `BR-07`: Technician chỉ được cập nhật Work Order được phân công cho mình; có thể từ chối kèm lý do.
  - `BR-08`: Mỗi Work Order phải liên kết với một Asset cụ thể.
  - `BR-09`: Work Order hoàn thành phải có kết quả lưu vào Maintenance History.
  - `BR-13`: IoT Device phải được mapping với Asset trước khi monitoring. Một Asset chỉ có một IoT Device trong MVP.
  - `BR-14`: Lifecycle Maintenance Request (`Submitted` → `Pending` → `In Progress` → `Resolved` → `Closed` / `Rejected`).
  - `BR-16`: Chỉ Facility Manager được thay đổi thủ công Asset Status.
  - `BR-17`: Lifecycle Work Order (`Assigned` → `In Progress` → `Completed` / `Cancelled`).
  - `BR-18`: Maintenance Request chỉ được Closed sau khi Technician hoàn thành Work Order và Facility Manager xác nhận kết quả.
- **Quyết định kiến trúc (`docs/02-vault/decision-log.md`):** 
  - `DEC-01`: Giới hạn phạm vi Asset và quyền xem của Requester (chỉ xem Asset thuộc phòng/khu vực được phép).
  - `DEC-02`: Quy trình xử lý Maintenance Request.
  - `DEC-03`: Quy trình Work Order và quyền từ chối của Technician.
  - `DEC-04`: Giám sát IoT và ngưỡng cảnh báo.
  - `DEC-06`: Authentication nội bộ và RBAC 4 Roles.
- **Tài liệu Kỹ thuật, Đặc tả Story & Báo cáo QA:**
  - `docs/05-technical/specs/US-01-01-Spec.md` đến `US-01-04-Spec.md`.
  - `docs/03-product/acceptance-criteria.md` (EPIC-01).
  - `docs/05-technical/api-contract.md`.
  - `docs/06-test/security-nfr.md` (Biên bản kiểm tra bảo mật ngày 18/09/2026).
  - `docs/06-test/code-review.md` (Báo cáo review code & findings).
  - `docs/06-test/qa-verification.md` (Biên bản chạy lại test).
  - `docs/06-test/Bug_Log_Smart_Maintenance.xlsx` (Bug Log chuẩn hóa).

---

## 2. Authentication (Xác thực)

### 2.1. Phương thức đăng nhập
- **Endpoint:** `POST /api/auth/login` (cho phép truy cập ẩn danh `[AllowAnonymous]`).  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Controllers/AuthController.cs:20-30`.
- **Cơ chế xác thực thực tế (As-built):**
  - Client gửi payload JSON gồm `username` và `password`.
  - Hệ thống tìm kiếm bản ghi trong bảng `Users` theo điều kiện `u.Username == request.Username.Trim() && u.IsActive`.  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:32-34`.
  - Nếu không tìm thấy người dùng hoặc tài khoản bị vô hiệu hóa (`IsActive == false`), hệ thống trả về `null` và Controller phản hồi `401 Unauthorized` kèm `{ "error": "Invalid username or password." }`.  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:29-37` và `AuthController.cs:27-28`.
  - Mật khẩu người dùng được so khớp bằng hàm `BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)`.  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:36`.
  - Hệ thống không hỗ trợ đăng nhập một lần (SSO) theo ràng buộc `CON-08`.

### 2.2. JSON Web Token (JWT)
- **Thời hạn hiệu lực:** 480 phút (tương đương 8 giờ làm việc), được cấu hình tại khóa `Jwt:ExpiresMinutes` trong file cấu hình và nạp vào token handler.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/appsettings.json:12` và `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:82-84, 103`.
- **Thuật toán ký & Secret Key:** Sử dụng thuật toán `HmacSha256` với khóa bí mật đối xứng `Jwt:Secret`. Khóa bí mật bắt buộc phải có độ dài tối thiểu từ 32 ký tự trở lên.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Program.cs:33-36` và `AuthService.cs:86-87`.
- **Danh sách Claims được nhúng trong Token:**
  1. `JwtRegisteredClaimNames.Sub`: Lưu `UserId` (kiểu chuỗi).
  2. `JwtRegisteredClaimNames.Jti`: Định danh token duy nhất sinh ngẫu nhiên dạng Guid (`Guid.NewGuid().ToString("N")`).
  3. `ClaimTypes.NameIdentifier`: Lưu `UserId`.
  4. `ClaimTypes.Name`: Lưu `username`.
  5. `ClaimTypes.Role`: Lưu chuỗi Role (`Requester`, `Technician`, `FacilityManager`, `Admin`).
  6. `"role"`: Lưu chuỗi Role (phục vụ tương thích claims mở rộng của frontend/client).  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:88-97`.
- **Cấu hình xác thực Token:** `MapInboundClaims = false`, bắt buộc kiểm tra Issuer (`Jwt:Issuer`), Audience (`Jwt:Audience`), thời hạn còn hiệu lực (`ValidateLifetime = true`) và chữ ký hợp lệ (`ValidateIssuerSigningKey = true`).  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Program.cs:42-52`.

### 2.3. Đăng xuất và Cơ chế Blacklist Token
- **Endpoint:** `POST /api/auth/logout` (yêu cầu token xác thực `[Authorize]`).  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Controllers/AuthController.cs:32-45`.
- **Cơ chế thu hồi (Revocation / Blacklist):**
  - Hệ thống đọc chuỗi Raw Bearer Token từ header `Authorization`, phân tích giải mã JWT token.
  - Lấy claim `jti` của token. Trường hợp token không có `jti`, hệ thống băm chuỗi token bằng thuật toán SHA-256 (`SHA256.HashData`) chuyển sang chuỗi Hex làm khóa đại diện.  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:54-63`.
  - Khóa này cùng thời điểm hết hạn của token (`jwt.ValidTo`) được đưa vào dịch vụ quản lý danh sách đen `ITokenBlacklist` (được đăng ký dưới dạng Singleton).  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:65-66` và `ServiceCollectionExtensions.cs:34`.
  - Cấu trúc `TokenBlacklist` lưu trữ trên bộ nhớ RAM bằng `ConcurrentDictionary<string, DateTime>` và tự động dọn dẹp các mục hết hạn (`CleanupExpired`).  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/TokenBlacklist.cs:8-44`.
  - Khi có request gửi lên, sự kiện `OnTokenValidated` trong JwtBearer Middleware sẽ kiểm tra khóa token đối với `ITokenBlacklist`. Nếu khóa tồn tại trong danh sách đen, request lập tức bị từ chối với lệnh `context.Fail("Token has been revoked.")`, trả về mã lỗi `401 Unauthorized`.  
    *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Program.cs:53-76`.
- **Hạn chế kỹ thuật trọng yếu của In-Memory Blacklist (Technical Limitations):**
  1. *Khi ứng dụng khởi động lại (Restart/Crash):* Do danh sách đen chỉ lưu trên bộ nhớ RAM, toàn bộ `jti` đã bị đưa vào blacklist sẽ mất hoàn toàn khi service restart. Một token đã đăng xuất nhưng còn hạn hiệu lực (tối đa 480 phút) sẽ lại trở nên hợp lệ và sử dụng được tiếp.
  2. *Môi trường chạy cụm nhiều instance (Multi-instance / Horizontal Scaling):* Blacklist nằm cục bộ ở RAM của từng tiến trình backend. Một token đăng xuất tại Node A sẽ không bị chặn nếu request kế tiếp được định tuyến tới Node B (thiếu Distributed Cache như Redis hoặc bảng lưu trữ tập trung).

### 2.4. Băm mật khẩu (Password Hashing)
- Sử dụng thuật toán BCrypt thông qua thư viện `BCrypt.Net-Next`. Mật khẩu plaintext được hash với salt tự động trước khi lưu vào cột `PasswordHash` trong cơ sở dữ liệu.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Application/Users/UserService.cs:74` và `DbSeeder.cs:13`.
- **Thuật ngữ chuẩn:** Đây là cơ chế **Băm mật khẩu (Password Hashing)** một chiều có salt, không phải là "Mã hóa" (Encryption) vì không có chức năng giải mã ngược lại mật khẩu ban đầu.
- **Mật khẩu khởi tạo demo:** Hệ thống khởi tạo tài khoản demo ban đầu theo thông tin quy định tại `appsettings.json`, `README.md` hoặc `.env.example`.
- **Bảo vệ DTO:** Tuyệt đối không bao giờ trả trường `PasswordHash` ra ngoài client trong bất kỳ API nào. Đối tượng `UserSummary` chỉ chứa: `UserId`, `Username`, `FullName`, `Role`, `IsActive`.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Application/Users/UserService.cs:40-47, 153-160`.

> **Cảnh báo bảo mật (Security Warning):** File `appsettings.json` trong kho mã nguồn hiện đang chứa các giá trị placeholder dành riêng cho môi trường phát triển (secret key đối xứng, default api key). Khi triển khai lên môi trường kiểm thử chính thức hoặc môi trường vận hành (production), các giá trị này **bắt buộc phải được chuyển sang biến môi trường (Environment Variables)** hoặc hệ thống quản lý khóa bảo mật chuyên dụng (Secret Manager), tuyệt đối không commit secret vào kho mã nguồn.

### 2.5. Xác thực API Key của IoT Gateway
- **Endpoint:** `POST /api/iot/ingest`.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Controllers/IotController.cs:20-34`.
- **Cơ chế:** Endpoint được cấu hình `[AllowAnonymous]` (không sử dụng JWT Bearer Token) kết hợp với Custom Authorization Filter `[IotGatewayAuthorize]`.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Controllers/IotController.cs:22-23`.
- **Filter thực thi:** `IotGatewayAuthorizeAttribute` kiểm tra sự tồn tại của header `X-Api-Key` trong HTTP Request và so khớp chuỗi với giá trị cấu hình `Iot:GatewayApiKey` (giá trị cấu hình phát triển được định nghĩa trong `appsettings.json` / `.env.example`).  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Security/IotGatewayAuthorizeAttribute.cs:6-21` và `appsettings.json:15`.
- Nếu header `X-Api-Key` bị thiếu hoặc giá trị không khớp chính xác, filter sẽ chặn đứng request và trả về ngay mã `401 Unauthorized` kèm body: `{ "error": "Invalid or missing gateway API key." }`.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Security/IotGatewayAuthorizeAttribute.cs:16`.
- **Lưu ý kiểm thử:** Test tự động `Ingest_BadApiKey_Returns401` hiện chỉ kiểm tra trường hợp gửi key sai (`X-Api-Key: wrong`), chưa có test case tự động kiểm thử riêng cho trường hợp request hoàn toàn **thiếu header `X-Api-Key`**.

### 2.6. Hành vi khi thay đổi Role hoặc Vô hiệu hóa tài khoản
- Admin thay đổi vai trò hoặc khóa tài khoản thông qua endpoint `PUT /api/users/{id}`.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Api/Controllers/UsersController.cs:48-62`.
- **Hành vi thực tế trong mã nguồn (As-built behavior):** Khi Admin cập nhật `Role` mới hoặc gán `IsActive = false`, hệ thống cập nhật bản ghi trong cơ sở dữ liệu (`_db.SaveChangesAsync`). Tuy nhiên, mã nguồn **KHÔNG** thực hiện gọi hàm `_tokenBlacklist.Blacklist(...)` đối với các token đang hoạt động của người dùng đó.  
  *Vị trí mã nguồn:* `src/backend/SmartMaintenance.Application/Users/UserService.cs:141-144`.
- **Khoảng cách bảo mật (Security Gap):**
  - *Expected Behavior (Kỳ vọng an toàn):* Khi tài khoản bị vô hiệu hóa (`IsActive = false`) hoặc bị giáng cấp Role, toàn bộ phiên đăng nhập hiện hữu phải bị vô hiệu hóa tức thời.
  - *Actual Behavior (Thực tế As-built):* Token JWT đã cấp cho người dùng vẫn tiếp tục có hiệu lực đầy đủ với các quyền của Role cũ cho tới khi token hết thời hạn (tối đa 480 phút) hoặc người dùng chủ động gọi đăng xuất. Người dùng chỉ bị áp dụng Role mới hoặc bị chặn đăng nhập khi thực hiện đăng nhập lại để nhận token mới. (Xem mục 7: Finding FIND-05).

---

## 3. Ma trận Role × Endpoint

Bảng ma trận dưới đây tổng hợp toàn bộ 28 endpoints thực tế trong 10 Backend Controllers, xác định Role được phép, ràng buộc dữ liệu (Ownership/Scope), nơi thực thi và vị trí mã nguồn chứng minh.

| Endpoint | Method | Role được phép | Quy tắc Dữ liệu & Ownership | Thực thi ở | Nguồn Code (file:dòng) |
|---|---|---|---|---|---|
| `/api/auth/login` | POST | Anonymous (Public) | Không kiểm tra quyền; kiểm tra credentials đăng nhập | Backend | `SmartMaintenance.Api/Controllers/AuthController.cs:20-21` |
| `/api/auth/logout` | POST | Requester, Technician, FacilityManager, Admin | Thu hồi chính token của phiên hiện tại vào blacklist | Backend | `SmartMaintenance.Api/Controllers/AuthController.cs:32-33` |
| `/api/users` | GET | FacilityManager, Admin | FM bắt buộc phải kèm query `?role=...` (thường là `role=Technician`); Admin xem tất cả | Backend | `SmartMaintenance.Api/Controllers/UsersController.cs:21-28` |
| `/api/users` | POST | Admin | Tạo user mới; cấm trùng username; role thuộc 4 roles hợp lệ | Backend | `SmartMaintenance.Api/Controllers/UsersController.cs:34-35` |
| `/api/users/{id}` | PUT | Admin | Cấm sửa Admin (403); cấm đổi Requester (400); cấm gán Admin (400); chỉ xoay Technician ↔ FacilityManager | Backend | `SmartMaintenance.Api/Controllers/UsersController.cs:48-49` & `UserService.cs:99-131` |
| `/api/roles/permissions` | GET | Admin | Không; trả về danh sách phân quyền tĩnh của hệ thống | Backend | `SmartMaintenance.Api/Controllers/RolesController.cs:20-21` |
| `/api/assets` | POST | FacilityManager | Gán `CreatedByUserId` theo ID của FacilityManager đăng nhập | Backend | `SmartMaintenance.Api/Controllers/AssetsController.cs:30-31` |
| `/api/assets` | GET | Requester, Technician, FacilityManager, Admin | BE cho phép cả 4 Role xem toàn bộ Asset; FE bọc chặn Requester (xem Finding FIND-03) | Backend + FE-only chặn Requester | `SmartMaintenance.Api/Controllers/AssetsController.cs:43-44` & `frontend/App.tsx:50` |
| `/api/assets/{id}` | GET | Requester, Technician, FacilityManager, Admin | BE cho phép cả 4 Role xem chi tiết Asset; FE bọc chặn Requester | Backend + FE-only chặn Requester | `SmartMaintenance.Api/Controllers/AssetsController.cs:52-53` & `frontend/App.tsx:50` |
| `/api/assets/{id}` | PUT | FacilityManager | Cập nhật thông tin tài sản cơ bản | Backend | `SmartMaintenance.Api/Controllers/AssetsController.cs:62-63` |
| `/api/assets/{id}/status` | PATCH | FacilityManager | Cập nhật thủ công trạng thái tài sản theo BR-16 | Backend | `SmartMaintenance.Api/Controllers/AssetsController.cs:71-72` |
| `/api/assets/{id}/iot-data` | GET | FacilityManager, Technician | Xem danh sách telemetry đo đạc của thiết bị gắn với tài sản | Backend | `SmartMaintenance.Api/Controllers/AssetsController.cs:80-81` |
| `/api/assets/{id}/prediction` | GET | FacilityManager, Technician | Xem kết quả dự báo bảo trì AI gần nhất của tài sản | Backend | `SmartMaintenance.Api/Controllers/AssetsController.cs:89-90` |
| `/api/requests` | POST | Requester | Gán `RequesterId` theo JWT Claim của người gửi | Backend | `SmartMaintenance.Api/Controllers/RequestsController.cs:21-22` & `RequestService.cs:52` |
| `/api/requests` | GET | Requester, FacilityManager | **Ownership:** Requester chỉ thấy yêu cầu do chính mình tạo; FM thấy toàn bộ | Backend | `SmartMaintenance.Api/Controllers/RequestsController.cs:34-35` & `RequestService.cs:74-77` |
| `/api/requests/{id}` | GET | Requester, FacilityManager | **Ownership:** Requester truy cập ID không phải của mình bị 403 Forbidden; FM thấy tất cả | Backend | `SmartMaintenance.Api/Controllers/RequestsController.cs:48-49` & `RequestService.cs:91-93` |
| `/api/requests/{id}/status` | PATCH | FacilityManager | Quản lý chuyển đổi trạng thái Request; BR-18 bắt buộc WO Completed trước khi Closed | Backend | `SmartMaintenance.Api/Controllers/RequestsController.cs:62-63` & `RequestService.cs:121-126` |
| `/api/requests/{id}/history` | GET | FacilityManager | Xem lịch sử nghiệm thu bảo trì của Request | Backend | `SmartMaintenance.Api/Controllers/RequestsController.cs:71-72` |
| `/api/work-orders` | POST | FacilityManager | Tạo WO từ Request (Request phải là Submitted, có AssetId, chưa có WO theo BR-06) | Backend | `SmartMaintenance.Api/Controllers/WorkOrdersController.cs:21-22` & `WorkOrderService.cs:41-71` |
| `/api/work-orders` | GET | FacilityManager, Technician | **Ownership:** Technician chỉ thấy WO được phân công cho mình; FM thấy tất cả | Backend | `SmartMaintenance.Api/Controllers/WorkOrdersController.cs:34-35` & `WorkOrderService.cs:100-103` |
| `/api/work-orders/{id}` | GET | FacilityManager, Technician | **Ownership:** Technician xem WO của người khác bị 403 Forbidden (BR-07); FM xem tất cả | Backend | `SmartMaintenance.Api/Controllers/WorkOrdersController.cs:48-49` & `WorkOrderService.cs:327-332` |
| `/api/work-orders/{id}` | PATCH | FacilityManager, Technician | **Ownership & Action Matrix:** Xem chi tiết ma trận tại Mục 4.2 và 4.3 | Backend | `SmartMaintenance.Api/Controllers/WorkOrdersController.cs:62-63` & `WorkOrderService.cs:158-271` |
| `/api/iot/ingest` | POST | Gateway (API Key) | Xác thực qua header `X-Api-Key`; Sensor phải được mapping với Asset (422 nếu unmapped) | Backend | `SmartMaintenance.Api/Controllers/IotController.cs:20-23` & `IotGatewayAuthorizeAttribute.cs:6-21` |
| `/api/iot-alerts` | GET | FacilityManager, Technician | Xem danh sách cảnh báo bất thường IoT | Backend | `SmartMaintenance.Api/Controllers/IotAlertsController.cs:20-21` |
| `/api/iot-mappings` | POST | Admin | Tạo mapping Sensor - Asset (Mỗi Asset tối đa 1 Sensor theo BR-13) | Backend | `SmartMaintenance.Api/Controllers/IotMappingsController.cs:20-21` |
| `/api/iot-mappings` | GET | Admin | Xem danh sách mapping giữa Sensor và Asset | Backend | `SmartMaintenance.Api/Controllers/IotMappingsController.cs:31-32` |
| `/api/iot-mappings/{id}` | PUT | Admin | Cập nhật cấu hình DeviceId cho mapping | Backend | `SmartMaintenance.Api/Controllers/IotMappingsController.cs:40-41` |
| `/api/predictions` | GET | FacilityManager | Xem tổng quan danh sách rủi ro AI của toàn bộ thiết bị | Backend | `SmartMaintenance.Api/Controllers/PredictionsController.cs:20-21` |

---

## 4. Quy tắc Ownership (Quyền sở hữu dữ liệu)

Các quy tắc phân quyền sở hữu dữ liệu được thực thi trực tiếp tại tầng nghiệp vụ (`Application Layer`), đảm bảo nguyên tắc: *người dùng chỉ được tiếp cận và thao tác trên dữ liệu thuộc phạm vi thẩm quyền của mình*, kể cả khi họ cố tình giả mạo tham số gọi API.

### 4.1. Quy tắc sở hữu của Requester (Báo cáo sự cố)
Thực thi tại `src/backend/SmartMaintenance.Application/Requests/RequestService.cs`:
1. **Chống giả mạo người tạo (Identity Binding):**
   - Khi tạo yêu cầu bảo trì (`CreateAsync`), Controller tự động trích xuất `requesterId` từ claim định danh người dùng trong JWT token (`ClaimTypes.NameIdentifier` hoặc `sub`).
   - Requester không thể chỉ định `requesterId` trong payload JSON, ngăn chặn hoàn toàn việc tạo yêu cầu mạo danh người khác.  
   *Vị trí mã nguồn:* `RequestsController.cs:26-30` và `RequestService.cs:52`.
2. **Lọc danh sách theo người tạo (List Isolation):**
   - Trong phương thức `ListAsync(userId, role)`:
     ```csharp
     if (string.Equals(role, UserRoles.Requester, StringComparison.Ordinal))
         query = query.Where(r => r.RequesterId == userId);
     ```
   - Requester gọi `GET /api/requests` chỉ nhận được danh sách các bản ghi do chính mình tạo. FacilityManager không bị lọc điều kiện này và xem được toàn bộ danh sách.  
   *Vị trí mã nguồn:* `RequestService.cs:75-77`.
3. **Chặn truy cập chi tiết trái phép (Read Authorization):**
   - Trong phương thức `GetByIdAsync(id, userId, role)`:
     ```csharp
     if (string.Equals(role, UserRoles.Requester, StringComparison.Ordinal) && entity.RequesterId != userId)
         throw new AppException("You do not have access to this request.", 403);
     ```
   - Nếu Requester cố tình gửi yêu cầu `GET /api/requests/{id}` với `id` của một yêu cầu thuộc về người khác, hệ thống lập tức ném ngoại lệ trả về mã `403 Forbidden`.  
   *Vị trí mã nguồn:* `RequestService.cs:91-93`.

### 4.2. Quy tắc sở hữu của Technician (Phiếu công việc)
Thực thi tại `src/backend/SmartMaintenance.Application/WorkOrders/WorkOrderService.cs`:
1. **Lọc danh sách phiếu phân công (Assigned Work Order Isolation):**
   - Trong phương thức `ListAsync(userId, role)`:
     ```csharp
     if (string.Equals(role, UserRoles.Technician, StringComparison.Ordinal))
         query = query.Where(w => w.TechnicianId == userId);
     ```
   - Technician gọi `GET /api/work-orders` chỉ nhận được các phiếu có `TechnicianId` trùng với mã `userId` của mình.  
   *Vị trí mã nguồn:* `WorkOrderService.cs:101-102`.
2. **Chặn xem chi tiết phiếu của người khác (Enforce BR-07):**
   - Trong phương thức `GetByIdAsync`: Hàm gọi kiểm tra `EnsureTechnicianAccess(workOrder, userId, role)`:
     ```csharp
     private static void EnsureTechnicianAccess(WorkOrder workOrder, int userId, string role)
     {
         if (string.Equals(role, UserRoles.Technician, StringComparison.Ordinal)
             && workOrder.TechnicianId != userId)
             throw new AppException("Technician is not assigned to this Work Order (BR-07).", 403);
     }
     ```
   - Technician cố tình đọc phiếu của đồng nghiệp khác lập tức nhận mã `403 Forbidden`.  
   *Vị trí mã nguồn:* `WorkOrderService.cs:116, 327-332`.
3. **Ma trận kiểm soát cập nhật PATCH Work Order (Action & State Matrix):**
   Thực thi chi tiết trong phương thức `PatchAsync(id, payload, userId, role)` (`WorkOrderService.cs:158-271`):
   - **Quy tắc trạng thái đóng (Terminal Status):** Nếu Work Order đang ở trạng thái `Completed` hoặc `Cancelled`, hệ thống chặn mọi thao tác cập nhật và trả về `400 Bad Request` (`"Work Order status '...' is terminal and cannot be changed."`, dòng 175-177).
   - **Xác thực Role:** Chỉ cho phép `FacilityManager` và `Technician`. Các Role khác nhận ngay `403 Forbidden` (`"Forbidden."`, dòng 169-170).
   - **Quy tắc phân quyền chi tiết cho Technician (`isTech`):**
     - *Kiểm tra sở hữu:* Bắt buộc gọi `EnsureTechnicianAccess(workOrder, userId, role)` (dòng 172-173); nếu phiếu không phân công cho mình, trả về `403 Forbidden`.
     - *Chặn tự chuyển việc (Reassign Prevention):* Nếu payload chứa `technicianId`, hệ thống lập tức ném ngoại lệ `403 Forbidden` (`"Technician cannot reassign Work Orders."`, dòng 219-220).
     - *Từ chối phiếu công việc (`rejectionReason`):* Technician chỉ được từ chối khi trạng thái hiện tại là `Assigned` hoặc `InProgress` (dòng 224-226, sai trả 400). Khi từ chối, phiếu chuyển sang `Cancelled`, lưu `RejectionReason`, và đồng bộ trạng thái sang Maintenance Request (`SyncRequestFromWorkOrder`, dòng 228-230).
     - *Chuyển sang `InProgress`:* Chỉ hợp lệ khi trạng thái hiện tại là `Assigned` (dòng 237-238, sai trả 400); cập nhật trạng thái phiếu và đồng bộ Request (dòng 239-240).
     - *Chuyển sang `Completed`:* Chỉ hợp lệ khi trạng thái hiện tại là `Assigned` hoặc `InProgress` (dòng 244-246, sai trả 400). **Bắt buộc phải kèm kết quả xử lý (`hasResult`) theo quy tắc `BR-09`**; nếu thiếu `result`, trả về `400 Bad Request` (`"result is required when completing a Work Order (BR-09)."`, dòng 248-249). Khi hoàn thành, phiếu chuyển `Completed`, tự động ghi bản ghi mới vào bảng `MaintenanceHistory` (chứa `WorkOrderId`, `AssetId`, `Result`, `CompletedAt`), và đồng bộ Request (dòng 252-260).
     - *Cấm chuyển trạng thái khác:* Nếu truyền trạng thái khác `InProgress`/`Completed`, trả về `400 Bad Request` (`"Technician may only set status to In Progress or Completed."`, dòng 264).
     - *Không truyền trường hợp lệ:* Nếu không có `rejectionReason` và không có `status`, trả về `400 Bad Request` (dòng 268-269).

### 4.3. Giới hạn quyền hạn của Facility Manager
- Facility Manager có quyền xem toàn bộ danh mục tài sản, yêu cầu bảo trì và phiếu công việc trên toàn trường.
- **Giới hạn can thiệp kỹ thuật trong PATCH Work Order (`WorkOrderService.cs:184-216`):**
  - **Cấm nhập kết quả kỹ thuật và lý do từ chối:** Facility Manager **bị cấm tuyệt đối** việc truyền `result` hoặc `rejectionReason` thay cho Technician; vi phạm trả về ngay `403 Forbidden` (`"FacilityManager cannot set rejectionReason or result."`, dòng 186-187).
  - **Phân công lại kỹ thuật viên (`technicianId`):** Chỉ được phép phân công lại khi Work Order đang ở trạng thái `Assigned` (nếu đang `InProgress` trả `400 Bad Request`, dòng 191-192). Kỹ thuật viên mới phải là người dùng đang hoạt động (`IsActive = true`) và có đúng vai trò `Technician` (dòng 198-200).
  - **Hủy phiếu (`status`):** Facility Manager chỉ được phép đổi trạng thái sang duy nhất `Cancelled` (nếu truyền trạng thái khác trả `400 Bad Request`, dòng 208-209). Trạng thái sau đó được đồng bộ sang Request (dòng 211).
  - **Không truyền trường hợp lệ:** Nếu không có `technicianId` và không có `status`, trả về `400 Bad Request` (dòng 214-215).

### 4.4. Giới hạn bất biến của tài khoản Admin
Thực thi tại `src/backend/SmartMaintenance.Application/Users/UserService.cs`:
1. **Bảo vệ tài khoản Admin khỏi chỉnh sửa (QT-4):**
   - `PUT /api/users/{id}` kiểm tra: `if (user.Role == UserRoles.Admin) throw new AppException("Admin accounts cannot be modified via this endpoint.", 403);`.
   - Ngăn chặn việc Admin tự tước quyền, tự khóa tài khoản hoặc chỉnh sửa tài khoản Admin khác qua API thông thường.  
   *Vị trí mã nguồn:* `UserService.cs:100-101`.
2. **Chống leo thang đặc quyền (Privilege Escalation Prevention - QT-3 & QT-2):**
   - Không được nâng cấp bất kỳ tài khoản nào lên Admin qua PUT: `if (newRole == UserRoles.Admin) throw new AppException("Cannot assign Admin role via this endpoint...", 400);`.  
     *Vị trí mã nguồn:* `UserService.cs:111-116`.
   - Không được đổi vai trò của tài khoản Requester: `if (user.Role == UserRoles.Requester) throw new AppException("Cannot change role of a Requester account.", 400);`.  
     *Vị trí mã nguồn:* `UserService.cs:119-120`.
   - Điểm cuối PUT chỉ cho phép luân chuyển vai trò giữa Technician và FacilityManager: `if (!RotatableRoles.Contains(user.Role) || !RotatableRoles.Contains(newRole)) throw new AppException("This endpoint only allows rotating roles between Technician and FacilityManager.", 400);`.  
     *Vị trí mã nguồn:* `UserService.cs:126-131`.

---

## 5. Acceptance Criteria (Tiêu chí Chấp nhận)

### 5.1. Nhóm Xác thực (AC-AUTH-xx)

#### AC-AUTH-01: Đăng nhập thành công với tài khoản hợp lệ
- **Given:** Người dùng có tài khoản hợp lệ trong hệ thống với trạng thái `IsActive = true`.
- **When:** Người dùng gửi `POST /api/auth/login` với `username` và `password` chính xác.
- **Then:** Hệ thống trả về mã `200 OK`, body chứa JWT `token`, `role`, `userId`, `username`. Token có thời hạn 480 phút và chứa đầy đủ các claims chuẩn.
- **Nguồn:** `REQ-01`, `NFR-01`, `NFR-02`, `DEC-06`.
- **Test tự động:** **THIẾU TEST (login chỉ được dùng gián tiếp trong helper của các test khác)**.

#### AC-AUTH-02: Đăng nhập thất bại do sai thông tin xác thực
- **Given:** Người dùng chưa đăng nhập.
- **When:** Người dùng gửi `POST /api/auth/login` với `username` không tồn tại, hoặc `password` không khớp, hoặc tài khoản có `IsActive = false`.
- **Then:** Hệ thống từ chối đăng nhập và trả về mã `401 Unauthorized` kèm `{ "error": "Invalid username or password." }`.
- **Nguồn:** `REQ-01`, `NFR-02`.
- **Test tự động:** **THIẾU TEST** (Trong test suite backend chưa có test case gọi riêng `POST /api/auth/login` kiểm tra 401 khi sai password).

#### AC-AUTH-03: Đăng xuất và đưa token vào danh sách đen (Blacklist)
- **Given:** Người dùng đã đăng nhập và sở hữu một JWT token hợp lệ.
- **When:** Người dùng gửi `POST /api/auth/logout` kèm header `Authorization: Bearer <token>`.
- **Then:** Hệ thống trả về mã `200 OK` `{ "message": "Logged out." }`. Token được lưu vào `TokenBlacklist` cho đến khi hết hạn; mọi request tiếp theo dùng token này sẽ bị từ chối với mã `401 Unauthorized`.
- **Nguồn:** `NFR-02`, `SEC-04`.
- **Test tự động:** **THIẾU TEST** (Chưa có test tự động cho endpoint `/api/auth/logout` và xác minh blacklist trong `SmartMaintenance.Tests`).

#### AC-AUTH-04: Từ chối truy cập khi không có Token xác thực
- **Given:** Người dùng hoặc client ẩn danh không truyền header `Authorization`.
- **When:** Client gửi request tới bất kỳ endpoint nào yêu cầu bảo vệ (ví dụ: `POST /api/assets`, `POST /api/requests`, `POST /api/work-orders`).
- **Then:** Hệ thống chặn request và phản hồi mã `401 Unauthorized`.
- **Nguồn:** `NFR-01`, `NFR-02`, `SEC-01`.
- **Test tự động:** 
  - `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_Anonymous_Returns401`
  - `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_Anonymous_Returns401`
  - `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Anonymous_Returns401`

#### AC-AUTH-05: IoT Gateway gửi dữ liệu với API Key hợp lệ
- **Given:** Cấu hình gateway có API Key chính xác khớp với `Iot:GatewayApiKey`.
- **When:** Gateway gửi `POST /api/iot/ingest` kèm header `X-Api-Key: <configured_gateway_key>` với dữ liệu của sensor đã mapping.
- **Then:** Hệ thống xác thực thành công và lưu dữ liệu, phản hồi mã `201 Created`.
- **Nguồn:** `REQ-28`, `NFR-02`, `DEC-04`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_MappedDevice_Returns201_AndPersists`

#### AC-AUTH-06: IoT Gateway gửi dữ liệu với API Key sai hoặc bị thiếu
- **Given:** Client không truyền header `X-Api-Key` hoặc truyền sai API Key.
- **When:** Client gửi `POST /api/iot/ingest`.
- **Then:** Hệ thống chặn request tại filter `IotGatewayAuthorize` và trả về mã `401 Unauthorized` kèm `{ "error": "Invalid or missing gateway API key." }`.
- **Nguồn:** `NFR-02`, `SEC-05`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_BadApiKey_Returns401` (Lưu ý: Test case hiện tại chỉ kiểm tra key sai, chưa có test case tự động riêng biệt cho trường hợp thiếu hoàn toàn header `X-Api-Key`).

---

### 5.2. Nhóm Phân quyền (AC-AUTHZ-xx)

#### AC-AUTHZ-01: Requester tạo yêu cầu bảo trì thành công
- **Given:** Người dùng đã đăng nhập với vai trò `Requester`.
- **When:** Gửi `POST /api/requests` với `AssetId` hợp lệ và `Description` hợp lệ.
- **Then:** Hệ thống tạo bản ghi với trạng thái `Submitted`, gán `RequesterId` lấy từ JWT token, trả về mã `201 Created`.
- **Nguồn:** `REQ-04`, `NFR-01`.
- **Test tự động:** 
  - `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_ValidRequester_Returns201_Submitted`
  - `SmartMaintenance.Tests.Requests.RequestServiceTests.Create_Valid_PersistsSubmitted_WithJwtRequesterId`

#### AC-AUTHZ-02: Vai trò không phải Requester cố tạo yêu cầu bảo trì
- **Given:** Người dùng đăng nhập với vai trò `FacilityManager` (hoặc `Technician`, `Admin`).
- **When:** Gửi `POST /api/requests`.
- **Then:** Hệ thống chặn request và trả về mã lỗi `403 Forbidden`.
- **Nguồn:** `REQ-04`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_FacilityManager_Returns403`

#### AC-AUTHZ-03: Requester chỉ xem được danh sách yêu cầu của chính mình (Ownership List)
- **Given:** Có nhiều yêu cầu sự cố trong hệ thống do các Requester khác nhau tạo.
- **When:** Requester A gửi `GET /api/requests`.
- **Then:** Hệ thống chỉ trả về danh sách các yêu cầu có `RequesterId == A.UserId`.
- **Nguồn:** `REQ-05`, `NFR-01`.
- **Test tự động:** **THIẾU TEST** (Chưa có test case tự động kiểm tra logic lọc danh sách theo RequesterId trong `SmartMaintenance.Tests`).

#### AC-AUTHZ-04: Requester xem chi tiết yêu cầu của người khác bị từ chối (Ownership Detail 403)
- **Given:** Yêu cầu bảo trì ID = 10 được tạo bởi Requester B.
- **When:** Requester A (với `UserId != B.UserId`) gửi `GET /api/requests/10`.
- **Then:** Hệ thống ném ngoại lệ và trả về mã `403 Forbidden` với thông báo `"You do not have access to this request."`.
- **Nguồn:** `REQ-05`, `NFR-01`.
- **Test tự động:** **THIẾU TEST** (Chưa có test tự động cho logic ném 403 trong `RequestService.GetByIdAsync`).

#### AC-AUTHZ-05: FacilityManager tạo Work Order thành công
- **Given:** Người dùng đăng nhập với vai trò `FacilityManager`, yêu cầu bảo trì đang ở trạng thái `Submitted` và đã gắn `AssetId`.
- **When:** Gửi `POST /api/work-orders` với `requestId`, `technicianId`, `assetId`.
- **Then:** Hệ thống tạo Work Order trạng thái `Assigned`, đồng bộ Request sang `Pending`, trả về mã `201 Created`.
- **Nguồn:** `REQ-14`, `REQ-15`, `BR-06`, `BR-08`, `NFR-01`.
- **Test tự động:** 
  - `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_ValidManager_Returns201_Assigned`
  - `SmartMaintenance.Tests.WorkOrders.WorkOrderServiceTests.Create_Valid_PersistsAssigned`

#### AC-AUTHZ-06: Requester cố tình tạo Work Order
- **Given:** Người dùng đăng nhập với vai trò `Requester`.
- **When:** Gửi `POST /api/work-orders`.
- **Then:** Hệ thống chặn quyền và phản hồi mã `403 Forbidden`.
- **Nguồn:** `REQ-14`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Requester_Returns403`

#### AC-AUTHZ-07: Technician xem chi tiết Work Order được phân công (200 OK)
- **Given:** Người dùng đăng nhập với vai trò `Technician` và được giao Work Order ID = 5.
- **When:** Gửi `GET /api/work-orders/5`.
- **Then:** Hệ thống trả về mã `200 OK` kèm thông tin chi tiết của Work Order, Asset và MaintenanceHistory.
- **Nguồn:** `REQ-17`, `BR-07`, `NFR-01`.
- **Test tự động:** **THIẾU TEST** (Chưa có test case gọi `GET /api/work-orders/{id}` thành công cho Technician).

#### AC-AUTHZ-08: Technician xem hoặc cập nhật Work Order của người khác (403 Forbidden)
- **Given:** Work Order ID = 5 được phân công cho `tech1`. Người dùng đăng nhập là `tech2`.
- **When:** `tech2` gửi `GET /api/work-orders/5` hoặc `PATCH /api/work-orders/5`.
- **Then:** Hệ thống chặn thao tác và phản hồi `403 Forbidden` (`"Technician is not assigned to this Work Order (BR-07)."`).
- **Nguồn:** `REQ-17`, `REQ-20`, `BR-07`, `NFR-01`.
- **Test tự động:** **THIẾU TEST** (Chưa có test tự động kiểm tra `EnsureTechnicianAccess` trả về 403).

#### AC-AUTHZ-09: Technician cố tình phân công lại Work Order cho người khác
- **Given:** `tech1` được giao Work Order ID = 5.
- **When:** `tech1` gửi `PATCH /api/work-orders/5` kèm `technicianId: 2`.
- **Then:** Hệ thống từ chối và trả về mã `403 Forbidden` (`"Technician cannot reassign Work Orders."`).
- **Nguồn:** `BR-07`, `NFR-01`.
- **Test tự động:** **THIẾU TEST**.

#### AC-AUTHZ-10: FacilityManager cố tình ghi nhận kết quả bảo trì hoặc lý do từ chối
- **Given:** Người dùng đăng nhập với vai trò `FacilityManager`.
- **When:** Gửi `PATCH /api/work-orders/5` kèm `result` hoặc `rejectionReason`.
- **Then:** Hệ thống từ chối và phản hồi `403 Forbidden` (`"FacilityManager cannot set rejectionReason or result."`).
- **Nguồn:** `REQ-21`, `BR-07`, `NFR-01`.
- **Test tự động:** **THIẾU TEST**.

#### AC-AUTHZ-11: FacilityManager tạo tài sản mới thành công
- **Given:** Người dùng đăng nhập với vai trò `FacilityManager`.
- **When:** Gửi `POST /api/assets` với thông tin tài sản hợp lệ.
- **Then:** Hệ thống tạo tài sản mới và trả về mã `201 Created`.
- **Nguồn:** `REQ-06`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_ValidFacilityManager_Returns201_AndPersists`

#### AC-AUTHZ-12: Requester cố tình tạo tài sản mới
- **Given:** Người dùng đăng nhập với vai trò `Requester`.
- **When:** Gửi `POST /api/assets`.
- **Then:** Hệ thống từ chối và phản hồi mã `403 Forbidden`.
- **Nguồn:** `REQ-06`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_RequesterToken_Returns403`

#### AC-AUTHZ-13: Requester cố tình xem dữ liệu IoT của tài sản
- **Given:** Người dùng đăng nhập với vai trò `Requester`.
- **When:** Gửi `GET /api/assets/{id}/iot-data`.
- **Then:** Hệ thống từ chối và trả về mã `403 Forbidden`.
- **Nguồn:** `REQ-09`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.IotApiTests.GetIotData_Requester_Returns403`

#### AC-AUTHZ-14: Requester cố tình xem kết quả dự báo AI của tài sản
- **Given:** Người dùng đăng nhập với vai trò `Requester`.
- **When:** Gửi `GET /api/assets/{id}/prediction`.
- **Then:** Hệ thống từ chối và trả về mã `403 Forbidden`.
- **Nguồn:** `REQ-11`, `REQ-19`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Requester_Returns403`

#### AC-AUTHZ-15: Technician xem kết quả dự báo AI của tài sản (Được phép)
- **Given:** Người dùng đăng nhập với vai trò `Technician`.
- **When:** Gửi `GET /api/assets/{id}/prediction`.
- **Then:** Hệ thống cho phép truy cập và trả về mã `200 OK` kèm mức độ rủi ro `Risk`.
- **Nguồn:** `REQ-19`, `NFR-01`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Technician_Allowed`

#### AC-AUTHZ-16: Chặn sửa đổi hoặc vô hiệu hóa tài khoản Admin (QT-4)
- **Given:** Người dùng là Admin. Đối tượng chỉnh sửa trong bảng `Users` có vai trò là `Admin`.
- **When:** Gửi `PUT /api/users/{id}` để đổi Role hoặc đặt `IsActive = false`.
- **Then:** Hệ thống từ chối thao tác và ném ngoại lệ `403 Forbidden` (`"Admin accounts cannot be modified via this endpoint."`).
- **Nguồn:** `REQ-24`, `NFR-01`, `SEC-06`.
- **Test tự động:** 
  - `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminTarget_Throws403_QT4`
  - `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminSelfDeactivate_Throws403_QT4`

#### AC-AUTHZ-17: Chặn leo thang quyền gán vai trò Admin qua cập nhật (QT-3)
- **Given:** Người dùng là Admin.
- **When:** Gửi `PUT /api/users/{id}` với payload `{ "role": "Admin" }`.
- **Then:** Hệ thống từ chối và phản hồi mã `400 Bad Request` (`"Cannot assign Admin role via this endpoint. Use POST /api/users to create an Admin account."`).
- **Nguồn:** `REQ-24`, `NFR-01`, `SEC-06`.
- **Test tự động:** `SmartMaintenance.Tests.Users.UserServiceTests.Update_AssignAdmin_Throws400_QT3`

#### AC-AUTHZ-18: Chặn thay đổi vai trò của tài khoản Requester (QT-2)
- **Given:** Tài khoản mục tiêu có vai trò hiện tại là `Requester`.
- **When:** Admin gửi `PUT /api/users/{id}` với vai trò mới.
- **Then:** Hệ thống từ chối và phản hồi mã `400 Bad Request` (`"Cannot change role of a Requester account."`).
- **Nguồn:** `REQ-24`, `NFR-01`, `SEC-06`.
- **Test tự động:** `SmartMaintenance.Tests.Users.UserServiceTests.Update_Requester_Throws400_QT2`

#### AC-AUTHZ-19: Xoay vòng vai trò hợp lệ giữa Technician và FacilityManager (QT-1)
- **Given:** Tài khoản mục tiêu có vai trò là `Technician` (hoặc `FacilityManager`).
- **When:** Admin gửi `PUT /api/users/{id}` để đổi sang `FacilityManager` (hoặc `Technician`).
- **Then:** Hệ thống cập nhật thành công và trả về mã `200 OK` phản ánh vai trò mới.
- **Nguồn:** `REQ-24`, `REQ-25`, `NFR-01`.
- **Test tự động:** 
  - `SmartMaintenance.Tests.Users.UserServiceTests.Update_TechnicianToFacilityManager_Ok_QT1`
  - `SmartMaintenance.Tests.Users.UserServiceTests.Update_FacilityManagerToTechnician_Ok_QT1`

#### AC-AUTHZ-20: Admin xem ma trận quyền cố định của hệ thống
- **Given:** Người dùng đăng nhập với vai trò `Admin`.
- **When:** Gửi `GET /api/roles/permissions`.
- **Then:** Hệ thống trả về mã `200 OK` chứa mapping quyền tĩnh 4 Role. Nếu gọi bởi Non-Admin, hệ thống phản hồi `403 Forbidden`.
- **Nguồn:** `REQ-25`, `NFR-01`.
- **Test tự động:** **THIẾU TEST** (Chưa có test tự động cho `RolesController`).

---

## 6. Bảng truy vết và Bằng chứng kiểm thử (Traceability & Evidence Matrix)

Bảng dưới đây chuẩn hóa ánh xạ toàn diện từ Tiêu chí chấp nhận (AC) → Yêu cầu/Quy tắc nguồn → Kỳ vọng (Expected) → Thực tế mã nguồn (Actual) → Bằng chứng kiểm thử (Evidence) → Trạng thái xác minh (Status) → Khoảng cách cần theo dõi (Gap/Follow-up).

*Quy ước trạng thái:*
- **PASS:** Đã có test tự động trong test suite và đang chạy thành công.
- **PARTIALLY VERIFIED:** Đã có test một phần hoặc chỉ kiểm thử ở tầng Unit/Service, chưa có test mức API Controller.
- **NOT TESTED:** Chưa có test tự động trong hệ thống.
- **PENDING DECISION:** Điểm lệch yêu cầu nghiệp vụ đang chờ BA/PO phê duyệt.

| AC ID | Requirement / Rule | Expected Behavior | Actual Behavior | Test / Evidence | Status | Gap / Follow-up |
|---|---|---|---|---|---|---|
| **AC-AUTH-01** | REQ-01, NFR-01, DEC-06 | Đăng nhập đúng thông tin nhận token 200 OK | Backend cấp JWT token 480 phút | Chưa có test tự động (chỉ dùng gián tiếp trong helper) | **NOT TESTED** | Cần bổ sung test case `[Fact]` độc lập cho login thành công |
| **AC-AUTH-02** | REQ-01, NFR-02 | Đăng nhập sai trả 401 Unauthorized | Backend trả 401 khi sai credential | Chưa có test tự động | **NOT TESTED** | Cần viết test tự động kiểm tra 401 khi sai mật khẩu |
| **AC-AUTH-03** | NFR-02, SEC-04 | Đăng xuất đưa token vào blacklist | Backend lưu jti vào RAM TokenBlacklist | Kiểm tra thủ công (SEC-04 pass) | **NOT TESTED** | Cần test tự động gọi `/api/auth/logout` và xác minh token bị từ chối |
| **AC-AUTH-04** | NFR-01, NFR-02, SEC-01 | Request ẩn danh bị chặn 401 | Backend trả 401 khi thiếu Bearer token | `PostAssets_Anonymous_Returns401`, `PostRequest_Anonymous_Returns401`, `PostWorkOrder_Anonymous_Returns401` | **PASS** | Đạt yêu cầu bảo mật ở các endpoint nghiệp vụ chính |
| **AC-AUTH-05** | REQ-28, NFR-02, DEC-04 | Gateway gửi API Key đúng nhận 201 | Backend xác thực API Key và lưu telemetry | `Ingest_MappedDevice_Returns201_AndPersists` | **PASS** | Đạt yêu cầu xác thực API Key |
| **AC-AUTH-06** | NFR-02, SEC-05 | Gateway gửi key sai/thiếu bị 401 | Filter chặn và trả 401 | `Ingest_BadApiKey_Returns401` | **PARTIALLY VERIFIED** | Đã test key sai; cần bổ sung test case cho trường hợp thiếu header |
| **AC-AUTHZ-01** | REQ-04, NFR-01 | Requester tạo Request nhận 201 Submitted | Backend bind requesterId từ token và lưu | `PostRequest_ValidRequester_Returns201_Submitted`, `Create_Valid_PersistsSubmitted_WithJwtRequesterId` | **PASS** | Đạt yêu cầu ở cả tầng API và Service |
| **AC-AUTHZ-02** | REQ-04, NFR-01 | Non-Requester tạo Request bị 403 | Backend chặn vai trò khác tạo request | `PostRequest_FacilityManager_Returns403` | **PASS** | Đạt yêu cầu phân quyền Role |
| **AC-AUTHZ-03** | REQ-05, NFR-01 | Requester chỉ thấy danh sách của mình | Backend lọc `r.RequesterId == userId` | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test tự động kiểm tra cách ly danh sách (Isolation List) |
| **AC-AUTHZ-04** | REQ-05, NFR-01 | Requester xem chi tiết của người khác bị 403 | Backend ném 403 trong `GetByIdAsync` | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test tự động kiểm tra 403 khi truy cập ID của người khác |
| **AC-AUTHZ-05** | REQ-14, REQ-15, BR-06, BR-08 | FM tạo Work Order nhận 201 Assigned | Backend tạo WO và sync Request sang Pending | `PostWorkOrder_ValidManager_Returns201_Assigned`, `Create_Valid_PersistsAssigned` | **PASS** | Đạt yêu cầu ở cả tầng API và Service |
| **AC-AUTHZ-06** | REQ-14, NFR-01 | Requester cố tạo WO bị 403 | Backend chặn Requester gọi POST WO | `PostWorkOrder_Requester_Returns403` | **PASS** | Đạt yêu cầu phân quyền Role |
| **AC-AUTHZ-07** | REQ-17, BR-07, NFR-01 | Tech xem chi tiết WO của mình (200 OK) | Backend trả chi tiết kèm Asset và History | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test case GET WO detail cho Technician |
| **AC-AUTHZ-08** | REQ-17, REQ-20, BR-07 | Tech xem/sửa WO người khác bị 403 | `EnsureTechnicianAccess` chặn ném 403 | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test tự động kiểm tra 403 khi Tech truy cập WO người khác |
| **AC-AUTHZ-09** | BR-07, NFR-01 | Tech cố reassign WO bị 403 | Backend chặn ném 403 trong PatchAsync | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test case Tech truyền `technicianId` bị chặn 403 |
| **AC-AUTHZ-10** | REQ-21, BR-07, NFR-01 | FM cố nhập result/rejection bị 403 | Backend chặn ném 403 trong PatchAsync | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test case FM truyền result/rejection bị chặn 403 |
| **AC-AUTHZ-11** | REQ-06, NFR-01 | FM tạo Asset nhận 201 Created | Backend tạo Asset thành công | `PostAssets_ValidFacilityManager_Returns201_AndPersists` | **PASS** | Đạt yêu cầu phân quyền tạo Asset |
| **AC-AUTHZ-12** | REQ-06, NFR-01 | Requester tạo Asset bị 403 | Backend chặn Requester tạo Asset | `PostAssets_RequesterToken_Returns403` | **PASS** | Đạt yêu cầu phân quyền Role |
| **AC-AUTHZ-13** | REQ-09, NFR-01 | Requester xem IoT Data bị 403 | Backend chặn Requester đọc telemetry | `GetIotData_Requester_Returns403` | **PASS** | Đạt yêu cầu bảo vệ dữ liệu IoT |
| **AC-AUTHZ-14** | REQ-11, REQ-19, NFR-01 | Requester xem AI Prediction bị 403 | Backend chặn Requester đọc dự báo AI | `GetPrediction_Requester_Returns403` | **PASS** | Đạt yêu cầu bảo vệ dự báo AI |
| **AC-AUTHZ-15** | REQ-19, NFR-01 | Tech xem AI Prediction được phép 200 OK | Backend cho phép Tech đọc dự báo AI | `GetPrediction_Technician_Allowed` | **PASS** | Đạt yêu cầu chia sẻ dự báo cho Tech |
| **AC-AUTHZ-16** | REQ-24, SEC-06 | Cấm sửa/vô hiệu hóa tài khoản Admin | Service ném 403 khi target là Admin | `Update_AdminTarget_Throws403_QT4`, `Update_AdminSelfDeactivate_Throws403_QT4` | **PARTIALLY VERIFIED** | Đạt ở tầng Service (QT-4); chưa có integration test ở mức API |
| **AC-AUTHZ-17** | REQ-24, SEC-06 | Cấm nâng cấp tài khoản lên Admin qua PUT | Service ném 400 khi gán Role Admin | `Update_AssignAdmin_Throws400_QT3` | **PARTIALLY VERIFIED** | Đạt ở tầng Service (QT-3); chưa có integration test ở mức API |
| **AC-AUTHZ-18** | REQ-24, SEC-06 | Cấm đổi vai trò tài khoản Requester | Service ném 400 khi target là Requester | `Update_Requester_Throws400_QT2` | **PARTIALLY VERIFIED** | Đạt ở tầng Service (QT-2); chưa có integration test ở mức API |
| **AC-AUTHZ-19** | REQ-24, REQ-25 | Xoay vai trò Tech ↔ FM hợp lệ | Service cho phép xoay vòng vai trò | `Update_TechnicianToFacilityManager_Ok_QT1`, `Update_FacilityManagerToTechnician_Ok_QT1` | **PARTIALLY VERIFIED** | Đạt ở tầng Service (QT-1); chưa có integration test ở mức API |
| **AC-AUTHZ-20** | REQ-25, NFR-01 | Admin xem ma trận quyền cố định | Backend trả ma trận quyền tĩnh | Chưa có test tự động | **NOT TESTED** | Cần bổ sung test tự động cho RolesController |

---

## 7. Bảng Open Findings / Decision Required (Điểm lệch cần BA & PO chốt)

Bảng dưới đây tổng hợp toàn bộ các điểm lệch quan trọng, mâu thuẫn giữa tài liệu nghiệp vụ, đặc tả kỹ thuật và mã nguồn thực tế:

| Finding ID | Mô tả tóm tắt | Nguồn tham chiếu (REQ / AC / Spec) | Hành vi thực tế (As-built Behavior) | Mức độ tác động (Impact) | Đề xuất xử lý của BA (Proposed Action) | Người ra quyết định (Decision Owner) | Trạng thái (Status) | Bằng chứng (Evidence) |
|---|---|---|---|---|---|---|---|---|
| **FIND-01** | Mâu thuẫn AC sửa quyền Role với RBAC tĩnh | `acceptance-criteria.md:137` (`AC-US-01-04-02`) vs `US-01-04-Spec.md:69` (`CON-03`) | `RolesController.cs:11-24` chỉ có API đọc tĩnh `GET /api/roles/permissions`, không có API lưu/sửa quyền | High (Mâu thuẫn tài liệu nghiệm thu) | Giữ nguyên AC ID; BA đề xuất cập nhật `AC-US-01-04-02` thành "Xem ma trận quyền tĩnh", giữ đúng RBAC tĩnh cho MVP | BA Lead / Product Owner | **PENDING DECISION** | `RolesController.cs:11-24`, `US-01-04-Spec.md:69` |
| **FIND-02** | Bảng quyền tĩnh RolesController không khớp API Backend | `RolesController.cs:12-18` vs `AssetsController.cs` | Bảng tĩnh thiếu quyền `ViewAsset`, `ViewIoTData`, `ViewPrediction` của Tech; thiếu `ViewAsset` của Admin | Medium (Hiển thị sai danh mục quyền trên UI) | Cập nhật mảng `PermissionsByRole` trong Backend khớp với danh sách API Controller thực tế | Technical Lead / BA | **OPEN** | `RolesController.cs:12-18`, `AssetsController.cs:44,53,81,90` |
| **FIND-03** | Yêu cầu Requester chỉ xem Asset theo phòng chưa có ở Backend | `REQ-02`, `DEC-01`, `US-01-02-Spec.md` (kèm `BR-04`) | `AssetService.cs:64` chỉ lọc query `location` tự nguyện; DB không có liên kết Requester - Location; FE chặn cả route | High (Lệch phạm vi bảo mật dữ liệu) | Chốt quyết định: Cho phép Requester xem catalog công cộng để báo hỏng trong MVP, hoặc bổ sung gán phòng trong DB | Product Owner / Stakeholder | **PENDING DECISION** | `AssetService.cs:64`, `App.tsx:50-53` |
| **FIND-04** | Route Frontend thiếu guard Role (Hở deep-link) | `security-nfr.md` (SEC-09) vs `App.tsx:55-60` | Các trang `/admin/*`, `/predictions`, `/alerts`, `/work-orders` chỉ bọc `RequireAuth`, gõ URL vẫn mở được trang | Medium (Lộ giao diện đặc quyền; API vẫn chặn 403) | Bọc `<RequireRoles>` trong `App.tsx` cho các route này. Cập nhật mã bug `BUG-013`, `BUG-014` | Frontend Lead | **OPEN** | `App.tsx:55-60`, Bug Log mới `BUG-013`, `BUG-014` |
| **FIND-05** | Thiếu thu hồi token khi Admin đổi Role hoặc khóa tài khoản | `NFR-02`, `SEC-06` vs `UserService.cs:141-144` | Đổi Role hoặc `IsActive = false` chỉ lưu DB, không blacklist token; user dùng được token cũ tối đa 8h | High (Rủi ro chiếm dụng quyền sau khi bị khóa) | Bổ sung middleware kiểm tra `IsActive` hoặc cơ chế User Session Invalidation khi đổi tài khoản | Technical Lead / Security Reviewer | **OPEN** | `UserService.cs:141-144` |
| **FIND-06** | Lệch số lượng test giữa QA Verification và Repo thực tế | `qa-verification.md:17, 21-25` vs `SmartMaintenance.Tests` | Biên bản ghi 68 test (kèm 3 test `CreateMapping_*`); Repo GitHub/local thực tế chỉ có 65 test (không có 3 test này) | Low (Tài liệu kiểm thử chưa đồng bộ với mã nguồn) | Cập nhật lại biên bản QA Verification theo đúng mã nguồn thực tế (65 test) hoặc push 3 test bị thiếu lên repo | QA Lead / Dev Team | **OPEN** | `SmartMaintenance.Tests` (65 tests), `qa-verification.md:21-25` |

### Chi tiết các điểm lệch trọng yếu:

#### Điểm lệch 1 (FIND-01): Đặc tả "Admin thay đổi quyền của Role" mâu thuẫn với Kiến trúc RBAC cố định
- **Hiện trạng tài liệu:** Trong `docs/03-product/acceptance-criteria.md` (dòng 137-142), tiêu chí `AC-US-01-04-02 — Cập nhật quyền` mô tả: *"Given: Admin có quyền quản lý Role. When: Admin thay đổi quyền của một Role và lưu. Then: Hệ thống lưu cấu hình quyền mới."*
- **Thực tế trong Spec và Code:**
  - `docs/05-technical/specs/US-01-04-Spec.md` (dòng 69) khẳng định: *"Chính sách phân quyền (permission matrix) là cố định trong MVP; Admin chỉ có thể gán Role cho User, không tùy chỉnh từng quyền riêng lẻ."*
  - `src/backend/SmartMaintenance.Api/Controllers/RolesController.cs` (dòng 11-24) chỉ cung cấp duy nhất endpoint đọc `GET /api/roles/permissions` trả về biến tĩnh `PermissionsByRole`, hoàn toàn **không có API cập nhật hay lưu trữ quyền**.
  - Việc tồn tại API đọc `GET /api/roles/permissions` cũng chưa chứng minh rằng giao diện (UI) xem ma trận quyền đã được hoàn thiện.
- **Đề xuất BA xử lý:** Giữ nguyên ID `AC-US-01-04-02`. BA đề xuất PO chính thức phê duyệt điều chỉnh nội dung tiêu chí thành *"Xem ma trận phân quyền tĩnh"* nhằm đảm bảo tính khả thi của MVP và tuân thủ ràng buộc `CON-03`.

#### Điểm lệch 2 (FIND-02): Bảng quyền mô tả trong RolesController không khớp với [Authorize] thực tế của Controllers
- **Hiện trạng Code:** Trong `RolesController.cs` (dòng 12-18), ma trận quyền tĩnh được khai báo như sau:
  - `Requester`: `["ViewAsset", "CreateRequest", "TrackRequest"]`
  - `Technician`: `["ViewWorkOrder", "UpdateWorkOrder", "ViewIoTAlert"]`
  - `FacilityManager`: `["ManageAsset", "ManageRequest", "ManageWorkOrder", "ViewIoTData"]`
  - `Admin`: `["ManageUsers", "ManageRoles", "ManageIoTMapping"]`
- **Sự không khớp với các Controller khác:**
  1. `Technician`: Trong `AssetsController.cs:44, 53, 81, 90`, Technician được phép gọi `GET /api/assets`, `GET /api/assets/{id}`, `GET /api/assets/{id}/iot-data`, `GET /api/assets/{id}/prediction`. Các quyền này hoàn toàn thiếu trong danh sách của Technician tại `RolesController`.
  2. `Admin`: Trong `AssetsController.cs:44, 53`, Admin được phép gọi `GET /api/assets` và `GET /api/assets/{id}`, nhưng `RolesController` không gán quyền `ViewAsset` cho Admin.
  3. `Requester`: Được khai báo có quyền `ViewAsset`, nhưng ở Frontend (file `App.tsx:50`), route `/assets` bị chặn không cho Requester truy cập.
- **Đề xuất BA xử lý:** Cập nhật lại mảng `PermissionsByRole` trong `RolesController.cs` để phản ánh đúng tập hợp các API mà các Role thực sự được phép gọi trên Backend.

#### Điểm lệch 3 (FIND-03): Yêu cầu "Requester chỉ xem Asset thuộc phòng của mình" (REQ-02, DEC-01) chưa được thực thi ở Backend
- **Hiện trạng tài liệu:**
  - `REQ-02` (`requirements.md`): *"Requester chỉ được xem Asset thuộc phòng/khu vực mình được phép sử dụng."*
  - `DEC-01` (`decision-log.md`): *"Requester chỉ xem Asset thuộc phòng/khu vực được phép sử dụng, không xem toàn bộ hệ thống."*
  - `US-01-02-Spec.md` đặt mục tiêu cho Requester chỉ xem danh sách Asset thuộc phòng được phép. `BR-04` ("Mỗi Asset phải có Location/Room") chỉ là điều kiện dữ liệu đi kèm, không phải quy tắc phân quyền người dùng.
- **Thực tế trong Code Backend & Frontend:**
  - `AssetsController.cs:44` đặt `[Authorize(Roles = UserRoles.Requester + ...)]`.
  - Phương thức `AssetService.ListAsync(string? location, ...)` tại `src/backend/SmartMaintenance.Application/Assets/AssetService.cs:64` chỉ lọc theo query param `location` nếu client chủ động truyền lên, hoàn toàn **không nhận tham số `userId` hay `role`**. Việc client truyền query param không đồng nghĩa với kiểm soát quyền theo thẩm quyền người dùng.
  - Thực thể `User` trong CSDL không có trường `Location` hoặc bảng liên kết `UserLocations` để định danh Requester phụ trách phòng nào.
  - Phía Frontend (`src/frontend/src/App.tsx:50-53`) xử lý bằng cách chặn hẳn vai trò Requester truy cập vào trang `/assets` (`<RequireRoles roles={["FacilityManager", "Technician", "Admin"]} />`). Việc chặn route ở Frontend không chứng minh rằng quy tắc giới hạn dữ liệu đã được thực thi ở Backend.
- **Đề xuất BA xử lý:** Đây là khoảng cách nghiệp vụ (Gap) cần PO/Stakeholder quyết định: Chấp nhận cho Requester xem toàn bộ danh mục Asset công cộng trong trường để phục vụ báo sự cố tại `POST /api/requests` trong phạm vi MVP, hoặc bổ sung thực thể mapping User - Location trong CSDL ở giai đoạn tiếp theo.

#### Điểm lệch 4 (FIND-04): Route Frontend thiếu bọc RequireRoles (Hở Deep-link - BUG-013, BUG-014)
- **Hiện trạng Code Frontend:** Trong `src/frontend/src/App.tsx` (dòng 55-60):
  ```tsx
  <Route path="/work-orders" element={<WorkOrdersPage />} />
  <Route path="/alerts" element={<AlertsPage />} />
  <Route path="/predictions" element={<PredictionsPage />} />
  <Route path="/admin" element={<AdminPage />} />
  <Route path="/admin/users" element={<AdminUsersPage />} />
  <Route path="/admin/iot" element={<AdminIotPage />} />
  ```
  Các route này chỉ được bọc trong `RequireAuth` (chỉ kiểm tra đã đăng nhập), hoàn toàn **không có `RequireRoles`**.
- **Hệ quả thực tế:** Bất kỳ người dùng nào sau khi đăng nhập (kể cả Requester) đều có thể gõ trực tiếp URL `/admin/users`, `/admin/iot`, `/predictions` trên thanh địa chỉ trình duyệt để hiển thị giao diện trang đặc quyền. Mặc dù Backend vẫn thực thi chặn gọi API (`403 Forbidden`), nhưng giao diện và cấu trúc chức năng đặc quyền đã bị hiển thị.
- **Viện dẫn mã lỗi Bug Log mới:**
  - Vấn đề này đã được ghi nhận trong bộ Bug Log chuẩn hóa (`docs/06-test/Bug_Log_Smart_Maintenance.xlsx`):
    - **`BUG-013`**: Facility Manager mở được trang Người dùng bằng URL (`/admin/users`) (Mức độ: Minor, Trạng thái: Open).
    - **`BUG-014`**: Requester mở được trang AI Risks bằng URL (`/predictions`) (Mức độ: Minor, Trạng thái: Open).
  - *Ghi chú đồng bộ tài liệu:* Các tài liệu `docs/06-test/security-nfr.md` (mục SEC-09) và `docs/06-test/code-review.md` hiện vẫn đang ghi mã bug cũ (`BUG-011`, `BUG-013`). Cần đồng bộ lại các file đó theo bộ Bug Log mới trong đợt cập nhật tài liệu test kế tiếp.
- **Đề xuất BA xử lý:** Đề xuất đội ngũ Frontend cập nhật `App.tsx`:
  - Bọc các route `/admin`, `/admin/users`, `/admin/iot` trong `<RequireRoles roles={["Admin"]} />`.
  - Bọc route `/predictions` trong `<RequireRoles roles={["FacilityManager"]} />`.
  - Bọc route `/alerts` và `/work-orders` trong `<RequireRoles roles={["FacilityManager", "Technician"]} />`.

#### Điểm lệch 5 (FIND-05): Thiếu cơ chế thu hồi phiên đăng nhập khi Admin đổi Role hoặc khóa tài khoản
- **Hiện trạng Code:** Trong `src/backend/SmartMaintenance.Application/Users/UserService.cs:141-144`, khi Admin cập nhật Role hoặc đặt `IsActive = false`, code chỉ lưu database (`_db.SaveChangesAsync`).
- **Hệ quả bảo mật:** Người dùng bị đổi quyền hoặc bị khóa tài khoản vẫn có thể tiếp tục sử dụng Token cũ (có thời hạn lên tới 8 tiếng) để thao tác các API theo Role cũ cho đến khi token hết hạn.
- **Đề xuất BA xử lý:** Bổ sung kiểm tra trạng thái tài khoản `user.IsActive` trong token validation hoặc xây dựng cơ chế Revoke toàn bộ token của User khi tài khoản bị thay đổi trạng thái hoặc quyền hạn.

#### Điểm lệch 6 (FIND-06): Lệch số lượng test tự động giữa tài liệu QA Verification và Mã nguồn thực tế
- **Hiện trạng tài liệu QA:** Trong `docs/06-test/qa-verification.md` (dòng 17, 21-25) ghi nhận: *"Passed: 68, Total: 68"* và nêu rõ so với 65 test cũ đã thêm 3 test:
  - `CreateMapping_OnlyAssetId_AssignsSensorPrefix`
  - `CreateMapping_OnlyAssetId_Returns201_AndAutoSensorId`
  - `CreateMapping_Requester_Returns403`
- **Thực tế mã nguồn trong Repo:** Khi chạy lệnh `dotnet test` trực tiếp trên kho mã nguồn, kết quả thực tế là **65 test Passed, 0 Failed**. Thực hiện tìm kiếm toàn bộ codebase hoàn toàn **không tìm thấy** 3 hàm test `CreateMapping_*` nêu trên.
- **Đánh giá:** Đây là bằng chứng cho thấy bộ test tự động giữa tài liệu báo cáo của QA và mã nguồn thực tế chưa được đồng bộ (có thể do 3 test này nằm ở nhánh làm việc khác hoặc chưa được commit/push vào repo).
- **Đề xuất BA xử lý:** Đội ngũ QA và Dev cần rà soát lại để đưa 3 bài test mapping này vào test suite chính thức của dự án.

---

## 8. Câu hỏi mở (UNKNOWN / Open Questions)

1. **UNKNOWN-01 (Cơ chế liên kết Requester - Location theo REQ-02):**  
   *Câu hỏi:* Trong các phiên bản sau MVP, nhà trường quản lý quyền sử dụng phòng của Giảng viên/Sinh viên theo cơ chế nào (Ví dụ: theo thời khóa biểu học tập, theo danh sách phòng ban công tác, hay người dùng được tự do báo cáo sự cố cho mọi tài sản trong khuôn viên trường)?
2. **UNKNOWN-02 (Thời hạn Token 480 phút trong môi trường trường học):**  
   *Câu hỏi:* Thời hạn JWT Token 8 giờ (480 phút) không có Refresh Token có đáp ứng tiêu chuẩn an toàn thông tin khi sinh viên/giảng viên đăng nhập trên các máy tính công cộng tại giảng đường/phòng lab hay không? Có cần rút ngắn thời hạn xuống 60-120 phút và bổ sung cơ chế Refresh Token không?
3. **UNKNOWN-03 (Định hướng phát triển của RolesController):**  
   *Câu hỏi:* Bảng ánh xạ quyền tĩnh trong `RolesController.PermissionsByRole` có tiếp tục được sử dụng làm danh mục tham chiếu cho Frontend hay sẽ được thiết kế thành hệ thống Dynamic Permission quản lý trong cơ sở dữ liệu ở giai đoạn 2?

---

## 9. Bằng chứng kiểm tra và xác minh (Verification Outputs)

Mục này ghi lại toàn bộ output thực tế thu được từ các lệnh kiểm tra và kiểm thử tự động trực tiếp trên repository.

### 9.1. Đối chiếu toàn bộ [Authorize] và [AllowAnonymous] trong Controllers
Kết quả trích xuất từ `SmartMaintenance.Api/Controllers/*.cs` chứng minh **100% endpoint trong ma trận (Mục 3)** đều khớp chính xác với mã nguồn:

```text
[AuthController.cs:21]        [AllowAnonymous]                                                     -> POST /api/auth/login
[AuthController.cs:33]        [Authorize]                                                          -> POST /api/auth/logout
[UsersController.cs:22]       [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Admin)] -> GET  /api/users
[UsersController.cs:35]       [Authorize(Roles = UserRoles.Admin)]                                 -> POST /api/users
[UsersController.cs:49]       [Authorize(Roles = UserRoles.Admin)]                                 -> PUT  /api/users/{id}
[RolesController.cs:21]       [Authorize(Roles = UserRoles.Admin)]                                 -> GET  /api/roles/permissions
[AssetsController.cs:31]      [Authorize(Roles = UserRoles.FacilityManager)]                       -> POST /api/assets
[AssetsController.cs:44]      [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager + "," + UserRoles.Technician + "," + UserRoles.Admin)] -> GET  /api/assets
[AssetsController.cs:53]      [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager + "," + UserRoles.Technician + "," + UserRoles.Admin)] -> GET  /api/assets/{id}
[AssetsController.cs:63]      [Authorize(Roles = UserRoles.FacilityManager)]                       -> PUT  /api/assets/{id}
[AssetsController.cs:72]      [Authorize(Roles = UserRoles.FacilityManager)]                       -> PATCH /api/assets/{id}/status
[AssetsController.cs:81]      [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)] -> GET  /api/assets/{id}/iot-data
[AssetsController.cs:90]      [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)] -> GET  /api/assets/{id}/prediction
[RequestsController.cs:22]    [Authorize(Roles = UserRoles.Requester)]                             -> POST /api/requests
[RequestsController.cs:35]    [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager)] -> GET  /api/requests
[RequestsController.cs:49]    [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager)] -> GET  /api/requests/{id}
[RequestsController.cs:63]    [Authorize(Roles = UserRoles.FacilityManager)]                       -> PATCH /api/requests/{id}/status
[RequestsController.cs:72]    [Authorize(Roles = UserRoles.FacilityManager)]                       -> GET  /api/requests/{id}/history
[WorkOrdersController.cs:22]  [Authorize(Roles = UserRoles.FacilityManager)]                       -> POST /api/work-orders
[WorkOrdersController.cs:35]  [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)] -> GET  /api/work-orders
[WorkOrdersController.cs:49]  [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)] -> GET  /api/work-orders/{id}
[WorkOrdersController.cs:63]  [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)] -> PATCH /api/work-orders/{id}
[IotController.cs:22-23]      [AllowAnonymous], [IotGatewayAuthorize]                              -> POST /api/iot/ingest
[IotAlertsController.cs:21]   [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)] -> GET  /api/iot-alerts
[IotMappingsController.cs:21] [Authorize(Roles = UserRoles.Admin)]                                 -> POST /api/iot-mappings
[IotMappingsController.cs:32] [Authorize(Roles = UserRoles.Admin)]                                 -> GET  /api/iot-mappings
[IotMappingsController.cs:41] [Authorize(Roles = UserRoles.Admin)]                                 -> PUT  /api/iot-mappings/{id}
[PredictionsController.cs:21] [Authorize(Roles = UserRoles.FacilityManager)]                       -> GET  /api/predictions
```

### 9.2. Danh sách kiểm tra các hàm Test thực tế trong thư mục `src/backend/SmartMaintenance.Tests`
Dưới đây là 22 bài test thực tế có attribute `[Fact]` trong mã nguồn kiểm thử liên quan đến xác thực và phân quyền (không bao gồm phương thức helper):
1. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_Anonymous_Returns401`
2. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_RequesterToken_Returns403`
3. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_ValidFacilityManager_Returns201_AndPersists`
4. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_Anonymous_Returns401`
5. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_FacilityManager_Returns403`
6. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_ValidRequester_Returns201_Submitted`
7. `SmartMaintenance.Tests.Requests.RequestServiceTests.Create_Valid_PersistsSubmitted_WithJwtRequesterId`
8. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Anonymous_Returns401`
9. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Requester_Returns403`
10. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_ValidManager_Returns201_Assigned`
11. `SmartMaintenance.Tests.WorkOrders.WorkOrderServiceTests.Create_Valid_PersistsAssigned`
12. `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_BadApiKey_Returns401`
13. `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_MappedDevice_Returns201_AndPersists`
14. `SmartMaintenance.Tests.Integration.IotApiTests.GetIotData_Requester_Returns403`
15. `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Requester_Returns403`
16. `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Technician_Allowed`
17. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminTarget_Throws403_QT4`
18. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminSelfDeactivate_Throws403_QT4`
19. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AssignAdmin_Throws400_QT3`
20. `SmartMaintenance.Tests.Users.UserServiceTests.Update_Requester_Throws400_QT2`
21. `SmartMaintenance.Tests.Users.UserServiceTests.Update_TechnicianToFacilityManager_Ok_QT1`
22. `SmartMaintenance.Tests.Users.UserServiceTests.Update_FacilityManagerToTechnician_Ok_QT1`

### 9.3. Kết quả chạy kiểm thử thực tế trên hệ thống
- **Thời điểm thực thi:** 10/10/2026.
- **Commit tham chiếu:** `ecac1c9` (GitHub HEAD: `ecac1c9c88a631ba94ed464b334c410d90cf001d`).

#### Lệnh 1: Toàn bộ Test Suite Backend
```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```
Output thực tế:
```text
Test run for ...\src\backend\SmartMaintenance.Tests\bin\Debug\net9.0\SmartMaintenance.Tests.dll (.NETCoreApp,Version=v9.0)
VSTest version 17.14.1 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    65, Skipped:     0, Total:    65, Duration: 14 s - SmartMaintenance.Tests.dll (net9.0)
```
*(Ghi chú: Mã nguồn trên GitHub và local có 65 test passed; đối chiếu với tài liệu `qa-verification.md` ghi 68 test do thiếu 3 test `CreateMapping_*` đã được ghi nhận tại Finding FIND-06).*

#### Lệnh 2: Bộ lọc các Test xác thực & phân quyền (401 / 403)
```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj --filter "FullyQualifiedName~403|FullyQualifiedName~401"
```
Output thực tế:
```text
Test run for ...\src\backend\SmartMaintenance.Tests\bin\Debug\net9.0\SmartMaintenance.Tests.dll (.NETCoreApp,Version=v9.0)
VSTest version 17.14.1 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    11, Skipped:     0, Total:    11, Duration: 9 s - SmartMaintenance.Tests.dll (net9.0)
```

Chi tiết 11 test case chạy thành công trong bộ lọc:
1. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_Anonymous_Returns401` [Passed]
2. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_RequesterToken_Returns403` [Passed]
3. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_Anonymous_Returns401` [Passed]
4. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_FacilityManager_Returns403` [Passed]
5. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Anonymous_Returns401` [Passed]
6. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Requester_Returns403` [Passed]
7. `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_BadApiKey_Returns401` [Passed]
8. `SmartMaintenance.Tests.Integration.IotApiTests.GetIotData_Requester_Returns403` [Passed]
9. `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Requester_Returns403` [Passed]
10. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminTarget_Throws403_QT4` [Passed]
11. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminSelfDeactivate_Throws403_QT4` [Passed]

---

### 9.4. Bảng tổng kết số liệu

| Chỉ số đánh giá | Số lượng | Chi tiết và Phân tích kỹ thuật |
|---|---|---|
| **Tổng số API Endpoints** | **28** | Toàn bộ 28 action routes trong 10 Controllers backend |
| **(a) Số Endpoint có Test 401/403 ở mức API** | **6 / 28** | Gồm 6 endpoint: `POST /api/assets`, `POST /api/requests`, `POST /api/work-orders`, `GET /api/assets/{id}/iot-data`, `GET /api/assets/{id}/prediction`, `POST /api/iot/ingest` |
| **(a) Số Endpoint CHƯA CÓ Test 401/403 ở mức API** | **22 / 28** | **28 − 6 = 22 endpoints**, gồm:<br>1. `POST /api/auth/login` (AllowAnonymous, không cần test 403; chưa có test API cho 401)<br>2. `POST /api/auth/logout` (chỉ cần Authenticated, không cần test 403; chưa có test API cho 401)<br>3. `GET /api/users` (chưa có test 403)<br>4-9. **Các endpoint Admin-only chưa có test 403 ở mức API:** `POST /api/users`, `PUT /api/users/{id}`, `GET /api/roles/permissions`, `POST /api/iot-mappings`, `GET /api/iot-mappings`, `PUT /api/iot-mappings/{id}` *(Lưu ý: `UserServiceTests` hiện tại chỉ kiểm thử unit test ở tầng Service, chưa có test 403 ở tầng Controller khi Non-Admin gọi API)*<br>10-13. Các endpoint Assets khác: `GET /api/assets`, `GET /api/assets/{id}`, `PUT /api/assets/{id}`, `PATCH /api/assets/{id}/status`<br>14-17. Các endpoint Requests khác: `GET /api/requests`, `GET /api/requests/{id}`, `PATCH /api/requests/{id}/status`, `GET /api/requests/{id}/history`<br>18-20. Các endpoint Work Orders khác: `GET /api/work-orders`, `GET /api/work-orders/{id}`, `PATCH /api/work-orders/{id}`<br>21-22. Các endpoint còn lại: `GET /api/iot-alerts`, `GET /api/predictions` |
| **(b) Số AC chưa có Test tự động** | **10 / 26** | **10 / 26 AC** (chiếm 38.5%):<br>- *Nhóm Auth (3 AC):* `AC-AUTH-01` (login chỉ dùng qua helper), `AC-AUTH-02` (sai credential), `AC-AUTH-03` (logout/blacklist).<br>- *Nhóm AuthZ (7 AC):* `AC-AUTHZ-03` (lọc danh sách Request), `AC-AUTHZ-04` (chi tiết Request 403), `AC-AUTHZ-07` (Tech xem WO 200), `AC-AUTHZ-08` (Tech xem WO người khác 403), `AC-AUTHZ-09` (Tech reassign 403), `AC-AUTHZ-10` (FM set result/rejection 403), `AC-AUTHZ-20` (Roles permissions 403).<br>*(Số AC đã có test tự động: 16/26 AC = 61.5%)* |
| **Tổng số Open Findings cần BA/PO chốt** | **6** | Gồm FIND-01 (Mâu thuẫn sửa quyền), FIND-02 (Ma trận tĩnh chưa khớp API), FIND-03 (Phạm vi Asset theo phòng), FIND-04 (Hở deep-link frontend), FIND-05 (Thu hồi phiên khi đổi Role/khóa user), FIND-06 (Lệch số lượng test giữa QA và mã nguồn) |

---

## 10. Giới hạn tài liệu và Khuyến nghị hành động tiếp theo

### 10.1. Giới hạn tài liệu (Documentation Limitations)
- Tài liệu này phản ánh trung thực hiện trạng mã nguồn (As-built) tại commit `ecac1c9` và môi trường kiểm thử local.
- Tài liệu không tự ý thay đổi các yêu cầu nghiệp vụ đã được baselined (`REQ-02`, `CON-03`, `DEC-01`) mà ghi nhận các khoảng cách dưới dạng Open Findings.
- Tài liệu không bao gồm các cơ chế bảo mật nâng cao cấp hạ tầng (TLS/HTTPS, Web Application Firewall - WAF, Key Vault/Secret Manager) do nằm ngoài phạm vi MVP của đồ án.

### 10.2. Khuyến nghị hành động tiếp theo dành cho Đội ngũ Dự án
1. **Dành cho BA & Product Owner:**
   - Họp xem xét và ra quyết định chính thức cho **FIND-01** (điều chỉnh `AC-US-01-04-02` cho khớp với RBAC tĩnh) và **FIND-03** (chính sách phạm vi xem Asset của Requester).
2. **Dành cho Đội ngũ Frontend:**
   - Khắc phục **FIND-04** bằng cách bọc `<RequireRoles>` trong file `src/frontend/src/App.tsx` cho các route `/admin/*`, `/predictions`, `/alerts`, `/work-orders` để đóng lỗi `BUG-013` và `BUG-014`.
3. **Dành cho Đội ngũ Backend:**
   - Cập nhật ma trận tĩnh trong `RolesController.cs` (**FIND-02**).
   - Nghiên cứu giải pháp vô hiệu hóa token khi tài khoản bị khóa hoặc đổi Role (**FIND-05**).
4. **Dành cho Đội ngũ QA:**
   - Bổ sung các bài test tự động cho 10 AC còn thiếu, đặc biệt là kiểm thử phân quyền 403 mức API cho các endpoint quản trị Admin.
   - Đồng bộ hóa 3 bài test `CreateMapping_*` giữa biên bản QA và mã nguồn repository (**FIND-06**).
