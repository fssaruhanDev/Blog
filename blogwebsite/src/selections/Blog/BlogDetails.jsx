import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import ReactMarkdown from 'react-markdown';
import rehypeHighlight from 'rehype-highlight';
import 'highlight.js/styles/github-dark.css';
import blogPosts from './data/BlogData'; // fallback static
import { getPost } from '../../services/api';
import "../../styles/BlogDetails.css";

const BlogDetails = () => {
  const { id } = useParams();
  const [post, setPost] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');

  useEffect(() => {
    let cancelled = false;
    async function load() {
      // Try backend
      try {
        const resp = await getPost(id);
        if (!cancelled && resp) {
          setPost({
            id: resp.id || resp.ID,
            title: resp.title || resp.Title,
            content: resp.content || resp.Content || resp.excerpt || resp.Excerpt,
            date: (resp.publishedAt || resp.PublishedAt || resp.createdDate || resp.CreatedDate || '').toString().substring(0,10),
            image: resp.coverImage || resp.CoverImage || '/images/BlogExample.png'
          });
          return;
        }
      } catch (e) {
        // fallback uses local static list
      }
      // Fallback static list
      const foundPost = blogPosts.find((p) => p.id === parseInt(id));
      if (!cancelled) setPost(foundPost || null);
    }
    load();
    return () => { cancelled = true; };
  }, [id]);

  const handleAddComment = () => {
    if (newComment.trim()) {
      setComments([...comments, newComment]);
      setNewComment('');
    }
  };

  useEffect(() => {
    // Navbar için koyu tema zorlaması
    document.body.classList.add('navbar-red');
    return () => document.body.classList.remove('navbar-red');
  }, []);

  if (!post) return <div>Yükleniyor...</div>;

  return (
    <div className="blog-details-container">
  <img src={post.image} alt={post.title} className="blog-header-image" />
      <div className="blog-content">
        <h1>{post.title}</h1>
        <ReactMarkdown rehypePlugins={[rehypeHighlight]}>{post.content}</ReactMarkdown>

        <div className="comments-section">
          <h2>Yorumlar ({comments.length})</h2>
          {comments.length === 0 ? <p>Henüz yorum yapılmadı.</p> : (
            <ul>
              {comments.map((comment, index) => (
                <li key={index}>{comment}</li>
              ))}
            </ul>
          )}
          <textarea
            placeholder="Yorumunuzu yazın"
            value={newComment}
            onChange={(e) => setNewComment(e.target.value)}
          />
          <button onClick={handleAddComment}>Yorum Ekle</button>
        </div>
      </div>
    </div>
  );
};

export default BlogDetails;
