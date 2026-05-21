import type { ReactNode } from 'react'

export const ProductProvider = ({ children }: { children: ReactNode }) => {
  return <>{children}</>
}

export const useProducts = () => {
  throw new Error('useProducts is deprecated. Use TanStack Query hooks instead.')
}
