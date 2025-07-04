import { Component, OnInit } from '@angular/core';
import { DEFAULT_ERROR_LAYOUT } from '../../models/layout-config.interface';
import { LayoutService } from '../../services/layout.service';
import { ErrorHeaderComponent } from '../../components/header/variants/error-header/error-header.component';
import { ErrorFooterComponent } from '../../components/footer/variants/error-footer/error-footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-error-layout',
  imports: [ErrorHeaderComponent, 
    ErrorFooterComponent, 
    RouterOutlet],
  templateUrl: './error-layout.component.html',
  styleUrl: './error-layout.component.css'
})
export class ErrorLayoutComponent implements OnInit {
  constructor(private layoutService: LayoutService) {}

  ngOnInit(): void {
    this.layoutService.setLayoutConfig(DEFAULT_ERROR_LAYOUT);
  }
}
