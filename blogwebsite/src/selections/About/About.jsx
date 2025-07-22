import React from 'react';
import '../../styles/About.css';
import profileImage from '../../assets/profile-image.png';

function About() {
  return (
    <section className="about-section" id="hakkimda">
      <div className="about-container">
        <div className="about-image">
          <img src={profileImage} alt="Fatih Sultan Saruhan" />
        </div>
        <div className="about-content">
          <h1>Fatih Sultan Saruhan</h1>
<h3>Senior Software Developer</h3>
<p>
  Merhaba, ben Fatih. Yaklaşık on yıldır yazılım geliştiriyorum. Sadece kod yazmakla kalmıyor; aynı zamanda mimari tasarımlar üzerinde düşünüyor, sistemleri sürdürülebilir ve genişleyebilir şekilde inşa etmeyi hedefliyorum.
</p>
<p>
  Tecrübem sayesinde hem büyük ölçekli kurumsal sistemlerde hem de mikroservis tabanlı yapılarda farklı roller üstlendim. Analitik düşünebilme kabiliyetim ve çözüm odaklı yaklaşımım, projelerde sadece geliştirici değil aynı zamanda karar verici olarak da sorumluluk almamı sağladı.
</p>
<p>
  Backend teknolojilerinde güçlü bir altyapıya sahibim; .NET Core, Entity Framework, PostgreSQL gibi teknolojilerle yakından çalıştım. Ama sadece teknik bilgiyle değil, kullanıcıya değer sunan yazılımlar geliştirme bilinciyle ilerliyorum.
</p>
<p>
  Bu sitede yazılım geliştirme, sistem tasarımı, tecrübelerim ve fikirlerim hakkında notlar paylaşacağım. Eğer yazılıma gönül vermişsen, burada mutlaka kendine ait bir şeyler bulacaksın.
</p>

        </div>
      </div>
    </section>
  );
}

export default About;
