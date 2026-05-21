import { z } from 'zod'

export const ProductSchema = z.object({
  id: z.number(),
  title: z.string().min(3, 'Название слишком короткое'),
  price: z.number().gt(0, 'Цена должна быть больше 0'),
  category: z.string().optional(),
  description: z.string().optional(),
  thumbnail: z.string().url('Некорректный URL изображения').optional(),
})

export type Product = z.infer<typeof ProductSchema>
