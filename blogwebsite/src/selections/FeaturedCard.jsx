// components/FeaturedCard.jsx

import { Box, Typography } from "@mui/material";

export default function FeaturedCard({ title, image, date, description }) {
  return (
    <Box
      sx={{
        mb: 3,
        backgroundColor: "#fff",
        borderRadius: "8px",
        overflow: "hidden",
        boxShadow: "0 2px 6px rgba(0,0,0,0.05)",
      }}
    >
      <img
  src={image}
        alt={title}
        style={{
          width: "100%",
          height: "120px",
          objectFit: "cover",
        }}
  onError={(e)=>{ if(!e.currentTarget.dataset.fallback){ e.currentTarget.dataset.fallback='1'; const raw=e.currentTarget.getAttribute('src')||''; const idx=raw.indexOf('/uploads/'); if(idx>-1){ e.currentTarget.src= window.location.origin + raw.substring(idx); return; } } e.currentTarget.src='/images/BlogExample.png'; }}
      />
      <Box sx={{ p: 1.5 }}>
        <Typography
          variant="subtitle1"
          fontWeight="bold"
          gutterBottom
          sx={{ fontSize: "0.95rem" }}
        >
          {title}
        </Typography>

        <Typography
          variant="body2"
          color="text.secondary"
          sx={{ fontSize: "0.8rem", mb: 0.5 }}
        >
          {description?.slice(0, 80)}{description?.length > 80 ? "..." : ""}
        </Typography>

        <Typography
          variant="caption"
          sx={{ fontSize: "0.75rem", color: "#999" }}
        >
          {date}
        </Typography>
      </Box>
    </Box>
  );
}
