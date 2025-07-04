import { Component, inject, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ToastComponent } from './shared/components/toast/toast.component';
import { DialogComponent } from './shared/components/dialog/dialog.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, ToastComponent, DialogComponent],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  constructor(@Inject(PLATFORM_ID) private readonly platFormId: object){}
  ngOnInit(){

  }
}
