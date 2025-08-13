import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { createNews, getNewsItem, updateNews } from '../../../services/api';
import '../../../styles/AdminEditor.css';

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
  const rawTags = n?.tags || n?.Tags || '';
  const tagString = Array.isArray(rawTags) ? rawTags.join(', ') : rawTags;
  setTagsInput(tagString);
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
    } catch (e) {
      // Backend validation JSON içinden daha anlamlı mesaj seçmeye çalış
      const raw = e.message || '';
      let msg = raw;
      try {
        const jsonStart = raw.indexOf('{');
        if (jsonStart !== -1) {
          const parsed = JSON.parse(raw.slice(raw.indexOf('{')));
          if (parsed?.errors) {
            const firstKey = Object.keys(parsed.errors)[0];
            msg = parsed.errors[firstKey][0];
          }
        }
      } catch {}
      setError(msg || 'Kaydetme hatası');
    } finally { setLoading(false); }
  };

  return (
    <div className="editor-shell">
      <div className="editor-header">
        <h4 className="editor-header-title mb-0">{isEdit ? 'Haber Düzenle' : 'Yeni Haber'}</h4>
        <div className="spacer" />
        <button className="btn btn-sm btn-outline-secondary" onClick={()=>navigate(-1)} disabled={loading}>Geri</button>
        <button className="btn btn-sm btn-primary" onClick={onSave} disabled={loading}>{isEdit ? 'Güncelle' : 'Kaydet'}</button>
      </div>
      {error && <div className="alert alert-danger py-2 mb-3">{error}</div>}

      <div className="editor-grid">
        <div className="editor-main">
          <div className="form-row">
            <label className="form-label-sm">Başlık</label>
            <input className="form-control" value={title} onChange={(e)=>setTitle(e.target.value)} placeholder="Kısa ve net başlık" />
          </div>
          <div className="form-row">
            <label className="form-label-sm">Özet</label>
            <textarea className="form-control" rows={3} value={summary} onChange={(e)=>setSummary(e.target.value)} placeholder="Kısa özet / yorum" />
          </div>
          <div className="inline-fields">
            <div className="field">
              <label className="form-label-sm">Kaynak Adı</label>
              <input className="form-control" value={sourceName} onChange={(e)=>setSourceName(e.target.value)} placeholder="Örn. Medium" />
            </div>
            <div className="field">
              <label className="form-label-sm">Kaynak URL</label>
              <input className="form-control" value={sourceUrl} onChange={(e)=>setSourceUrl(e.target.value)} placeholder="https://..." />
            </div>
          </div>
          {/* save-bar kaldırıldı */}
        </div>
        <div className="editor-side">
          <div className="panel">
            <div className="panel-header">Yayın</div>
            <div className="panel-body">
              <div className="form-row status-inline">
                <label className="form-label-sm">DURUM</label>
                <select
                  className={`form-select status-select status-${status}`}
                  value={status}
                  onChange={(e)=>setStatus(e.target.value)}
                >
                  <option value="draft">Taslak</option>
                  <option value="published">Yayınlandı</option>
                </select>
              </div>
              <div className="form-row">
                <label className="form-label-sm">Yayın Tarihi</label>
                <input type="datetime-local" className="form-control" value={publishedAt} onChange={(e)=>setPublishedAt(e.target.value)} />
              </div>
            </div>
          </div>
          <div className="panel">
            <div className="panel-header">Etiketler</div>
            <div className="panel-body">
              <input className="form-control" value={tagsInput} onChange={(e)=>setTagsInput(e.target.value)} placeholder="virgül ile ekle: js, api" />
              <div className="form-label-sm mt-3 mb-1" style={{letterSpacing:0}}>İpucu</div>
              <p className="text-muted" style={{fontSize:'.65rem', margin:0}}>Etiketleri içerik keşfi ve filtreleme için kullanabilirsiniz.</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
