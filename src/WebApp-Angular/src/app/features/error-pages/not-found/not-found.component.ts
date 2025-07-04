import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  imports: [],
  standalone: true,
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css'
})
export class NotFoundComponent {
  constructor(
    private location: Location,
    private router: Router
  ) {}
  goHome(): void{
    this.router.navigate(['/']);
  }
  goBack(): void{
    this.location.back();  
  }
}
