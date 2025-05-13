import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User, Comment } from '../../../shared/components/comment/comment.component';

@Injectable({
  providedIn: 'root'
})
export class CommentBookService {
  private commentsSubject = new BehaviorSubject<Comment[]>([]);
  public comments$: Observable<Comment[]> = this.commentsSubject.asObservable();
  
  private currentUser: User = {
    id: 'user-1',
    name: 'Current User',
    avatar: ''
  };

  constructor() {
    // Initialize with demo data in a real app, you would fetch from an API
    this.commentsSubject.next(this.generateDemoComments());
  }

  getCurrentUser(): User {
    return this.currentUser;
  }

  getComments(): Comment[] {
    return this.commentsSubject.value;
  }

  addComment(content: string): void {
    const newComment: Comment = {
      id: `comment-${Date.now()}`,
      content,
      user: this.currentUser,
      createdAt: new Date(),
      replies: []
    };
    
    const updatedComments = [newComment, ...this.commentsSubject.value];
    this.commentsSubject.next(updatedComments);
  }

  addReply(parentId: string, content: string): void {
    const newReply: Comment = {
      id: `comment-${Date.now()}`,
      content,
      user: this.currentUser,
      createdAt: new Date(),
      replies: []
    };
    
    const updatedComments = this.addReplyToComment(
      this.commentsSubject.value, 
      parentId, 
      newReply
    );
    
    this.commentsSubject.next(updatedComments);
  }

  private addReplyToComment(comments: Comment[], parentId: string, newReply: Comment): Comment[] {
    return comments.map(comment => {
      if (comment.id === parentId) {
        return {
          ...comment,
          replies: [...comment.replies, newReply]
        };
      } else if (comment.replies.length > 0) {
        return {
          ...comment,
          replies: this.addReplyToComment(comment.replies, parentId, newReply)
        };
      }
      return comment;
    });
  }

  private generateDemoComments(): Comment[] {
    return [
      {
        id: 'comment-1',
        content: 'This is a great article! I especially liked the section about responsive design patterns.',
        user: {
          id: 'user-2',
          name: 'Jane Smith',
          avatar: ''
        },
        createdAt: new Date(Date.now() - 3600000 * 2), // 2 hours ago
        replies: [
          {
            id: 'comment-2',
            content: 'I agree! The examples were really helpful too.',
            user: {
              id: 'user-3',
              name: 'Alex Johnson',
              avatar: ''
            },
            createdAt: new Date(Date.now() - 3600000), // 1 hour ago
            replies: [
              {
                id: 'comment-3',
                content: 'Which example did you find most useful? I thought the navigation patterns were particularly well explained.',
                user: {
                  id: 'user-2',
                  name: 'Jane Smith',
                  avatar: ''
                },
                createdAt: new Date(Date.now() - 1800000), // 30 minutes ago
                replies: []
              }
            ]
          }
        ]
      }
    ];
  }
  public getCommentByBookId(id: string) : Comment[]{
    return [
      {
        id: 'comment-1',
        content: 'This is a great article! I especially liked the section about responsive design patterns.',
        user: {
          id: 'user-2',
          name: 'Jane Smith',
          avatar: ''
        },
        createdAt: new Date(Date.now() - 3600000 * 2), // 2 hours ago
        replies: [
          {
            id: 'comment-2',
            content: 'I agree! The examples were really helpful too.',
            user: {
              id: 'user-3',
              name: 'Alex Johnson',
              avatar: ''
            },
            createdAt: new Date(Date.now() - 3600000), // 1 hour ago
            replies: [
              {
                id: 'comment-3',
                content: 'Which example did you find most useful? I thought the navigation patterns were particularly well explained.',
                user: {
                  id: 'user-2',
                  name: 'Jane Smith',
                  avatar: ''
                },
                createdAt: new Date(Date.now() - 1800000), // 30 minutes ago
                replies: []
              }
            ]
          }
        ]
      },
      {
        id: 'comment-4',
        content: 'Has anyone tried implementing these techniques with Angular Material? I am curious if there are any gotchas to be aware of.',
        user: {
          id: 'user-4',
          name: 'Sam Wilson',
          avatar: ''
        },
        createdAt: new Date(Date.now() - 86400000), // 1 day ago
        replies: []
      }
    ];
  }
}