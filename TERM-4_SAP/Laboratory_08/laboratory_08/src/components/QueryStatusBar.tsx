import React, { useState, useEffect } from "react";

interface QueryStatusBarProps {
  isFetching: boolean;
  isLoading: boolean;
  dataUpdatedAt: number;
  staleTime: number;
}

export function QueryStatusBar({
  isFetching,
  isLoading,
  dataUpdatedAt,
  staleTime,
}: QueryStatusBarProps) {
  const [now, setNow] = useState(Date.now());

  // Обновляем таймер каждую секунду
  useEffect(() => {
    const id = setInterval(() => setNow(Date.now()), 1000);
    return () => clearInterval(id);
  }, []);

  if (!dataUpdatedAt) return null;

  const age = now - dataUpdatedAt;
  const isStale = age > staleTime;

  let status: "fetching" | "fresh" | "stale" = "fresh";
  if (isFetching || isLoading) status = "fetching";
  else if (isStale) status = "stale";

  const labels: Record<typeof status, string> = {
    fetching: "Fetching...",
    fresh: "Fresh",
    stale: "Stale",
  };

  const ageSeconds = Math.floor(age / 1000);

  return (
    <div className="query-status-bar visible">
      <span className={`status-dot ${status}`} />
      <span>{labels[status]}</span>
      <span className="mono" style={{ color: "var(--ink-faint)" }}>
        {ageSeconds}s ago
      </span>
      <span className="mono" style={{ color: "var(--ink-faint)", fontSize: 10 }}>
        staleTime={staleTime / 1000}s
      </span>
    </div>
  );
}
