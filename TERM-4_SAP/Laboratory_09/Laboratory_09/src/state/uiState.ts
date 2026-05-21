import { atom, selector } from 'recoil'
import { localStorageEffect } from './localStorageEffect'

export interface UiSettings {
  viewMode: 'grid' | 'list'
  theme: 'light' | 'dark'
}

export const defaultUiSettings: UiSettings = {
  viewMode: 'grid',
  theme: 'light',
}

export const uiSettingsState = atom<UiSettings>({
  key: 'uiSettingsState',
  default: defaultUiSettings,
  effects: [localStorageEffect<UiSettings>('lab9_ui_settings')],
})

export const productsLayoutClassState = selector<string>({
  key: 'productsLayoutClassState',
  get: ({ get }) => {
    const { viewMode } = get(uiSettingsState)
    return viewMode === 'list' ? 'product-list' : 'product-grid'
  },
})
