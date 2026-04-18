import { useState } from 'react';
import { useProducts } from '../contexts/useProducts';
import { ProductForm } from '../components/ProductForm';
import { ProductCard } from '../components/ProductCard';
import type { Product } from '../schemas/productSchema';
import { useAuth } from '../contexts/useAuth';

export const CatalogPage = () => {
  const { state, dispatch } = useProducts();
  const { logout } = useAuth();
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [isAdding, setIsAdding] = useState(false);

  const handleAdd = async (data: Product) => {
    const res = await fetch('https://dummyjson.com/products/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    if (res.ok) {
      dispatch({ type: 'ADD_PRODUCT', payload: { ...data, id: Date.now() } });
      setIsAdding(false);
    }
  };

  const handleUpdate = async (data: Product) => {
    const res = await fetch(`https://dummyjson.com/products/${data.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    if (res.ok) {
      dispatch({ type: 'UPDATE_PRODUCT', payload: data });
      setEditingProduct(null);
    }
  };

  const handleDelete = async (id: number) => {
    const res = await fetch(`https://dummyjson.com/products/${id}`, { method: 'DELETE' });
    if (res.ok) dispatch({ type: 'DELETE_PRODUCT', payload: id });
  };

  return (
    <div>
      <header style={{ display: 'flex', justifyContent: 'space-between', padding: '1rem' }}>
        <h1>Каталог</h1>
        <button onClick={logout}>Выйти</button>
      </header>

      <button onClick={() => setIsAdding(true)}>Добавить товар</button>

      {isAdding && <ProductForm onSubmit={handleAdd} onCancel={() => setIsAdding(false)} />}
      {editingProduct && (
        <ProductForm 
          initialData={editingProduct} 
          onSubmit={handleUpdate} 
          onCancel={() => setEditingProduct(null)} 
        />
      )}

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '20px', marginTop: '20px' }}>
        {state.products.map((product) => (
          <ProductCard
            key={product.id}
            product={product}
            onEdit={setEditingProduct}
            onDelete={handleDelete}
          />
        ))}
      </div>
    </div>
  );
};