import { environment } from "../../../environments/environment"

export class AuthConfig{
  public url: string = environment.keyCloak.url
  public publicUrl: string = environment.publicUrl
  public realm: string = environment.keyCloak.realm
  public clientId: string = environment.keyCloak.clientId

  public defaultLoginRedirectUri: string = '/dashboard';
  public loginRoute: string = '/login';
  public unauthorizedRoute: string = '/unauthorized';
}
