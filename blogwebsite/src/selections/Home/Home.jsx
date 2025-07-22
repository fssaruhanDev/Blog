import React from "react";
import "../../styles/Home.css";
import Slider from "../../components/Slider";

const Home = () => {

  const updates = [
    { icon: "🆕", title: "Yeni Blog Yayında!", date: "07 Temmuz 2025", description: "CQRS ve Event Sourcing’i gerçek bir projede nasıl kullandım?" },
    { icon: "🚀", title: "Service F13 Tamamlandı", date: "01 Temmuz 2025", description: "Yeni SaaS platformumuz yayına alındı." },  
    { icon: "🆕", title: "Yeni Blog Yayında!", date: "07 Temmuz 2025", description: "CQRS ve Event Sourcing’i gerçek bir projede nasıl kullandım?" },
    { icon: "🚀", title: "Service F13 Tamamlandı", date: "01 Temmuz 2025", description: "Yeni SaaS platformumuz yayına alındı." },  
    { icon: "🆕", title: "Yeni Blog Yayında!", date: "07 Temmuz 2025", description: "CQRS ve Event Sourcing’i gerçek bir projede nasıl kullandım?" },
    { icon: "🚀", title: "Service F13 Tamamlandı", date: "01 Temmuz 2025", description: "Yeni SaaS platformumuz yayına alındı." },  
    { icon: "🆕", title: "Yeni Blog Yayında!", date: "07 Temmuz 2025", description: "CQRS ve Event Sourcing’i gerçek bir projede nasıl kullandım?" },
    { icon: "🚀", title: "Service F13 Tamamlandı", date: "01 Temmuz 2025", description: "Yeni SaaS platformumuz yayına alındı." },
  ];

  const achievements = [
    { icon: "🟣", title: ".NET Core", description: "Mikroservis tabanlı e-ticaret platformu" },
    { icon: "🟥", title: "React.js", description: "Node.js tabanlı toplantı odası rezervasyon sistemi" },  
    { icon: "🟣", title: ".NET Core", description: "Mikroservis tabanlı e-ticaret platformu" },
    { icon: "🟥", title: "React.js", description: "Node.js tabanlı toplantı odası rezervasyon sistemi" }, 
    { icon: "🟣", title: ".NET Core", description: "Mikroservis tabanlı e-ticaret platformu" },
    { icon: "🟥", title: "React.js", description: "Node.js tabanlı toplantı odası rezervasyon sistemi" },  
    { icon: "🟣", title: ".NET Core", description: "Mikroservis tabanlı e-ticaret platformu" },
    { icon: "🟥", title: "React.js", description: "Node.js tabanlı toplantı odası rezervasyon sistemi" },
  ];

  const blogs = [
    { icon: "🟩", title: "CQRS ile Neden Tanıştım?", date: "05 Temmuz 2025", summary: "CRUD yetersiz kalınca çözüm CQRS oldu..." },
    { icon: "🟨", title: "Event Sourcing: Karmaşık Süreçlerin Kurtarıcısı", date: "25 Haziran 2025", summary: "Verinin geçmişine hükmetmek mümkün mü?" },   
    { icon: "🟩", title: "CQRS ile Neden Tanıştım?", date: "05 Temmuz 2025", summary: "CRUD yetersiz kalınca çözüm CQRS oldu..." },
    { icon: "🟨", title: "Event Sourcing: Karmaşık Süreçlerin Kurtarıcısı", date: "25 Haziran 2025", summary: "Verinin geçmişine hükmetmek mümkün mü?" },   
    { icon: "🟩", title: "CQRS ile Neden Tanıştım?", date: "05 Temmuz 2025", summary: "CRUD yetersiz kalınca çözüm CQRS oldu..." },
    { icon: "🟨", title: "Event Sourcing: Karmaşık Süreçlerin Kurtarıcısı", date: "25 Haziran 2025", summary: "Verinin geçmişine hükmetmek mümkün mü?" },   
    { icon: "🟩", title: "CQRS ile Neden Tanıştım?", date: "05 Temmuz 2025", summary: "CRUD yetersiz kalınca çözüm CQRS oldu..." },
    { icon: "🟨", title: "Event Sourcing: Karmaşık Süreçlerin Kurtarıcısı", date: "25 Haziran 2025", summary: "Verinin geçmişine hükmetmek mümkün mü?" },
  ];

  return (
    <div className="home-container">
      <Slider />
      <section className="section">
        <h2><span className="emoji">📘</span> Yenilikler</h2>
        <div className="cards">
          {updates.map((u, i) => (
            <div className="card" key={i}>
              <h3><span className="emoji">{u.icon}</span> {u.title}</h3>
              <small>{u.date}</small>
              <p>{u.description}</p>
            </div>
          ))}
        </div>
      </section>

      <section className="section">
        <h2><span className="emoji">🏆</span> Profesyonel Başarılarım</h2>
        <div className="cards">
          {achievements.map((a, i) => (
            <div className="card" key={i}>
              <h3><span className="emoji">{a.icon}</span> {a.title}</h3>
              <p>{a.description}</p>
            </div>
          ))}
        </div>
      </section>

      <section className="section">
        <h2><span className="emoji">📝</span> Son Bloglar</h2>
        <div className="cards">
          {blogs.map((b, i) => (
            <div className="card" key={i}>
              <h3><span className="emoji">{b.icon}</span> {b.title}</h3>
              <small>{b.date}</small>
              <p>{b.summary}</p>
              <button className="read-more">Devamını Oku</button>
            </div>
          ))}
        </div>
      </section>

    </div>
  );
};

export default Home;
