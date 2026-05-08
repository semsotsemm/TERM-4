// src/contexts/ProductContext.tsx
import React, { createContext, useContext, useReducer, useEffect } from 'react';
import { z } from 'zod';

// ──────────────────────────────────────────────
// Zod schema & type
// ──────────────────────────────────────────────
export const ProductSchema = z.object({
  id: z.number(),
  title: z.string().min(3, 'title: Название слишком короткое (минимум 3 символа)'),
  price: z.number().positive('price: Цена должна быть больше 0'),
  thumbnail: z.string().optional(),
  description: z.string().optional(),
  category: z.string().optional(),
});

export type IProduct = z.infer<typeof ProductSchema>;

// ──────────────────────────────────────────────
// Reducer
// ──────────────────────────────────────────────
interface IProductState {
  products: IProduct[];
  loading: boolean;
  error: string | null;
}

type TProductAction =
  | { type: 'FETCH_START' }
  | { type: 'FETCH_SUCCESS'; payload: IProduct[] }
  | { type: 'FETCH_ERROR'; payload: string }
  | { type: 'ADD_PRODUCT'; payload: IProduct }
  | { type: 'UPDATE_PRODUCT'; payload: IProduct }
  | { type: 'DELETE_PRODUCT'; payload: number };

const initialState: IProductState = {
  products: [],
  loading: false,
  error: null,
};

function productReducer(state: IProductState, action: TProductAction): IProductState {
  switch (action.type) {
    case 'FETCH_START':
      return { ...state, loading: true, error: null };
    case 'FETCH_SUCCESS':
      return { ...state, loading: false, products: action.payload };
    case 'FETCH_ERROR':
      return { ...state, loading: false, error: action.payload };
    case 'ADD_PRODUCT':
      return { ...state, products: [action.payload, ...state.products] };
    case 'UPDATE_PRODUCT':
      return {
        ...state,
        products: state.products.map((p) =>
          p.id === action.payload.id ? action.payload : p
        ),
      };
    case 'DELETE_PRODUCT':
      return {
        ...state,
        products: state.products.filter((p) => p.id !== action.payload),
      };
    default:
      return state;
  }
}

// ──────────────────────────────────────────────
// Context
// ──────────────────────────────────────────────
const API_URL = 'https://dummyjson.com/products';

interface IProductContext extends IProductState {
  addProduct: (data: Omit<IProduct, 'id'>) => Promise<void>;
  updateProduct: (data: IProduct) => Promise<void>;
  deleteProduct: (id: number) => Promise<void>;
}

const ProductContext = createContext<IProductContext | null>(null);

export const ProductProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [state, dispatch] = useReducer(productReducer, initialState);

  // READ — GET при первом рендере
  useEffect(() => {
    dispatch({ type: 'FETCH_START' });
    fetch(`${API_URL}?limit=20`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json();
      })
      .then((data: { products: unknown[] }) => {
        const parsed = data.products
          .map((item) => ProductSchema.safeParse(item))
          .filter((r) => r.success)
          .map((r) => (r as { success: true; data: IProduct }).data);
        dispatch({ type: 'FETCH_SUCCESS', payload: parsed });
      })
      .catch((err: Error) => {
        dispatch({ type: 'FETCH_ERROR', payload: err.message });
      });
  }, []);

  // CREATE — POST
  const addProduct = async (data: Omit<IProduct, 'id'>) => {
    const res = await fetch(API_URL + '/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    const json: unknown = await res.json();
    const result = ProductSchema.safeParse(json);
    if (result.success) {
      dispatch({ type: 'ADD_PRODUCT', payload: result.data });
    } else {
      // Fallback: генерируем временный id, чтобы не потерять данные
      const newProduct: IProduct = { id: Date.now(), ...data };
      dispatch({ type: 'ADD_PRODUCT', payload: newProduct });
    }
  };

  // UPDATE — PUT
  const updateProduct = async (data: IProduct) => {
    const res = await fetch(`${API_URL}/${data.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    dispatch({ type: 'UPDATE_PRODUCT', payload: data });
  };

  // DELETE
  const deleteProduct = async (id: number) => {
    const res = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    dispatch({ type: 'DELETE_PRODUCT', payload: id });
  };

  return (
    <ProductContext.Provider value={{ ...state, addProduct, updateProduct, deleteProduct }}>
      {children}
    </ProductContext.Provider>
  );
};

export const useProducts = (): IProductContext => {
  const ctx = useContext(ProductContext);
  if (!ctx) throw new Error('useProducts must be used within ProductProvider');
  return ctx;
};
