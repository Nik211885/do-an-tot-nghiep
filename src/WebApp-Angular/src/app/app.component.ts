import { Component, inject, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { AuthService } from './core/auth/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  constructor(@Inject(PLATFORM_ID) private readonly platFormId: object,
  private readonly authService: AuthService){}
  async ngOnInit(): Promise<void> {
    if(isPlatformBrowser(this.platFormId)){
      this.authService.initialize()
    }
  }
  Login(){
    this.authService.login("/");
  }
  Logout(){
    this.authService.logout();
  }
}
