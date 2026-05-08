// src/components/ProductForm/index.tsx
import React, { useState, useEffect } from 'react';
import { ProductSchema } from '../../contexts/ProductContext';
import type { IProduct } from '../../contexts/ProductContext';

interface IProductFormProps {
  initial?: IProduct | null;
  onSubmit: (data: IProduct) => void;
  onCancel: () => void;
}

interface IFormFields {
  id: string;
  title: string;
  price: string;
  description: string;
  category: string;
  thumbnail: string;
}

interface IFieldErrors {
  id?: string;
  title?: string;
  price?: string;
  description?: string;
  category?: string;
  thumbnail?: string;
}

const ProductForm: React.FC<IProductFormProps> = ({ initial, onSubmit, onCancel }) => {
  const [fields, setFields] = useState<IFormFields>({
    id: '',
    title: '',
    price: '',
    description: '',
    category: '',
    thumbnail: '',
  });
  const [fieldErrors, setFieldErrors] = useState<IFieldErrors>({});

  // Предзаполнение при редактировании
  useEffect(() => {
    if (initial) {
      setFields({
        id: String(initial.id),
        title: initial.title,
        price: String(initial.price),
        description: initial.description ?? '',
        category: initial.category ?? '',
        thumbnail: initial.thumbnail ?? '',
      });
    } else {
      setFields({ id: '', title: '', price: '', description: '', category: '', thumbnail: '' });
    }
    setFieldErrors({});
  }, [initial]);

  const handleChange = (field: keyof IFormFields, value: string) => {
    setFields((prev) => ({ ...prev, [field]: value }));
    setFieldErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const raw = {
      id: Number(fields.id),
      title: fields.title.trim(),
      price: Number(fields.price),
      description: fields.description.trim() || undefined,
      category: fields.category.trim() || undefined,
      thumbnail: fields.thumbnail.trim() || undefined,
    };

    // Валидация через Zod safeParse
    const result = ProductSchema.safeParse(raw);

    if (!result.success) {
      // Используем flatten() для безопасного извлечения ошибок
      const flat = result.error.flatten();
      const errors: IFieldErrors = {};
      for (const [field, messages] of Object.entries(flat.fieldErrors)) {
        if (messages && messages.length > 0) {
          errors[field as keyof IFieldErrors] = messages[0];
        }
      }
      setFieldErrors(errors);

      // Показываем все ошибки пользователю
      const allMessages = Object.values(errors).filter(Boolean).join('\n');
      alert(allMessages);
      return;
    }

    onSubmit(result.data);
  };

  const isEditing = !!initial;

  return (
    <div style={styles.overlay}>
      <div style={styles.modal}>
        <h2 style={styles.title}>{isEditing ? 'Редактировать товар' : 'Добавить товар'}</h2>
        <form onSubmit={handleSubmit}>
          {!isEditing && (
            <div style={styles.inputGroup}>
              <label style={styles.label}>ID (число):</label>
              <input
                style={styles.input}
                type="number"
                value={fields.id}
                onChange={(e) => handleChange('id', e.target.value)}
                placeholder="Уникальный числовой ID"
              />
              {fieldErrors.id && <span style={styles.error}>{fieldErrors.id}</span>}
            </div>
          )}
          <div style={styles.inputGroup}>
            <label style={styles.label}>Название (мин. 3 символа):</label>
            <input
              style={styles.input}
              type="text"
              value={fields.title}
              onChange={(e) => handleChange('title', e.target.value)}
              placeholder="Введите название товара"
            />
            {fieldErrors.title && <span style={styles.error}>{fieldErrors.title}</span>}
          </div>
          <div style={styles.inputGroup}>
            <label style={styles.label}>Цена (больше 0):</label>
            <input
              style={styles.input}
              type="number"
              step="0.01"
              value={fields.price}
              onChange={(e) => handleChange('price', e.target.value)}
              placeholder="0.00"
            />
            {fieldErrors.price && <span style={styles.error}>{fieldErrors.price}</span>}
          </div>
          <div style={styles.inputGroup}>
            <label style={styles.label}>Категория:</label>
            <input
              style={styles.input}
              type="text"
              value={fields.category}
              onChange={(e) => handleChange('category', e.target.value)}
              placeholder="Например: electronics"
            />
          </div>
          <div style={styles.inputGroup}>
            <label style={styles.label}>Описание:</label>
            <textarea
              style={{ ...styles.input, resize: 'vertical', minHeight: '60px' }}
              value={fields.description}
              onChange={(e) => handleChange('description', e.target.value)}
              placeholder="Краткое описание товара"
            />
          </div>
          <div style={styles.inputGroup}>
            <label style={styles.label}>Ссылка на изображение:</label>
            <input
              style={styles.input}
              type="url"
              value={fields.thumbnail}
              onChange={(e) => handleChange('thumbnail', e.target.value)}
              placeholder="https://..."
            />
          </div>
          <div style={styles.buttons}>
            <button type="button" style={styles.btnCancel} onClick={onCancel}>
              Отмена
            </button>
            <button type="submit" style={styles.btnSubmit}>
              {isEditing ? 'Сохранить' : 'Добавить'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  overlay: {
    position: 'fixed',
    inset: 0,
    background: 'rgba(0,0,0,0.45)',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    zIndex: 1000,
    padding: '16px',
  },
  modal: {
    background: '#fff',
    borderRadius: '12px',
    padding: '28px',
    width: '100%',
    maxWidth: '480px',
    maxHeight: '90vh',
    overflowY: 'auto',
    boxShadow: '0 8px 32px rgba(0,0,0,0.18)',
  },
  title: { margin: '0 0 20px', fontSize: '18px', color: '#1a1a1a' },
  inputGroup: { display: 'flex', flexDirection: 'column', marginBottom: '14px' },
  label: { fontSize: '13px', color: '#555', marginBottom: '4px' },
  input: { padding: '8px 12px', border: '1px solid #ccc', borderRadius: '6px', fontSize: '14px' },
  error: { color: '#e53935', fontSize: '12px', marginTop: '4px' },
  buttons: { display: 'flex', gap: '10px', marginTop: '20px' },
  btnSubmit: { flex: 1, padding: '10px', background: '#1976d2', color: '#fff', border: 'none', borderRadius: '6px', cursor: 'pointer', fontSize: '14px' },
  btnCancel: { flex: 1, padding: '10px', background: '#f5f5f5', color: '#333', border: '1px solid #ccc', borderRadius: '6px', cursor: 'pointer', fontSize: '14px' },
};

export default ProductForm;
