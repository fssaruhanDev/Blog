import React, { useEffect, useState } from "react";
import "../../styles/Home.css";
import Slider from "../../components/Slider";
import { getNews, getPosts } from "../../services/api";
import { useNavigate } from "react-router-dom";

const Home = () => {
  const [news, setNews] = useState([]); // backend news
  const [posts, setPosts] = useState([]); // backend blog posts
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    let cancelled = false;
    async function load() {
      setLoading(true);
      try {
        const [newsResp, postsResp] = await Promise.all([
          getNews({ page: 1, pageSize: 6, status: "published" }),
          getPosts({ page: 1, pageSize: 6, status: "published" })
        ]);
        if (!cancelled) {
          setNews(newsResp.items || newsResp.Items || []);
          setPosts(postsResp.items || postsResp.Items || []);
        }
      } catch (e) {
        if (!cancelled) setError(e.message);
      } finally {
        if (!cancelled) setLoading(false);
      }
    }
    load();
    return () => { cancelled = true; };
  }, []);

  return (
    <div className="home-container">
      <Slider />
      <section className="section">
        <h2><span className="emoji">📘</span> Haberler</h2>
        {loading && <p>Yükleniyor...</p>}
        {error && <p style={{color:'red'}}>{error}</p>}
        <div className="cards">
          {!loading && !error && news.length === 0 && <p>Henüz haber yok.</p>}
          {news.map((n) => (
            <div className="card" key={n.id || n.ID}>
              <h3>{n.title || n.Title}</h3>
              <small>{(n.publishedAt || n.PublishedAt || n.createdDate || n.CreatedDate || '').toString().substring(0,10)}</small>
              <p>{n.summary || n.Summary}</p>
              {(n.sourceName || n.SourceName) && (
                <a href={n.sourceUrl || n.SourceUrl} target="_blank" rel="noreferrer">Kaynak: {n.sourceName || n.SourceName}</a>
              )}
            </div>
          ))}
        </div>
      </section>

      <section className="section">
        <h2><span className="emoji">📝</span> Son Bloglar</h2>
        <div className="cards">
          {posts.map(p => (
            <div className="card" key={p.id || p.ID}>
              <h3>{p.title || p.Title}</h3>
              <small>{(p.publishedAt || p.PublishedAt || p.createdDate || p.CreatedDate || '').toString().substring(0,10)}</small>
              <p>{p.excerpt || p.Excerpt}</p>
              <button className="read-more" onClick={() => navigate(`/blog/${p.id || p.ID}`)}>Devamını Oku</button>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
};

export default Home;
