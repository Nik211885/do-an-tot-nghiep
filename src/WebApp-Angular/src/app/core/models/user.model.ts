export interface UserModel {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  attributes?: Record<string, any>;
}
