import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import { useRecoilCallback, useRecoilValue, useSetRecoilState } from 'recoil'
import { fetchProducts, productsQueryKey, toUiErrorMessage } from '../../lib/products'
import {
  cartCountState,
  cartProductsSelector,
  cartState,
  productsVersionState,
} from '../../state/shopState'

const CartPage = () => {
  const cartProducts = useRecoilValue(cartProductsSelector)
  const cartCount = useRecoilValue(cartCountState)
  const setCart = useSetRecoilState(cartState)
  const setProductsVersion = useSetRecoilState(productsVersionState)

  const { dataUpdatedAt, isLoading, isError, error } = useQuery({
    queryKey: productsQueryKey,
    queryFn: fetchProducts,
    staleTime: 60_000,
    gcTime: 300_000,
  })

  useEffect(() => {
    setProductsVersion((current) => current + 1)
  }, [dataUpdatedAt, setProductsVersion])

  const clearCart = useRecoilCallback(
    ({ set }) =>
      () => {
        set(cartState, [])
      },
    [],
  )

  const increment = (id: number) => {
    setCart((current) =>
      current.map((item) => {
        if (item.id !== id) {
          return item
        }

        return { ...item, quantity: item.quantity + 1 }
      }),
    )
  }

  const decrement = (id: number) => {
    setCart((current) =>
      current
        .map((item) => {
          if (item.id !== id) {
            return item
          }

          return { ...item, quantity: item.quantity - 1 }
        })
        .filter((item) => item.quantity > 0),
    )
  }

  const remove = (id: number) => {
    setCart((current) => current.filter((item) => item.id !== id))
  }

  const totalPrice = cartProducts.reduce((sum, entry) => sum + entry.product.price * entry.quantity, 0)

  if (isLoading && cartProducts.length === 0) {
    return <main className="cart-page">Загрузка корзины...</main>
  }

  if (isError) {
    return (
      <main className="cart-page">
        <div className="cart-card">
          <h1>Ошибка загрузки каталога</h1>
          <p>{toUiErrorMessage(error, 'Не удалось получить товары для корзины')}</p>
          <Link to="/catalog" className="landing-btn secondary">
            Назад в каталог
          </Link>
        </div>
      </main>
    )
  }

  return (
    <main className="cart-page">
      <div className="cart-card">
        <div className="cart-header-row">
          <h1>Корзина</h1>
          <div className="cart-header-actions">
            <span className="cart-chip">Товаров: {cartCount}</span>
            <button className="cart-clear-btn" type="button" onClick={clearCart}>
              Очистить все
            </button>
          </div>
        </div>

        {cartProducts.length === 0 ? (
          <div className="cart-empty">
            <p>Корзина пуста.</p>
            <Link to="/catalog" className="landing-btn primary">
              Перейти в каталог
            </Link>
          </div>
        ) : (
          <>
            <div className="cart-list">
              {cartProducts.map((entry) => (
                <article className="cart-item" key={entry.product.id}>
                  <img
                    className="cart-thumb"
                    src={entry.product.thumbnail ?? 'https://dummyjson.com/image/120x120/1a1714/f7f4ef?text=Product'}
                    alt={entry.product.title}
                  />
                  <div className="cart-item-body">
                    <h3>{entry.product.title}</h3>
                    <p>{entry.product.category ?? 'Без категории'}</p>
                    <strong>${entry.product.price}</strong>
                  </div>
                  <div className="cart-item-controls">
                    <button type="button" onClick={() => decrement(entry.product.id)}>
                      -
                    </button>
                    <span>{entry.quantity}</span>
                    <button type="button" onClick={() => increment(entry.product.id)}>
                      +
                    </button>
                    <button className="cart-remove-btn" type="button" onClick={() => remove(entry.product.id)}>
                      Remove
                    </button>
                  </div>
                </article>
              ))}
            </div>
            <div className="cart-footer">
              <span>Итого:</span>
              <strong>${totalPrice.toFixed(2)}</strong>
            </div>
          </>
        )}
      </div>
    </main>
  )
}

export default CartPage
