import { Box, Typography, Divider } from "@mui/material";
import BlogCard from "./BlogCard";
import blogPosts from "./data/BlogData";
import FeaturedCard from "../FeaturedCard";

export default function Blog() {
  return (
      <Box>
      {/* Header görseli */}
      <Box
        sx={{
          width: "100%",
          height: { xs: 100, md: 800 },
          backgroundImage: "url('/images/blog-header.png')",
          backgroundSize: "cover",
          backgroundPosition: "top center", // sadece 'top' değil
          position: "relative",
          mt: "-64px", // navbar yüksekliğini düşür
          pt: "54px" // içerikleri aşağıdan başlat
        }}
      />
       <Box sx={{ display: "flex", gap: 4, px: 4, py: 6 }}>
    
      {/* Sol taraf - Blog listesi */}
      <Box sx={{ flex: 3 }}>
        <Typography variant="h5" fontWeight="bold" mb={3} color="primary">
          Son Bloglar
        </Typography>
        {blogPosts.map((post) => (
          <Box key={post.id} sx={{ mb: 4 }}>
            <BlogCard post={post} />
            <Divider sx={{ mt: 3, borderColor: "#FF7A00" }} />
          </Box>
        ))}
      </Box>

      {/* Sağ taraf - Öne Çıkanlar */}
      <Box sx={{ flex: 1, borderLeft: "3px solid #FF7A00", pl: 3 }}>
        <Typography variant="h6" fontWeight="bold" mb={2}>
          Öne Çıkanlar
        </Typography>
        <FeaturedCard {...blogPosts[0]} />
        <FeaturedCard {...blogPosts[1]} />
      </Box>
    </Box>
      </Box>
   
  );
}
