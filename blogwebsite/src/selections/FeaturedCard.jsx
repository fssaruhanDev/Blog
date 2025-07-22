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
