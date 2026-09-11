import { FormEvent, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { endpoints, type Asset, type IotReading, type Prediction } from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

const statuses = ["Operational", "Warning", "Maintenance", "Out of Service"];
const types = ["Wi-Fi", "Air Conditioner", "Projector", "Light", "Fan"];

function statusClass(status: string) {
  return `pill ${status.replace(/\s/g, "").toLowerCase()}`;
}

export function AssetDetailPage() {
  const { id } = useParams();
  const assetId = Number(id);
  const { session } = useAuth();
  const [asset, setAsset] = useState<Asset | null>(null);
  const [iot, setIot] = useState<IotReading[]>([]);
  const [prediction, setPrediction] = useState<Prediction | null>(null);
  const [predError, setPredError] = useState("");
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  const [name, setName] = useState("");
  const [type, setType] = useState("Air Conditioner");
  const [location, setLocation] = useState("");
  const [status, setStatus] = useState("Operational");
  const isFm = session?.role === "FacilityManager";

  async function load() {
    if (!session || !assetId) return;
    setError("");
    setPredError("");
    const a = await endpoints.asset(session.token, assetId);
    setAsset(a);
    setName(a.name);
    setType(a.type);
    setLocation(a.location);
    setStatus(a.status);
    if (isFm || session.role === "Technician") {
      try {
        const readings = await endpoints.iotData(session.token, assetId);
        setIot(readings);
      } catch {
        setIot([]);
      }
    }
    try {
      setPrediction(await endpoints.prediction(session.token, assetId));
    } catch (e) {
      setPrediction(null);
      setPredError(e instanceof Error ? e.message : "Chưa đủ dữ liệu để dự đoán.");
    }
  }

  useEffect(() => {
    load().catch((e) => setError(e.message));
  }, [session, assetId]);

  async function onUpdate(e: FormEvent) {
    e.preventDefault();
    if (!session) return;
    setBusy(true);
    setError("");
    try {
      await endpoints.updateAsset(session.token, assetId, { name, type, location });
      await load();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không cập nhật được");
    } finally {
      setBusy(false);
    }
  }

  async function onStatus(e: FormEvent) {
    e.preventDefault();
    if (!session) return;
    setBusy(true);
    setError("");
    try {
      await endpoints.updateAssetStatus(session.token, assetId, status);
      await load();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không đổi được trạng thái");
    } finally {
      setBusy(false);
    }
  }

  const title = asset?.name ?? (assetId ? `Tài sản #${assetId}` : "Tài sản");
  const hint = asset
    ? `${asset.type} · ${asset.location} — dữ liệu IoT và dự đoán bảo trì 7 ngày tới.`
    : "Dữ liệu IoT và dự đoán nhu cầu bảo trì trong 7 ngày tới.";

  return (
    <section>
      <Link className="back" to="/assets">
        ← Quay lại danh mục
      </Link>
      <PageHeader kicker="GIÁM SÁT" title={title} hint={hint} />
      {asset && (
        <p className="asset-meta">
          <span className={statusClass(asset.status)}>{asset.status}</span>
          {asset.maintenanceRisk && (
            <span className={`risk ${asset.maintenanceRisk.toLowerCase()}`}>Rủi ro {asset.maintenanceRisk}</span>
          )}
        </p>
      )}
      {error && <p className="error">{error}</p>}

      {isFm && (
        <div className="detail-grid" style={{ marginBottom: "1rem" }}>
          <form className="card form-grid" onSubmit={onUpdate}>
            <h3>Cập nhật thông tin</h3>
            <label>
              Tên
              <input required value={name} onChange={(e) => setName(e.target.value)} />
            </label>
            <label>
              Loại
              <select value={type} onChange={(e) => setType(e.target.value)}>
                {types.map((t) => (
                  <option key={t}>{t}</option>
                ))}
              </select>
            </label>
            <label>
              Vị trí
              <input required value={location} onChange={(e) => setLocation(e.target.value)} />
            </label>
            <button className="btn primary btn-wide" disabled={busy} type="submit">
              Lưu thông tin
            </button>
          </form>
          <form className="card form-grid" onSubmit={onStatus}>
            <h3>Đổi trạng thái</h3>
            <label>
              Status
              <select value={status} onChange={(e) => setStatus(e.target.value)}>
                {statuses.map((s) => (
                  <option key={s}>{s}</option>
                ))}
              </select>
            </label>
            <button className="btn primary btn-wide btn-status" disabled={busy} type="submit">
              Cập nhật status
            </button>
          </form>
        </div>
      )}

      <div className="detail-grid">
        {(isFm || session?.role === "Technician") && (
          <div className="card">
            <h3>Dữ liệu IoT</h3>
            {iot.length === 0 ? (
              <p className="muted">Asset chưa có dữ liệu IoT được liên kết.</p>
            ) : (
              <table>
                <thead>
                  <tr>
                    <th>CHỈ SỐ</th>
                    <th>GIÁ TRỊ</th>
                    <th>THỜI ĐIỂM</th>
                  </tr>
                </thead>
                <tbody>
                  {iot.map((r, i) => (
                    <tr key={i}>
                      <td>{r.metricType}</td>
                      <td>{r.readingValue ?? r.value}</td>
                      <td>{new Date(r.timestamp).toLocaleString("vi-VN")}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        )}

        <div className="card">
          <h3>Dự đoán AI (7 ngày)</h3>
          {prediction ? (
            <p>
              Mức rủi ro: <strong className={`risk ${prediction.risk.toLowerCase()}`}>{prediction.risk}</strong>
              <br />
              Thời điểm: {new Date(prediction.predictedAt).toLocaleString("vi-VN")}
              <br />
              {prediction.stale && <span className="muted">Cảnh báo: prediction đã cũ (&gt;24h).</span>}
              {prediction.basedOnSampleData && (
                <>
                  <br />
                  <span className="muted">Dựa trên dữ liệu mẫu MVP.</span>
                </>
              )}
              <br />
              <small className="muted">AI chỉ hỗ trợ quyết định — không tự tạo Work Order.</small>
            </p>
          ) : (
            <p className="muted">{predError || "Chưa đủ dữ liệu để dự đoán."}</p>
          )}
        </div>
      </div>
    </section>
  );
}
