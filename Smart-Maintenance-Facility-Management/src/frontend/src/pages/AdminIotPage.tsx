import { FormEvent, useEffect, useMemo, useState } from "react";
import { endpoints, type Asset, type IotMapping } from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

const typeVi: Record<string, string> = {
  "Air Conditioner": "Điều hòa",
  Projector: "Máy chiếu",
  Light: "Đèn",
  Fan: "Quạt",
  "Wi-Fi": "Wi-Fi"
};

export function AdminIotPage() {
  const { session } = useAuth();
  const [assets, setAssets] = useState<Asset[]>([]);
  const [mappings, setMappings] = useState<IotMapping[]>([]);
  const [typeFilter, setTypeFilter] = useState("all");
  const [mapAssetId, setMapAssetId] = useState(0);
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);

  async function refresh() {
    if (!session) return;
    const m = await endpoints.iotMappings(session.token);
    setMappings(m);
    try {
      const as = await endpoints.assets(session.token);
      setAssets(as);
    } catch {
      setAssets([]);
    }
  }

  useEffect(() => {
    if (session?.role !== "Admin") return;
    refresh().catch((e) => setError(e.message));
  }, [session]);

  const mappedAssetIds = useMemo(() => new Set(mappings.map((m) => m.assetId)), [mappings]);

  const unmappedAssets = useMemo(
    () => assets.filter((a) => !mappedAssetIds.has(a.assetId)),
    [assets, mappedAssetIds]
  );

  const assetOptions = useMemo(() => {
    let list = unmappedAssets;
    if (typeFilter !== "all") list = list.filter((a) => a.type === typeFilter);
    return [...list].sort((a, b) => a.name.localeCompare(b.name, "vi"));
  }, [unmappedAssets, typeFilter]);

  const selectedAsset = unmappedAssets.find((a) => a.assetId === mapAssetId) ?? assets.find((a) => a.assetId === mapAssetId);

  useEffect(() => {
    if (assetOptions.length === 0) {
      if (mapAssetId !== 0) setMapAssetId(0);
      return;
    }
    if (!assetOptions.some((a) => a.assetId === mapAssetId)) {
      setMapAssetId(assetOptions[0].assetId);
    }
  }, [assetOptions, mapAssetId]);

  const assetLabel = useMemo(() => {
    const map = new Map(assets.map((a) => [a.assetId, a]));
    return (id: number) => {
      const a = map.get(id);
      return a ? { name: a.name, location: a.location, type: a.type } : { name: `Tài sản #${id}`, location: "", type: "" };
    };
  }, [assets]);

  async function onCreateMapping(e: FormEvent) {
    e.preventDefault();
    if (!session) return;
    setBusy(true);
    setError("");
    try {
      await endpoints.createIotMapping(session.token, { assetId: mapAssetId });
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không tạo được mapping");
    } finally {
      setBusy(false);
    }
  }

  if (session?.role !== "Admin") {
    return (
      <section>
        <PageHeader kicker="HỆ THỐNG" title="IoT Mapping" hint="Chỉ Admin mới truy cập được khu vực này." />
      </section>
    );
  }

  return (
    <section className="admin-page">
      <PageHeader
        kicker="HỆ THỐNG"
        title="IoT Mapping"
        hint="Liên kết cảm biến IoT với tài sản campus để theo dõi dữ liệu vận hành."
      />
      {error && <p className="error">{error}</p>}

      <div className="wo-summary admin-summary">
        <div className="wo-summary-item">
          <span>Đã map</span>
          <strong>{mappings.length}</strong>
        </div>
        <div className="wo-summary-item">
          <span>Tài sản</span>
          <strong>{assets.length}</strong>
        </div>
        <div className="wo-summary-item">
          <span>Chưa map</span>
          <strong>{unmappedAssets.length}</strong>
        </div>
      </div>

      <form className="card form-grid admin-iot-form" onSubmit={onCreateMapping}>
        <h3>Tạo liên kết mới</h3>
        {unmappedAssets.length === 0 ? (
          <p className="muted admin-note">Tất cả tài sản đã có mapping. Không còn thiết bị để liên kết mới.</p>
        ) : (
          <>
            <div className="form-row">
              <label>
                Lọc loại tài sản
                <select
                  value={typeFilter}
                  onChange={(e) => {
                    setTypeFilter(e.target.value);
                  }}
                >
                  <option value="all">Tất cả loại</option>
                  {Object.entries(typeVi).map(([value, label]) => (
                    <option key={value} value={value}>
                      {label}
                    </option>
                  ))}
                </select>
              </label>
              <label>
                Tài sản chưa map
                {assetOptions.length > 0 ? (
                  <select value={mapAssetId} onChange={(e) => setMapAssetId(Number(e.target.value))}>
                    {assetOptions.map((a) => (
                      <option key={a.assetId} value={a.assetId}>
                        {a.name} · {a.location}
                      </option>
                    ))}
                  </select>
                ) : (
                  <select disabled value={0}>
                    <option value={0}>Không còn tài sản loại này chưa map</option>
                  </select>
                )}
              </label>
            </div>
            {selectedAsset && assetOptions.some((a) => a.assetId === selectedAsset.assetId) && (
              <p className="req-preview req-preview-compact">
                {typeVi[selectedAsset.type] ?? selectedAsset.type} · {selectedAsset.name} · {selectedAsset.location}
              </p>
            )}
            <button className="btn primary" disabled={busy || !mapAssetId || assetOptions.length === 0} type="submit">
              {busy ? "Đang tạo..." : "Tạo mapping"}
            </button>
          </>
        )}
      </form>

      <h3 className="wo-section-title">Danh sách mapping</h3>
      <div className="table-wrap admin-table-wrap">
        <table className="admin-iot-table">
          <thead>
            <tr>
              <th>Tài sản</th>
              <th>Device ID</th>
              <th>Thời gian</th>
            </tr>
          </thead>
          <tbody>
            {mappings.map((m) => {
              const info = assetLabel(m.assetId);
              return (
                <tr key={m.mappingId}>
                  <td>
                    <div className="admin-map-asset">
                      <strong>{m.assetName || info.name}</strong>
                      {(info.location || info.type) && (
                        <small className="muted">
                          {info.type ? `${typeVi[info.type] ?? info.type} · ` : ""}
                          {info.location}
                        </small>
                      )}
                    </div>
                  </td>
                  <td>
                    <code className="admin-device-id">{m.deviceId}</code>
                  </td>
                  <td>
                    <time className="wo-time">
                      {m.createdAt ? new Date(m.createdAt).toLocaleString("vi-VN") : "—"}
                    </time>
                  </td>
                </tr>
              );
            })}
            {mappings.length === 0 && (
              <tr>
                <td className="empty" colSpan={3}>
                  Chưa có IoT mapping.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
