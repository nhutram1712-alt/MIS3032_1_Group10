export function PageHeader({ kicker, title, hint }: { kicker: string; title: string; hint?: string }) {
  return (
    <header className="page-head">
      <div>
        <p className="kicker">{kicker}</p>
        <h1>{title}</h1>
        {hint && <p className="hint">{hint}</p>}
      </div>
    </header>
  );
}
