import { NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../auth";

const roleLabel: Record<string, string> = {
  FacilityManager: "Facility Manager",
  Requester: "Requester",
  Technician: "Technician",
  Admin: "Admin"
};

const displayName: Record<string, string> = {
  manager: "Nguyễn Văn Quản",
  requester: "Nguyễn Thị Yêu Cầu",
  tech1: "Trần Văn Kỹ",
  admin: "Lê Thị Quản Trị"
};

type NavItem = { to: string; label: string; icon: IconName; prefix?: boolean };
type IconName = "home" | "box" | "alert" | "clipboard" | "users" | "chart" | "wifi";

const navByRole: Record<string, NavItem[]> = {
  Requester: [
    { to: "/overview", label: "Tổng quan", icon: "home" },
    { to: "/requests", label: "Yêu cầu sự cố", icon: "alert" }
  ],
  FacilityManager: [
    { to: "/overview", label: "Tổng quan", icon: "home" },
    { to: "/assets", label: "Tài sản", icon: "box", prefix: true },
    { to: "/work-orders", label: "Work Order", icon: "clipboard" },
    { to: "/alerts", label: "IoT Alerts", icon: "alert" },
    { to: "/predictions", label: "AI Risks", icon: "chart" }
  ],
  Technician: [
    { to: "/overview", label: "Tổng quan", icon: "home" },
    { to: "/work-orders", label: "Work Order", icon: "clipboard" },
    { to: "/assets", label: "Tài sản & AI", icon: "box", prefix: true },
    { to: "/alerts", label: "IoT Alerts", icon: "alert" }
  ],
  Admin: [
    { to: "/overview", label: "Tổng quan", icon: "home" },
    { to: "/admin/users", label: "Người dùng", icon: "users" },
    { to: "/admin/iot", label: "IoT", icon: "wifi" }
  ]
};

function NavIcon({ name }: { name: IconName }) {
  const common = {
    width: 18,
    height: 18,
    viewBox: "0 0 24 24",
    fill: "none",
    stroke: "currentColor",
    strokeWidth: 1.8,
    strokeLinecap: "round" as const,
    strokeLinejoin: "round" as const,
    "aria-hidden": true
  };
  switch (name) {
    case "home":
      return (
        <svg {...common}>
          <path d="M4 10.5 12 4l8 6.5V20a1 1 0 0 1-1 1h-5v-6H10v6H5a1 1 0 0 1-1-1z" />
        </svg>
      );
    case "box":
      return (
        <svg {...common}>
          <path d="M12 3 21 8v8l-9 5-9-5V8z" />
          <path d="M12 13V3M21 8 12 13 3 8" />
        </svg>
      );
    case "alert":
      return (
        <svg {...common}>
          <path d="M12 9v4" />
          <path d="M12 17h.01" />
          <path d="M10.3 4.7 2.8 18a2 2 0 0 0 1.7 3h15a2 2 0 0 0 1.7-3L13.7 4.7a2 2 0 0 0-3.4 0z" />
        </svg>
      );
    case "clipboard":
      return (
        <svg {...common}>
          <rect x="6" y="5" width="12" height="16" rx="2" />
          <path d="M9 5V4a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v1" />
          <path d="M9 11h6M9 15h4" />
        </svg>
      );
    case "chart":
      return (
        <svg {...common}>
          <path d="M4 19h16" />
          <path d="M7 16V10" />
          <path d="M12 16V6" />
          <path d="M17 16v-4" />
        </svg>
      );
    case "wifi":
      return (
        <svg {...common}>
          <path d="M5 12.5a9 9 0 0 1 14 0" />
          <path d="M8.5 15a5 5 0 0 1 7 0" />
          <path d="M12 18h.01" />
        </svg>
      );
    default:
      return (
        <svg {...common}>
          <circle cx="9" cy="8" r="3" />
          <circle cx="16" cy="9" r="2.4" />
          <path d="M4 19a5 5 0 0 1 10 0" />
          <path d="M14 17.5a4.5 4.5 0 0 1 6 0" />
        </svg>
      );
  }
}

function initials(name: string) {
  return name
    .split(" ")
    .filter(Boolean)
    .slice(-2)
    .map((p) => p[0])
    .join("")
    .toUpperCase();
}

export function AppLayout() {
  const { session, logout } = useAuth();
  const links = navByRole[session?.role ?? ""] ?? [];
  const name = displayName[session?.username ?? ""] ?? session?.username ?? "";

  return (
    <div className="app-shell">
      <aside>
        <div className="brand">
          <span>DUE</span>
          <div>
            <strong>Smart Campus</strong>
            <small>Facility Management</small>
          </div>
        </div>
        <p className="nav-label">Menu</p>
        <nav>
          {links.map((l) => (
            <NavLink
              key={l.to}
              to={l.to}
              end={!l.prefix}
              className={({ isActive }) => (isActive ? "nav-item is-current" : "nav-item")}
            >
              <NavIcon name={l.icon} />
              {l.label}
            </NavLink>
          ))}
        </nav>
        <div className="aside-user">
          <div className="user-chip">
            <span className="avatar">{initials(name)}</span>
            <div>
              <strong>{name}</strong>
              <small>{roleLabel[session?.role ?? ""] ?? session?.role}</small>
            </div>
          </div>
          <button className="btn ghost" onClick={() => void logout()} type="button">
            Đăng xuất
          </button>
        </div>
      </aside>
      <main>
        <Outlet />
      </main>
    </div>
  );
}
