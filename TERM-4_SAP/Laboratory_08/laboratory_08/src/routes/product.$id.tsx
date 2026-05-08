import React, { useState } from "react";
import { createFileRoute, Link } from "@tanstack/react-router";
import { useQuery } from "@tanstack/react-query";
import { fetchProductById } from "../api/products";

export const Route = createFileRoute("/product/$id")({
  component: ProductDetailPage,
});

function ProductDetailPage() {
  const { id } = Route.useParams();

  // =============================================
  // READ — useQuery для одного товара
  // staleTime: 60s, gcTime: 5min
  // =============================================
  const {
    data: product,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: ["product", id],
    queryFn: () => fetchProductById(Number(id)),
    staleTime: 60_000,
    gcTime: 300_000,
  });

  // ---- LOADING SKELETON ----
  if (isLoading) {
    return (
      <main className="detail-page">
        <div className="page-wrap">
          <div className="detail-skeleton-back skel" style={{ width: 80, height: 16, marginBottom: 32 }} />
          <div className="detail-layout">
            <div className="detail-img-wrap">
              <div className="skel" style={{ width: "100%", height: "100%" }} />
            </div>
            <div className="detail-info" style={{ gap: 20 }}>
              <div className="skel h12 w40" />
              <div className="skel h16 w80" style={{ height: 40 }} />
              <div className="skel h16 w40" style={{ height: 48 }} />
              <div className="skel h12 w60" />
              <div className="skel h12 w80" />
              <div className="skel h12 w70" />
            </div>
          </div>
        </div>
      </main>
    );
  }

  // ---- ERROR STATE — Zod или сеть ----
  if (isError) {
    const msg = error instanceof Error ? error.message : "Неизвестная ошибка";
    const isStructural =
      msg.includes("Expected") || msg.includes("ZodError") || msg.includes("invalid_type");
    return (
      <main className="detail-page">
        <div className="page-wrap">
          <div className="state-box">
            <div className="state-icon">{isStructural ? "🔍" : "⚠"}</div>
            <h2 className="state-title">
              {isStructural ? "Ошибка структуры данных" : "Товар не найден"}
            </h2>
            <p className="state-sub">{msg}</p>
            <Link to="/catalog" className="btn btn-lime" style={{ marginTop: 16 }}>
              ← Каталог
            </Link>
          </div>
        </div>
      </main>
    );
  }

  if (!product) return null;

  return (
    <main className="detail-page">
      <div className="page-wrap">
        <Link to="/catalog" className="detail-back">← Каталог</Link>

        <div className="detail-layout">
          {/* ---- ИЗОБРАЖЕНИЕ ---- */}
          <div className="detail-img-wrap">
            {product.thumbnail ? (
              <ProductImage src={product.thumbnail} alt={product.title} />
            ) : (
              <div className="detail-placeholder">📦</div>
            )}
          </div>

          {/* ---- ИНФОРМАЦИЯ ---- */}
          <div className="detail-info">
            {product.category && (
              <span className="tag tag-cat detail-category">{product.category}</span>
            )}

            <h1 className="detail-title">{product.title}</h1>

            <div className="detail-price">${product.price.toFixed(2)}</div>

            <div className="detail-meta-row">
              {product.rating !== undefined && (
                <span className="detail-meta-item">
                  <span className="detail-meta-label">Рейтинг</span>
                  <span className="detail-meta-value card-rating">★ {product.rating.toFixed(1)}</span>
                </span>
              )}
              {product.stock !== undefined && (
                <span className="detail-meta-item">
                  <span className="detail-meta-label">На складе</span>
                  <span className="detail-meta-value">{product.stock} шт.</span>
                </span>
              )}
              <span className="detail-meta-item">
                <span className="detail-meta-label">ID</span>
                <span className="detail-meta-value mono">#{product.id}</span>
              </span>
            </div>

            {product.description && (
              <p className="detail-desc">{product.description}</p>
            )}

            <div className="detail-actions">
              <Link to="/catalog" className="btn btn-ghost">← Назад к каталогу</Link>
              <Link
                to="/catalog"
                search={{ category: product.category }}
                className="btn btn-ghost"
              >
                {product.category ?? "Все"} →
              </Link>
            </div>

            {/* ---- ZOD INFO (демонстрация) ---- */}
            <div className="detail-zod-box">
              <div className="detail-zod-label">// Zod-валидация пройдена</div>
              <div className="detail-zod-fields">
                {[
                  { field: "id", value: String(product.id), type: "number" },
                  { field: "title", value: product.title, type: "string(min 3)" },
                  { field: "price", value: `${product.price}`, type: "number(>0)" },
                  { field: "category", value: product.category ?? "—", type: "string?" },
                ].map(({ field, value, type }) => (
                  <div key={field} className="zod-field-row">
                    <span className="zod-field-name">{field}</span>
                    <span className="zod-field-type">{type}</span>
                    <span className="zod-field-value">{value}</span>
                    <span className="zod-check">✓</span>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}

// ---- IMAGE с fallback ----
function ProductImage({ src, alt }: { src: string; alt: string }) {
  const [err, setErr] = useState(false);
  if (err) return <div className="detail-placeholder">📦</div>;
  return (
    <img
      src={src}
      alt={alt}
      className="detail-img"
      onError={() => setErr(true)}
    />
  );
}
