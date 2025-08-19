import { Link, Outlet, useLocation, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { logout } from "../services/api";
import "../styles/AdminBase.css";
// AdminLTE assets are loaded via CDN in index.html

export default function AdminLayout() {
  const location = useLocation();
  const navigate = useNavigate();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  // Hamburger menu toggle
  const toggleSidebar = () => {
    setSidebarOpen(!sidebarOpen);
  };

  // Get user info from localStorage
  const userInfo = JSON.parse(localStorage.getItem("auth_user") || "{}");
  const userName = userInfo.userName || userInfo.UserName || "Admin User";
  
  return (
    <div className={`admin-container ${sidebarOpen ? 'sidebar-collapsed' : ''}`}>
      <div className="hold-transition sidebar-mini layout-fixed">
        <div className="wrapper">
          <nav className="main-header navbar navbar-expand navbar-white navbar-light">
            <ul className="navbar-nav">
              <li className="nav-item">
                <a 
                  className="nav-link" 
                  onClick={toggleSidebar}
                  href="#" 
                  role="button"
                  style={{ cursor: 'pointer' }}
                >
                  <i className="fas fa-bars"></i>
                </a>
              </li>
              <li className="nav-item d-none d-sm-inline-block">
                <Link to="/" className="nav-link">Siteye Dön</Link>
              </li>
            </ul>
          </nav>

          <aside className="main-sidebar sidebar-dark-primary elevation-4">
            {/* Brand Logo - sadece logo, yazı yok */}
            <Link to="/admin" className="brand-link admin-brand-link">
              <img src="/FSSaruhan-white.png" alt="Logo" className="admin-brand-image" />
            </Link>
            
            <div className="sidebar admin-sidebar">
              {/* Sidebar user panel */}
              <div className="user-panel mt-3 pb-3 mb-3 d-flex">
                <div className="image">
                  <img src="/profile-image.png" className="img-circle elevation-2" alt="User Image" />
                </div>
                <div className="info">
                  <a href="#" className="d-block">{userName}</a>
                </div>
              </div>

              <nav className="mt-2">
                <ul className="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                  <li className="nav-item">
                    <Link to="/admin" className={`nav-link ${location.pathname === "/admin" ? "active" : ""}`}>
                      <i className="nav-icon fas fa-home"></i>
                      <p>Ana Sayfa</p>
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link to="/admin/news" className={`nav-link ${location.pathname.startsWith("/admin/news") ? "active" : ""}`}>
                      <i className="nav-icon fas fa-bolt"></i>
                      <p>News</p>
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link to="/admin/blog" className={`nav-link ${location.pathname.startsWith("/admin/blog") ? "active" : ""}`}>
                      <i className="nav-icon fas fa-blog"></i>
                      <p>Blog</p>
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link to="/admin/achievements" className={`nav-link ${location.pathname.startsWith("/admin/achievements") ? "active" : ""}`}>
                      <i className="nav-icon fas fa-trophy"></i>
                      <p>Achievements</p>
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link to="/admin/pages" className={`nav-link ${location.pathname.startsWith("/admin/pages") ? "active" : ""}`}>
                      <i className="nav-icon fas fa-file-alt"></i>
                      <p>Pages</p>
                    </Link>
                  </li>
                  <li className="nav-item">
                    <a 
                      href="#"
                      onClick={(e) => {
                        e.preventDefault();
                        handleLogout();
                      }}
                      className="nav-link"
                    >
                      <i className="nav-icon fas fa-sign-out-alt"></i>
                      <p>Logout</p>
                    </a>
                  </li>
                </ul>
              </nav>
            </div>
          </aside>

          <div className="content-wrapper admin-content-wrapper">
            <section className="content">
              <div className="container-fluid">
                <Outlet />
              </div>
            </section>
          </div>
        </div>
      </div>
    </div>
  );
}
