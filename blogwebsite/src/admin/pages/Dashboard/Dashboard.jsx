import { useEffect } from 'react';
import '../../../styles/AdminBase.css';

export default function Dashboard() {
  useEffect(()=>{ document.body.classList.add('admin-theme'); return ()=>document.body.classList.remove('admin-theme'); },[]);
  // later: fetch summary stats (posts, news, pages, drafts, etc.)
  const metrics = [
    { key:'posts', label:'Yazılar', value:'—' },
    { key:'news', label:'Haberler', value:'—' },
    { key:'pages', label:'Sayfalar', value:'—' },
    { key:'drafts', label:'Taslaklar', value:'—' }
  ];
  return (
    <div className="admin-shell">
      <div className="admin-toolbar" style={{marginBottom:10}}>
        <div className="admin-toolbar-title">Kontrol Paneli</div>
      </div>
      <div className="metric-cards">
        {metrics.map(m => (
          <div key={m.key} className="metric-card">
            <h4>{m.label}</h4>
            <div className="value">{m.value}</div>
          </div>
        ))}
      </div>
    </div>
  );
}
