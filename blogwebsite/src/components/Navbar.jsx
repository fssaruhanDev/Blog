import React, { useEffect, useState } from "react";
import { useLocation, Link, useNavigate } from "react-router-dom";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";

export default function Navbar() {
  const location = useLocation();
  const navigate = useNavigate();
  const isBlogDetailPage = location.pathname.startsWith("/blog/");

  const [scrolled, setScrolled] = useState(false);
  const [token, setToken] = useState(() => localStorage.getItem("auth_token"));
  // Tek açık tema (kurumsal) kullanılıyor.

  useEffect(() => {
    const handler = () => setToken(localStorage.getItem("auth_token"));
    window.addEventListener("storage", handler);
    return () => window.removeEventListener("storage", handler);
  }, []);

  // Tema toggling tamamen kaldırıldı

  const handleLogout = () => {
    localStorage.removeItem("auth_token");
    localStorage.removeItem("auth_user");
    setToken(null);
    navigate("/");
  };

  useEffect(() => {
    if (!isBlogDetailPage) {
      const handleScroll = () => {
        setScrolled(window.scrollY > 10);
      };
      window.addEventListener("scroll", handleScroll);
      return () => window.removeEventListener("scroll", handleScroll);
    }
  }, [isBlogDetailPage]);

  const backgroundStyle = (isBlogDetailPage || scrolled)
    ? "rgba(255,255,255,0.92)"
    : "transparent";
  const textColor = "#d33a2c";

  return (
    <AppBar
      position="fixed"
      sx={{
  background: backgroundStyle,
  boxShadow: (isBlogDetailPage || scrolled) ? '0 4px 14px -6px rgba(0,0,0,0.18)' : 'none',
  transition: "background 0.35s ease, box-shadow 0.3s ease",
  backdropFilter: (isBlogDetailPage || scrolled) ? 'blur(10px)' : 'none',
  borderBottom: (isBlogDetailPage || scrolled) ? '1px solid rgba(0,0,0,0.06)' : 'none'
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          position: "relative",
          height: "82px",
          maxWidth: '1440px',
          mx: 'auto',
          width: '100%',
        }}
      >
        {/* Logo */}
        <Box sx={{ display: "flex", alignItems: "center" }}>
          <img src="/FSSaruhan-white.png" alt="Logo" style={{ height: 50 }} />
        </Box>

        {/* Menü – ortalanmış ama sağda */}
        <Box
          sx={{
            flex:1,
            display: "flex",
            justifyContent:'center',
            alignItems: "center",
            gap: 3.2,
          }}
        >
          <Button component={Link} to="/" sx={{ color: textColor, fontSize:"0.95rem", fontWeight:600, position:'relative', '&:hover':{color:'var(--brand-accent)'}, '&.active:after':{content:'""', position:'absolute', left:12, right:12, bottom:-4, height:3, borderRadius:2, background:'linear-gradient(90deg,#d33a2c,#ff7e5f)'} }} className={location.pathname==='/'? 'active':''}>Ana Sayfa</Button>

          <Button component={Link} to="/hakkimda" sx={{ color: textColor, fontSize:"0.95rem", fontWeight:600, position:'relative', '&:hover':{color:'var(--brand-accent)'}, '&.active:after':{content:'""', position:'absolute', left:12, right:12, bottom:-4, height:3, borderRadius:2, background:'linear-gradient(90deg,#d33a2c,#ff7e5f)'} }} className={location.pathname.startsWith('/hakkimda')? 'active':''}>Hakkımda</Button>

          <Button component={Link} to="/blog" sx={{ color: textColor, fontSize:"0.95rem", fontWeight:600, position:'relative', '&:hover':{color:'var(--brand-accent)'}, '&.active:after':{content:'""', position:'absolute', left:12, right:12, bottom:-4, height:3, borderRadius:2, background:'linear-gradient(90deg,#d33a2c,#ff7e5f)'} }} className={location.pathname.startsWith('/blog')? 'active':''}>Blog</Button>

          {!token ? (
      <Button component={Link} to="/login" variant="outlined" sx={{ color:textColor, borderColor:'var(--brand-primary)', '&:hover':{borderColor:'var(--brand-accent)', background:'rgba(211,58,44,0.06)'} }}>Giriş</Button>
          ) : (
      <Button onClick={handleLogout} variant="outlined" sx={{ color:textColor, borderColor:'var(--brand-primary)', '&:hover':{borderColor:'var(--brand-accent)', background:'rgba(211,58,44,0.06)'} }}>Çıkış</Button>
          )}
        </Box>
    <Divider orientation="vertical" flexItem sx={{borderColor:'rgba(0,0,0,0.08)', mx:2, display:{xs:'none', md:'block'} }} />
  {/* Tema ikonu kaldırıldı */}
      </Toolbar>
    </AppBar>
  );
}
