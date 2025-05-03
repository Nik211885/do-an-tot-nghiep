import { Component, OnInit } from '@angular/core';
import { DEFAULT_PUBLIC_LAYOUT } from '../../models/layout-config.interface';
import { LayoutService } from '../../services/layout.service';
import { CommonModule } from '@angular/common';
import { PublicHeaderComponent } from '../../components/header/variants/public-header/public-header.component';
import { PublicFooterComponent } from '../../components/footer/variants/public-footer/public-footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-public-layout',
  imports: [CommonModule, 
    PublicHeaderComponent, 
    PublicFooterComponent,
    RouterOutlet],
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.css'
})
export class PublicLayoutComponent implements OnInit {
  contentClass = DEFAULT_PUBLIC_LAYOUT.contentClass;

  constructor(private layoutService: LayoutService) {}

  ngOnInit(): void {
    this.layoutService.setLayoutConfig(DEFAULT_PUBLIC_LAYOUT);
  }
}
