import { useMemo, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate } from '@tanstack/react-router'
import { useAuth } from '../../contexts/AuthContext'
import {
  createProduct,
  deleteProduct,
  fetchProducts,
  getProductQueryKey,
  HttpError,
  productsQueryKey,
  toUiErrorMessage,
  updateProduct,
  type CreateProductPayload,
  type Product,
  type UpdateProductPayload,
} from '../../lib/products'
import { catalogRoute } from '../../routes/catalog'
import ProductModal from './ProductModal'

interface ModalState {
  mode: 'create' | 'edit'
  product?: Product
}

const CatalogPage = () => {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const search = catalogRoute.useSearch()
  const [modalState, setModalState] = useState<ModalState | null>(null)

  const reserveLocalId = (existingProducts: Product[]) => {
    const existingIds = new Set(existingProducts.map((product) => product.id))
    let candidate = Math.floor(Date.now() / 1000)
    while (existingIds.has(candidate)) {
      candidate += 1
    }
    return candidate
  }

  const { data: products = [], isLoading, isError, error } = useQuery({
    queryKey: productsQueryKey,
    queryFn: fetchProducts,
    staleTime: 60_000,
    gcTime: 300_000,
  })

  const createMutation = useMutation({
    mutationFn: (payload: CreateProductPayload) => createProduct(payload),
    onSuccess: (createdProduct, payload) => {
      queryClient.setQueryData<Product[]>(productsQueryKey, (current = []) => {
        const localId =
          current.some((product) => product.id === createdProduct.id) || createdProduct.id <= 194
            ? reserveLocalId(current)
            : createdProduct.id

        const normalizedProduct: Product = {
          id: localId,
          title: createdProduct.title,
          price: createdProduct.price,
          category: createdProduct.category ?? payload.category,
          thumbnail: createdProduct.thumbnail ?? payload.thumbnail,
        }

        const hasSameId = current.some((product) => product.id === normalizedProduct.id)
        if (hasSameId) {
          return current.map((product) => (product.id === normalizedProduct.id ? normalizedProduct : product))
        }

        return [normalizedProduct, ...current]
      })
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: productsQueryKey, refetchType: 'none' })
    },
  })

  const updateMutation = useMutation({
    mutationFn: async (product: UpdateProductPayload) => {
      try {
        return await updateProduct(product)
      } catch (error) {
        if (error instanceof HttpError && error.status === 404) {
          return {
            id: product.id,
            title: product.title,
            price: product.price,
            category: product.category,
            thumbnail: product.thumbnail,
          }
        }

        throw error
      }
    },
    onSuccess: (updatedProduct) => {
      queryClient.setQueryData<Product[]>(productsQueryKey, (current = []) => {
        const hasSameId = current.some((product) => product.id === updatedProduct.id)
        if (!hasSameId) {
          return [updatedProduct, ...current]
        }

        return current.map((product) => (product.id === updatedProduct.id ? updatedProduct : product))
      })
      queryClient.setQueryData(getProductQueryKey(String(updatedProduct.id)), updatedProduct)
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: productsQueryKey, refetchType: 'none' })
    },
  })

  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      try {
        return await deleteProduct(id)
      } catch (error) {
        if (error instanceof HttpError && error.status === 404) {
          return { id, isDeleted: true }
        }

        throw error
      }
    },
    onMutate: async (id: number) => {
      await queryClient.cancelQueries({ queryKey: productsQueryKey })
      const previousProducts = queryClient.getQueryData<Product[]>(productsQueryKey) ?? []

      queryClient.setQueryData<Product[]>(productsQueryKey, (current = []) =>
        current.filter((product) => product.id !== id),
      )

      return { previousProducts }
    },
    onError: (_error, _id, context) => {
      if (!context) {
        return
      }

      queryClient.setQueryData(productsQueryKey, context.previousProducts)
    },
    onSuccess: (_result, id) => {
      queryClient.removeQueries({ queryKey: getProductQueryKey(String(id)), exact: true })
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: productsQueryKey, refetchType: 'none' })
    },
  })

  const category = search.category ?? ''

  const visibleProducts = useMemo(() => {
    const query = category.trim().toLowerCase()
    if (!query) {
      return products
    }

    return products.filter((product) => (product.category ?? '').toLowerCase().includes(query))
  }, [products, category])

  const initials = useMemo(() => {
    if (!user) {
      return '??'
    }

    return `${user.firstName[0] ?? ''}${user.lastName[0] ?? ''}`.toUpperCase()
  }, [user])

  const updateCategorySearch = (value: string) => {
    const nextValue = value.trim()

    void navigate({
      to: '/catalog',
      search: (previous) => ({
        ...previous,
        category: nextValue.length > 0 ? nextValue : undefined,
      }),
      replace: true,
    })
  }

  const handleCreate = async (payload: CreateProductPayload) => {
    try {
      await createMutation.mutateAsync(payload)
      return { ok: true }
    } catch (mutationError) {
      return {
        ok: false,
        message: toUiErrorMessage(mutationError, 'Ошибка при добавлении товара'),
      }
    }
  }

  const handleUpdate = async (payload: UpdateProductPayload) => {
    try {
      await updateMutation.mutateAsync(payload)
      return { ok: true }
    } catch (mutationError) {
      return {
        ok: false,
        message: toUiErrorMessage(mutationError, 'Ошибка при обновлении товара'),
      }
    }
  }

  const handleDelete = async (id: number) => {
    const approved = window.confirm('Удалить товар?')
    if (!approved) {
      return
    }

    try {
      await deleteMutation.mutateAsync(id)
    } catch (mutationError) {
      alert(toUiErrorMessage(mutationError, 'Ошибка при удалении товара'))
    }
  }

  const handleLogout = () => {
    logout()
    void navigate({ to: '/login' })
  }

  return (
    <div className="catalog-page">
      <header className="header">
        <Link to="/" className="brand">
          <div className="brand-mark">
            <svg viewBox="0 0 20 20" fill="none">
              <rect x="2" y="5" width="16" height="11" rx="1.5" stroke="white" strokeWidth="1.5" />
              <path
                d="M7 5V4a3 3 0 016 0v1"
                stroke="white"
                strokeWidth="1.5"
                strokeLinecap="round"
              />
              <circle cx="10" cy="10.5" r="1.5" fill="white" />
            </svg>
          </div>
          <span className="brand-name">ShopLab</span>
        </Link>

        <div className="header-search">
          <span className="search-icon">
            <svg viewBox="0 0 16 16" fill="none">
              <circle cx="7" cy="7" r="4.5" stroke="currentColor" strokeWidth="1.5" />
              <path d="M10.5 10.5L14 14" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
            </svg>
          </span>
          <input
            type="search"
            placeholder="Фильтр по категории (например: smartphones)"
            value={category}
            onChange={(event) => updateCategorySearch(event.target.value)}
          />
        </div>

        <div className="header-right">
          <div className="user-pill">
            <div className="user-avatar">{initials}</div>
            {user ? `${user.firstName} ${user.lastName}` : 'Гость'}
          </div>

          <button className="btn-logout" onClick={handleLogout} type="button">
            <svg viewBox="0 0 16 16" fill="none">
              <path
                d="M6 2H3a1 1 0 00-1 1v10a1 1 0 001 1h3M10 11l3-3-3-3M6 8h7"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
            Выйти
          </button>

          <button className="btn-add" onClick={() => setModalState({ mode: 'create' })} type="button">
            <svg viewBox="0 0 16 16" fill="none">
              <path d="M8 3v10M3 8h10" stroke="white" strokeWidth="2" strokeLinecap="round" />
            </svg>
            Добавить товар
          </button>
        </div>
      </header>

      <main className="main">
        <div className="page-header">
          <h1 className="page-title">Каталог</h1>
          <span className="product-count">{isLoading ? 'Загрузка...' : `${visibleProducts.length} товаров`}</span>
        </div>

        {isError && <div className="error-banner">{toUiErrorMessage(error, 'Ошибка загрузки каталога')}</div>}

        {isLoading ? (
          <div className="skeleton-grid">
            {Array.from({ length: 6 }).map((_, index) => (
              <div className="skeleton-card" key={`skeleton-${index}`}>
                <div className="skeleton-img"></div>
                <div className="skeleton-body">
                  <div className="skeleton-line"></div>
                  <div className="skeleton-line short"></div>
                  <div className="skeleton-line xs"></div>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="product-grid">
            {visibleProducts.map((product, index) => (
              <div className="product-card" key={product.id}>
                {index === 0 && <span className="card-badge new">Новинка</span>}
                <div className="card-img-wrap">
                  <img
                    src={product.thumbnail ?? 'https://dummyjson.com/image/300x200/1a1714/f7f4ef?text=Product'}
                    alt={product.title}
                    loading="lazy"
                  />
                </div>
                <div className="card-body">
                  <span className="card-category">{product.category ?? 'Без категории'}</span>
                  <h3 className="card-title">
                    <Link to="/product/$id" params={{ id: String(product.id) }}>
                      {product.title}
                    </Link>
                  </h3>
                  <div className="card-price-row">
                    <span className="card-price">${product.price}</span>
                  </div>
                </div>
                <div className="card-actions">
                  <button
                    className="btn-edit"
                    onClick={() => setModalState({ mode: 'edit', product })}
                    type="button"
                  >
                    Изменить
                  </button>
                  <button
                    className="btn-delete"
                    onClick={() => void handleDelete(product.id)}
                    type="button"
                    disabled={deleteMutation.isPending}
                  >
                    Удалить
                  </button>
                </div>
              </div>
            ))}

            {visibleProducts.length === 0 && (
              <div className="empty-state">
                <div className="empty-icon">
                  <svg viewBox="0 0 32 32" fill="none">
                    <rect x="4" y="8" width="24" height="19" rx="2" stroke="currentColor" strokeWidth="1.5" />
                    <path
                      d="M11 8V7a5 5 0 0110 0v1"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                    />
                  </svg>
                </div>
                <h3>Товары не найдены</h3>
                <p>Попробуйте изменить category в URL или добавить новый товар</p>
              </div>
            )}
          </div>
        )}
      </main>

      {modalState && (
        <ProductModal
          mode={modalState.mode}
          initialProduct={modalState.product}
          onClose={() => setModalState(null)}
          onCreate={handleCreate}
          onUpdate={handleUpdate}
        />
      )}
    </div>
  )
}

export default CatalogPage
