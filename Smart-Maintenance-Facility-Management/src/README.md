# Source Code

Mã nguồn hệ thống Smart Maintenance & Facility Management.

## Cấu trúc dự kiến

```
src/
├── backend/     # API, business logic
├── frontend/    # Giao diện người dùng
├── iot/         # Thu thập dữ liệu cảm biến, gateway
└── ai/          # Mô hình dự đoán, xử lý dữ liệu
```

Yêu cầu kỹ thuật xem tại `../docs/05-technical/system-architecture.md`.

Trước khi chạy, copy `../.env.example` thành `.env` và điền cấu hình thật.
