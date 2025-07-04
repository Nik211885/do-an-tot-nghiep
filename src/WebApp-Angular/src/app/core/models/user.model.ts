export class UserModel {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  attributes?: Record<string, any>;
  constructor(map: Record<string, any>) {
    const { id, username, email, firstName, lastName, roles, ...attributes } = map;
    this.id = id;
    this.username = username;
    this.email = email;
    this.firstName = firstName;
    this.lastName = lastName;
    this.roles = roles;
    this.attributes = attributes || {};
  }
  getFullName(): string {
    return `${this.firstName} ${this.lastName}`.trim();
  }
}
