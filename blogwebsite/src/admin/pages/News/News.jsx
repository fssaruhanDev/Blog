import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getNews, deleteNews } from '../../../services/api';
import '../../../styles/AdminBase.css';

export default function News() {
  const navigate = useNavigate();
  const [data, setData] = useState({ items: [], total: 0, page: 1, pageSize: 10 });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [search, setSearch] = useState('');
  const [status, setStatus] = useState('');

  const load = async (page = 1, pageSize = 10) => {
    try {
      setLoading(true);
      const res = await getNews({ page, pageSize, search, status });
      setData({ items: res.items ?? res.Items ?? [], total: res.total ?? res.Total ?? 0, page: res.page ?? res.Page ?? page, pageSize: res.pageSize ?? res.PageSize ?? pageSize });
    } catch (e) {
      setError(e.message || 'Hata');
    } finally { setLoading(false); }
  };

  // initial load
  useEffect(() => { load(1, 10); /* eslint-disable-next-line */ }, []);

  // auto reload when status changes
  const statusFirst = useRef(true);
  useEffect(() => {
    if (statusFirst.current) { statusFirst.current = false; return; }
    load(1, data.pageSize);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [status]);

  const openCreate = () => navigate('/admin/news/new');
  const openEdit = (n) => navigate(`/admin/news/${n.id || n.ID}/edit`);
  const onDelete = async (n) => {
    if (!confirm('Silmek istiyor musunuz?')) return;
    try { setLoading(true); await deleteNews(n.id || n.ID); await load(data.page, data.pageSize); } catch (e) { setError(e.message || 'Silme hatası'); } finally { setLoading(false); }
  };

  return (
    <div className="admin-shell">
      <div className="admin-toolbar">
        <div className="admin-toolbar-title">News <small>(Toplam {data.total})</small></div>
        <div className="spacer" />
        <select className="admin-select" value={status} onChange={e=>setStatus(e.target.value)} style={{minWidth:160,height:40}}>
          <option value="">Tüm durumlar</option>
          <option value="draft">Taslak</option>
          <option value="published">Yayınlandı</option>
        </select>
        <input className="admin-input" placeholder="Haberlerde ara" value={search} onChange={e=>setSearch(e.target.value)} style={{minWidth:200,height:40}} onKeyDown={e=>e.key==='Enter'&&load(1,data.pageSize)} />
        <button className="admin-btn" style={{height:40}} onClick={()=>load(1,data.pageSize)} disabled={loading}>Ara</button>
        <button className="admin-btn admin-btn-success" style={{height:40}} onClick={openCreate} disabled={loading}>+ Yeni</button>
      </div>

      {error && <div className="alert alert-danger py-2 mb-2" style={{borderRadius:8,fontSize:'.75rem'}}>{error}</div>}

      <div style={{overflowX:'auto'}}>
        <table className="admin-table">
            <thead>
              <tr>
                <th>Başlık</th>
                <th style={{width:230}}>Kaynak</th>
                <th style={{width:110}}>Durum</th>
                <th style={{width:180}}>Tarih</th>
                <th style={{width:70}}></th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr><td colSpan={5}>Yükleniyor...</td></tr>
              ) : (
                (data.items || []).map(n => {
                  const id = n.id || n.ID;
                  const title = n.title || n.Title;
                  const src = (n.sourceName || n.SourceName) || (n.sourceUrl || n.SourceUrl) || '—';
                  const statusVal = n.status || n.Status || 'draft';
                  const pub = n.publishedAt || n.PublishedAt;
                  const dateTxt = pub ? new Date(pub).toLocaleString() : (n.createdDate ? new Date(n.createdDate).toLocaleString() : '—');
                  const pending = n._pending;
                  return (
                    <tr key={id} className={pending ? 'offline-pending' : ''}>
                      <td>
                        <a href="#" className="news-title-link" onClick={(e)=>{e.preventDefault(); openEdit(n);}}>{title}</a>
                        <div className="summary">{n.summary || n.Summary || ''}</div>
                      </td>
                      <td className="small">
                        {src !== '—' ? (
                          <a href={n.sourceUrl || n.SourceUrl} target="_blank" rel="noreferrer" style={{ color:'#2563eb' }}>{src}</a>
                        ) : '—'}
                      </td>
                      <td>
                        <span className={`badge-status ${statusVal}`}>{statusVal}</span>
                      </td>
                      <td style={{whiteSpace:'nowrap'}}>{dateTxt}</td>
                      <td>
                        <button className="icon-btn danger" aria-label="Sil" title="Sil" onClick={()=>onDelete(n)}>
                          <svg viewBox="0 0 24 24" fill="none" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                            <path d="M3 6h18" />
                            <path d="M8 6V4a1 1 0 0 1 1-1h6a1 1 0 0 1 1 1v2" />
                            <path d="M10 11v6" />
                            <path d="M14 11v6" />
                            <path d="M5 6l1 14a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2l1-14" />
                          </svg>
                        </button>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>

      <div className="admin-footer">
        <div className="info">Gösterilen {(data.items||[]).length} / {data.total}</div>
        <div className="flex gap-8">
          <button className="admin-btn admin-btn-outline" disabled={data.page<=1||loading} onClick={()=>load(data.page-1, data.pageSize)}>Önceki</button>
          <button className="admin-btn admin-btn-outline" disabled={(data.page*data.pageSize)>=data.total||loading} onClick={()=>load(data.page+1, data.pageSize)}>Sonraki</button>
        </div>
      </div>
    </div>
  );
}
