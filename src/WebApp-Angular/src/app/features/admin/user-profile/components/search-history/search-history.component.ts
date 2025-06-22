import {Component, OnInit} from '@angular/core';
import {UserProfileService} from '../../services/user-profile.service';
import {PaginationSearchHistoryViewModel} from '../../models/search-history.model';

@Component({
  selector: 'app-search-history',
  imports: [],
  standalone: true,
  templateUrl: './search-history.component.html',
  styleUrl: './search-history.component.css'
})
export class SearchHistoryComponent implements OnInit {
  paginationSearchHistory!: PaginationSearchHistoryViewModel;
  constructor(private readonly userProfileServices: UserProfileService) {
  }
  ngOnInit(): void {
    this.userProfileServices.getHistorySearch(1,10)
      .subscribe({
        next: result => {
          this.paginationSearchHistory = result;
          console.log(this.paginationSearchHistory);
        },
        error: error => {
          console.error(error);
        }
      })
  }
}
