import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { endpoints, type Prediction } from "../api";
import { useAuth } from "../auth";
import { PageHeader } from "./PageHeader";

export function PredictionsPage() {
  const { session } = useAuth();
  const [items, setItems] = useState<Prediction[]>([]);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!session) return;
    endpoints
      .predictions(session.token, "risk_desc")
      .then(setItems)
      .catch((e) => setError(e.message));
  }, [session]);

  return (
    <section>
      <PageHeader
        kicker="AI"
        title="Dashboard rủi ro bảo trì"
        hint="Tổng hợp dự đoán rủi ro cho toàn bộ tài sản — AI chỉ hỗ trợ quyết định, không tự tạo Work Order."
      />
      {error && <p className="error">{error}</p>}
      <div className="table-wrap">
        <table>
          <thead>
            <tr>
              <th>TÀI SẢN</th>
              <th>VỊ TRÍ</th>
              <th>RỦI RO</th>
              <th>DỰ ĐOÁN LÚC</th>
              <th>GHI CHÚ</th>
            </tr>
          </thead>
          <tbody>
            {items.map((p) => (
              <tr key={p.assetId}>
                <td>
                  <Link to={`/assets/${p.assetId}`}>
                    {p.assetName?.trim() || `Tài sản #${p.assetId}`}
                  </Link>
                </td>
                <td>{p.assetLocation ?? p.location ?? "—"}</td>
                <td>
                  <strong className={`risk ${p.risk.toLowerCase()}`}>{p.risk}</strong>
                </td>
                <td>{new Date(p.predictedAt).toLocaleString("vi-VN")}</td>
                <td className="muted">
                  {p.stale ? "Stale (>24h) · " : ""}
                  {p.basedOnSampleData ? "Dữ liệu mẫu MVP" : "Dữ liệu thực"}
                </td>
              </tr>
            ))}
            {items.length === 0 && (
              <tr>
                <td className="empty" colSpan={5}>
                  Chưa có prediction.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
