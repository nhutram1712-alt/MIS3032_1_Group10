# Data Requirements

# Data Requirements & Database Model

## 1. Entity-Relationship Diagram (ERD)

```mermaid
erDiagram
    USERS ||--o{ MAINTENANCE_REQUESTS : creates
    USERS ||--o{ WORK_ORDERS : assigned_to
    ASSETS ||--o{ MAINTENANCE_REQUESTS : relates_to
    ASSETS ||--o{ WORK_ORDERS : has
    ASSETS ||--o| IOT_MAPPINGS : mapped_via
    ASSETS ||--o{ MAINTENANCE_HISTORY : records
    IOT_DEVICES ||--o| IOT_MAPPINGS : connects
    IOT_DEVICES ||--o{ IOT_DATA : generates
    WORK_ORDERS ||--o| MAINTENANCE_HISTORY : results_in

    USERS {
        int UserID PK
        varchar Username
        varchar PasswordHash
        varchar Role "Requester, Technician, FacilityManager, Admin"
    }
    
    ASSETS {
        int AssetID PK
        varchar Name
        varchar Type "Wi-Fi, Air Conditioner, Projector, Light, Fan"
        varchar Location
        varchar Status "Operational, Warning, Maintenance, Out of Service"
    }

    MAINTENANCE_REQUESTS {
        int RequestID PK
        int RequesterID FK
        int AssetID FK
        varchar Description
        varchar Status "Submitted, Pending, In Progress, Resolved, Closed, Rejected"
        datetime CreatedAt
    }

    WORK_ORDERS {
        int OrderID PK
        int RequestID FK
        int TechnicianID FK
        int AssetID FK
        varchar Status "Assigned, In Progress, Completed, Cancelled"
        varchar RejectionReason
        datetime CreatedAt
    }

    IOT_DATA {
        int DataID PK
        int DeviceID FK
        float ReadingValue
        datetime Timestamp
    }


