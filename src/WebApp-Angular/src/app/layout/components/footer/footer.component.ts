import { Component, Input } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-footer',
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {
  @Input() variant: 'public' | 'admin' | 'reader' | 'error' = 'public';
}
