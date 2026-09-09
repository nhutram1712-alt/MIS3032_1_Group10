
# API Contract

Tài liệu này định nghĩa giao tiếp giữa Client (React/Vue/...) và Backend (C# ASP.NET Core MVC).

---

## 1. Authentication

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| POST | `/api/auth/login` | Public | `{ "username": "", "password": "" }` | `200 OK: { "token": "jwt_string", "role": "..." }` `401 Unauthorized` |
| POST | `/api/auth/logout` | Authenticated | *None* | `200 OK` |

> **Ghi chú (US-01-02):** `POST /api/auth/logout` được bổ sung từ Spec US-01-02. Token phải bị vô hiệu hoá phía server (blacklist hoặc cơ chế tương đương).

---

## 2. User Management

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| GET | `/api/users` | Admin | *None* | `200 OK: [{ "userId": 1, "username": "...", "role": "..." }]` |
| POST | `/api/users` | Admin | `{ "username": "tech_tran", "password": "P@ssw0rd", "role": "Technician" }` | `201 Created` `400 Bad Request` |
| PUT | `/api/users/{id}` | Admin | `{ "role": "FacilityManager" }` | `200 OK` `404 Not Found` `400 Bad Request` |
| GET | `/api/users?role=Technician` | FacilityManager | *None* | `200 OK: [{ "userId": 7, "username": "tech_tran", "role": "Technician" }]` |

> **Ghi chú (US-01-03, US-04-02):** Toàn bộ section User Management bổ sung từ Specs. `GET /api/users?role=Technician` hỗ trợ FM chọn Technician khi phân công WO. `PasswordHash` không bao giờ được trả về trong response.

---

## 3. Asset Management

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| GET | `/api/assets` | Requester, FacilityManager, Technician | *None* (Có thể query `?location=X`) | `200 OK: [{ "assetId": 1, "name": "...", "status": "..." }]` |
| POST | `/api/assets` | FacilityManager | `{ "name": "", "type": "", "location": "" }` | `201 Created` `400 Bad Request` |
| GET | `/api/assets/{id}` | Requester, FacilityManager, Technician | *None* | `200 OK: { "assetId": 1, "name": "...", "type": "...", "location": "...", "status": "..." }` `404 Not Found` |
| PUT | `/api/assets/{id}` | FacilityManager | `{ "name": "", "type": "", "location": "" }` | `200 OK` `404 Not Found` `400 Bad Request` |
| PATCH | `/api/assets/{id}/status` | FacilityManager | `{ "status": "Out of Service" }` | `200 OK` `403 Forbidden` |

> **Ghi chú (US-02-03, US-02-04):** `GET /api/assets/{id}` và `PUT /api/assets/{id}` bổ sung từ Specs US-02-03 và US-02-04. Status hợp lệ: `Operational`, `Warning`, `Maintenance`, `Out of Service`.

---

## 4. Maintenance Request

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| POST | `/api/requests` | Requester | `{ "assetId": 1, "description": "Lỗi WiFi" }` | `201 Created` `400 Bad Request` |
| GET | `/api/requests` | Requester, FacilityManager | *None* | `200 OK: [{ "requestId": 5, "assetId": 1, "description": "...", "status": "...", "createdAt": "..." }]` |
| GET | `/api/requests/{id}` | Requester, FacilityManager | *None* | `200 OK` `404 Not Found` |
| PATCH | `/api/requests/{id}/status` | FacilityManager | `{ "status": "Pending" }` | `200 OK` `400 Bad Request` `404 Not Found` |
| GET | `/api/requests/{id}/history` | FacilityManager | *None* | `200 OK: { "requestId": 5, "workOrderId": 3, "technicianId": 7, "result": "...", "completedAt": "..." }` `404 Not Found` |

> **Ghi chú (US-03-02, US-03-03, US-03-04):**
> - `GET /api/requests/{id}` bổ sung từ US-03-02 — Requester cần xem chi tiết Request.
> - `GET /api/requests/{id}/history` bổ sung từ US-03-04 — FM xem kết quả bảo trì trước khi Closed.
> - `PATCH /api/requests/{id}/status` hỗ trợ toàn bộ lifecycle BR-14: `Submitted → Pending → In Progress → Resolved → Closed / Rejected`. Server phải validate transition hợp lệ.
> - Khi chuyển sang `Closed`, server phải xác minh WO liên quan đã `Completed` (BR-18).

---

## 5. Work Order

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| POST | `/api/work-orders` | FacilityManager | `{ "requestId": 1, "technicianId": 2, "assetId": 1 }` | `201 Created` `400 Bad Request` |
| GET | `/api/work-orders` | FacilityManager, Technician | *None* | `200 OK: [{ "orderId": 3, "requestId": 5, "assetId": 1, "status": "...", "createdAt": "..." }]` |
| GET | `/api/work-orders/{id}` | FacilityManager, Technician | *None* | `200 OK: { "orderId": 3, "requestId": 5, "asset": {...}, "status": "...", "maintenanceHistory": [...] }` `404 Not Found` |
| PATCH | `/api/work-orders/{id}` | FacilityManager, Technician | Xem ghi chú bên dưới | `200 OK` `400 Bad Request` `403 Forbidden` `404 Not Found` |

**Payload cho `PATCH /api/work-orders/{id}` theo từng role/action:**

| Action | Thực hiện bởi | Payload |
|--------|---------------|---------|
| Phân công Technician | FacilityManager | `{ "technicianId": 7 }` |
| Hủy Work Order | FacilityManager | `{ "status": "Cancelled" }` |
| Bắt đầu thực hiện | Technician | `{ "status": "In Progress" }` |
| Từ chối (kèm lý do) | Technician | `{ "rejectionReason": "Không có dụng cụ phù hợp." }` |
| Hoàn thành (kèm kết quả) | Technician | `{ "status": "Completed", "result": "Mô tả công việc đã thực hiện..." }` |

> **Ghi chú (US-04-02, US-04-03, US-04-04, US-04-05, US-04-06):**
> - `GET /api/work-orders` bổ sung từ US-04-03/04-04 — cả FM và Technician xem danh sách WO (server filter theo role).
> - `GET /api/work-orders/{id}` bổ sung từ US-04-04 — response chi tiết bao gồm thông tin `asset` và `maintenanceHistory`.
> - Payload `{ "technicianId": 7 }` bổ sung từ US-04-02 (phân công Technician riêng biệt với cập nhật status).
> - Payload `{ "status": "Completed", "result": "..." }` bổ sung `result` field từ US-04-06 (BR-09: WO hoàn thành phải có kết quả).
> - Payload `{ "rejectionReason": "..." }` không kèm status change từ US-04-05.
> - BR-07: Server phải xác minh `TechnicianID` trong WO khớp JWT trước khi cho phép Technician thao tác.
> - Status lifecycle (BR-17): `Assigned → In Progress → Completed`. `Completed` và `Cancelled` là trạng thái cuối.

---

## 6. IoT Monitoring

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| POST | `/api/iot/ingest` | Gateway (API Key) | `{ "deviceId": "...", "metrics": { "temperature": 28.5 } }` | `201 Created` `400 Bad Request` |
| GET | `/api/assets/{id}/iot-data` | FacilityManager | *None* | `200 OK: [{ "metricType": "temperature", "readingValue": 28.5, "timestamp": "..." }]` `404 Not Found` |
| GET | `/api/iot-alerts` | FacilityManager, Technician | `?assetId={id}` (Tùy chọn — Technician lọc Alert theo Asset) | `200 OK: [{ "alertId": 12, "assetId": 1, "assetName": "...", "metricType": "...", "readingValue": 45.2, "threshold": 40.0, "severity": "High", "detectedAt": "..." }]` |
| POST | `/api/iot-mappings` | Admin | `{ "assetId": 1, "deviceId": "SENSOR-WIFI-P201" }` | `201 Created` `400 Bad Request` `404 Not Found` |
| GET | `/api/iot-mappings` | Admin | *None* | `200 OK: [{ "mappingId": 5, "assetId": 1, "assetName": "...", "deviceId": "...", "createdAt": "..." }]` |
| PUT | `/api/iot-mappings/{id}` | Admin | `{ "deviceId": "SENSOR-WIFI-P201-V2" }` | `200 OK` `400 Bad Request` `404 Not Found` |

> **Ghi chú (US-05-01, US-05-02, US-05-04):**
> - `GET /api/iot-alerts` bổ sung từ US-05-04 — FM xem danh sách IoT Alert với đầy đủ thông tin bất thường.
> - `POST /api/iot-mappings`, `GET /api/iot-mappings`, `PUT /api/iot-mappings/{id}` bổ sung hoàn toàn từ US-05-01 và US-05-02.
> - `POST /api/iot/ingest` dùng **API Key**, không dùng JWT — middleware auth riêng biệt.
> - BR-13: Mỗi Asset chỉ có một IoT Mapping trong MVP; server từ chối tạo mới nếu Asset đã có mapping.

---

## 7. AI Predictive Maintenance

| Method | Endpoint | Auth Level | Request Payload | Response |
|--------|----------|------------|-----------------|----------|
| GET | `/api/assets/{id}/prediction` | FacilityManager, Technician | *None* | `200 OK: { "assetId": 1, "assetName": "...", "assetType": "...", "assetLocation": "...", "risk": "High", "predictedAt": "...", "stale": false, "basedOnSampleData": false }` `404 Not Found` |
| GET | `/api/predictions` | FacilityManager | *None* | `200 OK: [{ "assetId": 1, "assetName": "...", "location": "...", "risk": "High", "predictedAt": "..." }]` |

> **Ghi chú (US-06-02, US-06-03):**
> - `GET /api/predictions` bổ sung từ US-06-02 — FM xem dashboard tổng quan Risk của tất cả Asset (hỗ trợ `?sort=risk_desc`).
> - Response bổ sung trường `stale: true` khi `PredictedAt` quá 24 giờ.
> - Response bổ sung trường `basedOnSampleData: true` khi AI dùng dữ liệu mẫu/giả lập trong MVP (BR-11).
> - BR-10: AI chỉ hỗ trợ quyết định — hệ thống không tự động tạo WO hay thay đổi Asset Status từ Prediction.
> - NFR-08: `assetId` và `predictedAt` bắt buộc có trong mọi response Prediction.

---

## Phụ lục: Quy ước chung

| Hạng mục | Quy ước |
|----------|---------|
| Authentication | JWT Bearer Token trong `Authorization` header cho mọi endpoint trừ `/api/auth/login` và `/api/iot/ingest` |
| IoT Ingest Auth | API Key trong header `X-API-Key` |
| DateTime format | ISO 8601: `2026-09-09T14:30:00` (UTC+7) |
| Error format | `{ "error": "Mô tả lỗi rõ ràng" }` |
| Status codes | `200 OK`, `201 Created`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found`, `500 Internal Server Error` |
