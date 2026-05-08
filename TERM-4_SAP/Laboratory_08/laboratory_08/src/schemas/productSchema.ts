import { z } from "zod";

// =============================================
// PRODUCT SCHEMA — единственный источник правды
// =============================================
export const ProductSchema = z.object({
  id: z.number(),
  title: z.string().min(3, "Название слишком короткое (минимум 3 символа)"),
  price: z.number().positive("Цена должна быть строго больше 0"),
  description: z.string().optional(),
  category: z.string().optional(),
  thumbnail: z.string().optional(),
  rating: z.number().optional(),
  stock: z.number().optional(),
});

// Тип выводится из схемы — не дублируем
export type Product = z.infer<typeof ProductSchema>;

// =============================================
// СПИСОК ТОВАРОВ (ответ API)
// =============================================
export const ProductsResponseSchema = z.object({
  products: z.array(ProductSchema),
  total: z.number(),
  skip: z.number(),
  limit: z.number(),
});

// =============================================
// ФОРМА ТОВАРА
// =============================================
export const ProductFormSchema = z.object({
  id: z
    .union([z.string(), z.number()])
    .transform(Number)
    .refine((v) => !isNaN(v) && v > 0, { message: "ID должен быть числом > 0" }),
  title: z.string().min(3, "Название слишком короткое (минимум 3 символа)"),
  price: z
    .union([z.string(), z.number()])
    .transform(Number)
    .refine((v) => !isNaN(v) && v > 0, { message: "Цена должна быть числом > 0" }),
  description: z.string().optional(),
  category: z.string().optional(),
});

export type ProductFormData = z.infer<typeof ProductFormSchema>;

// =============================================
// SEARCH PARAMS (для URL-валидации)
// =============================================
export const CatalogSearchSchema = z.object({
  category: z.string().optional(),
  q: z.string().optional(),
});

export type CatalogSearch = z.infer<typeof CatalogSearchSchema>;