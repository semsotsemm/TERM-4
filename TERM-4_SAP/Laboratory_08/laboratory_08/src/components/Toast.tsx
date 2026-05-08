import React from "react";

interface ToastProps {
  message: string;
  type: "success" | "error";
}

export function Toast({ message, type }: ToastProps) {
  const icon = type === "success" ? "✓" : "!";
  return (
    <div className={`toast ${type}`}>
      <span className="toast-icon">{icon}</span>
      <span className="toast-msg">{message}</span>
    </div>
  );
}
