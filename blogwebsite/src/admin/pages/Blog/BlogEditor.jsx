import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createPost, getPost, updatePost, uploadImage } from "../../../services/api";
import CkSuperEditor from '../../../editor/CkSuperEditor';
import '../../../styles/Blog.css';
import '../../../styles/AdminEditor.css';

export default function BlogEditor() {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = !!id && id !== "new";
  const [title, setTitle] = useState("");
  const [excerpt, setExcerpt] = useState("");
  const [content, setContent] = useState("");
  const [status, setStatus] = useState("draft");
  const [publishedAt, setPublishedAt] = useState("");
  const [coverImageUrl, setCoverImageUrl] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [saved, setSaved] = useState(false);
  // Using CKEditor only
  const [useCk] = useState(true);
  const [tags, setTags] = useState([]);
  const [tagInput, setTagInput] = useState("");
  const [uploading, setUploading] = useState(false);
  const codeLanguages = [
    { value: 'plaintext', label: 'Plain Text' },
    { value: 'markup', label: 'HTML' },
    { value: 'css', label: 'CSS' },
    { value: 'javascript', label: 'JavaScript' },
    { value: 'typescript', label: 'TypeScript' },
    { value: 'jsx', label: 'JSX' },
    { value: 'tsx', label: 'TSX' },
    { value: 'json', label: 'JSON' },
    { value: 'yaml', label: 'YAML' },
    { value: 'bash', label: 'Bash' },
    { value: 'sql', label: 'SQL' },
    { value: 'csharp', label: 'C#' },
    { value: 'java', label: 'Java' },
    { value: 'python', label: 'Python' },
    { value: 'go', label: 'Go' },
    { value: 'rust', label: 'Rust' },
    { value: 'php', label: 'PHP' },
  ];

  // TipTap removed

  useEffect(() => {
    const load = async () => {
      if (!isEdit) return;
      try {
        setLoading(true);
        const p = await getPost(id);
  if (typeof window !== 'undefined') console.debug('GetPost response', p);
        setTitle(p.title || p.Title || "");
        setExcerpt(p.excerpt || p.Excerpt || "");
  const html = p.content || p.Content || "";
  setContent(html);
  setStatus(p.status || p.Status || "draft");
  setCoverImageUrl(p.coverImageUrl || p.CoverImageUrl || "");
        const pub = p.publishedAt || p.PublishedAt;
        setPublishedAt(pub ? new Date(pub).toISOString().slice(0,16) : "");
  // TODO: API'den etiket/kategori geldiğinde buraya set edilecek
      } catch (e) {
        setError(e.message || "Yükleme hatası");
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [id, isEdit]);

  const onSave = async () => {
    try {
      setLoading(true);
      setError("");
      const normCover = (coverImageUrl||'').trim();
      // Extract relative upload path if full URL given
      const normalizedCover = (()=>{
        if(!normCover) return null;
        const idx = normCover.indexOf('/uploads/');
        if(idx > -1) return normCover.substring(idx); // '/uploads/xyz.png'
        return normCover; // external URL (keep as-is)
      })();
      const payload = {
        title,
        excerpt,
        content,
        // send null if empty so backend keeps existing (avoid wiping with empty string)
        coverImageUrl: normalizedCover && normalizedCover.length ? normalizedCover : null,
        status,
        publishedAt: publishedAt ? new Date(publishedAt).toISOString() : null,
      };
      if (isEdit) {
        await updatePost(id, payload);
        // reload to ensure we reflect server state
        try { const fresh = await getPost(id); setCoverImageUrl(fresh.coverImageUrl || fresh.CoverImageUrl || ''); } catch { /* ignore */ }
      } else {
        const created = await createPost(payload);
        // yeni oluşturulduysa URL değiştirmeden form üzerinde kalıyoruz
        if (!id && created?.ID) {
          // yönlendirmiyoruz, sadece başarı mesajı
        }
      }
      setSaved(true);
      setTimeout(()=>setSaved(false), 2500);
    } catch (e) {
      setError(e.message || "Kaydetme hatası");
    } finally {
      setLoading(false);
    }
  };

  // Tags helpers
  const addTag = (val) => {
    const t = val.trim();
    if (!t) return;
    setTags((prev) => (prev.includes(t) ? prev : [...prev, t]));
  };
  const removeTag = (t) => setTags((prev) => prev.filter((x) => x !== t));
  const onTagKeyDown = (e) => {
    if (e.key === 'Enter' || e.key === ',') {
      e.preventDefault();
      const parts = tagInput.split(',');
      parts.forEach((p) => addTag(p));
      setTagInput("");
    }
  };
  const popularTags = [
    'programlama', 'RESTful API', 'API Güvenliği', 'Web Hizmetleri', 'API Tasarımı',
    'odaklı', 'nesne', 'yazılım', 'software', 'developer'
  ];

  // Link helpers
  // TipTap link helpers removed

  const onFileChange = async (e) => {
    const file = e.target.files?.[0];
    if(!file) return;
    try {
      setUploading(true);
      const { url } = await uploadImage(file);
      setCoverImageUrl(url);
    } catch(err){
      setError(err.message || 'Yükleme hatası');
    } finally { setUploading(false); }
  };

  return (
    <div className="editor-shell">
      <div className="editor-header">
        <h4 className="editor-header-title mb-0">{isEdit ? 'Blog Düzenle' : 'Yeni Blog Yazısı'}</h4>
        <div className="spacer" />
        <button className="btn btn-sm btn-outline-secondary" onClick={() => navigate(-1)} disabled={loading}>Geri</button>
        <button className="btn btn-sm btn-primary" onClick={onSave} disabled={loading}>{isEdit ? 'Güncelle' : 'Kaydet'}</button>
      </div>
  {error && <div className="alert alert-danger py-2 mb-3">{error}</div>}
  {saved && !error && <div className="alert alert-success py-2 mb-3">Kaydedildi</div>}

      <div className="editor-grid">
        <div className="editor-main">
          <div className="form-row">
            <label className="form-label-sm">Başlık</label>
            <input className="form-control" value={title} onChange={(e)=>setTitle(e.target.value)} placeholder="Başlık" />
          </div>
          <div className="form-row">
            <label className="form-label-sm">İçerik</label>
            <CkSuperEditor value={content} onChange={setContent} />
          </div>
          <div className="form-row">
            <label className="form-label-sm">Öne Çıkan Görsel (URL)</label>
            <input className="form-control" placeholder="https://..." value={coverImageUrl} onChange={(e)=>setCoverImageUrl(e.target.value)} />
            {coverImageUrl && (
              <div className="mt-2" style={{maxWidth:'260px'}}>
                <img
                  src={coverImageUrl}
                  data-raw={coverImageUrl}
                  alt="cover preview"
                  style={{width:'100%',borderRadius:'8px',border:'1px solid #e5e7eb',objectFit:'cover',background:'#f8fafc'}}
                  onError={(ev)=>{
                    const el = ev.currentTarget;
                    if(!el.dataset.fallbackTried){
                      el.dataset.fallbackTried='1';
                      const raw = el.getAttribute('data-raw')||'';
                      const idx = raw.indexOf('/uploads/');
                      if(idx>-1){
                        const rel = raw.substring(idx);
                        el.src = `${window.location.origin}${rel}`; // second try absolute
                        return;
                      }
                    }
                    el.style.opacity='0.35';
                    el.style.border='1px solid #f87171';
                    el.alt='yüklenemedi';
                  }}
                />
                <div style={{fontSize:'.55rem',marginTop:4,color:'#64748b',wordBreak:'break-all'}}>{coverImageUrl}</div>
              </div>
            )}
          </div>
          <div className="form-row">
            <label className="form-label-sm">Özet</label>
            <textarea className="form-control" rows={3} value={excerpt} onChange={(e)=>setExcerpt(e.target.value)} placeholder="Kısa özet" />
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
                  <option value="archived">Arşiv</option>
                </select>
              </div>
              <div className="form-row">
                <label className="form-label-sm">Yayın Tarihi</label>
                <input type="datetime-local" className="form-control" value={publishedAt} onChange={(e)=>setPublishedAt(e.target.value)} />
              </div>
            </div>
          </div>

          <div className="panel">
            <div className="panel-header">Öne Çıkan Görsel</div>
            <div className="panel-body">
              <div className="d-flex align-items-center gap-2 mb-2">
                <input id="coverFileInput" type="file" accept="image/*" style={{display:'none'}} onChange={onFileChange} disabled={uploading} />
                <button type="button" className="btn btn-light p-1 d-inline-flex align-items-center justify-content-center" style={{width:34,height:34,borderRadius:8}} onClick={()=>document.getElementById('coverFileInput').click()} disabled={uploading} title="Görsel seç">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><rect x="3" y="3" width="18" height="18" rx="2" ry="2"></rect><circle cx="8.5" cy="8.5" r="1.5"></circle><path d="M21 15l-5-5L5 21" /></svg>
                </button>
                <input className="form-control form-control-sm" placeholder="veya URL yapıştır" value={coverImageUrl} onChange={(e)=>setCoverImageUrl(e.target.value)} />
                {coverImageUrl && <button type="button" className="btn btn-sm btn-outline-danger" onClick={()=>setCoverImageUrl("")}>X</button>}
              </div>
              {uploading && <div className="small text-muted mb-2">Yükleniyor...</div>}
              {coverImageUrl && (
                <div className="mb-2">
                  <img src={coverImageUrl} alt="Kapak" style={{maxWidth:'100%', borderRadius:8, border:'1px solid #e5e7eb'}} />
                  <div className="d-flex gap-2 mt-2">
                    <button type="button" className="btn btn-xs btn-outline-secondary" onClick={()=>navigator.clipboard.writeText(coverImageUrl)}>Kopyala</button>
                    <button type="button" className="btn btn-xs btn-outline-danger" onClick={()=>setCoverImageUrl("")}>Kaldır</button>
                  </div>
                </div>
              )}
              {!coverImageUrl && !uploading && <div className="text-muted" style={{fontSize:'.6rem'}}>Küçük ikon ile yükle veya URL gir.</div>}
            </div>
          </div>

              <div className="panel">
                <div className="panel-header">Etiketler</div>
                <div className="panel-body">
                  <input
                    className="form-control"
                    placeholder="Virgül ya da Enter ile ekleyin"
                    value={tagInput}
                    onChange={(e)=>setTagInput(e.target.value)}
                    onKeyDown={onTagKeyDown}
                  />
                  {tags.length > 0 && (
                    <div className="tag-chips mt-2">
                      {tags.map(t => (
                        <span key={t} className="tag-chip">{t}<button onClick={()=>removeTag(t)} aria-label={`${t} sil`}>×</button></span>
                      ))}
                    </div>
                  )}
                  <div className="mt-3">
                    <div className="form-label-sm mb-1" style={{letterSpacing:0}}>Popüler</div>
                    <div className="d-flex flex-wrap gap-1">
                      {popularTags.map(t => (
                        <button key={t} type="button" className="btn btn-xs btn-light" onClick={()=>addTag(t)}>{t}</button>
                      ))}
                    </div>
                  </div>
                </div>
              </div>

          {isEdit && (
            <div className="panel danger-zone">
              <h4>Risk Bölgesi</h4>
              <button className="btn btn-sm btn-outline-danger w-100" type="button" onClick={()=> window.confirm('Bu yazıyı silmek istediğinize emin misiniz?') && alert('Silme API’si bağlanacak')}>Sil / Çöpe At</button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
