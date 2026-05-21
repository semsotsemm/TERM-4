import { Link, Outlet, createRootRoute } from '@tanstack/react-router'
import { useRecoilCallback, useRecoilValue } from 'recoil'
import { useAuth } from '../contexts/AuthContext'
import { cartCountState } from '../state/shopState'
import { defaultUiSettings, uiSettingsState } from '../state/uiState'

const Navbar = () => {
  const { isAuthenticated } = useAuth()
  const cartCount = useRecoilValue(cartCountState)
  const resetUiSettings = useRecoilCallback(
    ({ set }) =>
      () => {
        set(uiSettingsState, defaultUiSettings)
      },
    [],
  )

  return (
    <nav className="app-nav">
      <Link to="/" className="nav-brand">
        ShopLab
      </Link>
      <div className="nav-links">
        <Link to="/" className="nav-link" activeProps={{ className: 'nav-link active' }}>
          Главная
        </Link>
        <Link to="/catalog" className="nav-link" activeProps={{ className: 'nav-link active' }}>
          Каталог
        </Link>
        <Link to="/cart" className="nav-link nav-cart-link" activeProps={{ className: 'nav-link nav-cart-link active' }}>
          <span className="nav-cart-icon" aria-hidden="true">
            <svg viewBox="0 0 16 16" fill="none">
              <path
                d="M2 3h1.2c0.3 0 0.5 0.2 0.6 0.5l1.1 5.3c0.1 0.3 0.3 0.5 0.6 0.5h6.4c0.3 0 0.5-0.2 0.6-0.5L13.8 5H5"
                stroke="currentColor"
                strokeWidth="1.2"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <circle cx="6.3" cy="12.4" r="0.9" fill="currentColor" />
              <circle cx="11.3" cy="12.4" r="0.9" fill="currentColor" />
            </svg>
            <span className="nav-cart-badge">{cartCount}</span>
          </span>
          Корзина
        </Link>
        <Link to="/login" className="nav-link" activeProps={{ className: 'nav-link active' }}>
          {isAuthenticated ? 'Аккаунт' : 'Войти'}
        </Link>
        <button className="nav-link nav-reset-btn" type="button" onClick={resetUiSettings}>
          Сбросить настройки
        </button>
      </div>
    </nav>
  )
}

const RootLayout = () => {
  return (
    <div className="app-shell">
      <Navbar />
      <Outlet />
    </div>
  )
}

export const rootRoute = createRootRoute({
  component: RootLayout,
})
