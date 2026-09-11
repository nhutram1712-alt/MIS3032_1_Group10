import { Navigate } from "react-router-dom";

/** Legacy /admin → Người dùng */
export function AdminPage() {
  return <Navigate to="/admin/users" replace />;
}
