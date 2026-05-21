import type { AtomEffect } from 'recoil'

export const localStorageEffect = <T,>(storageKey: string): AtomEffect<T> => {
  return ({ setSelf, onSet, trigger }) => {
    if (typeof window === 'undefined') {
      return
    }

    if (trigger === 'get') {
      const savedValue = window.localStorage.getItem(storageKey)
      if (savedValue) {
        try {
          setSelf(JSON.parse(savedValue) as T)
        } catch {
          window.localStorage.removeItem(storageKey)
        }
      }
    }

    onSet((newValue, _oldValue, isReset) => {
      if (isReset) {
        window.localStorage.removeItem(storageKey)
        return
      }

      window.localStorage.setItem(storageKey, JSON.stringify(newValue))
    })
  }
}
