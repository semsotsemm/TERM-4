import { QueryClientProvider } from '@tanstack/react-query'
import { RouterProvider } from '@tanstack/react-router'
import { RecoilRoot } from 'recoil'
import { AuthProvider } from './contexts/AuthContext'
import { appQueryClient } from './lib/queryClient'
import { router } from './router'

const App = () => {
  return (
    <AuthProvider>
      <RecoilRoot>
        <QueryClientProvider client={appQueryClient}>
          <RouterProvider router={router} />
        </QueryClientProvider>
      </RecoilRoot>
    </AuthProvider>
  )
}

export default App