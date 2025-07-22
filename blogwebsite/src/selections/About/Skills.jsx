import skills from "../../constants/skills";
import "../../styles/Skills.css";

function Skills() {
  return (
    <div className="skills-wrapper">
      {skills.map((group, index) => (
        <div className="skill-category-block" key={index}>
          <div className="skill-category-title">{group.category}</div>
          <div className="skill-bars">
            {group.list.map((skill, idx) => (
              <div key={idx} className="skill-bar-row">
                <div className="skill-name">{skill.name}</div>
                <div className="skill-bar-wrapper">
                  <div className="skill-bar-fill" style={{ width: `${skill.level}%` }}></div>
                </div>
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}

export default Skills;
