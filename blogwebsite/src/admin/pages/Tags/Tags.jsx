import { useState, useEffect } from 'react';
import { Plus, Edit2, Trash2, Tag, Hash } from 'lucide-react';
import { API_BASE_URL } from '../../../services/api.js';
import '../../../styles/AdminBase.css';
import '../../../styles/AdminNews.css';
import '../../../styles/AdminCategories.css';

const CONTENT_TYPES = [
  { value: 'news', label: 'Haberler', color: '#2563eb' },
  { value: 'blog', label: 'Blog', color: '#7c3aed' },
  { value: 'project', label: 'Projeler', color: '#059669' }
];

const DEFAULT_COLORS = [
  '#ef4444', '#f97316', '#eab308', '#22c55e', '#06b6d4', 
  '#3b82f6', '#8b5cf6', '#ec4899', '#64748b', '#374151'
];

export default function Tags() {
  const [tags, setTags] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedContentType, setSelectedContentType] = useState('all');
  const [showModal, setShowModal] = useState(false);
  const [editingTag, setEditingTag] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    contentType: 'project',
    color: '#3b82f6',
    icon: '',
    isActive: true,
    isFeatured: false
  });

  useEffect(() => {
    document.body.classList.add('admin-theme');
    loadTags();
    return () => document.body.classList.remove('admin-theme');
  }, []);

  const loadTags = async () => {
    try {
      setLoading(true);
      const response = await fetch(`${API_BASE_URL}/api/admin/tags`, {
        headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
      });
      if (response.ok) {
        const data = await response.json();
        setTags(data || []);
      }
    } catch (error) {
      console.error('Error loading tags:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const url = editingTag 
        ? `${API_BASE_URL}/api/admin/tags/${editingTag.id}`
        : `${API_BASE_URL}/api/admin/tags`;
      
      const method = editingTag ? 'PUT' : 'POST';
      
      const response = await fetch(url, {
        method,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(formData)
      });

      if (response.ok) {
        await loadTags();
        closeModal();
      }
    } catch (error) {
      console.error('Error saving tag:', error);
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu etiketi silmek istediğinizden emin misiniz?')) return;
    
    try {
      const response = await fetch(`${API_BASE_URL}/api/admin/tags/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
      });
      
      if (response.ok) {
        await loadTags();
      }
    } catch (error) {
      console.error('Error deleting tag:', error);
    }
  };

  const openModal = (tag = null) => {
    if (tag) {
      setEditingTag(tag);
      setFormData({
        name: tag.name,
        description: tag.description || '',
        contentType: tag.contentType || 'project',
        color: tag.color || '#3b82f6',
        icon: tag.icon || '',
        isActive: tag.isActive,
        isFeatured: tag.isFeatured
      });
    } else {
      setEditingTag(null);
      setFormData({
        name: '',
        description: '',
        contentType: 'project',
        color: '#3b82f6',
        icon: '',
        isActive: true,
        isFeatured: false
      });
    }
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingTag(null);
  };

  const filteredTags = selectedContentType === 'all' 
    ? tags 
    : tags.filter(tag => tag.contentType === selectedContentType);

  const getContentTypeInfo = (type) => {
    return CONTENT_TYPES.find(t => t.value === type) || { label: type, color: '#64748b' };
  };

  return (
    <div className="admin-shell">
      <div className="admin-toolbar">
        <div className="admin-toolbar-title">Etiketler</div>
        <div className="admin-toolbar-filters">
          <select 
            value={selectedContentType} 
            onChange={(e) => setSelectedContentType(e.target.value)}
            className="admin-select"
          >
            <option value="all">Tüm Etiketler</option>
            {CONTENT_TYPES.map(type => (
              <option key={type.value} value={type.value}>{type.label}</option>
            ))}
          </select>
        </div>
        <div className="spacer" />
        <button 
          className="admin-btn admin-btn-success" 
          onClick={() => openModal()}
        >
          <Plus size={16} /> Yeni Etiket
        </button>
      </div>

      {loading ? (
        <div className="admin-loading">Etiketler yükleniyor...</div>
      ) : (
        <div className="admin-content">
          <div className="categories-grid">
            {filteredTags.map(tag => {
              const typeInfo = getContentTypeInfo(tag.contentType);
              return (
                <div key={tag.id} className="category-card">
                  <div className="category-header">
                    <div className="category-color" style={{ backgroundColor: tag.color }}></div>
                    <div className="category-info">
                      <h3 className="category-name">
                        <Hash size={16} style={{ marginRight: '4px' }} />
                        {tag.name}
                      </h3>
                      <div className="category-meta">
                        <span 
                          className="category-type"
                          style={{ color: typeInfo.color }}
                        >
                          {typeInfo.label}
                        </span>
                      </div>
                      {tag.description && (
                        <p className="category-description">{tag.description}</p>
                      )}
                    </div>
                    <div className="category-badges">
                      {tag.isFeatured && (
                        <span className="badge badge-featured">
                          <Tag size={12} /> Öne Çıkan
                        </span>
                      )}
                      <span className={`badge badge-${tag.isActive ? 'active' : 'inactive'}`}>
                        {tag.isActive ? 'Aktif' : 'Pasif'}
                      </span>
                    </div>
                  </div>
                  
                  <div className="category-stats">
                    <span className="stat">
                      <strong>{tag.projectCount || 0}</strong> Proje
                    </span>
                  </div>

                  <div className="category-actions">
                    <button 
                      className="admin-btn admin-btn-sm admin-btn-outline"
                      onClick={() => openModal(tag)}
                    >
                      <Edit2 size={14} /> Düzenle
                    </button>
                    <button 
                      className="admin-btn admin-btn-sm admin-btn-danger"
                      onClick={() => handleDelete(tag.id)}
                    >
                      <Trash2 size={14} />
                    </button>
                  </div>
                </div>
              );
            })}
          </div>

          {filteredTags.length === 0 && (
            <div className="admin-empty">
              <p>Henüz etiket bulunmuyor.</p>
              <button 
                className="admin-btn admin-btn-primary"
                onClick={() => openModal()}
              >
                İlk etiketi oluştur
              </button>
            </div>
          )}
        </div>
      )}

      {/* Modal */}
      {showModal && (
        <div className="admin-modal-overlay" onClick={closeModal}>
          <div className="admin-modal" onClick={e => e.stopPropagation()}>
            <div className="admin-modal-header">
              <h2>{editingTag ? 'Etiket Düzenle' : 'Yeni Etiket'}</h2>
              <button className="admin-modal-close" onClick={closeModal}>×</button>
            </div>
            
            <form onSubmit={handleSubmit} className="admin-modal-body">
              <div className="form-grid">
                <div className="form-group">
                  <label>Etiket Adı *</label>
                  <input
                    type="text"
                    value={formData.name}
                    onChange={(e) => setFormData({...formData, name: e.target.value})}
                    required
                    className="form-control"
                    placeholder="Örn: React, JavaScript, API"
                  />
                </div>

                <div className="form-group">
                  <label>İçerik Türü *</label>
                  <select
                    value={formData.contentType}
                    onChange={(e) => setFormData({...formData, contentType: e.target.value})}
                    className="form-control"
                  >
                    {CONTENT_TYPES.map(type => (
                      <option key={type.value} value={type.value}>{type.label}</option>
                    ))}
                  </select>
                </div>

                <div className="form-group span-2">
                  <label>Açıklama</label>
                  <textarea
                    value={formData.description}
                    onChange={(e) => setFormData({...formData, description: e.target.value})}
                    className="form-control"
                    rows="3"
                    placeholder="Etiket hakkında kısa açıklama"
                  />
                </div>

                <div className="form-group">
                  <label>Renk</label>
                  <div className="color-picker">
                    <input
                      type="color"
                      value={formData.color}
                      onChange={(e) => setFormData({...formData, color: e.target.value})}
                      className="color-input"
                    />
                    <div className="color-presets">
                      {DEFAULT_COLORS.map(color => (
                        <button
                          key={color}
                          type="button"
                          className="color-preset"
                          style={{ backgroundColor: color }}
                          onClick={() => setFormData({...formData, color})}
                        />
                      ))}
                    </div>
                  </div>
                </div>

                <div className="form-group">
                  <label>İkon</label>
                  <input
                    type="text"
                    value={formData.icon}
                    onChange={(e) => setFormData({...formData, icon: e.target.value})}
                    className="form-control"
                    placeholder="Örn: code, database, globe"
                  />
                </div>

                <div className="form-group span-2">
                  <div className="checkbox-group">
                    <label className="checkbox-label">
                      <input
                        type="checkbox"
                        checked={formData.isActive}
                        onChange={(e) => setFormData({...formData, isActive: e.target.checked})}
                      />
                      Aktif
                    </label>
                    <label className="checkbox-label">
                      <input
                        type="checkbox"
                        checked={formData.isFeatured}
                        onChange={(e) => setFormData({...formData, isFeatured: e.target.checked})}
                      />
                      Öne Çıkan
                    </label>
                  </div>
                </div>
              </div>

              <div className="admin-modal-footer">
                <button type="button" className="admin-btn admin-btn-outline" onClick={closeModal}>
                  İptal
                </button>
                <button type="submit" className="admin-btn admin-btn-success">
                  {editingTag ? 'Güncelle' : 'Oluştur'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}