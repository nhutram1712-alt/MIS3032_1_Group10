# figma-handoff

# Smart Campus Property & Facility Management — Design Prototype & Engineering Handoff

## 1. Trạng thái tài liệu

- **Design system:** Smart Campus Core v0.1
- **Mục tiêu:** Ứng dụng web responsive
- **Frontend đề xuất:** React + TypeScript
- **Nguồn chuẩn:** `vault/`
- **Cơ sở áp dụng:** Trường Đại học Kinh tế - Đại học Đà Nẵng
- **Tài sản trong MVP:** Wifi, điều hòa, máy chiếu
- **Người dùng chính:** Customer, Technician, Facility Manager, Administrator
- **Múi giờ:** `Asia/Ho_Chi_Minh`; timestamp hệ thống có thể lưu ở UTC
- **Mục tiêu accessibility:** WCAG 2.1 Level AA
- **Trạng thái:** DESIGN PROTOTYPE — sẵn sàng cho bước thiết kế Figma và lập kế hoạch triển khai, nhưng các câu hỏi mở ở mục 15 vẫn cần được xác nhận

Tài liệu này định nghĩa hệ thống hình ảnh, cấu trúc màn hình, hành vi component, responsive, trạng thái giao diện và luồng prototype cho hệ thống Smart Campus Property & Facility Management.

Tài liệu này **không thay thế** Functional Requirements, Business Rules, Use Cases, User Stories, Acceptance Criteria hoặc Non-functional Requirements trong Vault.

### 1.1. Mức độ thẩm quyền

Mỗi quyết định thiết kế trong tài liệu thuộc một trong ba mức:

1. **Bắt buộc:** Được hỗ trợ trực tiếp bởi FR, BR, UC, US, AC hoặc NFR đã xác nhận.
2. **Quyết định prototype:** Lựa chọn UI có thể thay đổi, được dùng để prototype có thể hoạt động mạch lạc.
3. **Câu hỏi mở:** Hành vi chưa được xác nhận. Engineering không được xem prototype là phê duyệt cuối cùng cho hành vi này.

Nếu nội dung tài liệu này mâu thuẫn với Requirement trong Vault thì **Requirement được ưu tiên**.

---

## 2. Phạm vi và truy vết yêu cầu

| Khu vực | User Story | Use Case | Functional Requirement | Business Rules chính |
|---|---|---|---|---|
| Đăng nhập | US-001 | UC-001 | FR-001 | BR-011 |
| Gửi yêu cầu báo sự cố | US-002 | UC-002 | FR-002 | BR-003, BR-011 |
| Theo dõi yêu cầu đã gửi | US-003 | UC-003 | FR-003 | BR-003, BR-011 |
| Quản lý tài sản | US-004 | UC-004 | FR-004 | BR-001, BR-002, BR-010, BR-011 |
| Giám sát thiết bị qua IoT | US-005 | UC-005 | FR-005 | BR-006, BR-007, BR-011 |
| AI dự đoán sự cố và bảo trì | US-006 | UC-006 | FR-006 | BR-008, BR-009 |
| Tạo và phân công Work Order | US-007 | UC-007 | FR-007 | BR-004, BR-005, BR-011 |
| Xem Work Order được giao | US-008 | UC-008 | FR-008 | BR-004, BR-005, BR-011 |
| Cập nhật kết quả sửa chữa | US-009 | UC-009 | FR-009 | BR-005, BR-010 |
| Quản lý tài khoản và phân quyền | US-010 | UC-010 | FR-010 | BR-011 |
| Quản lý lịch trình bảo trì | US-011 | UC-011 | FR-011 | BR-008, BR-009, BR-010 |

Các màn hình IoT và AI phải bám trực tiếp vào UC/US/AC đã xây dựng cho FR-005 và FR-006.

Nếu một UC/US/AC khác chưa có file riêng trong Vault, mã ở bảng trên được dùng làm **mã truy vết prototype** và cần được đồng bộ lại khi nhóm hoàn thiện tài liệu phân tích.

---

## 3. Nguyên tắc trải nghiệm

1. **Tài sản là trung tâm.** Sự cố, dữ liệu IoT, dự đoán AI, Work Order và lịch bảo trì đều phải liên kết về một Asset ID.
2. **AI chỉ hỗ trợ quyết định.** Giao diện không được thể hiện dự đoán AI như một quyết định chắc chắn hoặc tự động tạo kết luận thay cho Facility Manager.
3. **Dữ liệu IoT phải thể hiện độ mới.** Người dùng phải biết dữ liệu đang cập nhật, cũ hay mất kết nối.
4. **Customer chỉ thấy phần việc của mình.** Giảng viên, sinh viên và nhân viên chỉ được báo sự cố và theo dõi yêu cầu của chính mình.
5. **Technician tập trung vào Work Order được giao.** Không hiển thị các chức năng quản trị không cần thiết.
6. **Facility Manager có cái nhìn vận hành.** Các tài sản, cảnh báo IoT, dự đoán AI, Work Order và lịch bảo trì được đặt trong cùng một nhóm điều hướng.
7. **Administrator chỉ quản lý tài khoản và quyền truy cập.** Không trộn tác vụ quản trị hệ thống vào luồng kỹ thuật hàng ngày.
8. **Lỗi không làm mất dữ liệu đã nhập.** Khi submit thất bại, nội dung biểu mẫu phải được giữ lại.
9. **Trạng thái không chỉ thể hiện bằng màu.** Badge luôn có text và có thể bổ sung icon.
10. **Không thêm tính năng ngoài MVP.** Prototype không mở rộng sang tài chính, mua sắm, quản lý nhân sự, hoặc tự động hóa sửa chữa hoàn toàn.

---

## 4. Kiến trúc thông tin

Các route bên dưới là **quyết định prototype** để tổ chức frontend; không phải yêu cầu bắt buộc của API.

| Route | Màn hình | Quyền truy cập | Traceability |
|---|---|---|---|
| `/login` | Đăng nhập | Chưa đăng nhập | FR-001 |
| `/report-issue` | Báo sự cố tài sản | Customer | FR-002 |
| `/my-requests` | Yêu cầu của tôi | Customer | FR-003 |
| `/assets` | Danh sách tài sản | Facility Manager | FR-004 |
| `/assets/:assetId` | Chi tiết tài sản | Facility Manager, Technician khi có Work Order liên quan | FR-004, FR-005 |
| `/iot-monitoring` | Giám sát IoT | Facility Manager | FR-005 |
| `/ai-predictions` | Dự đoán AI | Facility Manager | FR-006 |
| `/work-orders` | Quản lý Work Order | Facility Manager, Technician | FR-007, FR-008, FR-009 |
| `/work-orders/:workOrderId` | Chi tiết Work Order | Facility Manager, Technician được giao | FR-008, FR-009 |
| `/maintenance-schedule` | Lịch trình bảo trì | Facility Manager | FR-011 |
| `/admin/users` | Tài khoản và phân quyền | Administrator | FR-010 |

### 4.1. Mô hình điều hướng

**Customer**

- Báo sự cố
- Yêu cầu của tôi

**Technician**

- Work Order của tôi

**Facility Manager**

- Tài sản
- Giám sát IoT
- Dự đoán AI
- Work Order
- Lịch bảo trì

**Administrator**

- Tài khoản & phân quyền

**Quyết định prototype:** Sau khi đăng nhập, người dùng được chuyển đến màn hình mặc định theo vai trò:

- Customer → `/my-requests`
- Technician → `/work-orders`
- Facility Manager → `/assets`
- Administrator → `/admin/users`

---

## 5. Smart Campus Core design system

### 5.1. Color tokens

Sử dụng semantic token trong component. Không hard-code màu trực tiếp trong từng feature.

```css
:root {
  color-scheme: light;

  --sc-color-brand-50: #eff6ff;
  --sc-color-brand-100: #dbeafe;
  --sc-color-brand-600: #2563eb;
  --sc-color-brand-700: #1d4ed8;
  --sc-color-brand-800: #1e40af;

  --sc-color-neutral-0: #ffffff;
  --sc-color-neutral-50: #f8fafc;
  --sc-color-neutral-100: #f1f5f9;
  --sc-color-neutral-200: #e2e8f0;
  --sc-color-neutral-300: #cbd5e1;
  --sc-color-neutral-500: #64748b;
  --sc-color-neutral-700: #334155;
  --sc-color-neutral-900: #0f172a;

  --sc-color-success-50: #ecfdf3;
  --sc-color-success-700: #067647;

  --sc-color-warning-50: #fffaeb;
  --sc-color-warning-700: #b54708;

  --sc-color-danger-50: #fef3f2;
  --sc-color-danger-700: #b42318;

  --sc-color-info-50: #eff8ff;
  --sc-color-info-700: #175cd3;

  --sc-color-bg-page: var(--sc-color-neutral-50);
  --sc-color-bg-surface: var(--sc-color-neutral-0);
  --sc-color-border: var(--sc-color-neutral-200);
  --sc-color-text: var(--sc-color-neutral-900);
  --sc-color-text-muted: var(--sc-color-neutral-700);
  --sc-color-text-subtle: var(--sc-color-neutral-500);
  --sc-color-action: var(--sc-color-brand-600);
  --sc-color-action-hover: var(--sc-color-brand-700);
  --sc-color-focus: #2e90fa;
}
```

Semantic usage:

| Ý nghĩa | Foreground | Background | Sử dụng |
|---|---|---|---|
| Primary action | `neutral-0` | `brand-600` | Lưu, xác nhận, tạo Work Order |
| Neutral action | `neutral-900` | `neutral-0` | Quay lại, hủy |
| Normal/Online | `success-700` | `success-50` | Thiết bị hoạt động, IoT online |
| Cảnh báo | `warning-700` | `warning-50` | IoT bất thường, sắp đến hạn bảo trì |
| Nguy cơ cao/Lỗi | `danger-700` | `danger-50` | AI risk cao, Work Order lỗi xử lý |
| Thông tin | `info-700` | `info-50` | Dự đoán AI, ghi chú hệ thống |

Không dùng màu làm tín hiệu trạng thái duy nhất.

### 5.2. Typography

Ưu tiên `Inter`; nếu không bundle font thì dùng system stack.

```css
--sc-font-sans: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont,
  "Segoe UI", sans-serif;

--sc-text-xs: 0.75rem;
--sc-text-sm: 0.875rem;
--sc-text-md: 1rem;
--sc-text-lg: 1.125rem;
--sc-text-xl: 1.25rem;
--sc-text-2xl: 1.5rem;
--sc-text-3xl: 1.875rem;
```

| Vai trò | Cỡ chữ | Weight | Sử dụng |
|---|---|---:|---|
| Display | 30/38 | 700 | Màn hình đăng nhập |
| Page title | 24/32 | 700 | H1 |
| Section title | 20/30 | 650 | H2 |
| Card title | 16/24 | 650 | Asset/Work Order title |
| Body | 16/24 | 400 | Nội dung chính |
| UI label | 14/20 | 600 | Label, button |
| Supporting | 14/20 | 400 | Metadata, help text |
| Caption | 12/16 | 500 | Badge, timestamp |

### 5.3. Spacing, size và elevation

Dùng base unit 4px.

```css
--sc-space-0: 0;
--sc-space-1: 0.25rem;
--sc-space-2: 0.5rem;
--sc-space-3: 0.75rem;
--sc-space-4: 1rem;
--sc-space-5: 1.25rem;
--sc-space-6: 1.5rem;
--sc-space-8: 2rem;
--sc-space-10: 2.5rem;
--sc-space-12: 3rem;
--sc-space-16: 4rem;

--sc-radius-sm: 0.375rem;
--sc-radius-md: 0.5rem;
--sc-radius-lg: 0.75rem;
--sc-radius-pill: 999px;

--sc-shadow-sm: 0 1px 2px rgb(15 23 42 / 0.06);
--sc-shadow-md: 0 8px 24px rgb(15 23 42 / 0.10);

--sc-control-height-sm: 2rem;
--sc-control-height-md: 2.5rem;
--sc-control-height-lg: 3rem;
--sc-sidebar-width: 15rem;
--sc-content-max: 90rem;
```

Minimum pointer target: **44 × 44px**.

### 5.4. Layout grid và breakpoint

| Khoảng | Layout |
|---|---|
| `< 768px` | Một cột, gutter 16px, navigation drawer |
| `768–1199px` | 8-column grid, gutter 24px, navigation rail |
| `≥ 1200px` | 12-column grid, gutter 32px, sidebar 240px |

- Card tài sản: 1 cột mobile, 2 cột tablet, 3 cột desktop.
- Form: 1 cột mobile; tối đa khoảng 720px desktop.
- Bảng quản trị: dùng ở ≥768px; mobile chuyển thành card.

### 5.5. Motion và layering

```css
--sc-duration-fast: 120ms;
--sc-duration-normal: 180ms;
--sc-ease-standard: cubic-bezier(0.2, 0, 0, 1);

--sc-z-header: 20;
--sc-z-drawer: 40;
--sc-z-modal: 60;
--sc-z-toast: 80;
```

- Animation chỉ dùng để phản hồi hoặc thể hiện thay đổi trạng thái.
- Hỗ trợ `prefers-reduced-motion: reduce`.
- Không trì hoãn submit hoặc điều hướng để chờ animation.

---

## 6. Application shell

### 6.1. Desktop shell

```text
┌─────────────────────────────────────────────────────────────────────────┐
│ Sidebar 240px │ Header: Page context                         User menu   │
│               ├─────────────────────────────────────────────────────────│
│ Smart Campus  │ Breadcrumb                                               │
│               │ H1 + supporting text                     Primary action │
│ Assets        │                                                         │
│ IoT Monitoring│ Main content                                             │
│ AI Predictions│                                                         │
│ Work Orders   │                                                         │
│ Maintenance   │                                                         │
│ ────────────  │                                                         │
│ Administration│                                                         │
│ Users & Roles │                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

- Header desktop: 64px.
- Sidebar có border phải 1px.
- Navigation item active dùng brand-50, brand-700 và left indicator 3px.
- Menu hiển thị theo vai trò, không hiển thị action mà người dùng không có quyền.

### 6.2. Mobile shell

- Header có menu button, tên hệ thống và user menu.
- Navigation mở dưới dạng drawer bên trái.
- Drawer trap focus và đóng khi nhấn Escape.
- Main content bắt đầu dưới header 56px.
- Gutter 16px.

---

## 7. Core components

### 7.1. Button

Variants:

- `primary`
- `secondary`
- `tertiary`
- `danger`
- `icon`

States:

- default
- hover
- active
- focus-visible
- disabled
- loading

Nguyên tắc:

- Desktop height mặc định: 40px.
- Mobile height mặc định: 44px.
- Loading giữ nguyên chiều rộng button.
- Một khu vực quyết định chỉ nên có một primary action.

### 7.2. Form controls

Components:

- `TextField`
- `TextArea`
- `Select`
- `DateField`
- `TimeField`
- `DateTimeField`
- `Checkbox`
- `RadioGroup`
- `SearchField`

Anatomy:

`label → required marker → control → help text → error text`

Validation:

- Field đơn giản kiểm tra khi blur.
- Toàn form kiểm tra khi submit.
- Khi lỗi, focus vào validation summary.
- Không xóa dữ liệu người dùng đã nhập.
- Server validation là nguồn cuối cùng.

### 7.3. StatusBadge

| Domain | State | Label |
|---|---|---|
| Asset | NORMAL | Hoạt động |
| Asset | WARNING | Cần kiểm tra |
| Asset | MAINTENANCE | Đang bảo trì |
| Asset | OFFLINE | Không kết nối |
| IoT | ONLINE | Đang cập nhật |
| IoT | STALE | Dữ liệu cũ |
| IoT | OFFLINE | Mất kết nối |
| AI | LOW | Nguy cơ thấp |
| AI | MEDIUM | Nguy cơ trung bình |
| AI | HIGH | Nguy cơ cao |
| Work Order | NEW | Mới |
| Work Order | IN_PROGRESS | Đang xử lý |
| Work Order | COMPLETED | Đã hoàn thành |
| Work Order | CLOSED | Đã đóng |
| Schedule | UPCOMING | Sắp tới |
| Schedule | DUE | Đến hạn |
| Schedule | DONE | Đã thực hiện |

### 7.4. Asset card

```text
┌────────────────────────────────────────┐
│ Máy chiếu A201              Hoạt động  │
│ Asset ID: PJ-A201-01                    │
│ Phòng A201                              │
│                                         │
│ IoT: Đang cập nhật                      │
│ Bảo trì gần nhất: 12/08/2026            │
│                            [Xem chi tiết]│
└────────────────────────────────────────┘
```

Nội dung tối thiểu:

- Tên tài sản
- Asset ID
- Loại
- Vị trí
- Trạng thái
- Trạng thái IoT nếu có

### 7.5. IoT status card

```text
┌────────────────────────────────────────┐
│ Điều hòa B305                    Online │
│ Nhiệt độ: 26°C                         │
│ Thời gian cập nhật: 10:35              │
│ Trạng thái: Bình thường                │
│                           [Xem tài sản] │
└────────────────────────────────────────┘
```

Nếu dữ liệu quá cũ hoặc mất kết nối, card phải hiển thị rõ `Dữ liệu không còn cập nhật`.

### 7.6. AI Prediction card

```text
┌────────────────────────────────────────────┐
│ Máy chiếu A201               Nguy cơ cao   │
│ Dự đoán: Có khả năng cần bảo trì sớm       │
│ Dữ liệu: IoT + lịch sử bảo trì             │
│ Khuyến nghị: Kiểm tra trong 3 ngày tới     │
│                      [Xem tài sản] [Xử lý] │
└────────────────────────────────────────────┘
```

UI phải ghi rõ:

- Dự đoán AI là **khuyến nghị hỗ trợ**.
- Facility Manager là người quyết định có tạo Work Order hay không.

### 7.7. Work Order card/row

Nội dung đề xuất:

- Work Order ID
- Asset
- Vị trí
- Nguồn: Customer / IoT / AI / Schedule
- Technician
- Trạng thái
- Thời gian tạo/cập nhật

Technician chỉ thấy Work Order được giao cho mình.

### 7.8. Maintenance schedule row

Nội dung:

- Asset
- Loại tài sản
- Ngày bảo trì dự kiến
- Nội dung bảo trì
- Nguồn lịch: Định kỳ / Khuyến nghị AI
- Trạng thái

### 7.9. Table

Dùng cho:

- Asset management
- Work Order management
- Maintenance schedule
- User & role management

Ở mobile, table chuyển thành labelled cards.

### 7.10. Dialog

Dùng cho:

- Tạo Work Order từ cảnh báo IoT
- Tạo Work Order từ dự đoán AI
- Xác nhận đóng Work Order
- Xác nhận thay đổi vai trò

- Width desktop: khoảng 480px.
- Mobile: `calc(100vw - 32px)`.
- Focus vào heading hoặc control đầu tiên.
- Khi đóng, focus quay lại trigger.
- Không đóng dialog khi mutation đang submit.

### 7.11. Feedback

- Inline error cho lỗi field.
- Page alert cho lỗi tải dữ liệu.
- Dialog alert cho lỗi mutation.
- Toast cho thao tác thành công.
- `aria-live="polite"` cho thông báo không chặn.
- `role="alert"` cho lỗi chặn.

---

## 8. Frontend view models

Các type bên dưới phục vụ frontend prototype, không quyết định cấu trúc database.

```ts
type EntityId = string;

type UserRole =
  | "CUSTOMER"
  | "TECHNICIAN"
  | "FACILITY_MANAGER"
  | "ADMINISTRATOR";

type AssetType = "WIFI" | "AIR_CONDITIONER" | "PROJECTOR";

type AssetState =
  | "NORMAL"
  | "WARNING"
  | "MAINTENANCE"
  | "OFFLINE";

type IoTConnectionState = "ONLINE" | "STALE" | "OFFLINE";

type PredictionRisk = "LOW" | "MEDIUM" | "HIGH";

type WorkOrderStatus =
  | "NEW"
  | "IN_PROGRESS"
  | "COMPLETED"
  | "CLOSED";

interface CurrentUser {
  id: EntityId;
  roles: UserRole[];
}

interface Asset {
  id: EntityId;
  assetCode: string;
  name: string;
  type: AssetType;
  location: string;
  state: AssetState;
}

interface IssueRequest {
  id: EntityId;
  assetId: EntityId;
  requesterId: EntityId;
  description: string;
  status: string;
  createdAt: string;
}

interface IoTReading {
  id: EntityId;
  assetId: EntityId;
  recordedAt: string;
  connectionState: IoTConnectionState;
  values: Record<string, number | string | boolean>;
}

interface AIPrediction {
  id: EntityId;
  assetId: EntityId;
  generatedAt: string;
  risk: PredictionRisk;
  recommendation: string;
}

interface WorkOrder {
  id: EntityId;
  assetId: EntityId;
  technicianId?: EntityId;
  sourceType: "CUSTOMER" | "IOT" | "AI" | "SCHEDULE";
  status: WorkOrderStatus;
  description: string;
  createdAt: string;
  updatedAt: string;
}

interface MaintenanceSchedule {
  id: EntityId;
  assetId: EntityId;
  plannedAt: string;
  description: string;
  sourceType: "PERIODIC" | "AI_RECOMMENDATION";
  status: "UPCOMING" | "DUE" | "DONE";
}
```

Không thêm các domain ngoài MVP như ngân sách, hóa đơn, mua sắm, hợp đồng nhà cung cấp hoặc HR.

---

## 9. Screen specifications

### 9.1. Đăng nhập — `AUTH-01`

**Traceability:** FR-001 / BR-011 / NFR-001 / NFR-005

```text
┌──────────────────────────────────────────────┐
│              SMART CAMPUS                    │
│   Quản lý tài sản và bảo trì thông minh      │
│                                              │
│              [ Đăng nhập ]                   │
│                                              │
│ Sử dụng tài khoản được nhà trường cấp.       │
└──────────────────────────────────────────────┘
```

Required behavior:

- Không hiển thị chức năng ngoài phạm vi đăng nhập.
- Sau xác thực, route theo vai trò.
- Nếu tài khoản không hợp lệ, hiển thị lỗi rõ ràng.

Prototype states:

- Idle
- Loading
- Authentication failed
- Access denied

### 9.2. Báo sự cố — `ISSUE-NEW-01`

**Traceability:** US-002 / UC-002 / FR-002 / BR-003

Fields:

| Field | Control | Required |
|---|---|---:|
| Tài sản | Select/Search | Yes |
| Mô tả sự cố | TextArea | Yes |

Required behavior:

1. Customer chọn một tài sản cụ thể.
2. Customer nhập mô tả vấn đề.
3. Hệ thống gửi yêu cầu.
4. Sau thành công, hiển thị trạng thái và link sang `Yêu cầu của tôi`.

Không thêm ảnh, video, ưu tiên hoặc SLA vào MVP nếu chưa được requirement xác nhận.

### 9.3. Yêu cầu của tôi — `MY-REQUESTS-01`

**Traceability:** US-003 / UC-003 / FR-003 / BR-003

Nội dung:

- Mã yêu cầu
- Tài sản
- Vị trí
- Mô tả ngắn
- Trạng thái
- Ngày tạo

Customer chỉ được xem yêu cầu do chính mình tạo.

Prototype states:

- Loading
- Empty: `Bạn chưa có yêu cầu nào`
- Loaded
- Failed

### 9.4. Danh sách tài sản — `ASSETS-01`

**Traceability:** US-004 / UC-004 / FR-004 / BR-001 / BR-002

Header:

- H1: `Tài sản`
- Supporting: `Quản lý Wifi, điều hòa và máy chiếu trong trường.`
- Primary action: `Thêm tài sản` nếu nhóm triển khai tạo mới trong phạm vi quản lý

Content:

- Search theo Asset ID/tên
- Filter loại: Wifi / Điều hòa / Máy chiếu
- Filter trạng thái
- Asset cards hoặc table

Minimum display:

- Asset ID
- Tên
- Loại
- Vị trí
- Trạng thái

### 9.5. Chi tiết tài sản — `ASSET-DETAIL-01`

**Traceability:** FR-004 / FR-005 / FR-006 / FR-009 / BR-001 / BR-006 / BR-008 / BR-010

Các section:

1. Thông tin tài sản
2. Trạng thái hiện tại
3. IoT gần nhất
4. Dự đoán AI gần nhất
5. Work Order liên quan
6. Lịch sử bảo trì
7. Lịch bảo trì sắp tới

Nếu một tài sản chưa có IoT, hiển thị `Chưa có dữ liệu IoT` thay vì lỗi.

### 9.6. Giám sát IoT — `IOT-01`

**Traceability:** US-005 / UC-005 / FR-005 / BR-006 / BR-007 / NFR-007

```text
┌────────────────────────────────────────────────────────────────┐
│ Giám sát IoT                                                   │
│ [Tất cả] [Wifi] [Điều hòa] [Máy chiếu]   [Chỉ cảnh báo]       │
├────────────────────────────────────────────────────────────────┤
│ Thiết bị            Kết nối        Cập nhật       Trạng thái   │
│ Điều hòa B305       Online         10:35          Bình thường  │
│ AP A201             Offline        09:48          Cần kiểm tra │
└────────────────────────────────────────────────────────────────┘
```

Required interactions:

1. Dữ liệu IoT phải gắn đúng Asset ID.
2. Hiển thị thời điểm cập nhật.
3. Phân biệt `Online`, `Dữ liệu cũ`, `Mất kết nối`.
4. Cảnh báo bất thường không tự tạo Work Order.
5. Facility Manager có thể mở tài sản liên quan.
6. Technician chỉ được thấy IoT liên quan tới Work Order được giao.

Prototype actions:

- `Xem tài sản`
- `Tạo Work Order` — Facility Manager

### 9.7. Dự đoán AI — `AI-PREDICTIONS-01`

**Traceability:** US-006 / UC-006 / FR-006 / BR-008 / BR-009 / NFR-008

```text
┌──────────────────────────────────────────────────────────────────┐
│ Dự đoán bảo trì bằng AI                                         │
│ [Tất cả rủi ro] [Cao] [Trung bình] [Thấp]                       │
├──────────────────────────────────────────────────────────────────┤
│ Máy chiếu A201                             Nguy cơ cao            │
│ Có khả năng cần bảo trì sớm                                      │
│ Khuyến nghị: kiểm tra trong 3 ngày tới                           │
│ Dữ liệu: IoT + lịch sử bảo trì                                   │
│                                        [Xem tài sản] [Tạo WO]    │
└──────────────────────────────────────────────────────────────────┘
```

Required behavior:

1. AI phân tích dữ liệu IoT và lịch sử bảo trì.
2. Kết quả tối thiểu hiển thị:
   - tài sản;
   - mức rủi ro;
   - khuyến nghị.
3. Hiển thị nhãn: `AI hỗ trợ dự đoán; quyết định cuối cùng do Facility Manager`.
4. AI không tự phân công Technician.
5. AI không tự đóng Work Order.
6. Nếu dữ liệu đầu vào thiếu/cũ, phải thể hiện rõ tình trạng thay vì hiển thị dự đoán như bình thường.

### 9.8. Danh sách Work Order — `WORK-ORDERS-01`

**Traceability:** US-007, US-008, US-009 / UC-007–UC-009 / FR-007–FR-009

Facility Manager:

- Xem toàn bộ Work Order.
- Tạo Work Order.
- Phân công Technician.
- Kiểm tra Work Order hoàn thành trước khi đóng.

Technician:

- Chỉ xem Work Order được giao.
- Chuyển `NEW → IN_PROGRESS → COMPLETED`.

Columns/card fields:

- Work Order ID
- Asset
- Vị trí
- Nguồn
- Technician
- Trạng thái
- Updated time

### 9.9. Chi tiết Work Order — `WORK-ORDER-DETAIL-01`

**Traceability:** FR-007 / FR-008 / FR-009 / BR-004 / BR-005 / BR-010

Sections:

- Asset
- Nguồn phát sinh
- Nội dung sự cố
- IoT liên quan nếu có
- AI prediction liên quan nếu có
- Technician
- Trạng thái
- Kết quả xử lý

Technician actions:

- `Bắt đầu xử lý`
- `Hoàn thành`
- Cập nhật nội dung kết quả

Facility Manager actions:

- Phân công Technician
- `Đóng Work Order` sau khi kiểm tra kết quả

### 9.10. Lịch trình bảo trì — `MAINTENANCE-01`

**Traceability:** US-011 / UC-011 / FR-011 / FR-006 / BR-008 / BR-009 / BR-010

```text
┌─────────────────────────────────────────────────────────────────┐
│ Lịch bảo trì                                    [Tạo lịch mới] │
│ [Tháng này] [Sắp tới] [Đến hạn]                                │
├─────────────────────────────────────────────────────────────────┤
│ Điều hòa B305      12/09/2026      Định kỳ          Sắp tới     │
│ Máy chiếu A201     14/09/2026      Khuyến nghị AI   Đến hạn     │
└─────────────────────────────────────────────────────────────────┘
```

Fields khi tạo/cập nhật:

| Field | Required |
|---|---:|
| Tài sản | Yes |
| Thời gian dự kiến | Yes |
| Nội dung bảo trì | Yes |
| Nguồn lịch | Yes |

Nguồn lịch:

- Định kỳ
- Khuyến nghị AI

AI có thể gợi ý nhu cầu bảo trì nhưng Facility Manager là người tạo/xác nhận lịch.

### 9.11. Quản lý tài khoản và quyền — `ADMIN-USERS-01`

**Traceability:** US-010 / UC-010 / FR-010 / BR-011 / NFR-005

Minimum UI:

- Danh sách người dùng
- Vai trò hiện tại
- Thay đổi vai trò
- Lưu kết quả

Vai trò:

- Customer
- Technician
- Facility Manager
- Administrator

Không thêm phòng ban, chức danh, ảnh đại diện hoặc thông tin HR nếu chưa được Requirement xác nhận.

---

## 10. State and error matrix

| Context | State | Presentation | Recovery |
|---|---|---|---|
| Any query | Initial | Nội dung hướng dẫn | Thực hiện action chính |
| Any query | Loading | Skeleton | Chờ |
| Any query | Empty | Empty panel | Tạo dữ liệu/đổi filter |
| Any query | Failed | Page alert | Thử lại |
| Form | Invalid | Summary + inline error | Sửa input |
| Mutation | Submitting | Loading button | Chờ |
| Mutation | Success | State cập nhật + toast | Tiếp tục |
| IoT | Stale | Warning badge | Xem thời gian cập nhật |
| IoT | Offline | Offline badge + warning | Kiểm tra thiết bị/kết nối |
| AI | Missing data | Informational warning | Kiểm tra IoT/lịch sử |
| AI | High risk | Danger badge + recommendation | Facility Manager xem xét |
| Work Order | Completed | Success badge | Facility Manager review |
| Schedule | Due | Warning badge | Tạo/kiểm tra Work Order |

Thông báo lỗi trong prototype không được xem như contract chính thức của API nếu chưa được Requirement xác nhận.

---

## 11. Responsive behavior

### 11.1. Mobile

- Navigation dùng drawer.
- Asset/IoT/AI/Work Order dùng card thay cho table.
- Form xếp một cột.
- Primary button full-width nếu là action duy nhất.
- Filter xếp dọc hoặc horizontal scroll cho chip đơn giản.
- Dialog không được che bàn phím.
- Sticky action không được che content.

### 11.2. Tablet

- Asset grid 2 cột.
- Filter có thể chia 2 hàng.
- Table chỉ được dùng nếu các cột quan trọng hiển thị đủ.
- Sidebar dùng compact rail nếu phù hợp.

### 11.3. Desktop

- Sidebar cố định 240px.
- Asset grid tối đa 3 cột.
- Table cho Work Order, Schedule, User management.
- Action area căn phải.
- Nội dung giải thích giữ line length khoảng 80 ký tự.

---

## 12. Accessibility specification

- Một H1 trên mỗi màn hình.
- Heading không bỏ cấp.
- `Skip to main content` là focusable element đầu tiên.
- Visible focus ring rõ ràng.
- Form luôn có persistent label.
- Placeholder không thay thế label.
- Error nêu tên field và hướng sửa.
- Badge trạng thái có text.
- Icon decorative dùng `aria-hidden="true"`.
- Dialog dùng `aria-modal="true"` và focus trap.
- Table có `<th scope="col">`.
- Async result dùng live region phù hợp.
- Không dùng drag-and-drop là phương thức duy nhất.
- Hỗ trợ zoom 200% mà không mất action chính.
- Dữ liệu IoT phải có timestamp dễ đọc.
- AI risk không chỉ thể hiện bằng màu.

---

## 13. React implementation blueprint

```text
src/
  app/
    AppShell.tsx
    routes.tsx

  design-system/
    tokens.css
    Button.tsx
    Dialog.tsx
    FormField.tsx
    StatusBadge.tsx
    Alert.tsx
    ToastRegion.tsx

  features/
    auth/
      LoginPage.tsx

    issues/
      ReportIssuePage.tsx
      MyRequestsPage.tsx

    assets/
      AssetListPage.tsx
      AssetDetailPage.tsx
      AssetCard.tsx

    iot/
      IoTMonitoringPage.tsx
      IoTStatusCard.tsx

    ai/
      AIPredictionsPage.tsx
      PredictionCard.tsx

    work-orders/
      WorkOrdersPage.tsx
      WorkOrderDetailPage.tsx
      WorkOrderCard.tsx
      AssignTechnicianDialog.tsx

    maintenance/
      MaintenanceSchedulePage.tsx
      MaintenanceForm.tsx

    administration/
      UserRoleManagementPage.tsx

  shared/
    permissions.ts
    dateTime.ts
    domain.ts
```

Implementation rules:

- Server state tách khỏi form state.
- Frontend permission dùng chung một utility, backend vẫn phải kiểm tra quyền.
- Không optimistic success cho Work Order, role change hoặc schedule mutation trước khi server xác nhận.
- Dữ liệu IoT phải giữ timestamp.
- AI prediction luôn đi kèm asset reference.
- Preserve form state sau lỗi có thể phục hồi.
- Không dùng raw color ngoài `tokens.css`.

---

## 14. Prototype flow map

```text
Đăng nhập
  ├─ Customer
  │    ├─ Báo sự cố
  │    └─ Yêu cầu của tôi
  │
  ├─ Technician
  │    └─ Work Order của tôi
  │          ├─ Xem tài sản / IoT liên quan
  │          ├─ Bắt đầu xử lý
  │          └─ Hoàn thành
  │
  ├─ Facility Manager
  │    ├─ Tài sản
  │    │    └─ Chi tiết tài sản
  │    │          ├─ IoT
  │    │          ├─ AI prediction
  │    │          ├─ Work Order
  │    │          └─ Lịch sử / lịch bảo trì
  │    ├─ Giám sát IoT
  │    │    └─ Cảnh báo → Xem tài sản → Tạo Work Order
  │    ├─ Dự đoán AI
  │    │    └─ Prediction → Xem tài sản → Tạo Work Order / Lập lịch
  │    ├─ Work Order
  │    │    └─ Phân công Technician → Review → Close
  │    └─ Lịch bảo trì
  │         └─ Tạo / cập nhật lịch
  │
  └─ Administrator
       └─ Tài khoản & phân quyền
```

Luồng bảo trì thông minh cốt lõi:

```text
IoT thu thập dữ liệu
      ↓
Hệ thống lưu dữ liệu theo Asset ID
      ↓
AI phân tích dữ liệu IoT + lịch sử bảo trì
      ↓
Dự đoán nguy cơ / nhu cầu bảo trì
      ↓
Facility Manager xem xét
      ↓
Tạo Work Order hoặc lập lịch bảo trì
      ↓
Technician xử lý
      ↓
Lưu kết quả vào lịch sử bảo trì
```

---

## 15. Open questions cần Project Owner xác nhận

Các nội dung dưới đây không được tự xem là requirement chỉ vì đã xuất hiện trong prototype.

### Authentication

- Có dùng SSO của nhà trường hay tài khoản nội bộ?
- Session hết hạn sau bao lâu?
- Có cho phép đăng nhập đồng thời nhiều thiết bị không?

### Báo sự cố

- Có cần ảnh đính kèm trong phiên bản sau không?
- Trạng thái chính xác của yêu cầu Customer gồm những gì?
- Customer có được chỉnh sửa yêu cầu sau khi gửi không?

### Quản lý tài sản

- Facility Manager có được tạo/xóa tài sản hay chỉ cập nhật?
- Quy tắc định dạng Asset ID là gì?
- Có cần filter theo tòa nhà/tầng/phòng riêng hay chỉ tìm kiếm?

### IoT

- Dữ liệu nào được thu cho từng loại tài sản?
- Ngưỡng nào tạo cảnh báo?
- Bao lâu không có dữ liệu thì chuyển sang `STALE` hoặc `OFFLINE`?
- Có cần lưu raw IoT data dài hạn hay chỉ dữ liệu tổng hợp?

### AI

- Cách tính mức rủi ro LOW/MEDIUM/HIGH?
- Có cần hiển thị confidence score không?
- Bao lâu chạy lại mô hình dự đoán?
- Khi IoT thiếu dữ liệu, AI có được dự đoán chỉ từ lịch sử bảo trì không?
- Ai chịu trách nhiệm xác nhận prediction sai?

### Work Order

- Có cho phép đổi Technician sau khi đã phân công không?
- Có cần lý do khi đóng Work Order không?
- Có cần Facility Manager xác nhận mọi Work Order đã hoàn thành không?

### Lịch bảo trì

- Có cho phép lịch lặp định kỳ không?
- Nếu lịch bảo trì trùng nhau thì xử lý như thế nào?
- Khi đến hạn có tự tạo Work Order hay chỉ cảnh báo?
- Có cho phép đổi lịch sau khi Work Order đã được tạo không?

### Cross-cutting

- Nội dung thông báo lỗi chuẩn là gì?
- API response code chuẩn cho từng mutation là gì?
- Chính sách lưu dữ liệu IoT và AI là bao lâu?
- Có cần audit log riêng cho AI prediction và schedule change không?

---

## 16. Figma frame và component naming

Nếu dựng trong Figma, dùng quy ước:

```text
00 Foundations / Colors
00 Foundations / Typography
00 Foundations / Spacing

01 Components / Button / Primary
01 Components / Button / Secondary
01 Components / Form / TextField
01 Components / Form / Select
01 Components / Feedback / Alert
01 Components / Feedback / StatusBadge
01 Components / Overlay / Dialog
01 Components / Data / AssetCard
01 Components / Data / IoTStatusCard
01 Components / Data / PredictionCard
01 Components / Data / WorkOrderCard

02 Patterns / AppShell / Desktop
02 Patterns / AppShell / Mobile
02 Patterns / Table / Desktop
02 Patterns / CardList / Mobile

03 Screens / AUTH-01 / Desktop
03 Screens / AUTH-01 / Mobile

03 Screens / ISSUE-NEW-01 / Desktop
03 Screens / MY-REQUESTS-01 / Desktop

03 Screens / ASSETS-01 / Desktop
03 Screens / ASSETS-01 / Mobile
03 Screens / ASSET-DETAIL-01 / Desktop

03 Screens / IOT-01 / Desktop
03 Screens / IOT-01 / Mobile

03 Screens / AI-PREDICTIONS-01 / Desktop
03 Screens / AI-PREDICTIONS-01 / Mobile

03 Screens / WORK-ORDERS-01 / Desktop
03 Screens / WORK-ORDER-DETAIL-01 / Desktop

03 Screens / MAINTENANCE-01 / Desktop
03 Screens / MAINTENANCE-01 / Mobile

03 Screens / ADMIN-USERS-01 / Desktop
```

Component variants:

```text
Button:
  variant=primary|secondary|tertiary|danger
  size=sm|md|lg
  state=default|hover|focus|disabled|loading

StatusBadge:
  domain=asset|iot|ai|work-order|schedule
  state=<domain state>

AssetCard:
  state=normal|warning|maintenance|offline

IoTStatusCard:
  connection=online|stale|offline
  alert=true|false

PredictionCard:
  risk=low|medium|high

WorkOrderCard:
  status=new|in-progress|completed|closed
```

---

## 17. Handoff checklist

Trước khi một màn hình chuyển từ prototype sang implementation-ready:

- [ ] Screen có tham chiếu FR và BR liên quan.
- [ ] Nếu đã có UC/US/AC thì screen phải tham chiếu đúng mã.
- [ ] Hành vi chưa xác nhận được ghi rõ là open question.
- [ ] Có frame Desktop.
- [ ] Có frame Mobile với các màn hình người dùng thường xuyên truy cập.
- [ ] Có loading state nếu màn hình tải dữ liệu.
- [ ] Có empty state nếu danh sách có thể rỗng.
- [ ] Có error state.
- [ ] Có validation state cho form.
- [ ] Có success state cho mutation.
- [ ] Có permission state nếu vai trò khác nhau.
- [ ] Keyboard order được kiểm tra.
- [ ] Dialog có focus behavior.
- [ ] Badge không truyền đạt trạng thái chỉ bằng màu.
- [ ] Độ tương phản được kiểm tra.
- [ ] Zoom 200% không làm mất action.
- [ ] Component name khớp với design-system API.
- [ ] IoT screen thể hiện timestamp và trạng thái dữ liệu.
- [ ] AI screen thể hiện rõ AI chỉ hỗ trợ quyết định.
- [ ] Work Order screen tuân thủ quyền Facility Manager/Technician.
- [ ] Maintenance schedule không tự động biến AI recommendation thành quyết định.
- [ ] Engineering, QA và Project Owner review màn hình với Vault trước khi triển khai.
