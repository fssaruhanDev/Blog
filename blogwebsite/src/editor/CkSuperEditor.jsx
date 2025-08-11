import { useEffect, useRef, useState } from 'react';

// CKEditor 5 Classic Build loader + base64 upload adapter (guarded)
export default function CkSuperEditor({ value, onChange }) {
  const elRef = useRef(null);
  const editorRef = useRef(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let isMounted = true;

    const ensureScript = () => new Promise((resolve, reject) => {
      try {
  if (window.CKEDITOR?.ClassicEditor || window.ClassicEditor) return resolve();
  const existing = document.querySelector('script[src*="ckeditor.com/ckeditor5/"]');
        if (existing) {
          const done = () => (window.CKEDITOR?.ClassicEditor || window.ClassicEditor) ? resolve() : reject(new Error('CKEditor script loaded but ClassicEditor not found'));
          existing.addEventListener('load', done, { once: true });
          existing.addEventListener('error', () => reject(new Error('CKEditor script failed to load')), { once: true });
          if (existing.readyState === 'complete' || existing.getAttribute('data-loaded') === 'true') {
            done();
          }
          return;
        }
        const s = document.createElement('script');
  s.src = 'https://cdn.ckeditor.com/ckeditor5/41.4.2/classic/ckeditor.js';
        s.async = true;
        s.onload = () => resolve();
        s.onerror = () => reject(new Error('CKEditor script failed to load'));
        s.setAttribute('data-loaded', 'true');
        document.head.appendChild(s);
      } catch (e) { reject(e); }
    });

  function Base64UploadAdapterPlugin(editor) {
      const repo = editor?.plugins?.get ? editor.plugins.get('FileRepository') : null;
      if (!repo) return; // Classic may not include FileRepository without imageUpload
      repo.createUploadAdapter = (loader) => ({
        upload: async () => {
          const file = await loader.file;
          const dataUrl = await new Promise((res, rej) => {
            const reader = new FileReader();
            reader.onload = () => res(reader.result);
            reader.onerror = rej;
            reader.readAsDataURL(file);
          });
          return { default: dataUrl };
        },
        abort: () => {}
      });
  }

    const create = async () => {
      try {
        setLoading(true);
        await ensureScript();
        if (!isMounted || !elRef.current) return;
        const ClassicEditor = window.CKEDITOR?.ClassicEditor || window.ClassicEditor;
        if (!ClassicEditor) throw new Error('CKEditor global not found (Classic/Super)');

        const editor = await ClassicEditor.create(elRef.current, {
          // Classic build has only OSS plugins by default; no license key needed
          readOnly: false,
          toolbar: {
            items: [
              'heading', '|',
              'bold', 'italic', 'underline', 'removeFormat', '|',
              'link', 'blockQuote', 'codeBlock', 'horizontalLine', '|',
              'bulletedList', 'numberedList', '|', 'outdent', 'indent', 'alignment', '|',
              'insertTable', 'imageUpload', 'mediaEmbed', '|',
              'undo', 'redo'
            ]
          },
          // Table features
          table: {
            contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells']
          },
          // Keep base features only
          mediaEmbed: { previewsInData: true },
          // Rich list options
          list: { properties: { styles: true, startIndex: true, reversed: true } },
          // Classic build doesn’t include cloud/premium plugins, so no need to remove
        });

        // Attach a base64 upload adapter if FileRepository exists (some builds may not include it)
        try {
          const repo = editor?.plugins?.get ? editor.plugins.get('FileRepository') : null;
          if (repo) {
            repo.createUploadAdapter = (loader) => ({
              upload: async () => {
                const file = await loader.file;
                const dataUrl = await new Promise((res, rej) => {
                  const reader = new FileReader();
                  reader.onload = () => res(reader.result);
                  reader.onerror = rej;
                  reader.readAsDataURL(file);
                });
                return { default: dataUrl };
              },
              abort: () => {}
            });
          }
        } catch {}

  editor.setData(value || '');
        editor.model.document.on('change:data', () => {
          const data = editor.getData();
          onChange?.(data);
        });

        // Provide simple base64 upload without server if FileRepository available
        try {
          const repo = editor?.plugins?.get ? editor.plugins.get('FileRepository') : null;
          if (repo) {
            repo.createUploadAdapter = (loader) => ({
              upload: async () => {
                const file = await loader.file;
                const dataUrl = await new Promise((res, rej) => {
                  const reader = new FileReader();
                  reader.onload = () => res(reader.result);
                  reader.onerror = rej;
                  reader.readAsDataURL(file);
                });
                return { default: dataUrl };
              },
              abort: () => {}
            });
          }
        } catch {}

        editorRef.current = editor;
        setError('');
      } catch (e) {
        console.error('CKEditor init error:', e);
        if (isMounted) setError(e?.message || 'CKEditor yüklenemedi');
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    create();

    return () => {
      isMounted = false;
      if (editorRef.current) {
        editorRef.current.destroy().catch(() => {});
        editorRef.current = null;
      }
    };
  }, []);

  useEffect(() => {
    if (editorRef.current && value != null && value !== editorRef.current.getData()) {
      editorRef.current.setData(value);
    }
  }, [value]);

  return (
    <div className="ck-editor-wrapper">
      {error && <div className="alert alert-warning">CKEditor açılamadı: {error}</div>}
      <div ref={elRef} />
      {loading && !error && <div className="text-muted small mt-2">CKEditor yükleniyor…</div>}
    </div>
  );
}
