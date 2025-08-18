import { Box, Typography, Button, Chip } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function BlogCard({ post }) {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        gap: 4,
        p: 3,
        mb: 4,
        flexWrap: 'wrap',
        background: 'var(--surface)',
        border: '1px solid var(--border)',
        borderRadius: '14px',
        boxShadow: '2px 2px 8px rgba(0,0,0,0.05)',
  transition: 'box-shadow .25s ease, transform .25s ease, border-color .25s',
  '&:hover': { boxShadow:'3px 4px 14px -5px rgba(0,0,0,0.22)', transform:'translateY(-2px)', borderColor:'var(--brand-primary)' }
      }}
    >
      <Box sx={{ flex: 1, minWidth: "300px" }}>
        <Typography variant="h6" sx={{ fontWeight: 700, color: 'var(--brand-primary)', letterSpacing:'-.3px' }}>
          {post.title}
        </Typography>

        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
          {post.date}
        </Typography>

        <Typography variant="body1" sx={{ mt: 1 }}>
          {post.summary}
        </Typography>

  <Box sx={{ mt: 2, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
          {post.tags?.map((tag, idx) => (
            <Chip
              key={idx}
              label={tag}
              size="small"
              sx={{
    backgroundColor: 'var(--surface-alt)',
    color: 'var(--brand-primary)',
    fontWeight: 600,
    border:'1px solid var(--border)'
              }}
            />
          ))}
        </Box>

        <Button
          variant="contained"
          sx={{
            mt:2,
            background: 'var(--brand-primary)',
            textTransform:'none',
            fontWeight:600,
            letterSpacing:.2,
            boxShadow:'0 4px 12px -4px rgba(0,0,0,0.25)',
            '&:hover':{ background:'var(--brand-primary)', filter:'brightness(1.07)', boxShadow:'0 6px 18px -6px rgba(0,0,0,0.4)' }
          }}
          onClick={() => navigate(`/blog/${post.id}`)}
        >
          Devamını Oku
        </Button>
      </Box>

      <Box
        sx={{
          width: 200,
          height: 200,
          borderRadius: 2,
          overflow: "hidden",
          flexShrink: 0,
        }}
      >
        <img
          src={post.image}
          alt={post.title}
          style={{
            width:'100%',
            height:'100%',
            objectFit:'cover',
            borderRadius:'10px',
            background:'var(--surface-alt)'
          }}
          onError={(e)=>{ if(e.currentTarget.dataset.fallback) { e.currentTarget.src='/images/BlogExample.png'; return; } e.currentTarget.dataset.fallback='1'; const raw=e.currentTarget.getAttribute('src')||''; const idx=raw.indexOf('/uploads/'); if(idx>-1){ e.currentTarget.src = window.location.origin + raw.substring(idx); } else { e.currentTarget.src='/images/BlogExample.png'; } }}
        />
      </Box>
    </Box>
  );
}
