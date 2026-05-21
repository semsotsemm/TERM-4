import { useQuery } from '@tanstack/react-query'
import { Link, createRoute } from '@tanstack/react-router'
import { fetchProductById, getProductQueryKey, toUiErrorMessage } from '../lib/products'
import { rootRoute } from './__root'

const ProductDetailsPage = () => {
  const params = productRoute.useParams()

  const { data, isLoading, isError, error } = useQuery({
    queryKey: getProductQueryKey(params.id),
    queryFn: () => fetchProductById(params.id),
    staleTime: 60_000,
    gcTime: 300_000,
  })

  if (isLoading) {
    return <main className="landing-page">Загрузка товара...</main>
  }

  if (isError) {
    return (
      <main className="landing-page">
        <div className="landing-card">
          <h1>Ошибка</h1>
          <p>{toUiErrorMessage(error, 'Не удалось загрузить товар')}</p>
          <Link to="/catalog" className="landing-btn secondary">
            Назад в каталог
          </Link>
        </div>
      </main>
    )
  }

  if (!data) {
    return (
      <main className="landing-page">
        <div className="landing-card">
          <h1>Товар не найден</h1>
          <Link to="/catalog" className="landing-btn secondary">
            Назад в каталог
          </Link>
        </div>
      </main>
    )
  }

  return (
    <main className="landing-page">
      <div className="landing-card">
        <h1>{data.title}</h1>
        <p>Категория: {data.category ?? 'Без категории'}</p>
        <p>Цена: ${data.price}</p>
        <Link to="/catalog" className="landing-btn secondary">
          Назад в каталог
        </Link>
      </div>
    </main>
  )
}

export const productRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/product/$id',
  component: ProductDetailsPage,
})
