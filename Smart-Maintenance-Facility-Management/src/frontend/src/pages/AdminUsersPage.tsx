import { FormEvent, useEffect, useMemo, useState } from "react";
import { endpoints, type UserSummary } from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

const createRoles = ["Requester", "Technician", "FacilityManager", "Admin"];
const rotatableRoles = ["Technician", "FacilityManager"];

const roleVi: Record<string, string> = {
  Requester: "Người yêu cầu",
  Technician: "Kỹ thuật viên",
  FacilityManager: "Facility Manager",
  Admin: "Admin"
};

function canRotateRole(role: string) {
  return rotatableRoles.includes(role);
}

export function AdminUsersPage() {
  const { session } = useAuth();
  const [users, setUsers] = useState<UserSummary[]>([]);
  const [roleFilter, setRoleFilter] = useState("all");
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  const [showCreate, setShowCreate] = useState(false);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [fullName, setFullName] = useState("");
  const [role, setRole] = useState("Technician");

  async function refresh() {
    if (!session) return;
    setUsers(await endpoints.users(session.token));
  }

  useEffect(() => {
    if (session?.role !== "Admin") return;
    refresh().catch((e) => setError(e.message));
  }, [session]);

  const filtered = useMemo(
    () => (roleFilter === "all" ? users : users.filter((u) => u.role === roleFilter)),
    [users, roleFilter]
  );

  const summary = useMemo(() => {
    const active = users.filter((u) => u.isActive !== false).length;
    const byRole: Record<string, number> = {};
    for (const u of users) byRole[u.role] = (byRole[u.role] ?? 0) + 1;
    return { total: users.length, active, byRole };
  }, [users]);

  async function onCreateUser(e: FormEvent) {
    e.preventDefault();
    if (!session) return;
    setBusy(true);
    setError("");
    try {
      await endpoints.createUser(session.token, {
        username,
        password,
        role,
        fullName: fullName || undefined
      });
      setUsername("");
      setPassword("");
      setFullName("");
      setShowCreate(false);
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không tạo được user");
    } finally {
      setBusy(false);
    }
  }

  async function toggleActive(u: UserSummary) {
    if (!session) return;
    if (u.role === "Admin") {
      setError("Không thể sửa tài khoản Admin qua endpoint này.");
      return;
    }
    setError("");
    try {
      await endpoints.updateUser(session.token, u.userId, { isActive: !(u.isActive ?? true) });
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không cập nhật được user");
    }
  }

  async function changeRole(u: UserSummary, nextRole: string) {
    if (!session) return;
    if (!canRotateRole(u.role) || !canRotateRole(nextRole)) {
      setError("Chỉ luân chuyển được giữa Technician và FacilityManager.");
      return;
    }
    if (nextRole === u.role) return;
    setError("");
    try {
      await endpoints.updateUser(session.token, u.userId, { role: nextRole });
      await refresh();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Không đổi được role");
    }
  }

  if (session?.role !== "Admin") {
    return (
      <section>
        <PageHeader kicker="HỆ THỐNG" title="Người dùng" hint="Chỉ Admin mới truy cập được khu vực này." />
      </section>
    );
  }

  return (
    <section className="admin-page">
      <PageHeader
        kicker="HỆ THỐNG"
        title="Người dùng"
        hint="Tạo tài khoản, kích hoạt/vô hiệu hoá và luân chuyển Technician ↔ Facility Manager."
      />
      {error && <p className="error">{error}</p>}

      <div className="wo-summary admin-summary">
        <div className="wo-summary-item">
          <span>Tổng user</span>
          <strong>{summary.total}</strong>
        </div>
        <div className="wo-summary-item">
          <span>Đang hoạt động</span>
          <strong>{summary.active}</strong>
        </div>
        {Object.entries(summary.byRole).map(([r, n]) => (
          <button
            key={r}
            type="button"
            className={`wo-summary-item req-filter-chip${roleFilter === r ? " is-active" : ""}`}
            onClick={() => setRoleFilter(roleFilter === r ? "all" : r)}
          >
            <span>{roleVi[r] ?? r}</span>
            <strong>{n}</strong>
          </button>
        ))}
      </div>

      <div className="admin-toolbar">
        {!showCreate ? (
          <button className="btn primary" type="button" onClick={() => setShowCreate(true)}>
            Thêm người dùng
          </button>
        ) : null}
        {roleFilter !== "all" && (
          <button className="btn" type="button" onClick={() => setRoleFilter("all")}>
            Xem tất cả role
          </button>
        )}
      </div>

      {showCreate && (
        <form className="card form-grid admin-create" onSubmit={onCreateUser}>
          <div className="assets-create-head">
            <h3>Thêm người dùng</h3>
            <button className="btn btn-compact" type="button" onClick={() => setShowCreate(false)}>
              Đóng
            </button>
          </div>
          <div className="form-row">
            <label>
              Username
              <input required value={username} onChange={(e) => setUsername(e.target.value)} />
            </label>
            <label>
              Mật khẩu
              <input required type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </label>
          </div>
          <div className="form-row">
            <label>
              Họ tên
              <input value={fullName} onChange={(e) => setFullName(e.target.value)} placeholder="Nguyễn Văn A" />
            </label>
            <label>
              Vai trò
              <select value={role} onChange={(e) => setRole(e.target.value)}>
                {createRoles.map((r) => (
                  <option key={r} value={r}>
                    {roleVi[r] ?? r}
                  </option>
                ))}
              </select>
            </label>
          </div>
          <p className="muted admin-note">Admin chỉ gán khi tạo mới. Sau đó chỉ đổi được Technician ↔ Facility Manager.</p>
          <button className="btn primary" disabled={busy} type="submit">
            {busy ? "Đang tạo..." : "Tạo tài khoản"}
          </button>
        </form>
      )}

      <div className="table-wrap admin-table-wrap">
        <table className="admin-users-table">
          <thead>
            <tr>
              <th>Người dùng</th>
              <th>Vai trò</th>
              <th>Trạng thái</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {filtered.map((u) => {
              const rotatable = canRotateRole(u.role);
              const active = u.isActive ?? true;
              return (
                <tr key={u.userId} className={active ? undefined : "is-ended"}>
                  <td>
                    <div className="admin-user-cell">
                      <span className="wo-tech-avatar">{(u.fullName || u.username).slice(0, 2).toUpperCase()}</span>
                      <div>
                        <strong>{u.fullName || u.username}</strong>
                        <small className="muted">@{u.username}</small>
                      </div>
                    </div>
                  </td>
                  <td>
                    {rotatable ? (
                      <select
                        className="admin-role-select"
                        value={u.role}
                        onChange={(e) => changeRole(u, e.target.value)}
                      >
                        {rotatableRoles.map((r) => (
                          <option key={r} value={r}>
                            {roleVi[r] ?? r}
                          </option>
                        ))}
                      </select>
                    ) : (
                      <span className="pill assigned" title={u.role}>
                        {roleVi[u.role] ?? u.role}
                      </span>
                    )}
                  </td>
                  <td>
                    <span className={`pill ${active ? "operational" : "cancelled"}`}>
                      {active ? "Hoạt động" : "Vô hiệu"}
                    </span>
                  </td>
                  <td>
                    <button
                      className={`btn btn-compact ${active ? "btn-danger-outline" : "primary"}`}
                      type="button"
                      disabled={u.role === "Admin"}
                      onClick={() => toggleActive(u)}
                    >
                      {active ? "Vô hiệu hoá" : "Kích hoạt"}
                    </button>
                  </td>
                </tr>
              );
            })}
            {filtered.length === 0 && (
              <tr>
                <td className="empty" colSpan={4}>
                  Không có người dùng.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
