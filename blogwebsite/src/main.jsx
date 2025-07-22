import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom'; // EKLE
import App from './App';

ReactDOM.createRoot(document.getElementById('root')).render(
  <BrowserRouter>  {/* SAR */}
    <App />
  </BrowserRouter>
);