import { z } from 'zod';

export const ProductSchema = z.object({
  id: z.number(),
  title: z.string().min(3, "Название слишком короткое (мин. 3 символа)"),
  price: z.number().gt(0, "Цена должна быть больше 0"),
});

export type Product = z.infer<typeof ProductSchema>;