namespace Application.Models;

public class ChapterEmbeddingModel
{
    public string Id { get; set; }
    public string BookId { get; set; }
    public string ChapterId { get; set; }
    public string ChapterTitle { get; set; }
    public string AuthorId { get; set; }
    public string Content { get; set; }
    public float[] Embeddings { get; set; }
    public int EmbeddingDim { get; set; }
    public int WordCout { get; set; }
    public int CharCout { get;set; }
    public string ModelName { get; set; }
}
