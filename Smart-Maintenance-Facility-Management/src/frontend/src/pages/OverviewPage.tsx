import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
  endpoints,
  type Asset,
  type IotMapping,
  type MaintenanceRequest,
  type UserSummary,
  type WorkOrder
} from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

export function OverviewPage() {
  const { session } = useAuth();
  const [assets, setAssets] = useState<Asset[]>([]);
  const [requests, setRequests] = useState<MaintenanceRequest[]>([]);
  const [orders, setOrders] = useState<WorkOrder[]>([]);
  const [users, setUsers] = useState<UserSummary[]>([]);
  const [mappings, setMappings] = useState<IotMapping[]>([]);
  const [error, setError] = useState("");
  const isAdmin = session?.role === "Admin";

  useEffect(() => {
    if (!session) return;
    const load = async () => {
      if (session.role !== "Requester") {
        try {
          setAssets(await endpoints.assets(session.token));
        } catch {
          setAssets([]);
        }
      }

      if (session.role === "Requester" || session.role === "FacilityManager") {
        try {
          setRequests(await endpoints.requests(session.token));
        } catch {
          setRequests([]);
        }
      }

      if (session.role === "FacilityManager" || session.role === "Technician") {
        try {
          setOrders(await endpoints.workOrders(session.token));
        } catch {
          setOrders([]);
        }
      }

      if (session.role === "Admin") {
        try {
          setUsers(await endpoints.users(session.token));
          setMappings(await endpoints.iotMappings(session.token));
        } catch {
          setUsers([]);
          setMappings([]);
        }
      }
    };
    load().catch((e) => setError(e.message));
  }, [session]);

  const warning = assets.filter((a) => a.status !== "Operational").length;
  const activeUsers = users.filter((u) => u.isActive !== false).length;
  const isRequester = session?.role === "Requester";

  return (
    <section>
      <PageHeader
        kicker="TỔNG QUAN"
        title="Smart Campus Facility Management"
        hint={
          isRequester
            ? "Gửi và theo dõi yêu cầu sự cố thiết bị trên campus."
            : "Theo dõi tài sản, sự cố và công việc bảo trì trên cùng một màn hình."
        }
      />
      {error && <p className="error">{error}</p>}

      <div className="stat-grid">
        {isAdmin ? (
          <>
            <Link className="stat-card" to="/assets">
              <span>Tài sản</span>
              <strong>{assets.length}</strong>
              <small>{warning} thiết bị cần chú ý</small>
            </Link>
            <Link className="stat-card" to="/admin/users">
              <span>Người dùng</span>
              <strong>{users.length}</strong>
              <small>{activeUsers} đang hoạt động</small>
            </Link>
            <Link className="stat-card" to="/admin/iot">
              <span>IoT Mapping</span>
              <strong>{mappings.length}</strong>
              <small>Thiết bị đã liên kết</small>
            </Link>
          </>
        ) : isRequester ? (
          <Link className="stat-card" to="/requests">
            <span>Yêu cầu sự cố</span>
            <strong>{requests.length}</strong>
            <small>Gửi báo cáo và theo dõi trạng thái</small>
          </Link>
        ) : (
          <>
            <Link className="stat-card" to="/assets">
              <span>Tài sản</span>
              <strong>{assets.length}</strong>
              <small>{warning} thiết bị cần chú ý</small>
            </Link>
            <Link className="stat-card" to="/work-orders">
              <span>Work Order</span>
              <strong>{orders.length}</strong>
              <small>
                {session?.role === "FacilityManager"
                  ? `${requests.filter((r) => r.status === "Submitted").length} chờ phân công`
                  : "Công việc đang theo dõi"}
              </small>
            </Link>
          </>
        )}
      </div>
    </section>
  );
}
