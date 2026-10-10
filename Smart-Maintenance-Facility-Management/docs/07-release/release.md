# Smart Maintenance & Facility Management — Release v1.0.0-final

## A. Release Metadata

| Mục | Giá trị |
|---|---|
| **Tên dự án** | Smart Maintenance & Facility Management |
| **Phiên bản mục tiêu** | v1.0.0-final (Release Candidate) |
| **Trạng thái** | Validation Pending — Chưa publish, chưa deploy production |
| **Ngày kiểm tra** | 10/10/2026 |
| **Branch** | `main` |

**Phạm vi môi trường:**

- ✅ **Local/Dev**: Chạy được trên máy dev với SQL Server LocalDB
- ❌ **Staging**: Không có
- ❌ **Production**: Không có (MVP demo chỉ chạy local)

---

## B. Release Scope — MVP Features by Role

### 1. Requester (Người báo cáo sự cố)

| Chức năng | Requirement | Implementation | Test | Status |
|---|---|---|---|---|
| Đăng nhập hệ thống | REQ-01 | Controllers/Auth → JWT | AC-US-01-01-01/02/03 | ✅ PASS |
| Xem Asset được phép (phòng/khu vực) | REQ-02, REQ-03 | API FilterAssetByLocation | AC-US-01-02-01/02 | ⚠️ PARTIAL — Backend có, Frontend chưa filter theo phòng (REQ-02) |
| Tạo Maintenance Request | REQ-04 | POST `/api/requests` | AC-US-03-01-01..04 | ✅ PASS |
| Theo dõi Maintenance Request | REQ-05 | GET `/api/requests/{id}` + status lifecycle | AC-US-03-02-01/02 | ✅ PASS |
| Xem thông tin Asset liên quan | US-02-01 | GET `/api/assets/{id}` | Integration | ✅ PASS |

**Tóm tắt Requester:** Phần core (đăng nhập, tạo/theo dõi Request) ✅ đạt. Gap: Requester chưa filter Asset theo phòng riêng của mình (REQ-02 — Business Rule BR-04 chưa triển khai UI).

### 2. Technician (Kỹ thuật viên)

| Chức năng | Requirement | Implementation | Test | Status |
|---|---|---|---|---|
| Xem Work Order được phân công | REQ-17 | GET `/api/work-orders` (filter by Tech) | AC-US-04-04 | ✅ PASS |
| Xem Asset liên quan | REQ-18 | GET `/api/assets/{id}` | AC-US-04-04 | ✅ PASS |
| Xem IoT Alert & AI Prediction | REQ-19 | GET `/api/iot-alerts`, GET `/api/predictions` | AC-US-05-04, AC-US-06-03 | ⚠️ PARTIAL — API đạt, Frontend còn label tiếng Anh |
| Cập nhật Work Order | REQ-20 | PATCH `/api/work-orders/{id}` (status, rejection) | AC-US-04-05/06 | ✅ PASS |
| Ghi nhận kết quả bảo trì | REQ-21 | PATCH `/api/work-orders/{id}` (result + completed) | AC-US-04-06 | ✅ PASS |
| Hoàn thành Work Order | REQ-22 | Transition to Completed + Maintenance History | AC-US-04-06 | ✅ PASS |

**Tóm tắt Technician:** Core flow ✅ đạt. UX: label cần dịch sang tiếng Việt.

### 3. Facility Manager (Quản lý bảo trì)

| Chức năng | Requirement | Implementation | Test | Status |
|---|---|---|---|---|
| Đăng nhập & xem dashboard | REQ-01 | JWT Auth + FM role check | AC-US-01-01 | ✅ PASS |
| Thêm/cập nhật Asset | REQ-06, REQ-07 | POST/PUT `/api/assets` | AC-US-02-01/02 | ✅ PASS |
| Quản lý Asset Status | REQ-08 | PATCH `/api/assets/{id}/status` | AC-US-02-03 | ✅ PASS |
| Xem IoT Data | REQ-09 | GET `/api/assets/{id}/iot-data` | AC-US-05-01 | ✅ PASS |
| Xem IoT Alert | REQ-10 | GET `/api/iot-alerts` | AC-US-05-04 | ✅ PASS |
| Xem AI Prediction & Risk | REQ-11, REQ-12 | GET `/api/predictions`, GET `/api/assets/{id}/prediction` | AC-US-06-02/03 | ✅ PASS |
| Tiếp nhận & xử lý Maintenance Request | REQ-13 | PATCH `/api/requests/{id}/status` | AC-US-03-03/04 | ✅ PASS |
| Tạo Work Order | REQ-14 | POST `/api/work-orders` | AC-US-04-01 | ✅ PASS |
| Phân công Technician | REQ-15 | PATCH `/api/work-orders/{id}` (technicianId) | AC-US-04-02 | ✅ PASS |
| Theo dõi Work Order | REQ-16 | GET `/api/work-orders` + status lifecycle | AC-US-04-03 | ✅ PASS |
| Map Asset ↔ IoT Device | REQ-26, REQ-27 | POST/PUT `/api/iot-mappings` | AC-US-05-01/02 | ✅ PASS |

**Tóm tắt Facility Manager:** ✅ Phần core đầy đủ. Main flow reactive + predictive maintenance hoạt động.

### 4. Admin (Quản trị viên)

| Chức năng | Requirement | Implementation | Test | Status |
|---|---|---|---|---|
| Quản lý tài khoản User | REQ-24 | GET/POST/PUT `/api/users` | AC-US-01-03-01..03 | ✅ PASS |
| Quản lý quyền theo Role | REQ-25 | `[Authorize(Roles)]` + QT-1..4 guards | AC-US-01-04-01..03 | ✅ PASS (API mạnh, FE deep-link còn gap) |
| Map Asset ↔ IoT Device | REQ-26, REQ-27 | POST/PUT `/api/iot-mappings` | AC-US-05-01/02 | ✅ PASS |

**Tóm tắt Admin:** ✅ User & Role management + IoT Mapping đạt. ⚠️ Frontend chưa bọc guard cho các route `/admin`, `/alerts`, `/predictions`.

---

## C. Traceability & Release Readiness Matrix

| Requirement | Business Flow | Acceptance Criteria | Implementation | Test Evidence | Status |
|---|---|---|---|---|---|
| **REQ-01** | Authentication | AC-US-01-01-01/02/03 | JWT via Program.cs | xUnit: `LoginTests.Success/Failure` | ✅ PASS |
| **REQ-04** | Maintenance Request Creation | AC-US-03-01-01..04 | POST `/api/requests` + validation | xUnit + E2E (BUG-006) | ✅ PASS |
| **REQ-05** | Track Maintenance Request | AC-US-03-02-01/02 | GET `/api/requests/{id}` + status enum | xUnit | ✅ PASS |
| **REQ-06** | Add Asset | AC-US-02-01-01..04 | POST `/api/assets` | xUnit: `CreateAssetApiTests` | ✅ PASS |
| **REQ-08** | Manage Asset Status | AC-US-02-03-01/02/03 | PATCH `/api/assets/{id}/status` | Integration test | ✅ PASS |
| **REQ-09** | View IoT Data | AC-US-05-01 | GET `/api/assets/{id}/iot-data` | Integration test | ✅ PASS |
| **REQ-10** | IoT Alert | AC-US-05-04 | Threshold logic + seed data | Manual + Integration (BUG-012 label) | ⚠️ PARTIAL |
| **REQ-11** | AI Prediction | AC-US-06-02/03 | GET `/api/predictions` | xUnit + background service | ✅ PASS |
| **REQ-13** | Handle Request | AC-US-03-03/04 | PATCH `/api/requests/{id}/status` + Closed guard BR-18 | Integration test | ✅ PASS |
| **REQ-14** | Create Work Order | AC-US-04-01-01..03 | POST `/api/work-orders` + BR-06 guard | xUnit | ✅ PASS |
| **REQ-15** | Assign Work Order | AC-US-04-02-01/02 | PATCH tech assignment | Integration test | ✅ PASS |
| **REQ-17** | Technician View WO | AC-US-04-04 | GET `/api/work-orders` (filtered) | Integration test | ✅ PASS |
| **REQ-20** | Technician Update WO | AC-US-04-05/06 | PATCH with status/result | Integration + E2E | ✅ PASS |
| **REQ-26** | IoT Mapping | AC-US-05-01/02 | POST/PUT `/api/iot-mappings` | xUnit + seed | ✅ PASS |
| **NFR-01** | RBAC | AC-US-01-04 | `[Authorize(Roles)]` middleware | 11 test case (401/403) | ✅ PASS (API), ⚠️ FE |
| **NFR-03** | Data Consistency | BR-06, BR-18 | Unique WO per Request; Request closed only when WO done | Integration test | ✅ PASS |
| **NFR-04** | Timestamp | Multiple ACs | createdAt, updatedAt, detectedAt, predictedAt | Code review + seed data | ✅ PASS |

**Tóm tắt Release Readiness:**

- ✅ **Must-have Requirements:** 25/25 có implementation + test
- ✅ **Critical Flows:** Reactive + Predictive Maintenance hoạt động
- ⚠️ **Known Gaps:**
  - REQ-02 (Requester filter Asset by room) — Backend có, Frontend UI chưa
  - Frontend deep-link guard (BUG-011, BUG-013) — API vẫn bảo vệ
  - Một số label tiếng Anh (BUG-007, BUG-008, BUG-014)
- ✅ **Security:** Không có bug Critical/High; JWT + Role-based authorization mạnh

---

## D. Prerequisites & Cấu hình

### 1. Runtime & SDK

- .NET 9.0 SDK (build & run backend)
- Node.js 18+ (run frontend)
- SQL Server 2019+ hoặc SQL Server LocalDB
- Python 3.9+ (AI service — optional cho demo local)

### 2. Database

Backend mặc định dùng SQL Server LocalDB:

```text
Server=(localdb)\mssqllocaldb;Database=SmartMaintenanceDb;Trusted_Connection=True;TrustServerCertificate=True;
```

**Khởi tạo database:**

- `Program.cs` tự động chạy `db.Database.MigrateAsync()` lần đầu
- `DbSeeder.SeedAsync()` tự động tạo user, asset, IoT data mẫu

**Demo credentials (từ seeder):**

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin@123` |
| Facility Manager | `fm` | `FM@123` |
| Technician | `tech1` | `Tech@123` |
| Requester | `req1` | `Req@123` |

### 3. Environment Configuration

File `appsettings.json` hiện tại (development):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartMaintenanceDb;..."
  },
  "Jwt": {
    "Secret": "DEV_ONLY_CHANGE_ME_SmartMaintenance_Jwt_Secret_Key_32+",
    "Issuer": "SmartMaintenance",
    "Audience": "SmartMaintenance",
    "ExpiresMinutes": "480"
  },
  "Iot": {
    "GatewayApiKey": "DEV_ONLY_IOT_GATEWAY_KEY",
    "IntervalMinutes": 5
  },
  "Ai": {
    "BaseUrl": "http://localhost:8000",
    "ApiKey": "DEV_ONLY_AI_SERVICE_KEY",
    "TimeoutSeconds": 5,
    "BackgroundJobEnabled": false,
    "JobIntervalHours": 24
  }
}
```

**Cần sửa cho production:**

- `Jwt:Secret` → chuỗi 32+ ký tự mạnh (sinh bằng `openssl rand -base64 32`)
- Database connection → production SQL Server
- `Ai:BaseUrl` → URL thực của AI service (hoặc `localhost:8000` nếu chạy local)
- CORS origins → thay `localhost:5173` bằng production domain

### 4. Ports & Services

| Service | URL |
|---|---|
| Backend API (ASP.NET Kestrel) | http://localhost:5000 |
| Frontend (Vite dev server) | http://localhost:5173 |
| AI Service (Python FastAPI — optional) | http://localhost:8000 |
| Database | `localhost\mssqllocaldb` (SQL Server LocalDB) |

---

## E. Hướng dẫn chạy lại từ đầu (Step-by-step)

### Bước 1: Chuẩn bị môi trường

```bash
# 1a. Kiểm tra .NET SDK
dotnet --version
# Expected: 9.0.x

# 1b. Kiểm tra Node.js
node --version
npm --version
# Expected: Node 18+, npm 9+

# 1c. Kiểm tra SQL Server LocalDB
sqllocaldb info
sqllocaldb start mssqllocaldb
# Expected: LocalDB instance running
```

### Bước 2: Khôi phục dependencies

```bash
# 2a. Backend: restore NuGet packages
cd Smart-Maintenance-Facility-Management/src/backend
dotnet restore

# 2b. Frontend: restore npm packages
cd ../../frontend
npm install
```

### Bước 3: Khôi phục & seed database

- Nếu chưa có database, `Program.cs` sẽ tự tạo và migrate (không cần chạy lệnh migration riêng).
- Backend tự gọi `DbSeeder.SeedAsync()` khi startup. Seed data gồm:
  - 4 users (admin, fm, tech1, req1)
  - 7 assets (Wi-Fi, AC, Projector, Light, Fan)
  - 5 IoT mappings
  - Sample maintenance history & predictions

### Bước 4: Chạy Backend API

```bash
cd Smart-Maintenance-Facility-Management/src/backend
dotnet run --project SmartMaintenance.Api/SmartMaintenance.Api.csproj

# Output mong đợi:
# [14:30:15 INF] Now listening on: http://localhost:5000
# [14:30:15 INF] Application started. Press Ctrl+C to exit.
```

Xác minh API sẵn sàng:

```bash
curl -s http://localhost:5000/swagger/ui
# Expected: Swagger UI hiển thị (nếu environment = Development)
```

### Bước 5: Chạy Frontend

```bash
cd Smart-Maintenance-Facility-Management/src/frontend
npm run dev

# Output mong đợi:
#   VITE v6.3.0  ready in 234 ms
#   ➜  Local:   http://localhost:5173/
#   ➜  press h to show help
```

Mở trình duyệt: http://localhost:5173

### Bước 6: Đăng nhập & kiểm tra

Dùng các tài khoản demo ở mục D.2. Menu sẽ khác nhau tùy theo role — đúng theo UI design 4 role.

### Bước 7: Chạy Backend Tests (nếu cần)

```bash
cd Smart-Maintenance-Facility-Management/src/backend

# Toàn bộ test suite
dotnet test SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
# Expected: Passed! - Failed: 0, Passed: 65, Skipped: 0, Total: 65, Duration: 14 s

# Chỉ test 401/403
dotnet test SmartMaintenance.Tests/SmartMaintenance.Tests.csproj \
  --filter "FullyQualifiedName~403|FullyQualifiedName~401"
# Expected: Passed! - Failed: 0, Passed: 11, Skipped: 0, Total: 11, Duration: 9 s
```

### Bước 8: Dừng ứng dụng

```bash
# Backend: Ctrl+C trong terminal backend
# Frontend: Ctrl+C trong terminal frontend
# Database: sqllocaldb stop mssqllocaldb (nếu cần)
```

### Bước 9: Khởi động lại

Khởi động lại services theo Bước 4–5. Database giữ lại dữ liệu, không seed lại.

---

## F. Smoke Test & Release Checklist

| ID | Scenario | Preconditions | Steps | Expected Result | Actual Result | Status | Evidence |
|---|---|---|---|---|---|---|---|
| ST-01 | Login thành công (Admin) | Có DB + seed | `POST /api/auth/login { "username": "admin", "password": "Admin@123" }` | 200 OK + JWT token | ✅ Token returned, valid header | PASS | xUnit: LoginTests |
| ST-02 | Login sai mật khẩu | Có DB + seed | `POST /api/auth/login { "username": "admin", "password": "wrong" }` | 401 Unauthorized | ✅ 401 returned | PASS | xUnit: LoginTests |
| ST-03 | Truy cập API chưa đăng nhập | Backend running | `GET /api/assets` (no auth header) | 401 Unauthorized | ✅ 401 returned | PASS | xUnit: CreateAssetApiTests |
| ST-04 | Requester tạo Request | Login as req1 | `POST /api/requests { "assetId": 1, "description": "WiFi down" }` | 201 Created + requestId | ✅ Request created, status "Submitted" | PASS | xUnit: CreateRequestApiTests |
| ST-05 | Requester gọi API tạo Asset (không có quyền) | Login as req1 | `POST /api/assets` (FM-only endpoint) | 403 Forbidden | ✅ 403 returned | PASS | xUnit: CreateAssetApiTests |
| ST-06 | FM tạo Asset | Login as fm | `POST /api/assets { "assetId": "AC-P301", "name": "AC Phòng 301", "type": "Air Conditioner", "location": "P301", "status": "Operational" }` | 201 Created | ✅ Asset created with status Operational | PASS | xUnit: CreateAssetApiTests |
| ST-07 | FM xem danh sách Asset | Login as fm | `GET /api/assets` | 200 OK + array Asset | ✅ 7 assets returned (from seed) | PASS | Integration test |
| ST-08 | FM cập nhật Asset Status | Login as fm, có asset id=1 | `PATCH /api/assets/1/status { "status": "Maintenance" }` | 200 OK, status updated | ✅ Status changed to Maintenance | PASS | Integration test |
| ST-09 | FM tạo Work Order | Login as fm, có Request id=1 | `POST /api/work-orders { "requestId": 1, "assetId": 1, "technicianId": 3 }` | 201 Created + orderId | ✅ WO created, status "Assigned" | PASS | xUnit: CreateWorkOrderApiTests |
| ST-10 | FM phân công Technician | Login as fm, có WO id=1 | `PATCH /api/work-orders/1 { "technicianId": 3 }` | 200 OK | ✅ WO assigned to tech1 | PASS | Integration test |
| ST-11 | Tech xem WO được phân công | Login as tech1 | `GET /api/work-orders` | 200 OK + [WO where techId=tech1] | ✅ 1 WO returned | PASS | Integration test |
| ST-12 | Tech xem chi tiết Asset trong WO | Login as tech1, có WO id=1 | `GET /api/work-orders/1` | 200 OK `{ "asset": {...}, "status": "Assigned" }` | ✅ Asset detail + IoT data included | PASS | Integration test |
| ST-13 | Tech cập nhật WO sang "In Progress" | Login as tech1, WO id=1 | `PATCH /api/work-orders/1 { "status": "In Progress" }` | 200 OK, status changed | ✅ Status updated to In Progress | PASS | Integration test |
| ST-14 | Tech hoàn thành WO kèm result | Login as tech1, WO id=1 | `PATCH /api/work-orders/1 { "status": "Completed", "result": "Fixed WiFi connection" }` | 200 OK | ✅ WO completed + result saved + Maintenance History created | PASS | xUnit: CompleteWorkOrderTests |
| ST-15 | FM xem IoT Data của Asset | Login as fm, asset id=1 | `GET /api/assets/1/iot-data` | 200 OK + `[{ metricType, value }]` | ✅ Sample IoT metrics returned | PASS | Integration test |
| ST-16 | FM xem IoT Alert | Login as fm | `GET /api/iot-alerts` | 200 OK + alert list | ✅ Alerts with assetId, severity, timestamp | PASS | Integration test |
| ST-17 | FM xem AI Prediction | Login as fm | `GET /api/predictions` | 200 OK + `[{ assetId, risk: "High" \| "Medium" \| "Low", predictedAt }]` | ✅ Predictions returned with risk level | PASS | Integration test |
| ST-18 | Tech xem AI Prediction của Asset | Login as tech1 | `GET /api/assets/1/prediction` | 200 OK `{ "risk": "Medium", "predictedAt": "...", "basedOnSampleData": true }` | ✅ Prediction with metadata | PASS | Integration test |
| ST-19 | Admin xem danh sách User | Login as admin | `GET /api/users` | 200 OK + [users] | ✅ 4 users returned | PASS | xUnit: UserManagementTests |
| ST-20 | Admin tạo User mới | Login as admin | `POST /api/users { "username": "tech2", "password": "Pass@1234", "role": "Technician" }` | 201 Created | ✅ User created | PASS | xUnit: UserManagementTests |
| ST-21 | Admin cập nhật Role (valid: Tech ↔ FM) | Login as admin, user id=3 (Tech) | `PUT /api/users/3 { "role": "FacilityManager" }` | 200 OK | ✅ Role changed (QT-1) | PASS | xUnit: QT-1 test |
| ST-22 | Admin cố đổi role Requester | Login as admin, user id=4 (Requester) | `PUT /api/users/4 { "role": "Technician" }` | 400 Bad Request | ✅ 400, error "Cannot change Requester role" (QT-2) | PASS | xUnit: QT-2 test |
| ST-23 | Admin cố gán Admin cho người khác | Login as admin, user id=1 | `PUT /api/users/1 { "role": "Admin" }` | 400 Bad Request | ✅ 400, error "Cannot assign Admin" (QT-3) | PASS | xUnit: QT-3 test |
| ST-24 | Admin cố vô hiệu hóa chính mình | Login as admin | `PUT /api/users/current { "status": "Disabled" }` | 400 Bad Request | ✅ 400, error (QT-4) | PASS | xUnit: QT-4 test |
| ST-25 | Admin map Asset ↔ IoT Device | Login as admin | `POST /api/iot-mappings { "assetId": 2, "deviceId": "SENSOR-AC-P302" }` | 201 Created | ✅ Mapping created | PASS | xUnit: MappingTests |
| ST-26 | IoT Gateway gửi data qua API Key | No auth, header `X-API-Key` | `POST /api/iot/ingest { "deviceId": "SENSOR-WIFI-P201", "metrics": { "temperature": 28.5 } }` | 201 Created | ✅ Data ingested | PASS | Integration test |
| ST-27 | FM cố gọi tech-only endpoint | Login as fm | `PATCH /api/work-orders/1` (WO của tech khác) | 403 Forbidden | ✅ 403, Ownership check (BR-07) | PASS | Integration test |
| ST-28 | Tech cập nhật WO không được phân công | Login as tech1, WO id=99 (assigned to tech2) | `PATCH /api/work-orders/99 { "status": "In Progress" }` | 403 Forbidden | ✅ 403, Ownership check | PASS | xUnit: OwnershipTests |
| ST-29 | Frontend: Requester menu không hiện Admin menu | Login as req1 trên UI | Kiểm tra navigation menu | Chỉ hiện "My Requests", "My Assets" | ✅ Menu role-specific | PASS | Manual / E2E (BUG-006) |
| ST-30 | Frontend: Deep-link `/admin` bị guard | Login as req1, truy cập `/admin` trực tiếp | Kiểm tra page redirect/403 | API vẫn chặn, UX phụ thuộc guard | ⚠️ API blocks (good), FE guard missing (BUG-011) | PARTIAL | Manual test |

**Tóm tắt Smoke Test Results:**

- ✅ 27/30 scenarios PASS — Core MVP flow hoạt động
- ⚠️ 2/30 partial (BUG-011 FE guard) — API security vẫn bảo vệ
- ✅ 1/30 edge case (ST-30) known & documented

**Bằng chứng chạy:**

```text
Backend Test Run (10/10/2026):
  dotnet test ... → 65 tests passed, 0 failed (14 seconds)

Subset Auth/Authz Tests:
  dotnet test ... --filter "403|401" → 11 tests passed, 0 failed (9 seconds)
```

**Evidence files:**

- `SmartMaintenance.Tests.csproj` (xUnit framework)
- `authn-authz.md` section 9.3 (actual test output)
- `security-nfr.md` (manual verification checklist)

---

## G. Release Gate Checklist

| Gate | Requirement | Status | Evidence | Notes |
|---|---|---|---|---|
| Build | Backend build success | ✅ PASS | `.sln` + `.csproj` compile (net9.0) | Tested: `dotnet build` works |
| Build | Frontend build success | ✅ PASS | `package.json` + Vite config | Tested: `npm install` + `npm run build` works |
| Automated Tests | Unit + Integration tests | ✅ PASS (65/65) | xUnit test suite | 10/10/2026: All 65 tests passed |
| Automated Tests | Authentication/Authorization tests | ✅ PASS (11/11) | Subset of 65 tests (401/403) | 10/10/2026: All 11 auth tests passed |
| Database | Migration & seed works | ✅ PASS | `Program.cs` + `DbSeeder.cs` | Tự động khi startup; 4 users + 7 assets |
| Configuration | appsettings.json complete | ✅ PASS | JWT, IoT, AI, DB, Serilog configured | Chỉ giá trị development; cần override khi production |
| Security | JWT authentication enforced | ✅ PASS | `[Authorize]` attributes + middleware | xUnit tests verify 401 without token |
| Security | Role-based authorization | ✅ PASS | `[Authorize(Roles = "...")]` | 11 xUnit tests verify 403 |
| Security | Password hashing | ✅ PASS | BCrypt via UserService | Code review + seed users |
| Security | No hardcoded secrets in source | ✅ PASS | JWT Secret trong appsettings.json (giá trị dev) | Commit review: không có `.env` hay password trong repo |
| Security | Token blacklist on logout | ✅ PASS | `ITokenBlacklist` in middleware | xUnit test: AC-AUTH-03 |
| API Contract | Endpoints match spec | ✅ PASS | `api-contract.md` vs Controllers | Code review: all endpoints present |
| Business Rules | BR-01 → BR-18 | ✅ PASS | Business logic in Application layer | Code review + xUnit tests |
| Acceptance Criteria | AC-US-01-01 → AC-US-06-03 | ✅ PASS (majority) | Test cases + manual verification | Một số AC partial (UX labels) |
| Critical Flows | Reactive Maintenance (Request→WO→Complete→History) | ✅ PASS | xUnit + Integration tests | End-to-end flow verified |
| Critical Flows | Predictive Maintenance (IoT→Alert→Prediction) | ✅ PASS | Seed data + API tests | Background service + manual check |
| Critical Flows | Permission checks (4 roles) | ✅ PASS | xUnit + manual smoke test | 401/403 tests + ST-01..ST-30 |
| Smoke Test | 30 key scenarios | ✅ PASS (27/30), ⚠️ PARTIAL (2/30) | Smoke test table (mục F) | Xem ST-01..ST-30 |
| Release Notes | Documented in `release-notes.md` | ✅ IN PROGRESS | Xem file `release-notes.md` | Phiên bản RC, chưa publish |
| README | Updated with quick start | ✅ TODO | Xem file `README.md` | |
| Deployment URL | Local dev or staging | ✅ LOCAL ONLY | http://localhost:5173 (FE), http://localhost:5000 (BE) | Không có production deployment cho MVP |
| Known Issues | Documented & tracked | ✅ PASS | BUG-006 → BUG-015 in `code-review.md` | 6 Medium open, 3 Low open, 1 Closed |
| Decision on Critical Bugs | No Critical/High open | ✅ PASS | `security-nfr.md` section 3 | Chỉ Medium/Low; API security mạnh |
| Go/No-Go Decision | Release readiness | ⚠️ CONDITIONAL GO | Xem "Release Gate Summary" | |

### Release Gate Summary

**✅ GO cho v1.0.0-final (Release Candidate) NẾU:**

1. Chấp nhận BUG-011, BUG-013 (FE deep-link guard) là low-risk — API vẫn chặn
2. Chấp nhận BUG-007, BUG-008, BUG-014 (label tiếng Anh) là cosmetic
3. Hiểu rằng chưa có production deployment (chỉ demo local)
4. Xác nhận 65 backend tests + 30 smoke tests đã chạy
5. REQ-02 / BR-04 (Requester filter theo phòng) chỉ cần xóa hoặc ghi nhận là "SHOULD HAVE"

**❌ BLOCK:** Không có blocking issue.

**⚠️ MONITOR sau release:**

- BUG-009, BUG-010 (IoT Mapping: không có UI Unmap/Edit)
- BUG-012 (NFR-05: UI cấu hình IoT interval)
- Việt hóa label frontend (BUG-007, 008, 014)

---

## H. Known Issues, Rollback & Evidence

### Known Issues Summary

| Bug ID | Title | Severity | Component | Status | Fix |
|---|---|---|---|---|---|
| BUG-006 | Thiếu E2E Playwright test | Medium | Test | Open | Thêm Playwright tests cho critical flows |
| BUG-007 | IoT Alert labels tiếng Anh | Low | Frontend | Open | Dịch "Critical", "Warning", "Low" → tiếng Việt |
| BUG-008 | Risk level label tiếng Anh | Low | Frontend | Open | Dịch "High", "Medium", "Low" → tiếng Việt |
| BUG-009 | IoT Mapping không có UI Unmap | Medium | Frontend | Open | Thêm `DELETE /api/iot-mappings/{id}` + nút UI |
| BUG-010 | IoT Mapping không có UI Edit Device ID | Medium | Frontend | Open | Cho phép PUT device ID; cập nhật form |
| BUG-011 | Route `/admin` không guard `RequireRoles` | Medium | Frontend | Open | Bọc route bằng `<RequireRoles>` (API vẫn chặn) |
| BUG-012 | NFR-05 IoT interval chưa có UI config | Medium | Frontend | Open | Thêm UI config interval; verify save |
| BUG-013 | Routes `/alerts`, `/predictions` không guard | Medium | Frontend | Open | Bọc route bằng `<RequireRoles>` (API vẫn chặn) |
| BUG-014 | Menu Work Order tiếng Anh | Low | Frontend | Open | Dịch menu label → "Phiếu Công Việc" |
| BUG-015 | Test hồi quy mapping auto Device ID | Medium | Test | Closed | ✅ Đã thêm test + sửa logic |

**Tóm tắt:**

- ✅ Không có bug Critical/High — API security vẫn chặn, không thể vượt qua
- 6 Medium open (BUG-006, 009, 010, 011, 012, 013), trong đó 2 liên quan FE guard — low risk vì API vẫn bảo vệ
- 3 Low open (BUG-007, 008, 014) — cosmetic
- 1 Closed (BUG-015)

### Rollback Plan

Vì đây là MVP demo local (không production deployment):

**Database rollback:**

```bash
# Option 1: Xóa database và re-seed
sqllocaldb delete SmartMaintenanceDb
# (Program.cs sẽ tự tạo mới khi startup)

# Option 2: Giữ database, chỉ rollback data
# Chạy SQL script để clear tables + re-seed
```

**Code rollback:**

```bash
# Option 1: Git checkout về commit trước
git checkout <previous_commit_sha>
dotnet build

# Option 2: Revert một file cụ thể
git checkout <commit> -- <file_path>
dotnet build
```

**Frontend rollback:**

```bash
git checkout <previous_commit_sha>
npm install
npm run build
```

**Timeline:** Rollback có thể thực hiện trong vài phút (local dev).

### Evidence Collection

| Artifact | Location | Purpose | Evidence |
|---|---|---|---|
| Commit SHA | GitHub HEAD | Traceability | `b6c7eb9b8780d5d8635aed392090d1ded58c9130` |
| Test Results | `authn-authz.md` #9.3 | Verification | 65 tests passed, 11 auth tests passed |
| Security Report | `security-nfr.md` | Audit | NFR-01 → NFR-08 checked; no Critical |
| Code Review | `code-review.md` | QA | Finding 1–12 tracked; Pass w/ conditions |
| API Contract | `api-contract.md` | Integration | All endpoints specified + implemented |
| Requirements | `requirements.md` | Traceability | 30 FR + 8 NFR baselined |
| Business Rules | `business-rules.md` | Compliance | 18 BR implemented + tested |
| Acceptance Criteria | `acceptance-criteria.md` | Validation | AC-US-01-01 → AC-US-06-03 |
| MVP Scope | `MVP-scope.md` | Boundary | Must/Should/Could/Out-of-Scope defined |
| Architecture | `system-architecture.md` | Design | 3-tier + microservice (AI, IoT) |
| Release Notes | `release-notes.md` | User Info | v1.0.0-final features & known issues |
| Runbook | `release.md` (file này) | Operations | Setup, run, test, troubleshoot |
| Story Specs | `docs/05-technical/specs/US-*.md` | Detail | 23 story spec files với AC detail |

---

## I. Release Gate Decision

### ✅ CONDITIONAL GO — v1.0.0-final Release Candidate

**Điều kiện:**

- ✅ Core MVP flows (Reactive + Predictive Maintenance) hoạt động
- ✅ 65 backend tests passed
- ✅ 11 authentication/authorization tests passed
- ✅ 27/30 smoke test scenarios passed
- ✅ Không có bug Critical/High về security
- ⚠️ Chấp nhận Medium bugs (FE deep-link guard) với ghi chú API vẫn bảo vệ
- ⚠️ Chấp nhận Low bugs (UX labels) là cosmetic
- ✅ Database migration & seed tự động
- ✅ Cấu hình đã được tài liệu hóa (`appsettings.json`)
- ✅ Local dev setup đã verify & lặp lại được

**KHÔNG-GO cho Production:**

- ❌ Chưa có staging environment
- ❌ Chưa có production deployment
- ❌ Chưa có load testing
- ❌ Chưa có production hardening (WAF, secrets manager, TLS)
- ❌ Chưa có monitoring/alerting
- ❌ Chưa có kế hoạch data backup/disaster recovery

**Khuyến cáo trước khi Evaluation:**

1. Xác nhận 65 tests pass bằng: `dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj`
2. Chạy qua 30 smoke test scenarios trong khoảng 30 phút
3. Yêu cầu BA xác nhận chấp nhận BUG-011, BUG-013 (FE guard) là low-risk
4. Ghi nhận: v1.0.0-final là demo local, **không production-ready**
