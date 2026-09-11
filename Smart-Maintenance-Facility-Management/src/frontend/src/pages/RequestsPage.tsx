import { FormEvent, useEffect, useMemo, useState } from "react";
import {
  endpoints,
  type Asset,
  type MaintenanceRequest
} from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

/** Simplified UI statuses mapped from BR-14 values. */
function displayStatus(status: string): { label: string; cls: string } {
  if (status === "Submitted") return { label: "Chờ phân công", cls: "submitted" };
  if (status === "Pending" || status === "In Progress") return { label: "Đang xử lý", cls: "inprogress" };
  if (status === "Resolved" || status === "Closed") return { label: "Hoàn thành", cls: "resolved" };
  if (status === "Rejected") return { label: "Từ chối", cls: "rejected" };
  return { label: status, cls: "" };
}

const typeVi: Record<string, string> = {
  "Air Conditioner": "Điều hòa",
  Projector: "Máy chiếu",
  "Wi-Fi": "Wi-Fi",
  Light: "Đèn",
  Fan: "Quạt"
};

function typeLabel(type: string) {
  return typeVi[type] ?? type;
}

function formatWhen(iso: string) {
  return new Date(iso).toLocaleString("vi-VN", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit"
  });
}

export function RequestsPage() {
  const { session } = useAuth();
  const [items, setItems] = useState<MaintenanceRequest[]>([]);
  const [assets, setAssets] = useState<Asset[]>([]);
  const [location, setLocation] = useState("");
  const [assetId, setAssetId] = useState<number>(0);
  const [description, setDescription] = useState("");
  const [filter, setFilter] = useState<string>("All");
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);

  const assetMap = useMemo(() => new Map(assets.map((a) => [a.assetId, a])), [assets]);

  const locations = useMemo(
    () => [...new Set(assets.map((a) => a.location).filter(Boolean))].sort((a, b) => a.localeCompare(b, "vi")),
    [assets]
  );

  const assetsInLocation = useMemo(
    () => assets.filter((a) => a.location === location).sort((a, b) => a.name.localeCompare(b.name, "vi")),
    [assets, location]
  );

  const selectedAsset = assetId ? assetMap.get(assetId) : undefined;

  function assetLabel(id: number | null) {
    if (!id) return "Chưa gắn tài sản";
    const a = assetMap.get(id);
    return a ? a.name : `Tài sản #${id}`;
  }

  function assetLocation(id: number | null) {
    if (!id) return "";
    return assetMap.get(id)?.location ?? "";
  }

  const summary = useMemo(() => {
    let waiting = 0;
    let working = 0;
    let done = 0;
    let rejected = 0;
    for (const r of items) {
      const d = displayStatus(r.status).label;
      if (d === "Chờ phân công") waiting += 1;
      else if (d === "Đang xử lý") working += 1;
      else if (d === "Hoàn thành") done += 1;
      else if (d === "Từ chối") rejected += 1;
    }
    return { total: items.length, waiting, working, done, rejected };
  }, [items]);

  const filteredItems = useMemo(() => {
    if (filter === "All") return items;
    return items.filter((r) => displayStatus(r.status).label === filter);
  }, [items, filter]);

  function pickLocation(nextLocation: string, list: Asset[] = assets) {
    setLocation(nextLocation);
    const first = list.find((a) => a.location === nextLocation);
    setAssetId(first?.assetId ?? 0);
  }

  async function refresh() {
    if (!session) return;
    const [reqs, as] = await Promise.all([endpoints.requests(session.token), endpoints.assets(session.token)]);
    setItems(reqs);
    setAssets(as);
    const locs = [...new Set(as.map((a) => a.location).filter(Boolean))].sort((a, b) => a.localeCompare(b, "vi"));
    const keep = location && locs.includes(location) ? location : locs[0] ?? "";
    const stillValid = keep && as.some((a) => a.location === keep && a.assetId === assetId);
    if (!stillValid) {
      pickLocation(keep, as);
    } else {
      setLocation(keep);
    }
  }

  useEffect(() => {
    refresh().catch((e) => setError(e.message));
  }, [session]);

  async function onCreate(e: FormEvent) {
    e.preventDefault();
    if (!session) return;
    setError("");
    setBusy(true);
    try {
      await endpoints.createRequest(session.token, { assetId, description });
      setDescription("");
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không tạo được yêu cầu");
    } finally {
      setBusy(false);
    }
  }

  const filters = [
    { key: "All", label: "Tổng", count: summary.total },
    { key: "Chờ phân công", label: "Chờ phân công", count: summary.waiting },
    { key: "Đang xử lý", label: "Đang xử lý", count: summary.working },
    { key: "Hoàn thành", label: "Hoàn thành", count: summary.done },
    { key: "Từ chối", label: "Từ chối", count: summary.rejected }
  ];

  return (
    <section className="req-page">
      <PageHeader
        kicker="SỰ CỐ"
        title="Yêu cầu sự cố"
        hint="Chọn vị trí → thiết bị, mô tả ngắn. Facility Manager sẽ phân công kỹ thuật trên Work Order."
      />

      <div className="wo-summary req-summary">
        {filters.map((f) => (
          <button
            key={f.key}
            type="button"
            className={`wo-summary-item req-filter-chip${filter === f.key ? " is-active" : ""}`}
            onClick={() => setFilter(f.key)}
          >
            <span>{f.label}</span>
            <strong>{f.count}</strong>
          </button>
        ))}
      </div>

      {error && <p className="error">{error}</p>}

      <form className="card form-grid req-create" onSubmit={onCreate}>
        <div className="req-create-head">
          <h3>Báo cáo sự cố</h3>
          <p className="muted req-hint">Chọn phòng/khu vực trước, rồi chọn đúng thiết bị tại đó.</p>
        </div>

        <div className="req-asset-picker">
          <label>
            1. Vị trí
            <select value={location} onChange={(e) => pickLocation(e.target.value)}>
              {locations.length === 0 && <option value="">Chưa có vị trí</option>}
              {locations.map((loc) => (
                <option key={loc} value={loc}>
                  {loc}
                </option>
              ))}
            </select>
          </label>
          <label>
            2. Thiết bị tại vị trí
            <select
              value={assetId}
              onChange={(e) => setAssetId(Number(e.target.value))}
              disabled={!location || assetsInLocation.length === 0}
            >
              {assetsInLocation.length === 0 && <option value={0}>Chưa có thiết bị</option>}
              {assetsInLocation.map((a) => (
                <option key={a.assetId} value={a.assetId}>
                  {a.name} ({typeLabel(a.type)})
                </option>
              ))}
            </select>
          </label>
        </div>

        {selectedAsset && (
          <div className="req-selected" aria-live="polite">
            <span className="req-selected-label">Đã chọn</span>
            <strong>{selectedAsset.name}</strong>
            <span className="muted">
              {typeLabel(selectedAsset.type)} · {selectedAsset.location}
            </span>
          </div>
        )}

        <label>
          Mô tả sự cố
          <textarea
            required
            maxLength={500}
            placeholder="VD: Điều hòa kêu to, làm mát chậm khi giờ học…"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </label>
        <div className="req-create-foot">
          <span className="muted">{description.length}/500</span>
          <button className="btn primary" disabled={busy || !assetId} type="submit">
            {busy ? "Đang gửi..." : "Gửi yêu cầu"}
          </button>
        </div>
      </form>

      <div className="table-wrap wo-table-wrap">
        <table className="wo-table req-table req-table-v2">
          <thead>
            <tr>
              <th>Tài sản</th>
              <th>Nội dung</th>
              <th>Trạng thái</th>
              <th>Thời gian</th>
            </tr>
          </thead>
          <tbody>
            {filteredItems.map((r) => {
              const st = displayStatus(r.status);
              return (
                <tr key={r.requestId}>
                  <td>
                    {r.assetId ? (
                      <div className="req-asset">
                        <strong className="req-asset-name">{assetLabel(r.assetId)}</strong>
                        {assetLocation(r.assetId) && (
                          <small className="muted">{assetLocation(r.assetId)}</small>
                        )}
                      </div>
                    ) : (
                      <span className="muted">—</span>
                    )}
                  </td>
                  <td>
                    <span className="req-desc" title={r.description}>
                      {r.description}
                    </span>
                  </td>
                  <td>
                    <span className={`pill ${st.cls}`} title={r.status}>
                      {st.label}
                    </span>
                  </td>
                  <td>
                    <time className="wo-time">{formatWhen(r.createdAt)}</time>
                  </td>
                </tr>
              );
            })}
            {filteredItems.length === 0 && (
              <tr>
                <td className="empty" colSpan={4}>
                  {filter === "All" ? "Chưa có yêu cầu sự cố." : `Không có yêu cầu ở trạng thái ${filter}.`}
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
