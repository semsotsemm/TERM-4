import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App'
import './assets/styles/auth.css'
import './assets/styles/catalog.css'
import './assets/styles/router-layout.css'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
