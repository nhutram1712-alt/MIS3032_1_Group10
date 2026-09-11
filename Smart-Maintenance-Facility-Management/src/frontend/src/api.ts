export type Role = "Requester" | "Technician" | "FacilityManager" | "Admin";

export type Session = {
  token: string;
  role: Role;
  userId: number;
  username: string;
};

export type Asset = {
  assetId: number;
  name: string;
  type: string;
  location: string;
  status: string;
  maintenanceRisk?: string | null;
  createdAt: string;
};

export type MaintenanceRequest = {
  requestId: number;
  requesterId: number;
  assetId: number | null;
  description: string;
  status: string;
  createdAt: string;
};

export type RequestHistory = {
  requestId: number;
  workOrderId: number | null;
  technicianId: number | null;
  result: string | null;
  completedAt: string | null;
};

export type WorkOrder = {
  orderId: number;
  requestId: number;
  technicianId: number;
  assetId: number;
  status: string;
  rejectionReason?: string | null;
  createdAt: string;
};

export type WorkOrderDetail = WorkOrder & {
  asset?: Asset | null;
  maintenanceHistory?: {
    historyId: number;
    result: string;
    completedAt: string;
  }[];
};

export type UserSummary = {
  userId: number;
  username: string;
  fullName: string;
  role: string;
  isActive?: boolean;
};

export type IotReading = {
  metricType: string;
  value: number;
  readingValue?: number;
  timestamp: string;
};

export type Prediction = {
  assetId: number;
  assetName?: string;
  assetType?: string;
  assetLocation?: string;
  location?: string;
  risk: string;
  predictedAt: string;
  horizonDays?: number;
  stale?: boolean;
  basedOnSampleData?: boolean;
};

export type IotMapping = {
  mappingId: number;
  assetId: number;
  assetName?: string;
  deviceId: string;
  createdAt?: string;
};

export type IotAlert = {
  alertId: number;
  assetId: number;
  assetName?: string;
  metricType: string;
  readingValue: number;
  threshold: number;
  severity: string;
  detectedAt: string;
};

const TOKEN_KEY = "sm_session";

export function loadSession(): Session | null {
  const raw = localStorage.getItem(TOKEN_KEY);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as Session;
  } catch {
    return null;
  }
}

export function saveSession(session: Session) {
  localStorage.setItem(TOKEN_KEY, JSON.stringify(session));
}

export function clearSession() {
  localStorage.removeItem(TOKEN_KEY);
}

async function parseError(res: Response): Promise<string> {
  try {
    const body = await res.json();
    return body.error ?? body.title ?? res.statusText;
  } catch {
    return res.statusText;
  }
}

export async function api<T>(
  path: string,
  options: RequestInit & { token?: string } = {}
): Promise<T> {
  const { token, headers, ...rest } = options;
  const res = await fetch(path, {
    ...rest,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...headers
    }
  });
  if (!res.ok) {
    throw new Error(await parseError(res));
  }
  if (res.status === 204) return undefined as T;
  const text = await res.text();
  if (!text) return undefined as T;
  return JSON.parse(text) as T;
}

export async function login(username: string, password: string) {
  const raw = await api<Record<string, unknown>>("/api/auth/login", {
    method: "POST",
    body: JSON.stringify({ username, password })
  });
  return {
    token: String(raw.token ?? raw.Token ?? ""),
    role: String(raw.role ?? raw.Role ?? "") as Role,
    userId: Number(raw.userId ?? raw.UserId ?? 0),
    username: String(raw.username ?? raw.Username ?? username)
  } satisfies Session;
}

export async function logout(token: string) {
  try {
    await api("/api/auth/logout", { method: "POST", token });
  } catch {
    // Client still clears session even if server logout fails
  }
}

export const endpoints = {
  assets: (token: string, location?: string) =>
    api<Asset[]>(`/api/assets${location ? `?location=${encodeURIComponent(location)}` : ""}`, { token }),
  asset: (token: string, id: number) => api<Asset>(`/api/assets/${id}`, { token }),
  createAsset: (token: string, payload: { name: string; type: string; location: string; status: string }) =>
    api<Asset>("/api/assets", { method: "POST", token, body: JSON.stringify(payload) }),
  updateAsset: (token: string, id: number, payload: { name: string; type: string; location: string }) =>
    api<Asset>(`/api/assets/${id}`, { method: "PUT", token, body: JSON.stringify(payload) }),
  updateAssetStatus: (token: string, id: number, status: string) =>
    api<Asset>(`/api/assets/${id}/status`, { method: "PATCH", token, body: JSON.stringify({ status }) }),

  iotData: (token: string, id: number) => api<IotReading[]>(`/api/assets/${id}/iot-data`, { token }),
  prediction: (token: string, id: number) => api<Prediction>(`/api/assets/${id}/prediction`, { token }),
  predictions: (token: string, sort?: string) =>
    api<Prediction[]>(`/api/predictions${sort ? `?sort=${encodeURIComponent(sort)}` : ""}`, { token }),

  requests: (token: string) => api<MaintenanceRequest[]>("/api/requests", { token }),
  request: (token: string, id: number) => api<MaintenanceRequest>(`/api/requests/${id}`, { token }),
  createRequest: (token: string, payload: { assetId: number; description: string }) =>
    api<MaintenanceRequest>("/api/requests", { method: "POST", token, body: JSON.stringify(payload) }),
  updateRequestStatus: (token: string, id: number, status: string) =>
    api<MaintenanceRequest>(`/api/requests/${id}/status`, {
      method: "PATCH",
      token,
      body: JSON.stringify({ status })
    }),
  requestHistory: (token: string, id: number) =>
    api<RequestHistory>(`/api/requests/${id}/history`, { token }),

  workOrders: (token: string) => api<WorkOrder[]>("/api/work-orders", { token }),
  workOrder: (token: string, id: number) => api<WorkOrderDetail>(`/api/work-orders/${id}`, { token }),
  createWorkOrder: (token: string, payload: { requestId: number; technicianId: number; assetId: number }) =>
    api<WorkOrder>("/api/work-orders", { method: "POST", token, body: JSON.stringify(payload) }),
  patchWorkOrder: (
    token: string,
    id: number,
    payload: {
      technicianId?: number;
      status?: string;
      rejectionReason?: string;
      result?: string;
    }
  ) => api<WorkOrder>(`/api/work-orders/${id}`, { method: "PATCH", token, body: JSON.stringify(payload) }),

  users: (token: string, role?: string) =>
    api<UserSummary[]>(`/api/users${role ? `?role=${encodeURIComponent(role)}` : ""}`, { token }),
  createUser: (token: string, payload: { username: string; password: string; role: string; fullName?: string }) =>
    api<UserSummary>("/api/users", { method: "POST", token, body: JSON.stringify(payload) }),
  updateUser: (token: string, id: number, payload: { role?: string; isActive?: boolean; fullName?: string }) =>
    api<UserSummary>(`/api/users/${id}`, { method: "PUT", token, body: JSON.stringify(payload) }),

  iotMappings: (token: string) => api<IotMapping[]>("/api/iot-mappings", { token }),
  createIotMapping: (token: string, payload: { assetId: number; deviceId?: string }) =>
    api<IotMapping>("/api/iot-mappings", { method: "POST", token, body: JSON.stringify(payload) }),
  updateIotMapping: (token: string, id: number, payload: { deviceId: string }) =>
    api<IotMapping>(`/api/iot-mappings/${id}`, { method: "PUT", token, body: JSON.stringify(payload) }),

  iotAlerts: (token: string, assetId?: number) =>
    api<IotAlert[]>(`/api/iot-alerts${assetId ? `?assetId=${assetId}` : ""}`, { token }),

  rolePermissions: (token: string) =>
    api<Record<string, string[]>>("/api/roles/permissions", { token })
};
