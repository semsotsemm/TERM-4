import { useMemo, useState, type FormEvent } from 'react'
import { ProductSchema, type Product } from '../../types/product'

interface ProductModalProps {
  mode: 'create' | 'edit'
  initialProduct?: Product
  onClose: () => void
  onCreate: (payload: { title: string; price: number; category?: string; thumbnail?: string }) => Promise<{
    ok: boolean
    message?: string
  }>
  onUpdate: (product: Product) => Promise<{ ok: boolean; message?: string }>
}

interface ProductFormValues {
  id: string
  title: string
  price: string
  category: string
  thumbnail: string
}

const toFormValues = (product?: Product): ProductFormValues => ({
  id: product ? String(product.id) : '',
  title: product?.title ?? '',
  price: product ? String(product.price) : '',
  category: product?.category ?? '',
  thumbnail: product?.thumbnail ?? '',
})

const ProductModal = ({ mode, initialProduct, onClose, onCreate, onUpdate }: ProductModalProps) => {
  const [formValues, setFormValues] = useState<ProductFormValues>(() => toFormValues(initialProduct))
  const [summaryErrors, setSummaryErrors] = useState<string[]>([])
  const [fieldErrors, setFieldErrors] = useState<Partial<Record<keyof ProductFormValues, string>>>({})
  const [submitting, setSubmitting] = useState(false)

  const modalTitle = mode === 'create' ? 'Добавить товар' : 'Изменить товар'
  const submitLabel = mode === 'create' ? 'Сохранить товар' : 'Обновить товар'

  const submitIcon = useMemo(
    () => (
      <svg viewBox="0 0 16 16" fill="none">
        <path
          d="M13 4L6 11l-3-3"
          stroke="white"
          strokeWidth="1.8"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
      </svg>
    ),
    [],
  )

  const setValue = (field: keyof ProductFormValues, value: string) => {
    setFormValues((current) => ({ ...current, [field]: value }))
  }

  const parseSummaryFromMessage = (message: string): string[] => {
    return message
      .split('\n')
      .map((line) => line.trim())
      .filter((line) => line.length > 0)
  }

  const validateLocal = () => {
    const candidate = {
      id:
        mode === 'create'
          ? Number(Date.now())
          : Number(initialProduct ? initialProduct.id : formValues.id || 0),
      title: formValues.title.trim(),
      price: Number(formValues.price),
      category: formValues.category.trim() || undefined,
      thumbnail: formValues.thumbnail.trim() || undefined,
    }

    const result = ProductSchema.safeParse(candidate)
    if (result.success) {
      setFieldErrors({})
      setSummaryErrors([])
      return { ok: true as const, data: result.data }
    }

    const flattened = result.error.flatten()
    const nextFieldErrors: Partial<Record<keyof ProductFormValues, string>> = {}

    if (flattened.fieldErrors.title?.[0]) {
      nextFieldErrors.title = flattened.fieldErrors.title[0]
    }

    if (flattened.fieldErrors.price?.[0]) {
      nextFieldErrors.price = flattened.fieldErrors.price[0]
    }

    if (flattened.fieldErrors.id?.[0]) {
      nextFieldErrors.id = flattened.fieldErrors.id[0]
    }

    if (flattened.fieldErrors.thumbnail?.[0]) {
      nextFieldErrors.thumbnail = flattened.fieldErrors.thumbnail[0]
    }

    setFieldErrors(nextFieldErrors)

    const summary = Object.entries(flattened.fieldErrors)
      .filter(([, messages]) => Array.isArray(messages) && messages.length > 0)
      .map(([field, messages]) => `${field}: ${messages[0]}`)
    setSummaryErrors(summary)

    return { ok: false as const }
  }

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    if (submitting) {
      return
    }

    const localValidation = validateLocal()
    if (!localValidation.ok) {
      return
    }

    setSubmitting(true)
    try {
      if (mode === 'create') {
        const createResult = await onCreate({
          title: localValidation.data.title,
          price: localValidation.data.price,
          category: localValidation.data.category,
          thumbnail: localValidation.data.thumbnail,
        })

        if (!createResult.ok) {
          if (createResult.message) {
            setSummaryErrors(parseSummaryFromMessage(createResult.message))
          }
          return
        }
      } else {
        const updateResult = await onUpdate(localValidation.data)
        if (!updateResult.ok) {
          if (updateResult.message) {
            setSummaryErrors(parseSummaryFromMessage(updateResult.message))
          }
          return
        }
      }

      onClose()
    } finally {
      setSubmitting(false)
    }
  }

  const onOverlayClick = (target: EventTarget | null, currentTarget: EventTarget | null) => {
    if (target === currentTarget) {
      onClose()
    }
  }

  return (
    <div className="overlay" onClick={(event) => onOverlayClick(event.target, event.currentTarget)}>
      <div className="modal" role="dialog" aria-modal="true" aria-labelledby="modal-title">
        <div className="modal-header">
          <div className="modal-header-left">
            <div className={mode === 'create' ? 'modal-mode-badge' : 'modal-mode-badge edit-mode'}>
              {mode === 'create' ? 'Новый товар' : 'Редактирование'}
            </div>
            <h2 className="modal-title" id="modal-title">
              {modalTitle}
            </h2>
          </div>
          <button className="btn-close" onClick={onClose} aria-label="Закрыть" type="button">
            <svg viewBox="0 0 14 14" fill="none">
              <path
                d="M2 2l10 10M12 2L2 12"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
              />
            </svg>
          </button>
        </div>

        <div className="modal-divider"></div>

        <div className="modal-body">
          <div className={summaryErrors.length > 0 ? 'validation-summary visible' : 'validation-summary'}>
            <div className="validation-summary-title">Исправьте ошибки валидации:</div>
            <ul className="validation-list">
              {summaryErrors.map((errorText) => (
                <li key={errorText}>{errorText}</li>
              ))}
            </ul>
          </div>

          <form id="product-form" noValidate onSubmit={handleSubmit}>
            <div className="form-section-label">Основная информация</div>
            <div className="form-grid">
              {mode === 'edit' && (
                <div className="form-group">
                  <label className="form-label" htmlFor="f-id">
                    ID
                  </label>
                  <input
                    className={fieldErrors.id ? 'form-input error' : 'form-input'}
                    type="number"
                    id="f-id"
                    value={formValues.id}
                    readOnly
                  />
                  <span className="field-error">{fieldErrors.id ?? ''}</span>
                </div>
              )}

              <div className="form-group span-2">
                <label className="form-label" htmlFor="f-title">
                  Название товара
                </label>
                <input
                  className={fieldErrors.title ? 'form-input error' : 'form-input'}
                  type="text"
                  id="f-title"
                  placeholder="напр. iPhone 14 Pro 256GB"
                  value={formValues.title}
                  onChange={(event) => setValue('title', event.target.value)}
                />
                <span className="field-error">{fieldErrors.title ?? ''}</span>
              </div>

              <div className="form-group">
                <label className="form-label" htmlFor="f-price">
                  Цена
                </label>
                <input
                  className={fieldErrors.price ? 'form-input error' : 'form-input'}
                  type="number"
                  id="f-price"
                  min={0.01}
                  step={0.01}
                  value={formValues.price}
                  onChange={(event) => setValue('price', event.target.value)}
                />
                <span className="field-error">{fieldErrors.price ?? ''}</span>
              </div>

              <div className="form-group">
                <label className="form-label" htmlFor="f-category">
                  Категория
                </label>
                <input
                  className="form-input"
                  type="text"
                  id="f-category"
                  value={formValues.category}
                  onChange={(event) => setValue('category', event.target.value)}
                />
              </div>

              <div className="form-group span-2">
                <label className="form-label" htmlFor="f-thumbnail">
                  URL изображения
                </label>
                <input
                  className={fieldErrors.thumbnail ? 'form-input error' : 'form-input'}
                  type="url"
                  id="f-thumbnail"
                  placeholder="https://..."
                  value={formValues.thumbnail}
                  onChange={(event) => setValue('thumbnail', event.target.value)}
                />
                <span className="field-error">{fieldErrors.thumbnail ?? ''}</span>
              </div>
            </div>

            <div className="modal-footer">
              <button className="btn-cancel" type="button" onClick={onClose}>
                Отмена
              </button>
              <button className={submitting ? 'btn-submit loading' : 'btn-submit'} type="submit">
                {submitting ? <div className="spinner"></div> : submitIcon}
                {submitting ? 'Сохранение...' : submitLabel}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}

export default ProductModal
