import React from "react";
import {
  createRootRouteWithContext,
  Link,
  Outlet,
  useRouterState,
} from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/router-devtools";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import type { QueryClient } from "@tanstack/react-query";
import { useAuth } from "../context/AuthContext";

// Контекст роутера — передаём queryClient для использования в beforeLoad
interface RouterContext {
  queryClient: QueryClient;
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: RootLayout,
});

function RootLayout() {
  const { user, isAuthenticated, logout } = useAuth();
  const routerState = useRouterState();
  const currentPath = routerState.location.pathname;

  return (
    <>
      {/* ====== NAVBAR ====== */}
      <nav className="navbar">
        <div className="navbar-inner">
          {/* Логотип */}
          <Link to="/" className="navbar-logo">
            <span className="navbar-logo-bracket">[</span>
            KATALOG
            <span className="navbar-logo-bracket">]</span>
          </Link>

          {/* Ссылки */}
          <div className="navbar-links">
            <Link
              to="/"
              className={`nav-link ${currentPath === "/" ? "active" : ""}`}
            >
              ~/home
            </Link>
            <Link
              to="/catalog"
              className={`nav-link ${currentPath.startsWith("/catalog") ? "active" : ""}`}
            >
              ~/catalog
            </Link>
          </div>

          <div className="navbar-spacer" />

          {/* Пользователь */}
          {isAuthenticated && user && (
            <div className="navbar-user">
              <div className="user-chip">
                <div className="user-dot">
                  {user.name.charAt(0).toUpperCase()}
                </div>
                <span className="user-name">{user.name}</span>
              </div>
              <button className="btn btn-ghost btn-sm" onClick={logout}>
                exit()
              </button>
            </div>
          )}

          {!isAuthenticated && (
            <Link to="/login" className="btn btn-lime btn-sm">
              login()
            </Link>
          )}
        </div>
      </nav>

      {/* ====== PAGE CONTENT ====== */}
      <Outlet />

      {/* ====== DEVTOOLS (только в dev) ====== */}
      <TanStackRouterDevtools position="bottom-right" />
      <ReactQueryDevtools initialIsOpen={false} buttonPosition="bottom-left" />
    </>
  );
}
