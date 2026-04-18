import React, { useState } from 'react';
import { useAuth } from '../contexts/useAuth';

export const AuthForm: React.FC = () => {
  const { login } = useAuth();
  const [username, setUsername] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (username.trim().length >= 3) {
      login({ name: username, id: Date.now() });
    } else {
      alert("Имя должно быть не короче 3 символов");
    }
  };

  return (
    <div className="auth-wrapper">
      <form className="auth-form" onSubmit={handleSubmit}>
        <h2>Авторизация</h2>
        <div className="form-group">
          <label>Логин</label>
          <input 
            type="text" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
            placeholder="Введите ваше имя..."
          />
        </div>
        <button type="submit" className="btn-primary">Войти</button>
      </form>
    </div>
  );
};