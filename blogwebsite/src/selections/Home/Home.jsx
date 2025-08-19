import React, { useEffect, useState, useCallback, useMemo } from "react";
import { useNavigate } from "react-router-dom";

// Components
import Slider from "../../components/Slider";

// Services
import { getNews, getPublicPosts, resolveMediaUrl } from "../../services/api";

// Styles
import "../../styles/globals.css";
import "../../styles/components/Home.css";

// Helper function to format dates - Memoized
const formatDate = (dateString) => {
  if (!dateString) return 'Tarih yok';
  try {
    return new Date(dateString).toLocaleDateString('tr-TR', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  } catch {
    return 'Geçersiz tarih';
  }
};

// Loading component - Memoized
const LoadingSpinner = React.memo(() => (
  <div className="loading-container">
    <div className="loading-spinner"></div>
    <p>İçerik yükleniyor...</p>
  </div>
));

// Error component - Memoized
const ErrorMessage = React.memo(({ message }) => (
  <div className="error-container">
    <h3 className="error-title">Bir Hata Oluştu</h3>
    <p>{message}</p>
  </div>
));

// Empty state component - Memoized
const EmptyState = React.memo(({ title, description }) => (
  <div className="empty-state">
    <div className="empty-state-icon">📰</div>
    <h3 className="empty-state-title">{title}</h3>
    <p className="empty-state-text">{description}</p>
  </div>
));

// Content card component - Memoized
const ContentCard = React.memo(({ item, type, onClick }) => {
  const imageUrl = useMemo(() => {
    return type === 'post' 
      ? resolveMediaUrl(item.coverImageUrl || item.CoverImageUrl)
      : '/images/default-news.jpg';
  }, [item.coverImageUrl, item.CoverImageUrl, type]);
    
  const title = item.title || item.Title || 'Başlık Yok';
  const excerpt = item.excerpt || item.Excerpt || item.summary || item.Summary || '';
  const date = item.publishedAt || item.PublishedAt || item.createdDate || item.CreatedDate;
  
  const handleImageError = useCallback((e) => {
    e.target.src = '/images/default-placeholder.jpg';
  }, []);
  
  return (
    <article className="content-card" onClick={onClick}>
      <img 
        src={imageUrl} 
        alt={title}
        className="card-image"
        onError={handleImageError}
        loading="lazy"
      />
      <div className="card-content">
        <h3 className="card-title">{title}</h3>
        {excerpt && <p className="card-excerpt">{excerpt}</p>}
        <div className="card-meta">
          <span className="card-date">{formatDate(date)}</span>
          <span className="card-category">{type === 'post' ? 'Blog' : 'Haber'}</span>
        </div>
      </div>
    </article>
  );
});

const Home = () => {
  // State
  const [news, setNews] = useState([]);
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // Hooks
  const navigate = useNavigate();

  // Memoized handlers
  const handleNewsClick = useCallback((item) => {
    const id = item.id || item.Id;
    if (id) navigate(`/news/${id}`);
  }, [navigate]);

  const handlePostClick = useCallback((item) => {
    const id = item.id || item.Id;
    if (id) navigate(`/blog/${id}`);
  }, [navigate]);

  // Data loading effect with debounced state updates
  useEffect(() => {
    let cancelled = false;
    let timeoutId;
    
    const loadContent = async () => {
      if (cancelled) return;
      
      try {
        // Show loading only after a small delay to prevent flash
        timeoutId = setTimeout(() => {
          if (!cancelled) {
            setLoading(true);
            setError(null);
          }
        }, 100);
        
        const [newsResponse, postsResponse] = await Promise.allSettled([
          getNews({ page: 1, pageSize: 6, status: "published" }).catch(() => 
            getNews({ page: 1, pageSize: 6 }) // Fallback without status filter
          ),
          getPublicPosts({ page: 1, pageSize: 6 })
        ]);
        
        if (!cancelled) {
          clearTimeout(timeoutId);
          
          // Batch state updates to prevent multiple re-renders
          const updates = {};
          
          // Process news data
          if (newsResponse.status === 'fulfilled') {
            const newsItems = newsResponse.value?.items || newsResponse.value?.Items || [];
            updates.news = newsItems;
          }
          
          // Process posts data  
          if (postsResponse.status === 'fulfilled') {
            const postItems = postsResponse.value?.items || postsResponse.value?.Items || [];
            updates.posts = postItems;
          }
          
          // Apply all updates at once
          setNews(updates.news || []);
          setPosts(updates.posts || []);
          setLoading(false);
          
          // Check if both failed
          if (newsResponse.status === 'rejected' && postsResponse.status === 'rejected') {
            setError('İçerik yüklenemedi. Lütfen sayfayı yenileyin.');
          }
        }
      } catch (err) {
        if (!cancelled) {
          clearTimeout(timeoutId);
          console.error('Error loading home content:', err);
          setError(err.message || 'İçerik yüklenirken bir hata oluştu');
          setLoading(false);
        }
      }
    };
    
    loadContent();
    
    return () => {
      cancelled = true;
      if (timeoutId) clearTimeout(timeoutId);
    };
  }, []);

  // Render loading state
  if (loading) {
    return (
      <div className="home">
        <div className="container">
          <LoadingSpinner />
        </div>
      </div>
    );
  }

  // Render error state
  if (error) {
    return (
      <div className="home">
        <div className="container">
          <ErrorMessage message={error} />
        </div>
      </div>
    );
  }

  return (
    <div className="home">
      {/* Hero Section */}
      <section className="hero-section">
        <Slider className="hero-slider" />
      </section>

      {/* Recent News Section */}
      <section className="content-section">
        <div className="container">
          <header className="section-header">
            <h2 className="section-title">Son Haberler</h2>
            <p className="section-subtitle">
              Teknoloji dünyasından en güncel haberler ve gelişmeler
            </p>
          </header>
          
          <div className="cards-grid" style={{ minHeight: '400px' }}>
            {loading ? (
              <div className="loading-placeholder">
                {[...Array(3)].map((_, i) => (
                  <div key={i} className="card-skeleton" />
                ))}
              </div>
            ) : news.length === 0 ? (
              <EmptyState 
                title="Henüz Haber Yok"
                description="Yakında güncel haberlerle burada karşınızda olacağız."
              />
            ) : (
              news.map((item, index) => (
                <ContentCard
                  key={item.id || item.ID || index}
                  item={item}
                  type="news"
                  onClick={() => handleNewsClick(item)}
                />
              ))
            )}
          </div>
        </div>
      </section>

      {/* Recent Blog Posts Section */}
      <section className="content-section">
        <div className="container">
          <header className="section-header">
            <h2 className="section-title">Son Blog Yazıları</h2>
            <p className="section-subtitle">
              Yazılım geliştirme, teknoloji trendleri ve kişisel deneyimlerim
            </p>
          </header>
          
          <div className="cards-grid" style={{ minHeight: '400px' }}>
            {loading ? (
              <div className="loading-placeholder">
                {[...Array(3)].map((_, i) => (
                  <div key={i} className="card-skeleton" />
                ))}
              </div>
            ) : posts.length === 0 ? (
              <EmptyState 
                title="Henüz Blog Yazısı Yok"
                description="Yakında ilginç blog yazılarıyla burada olacağım."
              />
            ) : (
              posts.map((item, index) => (
                <ContentCard
                  key={item.id || item.ID || index}
                  item={item}
                  type="post"
                  onClick={() => handlePostClick(item)}
                />
              ))
            )}
          </div>
        </div>
      </section>
    </div>
  );
};

export default Home;