import {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useReducer,
  useState,
  type ReactNode,
} from 'react'
import { ProductSchema, type Product } from '../types/product'

const PRODUCTS_URL = 'https://dummyjson.com/products'

interface ProductState {
  products: Product[]
}

type ProductAction =
  | { type: 'SET_PRODUCTS'; payload: Product[] }
  | { type: 'ADD_PRODUCT'; payload: Product }
  | { type: 'UPDATE_PRODUCT'; payload: Product }
  | { type: 'DELETE_PRODUCT'; payload: number }

interface ProductContextValue {
  products: Product[]
  loading: boolean
  error: string | null
  createProduct: (payload: { title: string; price: number; category?: string; thumbnail?: string }) => Promise<{
    ok: boolean
    message?: string
  }>
  updateProduct: (
    payload: Product,
  ) => Promise<{
    ok: boolean
    message?: string
  }>
  deleteProduct: (id: number) => Promise<{ ok: boolean; message?: string }>
}

const initialState: ProductState = {
  products: [],
}

const productReducer = (state: ProductState, action: ProductAction): ProductState => {
  switch (action.type) {
    case 'SET_PRODUCTS':
      return { ...state, products: action.payload }
    case 'ADD_PRODUCT':
      return { ...state, products: [action.payload, ...state.products] }
    case 'UPDATE_PRODUCT':
      return {
        ...state,
        products: state.products.map((product) =>
          product.id === action.payload.id ? action.payload : product,
        ),
      }
    case 'DELETE_PRODUCT':
      return {
        ...state,
        products: state.products.filter((product) => product.id !== action.payload),
      }
    default:
      return state
  }
}

const ProductContext = createContext<ProductContextValue | undefined>(undefined)

const mapZodField = (field: string) => {
  if (field === 'title') {
    return 'title'
  }

  if (field === 'price') {
    return 'price'
  }

  if (field === 'id') {
    return 'id'
  }

  return field
}

const toErrorMessage = (error: unknown, fallback: string) => {
  if (error instanceof Error) {
    return error.message
  }

  return fallback
}

export const ProductProvider = ({ children }: { children: ReactNode }) => {
  const [state, dispatch] = useReducer(productReducer, initialState)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const loadProducts = async () => {
      setLoading(true)
      setError(null)

      try {
        const response = await fetch(PRODUCTS_URL)
        if (!response.ok) {
          throw new Error('Не удалось загрузить товары')
        }

        const data = (await response.json()) as { products: unknown[] }
        const parsedProducts = data.products
          .map((rawProduct) => ProductSchema.safeParse(rawProduct))
          .filter((result) => result.success)
          .map((result) => result.data)

        dispatch({ type: 'SET_PRODUCTS', payload: parsedProducts })
      } catch (fetchError) {
        setError(toErrorMessage(fetchError, 'Ошибка загрузки каталога'))
      } finally {
        setLoading(false)
      }
    }

    void loadProducts()
  }, [])

  const createProduct: ProductContextValue['createProduct'] = async (payload) => {
    const candidate = {
      id: Date.now(),
      title: payload.title,
      price: payload.price,
      category: payload.category,
      thumbnail: payload.thumbnail,
    }

    const validationResult = ProductSchema.safeParse(candidate)
    if (!validationResult.success) {
      const flattenedErrors = validationResult.error.flatten()
      const fieldEntries = Object.entries(flattenedErrors.fieldErrors).filter(
        ([, messages]) => Array.isArray(messages) && messages.length > 0,
      )
      const prettyError = fieldEntries
        .map(([field, messages]) => `${mapZodField(field)}: ${messages[0]}`)
        .join('\n')

      return {
        ok: false,
        message: prettyError || 'Ошибка валидации данных товара',
      }
    }

    try {
      const response = await fetch(`${PRODUCTS_URL}/add`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        throw new Error('Не удалось добавить товар')
      }

      dispatch({ type: 'ADD_PRODUCT', payload: validationResult.data })
      return { ok: true }
    } catch (createError) {
      return { ok: false, message: toErrorMessage(createError, 'Ошибка при добавлении товара') }
    }
  }

  const updateProduct: ProductContextValue['updateProduct'] = async (payload) => {
    const validationResult = ProductSchema.safeParse(payload)
    if (!validationResult.success) {
      const flattenedErrors = validationResult.error.flatten()
      const fieldEntries = Object.entries(flattenedErrors.fieldErrors).filter(
        ([, messages]) => Array.isArray(messages) && messages.length > 0,
      )
      const prettyError = fieldEntries
        .map(([field, messages]) => `${mapZodField(field)}: ${messages[0]}`)
        .join('\n')

      return {
        ok: false,
        message: prettyError || 'Ошибка валидации данных товара',
      }
    }

    try {
      const existsInLocalState = state.products.some((product) => product.id === payload.id)
      const response = await fetch(`${PRODUCTS_URL}/${payload.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        if (response.status === 404 && existsInLocalState) {
          dispatch({ type: 'UPDATE_PRODUCT', payload: validationResult.data })
          return { ok: true }
        }

        throw new Error('Не удалось обновить товар')
      }

      dispatch({ type: 'UPDATE_PRODUCT', payload: validationResult.data })
      return { ok: true }
    } catch (updateError) {
      return { ok: false, message: toErrorMessage(updateError, 'Ошибка при обновлении товара') }
    }
  }

  const deleteProduct: ProductContextValue['deleteProduct'] = async (id) => {
    try {
      const existsInLocalState = state.products.some((product) => product.id === id)
      const response = await fetch(`${PRODUCTS_URL}/${id}`, {
        method: 'DELETE',
      })

      if (!response.ok) {
        if (response.status === 404 && existsInLocalState) {
          dispatch({ type: 'DELETE_PRODUCT', payload: id })
          return { ok: true }
        }

        throw new Error('Не удалось удалить товар')
      }

      dispatch({ type: 'DELETE_PRODUCT', payload: id })
      return { ok: true }
    } catch (deleteError) {
      return { ok: false, message: toErrorMessage(deleteError, 'Ошибка при удалении товара') }
    }
  }

  const value = useMemo<ProductContextValue>(
    () => ({
      products: state.products,
      loading,
      error,
      createProduct,
      updateProduct,
      deleteProduct,
    }),
    [state.products, loading, error],
  )

  return <ProductContext.Provider value={value}>{children}</ProductContext.Provider>
}

export const useProducts = () => {
  const context = useContext(ProductContext)
  if (!context) {
    throw new Error('useProducts must be used inside ProductProvider')
  }

  return context
}
