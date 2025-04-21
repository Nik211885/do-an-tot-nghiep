import { Component } from '@angular/core';
import { AuthKeyCloakService } from '../../../core/auth/auth-key-cloak.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(private readonly authKeyCloackService: AuthKeyCloakService){}
  login(){
    this.authKeyCloackService.login()
  }
  register(){
    this.authKeyCloackService.register()
  }
  logout(){
    this.authKeyCloackService.logout()
  }
}
