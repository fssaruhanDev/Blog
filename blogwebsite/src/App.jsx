import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import About from './selections/About/About';
import Blog from './selections/Blog/Blog';
import BlogDetails from './selections/Blog/BlogDetails';
import Home from './selections/Home/Home';
import ROUTES from './constants/routes';

function App() {
  return (
    <>
         
      <Navbar />
      <main className="main-content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/blog" element={<Blog />} />
          <Route path="/blog/:id" element={<BlogDetails />} />

        </Routes>
      </main>
      <Footer />
    </>
  );
}


export default App;
