import { Routes, Route, useLocation, useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import About from './selections/About/About';
import Blog from './selections/Blog/Blog';
import BlogDetails from './selections/Blog/BlogDetails';
import Home from './selections/Home/Home';
import Login from './selections/Auth/Login';
import ROUTES from './constants/routes';
import AdminRoute from './admin/AdminRoute';
import AdminLayout from './admin/AdminLayout';
import Dashboard from './admin/pages/Dashboard/Dashboard';
import News from './admin/pages/News/News';
import NewsEditor from './admin/pages/News/NewsEditor';
import AdminBlog from './admin/pages/Blog/Blog';
import BlogEditor from './admin/pages/Blog/BlogEditor';
import Achievements from './admin/pages/Achievements/Achievements';
import Pages from './admin/pages/Pages/Pages';
import { setAuthRedirectHandler } from './services/api';

function App() {
  const location = useLocation();
  const navigate = useNavigate();
  const isAdmin = location.pathname.startsWith('/admin');

  // Set up global auth redirect handler
  useEffect(() => {
    setAuthRedirectHandler((path) => {
      navigate(path, { replace: true });
    });
  }, [navigate]);

  return (
    <>
      {!isAdmin && <Navbar />}
      <main className="main-content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/blog" element={<Blog />} />
          <Route path="/blog/:id" element={<BlogDetails />} />
          <Route path="/login" element={<Login />} />

          <Route element={<AdminRoute />}>
            <Route path="/admin" element={<AdminLayout />}>
              <Route index element={<Dashboard />} />
              <Route path="news" element={<News />} />
              <Route path="news/new" element={<NewsEditor />} />
              <Route path="news/:id/edit" element={<NewsEditor />} />
              <Route path="blog" element={<AdminBlog />} />
              <Route path="blog/new" element={<BlogEditor />} />
              <Route path="blog/:id/edit" element={<BlogEditor />} />
              <Route path="achievements" element={<Achievements />} />
              <Route path="pages" element={<Pages />} />
            </Route>
          </Route>

        </Routes>
      </main>
      {!isAdmin && <Footer />}
    </>
  );
}


export default App;
