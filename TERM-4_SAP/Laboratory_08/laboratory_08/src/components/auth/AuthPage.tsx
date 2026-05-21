import { useState, type FormEvent } from 'react'
import { useNavigate } from '@tanstack/react-router'
import { useAuth } from '../../contexts/AuthContext'

type AuthTab = 'login' | 'register'

interface LoginFormState {
  email: string
  password: string
}

interface RegisterFormState {
  firstName: string
  lastName: string
  email: string
  password: string
  confirmPassword: string
}

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

const AuthPage = () => {
  const { login, register } = useAuth()
  const navigate = useNavigate()
  const [tab, setTab] = useState<AuthTab>('login')
  const [loginForm, setLoginForm] = useState<LoginFormState>({ email: '', password: '' })
  const [registerForm, setRegisterForm] = useState<RegisterFormState>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
  })

  const [loginErrors, setLoginErrors] = useState<Partial<Record<keyof LoginFormState, string>>>({})
  const [registerErrors, setRegisterErrors] = useState<
    Partial<Record<keyof RegisterFormState, string>>
  >({})

  const handleLoginSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    const nextErrors: Partial<Record<keyof LoginFormState, string>> = {}
    if (!emailRegex.test(loginForm.email.trim())) {
      nextErrors.email = 'Введите корректный email'
    }

    if (loginForm.password.length < 6) {
      nextErrors.password = 'Пароль — минимум 6 символов'
    }

    setLoginErrors(nextErrors)
    if (Object.keys(nextErrors).length > 0) {
      return
    }

    const result = login(loginForm.email, loginForm.password)
    if (!result.ok) {
      alert(result.message)
      return
    }

    void navigate({ to: '/catalog' })
  }

  const handleRegisterSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    const nextErrors: Partial<Record<keyof RegisterFormState, string>> = {}
    if (registerForm.firstName.trim().length < 2) {
      nextErrors.firstName = 'Минимум 2 символа'
    }

    if (registerForm.lastName.trim().length < 2) {
      nextErrors.lastName = 'Минимум 2 символа'
    }

    if (!emailRegex.test(registerForm.email.trim())) {
      nextErrors.email = 'Введите корректный email'
    }

    if (registerForm.password.length < 6) {
      nextErrors.password = 'Минимум 6 символов'
    }

    if (registerForm.password !== registerForm.confirmPassword) {
      nextErrors.confirmPassword = 'Пароли не совпадают'
    }

    setRegisterErrors(nextErrors)
    if (Object.keys(nextErrors).length > 0) {
      return
    }

    register({
      firstName: registerForm.firstName,
      lastName: registerForm.lastName,
      email: registerForm.email,
      password: registerForm.password,
    })

    void navigate({ to: '/catalog' })
  }

  return (
    <div className="auth-page">
      <div className="page">
        <div className="panel-left">
          <div className="brand">
            <div className="brand-mark">
              <svg viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
                <rect x="2" y="5" width="16" height="11" rx="1.5" stroke="white" strokeWidth="1.5" />
                <path
                  d="M7 5V4a3 3 0 016 0v1"
                  stroke="white"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                />
                <circle cx="10" cy="10.5" r="1.5" fill="white" />
              </svg>
            </div>
            <span className="brand-name">ShopLab</span>
          </div>

          <div className="panel-hero">
            <h1>
              Каталог <em>товаров</em>
            </h1>
            <p>Авторизуйтесь, чтобы управлять товарами, обновлять каталог и работать с CRUD.</p>
          </div>
        </div>

        <div className="panel-right">
          <div className="auth-box">
            <div className="tabs">
              <button
                type="button"
                className={tab === 'login' ? 'tab active' : 'tab'}
                onClick={() => setTab('login')}
              >
                Войти
              </button>
              <button
                type="button"
                className={tab === 'register' ? 'tab active' : 'tab'}
                onClick={() => setTab('register')}
              >
                Регистрация
              </button>
            </div>

            {tab === 'login' && (
              <div className="form-section active">
                <h2 className="auth-title">С возвращением</h2>
                <p className="auth-subtitle">Войдите в аккаунт, чтобы продолжить</p>

                <form noValidate onSubmit={handleLoginSubmit}>
                  <div className="form-group">
                    <label className="form-label" htmlFor="login-email">
                      Email
                    </label>
                    <input
                      className={loginErrors.email ? 'form-input error' : 'form-input'}
                      type="email"
                      id="login-email"
                      placeholder="you@example.com"
                      autoComplete="email"
                      value={loginForm.email}
                      onChange={(event) =>
                        setLoginForm((current) => ({ ...current, email: event.target.value }))
                      }
                    />
                    <span className="field-error">{loginErrors.email ?? ''}</span>
                  </div>

                  <div className="form-group">
                    <label className="form-label" htmlFor="login-password">
                      Пароль
                    </label>
                    <input
                      className={loginErrors.password ? 'form-input error' : 'form-input'}
                      type="password"
                      id="login-password"
                      placeholder="Введите пароль"
                      autoComplete="current-password"
                      value={loginForm.password}
                      onChange={(event) =>
                        setLoginForm((current) => ({ ...current, password: event.target.value }))
                      }
                    />
                    <span className="field-error">{loginErrors.password ?? ''}</span>
                  </div>

                  <button type="submit" className="btn-primary">
                    Войти в систему
                  </button>
                </form>

                <p className="terms" style={{ marginTop: '1.25rem' }}>
                  Нет аккаунта?{' '}
                  <button type="button" className="link-button" onClick={() => setTab('register')}>
                    Зарегистрироваться
                  </button>
                </p>
              </div>
            )}

            {tab === 'register' && (
              <div className="form-section active">
                <h2 className="auth-title">Создать аккаунт</h2>
                <p className="auth-subtitle">Заполните данные для регистрации</p>
                <form noValidate onSubmit={handleRegisterSubmit}>
                  <div className="form-row">
                    <div className="form-group">
                      <label className="form-label" htmlFor="reg-firstname">
                        Имя
                      </label>
                      <input
                        className={registerErrors.firstName ? 'form-input error' : 'form-input'}
                        type="text"
                        id="reg-firstname"
                        placeholder="Иван"
                        autoComplete="given-name"
                        value={registerForm.firstName}
                        onChange={(event) =>
                          setRegisterForm((current) => ({
                            ...current,
                            firstName: event.target.value,
                          }))
                        }
                      />
                      <span className="field-error">{registerErrors.firstName ?? ''}</span>
                    </div>
                    <div className="form-group">
                      <label className="form-label" htmlFor="reg-lastname">
                        Фамилия
                      </label>
                      <input
                        className={registerErrors.lastName ? 'form-input error' : 'form-input'}
                        type="text"
                        id="reg-lastname"
                        placeholder="Иванов"
                        autoComplete="family-name"
                        value={registerForm.lastName}
                        onChange={(event) =>
                          setRegisterForm((current) => ({
                            ...current,
                            lastName: event.target.value,
                          }))
                        }
                      />
                      <span className="field-error">{registerErrors.lastName ?? ''}</span>
                    </div>
                  </div>

                  <div className="form-group">
                    <label className="form-label" htmlFor="reg-email">
                      Email
                    </label>
                    <input
                      className={registerErrors.email ? 'form-input error' : 'form-input'}
                      type="email"
                      id="reg-email"
                      placeholder="you@example.com"
                      autoComplete="email"
                      value={registerForm.email}
                      onChange={(event) =>
                        setRegisterForm((current) => ({ ...current, email: event.target.value }))
                      }
                    />
                    <span className="field-error">{registerErrors.email ?? ''}</span>
                  </div>

                  <div className="form-group">
                    <label className="form-label" htmlFor="reg-password">
                      Пароль
                    </label>
                    <input
                      className={registerErrors.password ? 'form-input error' : 'form-input'}
                      type="password"
                      id="reg-password"
                      placeholder="Минимум 6 символов"
                      autoComplete="new-password"
                      value={registerForm.password}
                      onChange={(event) =>
                        setRegisterForm((current) => ({
                          ...current,
                          password: event.target.value,
                        }))
                      }
                    />
                    <span className="field-error">{registerErrors.password ?? ''}</span>
                  </div>

                  <div className="form-group">
                    <label className="form-label" htmlFor="reg-confirm">
                      Подтверждение пароля
                    </label>
                    <input
                      className={registerErrors.confirmPassword ? 'form-input error' : 'form-input'}
                      type="password"
                      id="reg-confirm"
                      placeholder="Повторите пароль"
                      autoComplete="new-password"
                      value={registerForm.confirmPassword}
                      onChange={(event) =>
                        setRegisterForm((current) => ({
                          ...current,
                          confirmPassword: event.target.value,
                        }))
                      }
                    />
                    <span className="field-error">{registerErrors.confirmPassword ?? ''}</span>
                  </div>

                  <button type="submit" className="btn-primary">
                    Создать аккаунт
                  </button>
                </form>

                <p className="terms">
                  Уже есть аккаунт?{' '}
                  <button type="button" className="link-button" onClick={() => setTab('login')}>
                    Войти
                  </button>
                </p>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}

export default AuthPage
