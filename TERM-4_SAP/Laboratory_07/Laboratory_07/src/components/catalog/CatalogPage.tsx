import { useMemo, useState } from 'react'
import { useAuth } from '../../contexts/AuthContext'
import { useProducts } from '../../contexts/ProductContext'
import type { Product } from '../../types/product'
import ProductModal from './ProductModal'

interface ModalState {
  mode: 'create' | 'edit'
  product?: Product
}

const CatalogPage = () => {
  const { user, logout } = useAuth()
  const { products, loading, error, createProduct, updateProduct, deleteProduct } = useProducts()
  const [search, setSearch] = useState('')
  const [modalState, setModalState] = useState<ModalState | null>(null)

  const visibleProducts = useMemo(() => {
    const query = search.trim().toLowerCase()
    if (!query) {
      return products
    }

    return products.filter((product) => {
      const titleMatch = product.title.toLowerCase().includes(query)
      const categoryMatch = (product.category ?? '').toLowerCase().includes(query)
      return titleMatch || categoryMatch
    })
  }, [products, search])

  const initials = useMemo(() => {
    if (!user) {
      return '??'
    }

    return `${user.firstName[0] ?? ''}${user.lastName[0] ?? ''}`.toUpperCase()
  }, [user])

  const handleDelete = async (id: number) => {
    const approved = window.confirm('Удалить товар?')
    if (!approved) {
      return
    }

    const result = await deleteProduct(id)
    if (!result.ok) {
      alert(result.message)
    }
  }

  return (
    <div className="catalog-page">
      <header className="header">
        <a href="#" className="brand" onClick={(event) => event.preventDefault()}>
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
        </a>

        <div className="header-search">
          <span className="search-icon">
            <svg viewBox="0 0 16 16" fill="none">
              <circle cx="7" cy="7" r="4.5" stroke="currentColor" strokeWidth="1.5" />
              <path d="M10.5 10.5L14 14" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
            </svg>
          </span>
          <input
            type="search"
            placeholder="Поиск по товарам..."
            value={search}
            onChange={(event) => setSearch(event.target.value)}
          />
        </div>

        <div className="header-right">
          <div className="user-pill">
            <div className="user-avatar">{initials}</div>
            {user ? `${user.firstName} ${user.lastName}` : 'Гость'}
          </div>

          <button className="btn-logout" onClick={logout} type="button">
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
          <span className="product-count">{loading ? 'Загрузка...' : `${visibleProducts.length} товаров`}</span>
        </div>

        {error && <div className="error-banner">{error}</div>}

        {loading ? (
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
                  <h3 className="card-title">{product.title}</h3>
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
                  <button className="btn-delete" onClick={() => void handleDelete(product.id)} type="button">
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
                <p>Попробуйте изменить поиск или добавить новый товар</p>
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
          onCreate={createProduct}
          onUpdate={updateProduct}
        />
      )}
    </div>
  )
}

export default CatalogPage
