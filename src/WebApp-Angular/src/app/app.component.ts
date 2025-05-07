import { Component, inject, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { AuthService } from './core/auth/auth.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  constructor(@Inject(PLATFORM_ID) private readonly platFormId: object){}
  ngOnInit(){
    
  }
}
