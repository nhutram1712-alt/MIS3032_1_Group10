# AI Prediction Service (US-06-01)

Python FastAPI microservice. Backend C# gọi `POST /predict` qua HTTP nội bộ + API Key.

AI **chỉ hỗ trợ quyết định** — không tạo Work Order, không đổi Asset Status.

## Run

```bash
cd src/ai
pip install -r requirements.txt
uvicorn main:app --host 0.0.0.0 --port 8000
```

Default API key: `DEV_ONLY_AI_SERVICE_KEY` (header `X-Api-Key`).

## Contract

`POST /predict`

```json
{
  "assetId": 1,
  "horizonDays": 7,
  "iotReadings": [{ "metricType": "temperature", "value": 28.5, "timestamp": "..." }],
  "historyCount": 0
}
```

Response: `{ "risk": "Low|Medium|High", "horizonDays": 7 }`
