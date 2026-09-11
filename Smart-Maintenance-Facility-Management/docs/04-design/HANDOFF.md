# HANDOFF SPECIFICATION

## 1. Foundations

| Frame name | Nội dung | Trạng thái |
| :--- | :--- | :--- |
| **Foundations/Colors** | Brand, Surface, Text, Status token | Có sẵn |
| **Foundations/Typography** | Type scale 6 cấp | Có sẵn |
| **Foundations/Spacing & Radius** | Spacing 4$ightarrow$48px, radius sm/md/lg | Có sẵn |
| **Foundations/Shadow** | shadow/card, shadow/modal | Có sẵn |

---

## 2. Components

| Frame name | Variants / States đã dựng | Trạng thái |
| :--- | :--- | :--- |
| **Components/Button** | primary (default/hover/disabled/loading), secondary, danger | Có sẵn |
| **Components/Input & Textarea** | default/focus/error, textarea + counter | Có sẵn |
| **Components/Dropdown** | default/disabled | Có sẵn |
| **Components/Badge** | info/success/warning/danger/neutral/rejected | Có sẵn |
| **Components/Card** | stat-card, list-row card | Có sẵn |
| **Components/Toast** | success, error | Có sẵn |
| **Components/Modal-Confirm** | xác nhận đóng yêu cầu | Có sẵn |
| **Components/Empty state** | icon + heading + mô tả | Có sẵn |
| **Components/Voice control** | idle/listening/disabled | Có sẵn — *[ASSUMPTION, chưa dùng trong MVP]* |

---

## 3. Flows — Requester

| Frame name | UC / US | BR liên quan | Trạng thái |
| :--- | :--- | :--- | :--- |
| **Requester/Overview/Default** | — | — | Có sẵn |
| **Requester/Requests/Default** | UC-05, UC-06 / US-03-01, US-03-02 | BR-05, BR-14 | Có sẵn |
| **Requester/Requests/Loading** | US-03-01 | — | Cần vẽ mới |
| **Requester/Requests/Empty** | US-03-01 | — | Cần vẽ mới |
| **Requester/Requests/Error-MissingAsset** | US-03-01 | BR-05 (AC-US-03-01-03) | Cần vẽ mới |
| **Requester/Requests/Confirmation** | US-03-02 | BR-14 | Cần vẽ mới *(toast + highlight dòng mới)* |

---

## 4. Flows — Facility Manager

| Frame name | UC / US | BR liên quan | Trạng thái |
| :--- | :--- | :--- | :--- |
| **FM/Overview/Default** | — | — | Có sẵn |
| **FM/Assets/Default** | — | — | Có sẵn |
| **FM/Assets/AddAssetForm** | — | — | Có sẵn |
| **FM/WorkOrder/Default** | UC-07, UC-08 / US-03-03, US-04-01/02 | BR-06 | Có sẵn |
| **FM/WorkOrder/Empty-NoPending** | US-04-01 | — | Cần vẽ mới |
| **FM/WorkOrder/Error-AlreadyHasWO** | US-04-02 | BR-06 | Cần vẽ mới |
| **FM/WorkOrder/Error-NoTechnicianSelected** | US-04-02 | AC-US-04-02-02 | Cần vẽ mới |
| **FM/WorkOrder/RejectRequestLabelFix** | UC-07, UC-08 | — | Cần sửa — đổi nhãn nút "Từ chối" $ightarrow$ "Từ chối yêu cầu" *(Q-22/DEC-12)* |
| **FM/WorkOrder/ConfirmCloseModal** | US-03-04 | BR-18 | Cần vẽ mới — **ưu tiên cao (DEC-09)** |
| **FM/IoTAlerts/Default** | UC-12$ightarrow$15 | BR-10 | Có sẵn |
| **FM/IoTAlerts/Empty** | UC-12 | — | Cần vẽ mới |
| **FM/IoTAlerts/Error-NoNewData** | UC-12 (A2) | — | Cần vẽ mới |
| **FM/AIRisks/Default** | UC-14, UC-15 | BR-10 | Có sẵn |
| **FM/AIRisks/Empty-InsufficientData** | UC-14 (A1) | — | Cần vẽ mới |

---

## 5. Flows — Technician

| Frame name | UC / US | BR liên quan | Trạng thái |
| :--- | :--- | :--- | :--- |
| **Technician/Overview/Default** | — | — | Có sẵn |
| **Technician/WorkOrder/Default** | UC-09, UC-10 | BR-07, BR-17 | Có sẵn |
| **Technician/WorkOrder/Error-MissingReason** | US-04-05 | AC-US-04-05-03 | Cần vẽ mới |
| **Technician/WorkOrder/Error-MissingResult** | US-04-06 | AC-US-04-06-03 | Cần vẽ mới |
| **Technician/WorkOrder/Permission-NotAssignedToMe** | US-04-04 | AC-US-04-04-03, BR-07 | Cần vẽ mới |
| **Technician/WorkOrder/Rejected** | UC-09, UC-10 / US-04-05 | BR-17 *(đề xuất mở rộng)* | Cần vẽ mới — **ưu tiên cao (OQ-01)** |
| **Technician/Assets/Default** | — | — | Có sẵn |
| **Technician/IoTAlerts/Default** | UC-15 *(bước 5)* | — | Có sẵn — *chỉ hiển thị asset liên quan WO của họ* |

---

## 6. Flows — Admin

| Frame name | UC / US | BR liên quan | Trạng thái |
| :--- | :--- | :--- | :--- |
| **Admin/Overview/Default** | — | — | Có sẵn |
| **Admin/Users/Default** | — | — | Có sẵn |
| **Admin/Users/AddUserForm** | — | — | Có sẵn |
| **Admin/IoTMapping/Default** | — | — | Có sẵn |
| **Admin/IoTMapping/Error-DuplicateMapping** | — | — | Cần vẽ mới |
