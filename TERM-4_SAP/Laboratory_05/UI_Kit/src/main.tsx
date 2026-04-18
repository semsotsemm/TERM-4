import React from 'react'
import ReactDOM from 'react-dom/client'
import { App } from './App.tsx' // проверь, что App тоже на месте

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)