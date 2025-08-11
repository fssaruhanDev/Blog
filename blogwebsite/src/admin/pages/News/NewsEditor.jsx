import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { createNews, getNewsItem, updateNews } from '../../../services/api';

export default function NewsEditor() {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = !!id && id !== 'new';
  const [title, setTitle] = useState('');
  const [summary, setSummary] = useState('');
  const [sourceUrl, setSourceUrl] = useState('');
  const [sourceName, setSourceName] = useState('');
  const [status, setStatus] = useState('draft');
  const [publishedAt, setPublishedAt] = useState('');
  const [tagsInput, setTagsInput] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const load = async () => {
      if (!isEdit) return;
      try {
        setLoading(true);
        const n = await getNewsItem(id);
        setTitle(n?.title || n?.Title || '');
        setSummary(n?.summary || n?.Summary || '');
        setSourceUrl(n?.sourceUrl || n?.SourceUrl || '');
        setSourceName(n?.sourceName || n?.SourceName || '');
        setStatus(n?.status || n?.Status || 'draft');
        const pub = n?.publishedAt || n?.PublishedAt;
        setPublishedAt(pub ? new Date(pub).toISOString().slice(0,16) : '');
        setTagsInput((n?.tags || n?.Tags || []).join(', '));
      } catch (e) { setError(e.message || 'Yükleme hatası'); } finally { setLoading(false); }
    };
    load();
  }, [id, isEdit]);

  const onSave = async () => {
    try {
      setLoading(true);
      setError('');
      const tags = tagsInput.split(',').map(t=>t.trim()).filter(Boolean);
      const payload = { title, summary, sourceUrl, sourceName, tags, status, publishedAt: publishedAt ? new Date(publishedAt).toISOString() : null };
      if (isEdit) await updateNews(id, payload);
      else await createNews(payload);
      navigate('/admin/news');
    } catch (e) { setError(e.message || 'Kaydetme hatası'); } finally { setLoading(false); }
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="d-flex align-items-center mb-3">
          <h6 className="mb-0 flex-grow-1">{isEdit ? 'Haber Düzenle' : 'Yeni Haber'}</h6>
          <button className="btn btn-secondary me-2" onClick={()=>navigate(-1)} disabled={loading}>Geri</button>
          <button className="btn btn-primary" onClick={onSave} disabled={loading}>{isEdit ? 'Güncelle' : 'Kaydet'}</button>
        </div>
        {error && <div className="alert alert-danger py-2">{error}</div>}

        <div className="row g-3">
          <div className="col-md-8">
            <label className="form-label">Başlık</label>
            <input className="form-control" value={title} onChange={(e)=>setTitle(e.target.value)} placeholder="Kısa ve net başlık" />
          </div>
          <div className="col-md-4">
            <label className="form-label">Durum</label>
            <select className="form-select" value={status} onChange={(e)=>setStatus(e.target.value)}>
              <option value="draft">Taslak</option>
              <option value="published">Yayınlandı</option>
            </select>
          </div>
          <div className="col-12">
            <label className="form-label">Özet</label>
            <textarea className="form-control" rows={3} value={summary} onChange={(e)=>setSummary(e.target.value)} placeholder="Kısa özet / yorum"></textarea>
          </div>
          <div className="col-md-6">
            <label className="form-label">Kaynak Adı</label>
            <input className="form-control" value={sourceName} onChange={(e)=>setSourceName(e.target.value)} placeholder="Örn. Medium, Dev.to, Resmi duyuru" />
          </div>
          <div className="col-md-6">
            <label className="form-label">Kaynak URL</label>
            <input className="form-control" value={sourceUrl} onChange={(e)=>setSourceUrl(e.target.value)} placeholder="https://..." />
          </div>
          <div className="col-md-6">
            <label className="form-label">Yayın Tarihi</label>
            <input type="datetime-local" className="form-control" value={publishedAt} onChange={(e)=>setPublishedAt(e.target.value)} />
          </div>
          <div className="col-md-6">
            <label className="form-label">Etiketler</label>
            <input className="form-control" value={tagsInput} onChange={(e)=>setTagsInput(e.target.value)} placeholder="virgülle ayır: js, azure, api" />
          </div>
        </div>
      </div>
    </div>
  );
}
