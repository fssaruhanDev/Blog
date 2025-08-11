import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getNews, deleteNews } from '../../../services/api';

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

  useEffect(() => { load(1, 10); }, []);

  const openCreate = () => navigate('/admin/news/new');
  const openEdit = (n) => navigate(`/admin/news/${n.id || n.ID}/edit`);
  const onDelete = async (n) => {
    if (!confirm('Silmek istiyor musunuz?')) return;
    try { setLoading(true); await deleteNews(n.id || n.ID); await load(data.page, data.pageSize); } catch (e) { setError(e.message || 'Silme hatası'); } finally { setLoading(false); }
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="d-flex align-items-center mb-3">
          <h6 className="mb-0 flex-grow-1">News
            <small className="ms-3 text-muted">Tümü ({data.total})</small>
          </h6>
          <button className="btn btn-success me-2" onClick={openCreate} disabled={loading}>+ Yeni</button>
          <div className="input-group" style={{ maxWidth: 420 }}>
            <select className="form-select" style={{ maxWidth: 160 }} value={status} onChange={(e)=>setStatus(e.target.value)}>
              <option value="">Tüm durumlar</option>
              <option value="draft">Taslak</option>
              <option value="published">Yayınlandı</option>
            </select>
            <input className="form-control" placeholder="Haberlerde ara" value={search} onChange={(e)=>setSearch(e.target.value)} />
            <button className="btn btn-primary" onClick={() => load(1, data.pageSize)} disabled={loading}>Ara</button>
          </div>
        </div>

        {error && <div className="alert alert-danger py-2">{error}</div>}

        <div className="table-responsive">
          <table className="table table-striped align-middle">
            <thead>
              <tr>
                <th>Başlık</th>
                <th style={{width:260}}>Kaynak</th>
                <th style={{width:140}}>Durum</th>
                <th style={{width:200}}>Tarih</th>
                <th style={{width:140}}></th>
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
                  const status = n.status || n.Status || 'draft';
                  const pub = n.publishedAt || n.PublishedAt;
                  const dateTxt = pub ? new Date(pub).toLocaleString() : (n.createdDate ? new Date(n.createdDate).toLocaleString() : '—');
                  return (
                    <tr key={id}>
                      <td>
                        <div className="fw-semibold"><a href="#" onClick={(e)=>{e.preventDefault(); openEdit(n);}}>{title}</a></div>
                        <div className="small text-muted">{n.summary || n.Summary || ''}</div>
                      </td>
                      <td className="small">
                        {src !== '—' ? (
                          <a href={n.sourceUrl || n.SourceUrl} target="_blank" rel="noreferrer">{src}</a>
                        ) : '—'}
                      </td>
                      <td>
                        <span className={`badge ${status==='published'?'bg-success':'bg-secondary'}`}>{status}</span>
                      </td>
                      <td>{dateTxt}</td>
                      <td className="text-end">
                        <button className="btn btn-sm btn-outline-primary me-2" onClick={()=>openEdit(n)}>Düzenle</button>
                        <button className="btn btn-sm btn-outline-danger" onClick={()=>onDelete(n)}>Sil</button>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>

        <div className="d-flex justify-content-between align-items-center">
          <div>Toplam: {data.total}</div>
          <div className="btn-group">
            <button className="btn btn-outline-secondary" disabled={data.page <= 1 || loading} onClick={() => load(data.page - 1, data.pageSize)}>Önceki</button>
            <button className="btn btn-outline-secondary" disabled={(data.page * data.pageSize) >= data.total || loading} onClick={() => load(data.page + 1, data.pageSize)}>Sonraki</button>
          </div>
        </div>
      </div>
    </div>
  );
}
