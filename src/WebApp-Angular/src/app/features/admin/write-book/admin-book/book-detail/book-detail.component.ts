import {Component, OnInit, Renderer2} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ActivatedRoute, Router} from '@angular/router';
import {Bookv1, Genre} from '../../models/book.model';
import {BookService} from '../../services/book.service';
import {ImageUploadComponent} from '../../../../../shared/components/image-upload/image-upload.component';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {GenresService} from '../../services/genere.service';
import {BookPolicy, BookReleaseType, UpdateBookCommand} from '../../models/create-book.model';
import {ToastService} from '../../../../../shared/components/toast/toast.service';


@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [CommonModule, ImageUploadComponent, ReactiveFormsModule, FormsModule],
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnInit {
  book!: Bookv1;
  form!: FormGroup;
  genres!: Genre[];
  bookId!: string;
  isPaid: boolean = false;
  policy!: BookPolicy;
  selectedGenres: string[] = [];
  constructor( private route: ActivatedRoute,
               private toastService: ToastService,
               private fb: FormBuilder,
               private genreService: GenresService,
               private bookService: BookService) {
    this.form = fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      policy: [BookPolicy, Validators.required],
      tagName: ['', Validators.required],
      price: [0, Validators.required],
    })
  }

  ngOnInit(): void {
      this.bookId = this.route.snapshot.params['id'];
      this.bookService.getBookById(this.bookId).subscribe(
        {
          next: result => {
            if (result) {
              this.book = result;
              this.form.patchValue({
                title: this.book.title,
                description: this.book.description,
                price: this.book.price,
              });
              this.selectedGenres = this.book.genres.map(x => x.id);
              this.policy = this.book.isPaid ? BookPolicy.Paid
                : this.book.requiresRegistration ? BookPolicy.Subscription
                  : BookPolicy.Free
              if (this.policy == BookPolicy.Paid) {
                this.isPaid = true;
              }
            }
          },
          error: error => {
            console.log(error);
          }
        }
      );
      this.genreService.getAllGenre().subscribe({
        next: result => {
          this.genres = result;
        }
      })
  }

  addTag() {
    const tagControl = this.form.get('tagName');
    const newTag = tagControl?.value?.trim();
    if (newTag && tagControl?.valid && !this.book.tags.includes(newTag)) {
      this.book.tags.push(newTag);
      tagControl.setValue('');
    }
  }

  removeTag(tag: string) {
    this.book.tags = this.book.tags.filter(t=>t !== tag);
  }

  toggleGenre(genre: Genre) {
    if(this.selectedGenres.includes(genre.id)){
      this.selectedGenres = this.selectedGenres.filter(g => g !== genre.id);
    }
    else{
      this.selectedGenres.push(genre.id)
    }
  }

  protected readonly BookPolicy = BookPolicy;

  changePolicy(policy: BookPolicy) {
    this.policy = policy;
    if(policy == BookPolicy.Paid){
      this.isPaid =true;
    }
    else{
      this.isPaid = false;
      this.form.patchValue({
        price:0
      })
    }
  }

  updateBook() {
    console.log("Update book")
    this.bookService.updateBook({
      title: this.form.value.title,
      avatarUrl: this.book.coverImage,
      description: this.form.value.description,
      readerBookPolicy: this.policy,
      bookReleaseType: this.book.isCompleted ? BookReleaseType.Complete : BookReleaseType.Serialized,
      tagsName: this.book.tags,
      readerBookPolicyPrice: this.form.value.price,
      genreIds: this.selectedGenres,
      visibility: true,
    } as UpdateBookCommand, this.bookId).subscribe({
      next: result => {
        this.book = result;
        this.toastService.success("Cập nhật thông tin sách thành công")
      },
      error: error => {
        this.toastService.error("Có lôi trong quá trình xử lý")
      }
    })
  }
}
