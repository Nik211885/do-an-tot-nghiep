import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, of } from "rxjs";
import { Book } from "../models/book.model";
import {Genere} from "../models/genere.model"

@Injectable({
    providedIn: "root"
})

export class GenereService{
    private genres: Genere[] = [
        {
            id: '1',
            name: 'Khoa học viễn tưởng',
            description: 'Tiểu thuyết tưởng tượng khám phá khoa học và công nghệ tiên tiến, du hành vũ trụ, du hành thời gian, vũ trụ song song và nhiều hơn thế nữa.',
            slug: 'khoa-hoc-vien-tuong',
            bookCount: 5,
            imageUrl: 'https://images.pexels.com/photos/1169754/pexels-photo-1169754.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2'
        },
        {
            id: '2',
            name: 'Giả tưởng',
            description: 'Tiểu thuyết có yếu tố phép thuật và siêu nhiên không tồn tại trong thế giới thực.',
            slug: 'gia-tuong',
            bookCount: 4,
            imageUrl: 'https://images.pexels.com/photos/5363891/pexels-photo-5363891.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2'
        },
        {
            id: '3',
            name: 'Trinh thám',
            description: 'Tiểu thuyết xoay quanh một vụ án (thường là giết người) từ lúc xảy ra đến khi được phát hiện và giải quyết.',
            slug: 'trinh-tham',
            bookCount: 3,
            imageUrl: 'https://images.pexels.com/photos/3646172/pexels-photo-3646172.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2'
        },
        {
            id: '4',
            name: 'Tiểu thuyết lịch sử',
            description: 'Tiểu thuyết lấy bối cảnh trong quá khứ, kết hợp giữa nhân vật và sự kiện hư cấu với những sự kiện, thời kỳ hoặc nhân vật có thật trong lịch sử.',
            slug: 'tieu-thuyet-lich-su',
            bookCount: 4,
            imageUrl: 'https://images.pexels.com/photos/6474475/pexels-photo-6474475.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2'
        },
        {
            id: '5',
            name: 'Lãng mạn',
            description: 'Tiểu thuyết tập trung vào câu chuyện tình yêu giữa hai người.',
            slug: 'lang-man',
            bookCount: 6,
            imageUrl: 'https://images.pexels.com/photos/1024975/pexels-photo-1024975.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2'
        },
        {
            id: '6',
            name: 'Giật gân',
            description: 'Tiểu thuyết có nhịp độ nhanh, nhiều hành động, với những anh hùng tài giỏi phải chống lại các thế lực mạnh mẽ và nguy hiểm hơn.',
            slug: 'giat-gan',
            bookCount: 5,
            imageUrl: 'https://images.pexels.com/photos/3165335/pexels-photo-3165335.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2'
        }
    ];
    constructor(httpClient: HttpClient){}
    private selectedGenreSubject = new BehaviorSubject<Genere | null>(null);
    selectedGenre$ = this.selectedGenreSubject.asObservable();

    getGeneres(): Observable<Genere[]> {
        return of(this.genres);
    }

    getGenereById(id: string): Observable<Genere | undefined> {
        const genre = this.genres.find(g => g.id === id);
        return of(genre);
    }

    getBooksForGenere(genreId: string): Observable<Book[]> {
        return of([]);
    }
    selectGenere(genre: Genere): void {
        this.selectedGenreSubject.next(genre);
    }
    getGenereBySlug(slug: string) : Observable<Genere| undefined>{
        const genere = this.genres.find(x=>x.slug === slug);
        return of(genere);
    }
}