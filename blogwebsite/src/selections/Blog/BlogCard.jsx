import { Box, Typography, Button, Chip } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function BlogCard({ post }) {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "space-between",
        gap: 4,
        p: 3,
        mb: 4,
        borderBottom: "2px solid #F5B700",
        flexWrap: "wrap",
      }}
    >
      <Box sx={{ flex: 1, minWidth: "300px" }}>
        <Typography variant="h6" sx={{ fontWeight: "bold", color: "#E94F1D" }}>
          {post.title}
        </Typography>

        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
          {post.date}
        </Typography>

        <Typography variant="body1" sx={{ mt: 1 }}>
          {post.summary}
        </Typography>

        <Box sx={{ mt: 2, display: "flex", gap: 1, flexWrap: "wrap" }}>
          {post.tags?.map((tag, idx) => (
            <Chip
              key={idx}
              label={tag}
              size="small"
              sx={{
                backgroundColor: "#FFE5DC",
                color: "#E94F1D",
                fontWeight: "bold",
              }}
            />
          ))}
        </Box>

        <Button
          variant="contained"
          sx={{
            mt: 2,
            backgroundColor: "#E94F1D",
            textTransform: "none",
            fontWeight: "bold",
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
            width: "100%",
            height: "100%",
            objectFit: "cover",
            borderRadius: "8px",
          }}
        />
      </Box>
    </Box>
  );
}
