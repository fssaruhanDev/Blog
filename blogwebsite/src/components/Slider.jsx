import { Swiper, SwiperSlide } from 'swiper/react';
import { Autoplay, Navigation, Pagination } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';
import '../styles/Slider.css';

const sliderItems = [
  {
    img: '/slider/1.png',
    text: "CQRS'i neden seçtim, ya da o mu beni seçti.",
    link: "/blog/cqrs",
  },
  {
    img: '/slider/3.png',
    text: "Ne zaman Microservice kullanmalıyız?",
    link: "/blog/microservice",
  },
  {
    img: '/slider/2.png',
    text: "Bir Projenin batışı çıkış mıdır?",
    link: "/blog/failure",
  }
];

export default function Slider() {
  return (
    <section className="slider-section">
      <Swiper
        modules={[Autoplay, Navigation, Pagination]}
        spaceBetween={30}
        slidesPerView={1}
        navigation
        pagination={{ clickable: true }}
        autoplay={{ delay: 5000 }}
        loop
      >
        {sliderItems.map((item, index) => (
          <SwiperSlide key={index}>
            <div
              className="slide"
              style={{ backgroundImage: `url(${item.img})` }}
            >
              <a href={item.link} className="slide-text">
                {item.text}
              </a>
            </div>
          </SwiperSlide>
        ))}
      </Swiper>
    </section>
  );
}
