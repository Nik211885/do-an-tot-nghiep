import { Component, inject, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { AuthService } from './core/auth/auth.service';
import { BehaviorSubject } from 'rxjs';
import { DiffDOM } from 'diff-dom';
import { getDelta, undoDiffJson } from './core/utils/diff-content.until';
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
    const newValue = `<div>hello</div><div>world</div>`;
    const oldValue = `<div>hello</div><div>back back world</div>`;

    const delta = getDelta(oldValue, newValue);
    console.log('Delta:', delta);
    const undo = undoDiffJson(newValue, delta);
    console.log('Undo:', undo);
  }
}
