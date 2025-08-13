import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getPosts, deletePost } from "../../../services/api";
import '../../../styles/AdminBase.css';

export default function Blog() {
  const navigate = useNavigate();
  const [data, setData] = useState({ items: [], total: 0, page: 1, pageSize: 10 });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");
  const [bulk, setBulk] = useState("");
  const currentUserName = useMemo(() => {
    try { const u = JSON.parse(localStorage.getItem("auth_user") || "null"); return u?.firstName && u?.lastName ? `${u.firstName} ${u.lastName}` : (u?.userName || "—"); } catch { return "—"; }
  }, []);

  const load = async (page = 1, pageSize = 10) => {
    try {
      setLoading(true);
      const res = await getPosts({ page, pageSize, search });
      setData({ items: res.items ?? res.Items ?? [], total: res.total ?? res.Total ?? 0, page: res.page ?? res.Page ?? page, pageSize: res.pageSize ?? res.PageSize ?? pageSize });
    } catch (e) {
      setError(e.message || "Hata");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    document.body.classList.add('admin-theme');
    load(1, 10);
    return () => document.body.classList.remove('admin-theme');
  }, []);

  const openCreate = () => navigate('/admin/blog/new');
  const openEdit = (p) => navigate(`/admin/blog/${p.id || p.ID}/edit`);
  const onDelete = async (p) => {
    if (!confirm("Silmek istiyor musunuz?")) return;
    try {
      setLoading(true);
      await deletePost(p.id || p.ID);
      await load(data.page, data.pageSize);
    } catch (e) {
      setError(e.message || "Silme hatası");
    } finally {
      setLoading(false);
    }
  };

  const publishedCount = (data.items || []).filter(p => (p.status || p.Status) === 'published').length;
  const allCount = data.total || (data.items || []).length;

  return (
    <div className="admin-shell">
      <div className="admin-toolbar">
        <div className="admin-toolbar-title">Yazılar <small>(Tümü {allCount} | Yayınlanmış {publishedCount})</small></div>
        <div className="spacer" />
  <input className="admin-input" style={{maxWidth:240,height:40}} placeholder="Yazılarda ara" value={search} onChange={e=>setSearch(e.target.value)} onKeyDown={e=>e.key==='Enter'&&load(1,data.pageSize)} />
  <button className="admin-btn" style={{height:40}} onClick={()=>load(1, data.pageSize)} disabled={loading}>Ara</button>
  <button className="admin-btn admin-btn-success" style={{height:40}} onClick={openCreate} disabled={loading}>+ Yeni</button>
      </div>

      <div className="flex gap-8 flex-wrap items-center mt-2" style={{marginBottom:14}}>
        <select className="admin-select" value={bulk} onChange={e=>setBulk(e.target.value)} style={{minWidth:160}}>
          <option value="">Toplu işlemler</option>
          <option value="delete">Çöpe taşı</option>
        </select>
        <button className="admin-btn admin-btn-outline" style={{padding:'7px 14px'}} disabled={!bulk}>Uygula</button>
      </div>

      {error && <div className="alert alert-danger py-2" style={{borderRadius:8, fontSize:'.75rem'}}>{error}</div>}

      <div style={{overflowX:'auto'}}>
        <table className="admin-table">
          <thead>
            <tr>
              <th style={{width:32}}><input type="checkbox" /></th>
              <th style={{width:70}}>Görsel</th>
              <th>Başlık</th>
              <th style={{width:150}}>Yazar</th>
              <th style={{width:120}}>Durum</th>
              <th style={{width:180}}>Tarih</th>
              <th style={{width:60}}></th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={7}>Yükleniyor...</td></tr>
            ) : (
              (data.items || []).map(p => {
                const id = p.id || p.ID;
                const title = p.title || p.Title;
                const status = (p.status || p.Status) || 'draft';
                const pub = p.publishedAt || p.PublishedAt;
                const cover = p.coverImageUrl || p.CoverImageUrl || '';
                if (typeof window !== 'undefined') console.debug('Post row', {id, cover, raw:p});
                const dateTxt = pub ? new Date(pub).toLocaleString() : new Date(p.createdDate || p.CreatedDate).toLocaleString();
                return (
                  <tr key={id}>
                    <td><input type="checkbox" /></td>
          <td>{cover ? <img src={cover} data-raw={cover} alt="kapak" style={{width:56,height:38,objectFit:'cover',borderRadius:6,border:'1px solid #e2e8f0',background:'#f1f5f9'}} onError={(ev)=>{
                      const el=ev.currentTarget;
                      if(!el.dataset.fallbackTried){
                        el.dataset.fallbackTried='1';
            const r=el.getAttribute('data-raw')||'';
                        const idx=r.indexOf('/uploads/');
                        if(idx>-1){
                          const rel=r.substring(idx);
                          el.src=`${window.location.origin}${rel}`;
                          return;
                        }
                      }
                      el.style.opacity='0.35'; el.style.border='1px solid #f87171';
                    }} /> : <span className="text-soft" style={{fontSize:'.55rem'}}>Yok</span>}</td>
                    <td>
                      <div style={{fontWeight:600}}>
                        <a href="#" onClick={(e)=>{e.preventDefault(); openEdit(p);}}>{title}</a>
                      </div>
                    </td>
                    <td>{currentUserName}</td>
                    <td><span className={`badge-status ${status==='published'?'published':'draft'}`}>{status}</span></td>
                    <td style={{fontSize:'.7rem'}}>{dateTxt}</td>
                    <td>
                      <button className="icon-btn danger" aria-label="Sil" title="Sil" onClick={()=>onDelete(p)}>
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
                );})
            )}
            {!loading && (data.items||[]).length===0 && <tr><td colSpan={6} className="text-soft" style={{fontSize:'.7rem'}}>Kayıt yok.</td></tr>}
          </tbody>
        </table>
      </div>

      <div className="admin-footer">
        <div className="info">Toplam {data.total}</div>
        <div className="flex gap-8">
          <button className="admin-btn admin-btn-outline" disabled={data.page<=1||loading} onClick={()=>load(data.page-1, data.pageSize)}>Önceki</button>
          <button className="admin-btn admin-btn-outline" disabled={(data.page*data.pageSize)>=data.total||loading} onClick={()=>load(data.page+1, data.pageSize)}>Sonraki</button>
        </div>
      </div>
    </div>
  );
}
