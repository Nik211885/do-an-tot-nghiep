import { Component, OnInit } from '@angular/core';
import { DEFAULT_READER_LAYOUT } from '../../models/layout-config.interface';
import { LayoutService } from '../../services/layout.service';
import { ReaderHeaderComponent } from '../../components/header/variants/reader-header/reader-header.component';
import { ReaderFooterComponent } from '../../components/footer/variants/reader-footer/reader-footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-reader-layout',
  imports: [ReaderHeaderComponent,
     ReaderFooterComponent, 
     RouterOutlet],
  templateUrl: './reader-layout.component.html',
  styleUrl: './reader-layout.component.css'
})
export class ReaderLayoutComponent implements OnInit {
  constructor(private layoutService: LayoutService) {}

  ngOnInit(): void {
    this.layoutService.setLayoutConfig(DEFAULT_READER_LAYOUT);
  }
}
