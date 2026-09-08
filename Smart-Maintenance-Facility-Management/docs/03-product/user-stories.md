# User Stories
**Project:** Smart Maintenance & Facility Management
**Organization:** Trường Đại học Kinh tế – Đại học Đà Nẵng
**Version:** 1.0
**Status:** Draft for Acceptance Criteria Review
**Role:** Business Analyst

---

## 1. Overview

This document decomposes the baselined functional requirements into user-centered Epics and User Stories.

The User Stories are derived from **Requirements v1.0 (Baselined)** and mapped to the relevant Business Rules. No new product scope is introduced in this document.

### User Story Format

> As a [role], I want [action], so that [benefit].

### Traceability Principle

Each User Story should be traceable to one or more:

* Functional Requirements (REQ)
* Business Rules (BR)
* Non-Functional Requirements (NFR), where applicable
* Design artifacts, when available

---

# 2. Epic Overview

| Epic ID   | Epic                             | Objective                                                              | User Stories |
| --------- | -------------------------------- | ---------------------------------------------------------------------- | -----------: |
| EPIC-01   | Authentication & Access Control  | Provide secure access and role-based permissions.                      |            4 |
| EPIC-02   | Asset Management                 | Manage Asset information and operational status.                       |            4 |
| EPIC-03   | Maintenance Request              | Report, track, process, and close maintenance requests.                |            4 |
| EPIC-04   | Work Order & Maintenance History | Create, assign, execute, complete, and record maintenance work.        |            6 |
| EPIC-05   | IoT Monitoring                   | Connect Assets to IoT Devices/Sensors and monitor conditions.          |            4 |
| EPIC-06   | AI Predictive Maintenance        | Predict maintenance risk and support preventive maintenance decisions. |            3 |
| **Total** |                                  |                                                                        |       **25** |

---

# 3. EPIC-01 — Authentication & Access Control

**Objective:** Allow users to access the system and ensure access is appropriate to their Role.

## US-01-01 — Login

**User Story**

> As a **User**, I want to log in with my system account, so that I can access the system securely.

**Priority:** Must

**Requirement Mapping:**

* REQ-01

**Business Rule Mapping:**

* None

**NFR Mapping:**

* NFR-01
* NFR-02

**Design:** Not designed yet

---

## US-01-02 — View Authorized Assets

**User Story**

> As a **Requester**, I want to view only Assets in rooms/areas I am allowed to use, so that I only access relevant Asset information.

**Priority:** Should

**Requirement Mapping:**

* REQ-02

**Business Rule Mapping:**

* BR-04

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Not designed yet

---

## US-01-03 — Manage User Accounts

**User Story**

> As an **Admin**, I want to manage user accounts, so that system access can be maintained.

**Priority:** Should

**Requirement Mapping:**

* REQ-24

**Business Rule Mapping:**

* None

**NFR Mapping:**

* NFR-01
* NFR-02

**Design:** Not designed yet

---

## US-01-04 — Manage Role Permissions

**User Story**

> As an **Admin**, I want to manage access permissions by Role, so that users can only access authorized functions.

**Priority:** Should

**Requirement Mapping:**

* REQ-25

**Business Rule Mapping:**

* None

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Not designed yet

---

# 4. EPIC-02 — Asset Management

**Objective:** Allow Facility Manager to maintain Asset information and status.

## US-02-01 — Add Asset

**User Story**

> As a **Facility Manager**, I want to add an Asset with its basic information, so that the Asset can be managed in the system.

**Priority:** Must

**Requirement Mapping:**

* REQ-06

**Business Rule Mapping:**

* BR-01
* BR-02
* BR-03
* BR-04

**NFR Mapping:**

* NFR-03
* NFR-04
* NFR-06

**Design:** Not designed yet

---

## US-02-02 — Update Asset

**User Story**

> As a **Facility Manager**, I want to update Asset information, so that Asset data remains accurate.

**Priority:** Must

**Requirement Mapping:**

* REQ-07

**Business Rule Mapping:**

* BR-01
* BR-02
* BR-04

**NFR Mapping:**

* NFR-03
* NFR-04
* NFR-06

**Design:** Not designed yet

---

## US-02-03 — Manage Asset Status

**User Story**

> As a **Facility Manager**, I want to view Asset information and manually change its status, so that I can maintain the current operational status.

**Priority:** Must

**Requirement Mapping:**

* REQ-08

**Business Rule Mapping:**

* BR-16

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-02-04 — View Asset Status

**User Story**

> As a **Requester**, I want to view the current status of an Asset I am allowed to access, so that I know whether the Asset is available or has an issue.

**Priority:** Should

**Requirement Mapping:**

* REQ-03

**Business Rule Mapping:**

* BR-04

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Not designed yet

---

# 5. EPIC-03 — Maintenance Request

**Objective:** Allow Requesters to report problems and Facility Managers to process maintenance requests.

## US-03-01 — Create Maintenance Request

**User Story**

> As a **Requester**, I want to create a Maintenance Request for an Asset or affected area, so that I can report a maintenance problem.

**Priority:** Must

**Requirement Mapping:**

* REQ-04

**Business Rule Mapping:**

* BR-05

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-03-02 — Track Maintenance Request

**User Story**

> As a **Requester**, I want to track my Maintenance Request status, so that I know the progress of my request.

**Priority:** Must

**Requirement Mapping:**

* REQ-05

**Business Rule Mapping:**

* BR-14

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-03-03 — Process Maintenance Request

**User Story**

> As a **Facility Manager**, I want to receive and process Maintenance Requests, so that reported problems can be handled.

**Priority:** Must

**Requirement Mapping:**

* REQ-13

**Business Rule Mapping:**

* BR-05
* BR-14

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-03-04 — Confirm and Close Maintenance Request

**User Story**

> As a **Facility Manager**, I want to confirm the maintenance result before closing a Request, so that only verified requests are closed.

**Priority:** Must

**Requirement Mapping:**

* REQ-13

**Business Rule Mapping:**

* BR-18

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

# 6. EPIC-04 — Work Order & Maintenance History

**Objective:** Manage the execution of maintenance work from Work Order creation through completion and history recording.

## US-04-01 — Create Work Order

**User Story**

> As a **Facility Manager**, I want to create a Work Order from a Maintenance Request or identified maintenance need, so that maintenance work can be formally assigned and tracked.

**Priority:** Must

**Requirement Mapping:**

* REQ-14

**Business Rule Mapping:**

* BR-05
* BR-06
* BR-08

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-04-02 — Assign Work Order

**User Story**

> As a **Facility Manager**, I want to assign a Work Order to a Technician, so that the maintenance task has a responsible person.

**Priority:** Must

**Requirement Mapping:**

* REQ-15

**Business Rule Mapping:**

* BR-07

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-04-03 — Track Work Order

**User Story**

> As a **Facility Manager**, I want to track Work Order status, so that I can monitor maintenance progress.

**Priority:** Must

**Requirement Mapping:**

* REQ-16

**Business Rule Mapping:**

* BR-17

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

## US-04-04 — View Assigned Work Order

**User Story**

> As a **Technician**, I want to view my assigned Work Orders and related Asset information, so that I know what maintenance work I need to perform.

**Priority:** Must

**Requirement Mapping:**

* REQ-17
* REQ-18

**Business Rule Mapping:**

* BR-07
* BR-08

**NFR Mapping:**

* NFR-01
* NFR-07

**Design:** Not designed yet

---

## US-04-05 — Update or Reject Work Order

**User Story**

> As a **Technician**, I want to update or reject an assigned Work Order with a reason, so that the Work Order accurately reflects my ability to perform the task.

**Priority:** Must

**Requirement Mapping:**

* REQ-20

**Business Rule Mapping:**

* BR-07
* BR-17

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

**Open Question:** The current Requirements define `Rejected` as a Technician action but do not define `Rejected` as a Work Order lifecycle status. Confirm whether rejection is an action while the Work Order remains `Assigned`, or whether `Rejected` should become a formal status.

---

## US-04-06 — Complete Work Order and Record Maintenance History

**User Story**

> As a **Technician**, I want to record maintenance results and complete the Work Order, so that the maintenance outcome is documented and stored in Asset history.

**Priority:** Must

**Requirement Mapping:**

* REQ-21
* REQ-22
* REQ-23

**Business Rule Mapping:**

* BR-09
* BR-18

**NFR Mapping:**

* NFR-03
* NFR-04

**Design:** Not designed yet

---

# 7. EPIC-05 — IoT Monitoring

**Objective:** Connect Assets to IoT Devices/Sensors, collect data, monitor conditions, and generate alerts.

## US-05-01 — Map Asset to IoT Device

**User Story**

> As an **Admin**, I want to map an Asset with an IoT Device/Sensor, so that IoT data can be associated with the correct Asset.

**Priority:** Must

**Requirement Mapping:**

* REQ-26

**Business Rule Mapping:**

* BR-13

**NFR Mapping:**

* NFR-06

**Design:** Not designed yet

---

## US-05-02 — Update IoT Mapping

**User Story**

> As an **Admin**, I want to update IoT Mapping, so that changes in IoT configuration can be maintained.

**Priority:** Must

**Requirement Mapping:**

* REQ-27

**Business Rule Mapping:**

* BR-13

**NFR Mapping:**

* NFR-06

**Design:** Not designed yet

---

## US-05-03 — Monitor IoT Data

**User Story**

> As a **Facility Manager**, I want to view IoT Data of an Asset, so that I can monitor its current condition.

**Priority:** Must

**Requirement Mapping:**

* REQ-09
* REQ-28

**Business Rule Mapping:**

* BR-12
* BR-13

**NFR Mapping:**

* NFR-04
* NFR-05
* NFR-06

**Design:** Not designed yet

---

## US-05-04 — Handle IoT Alert

**User Story**

> As a **Facility Manager**, I want to receive IoT Alerts when abnormal conditions are detected, so that I can respond to potential maintenance problems.

**Priority:** Must

**Requirement Mapping:**

* REQ-10
* REQ-30

**Business Rule Mapping:**

* BR-12

**NFR Mapping:**

* NFR-04

**Design:** Not designed yet

---

# 8. EPIC-06 — AI Predictive Maintenance

**Objective:** Use IoT Data and Maintenance History to predict maintenance risk and support preventive maintenance decisions.

## US-06-01 — AI Maintenance Prediction

**User Story**

> As a **Facility Manager**, I want to receive an AI prediction of whether an Asset may require maintenance within the next 7 days, so that I can prioritize preventive maintenance.

**Priority:** Must

**Requirement Mapping:**

* REQ-11
* REQ-29

**Business Rule Mapping:**

* BR-10
* BR-11
* BR-15

**NFR Mapping:**

* NFR-08

**Design:** Not designed yet

---

## US-06-02 — View Maintenance Risk

**User Story**

> As a **Facility Manager**, I want to view the Asset Maintenance Risk as Low, Medium, or High, so that I can understand the predicted maintenance risk.

**Priority:** Must

**Requirement Mapping:**

* REQ-12

**Business Rule Mapping:**

* BR-10
* BR-15

**NFR Mapping:**

* NFR-08

**Design:** Not designed yet

---

## US-06-03 — View AI Prediction and IoT Alert

**User Story**

> As a **Technician**, I want to view IoT Alerts and AI Predictions related to my Asset, so that I can understand potential problems before performing maintenance.

**Priority:** Must

**Requirement Mapping:**

* REQ-19

**Business Rule Mapping:**

* BR-10
* BR-11

**NFR Mapping:**

* NFR-08

**Design:** Not designed yet

---

# 9. Requirement Traceability Summary

## 9.1 Functional Requirements → User Stories

| Requirement | User Story         |
| ----------- | ------------------ |
| REQ-01      | US-01-01           |
| REQ-02      | US-01-02           |
| REQ-03      | US-02-04           |
| REQ-04      | US-03-01           |
| REQ-05      | US-03-02           |
| REQ-06      | US-02-01           |
| REQ-07      | US-02-02           |
| REQ-08      | US-02-03           |
| REQ-09      | US-05-03           |
| REQ-10      | US-05-04           |
| REQ-11      | US-06-01           |
| REQ-12      | US-06-02           |
| REQ-13      | US-03-03, US-03-04 |
| REQ-14      | US-04-01           |
| REQ-15      | US-04-02           |
| REQ-16      | US-04-03           |
| REQ-17      | US-04-04           |
| REQ-18      | US-04-04           |
| REQ-19      | US-06-03           |
| REQ-20      | US-04-05           |
| REQ-21      | US-04-06           |
| REQ-22      | US-04-06           |
| REQ-23      | US-04-06           |
| REQ-24      | US-01-03           |
| REQ-25      | US-01-04           |
| REQ-26      | US-05-01           |
| REQ-27      | US-05-02           |
| REQ-28      | US-05-03           |
| REQ-29      | US-06-01           |
| REQ-30      | US-05-04           |

**Coverage: 30/30 Functional Requirements mapped.**

---

## 9.2 Business Rules → User Stories

| Business Rule | User Story                             |
| ------------- | -------------------------------------- |
| BR-01         | US-02-01, US-02-02                     |
| BR-02         | US-02-01, US-02-02                     |
| BR-03         | US-02-01                               |
| BR-04         | US-01-02, US-02-01, US-02-02, US-02-04 |
| BR-05         | US-03-01, US-03-03, US-04-01           |
| BR-06         | US-04-01                               |
| BR-07         | US-04-02, US-04-04, US-04-05           |
| BR-08         | US-04-01, US-04-04                     |
| BR-09         | US-04-06                               |
| BR-10         | US-06-01, US-06-02, US-06-03           |
| BR-11         | US-06-01, US-06-03                     |
| BR-12         | US-05-03, US-05-04                     |
| BR-13         | US-05-01, US-05-02, US-05-03           |
| BR-14         | US-03-02, US-03-03                     |
| BR-15         | US-06-01, US-06-02                     |
| BR-16         | US-02-03                               |
| BR-17         | US-04-03, US-04-05                     |
| BR-18         | US-03-04, US-04-06                     |

**Coverage: 18/18 Business Rules mapped.**

---

# 10. Non-Functional Requirement Mapping

NFRs are treated as cross-cutting constraints rather than independent User Stories.

| NFR    | Related Epic / User Stories        |
| ------ | ---------------------------------- |
| NFR-01 | EPIC-01; role-specific Stories     |
| NFR-02 | US-01-01, US-01-03                 |
| NFR-03 | EPIC-03, EPIC-04                   |
| NFR-04 | EPIC-02, EPIC-03, EPIC-04, EPIC-05 |
| NFR-05 | US-05-03                           |
| NFR-06 | EPIC-02, EPIC-05                   |
| NFR-07 | EPIC-01 and role-specific UI       |
| NFR-08 | EPIC-06                            |

---

# 11. Out of Scope

The following are excluded from the MVP according to the current Requirements baseline:

* Finance Management
* Procurement
* Inventory Management
* Spare Parts Management
* Supplier/Vendor Management
* HR Management
* Academic Management
* Student Management
* General University Administration
* Automatic AI-based maintenance decisions
* SSO integration

---

# 12. Definition of Ready

A User Story is considered **Ready for Sprint** when:

* [ ] The Story follows the `As a / I want / so that` format.
* [ ] The user Role is clearly identified.
* [ ] The expected business/user value is clear.
* [ ] At least one Requirement ID is linked.
* [ ] Relevant Business Rules are linked.
* [ ] Acceptance Criteria are written in Given/When/Then format.
* [ ] The happy path is covered.
* [ ] Important validation/error cases are covered.
* [ ] Dependencies are identified.
* [ ] Design is available or explicitly marked as not required.
* [ ] No unresolved Open Question blocks implementation.
* [ ] The Story is small enough to be completed within approximately 1–3 days.

---

# 13. Open Questions

## OQ-01 — Work Order Rejection Status

The Requirements allow a Technician to reject a Work Order with a reason (REQ-20 / BR-07), but the defined Work Order statuses are only:

* Assigned
* In Progress
* Completed
* Cancelled

The project must confirm whether:

1. `Reject` is an action while the Work Order remains `Assigned`; or
2. `Rejected` should be added as a formal Work Order status.

**Status:** Open

---

# 14. Next Step

The next artifact to produce from this document is:

**Acceptance Criteria**

Each User Story should receive **2–6 Given/When/Then Acceptance Criteria**, covering:

* Happy path
* Required validation
* Important error/exception cases
* Relevant Business Rules

Acceptance Criteria should then be linked back to the Requirement and User Story IDs through the Traceability Matrix.

