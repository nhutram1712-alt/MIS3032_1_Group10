# DESIGN SYSTEM SPECIFICATION

## 1. Color Tokens

### Brand / Primary
| Token | Giá trị ước lượng | Dùng cho |
| :--- | :--- | :--- |
| `color/brand/primary` | `#2F5C41` (xanh rêu đậm) | Sidebar background, primary button, active nav item |
| `color/brand/primary-hover` | `#254A34` | Hover state của primary button/nav |
| `color/brand/on-primary` | `#FFFFFF` | Text/icon trên nền primary |

### Surface / Background
| Token | Giá trị ước lượng | Dùng cho |
| :--- | :--- | :--- |
| `color/surface/page` | `#F5F1E7` (kem nhạt) | Nền toàn trang (ngoài sidebar) |
| `color/surface/card` | `#FFFFFF` | Card, table, form panel |
| `color/surface/sidebar-active` | `rgba(255, 255, 255, 0.08 - 0.10)` | Item menu đang chọn trong sidebar |

### Text
| Token | Giá trị ước lượng | Dùng cho |
| :--- | :--- | :--- |
| `color/text/heading` | `#1C2620` (gần đen, ánh xanh) | Heading serif (H1/H2) |
| `color/text/body` | `#33403A` | Nội dung, label |
| `color/text/muted` | `#7A8580` | Placeholder, sub-label, caption |
| `color/text/link` | `#2F5C41` (đậm hơn khi hover) | Link dạng tên tài sản (underline) |

### Status / Badge (dùng nhất quán cho Trạng thái + Mức độ rủi ro)
| Token | Nền | Chữ | Dùng cho |
| :--- | :--- | :--- | :--- |
| `color/status/info` | `#E4EEF8` | `#2C5E8C` | Badge "Bảo trì" |
| `color/status/success` | `#E4F1E8` | `#2F7A4A` | Badge "Hoạt động", "Thấp" (rủi ro) |
| `color/status/warning` | `#FBEFDD` | `#B4791A` | Badge "Cảnh báo", "Trung bình" |
| `color/status/danger` | `#FAE3E1` | `#C33F33` | Badge "Cao", "High" |
| `color/status/neutral` | `#EFEFEA` | `#5A5A52` | Badge "Đã hủy", trạng thái kết thúc |

---

## 2. Typography

| Token | Font family (ước lượng) | Dùng cho |
| :--- | :--- | :--- |
| `font/display` | Serif kiểu Georgia/Playfair Display, bold | H1 trang (VD: "Smart Campus Facility Management", "Work Order") |
| `font/body` | Sans-serif hệ thống (Inter/Helvetica-like) | Body text, label, table, button |
| `font/eyebrow` | Sans-serif, uppercase, letter-spacing rộng, cỡ nhỏ | Nhãn breadcrumb phía trên H1 (VD: "TỔNG QUAN", "CÔNG VIỆC") |

### Scale
| Scale Token | Size / Weight | Dùng cho |
| :--- | :--- | :--- |
| `type/display-lg` | ~32px / 700 / serif | H1 trang |
| `type/heading-md` | ~20px / 700 / serif | Tên card lớn, tên mục nhóm (VD: "Điều hòa" trong danh mục) |
| `type/body-md` | ~15px / 400 / sans | Nội dung mặc định |
| `type/body-sm` | ~13px / 400 / sans | Sub-label dưới tên (VD: "Hội trường" dưới "Hội trường A") |
| `type/label-caps` | ~12px / 600 / sans, uppercase, tracking +0.05em | Eyebrow, header cột bảng |
| `type/stat-number` | ~28px / 700 / serif | Số liệu lớn trong stat card (78, 25, 9…) |

---

## 3. Spacing scale

- **Scale:** `4px` / `8px` / `12px` / `16px` / `24px` / `32px` / `48px` (base unit: 4px).
- **Padding card mặc định:** `24px`
- **Gap giữa các block dọc trong 1 trang:** `24px` – `32px`
- **Gap giữa sidebar item:** `4px`

---

## 4. Radius

| Token | Giá trị | Dùng cho |
| :--- | :--- | :--- |
| `radius/sm` | `6px` | Badge, input nhỏ |
| `radius/md` | `10px` | Button, input, dropdown |
| `radius/lg` | `16px` | Card, stat card, modal |
| `radius/pill` | `999px` | Filter chip (VD: "Tất cả 78", "Điều hòa 13") |

---

## 5. Shadow

| Token | Giá trị | Dùng cho |
| :--- | :--- | :--- |
| `shadow/card` | `0 1px 2px rgba(0,0,0,0.04), 0 1px 8px rgba(0,0,0,0.03)` | Card, table container trên nền kem |
| `shadow/modal` | `0 8px 24px rgba(0,0,0,0.16)` | Modal/Confirm (mới, chưa có trên prototype hiện tại) |
| `shadow/toast` | `0 4px 12px rgba(0,0,0,0.12)` | Toast xác nhận |

> **Nguyên tắc:** Không hardcode giá trị màu/spacing trực tiếp trong component — luôn tham chiếu token ở trên. Nếu 1 màn cần biến thể chưa có trong bảng, thêm token mới vào bảng này trước, không tự chế giá trị rời rạc trong file component.

---

## 6. Component set tối thiểu (Page "Components" trong Figma)

| Component | Variants bắt buộc | Ghi chú |
| :--- | :--- | :--- |
| **Button** | primary / secondary (viền) / danger (nút Hủy, Từ chối) — mỗi loại: default, hover, focus, disabled, loading | Danger dùng `color/status/danger` làm border/text, nền trắng (giống nút "Hủy"/"Từ chối" hiện tại) |
| **Input (text/textarea)** | default, focus, filled, disabled, error (viền đỏ + helper text) | Textarea cần thêm biến thể có counter (VD: "0/500") |
| **Dropdown/Select** | default, open, disabled, error | Dùng cho filter Khu vực/Tòa/Vị trí, chọn Technician, chọn Yêu cầu |
| **Voice control** | idle, listening, disabled | [ASSUMPTION — xem screen-inventory.md mục 3], chưa gắn vào flow MVP |
| **Card** | stat card, list-row card, form panel | Border 1px mờ hoặc shadow nhẹ, không cả hai cùng lúc |
| **Badge** | 5 màu status ở mục 1, size sm/md | Dùng chung cho Trạng thái WO/Request và Mức độ rủi ro |
| **Modal/Confirm** | default, với form bên trong (VD: xác nhận đóng yêu cầu) | Component mới — chưa có trên prototype, bắt buộc bổ sung theo DEC-09 |
| **Toast** | success, error | Vị trí góc trên phải hoặc dưới, auto-dismiss ~3s |
| **Empty state** | icon + heading + mô tả ngắn + (tuỳ chọn) CTA | Dùng chung 1 layout cho mọi bảng rỗng |
| **Error state (inline)** | icon cảnh báo + text đỏ dưới field | Không dùng modal cho lỗi validate nhẹ |

---

## 7. Responsive rules

*Prototype hiện tại chỉ có desktop (~1900px, sidebar cố định trái). Quy tắc tối thiểu cho breakpoint chính:*

| Breakpoint | Layout |
| :--- | :--- |
| **Desktop $\ge$ 1024px** | Sidebar cố định trái (256px), content chiếm phần còn lại, bảng hiển thị đầy đủ cột |
| **Tablet 768–1023px** | Sidebar thu gọn còn icon (collapse), content full-width, bảng có thể scroll ngang |
| **Mobile < 768px** | Sidebar chuyển thành bottom nav hoặc menu ẩn (hamburger), stat card xếp dọc 1 cột thay vì hàng ngang, bảng chuyển sang dạng list-card (mỗi row = 1 card dọc) |

---

## 8. Accessibility checklist

- [ ] **Contrast text/nền:** Đạt tối thiểu WCAG AA (4.5:1 cho text thường, 3:1 cho text lớn $\ge$ 18px bold) — đặc biệt kiểm tra badge màu warning (`#B4791A` trên `#FBEFDD`) và link xanh trên nền kem.
- [ ] **Input Labeling:** Mọi input có label gắn liền (không chỉ placeholder) — hiện tại placeholder đang đóng luôn vai trò label ở một số form, cần bổ sung label riêng.
- [ ] **Keyboard Navigation:** Focus state rõ ràng bằng keyboard (Tab) cho toàn bộ button/input/dropdown/link — chưa thể hiện trên prototype hiện tại, bắt buộc thiết kế ở Figma.
- [ ] **Touch Target Size:** Touch target tối thiểu `44×44px` trên mobile cho button/action trong bảng (nút "Bắt đầu", "Từ chối", "Hoàn thành" hiện đang nhỏ, cần tăng padding ở breakpoint mobile).
- [ ] **Error Messaging:** Error message luôn đi kèm hành động sửa lỗi cụ thể (xem ux-copy-table.md), không chỉ báo "có lỗi".
