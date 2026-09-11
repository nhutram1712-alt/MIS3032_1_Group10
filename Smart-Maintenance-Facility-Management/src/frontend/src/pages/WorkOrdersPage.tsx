import { FormEvent, useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import {
  endpoints,
  type Asset,
  type MaintenanceRequest,
  type UserSummary,
  type WorkOrder
} from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

const statusVi: Record<string, string> = {
  Assigned: "Đã phân công",
  "In Progress": "Đang thực hiện",
  Completed: "Hoàn thành",
  Cancelled: "Đã hủy"
};

function statusClass(status: string) {
  return `pill ${status.replace(/\s/g, "").toLowerCase()}`;
}

function statusLabel(status: string) {
  return statusVi[status] ?? status;
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

function initials(name: string) {
  const parts = name.trim().split(/\s+/).filter(Boolean);
  if (parts.length === 0) return "?";
  if (parts.length === 1) return parts[0].slice(0, 2).toUpperCase();
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}

export function WorkOrdersPage() {
  const { session } = useAuth();
  const [items, setItems] = useState<WorkOrder[]>([]);
  const [requests, setRequests] = useState<MaintenanceRequest[]>([]);
  const [assets, setAssets] = useState<Asset[]>([]);
  const [techs, setTechs] = useState<UserSummary[]>([]);
  const [woRequest, setWoRequest] = useState(0);
  const [woTech, setWoTech] = useState(0);
  const [creating, setCreating] = useState(false);
  const [rejectBusyId, setRejectBusyId] = useState<number | null>(null);
  const [error, setError] = useState("");
  const [busyId, setBusyId] = useState<number | null>(null);
  const [resultDraft, setResultDraft] = useState<Record<number, string>>({});
  const [rejectDraft, setRejectDraft] = useState<Record<number, string>>({});
  const [reassignTech, setReassignTech] = useState<Record<number, number>>({});
  const isTech = session?.role === "Technician";
  const isFm = session?.role === "FacilityManager";

  const assetMap = useMemo(() => new Map(assets.map((a) => [a.assetId, a])), [assets]);

  function assetInfo(id: number) {
    const a = assetMap.get(id);
    if (!a) return { name: `Tài sản #${id}`, location: "" };
    return { name: a.name, location: a.location };
  }

  const techName = useMemo(() => {
    const map = new Map(techs.map((t) => [t.userId, t.fullName || t.username]));
    return (id: number) => {
      if (map.has(id)) return map.get(id)!;
      if (session?.userId === id) return session.username;
      return `Kỹ thuật #${id}`;
    };
  }, [techs, session]);

  const assignedRequestIds = useMemo(() => new Set(items.map((w) => w.requestId)), [items]);

  const eligibleSubmitted = useMemo(
    () =>
      requests.filter(
        (r) => r.status === "Submitted" && r.assetId && !assignedRequestIds.has(r.requestId)
      ),
    [requests, assignedRequestIds]
  );

  const selectedRequest = eligibleSubmitted.find((r) => r.requestId === woRequest);

  const summary = useMemo(() => {
    const counts = { total: items.length, assigned: 0, inProgress: 0, completed: 0, cancelled: 0 };
    for (const w of items) {
      if (w.status === "Assigned") counts.assigned += 1;
      else if (w.status === "In Progress") counts.inProgress += 1;
      else if (w.status === "Completed") counts.completed += 1;
      else if (w.status === "Cancelled") counts.cancelled += 1;
    }
    return counts;
  }, [items]);

  async function refresh() {
    if (!session) return;
    const [list, as] = await Promise.all([
      endpoints.workOrders(session.token),
      endpoints.assets(session.token)
    ]);
    setItems(list);
    setAssets(as);
    if (isFm) {
      const [users, reqs] = await Promise.all([
        endpoints.users(session.token, "Technician"),
        endpoints.requests(session.token)
      ]);
      setTechs(users);
      setRequests(reqs);
      if (users[0] && !woTech) setWoTech(users[0].userId);
      const taken = new Set(list.map((w) => w.requestId));
      const eligible = reqs.filter((r) => r.status === "Submitted" && r.assetId && !taken.has(r.requestId));
      if (eligible[0] && !eligible.some((r) => r.requestId === woRequest)) {
        setWoRequest(eligible[0].requestId);
      } else if (eligible.length === 0) {
        setWoRequest(0);
      }
    }
  }

  useEffect(() => {
    refresh().catch((e) => setError(e.message));
  }, [session]);

  async function run(orderId: number, action: () => Promise<unknown>) {
    setError("");
    setBusyId(orderId);
    try {
      await action();
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Thao tác thất bại");
    } finally {
      setBusyId(null);
    }
  }

  async function onCreateWo(e: FormEvent) {
    e.preventDefault();
    if (!session || !selectedRequest?.assetId) return;
    setError("");
    setCreating(true);
    try {
      await endpoints.createWorkOrder(session.token, {
        requestId: woRequest,
        technicianId: woTech,
        assetId: selectedRequest.assetId
      });
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không tạo được Work Order");
    } finally {
      setCreating(false);
    }
  }

  async function rejectRequest(requestId: number) {
    if (!session) return;
    setError("");
    setRejectBusyId(requestId);
    try {
      await endpoints.updateRequestStatus(session.token, requestId, "Rejected");
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không từ chối được yêu cầu");
    } finally {
      setRejectBusyId(null);
    }
  }

  return (
    <section className="wo-page">
      <PageHeader
        kicker="CÔNG VIỆC"
        title="Work Order"
        hint={
          isTech
            ? "Bắt đầu, từ chối hoặc hoàn thành công việc được phân công cho bạn."
            : "Giao việc bảo trì từ yêu cầu mới gửi — trạng thái ticket tự theo tiến độ Work Order."
        }
      />

      <div className="wo-summary">
        <div className="wo-summary-item">
          <span>Tổng WO</span>
          <strong>{summary.total}</strong>
        </div>
        {isFm && (
          <div className="wo-summary-item">
            <span>Chờ phân công</span>
            <strong>{eligibleSubmitted.length}</strong>
          </div>
        )}
        <div className="wo-summary-item">
          <span>Đã phân công</span>
          <strong>{summary.assigned}</strong>
        </div>
        <div className="wo-summary-item">
          <span>Đang thực hiện</span>
          <strong>{summary.inProgress}</strong>
        </div>
        <div className="wo-summary-item">
          <span>Hoàn thành</span>
          <strong>{summary.completed}</strong>
        </div>
        <div className="wo-summary-item">
          <span>Đã hủy</span>
          <strong>{summary.cancelled}</strong>
        </div>
      </div>

      {error && <p className="error">{error}</p>}

      {isFm && (
        <form className="card req-wo-compact" onSubmit={onCreateWo}>
          <div className="req-wo-compact-head">
            <h3>Phân công yêu cầu mới</h3>
            <p className="muted req-hint">Chọn yêu cầu đã gửi, giao kỹ thuật viên hoặc từ chối nếu không xử lý được.</p>
          </div>
          <div className="req-wo-compact-row wo-assign-row">
            <label>
              Yêu cầu chờ phân công
              <select value={woRequest} onChange={(e) => setWoRequest(Number(e.target.value))}>
                {eligibleSubmitted.length === 0 && <option value={0}>Không còn yêu cầu chờ phân công</option>}
                {eligibleSubmitted.map((r) => {
                  const asset = r.assetId ? assetInfo(r.assetId) : { name: "—", location: "" };
                  const where = asset.location ? ` · ${asset.location}` : "";
                  return (
                    <option key={r.requestId} value={r.requestId}>
                      {asset.name}
                      {where}
                    </option>
                  );
                })}
              </select>
            </label>
            <label>
              Kỹ thuật viên
              <select value={woTech} onChange={(e) => setWoTech(Number(e.target.value))}>
                {techs.map((t) => (
                  <option key={t.userId} value={t.userId}>
                    {t.fullName}
                  </option>
                ))}
              </select>
            </label>
            <div className="wo-assign-actions">
              <button
                className="btn btn-danger-outline"
                type="button"
                disabled={!woRequest || rejectBusyId === woRequest}
                onClick={() => rejectRequest(woRequest)}
              >
                {rejectBusyId === woRequest ? "Đang từ chối..." : "Từ chối"}
              </button>
              <button className="btn primary" disabled={creating || !woRequest || !woTech} type="submit">
                {creating ? "Đang phân công..." : "Phân công"}
              </button>
            </div>
          </div>
          {selectedRequest && <p className="req-preview req-preview-compact">{selectedRequest.description}</p>}
        </form>
      )}

      <h3 className="wo-section-title">Danh sách Work Order</h3>

      <div className="table-wrap wo-table-wrap">
        <table className="wo-table wo-table-v2">
          <thead>
            <tr>
              <th>Tài sản</th>
              <th>Kỹ thuật viên</th>
              <th>Trạng thái</th>
              <th>Thời gian</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {items.map((w) => {
              const busy = busyId === w.orderId;
              const ended = w.status === "Completed" || w.status === "Cancelled";
              const asset = assetInfo(w.assetId);
              const tech = techName(w.technicianId);
              return (
                <tr key={w.orderId} className={ended ? "is-ended" : undefined}>
                  <td>
                    <div className="wo-asset-cell">
                      <Link className="wo-asset" to={`/assets/${w.assetId}`} title={asset.name}>
                        {asset.name}
                      </Link>
                      {asset.location && <small className="muted">{asset.location}</small>}
                    </div>
                  </td>
                  <td>
                    <div className="wo-tech" title={tech}>
                      <span className="wo-tech-avatar">{initials(tech)}</span>
                      <span>{tech}</span>
                    </div>
                  </td>
                  <td>
                    <div className="wo-status">
                      <span className={statusClass(w.status)} title={w.status}>
                        {statusLabel(w.status)}
                      </span>
                      {w.rejectionReason && (
                        <p className="wo-reason" title={w.rejectionReason}>
                          {w.rejectionReason}
                        </p>
                      )}
                    </div>
                  </td>
                  <td>
                    <time className="wo-time">{formatWhen(w.createdAt)}</time>
                  </td>
                  <td>
                    {ended ? (
                      <span className="wo-done">Đã kết thúc</span>
                    ) : (
                      <div className="wo-actions">
                        {isTech && w.status === "Assigned" && (
                          <div className="wo-action-block">
                            <button
                              className="btn primary btn-compact"
                              disabled={busy}
                              type="button"
                              onClick={() =>
                                run(w.orderId, () =>
                                  endpoints.patchWorkOrder(session!.token, w.orderId, { status: "In Progress" })
                                )
                              }
                            >
                              Bắt đầu
                            </button>
                            <div className="wo-inline-field">
                              <input
                                placeholder="Lý do từ chối"
                                value={rejectDraft[w.orderId] ?? ""}
                                onChange={(e) => setRejectDraft((d) => ({ ...d, [w.orderId]: e.target.value }))}
                              />
                              <button
                                className="btn btn-compact btn-danger-outline"
                                disabled={busy || !(rejectDraft[w.orderId] ?? "").trim()}
                                type="button"
                                onClick={() =>
                                  run(w.orderId, () =>
                                    endpoints.patchWorkOrder(session!.token, w.orderId, {
                                      rejectionReason: rejectDraft[w.orderId]
                                    })
                                  )
                                }
                              >
                                Từ chối
                              </button>
                            </div>
                          </div>
                        )}

                        {isTech && w.status === "In Progress" && (
                          <div className="wo-inline-field wo-inline-field-wide">
                            <input
                              placeholder="Kết quả bảo trì"
                              value={resultDraft[w.orderId] ?? ""}
                              onChange={(e) => setResultDraft((d) => ({ ...d, [w.orderId]: e.target.value }))}
                            />
                            <button
                              className="btn primary btn-compact"
                              disabled={busy || !(resultDraft[w.orderId] ?? "").trim()}
                              type="button"
                              onClick={() =>
                                run(w.orderId, () =>
                                  endpoints.patchWorkOrder(session!.token, w.orderId, {
                                    status: "Completed",
                                    result: resultDraft[w.orderId]
                                  })
                                )
                              }
                            >
                              Hoàn thành
                            </button>
                          </div>
                        )}

                        {isFm && w.status === "Assigned" && (
                          <div className="wo-fm-actions">
                            <select
                              aria-label="Chọn kỹ thuật viên"
                              value={reassignTech[w.orderId] ?? w.technicianId}
                              onChange={(e) =>
                                setReassignTech((d) => ({ ...d, [w.orderId]: Number(e.target.value) }))
                              }
                            >
                              {techs.map((t) => (
                                <option key={t.userId} value={t.userId}>
                                  {t.fullName}
                                </option>
                              ))}
                            </select>
                            <div className="wo-fm-btns">
                              <button
                                className="btn primary btn-compact"
                                disabled={busy}
                                type="button"
                                onClick={() =>
                                  run(w.orderId, () =>
                                    endpoints.patchWorkOrder(session!.token, w.orderId, {
                                      technicianId: reassignTech[w.orderId] ?? w.technicianId
                                    })
                                  )
                                }
                              >
                                Đổi KT
                              </button>
                              <button
                                className="btn btn-compact btn-danger-outline"
                                disabled={busy}
                                type="button"
                                onClick={() =>
                                  run(w.orderId, () =>
                                    endpoints.patchWorkOrder(session!.token, w.orderId, { status: "Cancelled" })
                                  )
                                }
                              >
                                Hủy
                              </button>
                            </div>
                          </div>
                        )}

                        {isFm && w.status === "In Progress" && (
                          <span className="muted wo-note">Kỹ thuật đang thực hiện</span>
                        )}

                        {!isTech && !isFm && <span className="muted">—</span>}
                      </div>
                    )}
                  </td>
                </tr>
              );
            })}
            {items.length === 0 && (
              <tr>
                <td className="empty" colSpan={5}>
                  Chưa có work order.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
