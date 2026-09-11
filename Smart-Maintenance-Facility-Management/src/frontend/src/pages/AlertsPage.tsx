import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { endpoints, type IotAlert } from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

function severityClass(severity: string) {
  return `pill ${severity.toLowerCase()}`;
}

export function AlertsPage() {
  const { session } = useAuth();
  const [items, setItems] = useState<IotAlert[]>([]);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!session) return;
    endpoints
      .iotAlerts(session.token)
      .then(setItems)
      .catch((e) => setError(e.message));
  }, [session]);

  return (
    <section>
      <PageHeader
        kicker="IoT"
        title="Cảnh báo thiết bị"
        hint="Các chỉ số vượt ngưỡng được phát hiện từ dữ liệu cảm biến."
      />
      {error && <p className="error">{error}</p>}
      <div className="table-wrap">
        <table>
          <thead>
            <tr>
              <th>ALERT</th>
              <th>TÀI SẢN</th>
              <th>CHỈ SỐ</th>
              <th>GIÁ TRỊ</th>
              <th>NGƯỠNG</th>
              <th>MỨC ĐỘ</th>
              <th>THỜI ĐIỂM</th>
            </tr>
          </thead>
          <tbody>
            {items.map((a) => (
              <tr key={a.alertId}>
                <td>{a.alertId}</td>
                <td>
                  <Link to={`/assets/${a.assetId}`}>
                    {a.assetName?.trim() || `Tài sản #${a.assetId}`}
                  </Link>
                </td>
                <td>{a.metricType}</td>
                <td>{a.readingValue}</td>
                <td>{a.threshold}</td>
                <td>
                  <span className={severityClass(a.severity)}>{a.severity}</span>
                </td>
                <td>{new Date(a.detectedAt).toLocaleString("vi-VN")}</td>
              </tr>
            ))}
            {items.length === 0 && (
              <tr>
                <td className="empty" colSpan={7}>
                  Chưa có cảnh báo IoT.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
