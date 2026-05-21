import { Link, createRoute } from '@tanstack/react-router'
import { rootRoute } from './__root'

const LandingPage = () => {
  return (
    <main className="landing-page">
      <div className="landing-card">
        <h1>Лабораторная №8</h1>
        <p>TanStack Router и TanStack Query для маршрутизации и серверного состояния каталога товаров.</p>
        <div className="landing-actions">
          <Link to="/catalog" className="landing-btn primary">
            Перейти в каталог
          </Link>
          <Link to="/login" className="landing-btn secondary">
            Страница входа
          </Link>
        </div>
      </div>
    </main>
  )
}

export const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: LandingPage,
})
