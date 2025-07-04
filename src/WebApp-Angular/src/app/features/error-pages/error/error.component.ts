import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [],
  templateUrl: './error.component.html',
  styleUrl: './error.component.css'
})
export class ErrorComponent {
  statusCode: Number = 0;
  constructor(private routes: ActivatedRoute,private location: Location,
    private router: Router){
    this.routes.params.subscribe(params=>{
      this.statusCode = params['status-code']
    })
  }
  goHome(): void{
    this.router.navigate(['/']);
  }
  goBack(): void{
    this.location.back();  
  }
}
