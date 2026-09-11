import { createContext, useContext, useMemo, useState, type ReactNode } from "react";
import { clearSession, loadSession, logout as apiLogout, saveSession, type Session } from "./api";

type AuthState = {
  session: Session | null;
  setSession: (s: Session | null) => void;
  logout: () => Promise<void>;
};

const AuthContext = createContext<AuthState | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSessionState] = useState<Session | null>(() => loadSession());

  const value = useMemo<AuthState>(
    () => ({
      session,
      setSession: (s) => {
        setSessionState(s);
        if (s) saveSession(s);
        else clearSession();
      },
      logout: async () => {
        const token = session?.token;
        if (token) await apiLogout(token);
        clearSession();
        setSessionState(null);
      }
    }),
    [session]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
