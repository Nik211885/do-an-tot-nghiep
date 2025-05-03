import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FooterComponent } from '../../footer.component';

@Component({
  standalone: true,
  selector: 'app-reader-footer',
  imports: [CommonModule, FooterComponent],
  templateUrl: './reader-footer.component.html',
  styleUrl: './reader-footer.component.css'
})
export class ReaderFooterComponent {

}
