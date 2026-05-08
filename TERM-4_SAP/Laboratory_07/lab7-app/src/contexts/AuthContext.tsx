// src/contexts/AuthContext.tsx
import React, { createContext, useContext, useReducer, useEffect } from 'react';

export interface IUser {
  email: string;
  username: string;
}

interface IAuthState {
  user: IUser | null;
  isAuthenticated: boolean;
}

type TAuthAction =
  | { type: 'LOGIN'; payload: IUser }
  | { type: 'LOGOUT' };

const AUTH_STORAGE_KEY = 'lab7_user';

function authReducer(state: IAuthState, action: TAuthAction): IAuthState {
  switch (action.type) {
    case 'LOGIN':
      return { user: action.payload, isAuthenticated: true };
    case 'LOGOUT':
      return { user: null, isAuthenticated: false };
    default:
      return state;
  }
}

interface IAuthContext extends IAuthState {
  login: (user: IUser) => void;
  logout: () => void;
}

const AuthContext = createContext<IAuthContext | null>(null);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, {
    user: null,
    isAuthenticated: false,
  });

  // Восстановление сессии из localStorage при загрузке
  useEffect(() => {
    try {
      const stored = localStorage.getItem(AUTH_STORAGE_KEY);
      if (stored) {
        const user: IUser = JSON.parse(stored);
        dispatch({ type: 'LOGIN', payload: user });
      }
    } catch {
      localStorage.removeItem(AUTH_STORAGE_KEY);
    }
  }, []);

  const login = (user: IUser) => {
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(user));
    dispatch({ type: 'LOGIN', payload: user });
  };

  const logout = () => {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    dispatch({ type: 'LOGOUT' });
  };

  return (
    <AuthContext.Provider value={{ ...state, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): IAuthContext => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
};
