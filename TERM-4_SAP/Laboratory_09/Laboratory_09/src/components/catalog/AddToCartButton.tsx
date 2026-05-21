import { useRecoilValue, useSetRecoilState } from 'recoil'
import { cartQuantityByIdState, cartState } from '../../state/shopState'

interface AddToCartButtonProps {
  productId: number
}

const AddToCartButton = ({ productId }: AddToCartButtonProps) => {
  const quantity = useRecoilValue(cartQuantityByIdState(productId))
  const setCart = useSetRecoilState(cartState)

  const addToCart = () => {
    setCart((current) => {
      const existingIndex = current.findIndex((item) => item.id === productId)
      if (existingIndex === -1) {
        return [...current, { id: productId, quantity: 1 }]
      }

      return current.map((item) => {
        if (item.id !== productId) {
          return item
        }

        return { ...item, quantity: item.quantity + 1 }
      })
    })
  }

  return (
    <button className="btn-cart" type="button" onClick={addToCart}>
      <svg viewBox="0 0 16 16" fill="none">
        <path
          d="M2 3h1.2c0.3 0 0.5 0.2 0.6 0.5l1.1 5.3c0.1 0.3 0.3 0.5 0.6 0.5h6.4c0.3 0 0.5-0.2 0.6-0.5L13.8 5H5"
          stroke="currentColor"
          strokeWidth="1.2"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
        <circle cx="6.3" cy="12.4" r="0.9" fill="currentColor" />
        <circle cx="11.3" cy="12.4" r="0.9" fill="currentColor" />
      </svg>
      {quantity > 0 ? `В корзине: ${quantity}` : 'В корзину'}
    </button>
  )
}

export default AddToCartButton
