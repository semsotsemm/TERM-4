// src/components/ProductCard/index.tsx
import React from 'react';
import type { IProduct } from '../../contexts/ProductContext';

interface IProductCardProps {
  product: IProduct;
  onEdit: (product: IProduct) => void;
  onDelete: (id: number) => void;
}

const ProductCard: React.FC<IProductCardProps> = ({ product, onEdit, onDelete }) => {
  return (
    <div style={styles.card}>
      {product.thumbnail && (
        <img
          src={product.thumbnail}
          alt={product.title}
          style={styles.image}
        />
      )}
      <div style={styles.body}>
        <h3 style={styles.title}>{product.title}</h3>
        {product.category && <span style={styles.category}>{product.category}</span>}
        <p style={styles.price}>${product.price.toFixed(2)}</p>
        {product.description && (
          <p style={styles.description}>{product.description}</p>
        )}
      </div>
      <div style={styles.actions}>
        <button style={styles.btnEdit} onClick={() => onEdit(product)}>
          Изменить
        </button>
        <button style={styles.btnDelete} onClick={() => onDelete(product.id)}>
          Удалить
        </button>
      </div>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  card: {
    background: '#fff',
    border: '1px solid #e0e0e0',
    borderRadius: '10px',
    overflow: 'hidden',
    display: 'flex',
    flexDirection: 'column',
    boxShadow: '0 2px 8px rgba(0,0,0,0.06)',
    transition: 'box-shadow 0.2s',
  },
  image: {
    width: '100%',
    height: '180px',
    objectFit: 'cover',
  },
  body: {
    padding: '12px 14px',
    flex: 1,
  },
  title: {
    margin: '0 0 6px',
    fontSize: '15px',
    fontWeight: 600,
    color: '#1a1a1a',
    lineHeight: 1.3,
  },
  category: {
    display: 'inline-block',
    padding: '2px 8px',
    background: '#e3f2fd',
    color: '#1565c0',
    borderRadius: '12px',
    fontSize: '11px',
    marginBottom: '6px',
  },
  price: {
    margin: '6px 0',
    fontWeight: 700,
    color: '#2e7d32',
    fontSize: '16px',
  },
  description: {
    margin: '0',
    fontSize: '12px',
    color: '#666',
    overflow: 'hidden',
    display: '-webkit-box',
    WebkitLineClamp: 2,
    WebkitBoxOrient: 'vertical' as const,
  },
  actions: {
    display: 'flex',
    gap: '8px',
    padding: '10px 14px',
    borderTop: '1px solid #f0f0f0',
  },
  btnEdit: {
    flex: 1,
    padding: '7px',
    background: '#1976d2',
    color: '#fff',
    border: 'none',
    borderRadius: '6px',
    cursor: 'pointer',
    fontSize: '13px',
  },
  btnDelete: {
    flex: 1,
    padding: '7px',
    background: '#e53935',
    color: '#fff',
    border: 'none',
    borderRadius: '6px',
    cursor: 'pointer',
    fontSize: '13px',
  },
};

export default ProductCard;
