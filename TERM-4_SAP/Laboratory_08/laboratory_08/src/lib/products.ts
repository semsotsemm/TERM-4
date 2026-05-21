import { z } from 'zod'
import { ProductSchema, type Product } from '../types/product'

const PRODUCTS_URL = 'https://dummyjson.com/products'

const ProductsResponseSchema = z.object({
  products: z.array(ProductSchema),
})

const CreateProductPayloadSchema = z.object({
  title: z.string().min(3, 'Название слишком короткое'),
  price: z.number().gt(0, 'Цена должна быть больше 0'),
  category: z.string().optional(),
  thumbnail: z.string().url('Некорректный URL изображения').optional(),
})

export type CreateProductPayload = z.infer<typeof CreateProductPayloadSchema>

const UpdateProductPayloadSchema = z.object({
  id: z.number(),
  title: z.string().min(3, 'Название слишком короткое'),
  price: z.number().gt(0, 'Цена должна быть больше 0'),
  category: z.string().optional(),
  thumbnail: z.string().url('Некорректный URL изображения').optional(),
})

export type UpdateProductPayload = z.infer<typeof UpdateProductPayloadSchema>

export class DataShapeError extends Error {
  constructor() {
    super('Ошибка структуры данных')
    this.name = 'DataShapeError'
  }
}

export class HttpError extends Error {
  status: number

  constructor(status: number, message: string) {
    super(message)
    this.name = 'HttpError'
    this.status = status
  }
}

const parseWithSchema = <T,>(schema: z.ZodType<T>, data: unknown): T => {
  try {
    return schema.parse(data)
  } catch {
    throw new DataShapeError()
  }
}

const readErrorMessage = async (response: Response, fallback: string) => {
  try {
    const data = (await response.json()) as unknown
    if (
      typeof data === 'object' &&
      data !== null &&
      'message' in data &&
      typeof (data as { message?: unknown }).message === 'string'
    ) {
      return (data as { message: string }).message
    }
  } catch {
    return fallback
  }

  return fallback
}

export const toUiErrorMessage = (error: unknown, fallback: string) => {
  if (error instanceof DataShapeError) {
    return error.message
  }

  if (error instanceof Error) {
    return error.message
  }

  return fallback
}

export const fetchProducts = async () => {
  const response = await fetch(PRODUCTS_URL)
  if (!response.ok) {
    throw new Error('Не удалось загрузить товары')
  }

  const data = (await response.json()) as unknown
  const parsed = parseWithSchema(ProductsResponseSchema, data)
  return parsed.products.map((product) => parseWithSchema(ProductSchema, product))
}

export const fetchProductById = async (id: string) => {
  const response = await fetch(`${PRODUCTS_URL}/${id}`)
  if (!response.ok) {
    throw new Error('Не удалось загрузить товар')
  }

  const data = (await response.json()) as unknown
  return parseWithSchema(ProductSchema, data)
}

export const createProduct = async (payload: CreateProductPayload) => {
  const body = parseWithSchema(CreateProductPayloadSchema, payload)
  const response = await fetch(`${PRODUCTS_URL}/add`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    const message = await readErrorMessage(response, 'Не удалось добавить товар')
    throw new HttpError(response.status, message)
  }

  const data = (await response.json()) as unknown
  return parseWithSchema(ProductSchema, data)
}

export const updateProduct = async (payload: UpdateProductPayload) => {
  const parsed = parseWithSchema(UpdateProductPayloadSchema, payload)
  const body = parseWithSchema(CreateProductPayloadSchema, {
    title: parsed.title,
    price: parsed.price,
    category: parsed.category,
    thumbnail: parsed.thumbnail,
  })
  const response = await fetch(`${PRODUCTS_URL}/${parsed.id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    const message = await readErrorMessage(response, 'Не удалось обновить товар')
    throw new HttpError(response.status, message)
  }

  const data = (await response.json()) as unknown
  const updated = parseWithSchema(ProductSchema, data)
  return { ...updated, id: parsed.id }
}

export const deleteProduct = async (id: number) => {
  const response = await fetch(`${PRODUCTS_URL}/${id}`, {
    method: 'DELETE',
  })

  if (!response.ok) {
    const message = await readErrorMessage(response, 'Не удалось удалить товар')
    throw new HttpError(response.status, message)
  }

  return (await response.json()) as unknown
}

export const productsQueryKey = ['products'] as const
export const getProductQueryKey = (id: string) => ['product', id] as const

export type { Product }
