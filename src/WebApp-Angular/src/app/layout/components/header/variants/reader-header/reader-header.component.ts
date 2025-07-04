import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../header.component';
import { CommonModule } from '@angular/common';
import { FontSize, ThemeService } from '../../../../services/theme.service';

@Component({
  standalone: true,
  selector: 'app-reader-header',
  imports: [HeaderComponent, CommonModule],
  templateUrl: './reader-header.component.html',
  styleUrl: './reader-header.component.css'
})
export class ReaderHeaderComponent implements OnInit {
  currentFontSize: FontSize = 'medium';

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeService.fontSize$.subscribe(size => {
      this.currentFontSize = size;
    });
  }

  changeFontSize(size: FontSize): void {
    this.themeService.setFontSize(size);
  }
}
