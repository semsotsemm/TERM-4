import React, { useState } from "react";
import {
  createFileRoute,
  redirect,
  useNavigate,
  Link,
} from "@tanstack/react-router";
import {
  useQuery,
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";
import { CatalogSearchSchema, ProductFormSchema, type Product } from "../schemas/productSchema";
import { fetchProducts, createProduct, updateProduct, deleteProduct } from "../api/products";
import { ProductForm } from "../components/ProductForm";
import { Toast } from "../components/Toast";
import { QueryStatusBar } from "../components/QueryStatusBar";

// =============================================
// ROUTE DEFINITION
// =============================================
export const Route = createFileRoute("/catalog")({
  // Auth Guard — защита маршрута до рендера компонента
  beforeLoad: () => {
    const raw = localStorage.getItem("lab8_user");
    if (!raw) {
      throw redirect({ to: "/login" });
    }
  },

  // validateSearch — Zod-валидация search params из URL
  validateSearch: (search) => CatalogSearchSchema.parse(search),

  component: CatalogPage,
});

// =============================================
// TOAST STATE
// =============================================
interface ToastState {
  id: number;
  message: string;
  type: "success" | "error";
}

// =============================================
// CATALOG PAGE COMPONENT
// =============================================
function CatalogPage() {
  const queryClient = useQueryClient();
  const navigate = useNavigate({ from: "/catalog" });

  // Читаем типизированные search params (валидированы Zod в validateSearch)
  const { category, q } = Route.useSearch();

  const [formOpen, setFormOpen] = useState(false);
  const [editTarget, setEditTarget] = useState<Product | null>(null);
  const [toasts, setToasts] = useState<ToastState[]>([]);
  const [localSearch, setLocalSearch] = useState(q ?? "");

  function addToast(message: string, type: "success" | "error") {
    const id = Date.now();
    setToasts((p) => [...p, { id, message, type }]);
    setTimeout(() => setToasts((p) => p.filter((t) => t.id !== id)), 3500);
  }

  // =============================================
  // READ — useQuery заменяет useEffect + useState
  // staleTime: 60s, gcTime: 5min
  // =============================================
  const {
    data: products = [],
    isLoading,
    isError,
    error,
    isFetching,
    dataUpdatedAt,
  } = useQuery({
    queryKey: ["products"],
    queryFn: fetchProducts,
    staleTime: 60_000,   // данные свежие 1 минуту
    gcTime: 300_000,     // в кэше 5 минут
  });

  // =============================================
  // CREATE mutation
  // =============================================
  const createMutation = useMutation({
    mutationFn: createProduct,
    onSuccess: () => {
      // Инвалидация — Query автоматически перезапросит список
      queryClient.invalidateQueries({ queryKey: ["products"] });
      setFormOpen(false);
      addToast("Товар добавлен", "success");
    },
    onError: (err: Error) => {
      addToast(`Ошибка: ${err.message}`, "error");
    },
  });

  // =============================================
  // UPDATE mutation
  // =============================================
  const updateMutation = useMutation({
    mutationFn: updateProduct,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      setFormOpen(false);
      setEditTarget(null);
      addToast("Товар обновлён", "success");
    },
    onError: (err: Error) => {
      addToast(`Ошибка: ${err.message}`, "error");
    },
  });

  // =============================================
  // DELETE mutation — ОПТИМИСТИЧНОЕ УДАЛЕНИЕ
  // Товар исчезает мгновенно, до ответа сервера.
  // При ошибке — откат через onError.
  // =============================================
  const deleteMutation = useMutation({
    mutationFn: deleteProduct,

    // onMutate вызывается ДО запроса — оптимистично убираем из кэша
    onMutate: async (id: number) => {
      // Отменяем текущие фоновые рефетчи чтобы не перезаписали оптимистичное
      await queryClient.cancelQueries({ queryKey: ["products"] });

      // Сохраняем снимок кэша для отката
      const previousProducts = queryClient.getQueryData<Product[]>(["products"]);

      // Оптимистично обновляем кэш — убираем товар из списка
      queryClient.setQueryData<Product[]>(["products"], (old = []) =>
        old.filter((p) => p.id !== id)
      );

      // Возвращаем контекст с снимком для onError
      return { previousProducts };
    },

    // onError — откат при ошибке сервера
    onError: (err: Error, _id, context) => {
      if (context?.previousProducts) {
        queryClient.setQueryData(["products"], context.previousProducts);
      }
      addToast(`Ошибка удаления: ${err.message}`, "error");
    },

    // onSettled — синхронизируемся с сервером в любом случае
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
  });

  // =============================================
  // FORM HANDLER
  // =============================================
  function handleFormSave(raw: {
    id: string | number;
    title: string;
    price: string | number;
    description?: string;
    category?: string;
  }) {
    const result = ProductFormSchema.safeParse(raw);
    if (!result.success) {
      const flat = result.error.flatten();
      const messages = Object.entries(flat.fieldErrors)
        .map(([field, msgs]) => `${field}: ${(msgs ?? []).join(", ")}`)
        .join("\n");
      addToast(`Ошибки валидации:\n${messages}`, "error");
      return;
    }

    const data = result.data;
    if (editTarget) {
      updateMutation.mutate({ ...data, id: editTarget.id });
    } else {
      createMutation.mutate(data);
    }
  }

  // =============================================
  // SEARCH PARAMS — обновляем URL при изменении category
  // =============================================
  function setCategory(cat: string | undefined) {
    navigate({
      search: (prev) => ({ ...prev, category: cat }),
    });
  }

  function handleSearchInput(val: string) {
    setLocalSearch(val);
    navigate({
      search: (prev) => ({ ...prev, q: val || undefined }),
    });
  }

  // =============================================
  // ФИЛЬТРАЦИЯ — category и q из URL
  // =============================================
  const categories = Array.from(new Set(products.map((p) => p.category).filter(Boolean))) as string[];

  const filtered = products.filter((p) => {
    const matchCat = !category || p.category === category;
    const matchQ = !q || p.title.toLowerCase().includes(q.toLowerCase());
    return matchCat && matchQ;
  });

  // Мутации в процессе
  const isSaving = createMutation.isPending || updateMutation.isPending;

  // =============================================
  // ERROR STATE — Zod или сеть
  // =============================================
  if (isError) {
    const msg = error instanceof Error ? error.message : "Неизвестная ошибка";
    const isStructural = msg.includes("Expected") || msg.includes("ZodError") || msg.includes("invalid_type");
    return (
      <main style={{ flex: 1 }}>
        <div className="page-wrap">
          <div className="state-box">
            <div className="state-icon">{isStructural ? "🔍" : "⚠"}</div>
            <h2 className="state-title">
              {isStructural ? "Ошибка структуры данных" : "Ошибка загрузки"}
            </h2>
            <p className="state-sub">{msg}</p>
            <button
              className="btn btn-lime"
              style={{ marginTop: 16 }}
              onClick={() => queryClient.invalidateQueries({ queryKey: ["products"] })}
            >
              Повторить
            </button>
          </div>
        </div>
      </main>
    );
  }

  // =============================================
  // RENDER
  // =============================================
  return (
    <main className="catalog-page">
      <div className="page-wrap">
        {/* ---- TOOLBAR ---- */}
        <div className="catalog-toolbar">
          <h1 className="toolbar-title">Каталог</h1>
          <span className="toolbar-count mono">{filtered.length} / {products.length}</span>

          {/* Поиск — синхронизирован с ?q= */}
          <div className="toolbar-search-wrap">
            <span className="toolbar-search-icon">⌕</span>
            <input
              type="search"
              className="toolbar-search"
              placeholder="Поиск..."
              value={localSearch}
              onChange={(e) => handleSearchInput(e.target.value)}
            />
          </div>

          {/* Активный фильтр категории */}
          {category && (
            <div className="active-filter">
              {category}
              <button className="filter-remove" onClick={() => setCategory(undefined)}>×</button>
            </div>
          )}

          <div style={{ marginLeft: "auto" }}>
            <button
              className="btn btn-lime"
              onClick={() => { setEditTarget(null); setFormOpen(true); }}
            >
              + Добавить
            </button>
          </div>
        </div>

        {/* ---- CATEGORY CHIPS (из данных) ---- */}
        {!isLoading && categories.length > 0 && (
          <div className="toolbar-category-filter" style={{ marginBottom: 24 }}>
            <button
              className={`category-btn${!category ? " active" : ""}`}
              onClick={() => setCategory(undefined)}
            >
              Все
            </button>
            {categories.map((cat) => (
              <button
                key={cat}
                className={`category-btn${category === cat ? " active" : ""}`}
                onClick={() => setCategory(cat)}
              >
                {cat}
              </button>
            ))}
          </div>
        )}

        {/* ---- LOADING SKELETONS ---- */}
        {isLoading && (
          <div className="skeleton-grid">
            {Array.from({ length: 8 }).map((_, i) => (
              <div key={i} className="skeleton-card">
                <div className="skeleton-img" />
                <div className="skeleton-body">
                  <div className="skel h12 w40" />
                  <div className="skel h16 w80" />
                  <div className="skel h12 w60" />
                </div>
              </div>
            ))}
          </div>
        )}

        {/* ---- GRID ---- */}
        {!isLoading && (
          <>
            {filtered.length === 0 ? (
              <div className="state-box">
                <div className="state-icon">📭</div>
                <h2 className="state-title">Ничего не найдено</h2>
                <p className="state-sub">Попробуйте изменить фильтры</p>
              </div>
            ) : (
              <div className="catalog-grid">
                {filtered.map((product, i) => (
                  <ProductCard
                    key={product.id}
                    product={product}
                    index={i}
                    onEdit={(p) => { setEditTarget(p); setFormOpen(true); }}
                    onDelete={(id) => deleteMutation.mutate(id)}
                    isDeleting={deleteMutation.isPending && deleteMutation.variables === product.id}
                  />
                ))}
              </div>
            )}
          </>
        )}
      </div>

      {/* ---- ФОРМА ---- */}
      {formOpen && (
        <ProductForm
          initial={editTarget}
          onSave={handleFormSave}
          onClose={() => { setFormOpen(false); setEditTarget(null); }}
          isLoading={isSaving}
        />
      )}

      {/* ---- TOASTS ---- */}
      <div className="toast-area">
        {toasts.map((t) => (
          <Toast key={t.id} message={t.message} type={t.type} />
        ))}
      </div>

      {/* ---- Query Status Bar (для демонстрации stale/fresh) ---- */}
      <QueryStatusBar
        isFetching={isFetching}
        isLoading={isLoading}
        dataUpdatedAt={dataUpdatedAt}
        staleTime={60_000}
      />
    </main>
  );
}

// =============================================
// PRODUCT CARD (внутри catalog)
// =============================================
interface ProductCardProps {
  product: Product;
  index: number;
  onEdit: (p: Product) => void;
  onDelete: (id: number) => void;
  isDeleting: boolean;
}

const GRADIENTS = [
  "linear-gradient(135deg,#1a1a2e,#16213e)",
  "linear-gradient(135deg,#0f2027,#203a43)",
  "linear-gradient(135deg,#1e3c72,#2a5298)",
  "linear-gradient(135deg,#141e30,#243b55)",
  "linear-gradient(135deg,#0f0c29,#302b63)",
  "linear-gradient(135deg,#200122,#6f0000)",
];

function ProductCard({ product, index, onEdit, onDelete, isDeleting }: ProductCardProps) {
  const [imgErr, setImgErr] = useState(false);

  return (
    <article
      className={`product-card${isDeleting ? " is-optimistic" : ""}`}
      style={{ animationDelay: `${Math.min(index * 35, 350)}ms` }}
    >
      <div className="card-img-wrap">
        {product.thumbnail && !imgErr ? (
          <img
            src={product.thumbnail}
            alt={product.title}
            className="card-img"
            onError={() => setImgErr(true)}
            loading="lazy"
          />
        ) : (
          <div className="card-placeholder" style={{ background: GRADIENTS[product.id % GRADIENTS.length] }}>
            📦
          </div>
        )}
        <div className="card-id-badge">#{product.id}</div>
      </div>

      <div className="card-body">
        {product.category && <span className="tag tag-cat">{product.category}</span>}
        <h3 className="card-title">{product.title}</h3>
        {product.description && <p className="card-desc">{product.description}</p>}

        <div className="card-meta">
          <span className="card-price">${product.price.toFixed(2)}</span>
          {product.rating !== undefined && (
            <span className="card-rating">★ {product.rating.toFixed(1)}</span>
          )}
          {product.stock !== undefined && (
            <span className="card-stock">{product.stock} шт.</span>
          )}
        </div>
      </div>

      <div className="card-actions">
        <Link
          to="/product/$id"
          params={{ id: String(product.id) }}
          className="btn btn-ghost"
          style={{ flex: 1, justifyContent: "center" }}
        >
          Детали
        </Link>
        <button className="btn btn-ghost" onClick={() => onEdit(product)} style={{ flex: 1 }}>
          ✏
        </button>
        <button
          className="btn btn-danger"
          onClick={() => onDelete(product.id)}
          disabled={isDeleting}
          style={{ flex: 1 }}
        >
          {isDeleting ? <span className="spinner" /> : "✕"}
        </button>
      </div>
    </article>
  );
}
