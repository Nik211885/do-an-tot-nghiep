import { Component, inject, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
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
    const version1 = "<div>xin chao</div>"
    const version2  = "<div>xin chao toi ten la Le Khac Ninh</div>"
    const version3  = "<div>xin chao, toi ten Le Le Khac Nam</div>"
    const history1 = getDelta(version1, version2);
    const history2 = getDelta(version2, version3);
    console.log("history 1 = "+ history1);
    console.log("history 2 = " +history2);
  }
}
