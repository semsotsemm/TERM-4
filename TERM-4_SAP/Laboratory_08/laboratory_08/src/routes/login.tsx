import React, { useState } from "react";
import { createFileRoute, useNavigate, redirect } from "@tanstack/react-router";
import { useAuth } from "../context/AuthContext";

// Если уже авторизован — редиректим на каталог
export const Route = createFileRoute("/login")({
  beforeLoad: () => {
    const raw = localStorage.getItem("lab8_user");
    if (raw) {
      throw redirect({ to: "/catalog" });
    }
  },
  component: LoginPage,
});

interface FieldErrors {
  name?: string;
  email?: string;
  password?: string;
}

function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [fields, setFields] = useState({ name: "", email: "", password: "" });
  const [errors, setErrors] = useState<FieldErrors>({});
  const [loading, setLoading] = useState(false);

  function validate(): boolean {
    const e: FieldErrors = {};
    if (fields.name.trim().length < 2) e.name = "Минимум 2 символа";
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(fields.email)) e.email = "Некорректный email";
    if (fields.password.length < 6) e.password = "Минимум 6 символов";
    setErrors(e);
    return Object.keys(e).length === 0;
  }

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    setFields((p) => ({ ...p, [name]: value }));
    setErrors((p) => ({ ...p, [name]: undefined }));
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!validate()) return;
    setLoading(true);
    setTimeout(() => {
      login({ name: fields.name.trim(), email: fields.email.trim() });
      navigate({ to: "/catalog" });
    }, 500);
  }

  return (
    <main className="login-page">
      {/* Декоративные кольца */}
      <div className="login-deco">
        {[320, 520, 720, 920].map((s) => (
          <div
            key={s}
            className="login-deco-ring"
            style={{
              width: s, height: s,
              top: "50%", left: "50%",
              transform: "translate(-50%, -50%)",
              opacity: 0.06,
            }}
          />
        ))}
      </div>

      <div className="login-card">
        <div className="login-eyebrow">// AUTH</div>
        <h1 className="login-title">Войти</h1>
        <p className="login-sub">доступ к защищённому каталогу</p>

        <form onSubmit={handleSubmit} noValidate className="login-form">
          <div className="form-group">
            <label className="form-label" htmlFor="name">Имя</label>
            <input
              id="name" name="name" type="text"
              placeholder="Иван"
              value={fields.name}
              onChange={handleChange}
              className={`form-input${errors.name ? " is-error" : ""}`}
              autoComplete="name"
            />
            {errors.name && <span className="form-error-text">⚠ {errors.name}</span>}
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="email">Email</label>
            <input
              id="email" name="email" type="email"
              placeholder="ivan@mail.ru"
              value={fields.email}
              onChange={handleChange}
              className={`form-input${errors.email ? " is-error" : ""}`}
              autoComplete="email"
            />
            {errors.email && <span className="form-error-text">⚠ {errors.email}</span>}
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="password">Пароль</label>
            <input
              id="password" name="password" type="password"
              placeholder="••••••••"
              value={fields.password}
              onChange={handleChange}
              className={`form-input${errors.password ? " is-error" : ""}`}
              autoComplete="new-password"
            />
            {errors.password && <span className="form-error-text">⚠ {errors.password}</span>}
          </div>

          <button type="submit" className="btn btn-lime login-submit" disabled={loading}>
            {loading ? <><span className="spinner" /> Вход...</> : "login() →"}
          </button>
        </form>

        <p className="login-hint">
          Данные сохраняются в localStorage.
          <br />
          Сессия восстанавливается автоматически.
        </p>
      </div>
    </main>
  );
}
