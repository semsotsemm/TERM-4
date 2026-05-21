import { Link, Outlet, createRootRoute } from '@tanstack/react-router'
import { useAuth } from '../contexts/AuthContext'

const Navbar = () => {
  const { isAuthenticated } = useAuth()

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
        <Link to="/login" className="nav-link" activeProps={{ className: 'nav-link active' }}>
          {isAuthenticated ? 'Аккаунт' : 'Войти'}
        </Link>
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
