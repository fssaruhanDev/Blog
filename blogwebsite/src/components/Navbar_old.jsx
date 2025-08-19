import React, { useEffect, useState } from "react";
import { useLocation, Link, useNavigate } from "react-router-dom";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Avatar from "@mui/material/Avatar";
import "../styles/Navbar.css";

export default function Navbar() {
  const location = useLocation();
  const navigate = useNavigate();
  const isBlogDetailPage = location.pathname.startsWith("/blog/");
  const [isScrolled, setIsScrolled] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);
  const isDropdownOpen = Boolean(anchorEl);

  const [token, setToken] = useState(() => localStorage.getItem("auth_token"));

  useEffect(() => {
    const handler = () => setToken(localStorage.getItem("auth_token"));
    window.addEventListener("storage", handler);
    return () => window.removeEventListener("storage", handler);
  }, []);

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 50);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("auth_token");
    localStorage.removeItem("auth_user");
    // Clear any other user-related data
    localStorage.removeItem('news_items_v1');
    localStorage.removeItem('pending_ops_v1');
    setToken(null);
    setAnchorEl(null);
    navigate("/");
  };

  const handleDropdownClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleDropdownClose = () => {
    setAnchorEl(null);
  };

  const textColor = "#ffffff";

  return (
    <AppBar 
      position="fixed" 
      className={`navbar ${isScrolled ? 'scrolled' : ''}`}
      sx={{
        background: 'transparent',
        boxShadow: 'none',
        border: 'none'
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          height: "64px",
          maxWidth: '1200px',
          mx: 'auto',
          width: '100%',
          px: { xs: 2, md: 4 }
        }}
      >
        {/* Sol: Logo */}
        <Box sx={{ display: "flex", alignItems: "center" }}>
          <Link to="/" style={{ display: 'flex', alignItems: 'center', textDecoration: 'none' }}>
            <img src="/FSSaruhan-white.png" alt="Logo" style={{ height: 36 }} />
          </Link>
        </Box>

        {/* Orta: Navigation Menu (Desktop) */}
        <Box
          sx={{
            display: { xs: 'none', md: 'flex' },
            alignItems: "center",
            gap: 4,
            flex: 1,
            justifyContent: 'center'
          }}
        >
          {[
            {to:'/', label:'ANA SAYFA', active: location.pathname==='/'},
            {to:'/hakkimda', label:'HAKKIMDA', active: location.pathname.startsWith('/hakkimda')},
            {to:'/blog', label:'BLOG', active: location.pathname.startsWith('/blog')}
          ].map(item => (
            <Button 
              key={item.to} 
              component={Link} 
              to={item.to}
              sx={{
                color: textColor,
                fontSize: "0.875rem",
                fontWeight: 600,
                letterSpacing: 1.5,
                position: 'relative',
                padding: '8px 16px',
                '&:hover': {
                  backgroundColor: 'rgba(255,255,255,0.1)',
                  borderRadius: '4px'
                },
                '&::after': item.active ? {
                  content: '""',
                  position: 'absolute',
                  bottom: '-2px',
                  left: '50%',
                  transform: 'translateX(-50%)',
                  width: '40px',
                  height: '2px',
                  backgroundColor: '#fff',
                  borderRadius: '1px'
                } : {}
              }}
            >
              {item.label}
            </Button>
          ))}
        </Box>

        {/* Sağ: User Actions */}
        <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
          {!token ? (
            <Button 
              component={Link} 
              to="/login" 
              variant="outlined"
              sx={{ 
                color: '#fff',
                borderColor: 'rgba(255,255,255,0.3)',
                fontSize: '0.875rem',
                fontWeight: 600,
                letterSpacing: 1,
                px: 3,
                py: 1,
                borderRadius: '6px',
                display: { xs: 'none', md: 'flex' },
                '&:hover': {
                  borderColor: '#fff',
                  backgroundColor: 'rgba(255,255,255,0.1)'
                }
              }}
            >
              GİRİŞ
            </Button>
          ) : (
            <Box sx={{ display: { xs: 'none', md: 'flex' }, alignItems: 'center' }}>
              <IconButton
                onClick={handleDropdownClick}
                sx={{ p: 0 }}
              >
                <Avatar 
                  sx={{ 
                    width: 36, 
                    height: 36, 
                    bgcolor: 'rgba(255,255,255,0.15)',
                    color: '#fff',
                    fontSize: '1rem',
                    border: '1px solid rgba(255,255,255,0.2)'
                  }}
                >
                  👤
                </Avatar>
              </IconButton>
              
              <Menu
                anchorEl={anchorEl}
                open={isDropdownOpen}
                onClose={handleDropdownClose}
                anchorOrigin={{
                  vertical: 'bottom',
                  horizontal: 'right',
                }}
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                sx={{
                  '& .MuiPaper-root': {
                    minWidth: 180,
                    mt: 1.5,
                    borderRadius: '8px',
                    boxShadow: '0 8px 32px rgba(0,0,0,0.12)',
                    border: '1px solid rgba(0,0,0,0.08)'
                  }
                }}
              >
                <MenuItem 
                  component={Link} 
                  to="/admin"
                  onClick={handleDropdownClose}
                  sx={{ 
                    fontSize: '0.875rem', 
                    py: 1.5,
                    px: 2,
                    '&:hover': {
                      backgroundColor: 'rgba(247,109,85,0.08)'
                    }
                  }}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                    <span>🏠</span>
                    <span>Admin Paneli</span>
                  </Box>
                </MenuItem>
                <MenuItem 
                  onClick={handleLogout}
                  sx={{ 
                    fontSize: '0.875rem', 
                    py: 1.5,
                    px: 2,
                    color: '#d32f2f',
                    '&:hover': {
                      backgroundColor: 'rgba(211,47,47,0.08)'
                    }
                  }}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                    <span>🚪</span>
                    <span>Çıkış Yap</span>
                  </Box>
                </MenuItem>
              </Menu>
            </Box>
          )}

          {/* Mobile Menu Button */}
          <IconButton
            sx={{ 
              display: { xs: 'flex', md: 'none' },
              color: textColor,
              p: 1
            }}
            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
          >
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
              <path d="M3 12h18M3 6h18M3 18h18" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
            </svg>
          </IconButton>
        </Box>

        {/* Mobile Menu */}
        {isMobileMenuOpen && (
          <Box
            sx={{
              position: 'absolute',
              top: '100%',
              left: 0,
              right: 0,
              background: 'rgba(247,109,85,0.98)',
              backdropFilter: 'blur(10px)',
              display: { xs: 'flex', md: 'none' },
              flexDirection: 'column',
              py: 2,
              px: 3,
              gap: 0.5,
              zIndex: 1000,
              borderTop: '1px solid rgba(255,255,255,0.1)'
            }}
          >
            {[
              {to:'/', label:'ANA SAYFA'},
              {to:'/hakkimda', label:'HAKKIMDA'},
              {to:'/blog', label:'BLOG'}
            ].map(item => (
              <Button 
                key={item.to} 
                component={Link} 
                to={item.to}
                onClick={() => setIsMobileMenuOpen(false)}
                sx={{
                  color: '#fff',
                  justifyContent: 'flex-start',
                  fontSize: '0.875rem',
                  fontWeight: 600,
                  letterSpacing: 1.2,
                  py: 1.5,
                  px: 2,
                  borderRadius: '6px',
                  '&:hover': { 
                    backgroundColor: 'rgba(255,255,255,0.1)' 
                  }
                }}
              >
                {item.label}
              </Button>
            ))}
            
            <Box sx={{ height: '1px', backgroundColor: 'rgba(255,255,255,0.1)', my: 1 }} />
            
            {token ? (
              <>
                <Button 
                  component={Link} 
                  to="/admin"
                  onClick={() => setIsMobileMenuOpen(false)}
                  sx={{
                    color: '#fff',
                    justifyContent: 'flex-start',
                    fontSize: '0.875rem',
                    fontWeight: 600,
                    py: 1.5,
                    px: 2,
                    borderRadius: '6px',
                    '&:hover': { 
                      backgroundColor: 'rgba(255,255,255,0.1)' 
                    }
                  }}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                    <span>🏠</span>
                    <span>Admin Paneli</span>
                  </Box>
                </Button>
                <Button 
                  onClick={() => { handleLogout(); setIsMobileMenuOpen(false); }}
                  sx={{ 
                    color: '#ffcccb',
                    justifyContent: 'flex-start',
                    fontSize: '0.875rem',
                    fontWeight: 600,
                    py: 1.5,
                    px: 2,
                    borderRadius: '6px',
                    '&:hover': { 
                      backgroundColor: 'rgba(255,255,255,0.1)' 
                    }
                  }}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                    <span>🚪</span>
                    <span>Çıkış Yap</span>
                  </Box>
                </Button>
              </>
            ) : (
              <Button 
                component={Link} 
                to="/login" 
                onClick={() => setIsMobileMenuOpen(false)}
                sx={{ 
                  color: '#fff',
                  border: '1px solid rgba(255,255,255,0.3)',
                  fontSize: '0.875rem',
                  fontWeight: 600,
                  letterSpacing: 1,
                  py: 1.5,
                  px: 2,
                  mx: 1,
                  mt: 1,
                  borderRadius: '6px',
                  '&:hover': {
                    borderColor: '#fff',
                    backgroundColor: 'rgba(255,255,255,0.1)'
                  }
                }}
              >
                GİRİŞ
              </Button>
            )}
          </Box>
        )}
      </Toolbar>
    </AppBar>
  );
}
