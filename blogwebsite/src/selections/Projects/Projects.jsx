import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { 
  getPublicProjects, 
  getPublicCategories,
  getProjectTags
} from '../../services/api';
import ProjectGrid from '../../components/ProjectGrid';
import { 
  PROJECT_TYPES, 
  PROJECT_STATUSES, 
  PROJECT_GRID_SETTINGS,
  getProjectTypeLabel,
  getProjectStatusLabel
} from '../../constants/projects';
import '../../styles/Projects.css';

const Projects = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [projects, setProjects] = useState([]);
  const [categories, setCategories] = useState([]);
  const [tags, setTags] = useState([]);
  const [loading, setLoading] = useState(true);
  const [total, setTotal] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [showFilters, setShowFilters] = useState(false);

  // Filter states
  const [filters, setFilters] = useState({
    search: searchParams.get('search') || '',
    categoryId: searchParams.get('category') || '',
    type: searchParams.get('type') || '',
    status: searchParams.get('status') || '',
    featured: searchParams.get('featured') === 'true'
  });

  // Load projects
  const loadProjects = async (page = 1) => {
    try {
      setLoading(true);
      const response = await getPublicProjects({
        page,
        pageSize: PROJECT_GRID_SETTINGS.DEFAULT_PAGE_SIZE,
        ...filters
      });
      
      setProjects(response.items || []);
      setTotal(response.total || 0);
      setCurrentPage(page);
    } catch (error) {
      console.error('Error loading projects:', error);
      setProjects([]);
    } finally {
      setLoading(false);
    }
  };

  // Load filter options
  const loadFilterOptions = async () => {
    try {
      const [categoriesResponse, tagsResponse] = await Promise.all([
        getPublicCategories('project'),
        getProjectTags({ limit: 50 })
      ]);
      
      setCategories(categoriesResponse || []);
      setTags(tagsResponse || []);
    } catch (error) {
      console.error('Error loading filter options:', error);
    }
  };

  // Update URL params when filters change
  const updateFilters = (newFilters) => {
    const updatedFilters = { ...filters, ...newFilters };
    setFilters(updatedFilters);
    
    const params = new URLSearchParams();
    Object.entries(updatedFilters).forEach(([key, value]) => {
      if (value && value !== '') {
        params.set(key === 'categoryId' ? 'category' : key, value);
      }
    });
    
    setSearchParams(params);
    setCurrentPage(1);
  };

  // Clear all filters
  const clearFilters = () => {
    setFilters({
      search: '',
      categoryId: '',
      type: '',
      status: '',
      featured: false
    });
    setSearchParams({});
    setCurrentPage(1);
  };

  // Load data on mount and filter changes
  useEffect(() => {
    loadProjects(1);
  }, [filters]);

  useEffect(() => {
    loadFilterOptions();
  }, []);

  const totalPages = Math.ceil(total / PROJECT_GRID_SETTINGS.DEFAULT_PAGE_SIZE);
  const hasFilters = Object.values(filters).some(value => value && value !== '');

  return (
    <div className="projects-page">
      {/* Hero Section */}
      <section className="projects-hero">
        <div className="container">
          <div className="hero-content">
            <h1>My Projects</h1>
            <p>Explore my portfolio of web applications, mobile apps, and development projects</p>
            <div className="hero-stats">
              <div className="stat">
                <span className="stat-number">{total}</span>
                <span className="stat-label">Projects</span>
              </div>
              <div className="stat">
                <span className="stat-number">{categories.length}</span>
                <span className="stat-label">Categories</span>
              </div>
              <div className="stat">
                <span className="stat-number">{tags.length}</span>
                <span className="stat-label">Technologies</span>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Filters Section */}
      <section className="projects-filters">
        <div className="container">
          <div className="filters-header">
            <h2>Filter Projects</h2>
            <button 
              className="toggle-filters-btn"
              onClick={() => setShowFilters(!showFilters)}
            >
              {showFilters ? '🔽' : '🔼'} {showFilters ? 'Hide' : 'Show'} Filters
            </button>
          </div>

          {showFilters && (
            <div className="filters-content">
              {/* Search */}
              <div className="filter-group">
                <label>Search Projects</label>
                <input
                  type="text"
                  placeholder="Search by title or description..."
                  value={filters.search}
                  onChange={(e) => updateFilters({ search: e.target.value })}
                  className="search-input"
                />
              </div>

              {/* Category Filter */}
              <div className="filter-group">
                <label>Category</label>
                <select
                  value={filters.categoryId}
                  onChange={(e) => updateFilters({ categoryId: e.target.value })}
                  className="filter-select"
                >
                  <option value="">All Categories</option>
                  {categories.map(category => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </select>
              </div>

              {/* Type Filter */}
              <div className="filter-group">
                <label>Project Type</label>
                <select
                  value={filters.type}
                  onChange={(e) => updateFilters({ type: e.target.value })}
                  className="filter-select"
                >
                  <option value="">All Types</option>
                  {Object.values(PROJECT_TYPES).map(type => (
                    <option key={type.value} value={type.value}>
                      {type.icon} {type.label}
                    </option>
                  ))}
                </select>
              </div>

              {/* Status Filter */}
              <div className="filter-group">
                <label>Status</label>
                <select
                  value={filters.status}
                  onChange={(e) => updateFilters({ status: e.target.value })}
                  className="filter-select"
                >
                  <option value="">All Statuses</option>
                  {Object.values(PROJECT_STATUSES).map(status => (
                    <option key={status.value} value={status.value}>
                      {status.icon} {status.label}
                    </option>
                  ))}
                </select>
              </div>

              {/* Featured Toggle */}
              <div className="filter-group">
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    checked={filters.featured}
                    onChange={(e) => updateFilters({ featured: e.target.checked })}
                  />
                  ⭐ Featured Projects Only
                </label>
              </div>

              {/* Clear Filters */}
              {hasFilters && (
                <div className="filter-group">
                  <button onClick={clearFilters} className="clear-filters-btn">
                    🗑️ Clear All Filters
                  </button>
                </div>
              )}
            </div>
          )}

          {/* Active Filters Display */}
          {hasFilters && (
            <div className="active-filters">
              <span className="active-filters-label">Active Filters:</span>
              {filters.search && (
                <span className="active-filter">
                  Search: "{filters.search}"
                  <button onClick={() => updateFilters({ search: '' })}>×</button>
                </span>
              )}
              {filters.categoryId && (
                <span className="active-filter">
                  Category: {categories.find(c => c.id === filters.categoryId)?.name}
                  <button onClick={() => updateFilters({ categoryId: '' })}>×</button>
                </span>
              )}
              {filters.type && (
                <span className="active-filter">
                  Type: {getProjectTypeLabel(parseInt(filters.type))}
                  <button onClick={() => updateFilters({ type: '' })}>×</button>
                </span>
              )}
              {filters.status && (
                <span className="active-filter">
                  Status: {getProjectStatusLabel(parseInt(filters.status))}
                  <button onClick={() => updateFilters({ status: '' })}>×</button>
                </span>
              )}
              {filters.featured && (
                <span className="active-filter">
                  Featured Only
                  <button onClick={() => updateFilters({ featured: false })}>×</button>
                </span>
              )}
            </div>
          )}
        </div>
      </section>

      {/* Projects Grid */}
      <section className="projects-grid-section">
        <div className="container">
          <div className="grid-header">
            <h3>
              {hasFilters ? 'Filtered Results' : 'All Projects'} 
              <span className="count">({total} project{total !== 1 ? 's' : ''})</span>
            </h3>
          </div>

          <ProjectGrid 
            projects={projects}
            loading={loading}
            className="projects-main-grid"
          />

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="pagination">
              <button 
                onClick={() => loadProjects(currentPage - 1)}
                disabled={currentPage === 1}
                className="pagination-btn"
              >
                ← Previous
              </button>
              
              <div className="pagination-info">
                Page {currentPage} of {totalPages}
              </div>
              
              <button 
                onClick={() => loadProjects(currentPage + 1)}
                disabled={currentPage === totalPages}
                className="pagination-btn"
              >
                Next →
              </button>
            </div>
          )}
        </div>
      </section>
    </div>
  );
};

export default Projects;