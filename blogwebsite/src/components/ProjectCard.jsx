import React from 'react';
import { Link } from 'react-router-dom';
import { getProjectTypeIcon, getProjectStatusColor, getProjectStatusIcon, getProjectStatusLabel } from '../constants/projects';
import { resolveMediaUrl } from '../services/api';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:52888";

const ProjectCard = ({ 
  project, 
  showStatus = true, 
  showType = true, 
  showTags = true,
  showCategories = true,
  className = "" 
}) => {
  const {
    id,
    title,
    shortDescription,
    description,
    slug,
    featuredImage,
    projectUrl,
    githubUrl,
    demoUrl,
    type,
    status,
    isFeatured,
    viewCount,
    likeCount,
    projectCategories = [],
    projectTags = []
  } = project;

  const imageUrl = resolveMediaUrl(featuredImage);
  const hasImage = imageUrl && imageUrl !== '' && imageUrl !== API_BASE_URL;

  return (
    <div className={`project-card ${isFeatured ? 'featured' : ''} ${className}`}>
      {/* Featured Badge */}
      {isFeatured && (
        <div className="featured-badge">
          ⭐ Featured
        </div>
      )}

      {/* Project Image */}
      <div className="project-image">
        {hasImage ? (
          <img 
            src={imageUrl} 
            alt={title}
            loading="lazy"
            onError={(e) => {
              e.target.style.display = 'none';
              e.target.nextSibling.style.display = 'flex';
            }}
          />
        ) : null}
        <div 
          className="image-placeholder" 
          style={{ display: hasImage ? 'none' : 'flex' }}
        >
          {getProjectTypeIcon(type)}
          <span>No Image</span>
        </div>

        {/* Overlay with quick actions */}
        <div className="project-overlay">
          <div className="project-actions">
            <Link to={`/projects/${slug}`} className="action-btn view-btn">
              👁️ View
            </Link>
            {projectUrl && (
              <a 
                href={projectUrl} 
                target="_blank" 
                rel="noopener noreferrer" 
                className="action-btn live-btn"
              >
                🌐 Live
              </a>
            )}
            {githubUrl && (
              <a 
                href={githubUrl} 
                target="_blank" 
                rel="noopener noreferrer" 
                className="action-btn github-btn"
              >
                📂 Code
              </a>
            )}
            {demoUrl && (
              <a 
                href={demoUrl} 
                target="_blank" 
                rel="noopener noreferrer" 
                className="action-btn demo-btn"
              >
                🎥 Demo
              </a>
            )}
          </div>
        </div>
      </div>

      {/* Project Content */}
      <div className="project-content">
        {/* Status and Type */}
        <div className="project-meta">
          {showStatus && (
            <span 
              className="status-badge" 
              style={{ backgroundColor: getProjectStatusColor(status) }}
            >
              {getProjectStatusIcon(status)} {getProjectStatusLabel(status)}
            </span>
          )}
          {showType && (
            <span className="type-badge">
              {getProjectTypeIcon(type)}
            </span>
          )}
        </div>

        {/* Title */}
        <h3 className="project-title">
          <Link to={`/projects/${slug}`}>
            {title}
          </Link>
        </h3>

        {/* Description */}
        <p className="project-description">
          {shortDescription || description?.substring(0, 120) + (description?.length > 120 ? '...' : '')}
        </p>

        {/* Categories */}
        {showCategories && projectCategories?.length > 0 && (
          <div className="project-categories">
            {projectCategories.slice(0, 3).map((category, index) => (
              <span key={index} className="category-tag">
                📁 {category.name || `Category ${category.categoryId}`}
              </span>
            ))}
            {projectCategories.length > 3 && (
              <span className="more-categories">+{projectCategories.length - 3} more</span>
            )}
          </div>
        )}

        {/* Tags */}
        {showTags && projectTags?.length > 0 && (
          <div className="project-tags">
            {projectTags.slice(0, 5).map((tag, index) => (
              <span key={index} className="tag">
                #{tag.name || tag.tagName || `Tag ${tag.id}`}
              </span>
            ))}
            {projectTags.length > 5 && (
              <span className="more-tags">+{projectTags.length - 5}</span>
            )}
          </div>
        )}

        {/* Stats */}
        <div className="project-stats">
          <span className="stat">
            👁️ {viewCount || 0}
          </span>
          <span className="stat">
            ❤️ {likeCount || 0}
          </span>
        </div>
      </div>
    </div>
  );
};

export default ProjectCard;