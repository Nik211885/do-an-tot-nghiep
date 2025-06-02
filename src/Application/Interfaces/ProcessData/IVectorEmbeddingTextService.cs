using System.Text.Json.Serialization;

namespace Application.Interfaces.ProcessData;

public interface IVectorEmbeddingTextService
{
    Task<List<DocSource>?> GetVectorEmbeddingFromTextAsync(string sentence);
}


public class DocSource
{

    [JsonPropertyName("content")]
    public string Content { get;}

    [JsonPropertyName("embedding")]
    public List<float> Embedding { get; } 

    [JsonPropertyName("embedding_dim")]
    public int EmbeddingDim { get;  }

    [JsonPropertyName("token_count")]
    public int TokenCount { get; }

    [JsonPropertyName("word_count")]
    public int WordCount { get;  }

    [JsonPropertyName("char_count")]
    public int CharCount { get; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get;  } 

    [JsonPropertyName("model_name")]
    public string ModelName { get;  }

    public DocSource( string content, List<float> embedding, int embeddingDim, int tokenCount, int wordCount, int charCount, DateTimeOffset createdAt, string modelName)
    {
        Content = content;
        Embedding = embedding;
        EmbeddingDim = embeddingDim;
        TokenCount = tokenCount;
        WordCount = wordCount;
        CharCount = charCount;
        CreatedAt = createdAt;
        ModelName = modelName;
    }
}
