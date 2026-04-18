// src/App.tsx
import React from 'react';
import './App.css'; // Импорт стилей приложения
import RegistrationForm from './components/RegistrationForm';

const App: React.FC = () => {
  return (
    <div className="app-container">
      <RegistrationForm />
    </div>
  );
};

export default App;