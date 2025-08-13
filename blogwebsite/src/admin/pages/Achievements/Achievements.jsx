import { useEffect } from 'react';
import '../../../styles/AdminBase.css';

export default function Achievements() {
  useEffect(()=>{ document.body.classList.add('admin-theme'); return ()=>document.body.classList.remove('admin-theme'); },[]);
  return (
    <div className="admin-shell">
      <div className="admin-toolbar">
        <div className="admin-toolbar-title">Başarılar <small>(beta)</small></div>
        <div className="spacer" />
        <button className="admin-btn admin-btn-success">+ Yeni</button>
      </div>
      <div style={{fontSize:'.75rem'}} className="text-soft">Başarılar modülü için içerik listesi ve düzenleme ekranı henüz eklenmedi.</div>
    </div>
  );
}
