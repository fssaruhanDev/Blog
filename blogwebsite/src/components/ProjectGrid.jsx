import React from 'react';
import ProjectCard from '../components/ProjectCard';

const ProjectGrid = ({ 
  projects = [], 
  loading = false, 
  showFilters = false,
  className = "",
  cardProps = {}
}) => {
  if (loading) {
    return (
      <div className={`project-grid-loading ${className}`}>
        {Array.from({ length: 6 }).map((_, index) => (
          <div key={index} className="project-card-skeleton">
            <div className="skeleton-image"></div>
            <div className="skeleton-content">
              <div className="skeleton-line skeleton-title"></div>
              <div className="skeleton-line skeleton-description"></div>
              <div className="skeleton-line skeleton-description short"></div>
              <div className="skeleton-tags">
                <div className="skeleton-tag"></div>
                <div className="skeleton-tag"></div>
                <div className="skeleton-tag"></div>
              </div>
            </div>
          </div>
        ))}
      </div>
    );
  }

  if (!projects.length) {
    return (
      <div className={`project-grid-empty ${className}`}>
        <div className="empty-state">
          <div className="empty-icon">📂</div>
          <h3>No Projects Found</h3>
          <p>There are no projects matching your criteria.</p>
        </div>
      </div>
    );
  }

  return (
    <div className={`project-grid ${className}`}>
      {projects.map((project) => (
        <ProjectCard 
          key={project.id} 
          project={project} 
          {...cardProps}
        />
      ))}
    </div>
  );
};

export default ProjectGrid;