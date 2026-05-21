import { atom, selector, selectorFamily } from 'recoil'
import type { Product } from '../lib/products'
import { productsQueryKey } from '../lib/products'
import { appQueryClient } from '../lib/queryClient'
import { localStorageEffect } from './localStorageEffect'

export interface CartItem {
  id: number
  quantity: number
}

export interface CartProduct {
  product: Product
  quantity: number
}

export const productsVersionState = atom<number>({
  key: 'productsVersionState',
  default: 0,
})

export const favoritesState = atom<number[]>({
  key: 'favoritesState',
  default: [],
  effects: [localStorageEffect<number[]>('lab9_favorites')],
})

export const cartState = atom<CartItem[]>({
  key: 'cartState',
  default: [],
  effects: [localStorageEffect<CartItem[]>('lab9_cart')],
})

export const cartCountState = selector<number>({
  key: 'cartCountState',
  get: ({ get }) => {
    const cart = get(cartState)
    return cart.reduce((total, item) => total + item.quantity, 0)
  },
})

export const favoritesCountState = selector<number>({
  key: 'favoritesCountState',
  get: ({ get }) => {
    return get(favoritesState).length
  },
})

export const isFavoriteByIdState = selectorFamily<boolean, number>({
  key: 'isFavoriteByIdState',
  get:
    (productId) =>
    ({ get }) => {
      return get(favoritesState).includes(productId)
    },
})

export const cartQuantityByIdState = selectorFamily<number, number>({
  key: 'cartQuantityByIdState',
  get:
    (productId) =>
    ({ get }) => {
      const item = get(cartState).find((entry) => entry.id === productId)
      return item?.quantity ?? 0
    },
})

export const cartProductsSelector = selector<CartProduct[]>({
  key: 'cartProductsSelector',
  get: ({ get }) => {
    get(productsVersionState)
    const cart = get(cartState)
    const products = appQueryClient.getQueryData<Product[]>(productsQueryKey) ?? []
    const byId = new Map(products.map((product) => [product.id, product]))

    return cart
      .map((item) => {
        const product = byId.get(item.id)
        if (!product) {
          return null
        }

        return { product, quantity: item.quantity }
      })
      .filter((entry): entry is CartProduct => entry !== null)
  },
})
