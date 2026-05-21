import { useSetRecoilState, useRecoilValue } from 'recoil'
import { favoritesState, isFavoriteByIdState } from '../../state/shopState'

interface FavoriteToggleButtonProps {
  productId: number
}

const FavoriteToggleButton = ({ productId }: FavoriteToggleButtonProps) => {
  const isFavorite = useRecoilValue(isFavoriteByIdState(productId))
  const setFavorites = useSetRecoilState(favoritesState)

  const toggleFavorite = () => {
    setFavorites((current) => {
      if (current.includes(productId)) {
        return current.filter((id) => id !== productId)
      }

      return [...current, productId]
    })
  }

  return (
    <button
      className={isFavorite ? 'btn-favorite active' : 'btn-favorite'}
      type="button"
      onClick={toggleFavorite}
      aria-pressed={isFavorite}
    >
      <svg viewBox="0 0 16 16" fill={isFavorite ? 'currentColor' : 'none'}>
        <path
          d="M8 13.4l-0.6-0.5C4.5 10.4 2.5 8.6 2.5 6.4C2.5 4.7 3.8 3.4 5.5 3.4c1 0 1.9 0.5 2.5 1.2c0.6-0.8 1.5-1.2 2.5-1.2c1.7 0 3 1.3 3 3c0 2.2-2 4-4.9 6.5L8 13.4z"
          stroke="currentColor"
          strokeWidth="1.2"
          strokeLinejoin="round"
        />
      </svg>
      {isFavorite ? 'В избранном' : 'В избранное'}
    </button>
  )
}

export default FavoriteToggleButton
