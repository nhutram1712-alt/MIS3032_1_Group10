# Biên bản chạy lại test

**Dự án:** Smart Maintenance & Facility Management  
**Ngày chạy:** 18/09/2026  

Sau khi sửa code thì chạy lại từ đầu, không lấy số liệu vòng trước.

---

## 1. Backend

```bash
dotnet test src/backend/SmartMaintenance.Tests/SmartMaintenance.Tests.csproj
```

```
Passed!  Failed: 0, Passed: 68, Skipped: 0, Total: 68
Duration: ~13 s
```

So với 65 test cũ, vòng này thêm:

- `CreateMapping_OnlyAssetId_AssignsSensorPrefix`
- `CreateMapping_OnlyAssetId_Returns201_AndAutoSensorId`
- `CreateMapping_Requester_Returns403`

---

## 2. Playwright

```bash
cd src/frontend
npm run test:e2e
```

```
3 passed
```

Gồm: login sai mật khẩu, login manager vào Tổng quan, Facility Manager phân công WO.

---

## 3. Test tay

| | |
|---|---|
| Số case chạy (TC-01..60) | 50 |
| Pass | 42 |
| Fail | 8 |
| Bảng chi tiết | `testcase.md` |
| Bug còn mở | `bug-log.md` (BUG-007 → BUG-014) |

---

## 4. Kiểm tra nhanh API (đổi role + mapping)

| Check | Kết quả |
|---|---|
| Không sửa được user Admin | 403 |
| Không đổi Requester sang role khác | 400 |
| Không gán Admin qua PUT | 400 |
| Technician → Facility Manager | 200 |
| POST `/api/iot-mappings` body `{ assetId }` | 201, Device ID `SENSOR_{id}` |

---

## 5. Việc chưa làm / chưa đo

- Smoke trên URL ngrok: 18/09 tunnel tắt (`ERR_NGROK_3200`)
- Playwright chưa đi bước Technician hoàn thành WO
- Chưa `npm audit` chính thức
- NFR-05 (interval IoT), NFR-06 (tải) chưa đủ
- Chưa SQL Server / TLS production

09/10/2026 mở lại UI local: các lỗi chữ Anh và deep-link (BUG-007, 008, 009, 011, 013, 014) vẫn tái hiện.

**Chốt ngày 18/09:** BE 68 OK, Playwright 3 OK, manual pass có điều kiện (8 fail). Demo local được. Public URL chưa smoke.
