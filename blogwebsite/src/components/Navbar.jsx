import React, { useEffect, useState } from "react";
import { useLocation, Link, useNavigate } from "react-router-dom";
import { logout } from "../services/api";

// Material-UI Components
import {
  AppBar,
  Toolbar,
  Button,
  Divider,
  IconButton,
  Menu,
  MenuItem,
  Avatar,
  Collapse
} from "@mui/material";

// Material-UI Icons
import {
  Menu as MenuIcon,
  Close as CloseIcon,
  Dashboard,
  ExitToApp
} from "@mui/icons-material";

// Styles
import "../styles/components/Navbar.css";

// Navigation Configuration
const NAVIGATION_ITEMS = [
  { path: '/', label: 'ANA SAYFA' },
  { path: '/about', label: 'HAKKIMDA' },
  { path: '/blog', label: 'BLOG' }
];

const LOGO_PATH = "/FSSaruhan-white.png";

export default function Navbar() {
  // Hooks
  const location = useLocation();
  const navigate = useNavigate();
  
  // State
  const [isScrolled, setIsScrolled] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);
  const [token, setToken] = useState(() => localStorage.getItem("auth_token"));

  // Event Handlers
  const handleLogout = async () => {
    try {
      await logout();
      setToken(null);
      setAnchorEl(null);
      navigate("/");
    } catch (error) {
      console.error('Logout error:', error);
    }
  };

  const handleDropdownOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleDropdownClose = () => {
    setAnchorEl(null);
  };

  const toggleMobileMenu = () => {
    setIsMobileMenuOpen(!isMobileMenuOpen);
  };

  const handleNavigation = (path) => {
    navigate(path);
    setIsMobileMenuOpen(false);
  };

  // Effects - Stable token management
  useEffect(() => {
    const handleStorageChange = () => {
      const newToken = localStorage.getItem("auth_token");
      setToken(newToken);
    };
    
    window.addEventListener("storage", handleStorageChange);
    return () => window.removeEventListener("storage", handleStorageChange);
  }, []);

  // Throttled scroll handler to prevent flashing
  useEffect(() => {
    let ticking = false;
    
    const handleScroll = () => {
      if (!ticking) {
        requestAnimationFrame(() => {
          setIsScrolled(window.scrollY > 50);
          ticking = false;
        });
        ticking = true;
      }
    };
    
    window.addEventListener("scroll", handleScroll, { passive: true });
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  // Helpers
  const isActiveRoute = (path) => {
    if (path === '/') return location.pathname === '/';
    return location.pathname.startsWith(path);
  };

  return (
    <AppBar 
      position="fixed" 
      className={`navbar ${isScrolled ? 'scrolled' : ''}`}
      elevation={0}
      sx={{
        background: 'rgba(255, 255, 255, 0.1)',
        backdropFilter: 'blur(20px) saturate(120%)',
        WebkitBackdropFilter: 'blur(20px) saturate(120%)',
        borderBottom: '1px solid rgba(255, 255, 255, 0.15)',
        transition: 'all 0.3s ease',
        ...(isScrolled && {
          background: 'linear-gradient(90deg, rgba(247, 109, 85, 0.9) 0%, rgba(248, 154, 136, 0.9) 100%)',
          backdropFilter: 'blur(20px) saturate(120%)',
          WebkitBackdropFilter: 'blur(20px) saturate(120%)',
          borderBottom: '1px solid rgba(255, 255, 255, 0.2)',
          boxShadow: '0 4px 20px rgba(0,0,0,0.1)'
        })
      }}
    >
      <div className="navbar-container">
        <Toolbar 
          className="navbar-toolbar" 
          disableGutters
        >
          
          {/* Sol taraf: Logo + Navigation */}
          <div style={{ display: 'flex', alignItems: 'center', flex: 1 }}>
            {/* Logo Section */}
            <Link to="/" className="navbar-logo" style={{ textDecoration: 'none' }}>
              <img 
                src={LOGO_PATH} 
                alt="FS Saruhan Logo" 
                style={{ 
                  height: '36px',
                  transition: 'transform 0.2s ease'
                }} 
              />
            </Link>

            {/* Desktop Navigation - Logo yanında */}
            <nav className="navbar-nav">
              {NAVIGATION_ITEMS.map((item) => (
                <Button
                  key={item.path}
                  component={Link}
                  to={item.path}
                  className={`navbar-nav-item ${isActiveRoute(item.path) ? 'active' : ''}`}
                >
                  {item.label}
                </Button>
              ))}
            </nav>
          </div>

          {/* User Actions - Sağ Taraf */}
          <div className="navbar-actions">
            {!token ? (
              <Button 
                component={Link} 
                to="/login" 
                variant="outlined"
                className="navbar-login-btn"
              >
                GİRİŞ
              </Button>
            ) : (
              <div className="navbar-user-menu">
                <IconButton
                  onClick={handleDropdownOpen}
                  className="navbar-avatar-btn"
                >
                  <Avatar className="navbar-avatar">
                    👤
                  </Avatar>
                </IconButton>
                
                <Menu
                  anchorEl={anchorEl}
                  open={Boolean(anchorEl)}
                  onClose={handleDropdownClose}
                  anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                  transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                  className="navbar-dropdown"
                >
                  <MenuItem 
                    onClick={() => { handleNavigation('/admin'); handleDropdownClose(); }}
                    className="navbar-dropdown-item"
                  >
                    <Dashboard sx={{ mr: 1, fontSize: '1rem' }} />
                    Admin Paneli
                  </MenuItem>
                  <Divider />
                  <MenuItem 
                    onClick={handleLogout}
                    className="navbar-dropdown-item logout"
                  >
                    <ExitToApp sx={{ mr: 1, fontSize: '1rem' }} />
                    Çıkış
                  </MenuItem>
                </Menu>
              </div>
            )}
            
            {/* Mobile Menu Toggle */}
            <IconButton
              onClick={toggleMobileMenu}
              className="navbar-mobile-toggle"
            >
              {isMobileMenuOpen ? <CloseIcon /> : <MenuIcon />}
            </IconButton>
          </div>
        </Toolbar>

        {/* Mobile Menu */}
        <Collapse in={isMobileMenuOpen}>
          <div className="navbar-mobile-menu">
            {NAVIGATION_ITEMS.map((item) => (
              <Button
                key={item.path}
                fullWidth
                onClick={() => handleNavigation(item.path)}
                className="navbar-mobile-item"
              >
                {item.label}
              </Button>
            ))}
            
            <Divider className="navbar-mobile-divider" />
            
            {token ? (
              <>
                <Button
                  fullWidth
                  onClick={() => handleNavigation('/admin')}
                  className="navbar-mobile-item"
                >
                  <Dashboard sx={{ mr: 1 }} />
                  Admin Paneli
                </Button>
                <Button
                  fullWidth
                  onClick={handleLogout}
                  className="navbar-mobile-item"
                >
                  <ExitToApp sx={{ mr: 1 }} />
                  Çıkış
                </Button>
              </>
            ) : (
              <Button
                fullWidth
                onClick={() => handleNavigation('/login')}
                className="navbar-mobile-item"
              >
                GİRİŞ
              </Button>
            )}
          </div>
        </Collapse>
      </div>
    </AppBar>
  );
}