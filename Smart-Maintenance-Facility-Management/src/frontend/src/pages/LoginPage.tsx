import { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api";
import { useAuth } from "../auth";

const DEMO_PASSWORD = "Due@2026";

const demos = [
  { role: "Facility Manager", user: "manager" },
  { role: "Technician", user: "tech1" },
  { role: "Requester", user: "requester" },
  { role: "Admin", user: "admin" }
];

export function LoginPage() {
  const { setSession } = useAuth();
  const nav = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);

  async function onSubmit(e: FormEvent) {
    e.preventDefault();
    setError("");
    setBusy(true);
    try {
      const session = await login(username.trim(), password);
      setSession(session);
      nav("/");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Đăng nhập thất bại");
    } finally {
      setBusy(false);
    }
  }

  return (
    <div className="login-shell">
      <section className="login-hero" aria-label="Campus Đại học Kinh tế Đà Nẵng">
        <div className="login-hero-copy">
          <p className="login-kicker">Đại học Kinh tế – Đại học Đà Nẵng</p>
          <h1>Smart Campus</h1>
          <p className="login-lead">
            Quản lý tài sản, giám sát IoT và bảo trì chủ động với hỗ trợ dự đoán AI.
          </p>
        </div>
      </section>

      <section className="login-panel">
        <form className="login-form" onSubmit={onSubmit} autoComplete="off">
          <h2>Đăng nhập</h2>
          <label>
            Tên đăng nhập
            <input
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              autoComplete="off"
              name="campus-username"
              placeholder="Nhập tài khoản"
              required
            />
          </label>
          <label>
            Mật khẩu
            <div className="password-field">
              <input
                type={showPassword ? "text" : "password"}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="new-password"
                name="campus-password"
                placeholder="Nhập mật khẩu"
                required
              />
              <button type="button" className="text-btn" onClick={() => setShowPassword((v) => !v)}>
                {showPassword ? "Ẩn" : "Hiện"}
              </button>
            </div>
          </label>
          {error && <p className="error">{error}</p>}
          <button className="btn primary block" disabled={busy} type="submit">
            {busy ? "Đang đăng nhập..." : "Đăng nhập"}
          </button>

          <p className="demo-label">Tài khoản demo — mật khẩu mới: {DEMO_PASSWORD}</p>
          <div className="demo-pills">
            {demos.map((d) => (
              <button
                key={d.user}
                type="button"
                className={username === d.user ? "demo-pill active" : "demo-pill"}
                onClick={() => {
                  setUsername(d.user);
                  setPassword(DEMO_PASSWORD);
                  setError("");
                }}
              >
                {d.role}: {d.user}
              </button>
            ))}
          </div>
        </form>
      </section>
    </div>
  );
}
