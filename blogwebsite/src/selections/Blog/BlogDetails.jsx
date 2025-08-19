import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import ReactMarkdown from 'react-markdown';
import rehypeHighlight from 'rehype-highlight';
import rehypeRaw from 'rehype-raw';
import remarkGfm from 'remark-gfm';
import 'highlight.js/styles/github-dark.css';
import blogPosts from './data/BlogData'; // fallback static
import { getPublicPost, getComments, addComment } from '../../services/api';
import "../../styles/BlogDetails.css";

const BlogDetails = () => {
  const { id } = useParams();
  const [post, setPost] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [progress, setProgress] = useState(0);
  const [headings, setHeadings] = useState([]);
  const [commentName, setCommentName] = useState('');

  // Load post + comments
  useEffect(() => {
    let cancelled = false;
    (async () => {
      try {
        const resp = await getPublicPost(id); // Public API kullan
        try { console.log('[BlogDetails] Post API raw:', resp); window.__rawPost = resp; } catch {}
        if (!cancelled && resp) {
          const loaded = {
            id: resp.id || resp.ID,
            title: resp.title || resp.Title || resp.name || resp.Name || resp.titleText || resp.TitleText || resp.postTitle || resp.PostTitle,
            content: resp.content || resp.Content || resp.excerpt || resp.Excerpt || resp.body || resp.Body,
            date: (resp.publishedAt || resp.PublishedAt || resp.createdDate || resp.CreatedDate || resp.date || resp.Date || '').toString().substring(0,10),
            image: resp.coverImageUrl || resp.CoverImageUrl || resp.coverImage || resp.CoverImage || resp.image || resp.Image || '/images/BlogExample.png'
          };
          try { console.log('[BlogDetails] Post mapped:', loaded); window.__post = loaded; } catch {}
          setPost(loaded);
          try { const list = await getComments(resp.id || resp.ID); setComments(list); } catch {}
          return;
        }
      } catch { /* ignore */ }
      const foundPost = blogPosts.find((p) => String(p.id) === String(id));
      if(foundPost){ try { console.log('[BlogDetails] Using static fallback post:', foundPost); window.__postFallback = foundPost; } catch {} }
      if (!cancelled) setPost(foundPost || null);
    })();
    return () => { cancelled = true; };
  }, [id]);

  const handleAddComment = async () => {
    if (!newComment.trim()) return;
    try {
      const created = await addComment(post.id, { authorName: commentName || 'Anonim', content: newComment.trim() });
      setComments([...comments, created]);
      setNewComment('');
    } catch (e) {
      alert(e.message);
    }
  };

  useEffect(() => {
    // Navbar için kurumsal degrade zorlaması & okuma ilerleme çubuğu
    document.body.classList.add('reading-mode');
    
    const onScroll = () => {
      const doc = document.documentElement;
      const scrollTop = doc.scrollTop || document.body.scrollTop;
      const scrollHeight = doc.scrollHeight - doc.clientHeight;
      const p = scrollHeight > 0 ? (scrollTop / scrollHeight) * 100 : 0;
      setProgress(p);
    };

    // Intersection Observer for TOC
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          const id = entry.target.id;
          const tocLink = document.querySelector(`a[href="#${id}"]`);
          if (tocLink) {
            if (entry.isIntersecting) {
              document.querySelectorAll('.toc-list a').forEach(a => a.classList.remove('active'));
              tocLink.classList.add('active');
            }
          }
        });
      },
      { rootMargin: '-80px 0px -60% 0px' }
    );

    // Observe headings after a short delay to ensure they're rendered
    const observeHeadings = () => {
      const headings = document.querySelectorAll('.blog-content h1, .blog-content h2, .blog-content h3');
      headings.forEach(heading => observer.observe(heading));
    };

    setTimeout(observeHeadings, 500);
    
    window.addEventListener('scroll', onScroll);
    onScroll();
    
    return () => {
      document.body.classList.remove('reading-mode');
      window.removeEventListener('scroll', onScroll);
      observer.disconnect();
    };
  }, [post]);

  // Extract headings effect (supports # to ### + <h1-3>)
  useEffect(()=>{
    if(!post?.content) return;
    const slugify = (str="") => str
      .toLowerCase()
      .replace(/[ıİ]/g,'i')
      .replace(/ğ/g,'g')
      .replace(/ü/g,'u')
      .replace(/ş/g,'s')
      .replace(/ö/g,'o')
      .replace(/ç/g,'c')
      .replace(/&nbsp;/g,' ')
      .replace(/\u00A0/g,' ')
      .replace(/[^a-z0-9\s-]/g,'')
      .trim()
      .replace(/\s+/g,'-')
      .replace(/-+/g,'-');
    const hs = [];
    // HTML headings h1-h3
    const htmlMatches = [...post.content.matchAll(/<h([1-3])[^>]*>(.*?)<\/h[1-3]>/gi)];
    htmlMatches.forEach(m=>{ 
      const txt = m[2].replace(/<[^>]+>/g,'').replace(/&nbsp;/g,' ').replace(/\u00A0/g,' ').trim(); 
      if(txt) hs.push({ level: parseInt(m[1],10), text: txt, id: slugify(txt) }); 
    });
    // Markdown headings # .. ###
    const mdMatches = [...post.content.matchAll(/^(#{1,3})\s+(.+)$/gm)];
    mdMatches.forEach(m=>{ 
      const level = m[1].length; 
      const raw = m[2].replace(/#+$/,'').replace(/&nbsp;/g,' ').replace(/\u00A0/g,' ').trim(); 
      const id = slugify(raw); 
      hs.push({ level, text: raw, id }); 
    });
    // Dedupe & keep order of first appearance
    const seen = new Set();
    const unique = [];
    hs.forEach(h=>{ if(!seen.has(h.id)){ seen.add(h.id); unique.push(h);} });
    setHeadings(unique.slice(0,50));
  },[post]);

  // Add copy buttons to code blocks after render
  useEffect(()=>{
    const blocks = document.querySelectorAll('.blog-content pre code');
    blocks.forEach(block=>{
      const pre = block.parentElement;
      if(pre && !pre.querySelector('.copy-btn')){
        const btn = document.createElement('button');
        btn.className='copy-btn';
        btn.type='button';
        btn.innerText='Kopyala';
        btn.addEventListener('click',()=>{
          navigator.clipboard.writeText(block.innerText).then(()=>{
            btn.innerText='Kopyalandı';
            setTimeout(()=>btn.innerText='Kopyala',1500);
          });
        });
        pre.style.position='relative';
        btn.style.position='absolute';
        btn.style.top='8px';
        btn.style.right='8px';
        btn.style.fontSize='12px';
        btn.style.padding='4px 8px';
  btn.style.border='1px solid var(--border)';
  btn.style.borderRadius='6px';
  btn.style.background='linear-gradient(90deg,var(--brand-primary),var(--brand-accent))';
  btn.style.color='#fff';
        btn.style.cursor='pointer';
        pre.appendChild(btn);
      }
    });
  },[post]);

  if (!post) return <div>Yükleniyor...</div>;

  const isGuid = (val) => /^[0-9a-fA-F-]{36}$/i.test(val || '');

  return (
    <div className="blog-details-wrapper">
      <div className="reading-progress-bar" style={{ width: `${progress}%` }} />
      <header className="blog-hero">
        <div className="blog-hero-media">
          <img src={post.image} alt={post.title || 'Kapak'} className="blog-header-image fade-in" />
        </div>
        <div className="hero-layer hero-top" aria-hidden="true" />
        <div className="hero-layer hero-bottom-fade" aria-hidden="true" />
        <div className="blog-hero-titlebox fade-slide">
          {post.title ? (
            <h1 className="blog-title hero-title" data-role="post-title-hero">{post.title}</h1>
          ) : (
            <h1 className="blog-title hero-title" data-role="post-title-hero">(Başlık bulunamadı)</h1>
          )}
          {post.date && <span className="post-meta">{post.date}</span>}
        </div>
      </header>
      <main className="blog-details-container">
        <div className="blog-content fade-in">
          <ReactMarkdown 
            remarkPlugins={[remarkGfm]} 
            rehypePlugins={[rehypeRaw, rehypeHighlight]}
            components={{
              img: ({node, ...props}) => <img className="content-image" loading="lazy" {...props} />
            }}
          >
            {post.content}
          </ReactMarkdown>

          <div className="comments-section">
            <h2>Yorumlar ({comments.length})</h2>
            {comments.length === 0 ? <p>Henüz yorum yapılmadı.</p> : (
              <ul>
                {comments.map((c) => (
                  <li key={c.ID || c.id}>
                    <strong>{c.AuthorName || c.authorName || 'Anonim'}:</strong> {c.Content || c.content || ''}
                  </li>
                ))}
              </ul>
            )}
            <input
              type="text"
              placeholder="İsim (opsiyonel)"
              value={commentName}
              onChange={(e)=>setCommentName(e.target.value)}
              style={{width:'100%',padding:'0.7rem 1rem',borderRadius:12,border:'1px solid var(--border)',background:'var(--surface-alt)',color:'inherit',marginTop:'1rem'}}
            />
            <textarea
              placeholder="Yorumunuzu yazın"
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
            />
            <button onClick={handleAddComment} disabled={!isGuid(post.id)}>Yorum Ekle</button>
            {!isGuid(post.id) && <small style={{opacity:.7,display:'block',marginTop:8}}>Bu demo (statik) içerikte yorum gönderimi kapalı.</small>}
          </div>
        </div>
      </main>
    </div>
  );
};

export default BlogDetails;
