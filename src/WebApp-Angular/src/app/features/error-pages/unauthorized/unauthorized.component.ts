import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  imports: [],
  standalone: true,
  templateUrl: './unauthorized.component.html',
  styleUrl: './unauthorized.component.css'
})
export class UnauthorizedComponent {
  constructor(
    private location: Location,
    private router: Router
  ) {}

  goHome(): void{
    this.router.navigate(['/']);
  }
}
