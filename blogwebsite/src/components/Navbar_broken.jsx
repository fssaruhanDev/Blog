import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { logout, isTokenValid } from '../services/api';
import { 
  AppBar, 
  Toolbar, 
  Button, 
  Box, 
  IconButton, 
  Avatar, 
  Menu, 
  MenuItem, 
  Container,
  Typography,
  Divider,
  Collapse
} from '@mui/material';
import { 
  AccountCircle, 
  Menu as MenuIcon, 
  Dashboard, 
  ExitToApp,
  Close 
} from '@mui/icons-material';
import '../styles/Navbar.css';

export default function Navbar() {
  const navigate = useNavigate();
  const location = useLocation();
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);

  const handleUserMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleUserMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = async () => {
    try {
      await logout();
      setIsLoggedIn(false);
      handleUserMenuClose();
      navigate('/');
    } catch (error) {
      console.error('Logout error:', error);
    }
  };

  const handleLogin = () => {
    navigate('/login');
  };

  const toggleMobileMenu = () => {
    setIsMobileMenuOpen(!isMobileMenuOpen);
  };

  useEffect(() => {
    const checkLoginStatus = async () => {
      const valid = await isTokenValid();
      setIsLoggedIn(valid);
    };
    
    checkLoginStatus();
  }, []);

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 50);
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const navigationItems = [
    { label: 'ANA SAYFA', path: '/' },
    { label: 'HAKKIMDA', path: '/hakkimda' },
    { label: 'BLOG', path: '/blog' }
  ];

  return (
    <AppBar 
      position="fixed" 
      elevation={0}
      className={`navbar ${isScrolled ? 'scrolled' : ''}`}
      sx={{ 
        background: 'transparent',
        transition: 'all 0.3s ease',
        backdropFilter: 'blur(10px)',
        ...(isScrolled && {
          background: 'linear-gradient(90deg, #f76d55 0%, #f89a88 100%)',
          boxShadow: '0 4px 20px rgba(0,0,0,0.1)'
        })
      }}
    >
      <Container maxWidth="xl">
        <Toolbar 
          disableGutters 
          sx={{ 
            minHeight: '64px !important',
            display: 'flex',
            alignItems: 'center',
            maxWidth: '1200px',
            mx: 'auto',
            width: '100%',
            px: { xs: 2, md: 4 }
          }}
        >
          {/* Logo - Sol */}
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <Link to="/" style={{ display: 'flex', alignItems: 'center', textDecoration: 'none' }}>
              <img 
                src="/FSSaruhan-white.png" 
                alt="Logo" 
                style={{ height: '36px' }}
              />
            </Link>
          </Box>

          {/* Navigation Links - Orta (Hidden on mobile) */}
          <Box 
            sx={{ 
              display: { xs: 'none', md: 'flex' },
              alignItems: "center",
              gap: 4,
              flex: 1,
              justifyContent: 'center'
            }}
          >
            {navigationItems.map((item) => (
              <Button
                key={item.path}
                component={Link}
                to={item.path}
                sx={{
                  color: '#ffffff',
                  fontSize: '0.875rem',
                  fontWeight: 600,
                  letterSpacing: '1.5px',
                  position: 'relative',
                  padding: '8px 16px',
                  '&:hover': {
                    backgroundColor: 'rgba(255,255,255,0.1)',
                    borderRadius: '4px'
                  },
                  ...(location.pathname === item.path && {
                    '&::after': {
                      content: '""',
                      position: 'absolute',
                      bottom: '-2px',
                      left: '50%',
                      transform: 'translateX(-50%)',
                      width: '40px',
                      height: '2px',
                      backgroundColor: '#fff',
                      borderRadius: '1px'
                    }
                  })
                }}
              >
                {item.label}
              </Button>
            ))}
          </Box>

          {/* Login/User Section - Sağ */}
          <Box sx={{ display: { xs: 'none', md: 'flex' }, alignItems: 'center', gap: 2 }}>
            {isLoggedIn ? (
              <>
                <IconButton
                  onClick={handleUserMenuOpen}
                  className="user-avatar"
                  sx={{
                    width: 36,
                    height: 36,
                    backgroundColor: 'rgba(255,255,255,0.15)',
                    color: '#fff',
                    border: '1px solid rgba(255,255,255,0.2)',
                    '&:hover': {
                      backgroundColor: 'rgba(255,255,255,0.25)',
                      transform: 'scale(1.05)'
                    }
                  }}
                >
                  <AccountCircle />
                </IconButton>
                
                <Menu
                  anchorEl={anchorEl}
                  open={Boolean(anchorEl)}
                  onClose={handleUserMenuClose}
                  className="dropdown-menu"
                  anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'right',
                  }}
                  transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                  }}
                  sx={{
                    mt: 1,
                    '& .MuiPaper-root': {
                      minWidth: 180,
                      borderRadius: '8px',
                      boxShadow: '0 8px 32px rgba(0,0,0,0.12)',
                      border: '1px solid rgba(0,0,0,0.08)'
                    }
                  }}
                >
                  <MenuItem 
                    onClick={() => { navigate('/admin'); handleUserMenuClose(); }}
                    className="dropdown-item"
                    sx={{
                      fontSize: '0.875rem',
                      padding: '12px 16px',
                      '&:hover': {
                        backgroundColor: 'rgba(247,109,85,0.08)'
                      }
                    }}
                  >
                    <Dashboard sx={{ mr: 1, fontSize: '1rem' }} />
                    Admin Paneli
                  </MenuItem>
                  <Divider />
                  <MenuItem 
                    onClick={handleLogout}
                    className="dropdown-item logout"
                    sx={{
                      fontSize: '0.875rem',
                      padding: '12px 16px',
                      color: '#d32f2f',
                      '&:hover': {
                        backgroundColor: 'rgba(211,47,47,0.08)'
                      }
                    }}
                  >
                    <ExitToApp sx={{ mr: 1, fontSize: '1rem' }} />
                    Logout
                  </MenuItem>
                </Menu>
              </>
            ) : (
              <Button
                onClick={handleLogin}
                variant="outlined"
                sx={{
                  color: '#fff',
                  borderColor: 'rgba(255,255,255,0.3)',
                  fontSize: '0.875rem',
                  fontWeight: 600,
                  letterSpacing: '1px',
                  px: 3,
                  py: 1,
                  '&:hover': {
                    borderColor: '#fff',
                    backgroundColor: 'rgba(255,255,255,0.1)'
                  }
                }}
              >
                LOGIN
              </Button>
            )}
          </Box>

          {/* Mobile Menu Button */}
          <IconButton
            onClick={toggleMobileMenu}
            sx={{
              display: { xs: 'flex', md: 'none' },
              color: '#fff',
              padding: '8px'
            }}
          >
            {isMobileMenuOpen ? <Close /> : <MenuIcon />}
          </IconButton>
        </Toolbar>

        {/* Mobile Menu */}
        <Collapse in={isMobileMenuOpen}>
          <Box 
            sx={{
              background: 'rgba(247,109,85,0.98)',
              backdropFilter: 'blur(10px)',
              padding: '16px 24px',
              borderTop: '1px solid rgba(255,255,255,0.1)',
              display: { xs: 'block', md: 'none' }
            }}
          >
            {navigationItems.map((item) => (
              <Button
                key={item.path}
                component={Link}
                to={item.path}
                fullWidth
                onClick={() => setIsMobileMenuOpen(false)}
                sx={{
                  color: '#fff',
                  fontSize: '0.875rem',
                  fontWeight: 600,
                  letterSpacing: '1.2px',
                  padding: '12px 16px',
                  borderRadius: '6px',
                  justifyContent: 'flex-start',
                  marginBottom: '4px',
                  '&:hover': {
                    backgroundColor: 'rgba(255,255,255,0.1)'
                  }
                }}
              >
                {item.label}
              </Button>
            ))}
            
            <Divider sx={{ 
              backgroundColor: 'rgba(255,255,255,0.1)', 
              margin: '8px 0' 
            }} />
            
            {isLoggedIn ? (
              <>
                <Button
                  onClick={() => { navigate('/admin'); setIsMobileMenuOpen(false); }}
                  fullWidth
                  sx={{
                    color: '#fff',
                    fontSize: '0.875rem',
                    fontWeight: 600,
                    letterSpacing: '1.2px',
                    padding: '12px 16px',
                    borderRadius: '6px',
                    justifyContent: 'flex-start',
                    marginBottom: '4px',
                    '&:hover': {
                      backgroundColor: 'rgba(255,255,255,0.1)'
                    }
                  }}
                >
                  <Dashboard sx={{ mr: 1 }} />
                  Admin Paneli
                </Button>
                <Button
                  onClick={() => { handleLogout(); setIsMobileMenuOpen(false); }}
                  fullWidth
                  sx={{
                    color: '#fff',
                    fontSize: '0.875rem',
                    fontWeight: 600,
                    letterSpacing: '1.2px',
                    padding: '12px 16px',
                    borderRadius: '6px',
                    justifyContent: 'flex-start',
                    '&:hover': {
                      backgroundColor: 'rgba(255,255,255,0.1)'
                    }
                  }}
                >
                  <ExitToApp sx={{ mr: 1 }} />
                  Logout
                </Button>
              </>
            ) : (
              <Button
                onClick={() => { handleLogin(); setIsMobileMenuOpen(false); }}
                fullWidth
                sx={{
                  color: '#fff',
                  fontSize: '0.875rem',
                  fontWeight: 600,
                  letterSpacing: '1.2px',
                  padding: '12px 16px',
                  borderRadius: '6px',
                  justifyContent: 'flex-start',
                  '&:hover': {
                    backgroundColor: 'rgba(255,255,255,0.1)'
                  }
                }}
              >
                LOGIN
              </Button>
            )}
          </Box>
        </Collapse>
      </Container>
    </AppBar>
  );
}