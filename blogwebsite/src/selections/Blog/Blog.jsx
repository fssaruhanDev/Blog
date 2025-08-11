import { Box, Typography, Divider } from "@mui/material";
import BlogCard from "./BlogCard";
import FeaturedCard from "../FeaturedCard";
import { useEffect, useState } from "react";
import { getPosts } from "../../services/api";

export default function Blog() {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;
    async function load() {
      setLoading(true);
      try {
        const resp = await getPosts({ page: 1, pageSize: 20, status: "published" });
        if (!cancelled) setPosts(resp.items || resp.Items || []);
      } catch (e) {
        if (!cancelled) setError(e.message);
      } finally {
        if (!cancelled) setLoading(false);
      }
    }
    load();
    return () => { cancelled = true; };
  }, []);

  const featured = posts.slice(0, 2);

  return (
    <Box>
      <Box
        sx={{
          width: "100%",
          height: { xs: 100, md: 800 },
          backgroundImage: "url('/images/blog-header.png')",
          backgroundSize: "cover",
          backgroundPosition: "top center",
          position: "relative",
          mt: "-64px",
          pt: "54px"
        }}
      />
      <Box sx={{ display: "flex", gap: 4, px: 4, py: 6 }}>
        <Box sx={{ flex: 3 }}>
          <Typography variant="h5" fontWeight="bold" mb={3} color="primary">
            Son Bloglar
          </Typography>
          {loading && <Typography>Yükleniyor...</Typography>}
          {error && <Typography color="error">{error}</Typography>}
          {!loading && !error && posts.length === 0 && <Typography>Henüz blog yok.</Typography>}
          {posts.map((post) => (
            <Box key={post.id || post.ID} sx={{ mb: 4 }}>
              <BlogCard post={{
                id: post.id || post.ID,
                title: post.title || post.Title,
                date: (post.publishedAt || post.PublishedAt || post.createdDate || post.CreatedDate || '').toString().substring(0,10),
                summary: post.excerpt || post.Excerpt,
                tags: post.tags || post.Tags || [],
                image: post.coverImage || post.CoverImage || '/images/BlogExample.png'
              }} />
              <Divider sx={{ mt: 3, borderColor: "#FF7A00" }} />
            </Box>
          ))}
        </Box>
        <Box sx={{ flex: 1, borderLeft: "3px solid #FF7A00", pl: 3 }}>
          <Typography variant="h6" fontWeight="bold" mb={2}>
            Öne Çıkanlar
          </Typography>
          {featured.map(f => (
            <FeaturedCard key={f.id || f.ID} {...{
              id: f.id || f.ID,
              title: f.title || f.Title,
              date: (f.publishedAt || f.PublishedAt || f.createdDate || f.CreatedDate || '').toString().substring(0,10),
              summary: f.excerpt || f.Excerpt,
              image: f.coverImage || f.CoverImage || '/images/BlogExample.png'
            }} />
          ))}
        </Box>
      </Box>
    </Box>
  );
}
