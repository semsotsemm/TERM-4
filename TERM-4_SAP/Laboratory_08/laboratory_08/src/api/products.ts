import {
  ProductSchema,
  ProductsResponseSchema,
  type Product,
} from "../schemas/productSchema";

const BASE = "https://dummyjson.com/products";

// =============================================
// READ — список товаров
// Zod валидирует ответ ДО того как данные попадут в кэш Query
// =============================================
export async function fetchProducts(): Promise<Product[]> {
  const res = await fetch(`${BASE}?limit=30`);
  if (!res.ok) throw new Error(`Ошибка сети: HTTP ${res.status}`);

  const json: unknown = await res.json();

  // parse (не safeParse) — намеренно бросает при несоответствии типов
  // TanStack Query поймает ошибку и выставит isError = true
  const validated = ProductsResponseSchema.parse(json);
  return validated.products;
}

// =============================================
// READ — один товар по id
// =============================================
export async function fetchProductById(id: number): Promise<Product> {
  const res = await fetch(`${BASE}/${id}`);
  if (!res.ok) throw new Error(`Товар не найден: HTTP ${res.status}`);

  const json: unknown = await res.json();
  return ProductSchema.parse(json);
}

// =============================================
// CREATE
// =============================================
export async function createProduct(
  data: Omit<Product, "thumbnail" | "rating" | "stock">
): Promise<Product> {
  const res = await fetch(`${BASE}/add`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      title: data.title,
      price: data.price,
      description: data.description,
      category: data.category,
    }),
  });

  if (!res.ok) throw new Error(`Ошибка создания: HTTP ${res.status}`);

  const json: unknown = await res.json();
  // DummyJSON возвращает id=1 всегда, подменяем на введённый
  const raw = { ...(json as object), id: data.id, title: data.title, price: data.price };
  return ProductSchema.parse(raw);
}

// =============================================
// UPDATE
// =============================================
export async function updateProduct(
  data: Omit<Product, "thumbnail" | "rating" | "stock">
): Promise<Product> {
  // DummyJSON принимает PUT только для id 1-100; для локальных используем 1
  const apiId = data.id <= 100 ? data.id : 1;
  const res = await fetch(`${BASE}/${apiId}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      title: data.title,
      price: data.price,
      description: data.description,
      category: data.category,
    }),
  });

  if (!res.ok) throw new Error(`Ошибка обновления: HTTP ${res.status}`);

  // Возвращаем merged объект с нашими данными
  return ProductSchema.parse({
    id: data.id,
    title: data.title,
    price: data.price,
    description: data.description,
    category: data.category,
  });
}

// =============================================
// DELETE
// =============================================
export async function deleteProduct(id: number): Promise<void> {
  const apiId = id <= 100 ? id : 1;
  const res = await fetch(`${BASE}/${apiId}`, { method: "DELETE" });
  if (!res.ok) throw new Error(`Ошибка удаления: HTTP ${res.status}`);
}