# System Architecture
# System Architecture

# System Architecture

## 1. Kiến trúc tổng thể (System Overview)
Hệ thống Smart Maintenance Facility Management được thiết kế theo kiến trúc **Client-Server** kết hợp với **IoT Gateway** để thu thập dữ liệu thiết bị.
- **Client**: Web App phục vụ 4 vai trò: Requester, Technician, Facility Manager và Admin.
- **Server (Backend)**: RESTful API được xây dựng bằng **C# ASP.NET Core MVC**.
- **Database**: Cơ sở dữ liệu quan hệ **SQL Server** làm Source of Truth, lưu trữ dữ liệu nghiệp vụ và Lịch sử bảo trì.
- **IoT Gateway**: Điểm tiếp nhận dữ liệu từ các IoT Sensors (chu kỳ 5 phút/lần) trước khi đẩy về Backend xử lý.
- **AI Prediction Service**: Module AI phân tích dữ liệu IoT và Lịch sử bảo trì để xuất ra chỉ số Rủi ro bảo trì (Maintenance Risk).

## 2. Sơ đồ kiến trúc (Architecture Diagram)

```mermaid
graph TD
    subgraph Clients
        R[Requester UI]
        T[Technician UI]
        F[Facility Manager UI]
        A[Admin Panel]
    end

    subgraph Backend System
        API[ASP.NET Core Web API]
        Auth[Auth & RBAC Middleware]
        Logic[Business Logic Layer]
        Data[Data Access Layer / EF Core]
    end

    subgraph Data Storage
        DB[(SQL Server Database)]
    end

    subgraph IoT & AI
        Sensors[IoT Devices/Sensors]
        Gateway[IoT Gateway]
        AI_Engine[AI Predictive Service]
    end

    %% Flow Connections
    Clients -->|HTTPS/REST| API
    API --> Auth
    Auth --> Logic
    Logic --> Data
    Data --> DB
    
    Sensors -->|MQTT/HTTP| Gateway
    Gateway -->|IoT Data| API
    
    Logic -->|Request Prediction| AI_Engine
    DB -->|IoT Data + History| AI_Engine
    AI_Engine -->|Risk: Low/Med/High| Logic

