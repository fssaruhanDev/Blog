// Simple API helper for the frontend
// Reads base URL from Vite env (VITE_API_BASE_URL)

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:52888";

async function request(path, { method = "GET", body, token } = {}) {
  const headers = { "Content-Type": "application/json" };
  // Auto-attach JWT from localStorage if not explicitly provided
  const bearer = token || (typeof localStorage !== "undefined" ? localStorage.getItem("auth_token") : null);
  if (bearer) headers["Authorization"] = `Bearer ${bearer}`;

  let res;
  try {
    res = await fetch(`${API_BASE_URL}${path}`, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined,
    });
  } catch (networkErr) {
    // Network/CORS errors do not have a response; surface a clear message
    const msg = networkErr?.message || "Network error";
    throw new Error(`İstek başarısız: ${msg}. URL: ${API_BASE_URL}${path}`);
  }

  const contentType = res.headers.get("content-type") || "";
  const data = contentType.includes("application/json") ? await res.json() : await res.text();

  if (!res.ok) {
    const message = typeof data === "string" ? data : data?.message || data?.Message || "Request failed";
    const statusInfo = `HTTP ${res.status}${res.statusText ? ` ${res.statusText}` : ""}`;
    throw new Error(`${statusInfo}: ${message}`);
  }

  return data;
}

export function login(userName, password) {
  return request("/api/user/login", {
    method: "POST",
  // C# binder is case-insensitive, but we align casing to the model for clarity
  body: { UserName: userName, Password: password },
  });
}

export function getPosts({ page = 1, pageSize = 20, search = "", status = "" } = {}) {
  const params = new URLSearchParams();
  params.set("page", page);
  params.set("pageSize", pageSize);
  if (search) params.set("search", search);
  if (status) params.set("status", status);
  return request(`/api/posts?${params.toString()}`);
}

export function createPost({ title, excerpt, content, status = "draft", publishedAt = null }) {
  return request(`/api/posts`, {
    method: "POST",
    body: { Title: title, Excerpt: excerpt, Content: content, Status: status, PublishedAt: publishedAt },
  });
}

export function updatePost(id, { title, excerpt, content, status = "draft", publishedAt = null }) {
  return request(`/api/posts/${id}`, {
    method: "PUT",
    body: { Title: title, Excerpt: excerpt, Content: content, Status: status, PublishedAt: publishedAt },
  });
}

export function deletePost(id) {
  return request(`/api/posts/${id}`, { method: "DELETE" });
}

export function getPost(id) {
  return request(`/api/posts/${id}`);
}

export { API_BASE_URL };

// ---------------------------
// News API with local fallback
// ---------------------------

function lsGet(key, def) {
  try { return JSON.parse(localStorage.getItem(key) || JSON.stringify(def)); } catch { return def; }
}
function lsSet(key, val) { try { localStorage.setItem(key, JSON.stringify(val)); } catch {} }
const NEWS_LS_KEY = 'news_items_v1';
const uid = () => `${Date.now()}-${Math.random().toString(36).slice(2,8)}`;

function newsLocalList({ page = 1, pageSize = 20, search = '', status = '' } = {}) {
  const all = lsGet(NEWS_LS_KEY, []);
  let items = [...all].sort((a,b)=> new Date(b.publishedAt||0) - new Date(a.publishedAt||0));
  if (search) {
    const q = search.toLowerCase();
    items = items.filter(n => (n.title||'').toLowerCase().includes(q) || (n.summary||'').toLowerCase().includes(q) || (n.sourceName||'').toLowerCase().includes(q));
  }
  if (status) items = items.filter(n => (n.status||'draft') === status);
  const total = items.length;
  const start = (page-1)*pageSize;
  const paged = items.slice(start, start+pageSize);
  return { items: paged, total, page, pageSize };
}

function newsLocalGet(id) {
  const all = lsGet(NEWS_LS_KEY, []);
  return all.find(n => n.id === id);
}

function newsLocalCreate({ title, summary, sourceUrl, sourceName, tags = [], status = 'draft', publishedAt = null }) {
  const all = lsGet(NEWS_LS_KEY, []);
  const item = { id: uid(), title, summary, sourceUrl, sourceName, tags, status, publishedAt, createdDate: new Date().toISOString() };
  all.push(item); lsSet(NEWS_LS_KEY, all); return item;
}

function newsLocalUpdate(id, patch) {
  const all = lsGet(NEWS_LS_KEY, []);
  const idx = all.findIndex(n => n.id === id);
  if (idx === -1) throw new Error('News bulunamadı');
  all[idx] = { ...all[idx], ...patch }; lsSet(NEWS_LS_KEY, all); return all[idx];
}

function newsLocalDelete(id) {
  const all = lsGet(NEWS_LS_KEY, []);
  const next = all.filter(n => n.id !== id); lsSet(NEWS_LS_KEY, next); return { ok: true };
}

export async function getNews(params = {}) {
  try {
    const p = new URLSearchParams();
    if (params.page) p.set('page', params.page);
    if (params.pageSize) p.set('pageSize', params.pageSize);
    if (params.search) p.set('search', params.search);
    if (params.status) p.set('status', params.status);
    return await request(`/api/news?${p.toString()}`);
  } catch (e) {
    console.warn('getNews fallback to local:', e.message);
    return newsLocalList(params);
  }
}

export async function getNewsItem(id) {
  try { return await request(`/api/news/${id}`); } catch (e) { console.warn('getNewsItem fallback:', e.message); return newsLocalGet(id); }
}

export async function createNews({ title, summary, sourceUrl, sourceName, tags = [], status = 'draft', publishedAt = null }) {
  try {
    return await request(`/api/news`, { method: 'POST', body: { Title: title, Summary: summary, SourceUrl: sourceUrl, SourceName: sourceName, Tags: tags, Status: status, PublishedAt: publishedAt } });
  } catch (e) {
    console.warn('createNews fallback:', e.message);
    return newsLocalCreate({ title, summary, sourceUrl, sourceName, tags, status, publishedAt });
  }
}

export async function updateNews(id, { title, summary, sourceUrl, sourceName, tags = [], status = 'draft', publishedAt = null }) {
  try {
    return await request(`/api/news/${id}`, { method: 'PUT', body: { Title: title, Summary: summary, SourceUrl: sourceUrl, SourceName: sourceName, Tags: tags, Status: status, PublishedAt: publishedAt } });
  } catch (e) {
    console.warn('updateNews fallback:', e.message);
    return newsLocalUpdate(id, { title, summary, sourceUrl, sourceName, tags, status, publishedAt });
  }
}

export async function deleteNews(id) {
  try { return await request(`/api/news/${id}`, { method: 'DELETE' }); } catch (e) { console.warn('deleteNews fallback:', e.message); return newsLocalDelete(id); }
}
