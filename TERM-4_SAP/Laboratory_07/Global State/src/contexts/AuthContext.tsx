import React, { createContext, useState } from 'react';

interface AuthContextType {
  user: AuthUser | null;
  isAuthenticated: boolean;
  login: (userData: AuthUser) => void;
  logout: () => void;
}

export interface AuthUser {
  id: number;
  name: string;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<AuthUser | null>(() => {
    const savedUser = localStorage.getItem('user');
    if (!savedUser) return null;

    try {
      return JSON.parse(savedUser) as AuthUser;
    } catch {
      localStorage.removeItem('user');
      return null;
    }
  });

  const login = (userData: AuthUser) => {
    localStorage.setItem('user', JSON.stringify(userData));
    setUser(userData);
  };

  const logout = () => {
    localStorage.removeItem('user');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: !!user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthContext };