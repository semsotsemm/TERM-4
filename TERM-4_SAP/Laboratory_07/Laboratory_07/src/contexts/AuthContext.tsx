import { createContext, useContext, useMemo, useState, type ReactNode } from 'react'
import type { RegisterPayload, StoredAuthData, User } from '../types/auth'

const SESSION_KEY = 'lab7_auth_session'
const ACCOUNT_KEY = 'lab7_auth_account'

interface AuthContextValue {
  user: User | null
  isAuthenticated: boolean
  register: (payload: RegisterPayload) => void
  login: (email: string, password: string) => { ok: boolean; message?: string }
  logout: () => void
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined)

const parseJson = <T,>(value: string | null): T | null => {
  if (!value) {
    return null
  }

  try {
    return JSON.parse(value) as T
  } catch {
    return null
  }
}

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(() => {
    return parseJson<User>(localStorage.getItem(SESSION_KEY))
  })

  const register = (payload: RegisterPayload) => {
    const nextUser: User = {
      firstName: payload.firstName.trim(),
      lastName: payload.lastName.trim(),
      email: payload.email.trim().toLowerCase(),
    }

    const account: StoredAuthData = {
      user: nextUser,
      password: payload.password,
    }

    localStorage.setItem(ACCOUNT_KEY, JSON.stringify(account))
    localStorage.setItem(SESSION_KEY, JSON.stringify(nextUser))
    setUser(nextUser)
  }

  const login = (email: string, password: string) => {
    const stored = parseJson<StoredAuthData>(localStorage.getItem(ACCOUNT_KEY))
    if (!stored) {
      return { ok: false, message: 'Сначала зарегистрируйте аккаунт.' }
    }

    const normalizedEmail = email.trim().toLowerCase()
    const isValid = stored.user.email === normalizedEmail && stored.password === password

    if (!isValid) {
      return { ok: false, message: 'Неверный email или пароль.' }
    }

    localStorage.setItem(SESSION_KEY, JSON.stringify(stored.user))
    setUser(stored.user)
    return { ok: true }
  }

  const logout = () => {
    localStorage.removeItem(SESSION_KEY)
    setUser(null)
  }

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isAuthenticated: user !== null,
      register,
      login,
      logout,
    }),
    [user],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export const useAuth = () => {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used inside AuthProvider')
  }

  return context
}
