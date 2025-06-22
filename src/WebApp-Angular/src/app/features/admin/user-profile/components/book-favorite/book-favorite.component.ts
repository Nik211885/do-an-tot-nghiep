import {Component, OnInit} from '@angular/core';
import {UserProfileService} from '../../services/user-profile.service';
import {PaginationFavoriteBookViewModel} from '../../models/favorite-book.model';

@Component({
  selector: 'app-book-favorite',
  standalone: true,
  imports: [],
  templateUrl: './book-favorite.component.html',
  styleUrl: './book-favorite.component.css'
})
export class BookFavoriteComponent  implements OnInit {
  paginationFavoriteBook!: PaginationFavoriteBookViewModel;
  constructor(private userProfileService: UserProfileService) {
  }
    ngOnInit(): void {
        this.userProfileService
          .getFavoriteBook(1,10)
          .subscribe({
            next: (value)=>{
              this.paginationFavoriteBook = value;
              console.log(this.paginationFavoriteBook);
            },
            error:(err)=>{
              console.error(err);
            }
          })
    }

}
