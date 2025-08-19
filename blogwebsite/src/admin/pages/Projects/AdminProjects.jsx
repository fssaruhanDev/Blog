import React, { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, Eye, Search, Filter, CheckCircle, Clock, Archive, AlertCircle } from 'lucide-react';
import { Link } from 'react-router-dom';
import { projectsAPI } from '../../../services/api';
import '../../../styles/AdminProjects.css';

const AdminProjects = () => {
  const [projects, setProjects] = useState([]);
  const [categories, setCategories] = useState([]);
  const [tags, setTags] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedStatus, setSelectedStatus] = useState('all');
  const [selectedCategory, setSelectedCategory] = useState('all');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    loadProjects();
    loadCategories();
    loadTags();
  }, [currentPage, searchTerm, selectedStatus, selectedCategory]);

  const loadProjects = async () => {
    try {
      setLoading(true);
      const params = {
        page: currentPage,
        pageSize: 10,
        search: searchTerm,
        status: selectedStatus !== 'all' ? selectedStatus : undefined,
        categoryId: selectedCategory !== 'all' ? selectedCategory : undefined
      };
      
      const response = await projectsAPI.getProjects(params);
      setProjects(response.data || []);
      setTotalPages(response.totalPages || 1);
    } catch (err) {
      console.error('Error loading projects:', err);
      setError('Failed to load projects');
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

  const handleDelete = async (projectId) => {
    if (window.confirm('Are you sure you want to delete this project?')) {
      try {
        await projectsAPI.deleteProject(projectId);
        loadProjects(); // Reload the list
      } catch (err) {
        console.error('Error deleting project:', err);
        alert('Failed to delete project');
      }
    }
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case 'completed': return <CheckCircle className="w-4 h-4 text-green-500" />;
      case 'in-progress': return <Clock className="w-4 h-4 text-yellow-500" />;
      case 'archived': return <Archive className="w-4 h-4 text-gray-500" />;
      default: return <AlertCircle className="w-4 h-4 text-blue-500" />;
    }
  };

  const getStatusLabel = (status) => {
    switch (status) {
      case 'completed': return 'Completed';
      case 'in-progress': return 'In Progress';
      case 'archived': return 'Archived';
      default: return 'Unknown';
    }
  };

  if (loading && projects.length === 0) {
    return (
      <div className="admin-projects">
        <div className="loading-state">
          <div className="spinner"></div>
          <p>Loading projects...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="admin-projects">
      {/* Header */}
      <div className="page-header">
        <div className="header-content">
          <h1>Projects Management</h1>
          <p>Manage your portfolio projects</p>
        </div>
        <div className="header-actions">
          <Link to="/admin/projects/new" className="btn btn-primary">
            <Plus className="w-4 h-4" />
            Add New Project
          </Link>
        </div>
      </div>

      {/* Filters */}
      <div className="filters-section">
        <div className="search-box">
          <Search className="w-4 h-4" />
          <input
            type="text"
            placeholder="Search projects..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        
        <div className="filter-controls">
          <select
            value={selectedStatus}
            onChange={(e) => setSelectedStatus(e.target.value)}
          >
            <option value="all">All Status</option>
            <option value="completed">Completed</option>
            <option value="in-progress">In Progress</option>
            <option value="archived">Archived</option>
          </select>

          <select
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
          >
            <option value="all">All Categories</option>
            {categories.map(category => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
        </div>
      </div>

      {/* Error Message */}
      {error && (
        <div className="error-message">
          <AlertCircle className="w-4 h-4" />
          {error}
        </div>
      )}

      {/* Projects Table */}
      <div className="projects-table">
        <table>
          <thead>
            <tr>
              <th>Project</th>
              <th>Status</th>
              <th>Categories</th>
              <th>Tags</th>
              <th>Views</th>
              <th>Featured</th>
              <th>Created</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {projects.length === 0 ? (
              <tr>
                <td colSpan="8" className="no-data">
                  No projects found
                </td>
              </tr>
            ) : (
              projects.map(project => (
                <tr key={project.id}>
                  <td className="project-info">
                    <div className="project-details">
                      <h4>{project.title}</h4>
                      <p>{project.shortDescription}</p>
                    </div>
                  </td>
                  <td>
                    <div className="status-badge">
                      {getStatusIcon(project.status)}
                      {getStatusLabel(project.status)}
                    </div>
                  </td>
                  <td>
                    <div className="categories">
                      {project.projectCategories?.slice(0, 2).map((cat, index) => (
                        <span key={index} className="category-tag">
                          {cat.category?.name || `Cat ${cat.categoryId}`}
                        </span>
                      ))}
                      {project.projectCategories?.length > 2 && (
                        <span className="more-count">
                          +{project.projectCategories.length - 2}
                        </span>
                      )}
                    </div>
                  </td>
                  <td>
                    <div className="tags">
                      {project.projectTags?.slice(0, 2).map((tag, index) => (
                        <span key={index} className="tag-item">
                          #{tag.tag?.name || tag.tagName || `Tag ${tag.tagId}`}
                        </span>
                      ))}
                      {project.projectTags?.length > 2 && (
                        <span className="more-count">
                          +{project.projectTags.length - 2}
                        </span>
                      )}
                    </div>
                  </td>
                  <td>{project.viewCount || 0}</td>
                  <td>
                    <span className={`featured-badge ${project.isFeatured ? 'featured' : ''}`}>
                      {project.isFeatured ? '⭐' : '—'}
                    </span>
                  </td>
                  <td>
                    {new Date(project.createdDate).toLocaleDateString()}
                  </td>
                  <td className="actions">
                    <Link to={`/projects/${project.slug}`} className="action-btn view">
                      <Eye className="w-4 h-4" />
                    </Link>
                    <Link to={`/admin/projects/${project.id}/edit`} className="action-btn edit">
                      <Edit className="w-4 h-4" />
                    </Link>
                    <button 
                      onClick={() => handleDelete(project.id)}
                      className="action-btn delete"
                    >
                      <Trash2 className="w-4 h-4" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="pagination">
          <button 
            disabled={currentPage === 1}
            onClick={() => setCurrentPage(currentPage - 1)}
          >
            Previous
          </button>
          <span>Page {currentPage} of {totalPages}</span>
          <button 
            disabled={currentPage === totalPages}
            onClick={() => setCurrentPage(currentPage + 1)}
          >
            Next
          </button>
        </div>
      )}
    </div>
  );
};

export default AdminProjects;