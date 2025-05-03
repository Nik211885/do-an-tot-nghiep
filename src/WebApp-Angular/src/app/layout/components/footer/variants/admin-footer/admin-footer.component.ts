import { Component } from '@angular/core';
import { FooterComponent } from '../../footer.component';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-admin-footer',
  imports: [CommonModule, FooterComponent],
  templateUrl: './admin-footer.component.html',
  styleUrl: './admin-footer.component.css'
})
export class AdminFooterComponent {

}
