import React, { useEffect, useState } from "react";
import { useLocation, Link } from "react-router-dom";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";

export default function Navbar() {
  const location = useLocation();
  const isBlogDetailPage = location.pathname.startsWith("/blog/"); // örn: /blog/1

  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    if (!isBlogDetailPage) {
      const handleScroll = () => {
        setScrolled(window.scrollY > 10);
      };
      window.addEventListener("scroll", handleScroll);
      return () => window.removeEventListener("scroll", handleScroll);
    }
  }, [isBlogDetailPage]);

  const backgroundStyle = isBlogDetailPage
    ? "rgba(0, 0, 0, 0.85)" // detay sayfasında hep sabit koyu
    : scrolled
    ? "linear-gradient(to right,rgb(219, 91, 59), #feb47b)" // scroll varsa degrade
    : "transparent"; // scroll yoksa şeffaf

  return (
    <AppBar
      position="fixed"
      sx={{
        background: backgroundStyle,
        boxShadow: isBlogDetailPage || scrolled ? 3 : "none",
        transition: "background 0.3s ease, box-shadow 0.3s ease",
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          position: "relative",
          height: "90px",
        }}
      >
        {/* Logo */}
        <Box sx={{ display: "flex", alignItems: "center" }}>
          <img
            src="/FSSaruhan-white.png"
            alt="Logo"
            style={{ height: "50px" }}
          />
        </Box>

        {/* Menü – ortalanmış ama sağda */}
        <Box
          sx={{
            position: "absolute",
            right: "15%",
            top: "50%",
            transform: "translateY(-50%)",
            display: "flex",
            alignItems: "center",
            gap: 5,
          }}
        >
          <Button
            component={Link}
            to="/"
            sx={{ color: "white", fontSize: "1rem" }}
          >
            Ana Sayfa
          </Button>

          <Button
            component={Link}
            to="/hakkimda"
            sx={{ color: "white", fontSize: "1rem" }}
          >
            Hakkımda
          </Button>

          <Button
            component={Link}
            to="/blog"
            sx={{ color: "white", fontSize: "1rem" }}
          >
            Blog
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
}
