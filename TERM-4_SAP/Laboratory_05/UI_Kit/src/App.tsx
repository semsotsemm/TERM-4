import React, { useState } from 'react';
import { Button } from './components/ui/Button';
import { Input } from './components/ui/Input';
import { Badge } from './components/ui/Badge';
import { LayoutCard } from './components/ui/LayoutCard';

export const App = () => {
  const [inputValue, setInputValue] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInputValue(e.target.value);
  };

  return (
    <div style={{ padding: '20px', maxWidth: '500px' }}>
      <LayoutCard 
        title={<div style={{ display: 'flex', justifyContent: 'space-between' }}>
                 <span>Мониторинг сервера</span>
                 <Badge color="green" text="online" />
               </div>}
        footer={<Button variant="primary" isFullWidth>Сохранить настройки</Button>}
      >
        <Input 
          label="IP-адрес сервера"
          value={inputValue}
          onChange={handleChange}
          placeholder="192.168.1.1"
          isFullWidth
          error={inputValue.length > 15 ? 'Неверный формат IP' : undefined}
        />
        
        <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
          <Button variant="danger" size="small">Перезагрузить</Button>
          <Button variant="secondary" size="small" isLoading>Обновить логи</Button>
        </div>
      </LayoutCard>
    </div>
  );
};