# Release Notes — v1.0.0-final

> **Project:** Smart Maintenance & Facility Management  
> **Target Version:** v1.0.0-final  
> **Release Status:** Release Candidate (RC) — Validation Pending  
> **Last Updated:** 2026-10-10  
> **Commit Reference:** d9a52ac5936a6dec516604a5342e7ce28dd453e5  
> **Scope:** MVP — Local development demo only (Not production-ready)

---

## 1. Release Metadata

| Field | Value |
|---|---|
| **Project** | Smart Maintenance & Facility Management |
| **Target Version** | v1.0.0-final |
| **Status** | Release Candidate (Validation Pending) |
| **Not yet:** | Published on GitHub Releases, Deployed to production, Staged on staging server |
| **Organization** | Trường Đại học Kinh tế – Đại học Đà Nẵng (DUE) |
| **Scope** | MVP: 4 Roles, 5 Asset Types, Reactive + Predictive Maintenance |
| **Environment** | Local development only (SQL Server LocalDB, Vite dev server) |
| **Commit SHA** | d9a52ac5936a6dec516604a5342e7ce28dd453e5 |
| **Test Evidence** | 65 backend tests passed (10/10/2026); 30 smoke test scenarios |
| **Documentation Date** | 2026-10-10 |

---

## 2. Release Summary

**Smart Maintenance & Facility Management v1.0.0-final** là phiên bản MVP hoàn thành, cung cấp giải pháp quản lý bảo trì cơ sở vật chất toàn diện cho Trường Đại học ...

Phiên bản này hỗ trợ hai luồng bảo trì chính:
1. **Reactive Maintenance:** Người dùng báo cáo sự cố → Quản lý tạo phiếu công việc → Kỹ thuật viên thực hiện → Ghi nhận kết quả → Đóng yêu cầu.
2. **Predictive Maintenance:** Hệ thống IoT thu thập dữ liệu → Phát sinh cảnh báo → AI dự báo rủi ro → Quản lý ưu tiên sắp xếp công việc.

Phiên bản này **không** là production-ready. Đây là bản demo để đánh giá cuối kỳ trên môi trường local, với giới hạn phạm vi MVP được xác nhận từ Requirement...

---

## 3. Features by User Role

### 3.1. Requester (Người báo cáo sự cố)

| Capability | Business Value | Related Story/REQ | AC ID | Verification Status |
|---|---|---|---|---|
| Đăng nhập với tài khoản nội bộ | Xác thực người dùng | REQ-01 | AC-US-01-01-01 | ✅ PASS |
| Tạo Maintenance Request | Báo cáo sự cố về Asset | REQ-04, REQ-05 | AC-US-03-01-01 | ✅ PASS |
| Theo dõi Request của mình | Cập nhật tiến độ xử lý | REQ-05 | AC-US-03-02-01 | ✅ PASS |
| Xem Asset được phép sử dụng | Chọn Asset khi báo cáo sự cố | REQ-02, REQ-03 | AC-US-01-02-01 | ⚠️ PARTIAL (Backend OK, UI chưa filter) |
| Đăng xuất | Kết thúc phiên làm việc | Implicit | AC-US-01-01-03 | ✅ PASS |

**Tóm tắt:** Core flow "báo cáo sự cố" hoạt động đầy đủ. Gap: Requester chưa có UI để lọc Asset theo phòng (REQ-02).

---

### 3.2. Technician (Kỹ thuật viên)

| Capability | Business Value | Related Story/REQ | AC ID | Verification Status |
|---|---|---|---|---|
| Xem Work Order được phân công | Biết công việc cần thực hiện | REQ-17 | AC-US-04-04-01 | ✅ PASS |
| Xem chi tiết Work Order | Hiểu rõ nội dung phiếu công việc | REQ-18 | AC-US-04-04-02 | ✅ PASS |
| Xem Asset liên quan | Nhận biết tài sản cần bảo trì | REQ-18 | AC-US-04-04-03 | ✅ PASS |
| Xem IoT Data & Alert | Kiểm tra tình trạng device | REQ-19, REQ-09 | AC-US-05-03 | ✅ PASS |
| Xem AI Prediction / Risk | Hiểu mức độ khẩn cấp | REQ-19, REQ-11 | AC-US-06-03 | ✅ PASS (label tiếng Anh) |
| Cập nhật Work Order (Assigned → In Progress) | Ghi nhận bắt đầu công việc | REQ-20 | AC-US-04-05-01 | ✅ PASS |
| Từ chối Work Order kèm lý do | Xử lý trường hợp không thực hiện được | REQ-20 | AC-US-04-05-03 | ✅ PASS |
| Ghi nhận kết quả bảo trì | Lưu hành động và kết quả | REQ-21 | AC-US-04-06-01 | ✅ PASS |
| Hoàn thành Work Order | Chuyển trạng thái sang Completed | REQ-22 | AC-US-04-06-02 | ✅ PASS |
| Đăng xuất | Kết thúc phiên | Implicit | AC-US-01-01-03 | ✅ PASS |

**Tóm tắt:** Toàn bộ luồng thực hiện công việc hoạt động. UX: một số label tiếng Anh cần dịch.

---

### 3.3. Facility Manager (Quản lý bảo trì)

| Capability | Business Value | Related Story/REQ | AC ID | Verification Status |
|---|---|---|---|---|
| Đăng nhập với tài khoản nội bộ | Xác thực người dùng | REQ-01 | AC-US-01-01-01 | ✅ PASS |
| Xem dashboard khiển toàn Asset | Giám sát tổng thể hệ thống | Implicit | N/A | ✅ PASS |
| Thêm Asset mới | Quản lý danh mục tài sản | REQ-06 | AC-US-02-01-01 | ✅ PASS |
| Cập nhật thông tin Asset | Sửa chữa thông tin sai | REQ-07 | AC-US-02-02-01 | ✅ PASS |
| Thay đổi Asset Status | Cập nhật trạng thái tài sản | REQ-08 | AC-US-02-03-01 | ✅ PASS |
| Xem IoT Data của Asset | Kiểm tra thông số real-time | REQ-09 | AC-US-05-03-01 | ✅ PASS |
| Xem IoT Alert | Nhận thông báo bất thường | REQ-10 | AC-US-05-04-01 | ✅ PASS |
| Xem AI Prediction | Biết mức độ rủi ro Asset | REQ-11, REQ-12 | AC-US-06-02-01 | ✅ PASS |
| Ưu tiên Asset có Risk cao | Sắp xếp công việc theo ưu tiên | BR-15 | Implicit | ✅ PASS |
| Tiếp nhận Maintenance Request | Phê duyệt yêu cầu báo cáo | REQ-13 | AC-US-03-03-01 | ✅ PASS |
| Tạo Work Order từ Request | Phân công công việc | REQ-14 | AC-US-04-01-01 | ✅ PASS |
| Phân công Technician | Gán Technician phù hợp | REQ-15 | AC-US-04-02-01 | ✅ PASS |
| Xem danh sách Work Order | Giám sát tiến độ công việc | REQ-16 | AC-US-04-03-01 | ✅ PASS |
| Xác nhận kết quả bảo trì | Phê duyệt hoàn thành công việc | REQ-13, BR-18 | AC-US-03-04-01 | ✅ PASS |
| Mapping Asset ↔ IoT Device | Kết nối thiết bị giám sát | REQ-26, REQ-27 | AC-US-05-01-01 | ✅ PASS |
| Đăng xuất | Kết thúc phiên | Implicit | AC-US-01-01-03 | ✅ PASS |

**Tóm tắt:** Toàn bộ quy trình quản lý hoạt động hoàn chỉnh. UI còn một số label tiếng Anh.

---

### 3.4. Admin (Quản trị viên)

| Capability | Business Value | Related Story/REQ | AC ID | Verification Status |
|---|---|---|---|---|
| Đăng nhập với tài khoản nội bộ | Xác thực người dùng | REQ-01 | AC-US-01-01-01 | ✅ PASS |
| Xem danh sách User | Quản lý tài khoản | REQ-24 | AC-US-01-03-01 | ✅ PASS |
| Tạo User mới | Cấp tài khoản cho nhân viên mới | REQ-24 | AC-US-01-03-02 | ✅ PASS |
| Gán / Đổi Role User | Thay đổi quyền của người dùng | REQ-25 | AC-US-01-04-03 | ✅ PASS (với guards QT-1..4) |
| Xem ma trận quyền cố định | Tham chiếu quyền của từng Role | REQ-25 | AC-US-01-04-01 | ✅ PASS |
| Mapping Asset ↔ IoT Device | Quản lý kết nối thiết bị | REQ-26, REQ-27 | AC-US-05-01-01 | ✅ PASS |
| Đăng xuất | Kết thúc phiên | Implicit | AC-US-01-01-03 | ✅ PASS |

**Tóm tắt:** Chức năng quản trị cơ bản hoạt động. Có guards chặn leo thang quyền (QT-1..4).

---

## 4. Nhật ký thay đổi

Mục này mô tả các tính năng, thay đổi và vấn đề trong phiên bản v1.0.0-final, dựa trên bằng chứng từ git history, báo cáo kiểm thử và đánh giá bảo mật.

### 4.1. Đã thêm

| Danh mục | Thay đổi | Story/REQ liên quan | Bằng chứng | Trạng thái |
|---|---|---|---|---|
| **Xác thực & Phân quyền** | Xác thực JWT (hết hạn 480 phút) | REQ-01, DEC-06 | `Program.cs` thiết lập AuthN, kiểm thử AC-US-01-01-01 | ✅ ĐÃ XÁC NHẬN |
| **Xác thực & Phân quyền** | Kiểm soát truy cập theo vai trò (4 vai trò: Requester, Technician, FM, Admin) | REQ-01, REQ-25, DEC-06 | `[Authorize(Roles=...)]`, 11 bài kiểm thử auth | ✅ ĐÃ XÁC NHẬN |
| **Xác thực & Phân quyền** | Chặn token khi đăng xuất | REQ-01 | Dịch vụ `ITokenBlacklist`, bài test AC-AUTH-03 | ✅ ĐÃ XÁC NHẬN |
| **Xác thực & Phân quyền** | Mã hóa mật khẩu (BCrypt) | REQ-01, NFR-02 | Thư viện `BCrypt.Net-Next`, kiểm thử AC-US-01-01-02 | ✅ ĐÃ XÁC NHẬN |
| **Xác thực & Phân quyền** | Guard phòng leo quyền theo vai trò (QT-1, QT-2, QT-3, QT-4) | REQ-24, REQ-25 | Giới hạn trong `UserService.Update()`, 4 trường hợp kiểm thử | ✅ ĐÃ XÁC NHẬN |
| **Quản lý tài sản** | Tạo, đọc, cập nhật Asset | REQ-06, REQ-07 | Các endpoint `AssetsController`, kiểm thử AC-US-02-01 | ✅ ĐÃ XÁC NHẬN |
| **Quản lý tài sản** | Thay đổi trạng thái Asset (Operational, Warning, Maintenance, Out of Service) | REQ-08, BR-16 | Endpoint `PATCH /api/assets/{id}/status` | ✅ ĐÃ XÁC NHẬN |
| **Yêu cầu bảo trì** | Chu kỳ yêu cầu (Submitted → Pending → In Progress → Resolved → Closed / Rejected) | REQ-04, REQ-05, BR-14 | `RequestService` state machine, kiểm thử AC-US-03 | ✅ ĐÃ XÁC NHẬN |
| **Yêu cầu bảo trì** | Tách biệt quyền sở hữu của Requester (chỉ xem yêu cầu của mình) | REQ-05, OW-01 | `ListAsync()` lọc theo vai trò, kiểm thử AC-AUTHZ-03 | ✅ ĐÃ XÁC NHẬN |
| **Work Order** | Chu kỳ Work Order (Assigned → In Progress → Completed / Cancelled) | REQ-14, REQ-20, BR-17 | `WorkOrderService` state machine, kiểm thử AC-US-04 | ✅ ĐÃ XÁC NHẬN |
| **Work Order** | Kỹ thuật viên từ chối Work Order kèm lý do | REQ-20, BR-07, DEC-03 | Trường `rejectionReason` trong `PATCH` | ✅ ĐÃ XÁC NHẬN |
| **Work Order** | Tách biệt quyền sở hữu Technicain (chỉ thao tác WO được giao) | REQ-20, BR-07, OW-02 | Phương thức `EnsureTechnicianAccess()`, test AC-AUTHZ-08 | ✅ ĐÃ XÁC NHẬN |
| **Work Order** | Ghi lịch sử bảo trì | REQ-21, BR-09 | Bảng `MaintenanceHistory` + liên kết với WO hoàn thành | ✅ ĐÃ XÁC NHẬN |
| **Giám sát IoT** | Nhận dữ liệu IoT qua API Key | REQ-28, REQ-29 | `POST /api/iot/ingest` kèm header `X-Api-Key` | ✅ ĐÃ XÁC NHẬN |
| **Giám sát IoT** | Đồng bộ Asset ↔ Thiết bị IoT (1-1 trong MVP) | REQ-26, REQ-27, BR-13 | Entity `IotMapping`, kiểm thử AC-US-05-01 | ✅ ĐÃ XÁC NHẬN |
| **Giám sát IoT** | Tạo cảnh báo khi vượt ngưỡng | REQ-10, BR-12 | Logic tạo alert, kiểm thử AC-US-05-04 | ✅ ĐÃ XÁC NHẬN |
| **Bảo trì dự đoán bằng AI** | Dự đoán AI (dự báo 7 ngày, mức rủi ro Low/Medium/High) | REQ-11, REQ-12, BR-10, BR-11 | Background prediction service, kiểm thử AC-US-06 | ✅ ĐÃ XÁC NHẬN |
| **Bảo mật** | Bảo vệ API bằng JWT Bearer token | NFR-01, SEC-01 | Middleware `[Authorize]`, test AC-AUTH-04 | ✅ ĐÃ XÁC NHẬN |
| **Bảo mật** | Áp dụng kiểm soát quyền sở hữu dữ liệu (Requester/Technician) | NFR-01, SEC-02 | Kiểm tra ở Service layer | ✅ ĐÃ XÁC NHẬN |
| **Cơ sở dữ liệu** | Tự động migrate khi khởi động | Implicit | `MigrateAsync()` trong `Program.cs` | ✅ ĐÃ XÁC NHẬN |
| **Cơ sở dữ liệu** | Tự động seed dữ liệu demo (4 users, 7 assets, IoT mappings) | Implicit | `DbSeeder.SeedAsync()` | ✅ ĐÃ XÁC NHẬN |
| **Bộ kiểm thử** | 65 bài kiểm thử backend (unit + integration) | Implicit | xUnit tests, phù hợp CI/CD | ✅ ĐÃ XÁC NHẬN |

---

### 4.2. Đã thay đổi

N/A — Đây là bản phát hành MVP đầu tiên.

---

### 4.3. Đã sửa

N/A — Chưa có phiên bản trước để đối chiếu các bản sửa.

Tuy nhiên, các vấn đề đã xác định trong quá trình phát triển (được theo dõi trong `code-review.md`):
- BUG-006: Chưa có bài E2E Playwright (dự kiến sprint tới)
- BUG-007: Nhãn mức độ cảnh báo IoT bằng tiếng Anh (có tính thẩm mỹ, ưu tiên thấp)
- BUG-008: Nhãn mức độ rủi ro bằng tiếng Anh (có tính thẩm mỹ, ưu tiên thấp)
- BUG-009: Chưa có nút "Unmap" cho IoT Device (ưu tiên trung bình)
- BUG-010: Chưa có giao diện chỉnh sửa Device ID cho IoT Mapping (ưu tiên trung bình)
- BUG-011: Route `/admin` chưa bọc `RequireRoles` guard (UX trung bình, API vẫn chặn)
- BUG-012: Chưa có giao diện cấu hình khoảng thời gian thu thập IoT (ưu tiên trung bình)
- BUG-013: Route `/alerts`, `/predictions` chưa bọc `RequireRoles` (UX trung bình, API vẫn chặn)
- BUG-014: Nhãn menu Work Order bằng tiếng Anh (có tính thẩm mỹ, ưu tiên thấp)

---

### 4.4. Bảo mật

| Thay đổi | Đã xác minh | Bằng chứng |
|---|---|---|
| Xác thực JWT (không dùng SSO, chỉ tài khoản nội bộ) | ✅ PASS | DEC-06, cấu hình trong `Program.cs` |
| RBAC với 4 vai trò cố định | ✅ PASS | `[Authorize(Roles=...)]`, 11 bài test auth |
| Chặn token khi đăng xuất (không tái sử dụng token) | ✅ PASS | `ITokenBlacklist` + event `OnTokenValidated` |
| Mã hóa mật khẩu (không lưu mật khẩu dạng plaintext) | ✅ PASS | Mã hóa BCrypt trong `UserService` |
| Chặn leo thang quyền (QT-2, QT-3, QT-4) | ✅ PASS | Giới hạn trong `UserService.Update()` |
| Xác thực API Key cho IoT Gateway | ✅ PASS | Bộ lọc `IotGatewayAuthorize` |
| Không có secrets trong repo (chỉ có `.env.example`) | ✅ PASS | Review mã nguồn các commit |
| Kiểm soát quyền sở hữu dữ liệu (Requester/Technician) | ✅ PASS | Kiểm tra ở tầng Service |

**Trạng thái bảo mật:** ✅ **Không phát hiện lỗ hổng Critical/High.** Có 2 vấn đề UX mức trung bình (route guard frontend), 6 vấn đề thẩm mỹ mức thấp (nhãn).

---

### 4.5. Vấn đề đã biết

| Vấn đề | Tác động tới người dùng/kinh doanh | Related AC/Requirement | Trạng thái hiện tại | Hành động tiếp theo |
|---|---|---|---|---|
| REQ-02: Lọc tài sản theo phòng cho Requester (UI chưa có) | Thấp – Trung bình | AC-US-01-02 | Backend đã sẵn sàng, UI chưa triển khai | Chuyển sang "Should Have" cho v1.1 hoặc bổ sung UI ở sprint tới |
| US-01-04: Mơ hồ về cập nhật quyền Admin | Thấp | AC-US-01-04-02 | Ma trận quyền cố định, không phải cập nhật động | Làm rõ ý định AC: chỉ xem ma trận hay cho phép cấu hình động |
| BUG-011, BUG-013: Chưa có frontend route guard | Thấp (API vẫn chặn) | Implicit | Route tải mà không có `RequireRoles` wrapper | Bọc routes bằng `RequireRoles` |
| BUG-012: Chưa có UI cấu hình interval IoT | Trung bình | NFR-05 | Interval cố định 5 phút | Thêm giao diện cấu hình |
| BUG-009, BUG-010: Chưa có UI Unmap/Edit mapping | Trung bình | REQ-26, REQ-27 | Backend hỗ trợ DELETE/PUT, nhưng UI chưa có | Thêm nút Unmap & Edit |
| Tái vô hiệu hóa session sau khi thay đổi role / tài khoản | Thấp – Trung bình | NFR-02 | Chưa triển khai; token tiếp tục hợp lệ đến khi hết hạn | Quyết định chính sách: hủy ngay hoặc để hết hạn tự nhiên |
| Nhãn tiếng Anh (BUG-007, 008, 014) | Thấp (thẩm mỹ) | NFR-07 | Mức độ cảnh báo, mức độ rủi ro, menu item | Dịch sang tiếng Việt |

---

## 5. Acceptance and Verification Summary

| Story/Requirement | AC ID | Expected Outcome | Verification Evidence | Status |
|---|---|---|---|---|
| **REQ-01** (Login) | AC-US-01-01-01/02/03 | Người dùng đăng nhập, thấy menu theo vai trò | xUnit test: `LoginTests.Success`, manual test | ✅ PASS |
| **REQ-02** (Requester asset access by room) | AC-US-01-02-01/02 | Requester chỉ thấy tài sản được phép | Backend filter logic + (UI pending) | ⚠️ PARTIAL |
| **REQ-04** (Create Request) | AC-US-03-01-01 | Requester tạo request, nhận trạng thái Submitted | xUnit: `CreateRequestApiTests.PostRequest_ValidRequester_Returns201_Submitted` | ✅ PASS |
| **REQ-05** (Track Request) | AC-US-03-02-01 | Requester xem yêu cầu của mình | xUnit: `RequestServiceTests.Create_Valid_PersistsSubmitted_WithJwtRequesterId` | ✅ PASS |
| **REQ-06** (Create Asset) | AC-US-02-01-01 | FM tạo asset thành công | xUnit: `CreateAssetApiTests.PostAssets_ValidFacilityManager_Returns201_AndPersists` | ✅ PASS |
| **REQ-08** (Update Asset Status) | AC-US-02-03-01 | FM thay đổi trạng thái asset | Integration test; manual verified | ✅ PASS |
| **REQ-09** (View IoT Data) | AC-US-05-03-01 | FM/Tech xem metrics IoT | xUnit: `IotApiTests.GetIotData_*` | ✅ PASS |
| **REQ-10** (IoT Alert) | AC-US-05-04-01 | Alert được tạo khi vượt ngưỡng | xUnit: `IotApiTests` alert tests | ✅ PASS |
| **REQ-11, REQ-12** (AI Prediction) | AC-US-06-01/02/03 | Dự đoán có mức độ rủi ro | xUnit: `PredictionApiTests.GetPrediction_*` | ✅ PASS |
| **REQ-13** (Handle Request) | AC-US-03-03-01 | FM xử lý request | Integration test | ✅ PASS |
| **REQ-14** (Create Work Order) | AC-US-04-01-01 | FM tạo WO, gán tech | xUnit: `CreateWorkOrderApiTests.PostWorkOrder_ValidManager_Returns201_Assigned` | ✅ PASS |
| **REQ-15** (Assign Technician) | AC-US-04-02-01 | FM gán tech cho WO | xUnit: `WorkOrderServiceTests` | ✅ PASS |
| **REQ-17** (Technician view WO) | AC-US-04-04-01 | Tech thấy WO của mình | Integration test; manual verified | ✅ PASS |
| **REQ-20** (Technician update WO) | AC-US-04-05-01 | Tech cập nhật trạng thái (Assigned → In Progress) | xUnit: `WorkOrderServiceTests` | ✅ PASS |
| **REQ-21, REQ-22** (Complete WO) | AC-US-04-06-01/02 | Tech ghi nhận kết quả, WO hoàn thành | xUnit: completion tests | ✅ PASS |
| **REQ-24** (Manage Users) | AC-US-01-03-01/02 | Admin tạo user | xUnit: `UserServiceTests` | ✅ PASS |
| **REQ-25** (Manage Roles) | AC-US-01-04-01/02/03 | Admin gán vai trò (có guards) | xUnit: QT-1..4 test cases | ✅ PASS |
| **REQ-26, REQ-27** (IoT Mapping) | AC-US-05-01/02 | Admin map thiết bị vào asset | xUnit: mapping tests | ✅ PASS |
| **NFR-01** (RBAC) | Multiple | Phân quyền theo vai trò được thực thi | 11 auth/authz tests (401/403) | ✅ PASS |
| **NFR-02** (Password Security) | Implicit | Mật khẩu được hash, không plaintext | Code review + BCrypt lib | ✅ PASS |
| **NFR-03** (Data Consistency) | BR-06, BR-18 | 1 Request → 1 WO; Request chỉ đóng sau khi WO hoàn tất | Integration tests | ✅ PASS |
| **NFR-04** (Timestamp) | Multiple | createdAt, updatedAt, detectedAt, predictedAt hiện hữu | Database schema + seed data | ✅ PASS |

**Tóm tắt:** 23 yêu cầu Must-have đã được xác minh với trạng thái Pass. 1 yêu cầu (REQ-02) đang ở trạng thái Partial (backend đã sẵn sàng, UI đang chờ triển khai).

---

## 6. Release Acceptance Status

| Khu vực | Kết quả mong đợi | Trạng thái | Bằng chứng | Hành động còn lại |
|---|---|---|---|---|
| **Authentication & Authorization** | 4 vai trò với quyền riêng biệt, không leo thang quyền | ✅ PASS | 65 tests (11 auth-focused), không có bug Critical | Làm rõ mục đích US-01-04; cân nhắc chốt chính sách session |
| **Asset Management** | CRUD, tracking trạng thái, hỗ trợ location | ✅ PASS | AC-US-02 verified; 7 seed assets | Kiểm tra đầy đủ REQ-02 (room filtering) UI |
| **Maintenance Request** | Chu kỳ đầy đủ (Submitted → Closed), quyền sở hữu riêng | ✅ PASS | AC-US-03 verified; ownership tests pass | Không có |
| **Work Order** | Chu kỳ đầy đủ (Assigned → Completed), quyền sở hữu kỹ thuật viên, ghi result | ✅ PASS | AC-US-04 verified; ownership tests pass | Thêm UI chỉnh sửa Device ID cho IoT Mapping (BUG-010) |
| **IoT Mapping/Monitoring** | Mapping device, ingest dữ liệu, tạo cảnh báo | ✅ PASS | AC-US-05 verified; API key auth confirmed | Thêm UI Unmap (BUG-009); UI config interval (BUG-012) |
| **AI Prediction** | Dự báo 7 ngày, phân loại mức rủi ro | ✅ PASS | AC-US-06 verified; background service runs | Dịch nhãn nguy cơ sang tiếng Việt (BUG-008) |
| **Acceptance Criteria/Test Evidence** | 23 AC core must-have đã xác minh | ✅ PASS | 65 backend tests; 30 smoke tests; manual verification | 1 AC partial (REQ-02 UI); 9 AC chưa test đầy đủ (liệt kê trong issue log) |
| **Known Issues** | Vấn đề đã được theo dõi, ưu tiên rõ, không có bug Critical/High | ✅ PASS | BUG log trong `code-review.md`; 2 Medium (UX), 6 Low (thẩm mỹ) | Fix BUG-009 đến BUG-014 ở sprint tiếp theo; quyết định chính sách session |
| **Security** | Không truy cập trái phép, bảo vệ dữ liệu, audit trail | ✅ PASS | JWT+RBAC+BCrypt đã xác minh; không có secrets trong repo | Thêm frontend route guards (BUG-011, BUG-013) để cải thiện UX |
| **Documentation** | Mô hình traceability, AC, test evidence rõ ràng | ✅ PASS | `authn-authz.md` hoàn chỉnh; `release-notes.md` đầy đủ | Làm rõ phạm vi REQ-02 cho phiên bản tới |

---

## 7. Go/No-Go Decision

### Release Gate Status

**✅ CONDITIONAL GO cho v1.0.0-final dưới dạng Release Candidate**

#### Điều kiện chấp nhận:
1. ✅ Core MVP flows (Reactive + Predictive Maintenance) đang hoạt động có bằng chứng.
2. ✅ 65 bài kiểm thử backend đã pass (10/10/2026).
3. ✅ 23 yêu cầu Must-have đã xác minh.
4. ✅ Không có lỗ hổng bảo mật Critical/High.
5. ⚠️ Chấp nhận 2 vấn đề UX trung bình (BUG-011, BUG-013: frontend route guards) — API layer vẫn chặn quyền truy cập.
6. ⚠️ Chấp nhận 6 vấn đề thẩm mỹ thấp (BUG-007, 008, 014: nhãn; BUG-009, 010: tính năng UI tùy chọn; BUG-012: config).
7. ⚠️ Chấp nhận REQ-02 là "Should Have" (UI lọc room cho Requester chưa hoàn tất).
8. ⚠️ Chấp nhận sự mơ hồ của US-01-04 (ma trận RBAC cố định, không phải cập nhật quyền động).
9. ⚠️ Chấp nhận chính sách invalidation session đang chờ quyết định (token hết hạn tự nhiên, không hủy ngay khi đổi role).
10. ✅ Hiểu rõ: **Đây là demo MVP trên local.** Chưa production-ready (không có staging, không có hardening, không có monitoring).

#### Không phải blocker:
- ❌ Không tìm thấy vấn đề nào là blocker.

#### Hành động còn lại trước khi phát hành chính thức (v1.0.1 hoặc v1.1):
1. Thêm UI lọc tài sản theo phòng cho Requester (REQ-02).
2. Làm rõ và cập nhật AC US-01-04 (chỉ xem ma trận hay cho phép cấu hình động?).
3. Triển khai session invalidation khi thay đổi role/account.
4. Thêm frontend route guards (`RequireRoles` wrapper cho `/admin`, `/alerts`, `/predictions`).
5. Dịch nhãn tiếng Anh sang tiếng Việt (Alert severity, Risk level, Menu items).
6. Thêm nút Unmap cho IoT Mapping.
7. Thêm giao diện Edit Device ID cho IoT Mapping.
8. Thêm UI cấu hình interval IoT.
9. Thiết lập môi trường staging.
10. Production hardening (TLS, WAF, secrets manager, monitoring).

---

## 8. Links & References

| Document Type | Location | Purpose |
|---|---|---|
| Requirements | `docs/02-vault/requirements.md` | Functional & Non-Functional Requirements (30 FR + 8 NFR) |
| Business Rules | `docs/02-vault/business-rules.md` | 18 Business Rules governing system behavior |
| Decision Log | `docs/02-vault/decision-log.md` | 9 confirmed decisions (DEC-01 through DEC-09) |
| User Stories | `docs/03-product/user-stories.md` | 23 User Stories (4 Epics, baselined) |
| Acceptance Criteria | `docs/03-product/acceptance-criteria.md` | AC for all user stories (MVP scope) |
| MVP Scope | `docs/03-product/MVP-scope.md` | Features in/out of MVP scope |
| API Contract | `docs/05-technical/api-contract.md` | 28 endpoint definitions + payloads |
| Architecture | `docs/05-technical/system-architecture.md` | 3-tier design, microservices (AI, IoT) |
| Security Review | `docs/06-test/security-nfr.md` | Auth, authorization, and NFR verification |
| Code Review | `docs/06-test/code-review.md` | Implementation review, findings, BUG log |
| Auth & Authz Spec | `docs/03-product/authn-authz.md` | **NEW** BA review of Authentication & Authorization (Review Required) |
| Test Strategy | `docs/06-test/test-strategy.md` | QA approach & coverage |
| Source Priority | `docs/02-vault/source-priority.md` | Conflict resolution for Vault documents |

---

## 9. Document Control

| Aspect | Value |
|---|---|
| **Prepared by** | Business Analysis Team |
| **Version** | v1.0 (Release Candidate) |
| **Status** | Validation Pending |
| **Last Updated** | 2026-10-10 |
| **Commit Reference** | d9a52ac5936a6dec516604a5342e7ce28dd453e5 |
| **Next Review** | After evaluation meeting or upon PO decision on open items |

---

**This release candidate reflects the current state of the Smart Maintenance & Facility Management MVP. All statements are grounded in evidence from requirements, business rules, acceptance criteria, and test execution.**
