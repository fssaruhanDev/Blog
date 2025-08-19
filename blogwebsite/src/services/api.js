// Simple API helper for the frontend
// Reads base URL from Vite env (VITE_API_BASE_URL)

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:52888";

// Auth management
export function logout() {
  localStorage.removeItem("auth_token");
  localStorage.removeItem("auth_user");
  // Clear any other user-related data
  localStorage.removeItem(NEWS_LS_KEY);
  localStorage.removeItem(PENDING_OPS_KEY);
}

export function isAuthenticated() {
  return !!localStorage.getItem("auth_token");
}

export function isTokenValid() {
  const token = localStorage.getItem("auth_token");
  if (!token) return false;
  
  try {
    // JWT token'ın payload kısmını decode et
    const payload = JSON.parse(atob(token.split('.')[1]));
    const currentTime = Date.now() / 1000;
    
    // Token'ın expire olup olmadığını kontrol et
    return payload.exp > currentTime;
  } catch (error) {
    // Token geçersizse false döndür
    return false;
  }
}

// Global handler for 401 redirects
let authRedirectHandler = null;
export function setAuthRedirectHandler(handler) {
  authRedirectHandler = handler;
}

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
    // Network/CORS errors do not have a response; surface clearer message + probable cause
    const raw = networkErr?.message || "Network error";
    let hint = '';
    if (/failed to fetch/i.test(raw) || /network/i.test(raw)) {
      hint = ' (API sunucusu kapalı olabilir veya CORS/sertifika problemi var)';
    }
    throw new Error(`Bağlantı hatası: ${raw}${hint}. URL: ${API_BASE_URL}${path}`);
  }

  const contentType = res.headers.get("content-type") || "";
  const data = contentType.includes("application/json") ? await res.json() : await res.text();

  if (!res.ok) {
    // Handle 401 Unauthorized - Auto logout and redirect
    if (res.status === 401) {
      logout();
      if (authRedirectHandler) {
        authRedirectHandler("/login");
      }
    }
    
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

// ===== PUBLIC POSTS API (Anasayfa/Blog için) =====
export function getPublicPosts({ page = 1, pageSize = 20, search = "", featuredOnly = false } = {}) {
  const params = new URLSearchParams();
  params.set("page", page);
  params.set("pageSize", pageSize);
  if (search) params.set("search", search);
  if (featuredOnly) params.set("featuredOnly", "true");
  return request(`/api/public/posts?${params.toString()}`);
}

export function getPublicPost(id) {
  return request(`/api/public/posts/${id}`);
}

export function getFeaturedPosts({ page = 1, pageSize = 6 } = {}) {
  return request(`/api/public/posts/featured?page=${page}&pageSize=${pageSize}`);
}

// ===== ADMIN POSTS API (Admin paneli için) =====
export function getAdminPosts({ page = 1, pageSize = 20, search = "", status = "" } = {}) {
  const params = new URLSearchParams();
  params.set("page", page);
  params.set("pageSize", pageSize);
  if (search) params.set("search", search);
  if (status) params.set("status", status);
  return request(`/api/admin/posts?${params.toString()}`);
}

export function getAdminPost(id) {
  return request(`/api/admin/posts/${id}`);
}

export function createPost({ title, excerpt, content, coverImageUrl, status = "draft", publishedAt = null }) {
  return request(`/api/admin/posts`, {
    method: "POST",
    body: { Title: title, Excerpt: excerpt, Content: content, CoverImageUrl: coverImageUrl, Status: status, PublishedAt: publishedAt },
  });
}

export function updatePost(id, { title, excerpt, content, coverImageUrl, status = "draft", publishedAt = null }) {
  return request(`/api/admin/posts/${id}`, {
    method: "PUT",
    body: { Title: title, Excerpt: excerpt, Content: content, CoverImageUrl: coverImageUrl, Status: status, PublishedAt: publishedAt },
  });
}

// Internal helper to build auth header for non-JSON multipart calls
function buildAuthHeader(){
  if (typeof localStorage === 'undefined') return {};
  const token = localStorage.getItem('auth_token');
  return token ? { Authorization: `Bearer ${token}` } : {};
}

// Upload image (returns {url}) with 401 handling
export async function uploadImage(file){
  const form = new FormData();
  form.append('file', file);
  const res = await fetch(`${API_BASE_URL}/api/MediaUpload/image`, {
    method:'POST',
    body: form,
    headers: { ...buildAuthHeader() }
  });
  
  if (res.status === 401) {
    logout();
    if (authRedirectHandler) {
      authRedirectHandler("/login");
    }
  }
  
  if(!res.ok){
    const txt = await res.text();
    throw new Error(txt || 'Upload hata');
  }
  return res.json();
}

export function deletePost(id) {
  return request(`/api/admin/posts/${id}`, { method: "DELETE" });
}

// ----- Comments -----
export function getComments(postId){
  return request(`/api/posts/${postId}/comments`);
}
export function addComment(postId,{authorName, authorEmail, content}){
  return request(`/api/posts/${postId}/comments`, { method:'POST', body:{ authorName, authorEmail, content } });
}

export { API_BASE_URL };

// Media URL resolver: accepts stored value (possibly '/uploads/..', absolute, or external)
export function resolveMediaUrl(raw) {
  if (!raw) return '';
  const trimmed = raw.trim();
  if (/^https?:\/\//i.test(trimmed)) return trimmed; // already absolute
  if (trimmed.startsWith('/')) return `${API_BASE_URL.replace(/\/$/, '')}${trimmed}`;
  // fallback: treat as relative under uploads
  if (trimmed.includes('/uploads/')) {
    const idx = trimmed.indexOf('/uploads/');
    return `${API_BASE_URL.replace(/\/$/, '')}${trimmed.substring(idx)}`;
  }
  return trimmed;
}

// Basit API canlılık kontrolü (sunucu kapalıysa hızlı döner)
export async function checkApiStatus(timeoutMs = 2500) {
  const ctrl = new AbortController();
  const t = setTimeout(()=>ctrl.abort(), timeoutMs);
  try {
    const res = await fetch(`${API_BASE_URL}/api/news?page=1&pageSize=1`, { method: 'GET', signal: ctrl.signal });
    clearTimeout(t);
    return res.ok;
  } catch {
    clearTimeout(t);
    return false;
  }
}

// ---------------------------
// News API with local fallback
// ---------------------------

function lsGet(key, def) {
  try { return JSON.parse(localStorage.getItem(key) || JSON.stringify(def)); } catch { return def; }
}
function lsSet(key, val) { try { localStorage.setItem(key, JSON.stringify(val)); } catch {} }
const NEWS_LS_KEY = 'news_items_v1';
const PENDING_OPS_KEY = 'pending_ops_v1';
const OFFLINE_FLAG = '_pending';
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
  const item = { id: uid(), title, summary, sourceUrl, sourceName, tags, status, publishedAt, createdDate: new Date().toISOString(), [OFFLINE_FLAG]: true };
  all.push(item); lsSet(NEWS_LS_KEY, all); return item;
}

function newsLocalUpdate(id, patch) {
  const all = lsGet(NEWS_LS_KEY, []);
  const idx = all.findIndex(n => n.id === id);
  if (idx === -1) throw new Error('News bulunamadı');
  all[idx] = { ...all[idx], ...patch, [OFFLINE_FLAG]: true };
  lsSet(NEWS_LS_KEY, all); return all[idx];
}

function newsLocalDelete(id) {
  const all = lsGet(NEWS_LS_KEY, []);
  const next = all.filter(n => n.id !== id); lsSet(NEWS_LS_KEY, next); return { ok: true };
}

// -------------- Offline Queue --------------
function loadQueue() { return lsGet(PENDING_OPS_KEY, []); }
function saveQueue(q) { lsSet(PENDING_OPS_KEY, q); }
function enqueue(op) { const q = loadQueue(); q.push(op); saveQueue(q); }

async function processQueue() {
  const q = loadQueue();
  if (!q.length) return { processed: 0 };
  let processed = 0;
  const remaining = [];
  for (const op of q) {
    try {
      if (op.type === 'createNews') {
        const resp = await createNews(op.payload);
        // replace local temp item with server item (match by sourceUrl if exists, else by temp local id stored in op.tempId)
        if (resp && (resp.ID || resp.id)) {
          const all = lsGet(NEWS_LS_KEY, []);
          let idx = -1;
          if (op.tempId) idx = all.findIndex(n => n.id === op.tempId);
          if (idx === -1 && op.payload.sourceUrl) idx = all.findIndex(n => n.sourceUrl === op.payload.sourceUrl);
            if (idx !== -1) {
              all[idx] = {
                id: resp.ID || resp.id,
                title: resp.Title || resp.title,
                summary: resp.Summary || resp.summary,
                sourceUrl: resp.SourceUrl || resp.sourceUrl,
                sourceName: resp.SourceName || resp.sourceName,
                status: resp.Status || resp.status,
                publishedAt: resp.PublishedAt || resp.publishedAt,
                createdDate: resp.CreatedDate || resp.createdDate,
                tags: (resp.Tags || resp.tags || '')?.split?.(',') || resp.tags || [],
              };
              lsSet(NEWS_LS_KEY, all);
            }
        }
      } else if (op.type === 'updateNews') {
        await updateNews(op.id, op.payload);
      } else if (op.type === 'deleteNews') {
        await deleteNews(op.id);
      }
      processed++;
    } catch (e) {
      // keep op for next round
      op.lastError = e.message;
      remaining.push(op);
    }
  }
  saveQueue(remaining);
  return { processed, remaining: remaining.length };
}

let autoSyncStarted = false;
export function startAutoSync(intervalMs = 10000) {
  if (autoSyncStarted) return;
  autoSyncStarted = true;
  const tick = async () => {
    const up = await checkApiStatus(1500);
    if (!up) return;
    try { await processQueue(); } catch { /* ignore */ }
  };
  setInterval(tick, intervalMs);
  window.addEventListener('online', () => setTimeout(tick, 500));
  // initial
  tick();
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
  const tagsValue = Array.isArray(tags) ? tags.join(',') : (tags || '');
  const body = { Title: title, Summary: summary, SourceName: sourceName, SourceUrl: sourceUrl, Status: status, PublishedAt: publishedAt, Tags: tagsValue };
  try {
    const resp = await request(`/api/news`, { method: 'POST', body });
    return resp;
  } catch (e) {
    console.warn('createNews fallback (queued):', e.message);
    const local = newsLocalCreate({ title, summary, sourceUrl, sourceName, tags, status, publishedAt });
    enqueue({ type: 'createNews', payload: { title, summary, sourceUrl, sourceName, tags, status, publishedAt }, tempId: local.id, createdAt: Date.now() });
    return local;
  }
}

export async function updateNews(id, { title, summary, sourceUrl, sourceName, tags = [], status = 'draft', publishedAt = null }) {
  const tagsValue = Array.isArray(tags) ? tags.join(',') : (tags || '');
  const body = { Id: id, Title: title, Summary: summary, SourceName: sourceName, SourceUrl: sourceUrl, Status: status, PublishedAt: publishedAt, Tags: tagsValue };
  try {
    const resp = await request(`/api/news/${id}`, { method: 'PUT', body });
    return resp;
  } catch (e) {
    console.warn('updateNews fallback (queued):', e.message);
    const updated = newsLocalUpdate(id, { title, summary, sourceUrl, sourceName, tags, status, publishedAt });
    enqueue({ type: 'updateNews', id, payload: { title, summary, sourceUrl, sourceName, tags, status, publishedAt }, createdAt: Date.now() });
    return updated;
  }
}

export async function deleteNews(id) {
  try { return await request(`/api/news/${id}`, { method: 'DELETE' }); } catch (e) { console.warn('deleteNews fallback (queued):', e.message); enqueue({ type: 'deleteNews', id, createdAt: Date.now() }); return newsLocalDelete(id); }
}
