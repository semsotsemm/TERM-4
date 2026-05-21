import { createRouter } from '@tanstack/react-router'
import { rootRoute } from './routes/__root'
import { cartRoute } from './routes/cart'
import { catalogRoute } from './routes/catalog'
import { indexRoute } from './routes/index'
import { loginRoute } from './routes/login'
import { productRoute } from './routes/product.$id'

const routeTree = rootRoute.addChildren([indexRoute, loginRoute, catalogRoute, cartRoute, productRoute])

export const router = createRouter({
  routeTree,
})

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}
