import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FooterComponent } from '../../footer.component';

@Component({
  standalone: true,
  selector: 'app-public-footer',
  imports: [CommonModule, FooterComponent],
  templateUrl: './public-footer.component.html',
  styleUrl: './public-footer.component.css'
})
export class PublicFooterComponent {

}
