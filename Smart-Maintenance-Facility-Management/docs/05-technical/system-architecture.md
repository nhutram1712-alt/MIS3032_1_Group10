
# System Architecture

## 1. Kiến trúc tổng thể (System Overview)
Hệ thống Smart Maintenance Facility Management được thiết kế theo kiến trúc **Client-Server** kết hợp với **IoT Gateway** để thu thập dữ liệu thiết bị.
- **Client**: Web App phục vụ 4 vai trò: Requester, Technician, Facility Manager và Admin.
- **Server (Backend)**: RESTful API được xây dựng bằng **C# ASP.NET Core MVC**.
- **Database**: Cơ sở dữ liệu quan hệ **SQL Server** làm Source of Truth, lưu trữ dữ liệu nghiệp vụ và Lịch sử bảo trì.
- **IoT Gateway**: Điểm tiếp nhận dữ liệu từ các IoT Sensors (chu kỳ 5 phút/lần) trước khi đẩy về Backend xử lý.
- **AI Prediction Service**: Module AI phân tích dữ liệu IoT và Lịch sử bảo trì để xuất ra chỉ số Rủi ro bảo trì (Maintenance Risk).

## 2. Cơ chế Auth và Logging
- **Authentication & RBAC**: Sử dụng JWT (JSON Web Token). Phân quyền chặt chẽ 4 Role: Requester, Technician, Facility Manager, Admin. Không sử dụng SSO ngoài để hệ thống gọn nhẹ, phù hợp với MVP.
- **Logging**: Ghi log lỗi hệ thống và API traffic bằng thư viện Serilog. Các thay đổi trạng thái quan trọng (tạo Work Order, đổi Status Asset) bắt buộc ghi Audit Log vào cơ sở dữ liệu để phục vụ truy vết.

## 3. Architecture Decision Record (ADR)
### ADR-001: Lựa chọn Tech Stack Backend và Database
- **Decision**: Sử dụng C# ASP.NET Core MVC cho Backend và SQL Server cho Database.
- **Trade-off & Rationale (Lý do)**: 
  - Đứng trước bài toán thu thập dữ liệu IoT, một lựa chọn phổ biến là dùng cơ sở dữ liệu NoSQL (như MongoDB) để ghi log linh hoạt. Đánh đổi (Trade-off) ở đây là chúng ta từ chối sự linh hoạt của NoSQL để chọn tính toàn vẹn, chặt chẽ (ACID) của cơ sở dữ liệu quan hệ SQL Server.
  - Sự đánh đổi này là hoàn toàn xứng đáng vì Core Business của hệ thống là quản lý Asset và Maintenance History với các ràng buộc thực thể rất phức tạp. Đồng thời, cấu trúc bảng mạch lạc của SQL Server hỗ trợ cực tốt cho việc trích xuất dữ liệu mỏ (Data Mining) và thiết kế Data Warehousing sau này.
  - Lựa chọn này giúp tận dụng tối đa lợi thế chuyên môn hiện có về phát triển backend C# và tối ưu query, đảm bảo tốc độ triển khai (velocity) mà không rơi vào tình trạng over-engineer so với scope của một MVP.
- **Status**: Approved.
    Logic -->|Request Prediction| AI_Engine
    DB -->|IoT Data + History| AI_Engine
    AI_Engine -->|Risk: Low/Med/High| Logic

## 4. Sơ đồ kiến trúc (Architecture Diagram)

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
