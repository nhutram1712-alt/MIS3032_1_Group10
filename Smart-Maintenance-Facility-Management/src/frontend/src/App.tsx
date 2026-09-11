import { Navigate, Outlet, Route, Routes } from "react-router-dom";
import { useAuth } from "./auth";
import { AdminIotPage } from "./pages/AdminIotPage";
import { AdminPage } from "./pages/AdminPage";
import { AdminUsersPage } from "./pages/AdminUsersPage";
import { AlertsPage } from "./pages/AlertsPage";
import { AppLayout } from "./pages/AppLayout";
import { AssetDetailPage } from "./pages/AssetDetailPage";
import { AssetsPage } from "./pages/AssetsPage";
import { LoginPage } from "./pages/LoginPage";
import { OverviewPage } from "./pages/OverviewPage";
import { PredictionsPage } from "./pages/PredictionsPage";
import { RequestsPage } from "./pages/RequestsPage";
import { WorkOrdersPage } from "./pages/WorkOrdersPage";

function RequireAuth() {
  const { session } = useAuth();
  if (!session) return <Navigate to="/login" replace />;
  return <Outlet />;
}

function RequireRoles({ roles }: { roles: string[] }) {
  const { session } = useAuth();
  if (!session) return <Navigate to="/login" replace />;
  if (!roles.includes(session.role)) return <Navigate to="/" replace />;
  return <Outlet />;
}

function HomeRedirect() {
  const { session } = useAuth();
  if (session?.role === "Admin") return <Navigate to="/admin/users" replace />;
  return <Navigate to="/overview" replace />;
}

function RequestsRoute() {
  const { session } = useAuth();
  if (session?.role === "FacilityManager") return <Navigate to="/work-orders" replace />;
  if (session?.role !== "Requester") return <Navigate to="/" replace />;
  return <RequestsPage />;
}

export function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route element={<RequireAuth />}>
        <Route element={<AppLayout />}>
          <Route path="/" element={<HomeRedirect />} />
          <Route path="/overview" element={<OverviewPage />} />
          <Route element={<RequireRoles roles={["FacilityManager", "Technician", "Admin"]} />}>
            <Route path="/assets" element={<AssetsPage />} />
            <Route path="/assets/:id" element={<AssetDetailPage />} />
          </Route>
          <Route path="/requests" element={<RequestsRoute />} />
          <Route path="/work-orders" element={<WorkOrdersPage />} />
          <Route path="/alerts" element={<AlertsPage />} />
          <Route path="/predictions" element={<PredictionsPage />} />
          <Route path="/admin" element={<AdminPage />} />
          <Route path="/admin/users" element={<AdminUsersPage />} />
          <Route path="/admin/iot" element={<AdminIotPage />} />
        </Route>
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
