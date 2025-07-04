export interface LayoutConfig {
  headerType: 'public' | 'admin' | 'reader' | 'error';
  footerType: 'public' | 'admin' | 'reader' | 'error';
  showSidebar: boolean;
  sidebarExpanded: boolean;
  showBreadcrumb: boolean;
  showHeader: boolean;
  showFooter: boolean;
  fullWidth: boolean;
  contentClass: string;
}

export const DEFAULT_PUBLIC_LAYOUT: LayoutConfig = {
  headerType: 'public',
  footerType: 'public',
  showSidebar: false,
  sidebarExpanded: false,
  showBreadcrumb: false,
  showHeader: true,
  showFooter: true,
  fullWidth: false,
  contentClass: 'py-8',
};

export const DEFAULT_ADMIN_LAYOUT: LayoutConfig = {
  headerType: 'admin',
  footerType: 'admin',
  showSidebar: true,
  sidebarExpanded: true,
  showBreadcrumb: true,
  showHeader: true,
  showFooter: true,
  fullWidth: true,
  contentClass: 'p-6',
};

export const DEFAULT_READER_LAYOUT: LayoutConfig = {
  headerType: 'reader',
  footerType: 'reader',
  showSidebar: false,
  sidebarExpanded: false,
  showBreadcrumb: false,
  showHeader: true,
  showFooter: true,
  fullWidth: false,
  contentClass: 'py-6',
};

export const DEFAULT_ERROR_LAYOUT: LayoutConfig = {
  headerType: 'error',
  footerType: 'error',
  showSidebar: false,
  sidebarExpanded: false,
  showBreadcrumb: false,
  showHeader: true,
  showFooter: true,
  fullWidth: false,
  contentClass: 'py-8',
};
