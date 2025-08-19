// Project types for filtering and display
export const PROJECT_TYPES = {
  WEB_APPLICATION: { value: 1, label: "Web Application", icon: "🌐" },
  MOBILE_APPLICATION: { value: 2, label: "Mobile Application", icon: "📱" },
  DESKTOP_APPLICATION: { value: 3, label: "Desktop Application", icon: "💻" },
  WEBSITE: { value: 4, label: "Website", icon: "🖥️" },
  ECOMMERCE: { value: 5, label: "E-Commerce", icon: "🛒" },
  API: { value: 6, label: "API", icon: "🔌" },
  LIBRARY: { value: 7, label: "Library", icon: "📚" },
  PLUGIN: { value: 8, label: "Plugin", icon: "🔧" },
  THEME: { value: 9, label: "Theme", icon: "🎨" },
  OTHER: { value: 10, label: "Other", icon: "📦" }
};

// Project statuses
export const PROJECT_STATUSES = {
  PLANNING: { value: 1, label: "Planning", color: "#6B7280", icon: "📋" },
  IN_PROGRESS: { value: 2, label: "In Progress", color: "#F59E0B", icon: "⚡" },
  COMPLETED: { value: 3, label: "Completed", color: "#10B981", icon: "✅" },
  ON_HOLD: { value: 4, label: "On Hold", color: "#F97316", icon: "⏸️" },
  CANCELLED: { value: 5, label: "Cancelled", color: "#EF4444", icon: "❌" }
};

// Helper functions
export function getProjectTypeLabel(value) {
  const type = Object.values(PROJECT_TYPES).find(t => t.value === value);
  return type ? type.label : "Unknown";
}

export function getProjectTypeIcon(value) {
  const type = Object.values(PROJECT_TYPES).find(t => t.value === value);
  return type ? type.icon : "📦";
}

export function getProjectStatusLabel(value) {
  const status = Object.values(PROJECT_STATUSES).find(s => s.value === value);
  return status ? status.label : "Unknown";
}

export function getProjectStatusColor(value) {
  const status = Object.values(PROJECT_STATUSES).find(s => s.value === value);
  return status ? status.color : "#6B7280";
}

export function getProjectStatusIcon(value) {
  const status = Object.values(PROJECT_STATUSES).find(s => s.value === value);
  return status ? status.icon : "❓";
}

// Default project grid settings
export const PROJECT_GRID_SETTINGS = {
  DEFAULT_PAGE_SIZE: 12,
  FEATURED_COUNT: 6,
  RELATED_COUNT: 4,
  MOBILE_PAGE_SIZE: 6
};