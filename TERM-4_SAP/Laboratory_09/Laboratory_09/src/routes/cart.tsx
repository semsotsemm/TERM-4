import { createRoute, redirect } from '@tanstack/react-router'
import CartPage from '../components/cart/CartPage'
import { isUserAuthenticated } from '../contexts/AuthContext'
import { rootRoute } from './__root'

export const cartRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/cart',
  beforeLoad: () => {
    if (!isUserAuthenticated()) {
      throw redirect({ to: '/login' })
    }
  },
  component: CartPage,
})
