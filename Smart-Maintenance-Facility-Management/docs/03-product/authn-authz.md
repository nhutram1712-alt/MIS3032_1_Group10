# Đặc tả Xác thực và Phân quyền (Authentication & Authorization Specification)

> **Dự án:** Smart Maintenance & Facility Management  
> **Tổ chức:** Trường Đại học Kinh tế – Đại học Đà Nẵng (DUE)  
> **Phiên bản:** 1.0  
> **Trạng thái:** Official Baselined  
> **Vai trò thực hiện:** Business Analyst & Technical Reviewer  
> **Vị trí file:** `docs/03-product/authn-authz.md`

---

## 1. Mục đích, phạm vi và tham chiếu

### 1.1. Mục đích
Tài liệu này định nghĩa chi tiết cơ chế Xác thực (Authentication - AuthN) và Phân quyền (Authorization - AuthZ) của hệ thống **Smart Maintenance & Facility Management**. Tài liệu xác lập các quy tắc phân quyền theo vai trò (Role-Based Access Control - RBAC) và quy tắc sở hữu dữ liệu (Data Ownership) dựa trên **mã nguồn thực tế** của hệ thống, đồng thời đối chiếu với các tài liệu nghiệp vụ đã được baselined để chỉ ra các điểm nhất quán và điểm lệch cần xử lý.

### 1.2. Phạm vi
- **Backend:** Toàn bộ API Controller, Middleware, Filter bảo mật và Application Layer trong dự án `SmartMaintenance.Api`, `SmartMaintenance.Application`, `SmartMaintenance.Infrastructure`.
- **Frontend:** Hệ thống Route Guard (`RequireAuth`, `RequireRoles`) và phân chia Navigation Menu theo vai trò người dùng trong `src/frontend/src`.
- **Thiết bị ngoại vi:** Cơ chế xác thực qua API Key dành cho IoT Gateway khi gửi dữ liệu đo đạc (telemetry/ingest).

### 1.3. Tài liệu tham chiếu
- **Requirements:** 
  - `REQ-01`: Đăng nhập tài khoản nội bộ hệ thống.
  - `REQ-02`, `REQ-03`: Quyền xem Asset và trạng thái Asset của Requester.
  - `REQ-04`, `REQ-05`: Quyền tạo và theo dõi Maintenance Request của Requester.
  - `REQ-06`, `REQ-07`, `REQ-08`: Quyền tạo, cập nhật, đổi trạng thái Asset của FacilityManager.
  - `REQ-09`, `REQ-10`: Quyền xem IoT Data và cảnh báo IoT Alert của FacilityManager.
  - `REQ-11`, `REQ-12`: Quyền tiếp nhận và xem dự báo AI Prediction của FacilityManager.
  - `REQ-13`, `REQ-14`, `REQ-15`, `REQ-16`: Quyền xử lý Request, tạo và theo dõi Work Order của FacilityManager.
  - `REQ-17`, `REQ-18`, `REQ-19`: Quyền xem Work Order, Asset, IoT Alert, AI Prediction của Technician.
  - `REQ-20`, `REQ-21`, `REQ-22`, `REQ-23`: Quyền cập nhật, từ chối, hoàn thành Work Order của Technician.
  - `REQ-24`: Quyền quản lý tài khoản người dùng của Admin.
  - `REQ-25`: Quyền quản lý phân quyền theo Role của Admin.
  - `REQ-26`, `REQ-27`: Quyền thiết lập và cập nhật IoT Mapping của Admin.
  - `REQ-28`: Thu thập IoT Data chu kỳ 5 phút/lần.
- **Non-Functional Requirements:** 
  - `NFR-01`: Kiểm soát truy cập dựa trên vai trò (RBAC).
  - `NFR-02`: Bảo vệ thông tin xác thực, lưu trữ mật khẩu an toàn.
  - `NFR-07`: Giao diện hiển thị chức năng và dữ liệu phù hợp với từng Role.
- **Ràng buộc hệ thống (Constraints):** 
  - `CON-03`: Hệ thống MVP chỉ có 4 Role cố định: `Requester`, `Technician`, `FacilityManager`, `Admin`.
  - `CON-08`: MVP sử dụng tài khoản riêng của hệ thống, không tích hợp SSO.
- **Business Rules:** 
  - `BR-04`: Mỗi Asset phải có Location/Room.
  - `BR-05`: Maintenance Request phải xác định Asset trước khi tạo Work Order.
  - `BR-06`: Một Maintenance Request chỉ tạo tối đa một Work Order trong MVP.
  - `BR-07`: Technician chỉ được cập nhật Work Order được phân công cho mình; có thể từ chối kèm lý do.
  - `BR-08`: Mỗi Work Order phải liên kết với một Asset cụ thể.
  - `BR-09`: Work Order hoàn thành phải có kết quả lưu vào Maintenance History.
  - `BR-13`: IoT Device phải được mapping với Asset trước khi monitoring. Một Asset chỉ có một IoT Device trong MVP.
  - `BR-14`: Lifecycle Maintenance Request (`Submitted` → `Pending` → `In Progress` → `Resolved` → `Closed` / `Rejected`).
  - `BR-16`: Chỉ Facility Manager được thay đổi thủ công Asset Status.
  - `BR-17`: Lifecycle Work Order (`Assigned` → `In Progress` → `Completed` / `Cancelled`).
  - `BR-18`: Maintenance Request chỉ được Closed sau khi Technician hoàn thành Work Order và Facility Manager xác nhận kết quả.
- **Quyết định kiến trúc (Decision Log):** 
  - `DEC-01`: Giới hạn phạm vi Asset và quyền xem của Requester.
  - `DEC-02`: Quy trình xử lý Maintenance Request.
  - `DEC-03`: Quy trình Work Order và quyền từ chối của Technician.
  - `DEC-04`: Giám sát IoT và ngưỡng cảnh báo.
  - `DEC-06`: Authentication nội bộ và RBAC 4 Roles.
- **Kỹ thuật & Đặc tả Story:**
  - `docs/05-technical/specs/US-01-01-Spec.md` đến `US-01-04-Spec.md`.
  - `docs/03-product/acceptance-criteria.md` (EPIC-01).
  - `docs/05-technical/api-contract.md`.
  - `docs/06-test/security-nfr.md`.

---

## 2. Authentication (Xác thực)

### 2.1. Phương thức đăng nhập
- **Endpoint:** `POST /api/auth/login` (cho phép truy cập ẩn danh `[AllowAnonymous]`).  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Controllers/AuthController.cs:20-30`.
- **Cơ chế xác thực:**
  - Client gửi payload JSON gồm `username` và `password`.
  - Hệ thống tìm kiếm bản ghi trong bảng `Users` theo điều kiện `u.Username == request.Username.Trim() && u.IsActive`.  
    *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:32-34`.
  - Nếu không tìm thấy người dùng hoặc tài khoản bị vô hiệu hóa (`IsActive == false`), hệ thống trả về `null` và Controller phản hồi `401 Unauthorized` kèm `{ "error": "Invalid username or password." }`.  
    *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:29-37` và `src/backend/SmartMaintenance.Api/Controllers/AuthController.cs:27-28`.
  - Mật khẩu người dùng được so khớp bằng hàm `BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)`.  
    *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:36`.
  - Hệ thống không hỗ trợ đăng nhập một lần (SSO) theo ràng buộc `CON-08`.

### 2.2. JSON Web Token (JWT)
- **Thời hạn hiệu lực:** 480 phút (tương đương 8 giờ làm việc), được cấu hình tại khóa `Jwt:ExpiresMinutes` trong file cấu hình và nạp vào token handler.  
  *Nguồn:* `src/backend/SmartMaintenance.Api/appsettings.json:12` và `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:82-84, 103`.
- **Thuật toán ký & Secret Key:** Sử dụng thuật toán `HmacSha256` với khóa bí mật đối xứng `Jwt:Secret`. Khóa bí mật bắt buộc phải có độ dài tối thiểu từ 32 ký tự trở lên.  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Program.cs:33-36` và `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:86-87`.
- **Danh sách Claims được nhúng trong Token:**
  1. `JwtRegisteredClaimNames.Sub`: Lưu `UserId` (kiểu chuỗi).
  2. `JwtRegisteredClaimNames.Jti`: Định danh token duy nhất sinh ngẫu nhiên dạng Guid (`Guid.NewGuid().ToString("N")`).
  3. `ClaimTypes.NameIdentifier`: Lưu `UserId`.
  4. `ClaimTypes.Name`: Lưu `username`.
  5. `ClaimTypes.Role`: Lưu chuỗi Role (`Requester`, `Technician`, `FacilityManager`, `Admin`).
  6. `"role"`: Lưu chuỗi Role (phục vụ tương thích claims mở rộng).  
  *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:88-97`.
- **Cấu hình xác thực Token:** `MapInboundClaims = false`, bắt buộc kiểm tra Issuer (`Jwt:Issuer`), Audience (`Jwt:Audience`), thời hạn còn hiệu lực (`ValidateLifetime = true`) và chữ ký hợp lệ (`ValidateIssuerSigningKey = true`).  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Program.cs:42-52`.

### 2.3. Đăng xuất và Cơ chế Blacklist Token
- **Endpoint:** `POST /api/auth/logout` (yêu cầu token xác thực `[Authorize]`).  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Controllers/AuthController.cs:32-45`.
- **Cơ chế thu hồi (Revocation / Blacklist):**
  - Hệ thống đọc chuỗi Raw Bearer Token từ header `Authorization`, phân tích giải mã JWT token.
  - Lấy claim `jti` của token. Trường hợp token không có `jti`, hệ thống băm chuỗi token bằng thuật toán SHA-256 (`SHA256.HashData`) chuyển sang chuỗi Hex làm khóa đại diện.  
    *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:54-63`.
  - Khóa này cùng thời điểm hết hạn của token (`jwt.ValidTo`) được đưa vào dịch vụ quản lý danh sách đen `ITokenBlacklist` (được đăng ký dưới dạng Singleton).  
    *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/AuthService.cs:65-66` và `src/backend/SmartMaintenance.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs:34`.
  - Cấu trúc `TokenBlacklist` lưu trữ trên bộ nhớ RAM bằng `ConcurrentDictionary<string, DateTime>` và tự động dọn dẹp các mục hết hạn (`CleanupExpired`).  
    *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Auth/TokenBlacklist.cs:8-44`.
  - Khi có request gửi lên, sự kiện `OnTokenValidated` trong JwtBearer Middleware sẽ kiểm tra khóa token đối với `ITokenBlacklist`. Nếu khóa tồn tại trong danh sách đen, request lập tức bị từ chối với lệnh `context.Fail("Token has been revoked.")`, trả về mã lỗi `401 Unauthorized`.  
    *Nguồn:* `src/backend/SmartMaintenance.Api/Program.cs:53-76`.

### 2.4. Mã hóa mật khẩu (Password Hashing)
- Sử dụng thuật toán BCrypt thông qua thư viện `BCrypt.Net-Next`. Mật khẩu plaintext được hash với salt tự động trước khi lưu vào cột `PasswordHash` trong database.  
  *Nguồn:* `src/backend/SmartMaintenance.Application/Users/UserService.cs:74` và `src/backend/SmartMaintenance.Infrastructure/Persistence/DbSeeder.cs:13`.
- Mật khẩu khởi tạo demo cho toàn bộ tài khoản seed trong hệ thống là `"Due@2026"`.  
  *Nguồn:* `src/backend/SmartMaintenance.Infrastructure/Persistence/DbSeeder.cs:9`.
- Bảo mật DTO: Tuyệt đối không bao giờ trả trường `PasswordHash` ra ngoài client trong bất kỳ API nào. Đối tượng `UserSummary` chỉ chứa: `UserId`, `Username`, `FullName`, `Role`, `IsActive`.  
  *Nguồn:* `src/backend/SmartMaintenance.Application/Users/UserService.cs:40-47, 153-160`.

### 2.5. Xác thực API Key của IoT Gateway
- **Endpoint:** `POST /api/iot/ingest`.  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Controllers/IotController.cs:20-34`.
- **Cơ chế:** Endpoint được cấu hình `[AllowAnonymous]` (không sử dụng JWT Bearer Token) kết hợp với Custom Authorization Filter `[IotGatewayAuthorize]`.  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Controllers/IotController.cs:22-23`.
- **Filter thực thi:** `IotGatewayAuthorizeAttribute` kiểm tra sự tồn tại của header `X-Api-Key` trong HTTP Request và so khớp chuỗi với giá trị cấu hình `Iot:GatewayApiKey` (giá trị mặc định môi trường phát triển: `"DEV_ONLY_IOT_GATEWAY_KEY"`).  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Security/IotGatewayAuthorizeAttribute.cs:6-21` và `src/backend/SmartMaintenance.Api/appsettings.json:15`.
- Nếu header `X-Api-Key` bị thiếu hoặc giá trị không khớp chính xác, filter sẽ chặn đứng request và trả về ngay mã `401 Unauthorized` kèm body: `{ "error": "Invalid or missing gateway API key." }`.  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Security/IotGatewayAuthorizeAttribute.cs:16`.

### 2.6. Hành vi khi thay đổi Role hoặc Vô hiệu hóa tài khoản
- Admin thay đổi vai trò hoặc khóa tài khoản thông qua endpoint `PUT /api/users/{id}`.  
  *Nguồn:* `src/backend/SmartMaintenance.Api/Controllers/UsersController.cs:48-62`.
- **Hành vi thực tế trong mã nguồn:** Khi Admin cập nhật `Role` mới hoặc gán `IsActive = false`, hệ thống cập nhật bản ghi trong cơ sở dữ liệu (`_db.SaveChangesAsync`). Tuy nhiên, mã nguồn **KHÔNG** thực hiện gọi hàm `_tokenBlacklist.Blacklist(...)` đối với các token đang hoạt động của người dùng đó.  
  *Nguồn:* `src/backend/SmartMaintenance.Application/Users/UserService.cs:141-144`.
- **Hệ quả bảo mật:** Token JWT đã cấp cho người dùng vẫn tiếp tục có hiệu lực đầy đủ với các quyền của Role cũ cho tới khi token hết thời hạn (tối đa 480 phút) hoặc người dùng chủ động gọi đăng xuất. Người dùng chỉ bị áp dụng Role mới hoặc bị chặn đăng nhập khi thực hiện đăng nhập lại để nhận token mới. (Xem mục 7: Điểm lệch cần BA chốt).

---

## 3. Ma trận Role × Endpoint

Bảng ma trận dưới đây tổng hợp toàn bộ 28 endpoints thực tế trong Backend Controllers, xác định Role được phép, ràng buộc dữ liệu (Ownership/Scope), nơi thực thi và vị trí mã nguồn chứng minh.

| Endpoint | Method | Role được phép | Quy tắc Dữ liệu & Ownership | Thực thi ở | Nguồn Code (file:dòng) |
|---|---|---|---|---|---|
| `/api/auth/login` | POST | Anonymous (Public) | Không kiểm tra quyền; kiểm tra credentials đăng nhập | Backend | `SmartMaintenance.Api/Controllers/AuthController.cs:20-21` |
| `/api/auth/logout` | POST | Requester, Technician, FacilityManager, Admin | Thu hồi chính token của phiên hiện tại vào blacklist | Backend | `SmartMaintenance.Api/Controllers/AuthController.cs:32-33` |
| `/api/users` | GET | FacilityManager, Admin | FM bắt buộc phải kèm query `?role=...` (thường là `role=Technician`); Admin xem tất cả | Backend | `SmartMaintenance.Api/Controllers/UsersController.cs:21-28` |
| `/api/users` | POST | Admin | Tạo user mới; cấm trùng username; role thuộc 4 roles hợp lệ | Backend | `SmartMaintenance.Api/Controllers/UsersController.cs:34-35` |
| `/api/users/{id}` | PUT | Admin | Cấm sửa Admin (403); cấm đổi Requester (400); cấm gán Admin (400); chỉ xoay Technician ↔ FacilityManager | Backend | `SmartMaintenance.Api/Controllers/UsersController.cs:48-49` & `UserService.cs:99-131` |
| `/api/roles/permissions` | GET | Admin | Không; trả về danh sách phân quyền tĩnh của hệ thống | Backend | `SmartMaintenance.Api/Controllers/RolesController.cs:20-21` |
| `/api/assets` | POST | FacilityManager | Gán `CreatedByUserId` theo ID của FacilityManager đăng nhập | Backend | `SmartMaintenance.Api/Controllers/AssetsController.cs:30-31` |
| `/api/assets` | GET | Requester, Technician, FacilityManager, Admin | BE cho phép cả 4 Role xem toàn bộ Asset; FE bọc chặn Requester (xem mục 7) | Backend + FE-only chặn Requester | `SmartMaintenance.Api/Controllers/AssetsController.cs:43-44` & `frontend/App.tsx:50` |
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
| `/api/work-orders/{id}` | PATCH | FacilityManager, Technician | **Ownership & Action Matrix:**<br>- Technician chỉ được patch WO của mình (403 nếu sai); cấm gán lại người khác (403); chỉ chuyển In Progress, Completed (+result), Cancelled (+rejectionReason).<br>- FM cấm điền result/rejection (403); chỉ được reassign khi Assigned hoặc chuyển Cancelled. | Backend | `SmartMaintenance.Api/Controllers/WorkOrdersController.cs:62-63` & `WorkOrderService.cs:165-271, 327-332` |
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
   *Nguồn:* `RequestsController.cs:26-30` và `RequestService.cs:52`.
2. **Lọc danh sách theo người tạo (List Isolation):**
   - Trong phương thức `ListAsync(userId, role)`:
     ```csharp
     if (string.Equals(role, UserRoles.Requester, StringComparison.Ordinal))
         query = query.Where(r => r.RequesterId == userId);
     ```
   - Requester gọi `GET /api/requests` chỉ nhận được danh sách các bản ghi do chính mình tạo. FacilityManager không bị lọc điều kiện này và xem được toàn bộ danh sách.  
   *Nguồn:* `RequestService.cs:75-77`.
3. **Chặn truy cập chi tiết trái phép (Read Authorization):**
   - Trong phương thức `GetByIdAsync(id, userId, role)`:
     ```csharp
     if (string.Equals(role, UserRoles.Requester, StringComparison.Ordinal) && entity.RequesterId != userId)
         throw new AppException("You do not have access to this request.", 403);
     ```
   - Nếu Requester cố tình gửi yêu cầu `GET /api/requests/{id}` với `id` của một yêu cầu thuộc về người khác, hệ thống lập tức ném ngoại lệ trả về mã `403 Forbidden`.  
   *Nguồn:* `RequestService.cs:91-93`.

### 4.2. Quy tắc sở hữu của Technician (Phiếu công việc)
Thực thi tại `src/backend/SmartMaintenance.Application/WorkOrders/WorkOrderService.cs`:
1. **Lọc danh sách phiếu phân công (Assigned Work Order Isolation):**
   - Trong phương thức `ListAsync(userId, role)`:
     ```csharp
     if (string.Equals(role, UserRoles.Technician, StringComparison.Ordinal))
         query = query.Where(w => w.TechnicianId == userId);
     ```
   - Technician gọi `GET /api/work-orders` chỉ nhận được các phiếu có `TechnicianId` trùng với mã `userId` của mình.  
   *Nguồn:* `WorkOrderService.cs:101-102`.
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
   *Nguồn:* `WorkOrderService.cs:116, 327-332`.
3. **Kiểm soát cập nhật và ngăn chặn leo thang thao tác (Patch Enforce Matrix):**
   - Trong phương thức `PatchAsync`:
     - Bắt buộc kiểm tra quyền sở hữu: Nếu vai trò là Technician, phải vượt qua `EnsureTechnicianAccess` (không được sửa phiếu của người khác - trả mã `403`).  
       *Nguồn:* `WorkOrderService.cs:172-173`.
     - Chặn hành vi tự ý chuyển việc: Nếu Technician truyền `payload.TechnicianId` trong request, hệ thống chặn ngay với ngoại lệ `403 Forbidden` (`"Technician cannot reassign Work Orders."`).  
       *Nguồn:* `WorkOrderService.cs:219-220`.
     - Giới hạn hành động của Technician: Chỉ được chuyển trạng thái sang `In Progress`, hoặc `Cancelled` (kèm `rejectionReason`), hoặc `Completed` (bắt buộc kèm `result` theo quy tắc `BR-09`).

### 4.3. Giới hạn quyền hạn của Facility Manager
- Facility Manager có quyền xem toàn bộ danh mục tài sản, yêu cầu bảo trì và phiếu công việc trên toàn trường.
- **Giới hạn can thiệp kỹ thuật (Enforce role segregation):** Khi thực hiện `PATCH /api/work-orders/{id}`, FacilityManager **bị cấm tuyệt đối** việc nhập nội dung kết quả kỹ thuật (`result`) hoặc lý do từ chối kỹ thuật (`rejectionReason`) thay cho Technician:
  ```csharp
  if (isFm)
  {
      if (hasRejection || hasResult)
          throw new AppException("FacilityManager cannot set rejectionReason or result.", 403);
  ...
  ```
  *Nguồn:* `WorkOrderService.cs:186-187`.

### 4.4. Giới hạn bất biến của tài khoản Admin
Thực thi tại `src/backend/SmartMaintenance.Application/Users/UserService.cs`:
1. **Bảo vệ tài khoản Admin khỏi chỉnh sửa (QT-4):**
   - `PUT /api/users/{id}` kiểm tra: `if (user.Role == UserRoles.Admin) throw new AppException("Admin accounts cannot be modified via this endpoint.", 403);`.
   - Ngăn chặn việc Admin tự tước quyền, tự khóa tài khoản hoặc chỉnh sửa tài khoản Admin khác qua API thông thường.  
   *Nguồn:* `UserService.cs:100-101`.
2. **Chống leo thang đặc quyền (Privilege Escalation Prevention - QT-3 & QT-2):**
   - Không được nâng cấp bất kỳ tài khoản nào lên Admin qua PUT: `if (newRole == UserRoles.Admin) throw new AppException("Cannot assign Admin role via this endpoint...", 400);`.  
     *Nguồn:* `UserService.cs:111-116`.
   - Không được đổi vai trò của tài khoản Requester: `if (user.Role == UserRoles.Requester) throw new AppException("Cannot change role of a Requester account.", 400);`.  
     *Nguồn:* `UserService.cs:119-120`.
   - Điểm cuối PUT chỉ cho phép luân chuyển vai trò giữa Technician và FacilityManager: `if (!RotatableRoles.Contains(user.Role) || !RotatableRoles.Contains(newRole)) throw new AppException("This endpoint only allows rotating roles between Technician and FacilityManager.", 400);`.  
     *Nguồn:* `UserService.cs:126-131`.

---

## 5. Acceptance Criteria (Tiêu chí Chấp nhận)

### 5.1. Nhóm Xác thực (AC-AUTH-xx)

#### AC-AUTH-01: Đăng nhập thành công với tài khoản hợp lệ
- **Given:** Người dùng có tài khoản hợp lệ trong hệ thống với trạng thái `IsActive = true`.
- **When:** Người dùng gửi `POST /api/auth/login` với `username` và `password` chính xác.
- **Then:** Hệ thống trả về mã `200 OK`, body chứa JWT `token`, `role`, `userId`, `username`. Token có thời hạn 480 phút và chứa đầy đủ các claims chuẩn.
- **Nguồn:** `REQ-01`, `NFR-01`, `NFR-02`, `DEC-06`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.CreateAssetApiTests.LoginAsync` (phương thức helper đăng nhập tích hợp).

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
- **When:** Gateway gửi `POST /api/iot/ingest` kèm header `X-Api-Key: DEV_ONLY_IOT_GATEWAY_KEY` với dữ liệu của sensor đã mapping.
- **Then:** Hệ thống xác thực thành công và lưu dữ liệu, phản hồi mã `201 Created`.
- **Nguồn:** `REQ-28`, `NFR-02`, `DEC-04`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_MappedDevice_Returns201_AndPersists`

#### AC-AUTH-06: IoT Gateway gửi dữ liệu với API Key sai hoặc bị thiếu
- **Given:** Client không truyền header `X-Api-Key` hoặc truyền sai API Key.
- **When:** Client gửi `POST /api/iot/ingest`.
- **Then:** Hệ thống chặn request tại filter `IotGatewayAuthorize` và trả về mã `401 Unauthorized` kèm `{ "error": "Invalid or missing gateway API key." }`.
- **Nguồn:** `NFR-02`, `SEC-05`.
- **Test tự động:** `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_BadApiKey_Returns401`

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

## 6. Bảng truy vết (Traceability Matrix)

Bảng ánh xạ toàn diện từ Yêu cầu / Quy tắc nghiệp vụ (REQ / NFR / BR / DEC) → Tiêu chí chấp nhận (AC) → API Endpoint → Tên hàm Test tự động trong mã nguồn.

| Yêu cầu / BR nguồn | Mã AC | Endpoint thực tế | Method | Tên Test tự động trong Code |
|---|---|---|---|---|
| REQ-01, NFR-01, DEC-06 | AC-AUTH-01 | `/api/auth/login` | POST | `SmartMaintenance.Tests.Integration.CreateAssetApiTests.LoginAsync` |
| REQ-01, NFR-02 | AC-AUTH-02 | `/api/auth/login` | POST | **THIẾU TEST** |
| NFR-02, SEC-04 | AC-AUTH-03 | `/api/auth/logout` | POST | **THIẾU TEST** |
| NFR-01, NFR-02, SEC-01 | AC-AUTH-04 | `/api/assets` | POST | `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_Anonymous_Returns401` |
| NFR-01, NFR-02, SEC-01 | AC-AUTH-04 | `/api/requests` | POST | `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_Anonymous_Returns401` |
| NFR-01, NFR-02, SEC-01 | AC-AUTH-04 | `/api/work-orders` | POST | `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Anonymous_Returns401` |
| REQ-28, NFR-02, DEC-04 | AC-AUTH-05 | `/api/iot/ingest` | POST | `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_MappedDevice_Returns201_AndPersists` |
| NFR-02, SEC-05 | AC-AUTH-06 | `/api/iot/ingest` | POST | `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_BadApiKey_Returns401` |
| REQ-04, NFR-01 | AC-AUTHZ-01 | `/api/requests` | POST | `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_ValidRequester_Returns201_Submitted` |
| REQ-04, NFR-01 | AC-AUTHZ-01 | `/api/requests` | POST | `SmartMaintenance.Tests.Requests.RequestServiceTests.Create_Valid_PersistsSubmitted_WithJwtRequesterId` |
| REQ-04, NFR-01 | AC-AUTHZ-02 | `/api/requests` | POST | `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_FacilityManager_Returns403` |
| REQ-05, NFR-01 | AC-AUTHZ-03 | `/api/requests` | GET | **THIẾU TEST** |
| REQ-05, NFR-01 | AC-AUTHZ-04 | `/api/requests/{id}` | GET | **THIẾU TEST** |
| REQ-14, REQ-15, BR-06 | AC-AUTHZ-05 | `/api/work-orders` | POST | `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_ValidManager_Returns201_Assigned` |
| REQ-14, BR-08 | AC-AUTHZ-05 | `/api/work-orders` | POST | `SmartMaintenance.Tests.WorkOrders.WorkOrderServiceTests.Create_Valid_PersistsAssigned` |
| REQ-14, NFR-01 | AC-AUTHZ-06 | `/api/work-orders` | POST | `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Requester_Returns403` |
| REQ-17, BR-07 | AC-AUTHZ-07 | `/api/work-orders/{id}` | GET | **THIẾU TEST** |
| REQ-17, REQ-20, BR-07 | AC-AUTHZ-08 | `/api/work-orders/{id}` | GET/PATCH | **THIẾU TEST** |
| BR-07, NFR-01 | AC-AUTHZ-09 | `/api/work-orders/{id}` | PATCH | **THIẾU TEST** |
| REQ-21, BR-07 | AC-AUTHZ-10 | `/api/work-orders/{id}` | PATCH | **THIẾU TEST** |
| REQ-06, NFR-01 | AC-AUTHZ-11 | `/api/assets` | POST | `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_ValidFacilityManager_Returns201_AndPersists` |
| REQ-06, NFR-01 | AC-AUTHZ-12 | `/api/assets` | POST | `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_RequesterToken_Returns403` |
| REQ-09, NFR-01 | AC-AUTHZ-13 | `/api/assets/{id}/iot-data` | GET | `SmartMaintenance.Tests.Integration.IotApiTests.GetIotData_Requester_Returns403` |
| REQ-11, REQ-19 | AC-AUTHZ-14 | `/api/assets/{id}/prediction` | GET | `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Requester_Returns403` |
| REQ-19, NFR-01 | AC-AUTHZ-15 | `/api/assets/{id}/prediction` | GET | `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Technician_Allowed` |
| REQ-24, SEC-06 | AC-AUTHZ-16 | `/api/users/{id}` | PUT | `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminTarget_Throws403_QT4` |
| REQ-24, SEC-06 | AC-AUTHZ-16 | `/api/users/{id}` | PUT | `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminSelfDeactivate_Throws403_QT4` |
| REQ-24, SEC-06 | AC-AUTHZ-17 | `/api/users/{id}` | PUT | `SmartMaintenance.Tests.Users.UserServiceTests.Update_AssignAdmin_Throws400_QT3` |
| REQ-24, SEC-06 | AC-AUTHZ-18 | `/api/users/{id}` | PUT | `SmartMaintenance.Tests.Users.UserServiceTests.Update_Requester_Throws400_QT2` |
| REQ-24, REQ-25 | AC-AUTHZ-19 | `/api/users/{id}` | PUT | `SmartMaintenance.Tests.Users.UserServiceTests.Update_TechnicianToFacilityManager_Ok_QT1` |
| REQ-24, REQ-25 | AC-AUTHZ-19 | `/api/users/{id}` | PUT | `SmartMaintenance.Tests.Users.UserServiceTests.Update_FacilityManagerToTechnician_Ok_QT1` |
| REQ-25, NFR-01 | AC-AUTHZ-20 | `/api/roles/permissions` | GET | **THIẾU TEST** |

---

## 7. Điểm lệch cần BA chốt (Discrepancy Analysis)

Qua quá trình rà soát đối chiếu chéo giữa Requirements, Business Rules, Specs, Mã nguồn Backend và Mã nguồn Frontend, ghi nhận **5 điểm lệch trọng yếu** sau đây:

### Điểm lệch 1: Đặc tả "Admin thay đổi quyền của Role" mâu thuẫn với Kiến trúc RBAC cố định
- **Hiện trạng tài liệu:** Trong `docs/03-product/acceptance-criteria.md` (dòng 137-142), tiêu chí `AC-US-01-04-02 — Cập nhật quyền` mô tả: *"Given: Admin có quyền quản lý Role. When: Admin thay đổi quyền của một Role và lưu. Then: Hệ thống lưu cấu hình quyền mới."*
- **Thực tế trong Spec và Code:**
  - `docs/05-technical/specs/US-01-04-Spec.md` (dòng 69) khẳng định: *"Chính sách phân quyền (permission matrix) là cố định trong MVP; Admin chỉ có thể gán Role cho User, không tùy chỉnh từng quyền riêng lẻ."*
  - `src/backend/SmartMaintenance.Api/Controllers/RolesController.cs` (dòng 11-24) chỉ cung cấp duy nhất endpoint đọc `GET /api/roles/permissions` trả về biến tĩnh `PermissionsByRole`, hoàn toàn **không có API cập nhật hay lưu trữ quyền**.
- **Đề xuất BA xử lý:** Điều chỉnh `AC-US-01-04-02` trong `acceptance-criteria.md` thành tiêu chí *"Xem ma trận phân quyền tĩnh"* hoặc loại bỏ action sửa quyền của Role, giữ đúng tinh thần RBAC tĩnh cho MVP theo ràng buộc `CON-03`.

### Điểm lệch 2: Bảng quyền mô tả trong RolesController không khớp với [Authorize] thực tế của Controllers
- **Hiện trạng Code:** Trong `RolesController.cs` (dòng 12-18), ma trận quyền tĩnh được khai báo như sau:
  - `Requester`: `["ViewAsset", "CreateRequest", "TrackRequest"]`
  - `Technician`: `["ViewWorkOrder", "UpdateWorkOrder", "ViewIoTAlert"]`
  - `FacilityManager`: `["ManageAsset", "ManageRequest", "ManageWorkOrder", "ViewIoTData"]`
  - `Admin`: `["ManageUsers", "ManageRoles", "ManageIoTMapping"]`
- **Sự không khớp với các Controller khác:**
  1. `Technician`: Trong `AssetsController.cs:44, 53, 81, 90`, Technician được phép gọi `GET /api/assets`, `GET /api/assets/{id}`, `GET /api/assets/{id}/iot-data`, `GET /api/assets/{id}/prediction`. Các quyền `ViewAsset`, `ViewIoTData`, `ViewPrediction` này hoàn toàn thiếu trong danh sách của Technician tại `RolesController`.
  2. `Admin`: Trong `AssetsController.cs:44, 53`, Admin được phép gọi `GET /api/assets` và `GET /api/assets/{id}`, nhưng `RolesController` không gán quyền `ViewAsset` cho Admin.
  3. `Requester`: Được khai báo có quyền `ViewAsset`, nhưng ở Frontend (file `App.tsx:50`), route `/assets` bị chặn không cho Requester truy cập.
- **Đề xuất BA xử lý:** Cập nhật lại mảng `PermissionsByRole` trong `RolesController.cs` để phản ánh đúng tập hợp các API mà các Role thực sự được phép gọi trên Backend.

### Điểm lệch 3: Yêu cầu "Requester chỉ xem Asset thuộc phòng của mình" (REQ-02, BR-04) chưa được thực thi ở Backend
- **Hiện trạng tài liệu:** `REQ-02`, `BR-04` và `US-01-02-Spec.md` đều yêu cầu: *"Requester chỉ được xem Asset thuộc phòng/khu vực mình được phép sử dụng."*
- **Thực tế trong Code:**
  - `AssetsController.cs:44` đặt `[Authorize(Roles = UserRoles.Requester + ...)]`.
  - Phương thức `AssetService.ListAsync(string? location, ...)` tại `src/backend/SmartMaintenance.Application/Assets/AssetService.cs:48` chỉ lọc theo query param `location` nếu client truyền lên, hoàn toàn **không nhận tham số `userId` hay `role`**.
  - Thực thể `User` trong database không có trường `Location` hoặc bảng liên kết `UserLocations` để biết Requester phụ trách phòng nào.
  - Phía Frontend (`src/frontend/src/App.tsx:50-53`) xử lý bằng cách chặn hẳn vai trò Requester truy cập vào trang `/assets`:
    `<Route element={<RequireRoles roles={["FacilityManager", "Technician", "Admin"]} />}>`
- **Đề xuất BA xử lý:** Chốt lại yêu cầu cho MVP: Chấp nhận cho Requester xem toàn bộ danh mục Asset công cộng để có thể chọn báo sự cố tại `POST /api/requests`, hoặc bổ sung thuộc tính phân quyền phòng/khu vực cho Requester trong CSDL.

### Điểm lệch 4: Route Frontend thiếu bọc RequireRoles (Hở Deep-link - BUG-011, BUG-013)
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
- **Hệ quả:** Bất kỳ người dùng nào sau khi đăng nhập (kể cả Requester) đều có thể gõ trực tiếp URL `/admin/users`, `/admin/iot`, `/predictions` trên trình duyệt để hiển thị giao diện trang quản trị. Mặc dù Backend vẫn thực thi chặn gọi API (`403 Forbidden`), nhưng giao diện và cấu trúc chức năng đặc quyền đã bị lộ ra ngoài. Vấn đề này đã được QA ghi nhận tại `docs/06-test/security-nfr.md` (SEC-09: Fail) và `docs/06-test/code-review.md` (BUG-011, BUG-013).
- **Đề xuất BA xử lý:** Đề xuất đội ngũ Frontend cập nhật `App.tsx`:
  - Bọc các route `/admin`, `/admin/users`, `/admin/iot` trong `<RequireRoles roles={["Admin"]} />`.
  - Bọc route `/predictions` trong `<RequireRoles roles={["FacilityManager"]} />`.
  - Bọc route `/alerts` và `/work-orders` trong `<RequireRoles roles={["FacilityManager", "Technician"]} />`.

### Điểm lệch 5: Thiếu cơ chế thu hồi phiên đăng nhập khi Admin đổi Role hoặc khóa tài khoản
- **Hiện trạng Code:** Trong `src/backend/SmartMaintenance.Application/Users/UserService.cs:141-144`, khi Admin cập nhật Role hoặc đặt `IsActive = false`, code chỉ lưu database.
- **Hệ quả:** Người dùng bị đổi quyền hoặc bị khóa tài khoản vẫn có thể tiếp tục sử dụng Token cũ (có thời hạn lên tới 8 tiếng) để thao tác các API theo Role cũ cho đến khi token hết hạn.
- **Đề xuất BA xử lý:** Đề xuất bổ sung cơ chế kiểm tra `user.IsActive` hoặc tích hợp cơ chế Revoke toàn bộ token của User khi tài khoản bị thay đổi trạng thái hoặc quyền hạn.

---

## 8. Câu hỏi mở (UNKNOWN)

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
Tất cả các tên hàm test được viện dẫn trong tài liệu này đều tồn tại 100% trong mã nguồn kiểm thử và được xác nhận qua lệnh tìm kiếm:
1. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.LoginAsync` (Helper)
2. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_Anonymous_Returns401`
3. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_RequesterToken_Returns403`
4. `SmartMaintenance.Tests.Integration.CreateAssetApiTests.PostAssets_ValidFacilityManager_Returns201_AndPersists`
5. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_Anonymous_Returns401`
6. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_FacilityManager_Returns403`
7. `SmartMaintenance.Tests.Integration.CreateRequestApiTests.PostRequest_ValidRequester_Returns201_Submitted`
8. `SmartMaintenance.Tests.Requests.RequestServiceTests.Create_Valid_PersistsSubmitted_WithJwtRequesterId`
9. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Anonymous_Returns401`
10. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_Requester_Returns403`
11. `SmartMaintenance.Tests.Integration.CreateWorkOrderApiTests.PostWorkOrder_ValidManager_Returns201_Assigned`
12. `SmartMaintenance.Tests.WorkOrders.WorkOrderServiceTests.Create_Valid_PersistsAssigned`
13. `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_BadApiKey_Returns401`
14. `SmartMaintenance.Tests.Integration.IotApiTests.Ingest_MappedDevice_Returns201_AndPersists`
15. `SmartMaintenance.Tests.Integration.IotApiTests.GetIotData_Requester_Returns403`
16. `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Requester_Returns403`
17. `SmartMaintenance.Tests.Integration.PredictionApiTests.GetPrediction_Technician_Allowed`
18. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminTarget_Throws403_QT4`
19. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AdminSelfDeactivate_Throws403_QT4`
20. `SmartMaintenance.Tests.Users.UserServiceTests.Update_AssignAdmin_Throws400_QT3`
21. `SmartMaintenance.Tests.Users.UserServiceTests.Update_Requester_Throws400_QT2`
22. `SmartMaintenance.Tests.Users.UserServiceTests.Update_TechnicianToFacilityManager_Ok_QT1`
23. `SmartMaintenance.Tests.Users.UserServiceTests.Update_FacilityManagerToTechnician_Ok_QT1`

### 9.3. Kết quả chạy kiểm thử thực tế trên hệ thống
Lệnh thực thi trên môi trường local:
```bash
dotnet test src/backend/SmartMaintenance.Tests --filter "FullyQualifiedName~403|FullyQualifiedName~401"
```

Output thực tế nhận được:
```text
Test run for C:\Users\Admin\Downloads\MIS3032_1_Group10-main\MIS3032_1_Group10-main\Smart-Maintenance-Facility-Management\src\backend\SmartMaintenance.Tests\bin\Debug\net9.0\SmartMaintenance.Tests.dll (.NETCoreApp,Version=v9.0)
VSTest version 17.14.1 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    11, Skipped:     0, Total:    11, Duration: 8 s - SmartMaintenance.Tests.dll (net9.0)
```

Chi tiết 11 test case chạy thành công:
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

| Chỉ số đánh giá | Số lượng | Ghi chú |
|---|---|---|
| **Tổng số API Endpoints** | **28** | Toàn bộ 28 action routes trong 10 Controllers backend |
| **Số Endpoint có Test 401/403 tự động** | **6** | `POST /api/assets`, `POST /api/requests`, `POST /api/work-orders`, `GET /api/assets/{id}/iot-data`, `GET /api/assets/{id}/prediction`, `POST /api/iot/ingest` |
| **Số Endpoint/Hành vi "THIẾU TEST"** | **22** | Chưa có kiểm thử tự động 401/403 cho các endpoint còn lại (User management integration, Work order ownership patch, Request detail ownership, Roles controller) |
| **Số Điểm lệch cần BA chốt** | **5** | Gồm: Sửa quyền Role, Mâu thuẫn bảng Permissions tĩnh, Phạm vi Asset của Requester, Lỗ hổng Deep-link Frontend, và Thu hồi phiên Token khi đổi Role |

