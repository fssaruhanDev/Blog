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

  // Eski tasarım: sabit degrade bar, scroll efekt yok
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

  const textColor = "#ffffff";

  return (
    <AppBar position="fixed" sx={{
      background: 'var(--nav-gradient)',
      boxShadow: '0 2px 10px -4px rgba(0,0,0,0.25)',
      border: 'none'
    }}>
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

  {/* Menü */}
        <Box
          sx={{
            flex:1,
            display: "flex",
            justifyContent:'center',
            alignItems: "center",
            gap: 3.2,
          }}
        >
          {[
            {to:'/', label:'Ana Sayfa', active: location.pathname==='/'},
            {to:'/hakkimda', label:'Hakkımda', active: location.pathname.startsWith('/hakkimda')},
            {to:'/blog', label:'Blog', active: location.pathname.startsWith('/blog')}
          ].map(item => (
            <Button key={item.to} component={Link} to={item.to}
              sx={{
                color: textColor,
                fontSize:"0.95rem",
                fontWeight:600,
                position:'relative',
                letterSpacing:.3,
                '&:hover':{opacity:.9},
                '&:after': item.active ? {content:'""', position:'absolute', left:10, right:10, bottom:-6, height:3, borderRadius:2, background:'#fff'} : {}
              }}
              className={item.active? 'active':''}
            >{item.label}</Button>
          ))}

          {!token ? (
  <Button component={Link} to="/login" variant="contained" sx={{ background:'var(--brand-primary)', '&:hover':{background:'#c13326'} }}>Giriş</Button>
          ) : (
  <Button onClick={handleLogout} variant="contained" sx={{ background:'var(--brand-primary)', '&:hover':{background:'#c13326'} }}>Çıkış</Button>
          )}
        </Box>
    <Divider orientation="vertical" flexItem sx={{borderColor:'rgba(0,0,0,0.08)', mx:2, display:{xs:'none', md:'block'} }} />
  {/* Tema ikonu kaldırıldı */}
      </Toolbar>
    </AppBar>
  );
}
