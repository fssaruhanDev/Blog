import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getPosts, deletePost } from "../../../services/api";

export default function Blog() {
  const navigate = useNavigate();
  const [data, setData] = useState({ items: [], total: 0, page: 1, pageSize: 10 });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");
  const [bulk, setBulk] = useState("");
  const [filterDate, setFilterDate] = useState("");
  const [filterCategory, setFilterCategory] = useState("");
  const [filterFormat, setFilterFormat] = useState("");
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

  useEffect(() => { load(1, 10); }, []);

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
    <div className="card">
      <div className="card-body">
        <div className="d-flex align-items-center mb-3">
          <h6 className="mb-0 flex-grow-1">Yazılar
            <small className="ms-3 text-muted">Tümü ({allCount})</small>
            <small className="ms-2 text-muted">| Yayınlanmış ({publishedCount})</small>
          </h6>
          <button className="btn btn-success me-2" onClick={openCreate} disabled={loading}>+ Yeni</button>
          <div className="input-group" style={{ maxWidth: 320 }}>
            <input className="form-control" placeholder="Yazılarda ara" value={search} onChange={(e) => setSearch(e.target.value)} />
            <button className="btn btn-primary" onClick={() => load(1, data.pageSize)} disabled={loading}>Ara</button>
          </div>
        </div>

        {/* Filters row */}
        <div className="row g-2 align-items-center mb-3">
          <div className="col-auto">
            <select className="form-select form-select-sm" value={bulk} onChange={(e)=>setBulk(e.target.value)}>
              <option value="">Toplu işlemler</option>
              <option value="delete">Çöpe taşı</option>
            </select>
          </div>
          <div className="col-auto">
            <button className="btn btn-sm btn-outline-secondary">Uygula</button>
          </div>
          <div className="col-auto">
            <select className="form-select form-select-sm" value={filterDate} onChange={(e)=>setFilterDate(e.target.value)}>
              <option value="">Tüm tarihler</option>
            </select>
          </div>
          <div className="col-auto">
            <select className="form-select form-select-sm" value={filterCategory} onChange={(e)=>setFilterCategory(e.target.value)}>
              <option value="">Tüm kategoriler</option>
            </select>
          </div>
          <div className="col-auto">
            <select className="form-select form-select-sm" value={filterFormat} onChange={(e)=>setFilterFormat(e.target.value)}>
              <option value="">Tüm biçimler</option>
            </select>
          </div>
          <div className="col-auto">
            <button className="btn btn-sm btn-outline-secondary" onClick={() => load(1, data.pageSize)}>Süz</button>
          </div>
        </div>

        {error && <div className="alert alert-danger py-2">{error}</div>}

        <div className="table-responsive">
          <table className="table table-striped align-middle wp-table">
            <thead>
              <tr>
                <th style={{width:32}}><input type="checkbox" /></th>
                <th>Başlık</th>
                <th style={{width:180}}>Yazar</th>
                <th style={{width:200}}>Kategoriler</th>
                <th style={{width:220}}>Etiketler</th>
                <th className="col-comments" style={{width:80}}><span className="visually-hidden">Yorumlar</span></th>
                <th style={{width:180}}>Tarih</th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr><td colSpan={7}>Yükleniyor...</td></tr>
              ) : (
                (data.items || []).map(p => {
                  const id = p.id || p.ID;
                  const title = p.title || p.Title;
                  const status = p.status || p.Status;
                  const pub = p.publishedAt || p.PublishedAt;
                  const dateTxt = pub ? `Yayınlanmış\n${new Date(pub).toLocaleString()}` : `Taslak\n${new Date(p.createdDate || p.CreatedDate).toLocaleString()}`;
                  return (
                  <tr key={id}>
                    <td><input type="checkbox" /></td>
                    <td>
                      <div className="fw-semibold"><a href="#" onClick={(e)=>{e.preventDefault(); openEdit(p);}}>{title}</a></div>
                      <div className="small text-muted">
                        <a href="#" onClick={(e)=>{e.preventDefault(); openEdit(p);}}>Düzenle</a>
                        <span className="mx-1">|</span>
                        <a href="#" onClick={(e)=>{e.preventDefault(); onDelete(p);}}>Çöp</a>
                        <span className="mx-1">|</span>
                        <a href="#" onClick={(e)=>e.preventDefault()}>Görüntüle</a>
                      </div>
                    </td>
                    <td>{currentUserName}</td>
                    <td className="text-muted">—</td>
                    <td className="text-muted">—</td>
                    <td className="col-comments"><span className="comment-bubble">0</span></td>
                    <td style={{whiteSpace:'pre-line'}}>{dateTxt}</td>
                  </tr>
                );})
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
