import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import Keycloak, { KeycloakProfile } from 'keycloak-js';

@Injectable({
  providedIn: 'root'
})
export class AuthKeyCloakService {
  private keyCloak: Keycloak
  constructor() { 
    this.keyCloak = new Keycloak({
      clientId: 'book_store_angular_client',
      realm: 'book_store_realm',
      url: 'http://localhost:8080'
    });
  }
  async init() {
    await this.keyCloak.init({
      onLoad:'check-sso',
      pkceMethod: 'S256'
    })
  }
  login() : Promise<void>{
    return new Promise((resolve, reject)=>{
      this.keyCloak.login().then(()=>{
        resolve()
      }).catch(err=>{
        console.error('login services error', err)
        reject(err)
      })
    })
  }
  logout() : void{
    this.keyCloak?.logout()
  }
  isAuthenticated(): boolean{
    return this.keyCloak?.authenticated ?? false;
  }
  async loadProfile() : Promise<KeycloakProfile>{
    return await this.keyCloak?.loadUserProfile()
  }
  async updateToken(): Promise<void>{
    try{
      const refreshed = await this.keyCloak?.updateToken()
      if(refreshed){
      }
      else{
        console.log('token still valid, no need refresh')
      }
    }
    catch(ex){
      console.log("Error process update token")
      this.logout()
    }
  }
  register(): void{
    this.keyCloak.register()
  }
  getToken() : string | undefined{
    return this.keyCloak.token;
  }
}
