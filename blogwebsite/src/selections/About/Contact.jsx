import React from "react";
import socials from "../../constants/socials";
import '../../styles/Contact.css'

const Contact = () => {
  return (
    <section className="contact-section">
      <h2 className="contact-title">İletişim</h2>
      <p className="contact-subtitle">Aşağıdaki platformlardan bana ulaşabilirsiniz:</p>
      <div className="social-icons">
        {socials.map((item) => (
          <a
            key={item.name}
            href={item.url}
            target="_blank"
            rel="noopener noreferrer"
            className="social-link"
          >
            <i className={`fab ${item.iconClass}`}></i>
          </a>
        ))}
      </div>
    </section>
  );
};

export default Contact;