import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createPost, getPost, updatePost } from "../../../services/api";
import CkSuperEditor from '../../../editor/CkSuperEditor';
// Prism languages (popular set)
import '../../../styles/Blog.css';

export default function BlogEditor() {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = !!id && id !== "new";
  const [title, setTitle] = useState("");
  const [excerpt, setExcerpt] = useState("");
  const [content, setContent] = useState("");
  const [status, setStatus] = useState("draft");
  const [publishedAt, setPublishedAt] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  // Using CKEditor only
  const [useCk] = useState(true);
  const [tags, setTags] = useState([]);
  const [tagInput, setTagInput] = useState("");
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
        setTitle(p.title || p.Title || "");
        setExcerpt(p.excerpt || p.Excerpt || "");
  const html = p.content || p.Content || "";
  setContent(html);
        setStatus(p.status || p.Status || "draft");
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
      const payload = {
        title,
        excerpt,
        content,
        status,
        publishedAt: publishedAt ? new Date(publishedAt).toISOString() : null,
        // TODO: API desteklediğinde tags & categories gönderilecek
        // tags,
      };
      if (isEdit) await updatePost(id, payload);
      else await createPost(payload);
      navigate("/admin/blog");
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

  return (
    <div className="card">
      <div className="card-body">
        <div className="d-flex align-items-center mb-3">
          <h6 className="mb-0 flex-grow-1">{isEdit ? "Blog Düzenle" : "Yeni Blog"}</h6>
          <button className="btn btn-secondary me-2" onClick={() => navigate(-1)} disabled={loading}>Geri</button>
          <button className="btn btn-primary" onClick={onSave} disabled={loading}>{isEdit ? "Güncelle" : "Kaydet"}</button>
        </div>
        {error && <div className="alert alert-danger py-2">{error}</div>}

        <div className="editor-layout">
          {/* Left: Main editor */}
          <div className="editor-main">
            <div className="mb-3">
              <label className="form-label">Başlık</label>
              <input className="form-control" value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Başlık" />
            </div>

            <div className="mb-2 d-flex align-items-center">
              <label className="form-label me-2 mb-0">İçerik</label>
            </div>
            {/* TipTap toolbar removed */}

            {/* Editor */}
            <div className="editor-surface mb-3">
              <CkSuperEditor value={content} onChange={setContent} />
            </div>

            {/* Excerpt under editor (optional) */}
            <div className="mb-3">
              <label className="form-label">Özet</label>
              <textarea className="form-control" rows={3} value={excerpt} onChange={(e) => setExcerpt(e.target.value)} placeholder="Kısa özet" />
            </div>
            {/* TipTap toggle removed */}
          </div>

          {/* Right: Sidebar */}
          <aside className="editor-sidebar">
            {/* Publish settings */}
            <div className="sidebar-section">
              <div className="sidebar-section-header">Yayın Ayarları</div>
              <div className="sidebar-section-body">
                <div className="mb-2">
                  <label className="form-label">Durum</label>
                  <select className="form-select" value={status} onChange={(e) => setStatus(e.target.value)}>
                    <option value="draft">Taslak</option>
                    <option value="published">Yayınlandı</option>
                    <option value="archived">Arşiv</option>
                  </select>
                </div>
                <div>
                  <label className="form-label">Yayın Tarihi</label>
                  <input type="datetime-local" className="form-control" value={publishedAt} onChange={(e) => setPublishedAt(e.target.value)} />
                </div>
              </div>
            </div>

            {/* Categories placeholder */}
            <div className="sidebar-section">
              <div className="sidebar-section-header">Kategoriler</div>
              <div className="sidebar-section-body text-muted small">
                Kategoriler yakında eklenecek.
              </div>
            </div>

            {/* Tags section */}
            <div className="sidebar-section">
              <div className="sidebar-section-header">Etiketler</div>
              <div className="sidebar-section-body">
                <div className="tag-input-wrap">
                  <input
                    className="form-control tag-input"
                    placeholder="Virgül ya da Enter tuşuyla ayırın"
                    value={tagInput}
                    onChange={(e) => setTagInput(e.target.value)}
                    onKeyDown={onTagKeyDown}
                  />
                </div>
                {tags.length > 0 && (
                  <div className="tag-list mt-2">
                    {tags.map((t) => (
                      <span key={t} className="tag-chip">
                        {t}
                        <button type="button" className="tag-remove" onClick={() => removeTag(t)}>×</button>
                      </span>
                    ))}
                  </div>
                )}
                <div className="popular-tags mt-3">
                  <div className="text-muted small mb-1">En çok kullanılan</div>
                  <div className="d-flex flex-wrap gap-1">
                    {popularTags.map((t) => (
                      <button key={t} type="button" className="btn btn-sm btn-light" onClick={() => addTag(t)}>{t}</button>
                    ))}
                  </div>
                </div>
              </div>
            </div>

            {/* Danger zone */}
            {isEdit && (
              <div className="sidebar-section">
                <div className="sidebar-section-body">
                  <button type="button" className="btn btn-outline-danger w-100" onClick={() => window.confirm('Bu yazıyı silmek istediğinize emin misiniz?') && alert('Silme API’si bağlanacak')}>Çöpe at</button>
                </div>
              </div>
            )}

          </aside>
        </div>
      </div>
    </div>
  );
}
