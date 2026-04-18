import React from 'react';
import type { Product } from '../schemas/productSchema';

interface ProductCardProps {
  product: Product;
  onEdit: (product: Product) => void;
  onDelete: (id: number) => void;
}

export const ProductCard: React.FC<ProductCardProps> = ({ product, onEdit, onDelete }) => {
  return (
    <div className="product-card">
      <div className="product-info">
        <h3>{product.title}</h3>
        <p className="price">{product.price} $</p>
      </div>
      <div className="card-actions">
        <button className="btn-edit" onClick={() => onEdit(product)}>Изменить</button>
        <button className="btn-delete" onClick={() => onDelete(product.id)}>Удалить</button>
      </div>
    </div>
  );
};