import React from 'react';
import projects from '../../constants/project';
import '../../styles/Project.css';

const Projects = () => {
  return (
    <div className="projects-section">
      <h2>Projelerim</h2>
      {projects.map((project, idx) => (
        <div key={idx} className="project-row">
          <div className="project-title">
            <h3>{project.title}</h3>
          </div>
          <div className="project-info">
            <p className="description">{project.description}</p>
            <ul>
              {project.responsibilities.map((item, i) => (
                <li key={i}>✔️ {item}</li>
              ))}
            </ul>
          </div>
        </div>
      ))}
    </div>
  );
};

export default Projects;
