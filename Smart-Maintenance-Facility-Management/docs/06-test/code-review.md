# Code review

**Dự án:** Smart Maintenance & Facility Management  
**Phạm vi:** Auth/Users, tài sản, YC/WO, Admin Users + IoT, mapping tự sinh Device ID, AI  
**Ngày review:** 10/09/2026, đối chiếu test 18/09/2026  
**Người review:** QA (có hỏi lại nhóm dev khi cần)

---

## Nhận xét

Phần API làm chắc hơn UI. Quyền trên server (JWT, role, QT đổi user, IoT API key) có test và đang pass. Chỗ yếu nằm ở router frontend: menu ẩn rồi nhưng gõ địa chỉ vẫn vào trang.

Mapping IoT chỉ gửi `assetId` là đúng hướng (không cho admin bịa Device ID). Thiếu thao tác gỡ / sửa trên màn hình nên Admin kẹt nếu map nhầm.

Work Order sau redesign dùng được: có hàng chờ, tên kỹ thuật viên, nút theo trạng thái. Tên trang vẫn tiếng Anh.

AI không tự tạo phiếu — đúng yêu cầu. Nhãn rủi ro chưa dịch.

---

## Bảng finding

| Mức | Việc | Hướng xử lý | TT |
|---|---|---|---|
| High | PUT users leo thang role | Khóa QT-1..4, FE chỉ xoay Tech ↔ FM | Đã xong |
| High | Overview Admin = 0 | Load lại card Admin | Đã xong |
| Medium | WO khó dùng, chỉ hiện ID | Redesign + form phân công | Đã xong |
| Medium | Seed mỏng | Nới seed, để 7 tài sản chưa map | Đã xong |
| Medium | `/admin/*` không guard | Bọc `RequireRoles` trong `App.tsx` | BUG-011 |
| Medium | Mapping auto Device ID | Thêm test hồi quy | Đã xong (BUG-015) |
| Medium | IoT không Unmap / không sửa Device ID | Thêm UI (+ DELETE nếu thiếu) | BUG-009, BUG-010 |
| Medium | `/alerts`, `/predictions` mở cho mọi user đã login | Guard FM/Tech | BUG-013 |
| Medium | Chưa E2E | Playwright login + phân công | Đã xong (BUG-006) |
| Low | Alerts / AI còn chữ Anh | Map label Việt | BUG-007, BUG-008 |
| Low | Menu Work Order tiếng Anh | Đổi copy | BUG-014 |
| Low | Nút status lệch | Sửa CSS | Đã xong |

---

## Từng module

| Module | Nhìn vào | Kết luận |
|---|---|---|
| Users / Admin Users | QT, không hiện ma trận quyền trên UI | Đạt. API permissions vẫn giữ |
| Admin IoT | Chỉ máy chưa map, tự sinh SENSOR_* | Đạt phần tạo. Thiếu gỡ/sửa |
| Tài sản | Requester không vào catalog | Đạt |
| YC / WO | Đồng bộ trạng thái, không WO trùng | Luồng chính đạt. Copy UI còn sót |
| IoT ingest | API key, không persist máy chưa map | Ingest đạt. Label alert fail |
| AI | Không side-effect WO, tên máy không `#id` | Đạt phần nghiệp vụ. Label risk fail |

---

## Chạy lại sau review

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

18/09/2026: 68 pass / 0 fail. Playwright: 3 pass. Manual: 42 pass / 8 fail — không kết luận 100%.

**Chốt:** pass có điều kiện. Không còn High/Critical. Còn 8 việc UX/guard.
