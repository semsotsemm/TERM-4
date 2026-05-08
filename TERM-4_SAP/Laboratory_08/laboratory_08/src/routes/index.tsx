import React from "react";
import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/")({
  component: LandingPage,
});

function LandingPage() {
  return (
    <main className="landing">
      <div className="landing-grid-bg" />

      <div className="landing-badge">
        <span>⬡</span>
        ЛР №8 — TanStack Ecosystem
      </div>

      <h1 className="landing-title">
        MODERN
        <br />
        <span className="accent">CATALOG</span>
        <br />
        APP
      </h1>

      <p className="landing-sub">
        TanStack Router × TanStack Query × Zod.
        <br />
        Типобезопасный роутинг, серверное состояние и декларативный data-fetching.
      </p>

      <div className="landing-actions">
        <Link to="/catalog" className="btn btn-lime">
          Открыть каталог →
        </Link>
        <Link to="/login" className="btn btn-ghost">
          Войти
        </Link>
      </div>

      <div className="landing-tech">
        {[
          "TanStack Router v1",
          "TanStack Query v5",
          "Zod v3",
          "React 18",
          "TypeScript 5",
          "Vite 5",
        ].map((t) => (
          <div key={t} className="tech-pill">
            <span style={{ color: "var(--lime)" }}>✓</span> {t}
          </div>
        ))}
      </div>
    </main>
  );
}
