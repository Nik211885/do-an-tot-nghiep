    using Core.BoundContext.BookAuthoringContext.BookAggregate;
    using Core.BoundContext.ModerationContext.BookApprovalAggregate;

    namespace Application.BoundContext.ModerationContext.ViewModel;

    public class BookApprovalViewModel
    {
        public Guid Id { get;  }
        public Guid BookId { get; }
        public Guid ChapterId { get; }
        public Guid AuthorId { get; }
        public DateTimeOffset SubmittedAt { get;  }
        public string ChapterContent { get;  }
        public string ChapterTitle { get; }
        public int ChapterNumber { get; }
        public string ChapterSlug { get; }
        public string BookTitle { get; }
        public BookApprovalStatus Status { get; }
        public IReadOnlyCollection<ApprovalDecisionViewModel>? DecisionViewModels { get; }
        public CopyrightChapterViewModel? CopyrightChapterViewModel { get; }

        public BookApprovalViewModel(Guid id, Guid bookId, Guid chapterId, 
            Guid authorId, DateTimeOffset submittedAt, 
            string chapterTitle, int chapterNumber, string chapterSlug, string bookTitle,
            string content, BookApprovalStatus status,
            IReadOnlyCollection<ApprovalDecisionViewModel>? decisionViewModels,
            CopyrightChapterViewModel? copyrightChapterViewModel)
        {
            Id = id;
            BookId = bookId;
            ChapterId = chapterId;
            AuthorId = authorId;
            SubmittedAt = submittedAt;
            ChapterContent = content;
            Status = status;
            ChapterTitle = chapterTitle;
            ChapterNumber = chapterNumber;
            ChapterSlug = chapterSlug;
            BookTitle = bookTitle;
            DecisionViewModels = decisionViewModels;
            CopyrightChapterViewModel = copyrightChapterViewModel;
        }
    }

    public static class BookApprovalViewModelMappingExtension
    {
        public static BookApprovalViewModel ToViewModel(this BookApproval bookApproval)
        {
            return new BookApprovalViewModel(
                id: bookApproval.Id,
                bookId: bookApproval.BookId,
                chapterId: bookApproval.ChapterId,
                authorId: bookApproval.AuthorId,
                submittedAt: bookApproval.SubmittedAt,
                chapterTitle: bookApproval.ChapterTitle,
                chapterNumber: bookApproval.ChapterNumber,
                chapterSlug: bookApproval.ChapterSlug,
                bookTitle: bookApproval.BookTitle,
                content: bookApproval.ChapterContent,
                status: bookApproval.Status,
                decisionViewModels: bookApproval.Decision.ToViewModel(),
                copyrightChapterViewModel: bookApproval.CopyrightChapter?.ToViewModel()
            );
        }
    }

