import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header.component';

@Component({
  standalone: true,
  selector: 'app-error-header',
  imports: [HeaderComponent],
  templateUrl: './error-header.component.html',
  styleUrl: './error-header.component.css'
})
export class ErrorHeaderComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
