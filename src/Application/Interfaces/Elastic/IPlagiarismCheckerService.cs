    namespace Application.Interfaces.Elastic;

    public interface IPlagiarismCheckerService
    {
        Task AddBulkDocumentChunkAsync(List<DocumentChunk> document);
        Task<List<PlagiarismMatch>> CheckSimilarityAsync(float[] queryEmbedding, string docId, int topK =  5);
    }

    public class PlagiarismMatch
    {
        public string DocId { get;}
        public string Text { get;  }
        public float Similarity { get; }

        public PlagiarismMatch(string docId, string text, float similarity)
        {
            DocId = docId;
            Text = text;
            Similarity = similarity;
        }
    }


    public class DocumentChunk
    {
        public string DocId { get;}
        public int DocCout { get;  }
        public int ChunkCount {get;}
        public string Content { get; }
        public int ChunkIndex {get;}
        public float[] Embedding { get; }

        public DocumentChunk(string docId, int docCout, int chunkCount, string content, int chunkIndex, float[] embedding)
        {
            DocId = docId;
            DocCout = docCout;
            ChunkCount = chunkCount;
            Content = content;
            ChunkIndex = chunkIndex;
            Embedding = embedding;
        }
    }
