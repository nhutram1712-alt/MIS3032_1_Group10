"""Chạy eval set cho AI Prediction Service (src/ai/main.py) — không cần mở server.

Cách dùng (từ thư mục chứa file này):
    pip install fastapi==0.115.0 pydantic==2.9.2 httpx
    python run_eval.py                      # tự tìm ../../../src/ai
    python run_eval.py --service ../../src/ai   # chỉ định đường dẫn khác

Kết quả: in bảng PASS/FAIL ra màn hình và ghi eval-results.csv cùng thư mục.
"""
import argparse
import csv
import json
import sys
from pathlib import Path

HERE = Path(__file__).resolve().parent
VALID_RISK = {"Low", "Medium", "High"}
API_KEY = "DEV_ONLY_AI_SERVICE_KEY"  # key mặc định trong src/ai/main.py (chỉ dùng DEV)


def load_service(service_dir: Path):
    sys.path.insert(0, str(service_dir))
    import main  # noqa: E402  (src/ai/main.py)

    from fastapi.testclient import TestClient

    return TestClient(main.app)


def headers_for(mode: str) -> dict:
    return {
        "valid": {"X-Api-Key": API_KEY},
        "invalid": {"X-Api-Key": "WRONG_KEY"},
        "missing": {},
        "x-service-key": {"X-Service-Key": API_KEY},
    }[mode]


def evaluate(row: dict, client) -> dict:
    resp = client.post("/predict", content=row["request_json"],
                       headers={"Content-Type": "application/json", **headers_for(row["header_mode"])})
    status = resp.status_code
    try:
        body = resp.json()
    except ValueError:
        body = {}
    problems = []

    if status != int(row["expected_status"]):
        problems.append(f"status {status} ≠ {row['expected_status']}")

    actual_risk = body.get("risk", "") if status == 200 and isinstance(body, dict) else ""
    if status == 200:
        if actual_risk not in VALID_RISK:
            problems.append(f"risk '{actual_risk}' ngoài {{Low,Medium,High}}")
        if body.get("horizonDays") != 7:
            problems.append(f"horizonDays {body.get('horizonDays')} ≠ 7")
        if row["expected_risk"] and actual_risk != row["expected_risk"]:
            problems.append(f"risk {actual_risk} ≠ {row['expected_risk']}")

    spec = row["expected_fields"]
    if spec and status == 200 and isinstance(body, dict):
        kind, _, names = spec.partition(":")
        names = set(names.split(";"))
        keys = set(body.keys())
        if kind == "only" and not keys <= names:
            problems.append(f"có field ngoài phạm vi hỗ trợ: {sorted(keys - names)}")
        if kind == "has" and not names <= keys:
            problems.append(f"thiếu field: {sorted(names - keys)}")

    return {
        "case_id": row["case_id"], "group": row["group"], "scenario": row["scenario"],
        "expected": f"{row['expected_status']} {row['expected_risk']}".strip(),
        "actual": f"{status} {actual_risk}".strip(),
        "result": "PASS" if not problems else "FAIL",
        "detail": "; ".join(problems), "if_fail": row["if_fail"],
    }


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--service", default=str(HERE.parents[2] / "src" / "ai"))
    ap.add_argument("--eval-set", default=str(HERE / "eval-set.csv"))
    args = ap.parse_args()

    client = load_service(Path(args.service))
    with open(args.eval_set, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))
    results = [evaluate(r, client) for r in rows]

    out = HERE / "eval-results.csv"
    with open(out, "w", newline="", encoding="utf-8-sig") as f:
        w = csv.DictWriter(f, fieldnames=list(results[0].keys()))
        w.writeheader()
        w.writerows(results)

    for r in results:
        print(f"{r['case_id']:4} {r['result']:4} exp={r['expected']:<12} act={r['actual']:<12} {r['detail']}")
    passed = sum(r["result"] == "PASS" for r in results)
    print(f"\nTổng: {passed}/{len(results)} PASS ({passed / len(results):.0%}) — chi tiết ghi tại {out.name}")


if __name__ == "__main__":
    main()
