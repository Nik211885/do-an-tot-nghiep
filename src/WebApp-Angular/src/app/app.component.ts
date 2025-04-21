import { Component, inject, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { AuthKeyCloakService } from './core/auth/auth-key-cloak.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  constructor(@Inject(PLATFORM_ID) private readonly platFormId: object,
  private readonly authKeyCloakService: AuthKeyCloakService){}
  async ngOnInit(): Promise<void> {
    if(isPlatformBrowser(this.platFormId)){
      await this.authKeyCloakService.init()
    }
  }
  
}
