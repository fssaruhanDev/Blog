import { Link, Outlet, useLocation } from "react-router-dom";
// AdminLTE assets are loaded via CDN in index.html

export default function AdminLayout() {
  const location = useLocation();
  return (
    <div className="hold-transition sidebar-mini layout-fixed">
      <div className="wrapper">
        <nav className="main-header navbar navbar-expand navbar-white navbar-light">
          <ul className="navbar-nav">
            <li className="nav-item">
              <a className="nav-link" data-widget="pushmenu" href="#" role="button">
                <i className="fas fa-bars"></i>
              </a>
            </li>
            <li className="nav-item d-none d-sm-inline-block">
              <Link to="/" className="nav-link">Siteye Dön</Link>
            </li>
          </ul>
        </nav>

        <aside className="main-sidebar sidebar-dark-primary elevation-4">
          <Link to="/admin" className="brand-link text-center">
            <span className="brand-text font-weight-light">Admin Panel</span>
          </Link>
          <div className="sidebar">
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
              </ul>
            </nav>
          </div>
        </aside>

        <div className="content-wrapper p-3">
          <section className="content">
            <div className="container-fluid">
              <Outlet />
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
