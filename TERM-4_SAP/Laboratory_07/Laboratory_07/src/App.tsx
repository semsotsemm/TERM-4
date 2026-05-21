import { AuthProvider, useAuth } from './contexts/AuthContext'
import { ProductProvider } from './contexts/ProductContext'
import AuthPage from './components/auth/AuthPage'
import CatalogPage from './components/catalog/CatalogPage'

const AppContent = () => {
  const { isAuthenticated } = useAuth()

  if (!isAuthenticated) {
    return <AuthPage />
  }

  return (
    <ProductProvider>
      <CatalogPage />
    </ProductProvider>
  )
}

const App = () => {
  return (
    <AuthProvider>
      <AppContent />
    </AuthProvider>
  )
}

export default App