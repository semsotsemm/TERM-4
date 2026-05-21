import { redirect, createRoute } from '@tanstack/react-router'
import { z } from 'zod'
import CatalogPage from '../components/catalog/CatalogPage'
import { isUserAuthenticated } from '../contexts/AuthContext'
import { rootRoute } from './__root'

const CatalogSearchSchema = z.object({
  category: z
    .string()
    .trim()
    .min(1)
    .optional()
    .catch(undefined),
})

export type CatalogSearch = z.infer<typeof CatalogSearchSchema>

const CatalogRouteComponent = () => {
  return <CatalogPage />
}

export const catalogRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/catalog',
  beforeLoad: () => {
    if (!isUserAuthenticated()) {
      throw redirect({ to: '/login' })
    }
  },
  validateSearch: (search) => CatalogSearchSchema.parse(search),
  component: CatalogRouteComponent,
})
