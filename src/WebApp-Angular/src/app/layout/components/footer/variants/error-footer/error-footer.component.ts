import { Component } from '@angular/core';
import { FooterComponent } from '../../footer.component';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-error-footer',
  imports: [CommonModule, FooterComponent],
  templateUrl: './error-footer.component.html',
  styleUrl: './error-footer.component.css'
})
export class ErrorFooterComponent {

}
