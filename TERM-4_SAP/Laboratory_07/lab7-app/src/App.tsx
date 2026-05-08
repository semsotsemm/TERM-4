// src/App.tsx
import React from 'react';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { ProductProvider } from './contexts/ProductContext';
import RegistrationForm from './components/RegistrationForm';
import Catalog from './components/Catalog';

// Внутренний компонент, который видит AuthContext
const AppContent: React.FC = () => {
  const { isAuthenticated } = useAuth();

  if (!isAuthenticated) {
    return (
      <div style={{ minHeight: '100vh', background: '#f5f6fa' }}>
        <RegistrationForm />
      </div>
    );
  }

  return (
    <ProductProvider>
      <Catalog />
    </ProductProvider>
  );
};

const App: React.FC = () => (
  <AuthProvider>
    <AppContent />
  </AuthProvider>
);

export default App;
