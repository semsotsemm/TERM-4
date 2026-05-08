// src/components/Catalog/index.tsx
import React, { useState } from 'react';
import { useProducts } from '../../contexts/ProductContext';
import type { IProduct } from '../../contexts/ProductContext';
import { useAuth } from '../../contexts/AuthContext';
import ProductCard from '../ProductCard';
import ProductForm from '../ProductForm';

const Catalog: React.FC = () => {
  const { products, loading, error, addProduct, updateProduct, deleteProduct } = useProducts();
  const { user, logout } = useAuth();

  const [showForm, setShowForm] = useState(false);
  const [editingProduct, setEditingProduct] = useState<IProduct | null>(null);

  const handleAdd = () => {
    setEditingProduct(null);
    setShowForm(true);
  };

  const handleEdit = (product: IProduct) => {
    setEditingProduct(product);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Удалить товар?')) return;
    try {
      await deleteProduct(id);
    } catch (err) {
      alert(`Ошибка удаления: ${(err as Error).message}`);
    }
  };

  const handleFormSubmit = async (data: IProduct) => {
    try {
      if (editingProduct) {
        await updateProduct(data);
      } else {
        const { id, ...rest } = data;
        void id; // id генерируется сервером при POST
        await addProduct(rest);
      }
      setShowForm(false);
      setEditingProduct(null);
    } catch (err) {
      alert(`Ошибка сохранения: ${(err as Error).message}`);
    }
  };

  const handleFormCancel = () => {
    setShowForm(false);
    setEditingProduct(null);
  };

  return (
    <div style={styles.page}>
      {/* Шапка */}
      <header style={styles.header}>
        <div style={styles.headerLeft}>
          <h1 style={styles.headerTitle}>Каталог товаров</h1>
          {user && (
            <span style={styles.username}>
              Привет, <strong>{user.username}</strong>
            </span>
          )}
        </div>
        <div style={styles.headerRight}>
          <button style={styles.btnAdd} onClick={handleAdd}>
            + Добавить
          </button>
          <button style={styles.btnLogout} onClick={logout}>
            Выйти
          </button>
        </div>
      </header>

      {/* Контент */}
      <main style={styles.main}>
        {loading && <p style={styles.status}>Загрузка товаров...</p>}
        {error && <p style={{ ...styles.status, color: '#e53935' }}>Ошибка: {error}</p>}
        {!loading && !error && products.length === 0 && (
          <p style={styles.status}>Товары не найдены.</p>
        )}
        <div style={styles.grid}>
          {products.map((product) => (
            <ProductCard
              key={product.id}
              product={product}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </div>
      </main>

      {/* Модальная форма */}
      {showForm && (
        <ProductForm
          initial={editingProduct}
          onSubmit={handleFormSubmit}
          onCancel={handleFormCancel}
        />
      )}
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  page: {
    minHeight: '100vh',
    background: '#f5f6fa',
    fontFamily: 'sans-serif',
  },
  header: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: '16px 32px',
    background: '#1976d2',
    color: '#fff',
    boxShadow: '0 2px 8px rgba(0,0,0,0.12)',
    flexWrap: 'wrap',
    gap: '12px',
  },
  headerLeft: {
    display: 'flex',
    alignItems: 'center',
    gap: '16px',
  },
  headerTitle: {
    margin: 0,
    fontSize: '20px',
    fontWeight: 700,
  },
  username: {
    fontSize: '14px',
    opacity: 0.9,
  },
  headerRight: {
    display: 'flex',
    gap: '10px',
  },
  btnAdd: {
    padding: '8px 18px',
    background: '#fff',
    color: '#1976d2',
    border: 'none',
    borderRadius: '6px',
    cursor: 'pointer',
    fontWeight: 600,
    fontSize: '14px',
  },
  btnLogout: {
    padding: '8px 18px',
    background: 'rgba(255,255,255,0.15)',
    color: '#fff',
    border: '1px solid rgba(255,255,255,0.4)',
    borderRadius: '6px',
    cursor: 'pointer',
    fontSize: '14px',
  },
  main: {
    padding: '24px 32px',
    maxWidth: '1400px',
    margin: '0 auto',
  },
  status: {
    textAlign: 'center',
    padding: '40px',
    color: '#666',
    fontSize: '16px',
  },
  grid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(240px, 1fr))',
    gap: '20px',
  },
};

export default Catalog;
