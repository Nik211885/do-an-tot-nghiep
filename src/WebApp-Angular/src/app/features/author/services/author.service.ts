import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Author } from "../models/author.model";
import { Observable, of } from "rxjs";

@Injectable({
    providedIn: "root"
})

export class AuthorServcie{
    constructor(private httpClient: HttpClient){}
    private readonly author: Author[] = [
        {
            id: "author_001",
            name: "Hầu Khanh",
            bio: "Giang hồ rộng lớn, đạo nghĩa không bao giờ phai nhạt; giữa bão giông cuộc đời, lòng trung nhân vẫn vững như núi.",
            imageUrl: "https://scontent.fhan15-1.fna.fbcdn.net/v/t39.30808-6/470183594_122177464370044785_6848646154707743682_n.jpg?_nc_cat=109&ccb=1-7&_nc_sid=f727a1&_nc_ohc=hxohvb4I9fYQ7kNvwGIhudA&_nc_oc=AdlV-s0EbMMY7tTXirCl_fddCB24Pyh4YyTlxXzgdn1QHbp-F4k3wLzorWinfv2sGT-luww7ER6TBH5aKOnL7eyP&_nc_zt=23&_nc_ht=scontent.fhan15-1.fna&_nc_gid=A8DLL9ozh_wd_HKK5IXqig&oh=00_AfI-3H6givgtA2DiAqUPXRYLr1-9aWhiI_0T_JzhTZVb1Q&oe=682CCA53",
            followersCount: 5421,
            followingCount: 230,
            isFollowing: false,
        },
        {
            id: "author_002",
            name: "Hàng Thần",
            bio: "Thiên hạ hiểm ác, người người tự tranh đoạt; chỉ có kẻ không vướng bụi trần mới có thể giữ trọn thanh danh.",
            imageUrl: "https://scontent.fhan15-1.fna.fbcdn.net/v/t39.30808-6/470215470_122177464640044785_4974866635628738480_n.jpg?_nc_cat=109&ccb=1-7&_nc_sid=f727a1&_nc_ohc=TlaTuWgBCroQ7kNvwGvEtGe&_nc_oc=AdnMUETu5-rMRpYoKPOp9G9bYPmjQqEK8majEZzHsjWGe3GHlLpamQssQO9K4GMUQyq_8bdEFuqyQgEwpMzVd9YF&_nc_zt=23&_nc_ht=scontent.fhan15-1.fna&_nc_gid=RMvnZ9_mSqs5VltB9_5tig&oh=00_AfL2M_h6SV11nIpYSjKcqcEBabeycU8JCzqsGyue-AY0Iw&oe=682CD0C9",
            followersCount: 10532,
            followingCount: 78,
            isFollowing: true,
        },
        {
            id: "author_003",
            name: "Huỳnh Câu",
            bio: "Giang hồ này, người sống không chỉ bằng sức mạnh mà còn bằng tấm lòng chân thật.",
            imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRTPxkfCTJTHSEUUzBQN5R3Rps07PEkEyGnmw&s",
            followersCount: 8923,
            followingCount: 156,
            isFollowing: false,
        },
        {
            id: "author_003",
            name: "Hàn Bạt",
            bio: "Cuộc đời giang hồ là một trận chiến không hồi kết, chỉ kẻ kiên cường mới có thể sống sót đến cùng.",
            imageUrl: "https://vidian.vn/public-img/image-1704609772857.jpg",
            followersCount: 8923,
            followingCount: 156,
            isFollowing: false,
        },
    ];
    getAllAuthor(): Author[]{
        return this.author;
    }
    getAuthorById(id: string) : Author | undefined{
        return this.author.find(x=>x.id === id);
    }
    toggleFollow(authorId: string):  Observable<Author | undefined> {
        const author = this.author.find(author => author.id === authorId);
    
        if (author) {
        author.isFollowing = !author.isFollowing;
        if (author.isFollowing) {
            author.followersCount += 1;
        } else {
            author.followersCount -= 1;
        }
        }
        
        return of(author);
    }
}