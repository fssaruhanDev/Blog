import { useEffect, useState } from 'react';
import '../../../styles/AdminBase.css';

export default function Pages() {
  const [search, setSearch] = useState('');
  const [loading] = useState(false); // future use
  useEffect(()=>{ document.body.classList.add('admin-theme'); return ()=>document.body.classList.remove('admin-theme'); },[]);

  return (
    <div className="admin-shell">
      <div className="admin-toolbar">
        <div className="admin-toolbar-title">Sayfalar <small>(beta)</small></div>
        <div className="spacer" />
        <input className="admin-input" placeholder="Sayfalarda ara" value={search} onChange={e=>setSearch(e.target.value)} />
        <button className="admin-btn" disabled={loading}>Ara</button>
        <button className="admin-btn admin-btn-success" disabled={loading}>+ Yeni</button>
      </div>
      <div style={{fontSize:'.75rem'}} className="text-soft">Sayfa yönetimi yakında API entegrasyonu ile tamamlanacak.</div>
    </div>
  );
}
