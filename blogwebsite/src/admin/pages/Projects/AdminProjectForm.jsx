import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Save, ArrowLeft, Plus, X, Upload, Link as LinkIcon } from 'lucide-react';
import { projectsAPI } from '../../../services/api';
import '../../../styles/AdminProjectForm.css';

const AdminProjectForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = Boolean(id);

  const [formData, setFormData] = useState({
    title: '',
    shortDescription: '',
    description: '',
    slug: '',
    featuredImage: '',
    projectUrl: '',
    githubUrl: '',
    demoUrl: '',
    type: 'web',
    status: 'in-progress',
    isFeatured: false,
    categoryIds: [],
    tagIds: [],
    newTags: []
  });

  const [categories, setCategories] = useState([]);
  const [tags, setTags] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [newTag, setNewTag] = useState('');

  useEffect(() => {
    loadCategories();
    loadTags();
    if (isEdit) {
      loadProject();
    }
  }, [id, isEdit]);

  useEffect(() => {
    // Auto-generate slug from title
    if (formData.title && !isEdit) {
      const slug = formData.title
        .toLowerCase()
        .replace(/[^a-z0-9\s-]/g, '')
        .replace(/\s+/g, '-')
        .trim();
      setFormData(prev => ({ ...prev, slug }));
    }
  }, [formData.title, isEdit]);

  const loadProject = async () => {
    try {
      setLoading(true);
      const project = await projectsAPI.getProject(id);
      setFormData({
        title: project.title || '',
        shortDescription: project.shortDescription || '',
        description: project.description || '',
        slug: project.slug || '',
        featuredImage: project.featuredImage || '',
        projectUrl: project.projectUrl || '',
        githubUrl: project.githubUrl || '',
        demoUrl: project.demoUrl || '',
        type: project.type || 'web',
        status: project.status || 'in-progress',
        isFeatured: project.isFeatured || false,
        categoryIds: project.projectCategories?.map(pc => pc.categoryId) || [],
        tagIds: project.projectTags?.map(pt => pt.tagId) || [],
        newTags: []
      });
    } catch (err) {
      console.error('Error loading project:', err);
      setError('Failed to load project');
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const response = await projectsAPI.getCategories();
      setCategories(response.data || []);
    } catch (err) {
      console.error('Error loading categories:', err);
    }
  };

  const loadTags = async () => {
    try {
      const response = await projectsAPI.getTags();
      setTags(response.data || []);
    } catch (err) {
      console.error('Error loading tags:', err);
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleCategoryChange = (categoryId) => {
    setFormData(prev => ({
      ...prev,
      categoryIds: prev.categoryIds.includes(categoryId)
        ? prev.categoryIds.filter(id => id !== categoryId)
        : [...prev.categoryIds, categoryId]
    }));
  };

  const handleTagChange = (tagId) => {
    setFormData(prev => ({
      ...prev,
      tagIds: prev.tagIds.includes(tagId)
        ? prev.tagIds.filter(id => id !== tagId)
        : [...prev.tagIds, tagId]
    }));
  };

  const addNewTag = () => {
    if (newTag.trim()) {
      setFormData(prev => ({
        ...prev,
        newTags: [...prev.newTags, newTag.trim()]
      }));
      setNewTag('');
    }
  };

  const removeNewTag = (index) => {
    setFormData(prev => ({
      ...prev,
      newTags: prev.newTags.filter((_, i) => i !== index)
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const projectData = {
        ...formData,
        categoryIds: formData.categoryIds,
        tagIds: formData.tagIds,
        newTags: formData.newTags
      };

      if (isEdit) {
        await projectsAPI.updateProject(id, projectData);
      } else {
        await projectsAPI.createProject(projectData);
      }

      navigate('/admin/projects');
    } catch (err) {
      console.error('Error saving project:', err);
      setError('Failed to save project');
    } finally {
      setLoading(false);
    }
  };

  if (loading && isEdit) {
    return (
      <div className="admin-project-form">
        <div className="loading-state">
          <div className="spinner"></div>
          <p>Loading project...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="admin-project-form">
      {/* Header */}
      <div className="page-header">
        <div className="header-content">
          <button onClick={() => navigate('/admin/projects')} className="back-btn">
            <ArrowLeft className="w-4 h-4" />
            Back to Projects
          </button>
          <h1>{isEdit ? 'Edit Project' : 'Add New Project'}</h1>
        </div>
        <div className="header-actions">
          <button 
            type="submit" 
            form="project-form"
            disabled={loading}
            className="btn btn-primary"
          >
            <Save className="w-4 h-4" />
            {loading ? 'Saving...' : 'Save Project'}
          </button>
        </div>
      </div>

      {/* Error Message */}
      {error && (
        <div className="error-message">
          {error}
        </div>
      )}

      {/* Form */}
      <form id="project-form" onSubmit={handleSubmit} className="project-form">
        <div className="form-grid">
          {/* Basic Information */}
          <div className="form-section">
            <h3>Basic Information</h3>
            
            <div className="form-group">
              <label htmlFor="title">Title *</label>
              <input
                type="text"
                id="title"
                name="title"
                value={formData.title}
                onChange={handleInputChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="slug">Slug *</label>
              <input
                type="text"
                id="slug"
                name="slug"
                value={formData.slug}
                onChange={handleInputChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="shortDescription">Short Description</label>
              <textarea
                id="shortDescription"
                name="shortDescription"
                value={formData.shortDescription}
                onChange={handleInputChange}
                rows="3"
              />
            </div>

            <div className="form-group">
              <label htmlFor="description">Description</label>
              <textarea
                id="description"
                name="description"
                value={formData.description}
                onChange={handleInputChange}
                rows="6"
              />
            </div>
          </div>

          {/* Media & Links */}
          <div className="form-section">
            <h3>Media & Links</h3>
            
            <div className="form-group">
              <label htmlFor="featuredImage">Featured Image URL</label>
              <input
                type="url"
                id="featuredImage"
                name="featuredImage"
                value={formData.featuredImage}
                onChange={handleInputChange}
              />
            </div>

            <div className="form-group">
              <label htmlFor="projectUrl">Project URL</label>
              <input
                type="url"
                id="projectUrl"
                name="projectUrl"
                value={formData.projectUrl}
                onChange={handleInputChange}
              />
            </div>

            <div className="form-group">
              <label htmlFor="githubUrl">GitHub URL</label>
              <input
                type="url"
                id="githubUrl"
                name="githubUrl"
                value={formData.githubUrl}
                onChange={handleInputChange}
              />
            </div>

            <div className="form-group">
              <label htmlFor="demoUrl">Demo URL</label>
              <input
                type="url"
                id="demoUrl"
                name="demoUrl"
                value={formData.demoUrl}
                onChange={handleInputChange}
              />
            </div>
          </div>

          {/* Project Details */}
          <div className="form-section">
            <h3>Project Details</h3>
            
            <div className="form-group">
              <label htmlFor="type">Project Type</label>
              <select
                id="type"
                name="type"
                value={formData.type}
                onChange={handleInputChange}
              >
                <option value="web">Web Application</option>
                <option value="mobile">Mobile Application</option>
                <option value="desktop">Desktop Application</option>
                <option value="library">Library/Package</option>
                <option value="other">Other</option>
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="status">Status</label>
              <select
                id="status"
                name="status"
                value={formData.status}
                onChange={handleInputChange}
              >
                <option value="in-progress">In Progress</option>
                <option value="completed">Completed</option>
                <option value="archived">Archived</option>
              </select>
            </div>

            <div className="form-group checkbox-group">
              <label>
                <input
                  type="checkbox"
                  name="isFeatured"
                  checked={formData.isFeatured}
                  onChange={handleInputChange}
                />
                Featured Project
              </label>
            </div>
          </div>

          {/* Categories */}
          <div className="form-section">
            <h3>Categories</h3>
            <div className="checkbox-list">
              {categories.map(category => (
                <label key={category.id} className="checkbox-item">
                  <input
                    type="checkbox"
                    checked={formData.categoryIds.includes(category.id)}
                    onChange={() => handleCategoryChange(category.id)}
                  />
                  {category.name}
                </label>
              ))}
            </div>
          </div>

          {/* Tags */}
          <div className="form-section">
            <h3>Tags</h3>
            
            {/* Existing Tags */}
            <div className="checkbox-list">
              {tags.map(tag => (
                <label key={tag.id} className="checkbox-item">
                  <input
                    type="checkbox"
                    checked={formData.tagIds.includes(tag.id)}
                    onChange={() => handleTagChange(tag.id)}
                  />
                  {tag.name}
                </label>
              ))}
            </div>

            {/* Add New Tags */}
            <div className="new-tag-section">
              <h4>Add New Tags</h4>
              <div className="tag-input">
                <input
                  type="text"
                  value={newTag}
                  onChange={(e) => setNewTag(e.target.value)}
                  placeholder="Enter tag name"
                  onKeyPress={(e) => e.key === 'Enter' && (e.preventDefault(), addNewTag())}
                />
                <button type="button" onClick={addNewTag} className="btn-sm">
                  <Plus className="w-4 h-4" />
                </button>
              </div>
              
              {/* New Tags List */}
              {formData.newTags.length > 0 && (
                <div className="new-tags-list">
                  {formData.newTags.map((tag, index) => (
                    <span key={index} className="tag-item">
                      {tag}
                      <button
                        type="button"
                        onClick={() => removeNewTag(index)}
                        className="remove-tag"
                      >
                        <X className="w-3 h-3" />
                      </button>
                    </span>
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default AdminProjectForm;