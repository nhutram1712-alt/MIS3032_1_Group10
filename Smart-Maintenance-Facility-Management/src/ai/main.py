"""Smart Maintenance AI Prediction Service (US-06-01).

Rule-based MVP scorer. Does not create Work Orders or change Asset Status.
Horizon: 7 days. Risk: Low | Medium | High.
"""

from fastapi import FastAPI, Header, HTTPException
from pydantic import BaseModel, Field


app = FastAPI(title="Smart Maintenance AI Prediction", version="0.1.0")

API_KEY = "DEV_ONLY_AI_SERVICE_KEY"


class IotReading(BaseModel):
    metricType: str
    value: float
    timestamp: str | None = None


class PredictRequest(BaseModel):
    assetId: int
    horizonDays: int = 7
    iotReadings: list[IotReading] = Field(default_factory=list)
    historyCount: int = 0


class PredictResponse(BaseModel):
    risk: str
    horizonDays: int = 7


@app.get("/health")
def health() -> dict[str, str]:
    return {"status": "ok"}


@app.post("/predict", response_model=PredictResponse)
def predict(body: PredictRequest, x_api_key: str | None = Header(default=None)) -> PredictResponse:
    if x_api_key != API_KEY:
        raise HTTPException(status_code=401, detail="Invalid or missing AI service API key.")
    if body.horizonDays != 7:
        raise HTTPException(status_code=400, detail="Prediction horizon must be 7 days.")
    if not body.iotReadings:
        raise HTTPException(status_code=422, detail="Insufficient IoT data for prediction.")

    temperatures = [r.value for r in body.iotReadings if r.metricType == "temperature"]
    power = [r.value for r in body.iotReadings if r.metricType == "power_status"]
    max_temp = max(temperatures) if temperatures else 0.0
    min_power = min(power) if power else 1.0

    if min_power <= 0 or max_temp >= 35:
        risk = "High"
    elif max_temp >= 30 or body.historyCount >= 3:
        risk = "Medium"
    else:
        risk = "Low"

    return PredictResponse(risk=risk, horizonDays=7)
