import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom'; // EKLE
import App from './App';
import { startAutoSync } from './services/api';

// Başlangıçta offline kuyruk senkronizasyonunu başlat
startAutoSync();

ReactDOM.createRoot(document.getElementById('root')).render(
  <BrowserRouter>  {/* SAR */}
    <App />
  </BrowserRouter>
);