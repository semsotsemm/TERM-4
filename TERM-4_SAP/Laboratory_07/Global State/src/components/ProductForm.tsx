import React, { useState } from 'react';
import { ProductSchema } from '../schemas/productSchema';
import type { Product } from '../schemas/productSchema';

interface Props {
  initialData?: Product;
  onSubmit: (data: Product) => void;
  onCancel: () => void;
}

export const ProductForm: React.FC<Props> = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    title: initialData?.title || '',
    price: initialData?.price || 0
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    // Валидация
    const result = ProductSchema.safeParse({ 
      ...formData, 
      id: initialData?.id || Math.random() 
    });

    if (!result.success) {
      const errors = result.error.flatten().fieldErrors;
      const message = Object.entries(errors)
        .flatMap(([field, texts]) =>
          (texts ?? []).map((text) => `${field}: ${text}`)
        )
        .join('\n');
      alert(message || 'Проверьте корректность введенных данных');
      return;
    }

    onSubmit(result.data);
  };

  return (
    <form onSubmit={handleSubmit} style={{ border: '1px solid #ccc', padding: '10px' }}>
      <input 
        value={formData.title} 
        onChange={e => setFormData({...formData, title: e.target.value})} 
        placeholder="Title" 
      />
      <input 
        type="number" 
        value={formData.price} 
        onChange={e => setFormData({...formData, price: Number(e.target.value)})} 
      />
      <button type="submit">Сохранить</button>
      <button type="button" onClick={onCancel}>Отмена</button>
    </form>
  );
};