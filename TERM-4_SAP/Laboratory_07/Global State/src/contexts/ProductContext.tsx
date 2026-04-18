import React, { createContext, useReducer, useEffect } from 'react';
import type { Product } from '../schemas/productSchema';

type State = { products: Product[] };
type Action = 
  | { type: 'SET_PRODUCTS'; payload: Product[] }
  | { type: 'ADD_PRODUCT'; payload: Product }
  | { type: 'UPDATE_PRODUCT'; payload: Product }
  | { type: 'DELETE_PRODUCT'; payload: number };

const ProductContext = createContext<{ state: State; dispatch: React.Dispatch<Action> } | undefined>(undefined);

interface ProductsResponse {
  products: Product[];
}

function productReducer(state: State, action: Action): State {
  switch (action.type) {
    case 'SET_PRODUCTS': return { products: action.payload };
    case 'ADD_PRODUCT': return { products: [action.payload, ...state.products] };
    case 'UPDATE_PRODUCT': return {
      products: state.products.map(p => p.id === action.payload.id ? action.payload : p)
    };
    case 'DELETE_PRODUCT': return {
      products: state.products.filter(p => p.id !== action.payload)
    };
    default: return state;
  }
}

export const ProductProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [state, dispatch] = useReducer(productReducer, { products: [] });

  useEffect(() => {
    fetch('https://dummyjson.com/products?limit=10')
      .then((res) => {
        if (!res.ok) {
          throw new Error('Не удалось загрузить товары');
        }
        return res.json() as Promise<ProductsResponse>;
      })
      .then((data) => dispatch({ type: 'SET_PRODUCTS', payload: data.products }))
      .catch(() => {
        dispatch({ type: 'SET_PRODUCTS', payload: [] });
      });
  }, []);

  return (
    <ProductContext.Provider value={{ state, dispatch }}>
      {children}
    </ProductContext.Provider>
  );
};

export { ProductContext };