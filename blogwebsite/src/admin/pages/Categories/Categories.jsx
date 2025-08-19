import { useState, useEffect } from 'react';
import { Plus, Edit2, Trash2, Eye, EyeOff, Star, StarOff, Move } from 'lucide-react';
import { getCategories, createCategory, updateCategory, deleteCategory } from '../../../services/api.js';
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

export default function Categories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedContentType, setSelectedContentType] = useState('all');
  const [showModal, setShowModal] = useState(false);
  const [editingCategory, setEditingCategory] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    contentType: 'news',
    parentId: null,
    color: '#3b82f6',
    icon: '',
    isActive: true,
    isFeatured: false,
    order: 0
  });

  useEffect(() => {
    document.body.classList.add('admin-theme');
    loadCategories();
    return () => document.body.classList.remove('admin-theme');
  }, []);

  const loadCategories = async () => {
    try {
      setLoading(true);
      const data = await getCategories();
      setCategories(data || []);
    } catch (error) {
      console.error('Error loading categories:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingCategory) {
        await updateCategory(editingCategory.id, formData);
      } else {
        await createCategory(formData);
      }
      await loadCategories();
      closeModal();
    } catch (error) {
      console.error('Error saving category:', error);
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu kategoriyi silmek istediğinizden emin misiniz?')) return;
    
    try {
      await deleteCategory(id);
      await loadCategories();
    } catch (error) {
      console.error('Error deleting category:', error);
    }
  };

  const openModal = (category = null) => {
    if (category) {
      setEditingCategory(category);
      setFormData({
        name: category.name,
        description: category.description || '',
        contentType: category.contentType,
        parentId: category.parentId,
        color: category.color || '#3b82f6',
        icon: category.icon || '',
        isActive: category.isActive,
        isFeatured: category.isFeatured,
        order: category.order
      });
    } else {
      setEditingCategory(null);
      setFormData({
        name: '',
        description: '',
        contentType: 'news',
        parentId: null,
        color: '#3b82f6',
        icon: '',
        isActive: true,
        isFeatured: false,
        order: 0
      });
    }
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingCategory(null);
  };

  const filteredCategories = selectedContentType === 'all' 
    ? categories 
    : categories.filter(cat => cat.contentType === selectedContentType);

  const getContentTypeInfo = (type) => {
    return CONTENT_TYPES.find(t => t.value === type) || { label: type, color: '#64748b' };
  };

  return (
    <div className="admin-shell">
      <div className="admin-toolbar">
        <div className="admin-toolbar-title">Kategoriler</div>
        <div className="admin-toolbar-filters">
          <select 
            value={selectedContentType} 
            onChange={(e) => setSelectedContentType(e.target.value)}
            className="admin-select"
          >
            <option value="all">Tüm Kategoriler</option>
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
          <Plus size={16} /> Yeni Kategori
        </button>
      </div>

      {loading ? (
        <div className="admin-loading">Kategoriler yükleniyor...</div>
      ) : (
        <div className="admin-content">
          <div className="categories-grid">
            {filteredCategories.map(category => {
              const typeInfo = getContentTypeInfo(category.contentType);
              return (
                <div key={category.id} className="category-card">
                  <div className="category-header">
                    <div className="category-color" style={{ backgroundColor: category.color }}></div>
                    <div className="category-info">
                      <h3 className="category-name">{category.name}</h3>
                      <div className="category-meta">
                        <span 
                          className="category-type"
                          style={{ color: typeInfo.color }}
                        >
                          {typeInfo.label}
                        </span>
                        {category.parentName && (
                          <span className="category-parent">Alt kategori: {category.parentName}</span>
                        )}
                      </div>
                      {category.description && (
                        <p className="category-description">{category.description}</p>
                      )}
                    </div>
                    <div className="category-badges">
                      {category.isFeatured && (
                        <span className="badge badge-featured">
                          <Star size={12} /> Öne Çıkan
                        </span>
                      )}
                      <span className={`badge badge-${category.isActive ? 'active' : 'inactive'}`}>
                        {category.isActive ? 'Aktif' : 'Pasif'}
                      </span>
                    </div>
                  </div>
                  
                  <div className="category-stats">
                    <span className="stat">
                      <strong>{category.postCount}</strong> İçerik
                    </span>
                    {category.childrenCount > 0 && (
                      <span className="stat">
                        <strong>{category.childrenCount}</strong> Alt Kategori
                      </span>
                    )}
                    <span className="stat">
                      Sıra: <strong>{category.order}</strong>
                    </span>
                  </div>

                  <div className="category-actions">
                    <button 
                      className="admin-btn admin-btn-sm admin-btn-outline"
                      onClick={() => openModal(category)}
                    >
                      <Edit2 size={14} /> Düzenle
                    </button>
                    <button 
                      className="admin-btn admin-btn-sm admin-btn-danger"
                      onClick={() => handleDelete(category.id)}
                    >
                      <Trash2 size={14} />
                    </button>
                  </div>
                </div>
              );
            })}
          </div>

          {filteredCategories.length === 0 && (
            <div className="admin-empty">
              <p>Henüz kategori bulunmuyor.</p>
              <button 
                className="admin-btn admin-btn-primary"
                onClick={() => openModal()}
              >
                İlk kategoriyi oluştur
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
              <h2>{editingCategory ? 'Kategori Düzenle' : 'Yeni Kategori'}</h2>
              <button className="admin-modal-close" onClick={closeModal}>×</button>
            </div>
            
            <form onSubmit={handleSubmit} className="admin-modal-body">
              <div className="form-grid">
                <div className="form-group">
                  <label>Kategori Adı *</label>
                  <input
                    type="text"
                    value={formData.name}
                    onChange={(e) => setFormData({...formData, name: e.target.value})}
                    required
                    className="form-control"
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
                  <label>Sıra</label>
                  <input
                    type="number"
                    value={formData.order}
                    onChange={(e) => setFormData({...formData, order: parseInt(e.target.value) || 0})}
                    className="form-control"
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
                  {editingCategory ? 'Güncelle' : 'Oluştur'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}