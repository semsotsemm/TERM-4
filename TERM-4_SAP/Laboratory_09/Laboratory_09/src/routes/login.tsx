import { createRoute } from '@tanstack/react-router'
import AuthPage from '../components/auth/AuthPage'
import { rootRoute } from './__root'

export const loginRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/login',
  component: AuthPage,
})
