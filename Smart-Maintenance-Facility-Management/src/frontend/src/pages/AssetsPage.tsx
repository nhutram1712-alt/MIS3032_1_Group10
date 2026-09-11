import { FormEvent, useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { endpoints, type Asset } from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

const TYPE_ORDER = ["Air Conditioner", "Projector", "Light", "Fan", "Wi-Fi"] as const;

const typeVi: Record<string, string> = {
  "Air Conditioner": "Điều hòa",
  Projector: "Máy chiếu",
  Light: "Đèn",
  Fan: "Quạt",
  "Wi-Fi": "Wi-Fi"
};

const statusVi: Record<string, string> = {
  Operational: "Hoạt động",
  Warning: "Cảnh báo",
  Maintenance: "Bảo trì",
  "Out of Service": "Ngừng dùng"
};

const riskVi: Record<string, string> = {
  Low: "Thấp",
  Medium: "Trung bình",
  High: "Cao"
};

const ZONE_OPTIONS = [
  { value: "all", label: "Mọi khu vực" },
  { value: "room", label: "Phòng học / phòng họp" },
  { value: "corridor", label: "Hành lang" },
  { value: "hall", label: "Hội trường" },
  { value: "lab", label: "Lab / phòng thí nghiệm" },
  { value: "common", label: "Sảnh, cầu thang, bãi xe, KTX" }
] as const;

function statusClass(status: string) {
  return `pill ${status.replace(/\s/g, "").toLowerCase()}`;
}

function riskClass(risk: string | null | undefined) {
  if (!risk) return "pill";
  return `pill ${risk.toLowerCase()}`;
}

function zoneOf(location: string): string {
  const l = location.toLowerCase();
  if (l.includes("hành lang")) return "corridor";
  if (l.includes("hội trường")) return "hall";
  if (l.includes("lab")) return "lab";
  if (l.includes("sảnh") || l.includes("cầu thang") || l.includes("bãi xe") || l.includes("ktx")) return "common";
  if (l.includes("phòng")) return "room";
  return "other";
}

/** Tòa / khối từ mã phòng (A301 → A) hoặc nhãn khu vực chung. */
function buildingOf(location: string): string {
  const room = location.match(/\b([A-E])\d{2,3}\b/i);
  if (room) return room[1].toUpperCase();
  const phong = location.match(/phòng\s+([A-E])/i);
  if (phong) return phong[1].toUpperCase();
  if (/hành lang tầng\s*1/i.test(location)) return "Tầng 1";
  if (/hành lang tầng\s*2/i.test(location)) return "Tầng 2";
  if (/hành lang tầng\s*3/i.test(location)) return "Tầng 3";
  if (/hội trường/i.test(location)) return "Hội trường";
  if (/lab/i.test(location)) return "Lab";
  if (/sảnh/i.test(location)) return "Sảnh";
  if (/cầu thang/i.test(location)) return "Cầu thang";
  if (/bãi xe/i.test(location)) return "Bãi xe";
  if (/ktx/i.test(location)) return "KTX";
  return "Khác";
}

export function AssetsPage() {
  const { session } = useAuth();
  const [items, setItems] = useState<Asset[]>([]);
  const [zone, setZone] = useState("all");
  const [building, setBuilding] = useState("all");
  const [place, setPlace] = useState("all");
  const [typeFilter, setTypeFilter] = useState("all");
  const [error, setError] = useState("");
  const [name, setName] = useState("");
  const [type, setType] = useState("Air Conditioner");
  const [loc, setLoc] = useState("");
  const [status, setStatus] = useState("Operational");
  const [busy, setBusy] = useState(false);
  const [showCreate, setShowCreate] = useState(false);
  const isFm = session?.role === "FacilityManager";

  async function refresh() {
    if (!session) return;
    const data = await endpoints.assets(session.token);
    setItems(data);
  }

  useEffect(() => {
    refresh().catch((e) => setError(e.message));
  }, [session]);

  const buildings = useMemo(() => {
    const set = new Set(items.map((a) => buildingOf(a.location)));
    return ["all", ...[...set].sort((a, b) => a.localeCompare(b, "vi"))];
  }, [items]);

  const places = useMemo(() => {
    let list = items;
    if (zone !== "all") list = list.filter((a) => zoneOf(a.location) === zone);
    if (building !== "all") list = list.filter((a) => buildingOf(a.location) === building);
    const set = new Set(list.map((a) => a.location));
    return ["all", ...[...set].sort((a, b) => a.localeCompare(b, "vi"))];
  }, [items, zone, building]);

  const locationMatched = useMemo(() => {
    return items.filter((a) => {
      if (zone !== "all" && zoneOf(a.location) !== zone) return false;
      if (building !== "all" && buildingOf(a.location) !== building) return false;
      if (place !== "all" && a.location !== place) return false;
      return true;
    });
  }, [items, zone, building, place]);

  const filtered = useMemo(() => {
    if (typeFilter === "all") return locationMatched;
    return locationMatched.filter((a) => a.type === typeFilter);
  }, [locationMatched, typeFilter]);

  const typeCounts = useMemo(() => {
    const counts: Record<string, number> = { all: items.length };
    for (const t of TYPE_ORDER) counts[t] = items.filter((a) => a.type === t).length;
    return counts;
  }, [items]);

  const grouped = useMemo(() => {
    const map = new Map<string, Asset[]>();
    for (const t of TYPE_ORDER) map.set(t, []);
    for (const a of filtered) {
      if (!map.has(a.type)) map.set(a.type, []);
      map.get(a.type)!.push(a);
    }
    for (const [, list] of map) {
      list.sort((x, y) => x.location.localeCompare(y.location, "vi") || x.name.localeCompare(y.name, "vi"));
    }
    return [...map.entries()].filter(([, list]) => list.length > 0);
  }, [filtered]);

  async function onCreate(e: FormEvent) {
    e.preventDefault();
    if (!session) return;
    setError("");
    setBusy(true);
    try {
      await endpoints.createAsset(session.token, { name, type, location: loc, status });
      setName("");
      setLoc("");
      setShowCreate(false);
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không tạo được tài sản");
    } finally {
      setBusy(false);
    }
  }

  function resetFilters() {
    setZone("all");
    setBuilding("all");
    setPlace("all");
    setTypeFilter("all");
  }

  return (
    <section className="assets-page">
      <PageHeader
        kicker="TÀI SẢN"
        title="Danh mục cơ sở vật chất"
        hint="Xem theo loại thiết bị, lọc theo tòa / khu vực và vị trí cụ thể."
      />
      {error && <p className="error">{error}</p>}

      <div className="assets-filters card">
        <div className="assets-filter-row">
          <label>
            Khu vực
            <select
              value={zone}
              onChange={(e) => {
                setZone(e.target.value);
                setPlace("all");
              }}
            >
              {ZONE_OPTIONS.map((z) => (
                <option key={z.value} value={z.value}>
                  {z.label}
                </option>
              ))}
            </select>
          </label>
          <label>
            Tòa / khối
            <select
              value={building}
              onChange={(e) => {
                setBuilding(e.target.value);
                setPlace("all");
              }}
            >
              {buildings.map((b) => (
                <option key={b} value={b}>
                  {b === "all" ? "Tất cả" : b.length === 1 ? `Tòa ${b}` : b}
                </option>
              ))}
            </select>
          </label>
          <label>
            Vị trí cụ thể
            <select value={place} onChange={(e) => setPlace(e.target.value)}>
              {places.map((p) => (
                <option key={p} value={p}>
                  {p === "all" ? "Tất cả vị trí" : p}
                </option>
              ))}
            </select>
          </label>
          <button className="btn" type="button" onClick={resetFilters}>
            Xóa lọc
          </button>
        </div>

        <div className="assets-type-tabs" role="tablist" aria-label="Loại thiết bị">
          <button
            type="button"
            className={`assets-type-tab${typeFilter === "all" ? " is-active" : ""}`}
            onClick={() => setTypeFilter("all")}
          >
            Tất cả <span>{typeCounts.all}</span>
          </button>
          {TYPE_ORDER.map((t) => (
            <button
              key={t}
              type="button"
              className={`assets-type-tab${typeFilter === t ? " is-active" : ""}`}
              onClick={() => setTypeFilter(t)}
            >
              {typeVi[t] ?? t} <span>{typeCounts[t]}</span>
            </button>
          ))}
        </div>
      </div>

      {isFm && (
        <div className="assets-create-wrap">
          {!showCreate ? (
            <button className="btn primary" type="button" onClick={() => setShowCreate(true)}>
              Thêm tài sản mới
            </button>
          ) : (
            <form className="card form-grid assets-create" onSubmit={onCreate}>
              <div className="assets-create-head">
                <h3>Thêm tài sản mới</h3>
                <button className="btn btn-compact" type="button" onClick={() => setShowCreate(false)}>
                  Đóng
                </button>
              </div>
              <div className="form-row">
                <label>
                  Tên thiết bị
                  <input required placeholder="Ví dụ: Điều hòa Panasonic A303" value={name} onChange={(e) => setName(e.target.value)} />
                </label>
                <label>
                  Loại
                  <select value={type} onChange={(e) => setType(e.target.value)}>
                    {TYPE_ORDER.map((t) => (
                      <option key={t} value={t}>
                        {typeVi[t] ?? t}
                      </option>
                    ))}
                  </select>
                </label>
              </div>
              <div className="form-row">
                <label>
                  Vị trí / phòng
                  <input required placeholder="Ví dụ: Phòng A303" value={loc} onChange={(e) => setLoc(e.target.value)} />
                </label>
                <label>
                  Trạng thái
                  <select value={status} onChange={(e) => setStatus(e.target.value)}>
                    {Object.entries(statusVi).map(([value, label]) => (
                      <option key={value} value={value}>
                        {label}
                      </option>
                    ))}
                  </select>
                </label>
              </div>
              <button className="btn primary" disabled={busy} type="submit">
                {busy ? "Đang lưu..." : "Lưu tài sản"}
              </button>
            </form>
          )}
        </div>
      )}

      {grouped.length === 0 ? (
        <div className="card assets-empty">
          <p className="muted">Không có tài sản khớp bộ lọc.</p>
        </div>
      ) : (
        grouped.map(([typeKey, list]) => (
          <section key={typeKey} className="assets-group">
            <header className="assets-group-head">
              <h3>{typeVi[typeKey] ?? typeKey}</h3>
              <span className="muted">{list.length} thiết bị</span>
            </header>
            <div className="table-wrap assets-table-wrap">
              <table className="assets-table">
                <thead>
                  <tr>
                    <th>Tên</th>
                    <th>Vị trí</th>
                    <th>Trạng thái</th>
                    <th>Rủi ro</th>
                    <th />
                  </tr>
                </thead>
                <tbody>
                  {list.map((a) => (
                    <tr key={a.assetId}>
                      <td>
                        <strong className="assets-name">{a.name}</strong>
                      </td>
                      <td>
                        <span className="assets-loc">{a.location}</span>
                        <small className="muted assets-loc-meta">
                          {buildingOf(a.location).length === 1 ? `Tòa ${buildingOf(a.location)}` : buildingOf(a.location)}
                        </small>
                      </td>
                      <td>
                        <span className={statusClass(a.status)} title={a.status}>
                          {statusVi[a.status] ?? a.status}
                        </span>
                      </td>
                      <td>
                        <span className={riskClass(a.maintenanceRisk)} title={a.maintenanceRisk ?? ""}>
                          {a.maintenanceRisk ? riskVi[a.maintenanceRisk] ?? a.maintenanceRisk : "—"}
                        </span>
                      </td>
                      <td>
                        <Link className="assets-detail" to={`/assets/${a.assetId}`}>
                          Chi tiết
                        </Link>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </section>
        ))
      )}
    </section>
  );
}
