export interface SidebarItem {
  id: string;
  label: string;
  icon?: string;
  route?: string;
  children?: SidebarItem[];
  permissions?: string[];
  divider?: boolean;
  badge?: {
    text: string;
    color: string;
  };
}
