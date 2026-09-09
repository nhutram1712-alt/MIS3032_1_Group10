
# API Contract

Tài liệu này định nghĩa giao tiếp giữa Client (React/Vue/...) và Backend (C# ASP.NET Core MVC).

## 1. Authentication


| Method | Endpoint          | Auth Level | Request Payload                      | Response                                                              |
| ------ | ----------------- | ---------- | ------------------------------------ | --------------------------------------------------------------------- |
| POST   | `/api/auth/login` | Public     | `{ "username": "", "password": "" }` | `200 OK: { "token": "jwt_string", "role": "..." }` `401 Unauthorized` |


## 2. Asset Management


| Method | Endpoint                  | Auth Level                             | Request Payload                              | Response                                                       |
| ------ | ------------------------- | -------------------------------------- | -------------------------------------------- | -------------------------------------------------------------- |
| GET    | `/api/assets`             | Requester, FacilityManager, Technician | *None* (Có thể query `?location=X`)          | `200 OK: [ { "assetId": 1, "name": "...", "status": "..." } ]` |
| POST   | `/api/assets`             | FacilityManager                        | `{ "name": "", "type": "", "location": "" }` | `201 Created` `400 Bad Request`                                |
| PATCH  | `/api/assets/{id}/status` | FacilityManager                        | `{ "status": "Out of Service" }`             | `200 OK` `403 Forbidden`                                       |




## 3. Maintenance Request


| Method | Endpoint                    | Auth Level                 | Request Payload                               | Response      |
| ------ | --------------------------- | -------------------------- | --------------------------------------------- | ------------- |
| POST   | `/api/requests`             | Requester                  | `{ "assetId": 1, "description": "Lỗi WiFi" }` | `201 Created` |
| GET    | `/api/requests`             | Requester, FacilityManager | *None*                                        | `200 OK`      |
| PATCH  | `/api/requests/{id}/status` | FacilityManager            | `{ "status": "In Progress" }`                 | `200 OK`      |




## 4. Work Order


| Method | Endpoint                | Auth Level      | Request Payload                                       | Response      |
| ------ | ----------------------- | --------------- | ----------------------------------------------------- | ------------- |
| POST   | `/api/work-orders`      | FacilityManager | `{ "requestId": 1, "technicianId": 2, "assetId": 1 }` | `201 Created` |
| PATCH  | `/api/work-orders/{id}` | Technician      | `{ "status": "Completed", "rejectionReason": "" }`    | `200 OK`      |

## 5. IoT Monitoring
| Method | Endpoint | Auth Level | Request Payload | Response |
|---|---|---|---|---|
| POST | `/api/iot/ingest` | Gateway (API Key) | `{ "deviceId": "...", "metrics": { "temperature": 28.5 } }` | `201 Created` <br> `400 Bad Request` |
| GET | `/api/assets/{id}/iot-data` | FacilityManager | *None* | `200 OK: [ { "metric": "temperature", "value": 28.5, "time": "..." } ]` |

## 6. AI Predictive Maintenance
| Method | Endpoint | Auth Level | Request Payload | Response |
|---|---|---|---|---|
| GET | `/api/assets/{id}/prediction` | FacilityManager, Technician | *None* | `200 OK: { "risk": "High", "predictedAt": "..." }` <br> `404 Not Found` |



