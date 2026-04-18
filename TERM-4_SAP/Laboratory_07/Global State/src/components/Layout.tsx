import React from 'react';
import { useAuth } from '../contexts/useAuth';

interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, logout, isAuthenticated } = useAuth();

  return (
    <div className="min-h-screen">
      {/* Шапка отображается только для авторизованных пользователей */}
      {isAuthenticated && (
        <header className="main-header">
          <div className="header-content">
            <div className="logo">MyStore API</div>
            <div className="user-info">
              <span>Привет, <strong>{user?.name}</strong></span>
              <button className="btn-logout" onClick={logout}>
                Выйти
              </button>
            </div>
          </div>
        </header>
      )}

      <main className="container">
        {children}
      </main>

      <footer className="main-footer">
        <p>&copy; 2026 Учебная Лабораторная №7</p>
      </footer>
    </div>
  );
};