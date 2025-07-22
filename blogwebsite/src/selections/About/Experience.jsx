// src/components/Experience.jsx
import React from 'react';
import experiences from '../../constants/experiences';
import '../../styles/Experience.css';

const Experience = () => {
  return (
    <section className="experience-section">
      <h2 className="section-title">Tecrübelerim</h2>
      <hr />

      {experiences.map((exp, index) => (
        <div className="experience-card" key={index}>
          <div className="experience-left">
            <h4>{exp.title}</h4>
            <p className="company">{exp.company} — {exp.location}</p>
            <p className="date">{exp.date}</p>
          </div>
          <div className="experience-right">
            <p className="description">{exp.description}</p>
            <ul className="responsibilities">
              {exp.responsibilities.map((item, i) => (
                <li key={i}>✓ {item}</li>
              ))}
            </ul>
          </div>
        </div>
      ))}
    </section>
  );
};

export default Experience;
