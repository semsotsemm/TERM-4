import React, { useState } from "react";
import type { Product } from "../schemas/productSchema";

interface ProductFormProps {
  initial: Product | null;
  onSave: (data: {
    id: string | number;
    title: string;
    price: string | number;
    description?: string;
    category?: string;
  }) => void;
  onClose: () => void;
  isLoading: boolean;
}

export function ProductForm({ initial, onSave, onClose, isLoading }: ProductFormProps) {
  const [fields, setFields] = useState({
    id: String(initial?.id ?? ""),
    title: initial?.title ?? "",
    price: String(initial?.price ?? ""),
    description: initial?.description ?? "",
    category: initial?.category ?? "",
  });

  function handleChange(
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const { name, value } = e.target;
    setFields((prev) => ({ ...prev, [name]: value }));
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    onSave(fields);
  }

  const isEdit = initial !== null;

  return (
    <div
      className="modal-overlay"
      onClick={(e) => {
        if (e.target === e.currentTarget) onClose();
      }}
    >
      <div className="modal-box">
        <div className="modal-head">
          <h2 className="modal-title">
            {isEdit ? "Редактировать товар" : "Новый товар"}
          </h2>
          <button className="modal-close" onClick={onClose} type="button">
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className="modal-form">
          {/* ID — только при создании */}
          {!isEdit && (
            <div className="form-group">
              <label className="form-label" htmlFor="pf-id">
                ID товара
              </label>
              <input
                id="pf-id"
                name="id"
                type="number"
                min="1"
                placeholder="101"
                value={fields.id}
                onChange={handleChange}
                className="form-input"
                required
              />
            </div>
          )}

          {/* Название */}
          <div className="form-group">
            <label className="form-label" htmlFor="pf-title">
              Название
            </label>
            <input
              id="pf-title"
              name="title"
              type="text"
              placeholder="Название товара"
              value={fields.title}
              onChange={handleChange}
              className="form-input"
              required
              minLength={3}
            />
          </div>

          {/* Цена */}
          <div className="form-group">
            <label className="form-label" htmlFor="pf-price">
              Цена ($)
            </label>
            <input
              id="pf-price"
              name="price"
              type="number"
              min="0.01"
              step="0.01"
              placeholder="9.99"
              value={fields.price}
              onChange={handleChange}
              className="form-input"
              required
            />
          </div>

          {/* Категория */}
          <div className="form-group">
            <label className="form-label" htmlFor="pf-category">
              Категория
            </label>
            <input
              id="pf-category"
              name="category"
              type="text"
              placeholder="electronics"
              value={fields.category}
              onChange={handleChange}
              className="form-input"
            />
          </div>

          {/* Описание */}
          <div className="form-group">
            <label className="form-label" htmlFor="pf-description">
              Описание
            </label>
            <textarea
              id="pf-description"
              name="description"
              placeholder="Краткое описание товара"
              value={fields.description}
              onChange={handleChange}
              className="form-textarea"
              rows={3}
            />
          </div>

          {/* Кнопки */}
          <div className="modal-actions">
            <button
              type="button"
              className="btn btn-ghost"
              onClick={onClose}
              disabled={isLoading}
            >
              Отмена
            </button>
            <button
              type="submit"
              className="btn btn-lime"
              disabled={isLoading}
            >
              {isLoading ? (
                <>
                  <span className="spinner" /> Сохранение...
                </>
              ) : isEdit ? (
                "Сохранить изменения"
              ) : (
                "Создать товар"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
