import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-internal-server',
  standalone: true,
  imports: [],
  templateUrl: './internal-server.component.html',
  styleUrl: './internal-server.component.css'
})
export class InternalServerComponent {
  constructor(
    private location: Location,
    private router: Router
  ) {}
  goHome(): void{
    this.router.navigate(['/']);
  }
  tryAgain() : void{
    
  }
}
