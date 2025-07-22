const BlogData = [
  {
    id: 1,
    title: "CQRS ile Neden Tanıştım?",
    summary: "CQRS ve Command/Query ayrımı ile tanışmamın hikayesi.",
    content: `
## CQRS Nedir?

CQRS, Command ve Query işlemlerinin ayrılması prensibine dayanır.

\`\`\`js
if (action === "command") {
  mutate();
} else {
  fetchData();
}
\`\`\`

- Kod okunabilirliği artar
- Performans avantajı sağlar
    `,
    date: "2025-07-13",
    image: "/images/BlogExample.png",
    tags: ["CQRS", "Mimari", "Backend"],
  },
  {
    id: 2,
    title: "Microservice Mimarisi Neden Tercih Edilmeli?",
    summary: "Microservice mimarisine geçiş sürecimizi ve yaşadığımız avantajları anlatıyorum.",
    content: `
## Microservice ile Tanışma

Her servisin bağımsız deployment avantajı var.

\`\`\`bash
docker-compose up -d
\`\`\`

- Ölçeklenebilirlik
- Ekip bağımsızlığı
    `,
    date: "2025-07-10",
    image: "/images/BlogExample.png",
    tags: ["Microservice", "Docker", "Yazılım Mimarisi"],
  },
  {
    id: 3,
    title: "Event Sourcing’e Giriş",
    summary: "Verilerin nasıl birer olay olarak kaydedildiğini keşfedin.",
    content: `
## Event Sourcing Nedir?

Her değişiklik bir event olarak kaydedilir.

\`\`\`json
{
  "event": "UserCreated",
  "data": { "name": "Fatih" }
}
\`\`\`

- Geçmişe dönük sorgulama
- Sistem davranışı takibi
    `,
    date: "2025-06-28",
    image: "/images/BlogExample.png",
    tags: ["Event Sourcing", "Domain Driven Design", "Backend"],
  },
  {
    id: 4,
    title: "Redis ile Cache Yönetimi",
    summary: "Performansı artırmak için Redis ile cache yönetimi nasıl yapılır?",
    content: `
## Redis Kullanımı

Performansı artırmak için sık kullanılan verileri cache'e alın.

\`\`\`js
await redis.set("key", JSON.stringify(data));
\`\`\`

- Hızlı okuma
- Trafik yükünü azaltma
    `,
    date: "2025-06-14",
    image: "/images/BlogExample.png",
    tags: ["Redis", "Cache", "Performans"],
  },
  {
    id: 5,
    title: "Serilog ile Gelişmiş Loglama",
    summary: "Serilog ile yapılandırılmış ve detaylı loglar nasıl alınır?",
    content: `
## Serilog Nedir?

Structured logging destekleyen bir .NET loglama kütüphanesidir.

\`\`\`csharp
Log.Information("Kullanıcı giriş yaptı: {UserId}", userId);
\`\`\`

- ElasticSearch entegrasyonu
- JSON log formatı
    `,
    date: "2025-06-01",
    image: "/images/BlogExample.png",
    tags: ["Logging", "Serilog", ".NET"],
  },
  {
    id: 6,
    title: "Docker Compose ile Çoklu Servis Yönetimi",
    summary: "Projeni birden fazla servis ile nasıl ayağa kaldırırsın? İşte cevabı!",
    content: `
## Docker Compose Kullanımı

Tüm servisleri tek bir komutla ayağa kaldırın.

\`\`\`yaml
version: "3"
services:
  app:
    build: .
  redis:
    image: redis
\`\`\`

- Merkezi yapılandırma
- Hızlı başlatma
    `,
    date: "2025-05-22",
    image: "/images/BlogExample.png",
    tags: ["Docker", "DevOps", "CI/CD"],
  },
  {
    id: 7,
    title: "Git Hooks ile Kod Kalitesini Otomatik Koruyun",
    summary: "Pre-commit hook'ları ile hatalı kodları commit etmeden yakalayın.",
    content: `
## Git Hooks Nedir?

Kod kalitesini korumak için commit öncesi tetiklenen komutlardır.

\`\`\`bash
npx husky add .husky/pre-commit "npm run lint"
\`\`\`

- Otomatik test
- Kod format kontrolü
    `,
    date: "2025-05-12",
    image: "/images/BlogExample.png",
    tags: ["Git", "CI", "Yazılım Kalitesi"],
  },
  {
    id: 8,
    title: "Yazılımda SOLID Prensipleri",
    summary: "Yazılım geliştirmede sağlam temellerin sırrı: SOLID.",
    content: `
## SOLID Nedir?

SOLID, 5 temel prensipten oluşur:

1. Single Responsibility
2. Open/Closed
3. Liskov Substitution
4. Interface Segregation
5. Dependency Inversion

\`\`\`csharp
public interface IRepository<T> { ... }
\`\`\`

- Kod okunabilirliği
- Esnek yapı
    `,
    date: "2025-05-01",
    image: "/images/BlogExample.png",
    tags: ["SOLID", "OOP", "Clean Code"],
  },
];

export default BlogData;
