import { useAuth } from './contexts/useAuth';
import { CatalogPage } from './pages/CatalogPage';
import { ProductProvider } from './contexts/ProductContext';
import { AuthForm } from './components/AuthForm';
import { Layout } from './components/Layout'; 
import './App.css';

function App() {
  const { isAuthenticated } = useAuth();

  return (
    <Layout>
      {!isAuthenticated ? (
        <AuthForm />
      ) : (
        <ProductProvider>
          <CatalogPage />
        </ProductProvider>
      )}
    </Layout>
  );
}

export default App;