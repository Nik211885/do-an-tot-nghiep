using Application.Interfaces.Elastic;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;

namespace Infrastructure.Services.Elastic;

public class PlagiarismCheckerService : IPlagiarismCheckerService
{
    private readonly ElasticsearchClient _client;
    private readonly string _indexKey = "plagiarism-index";

    public PlagiarismCheckerService(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task AddBulkDocumentChunkAsync(IEnumerable<DocumentChunk> document)
    {
        var bulkRequest = new BulkRequest(_indexKey) { Operations = new List<IBulkOperation>() };
        foreach (var doc in document)
        {
            bulkRequest.Operations.Add(new BulkIndexOperation<DocumentChunk>(doc)
            {
                Id = $"{doc.DocId}_{doc.ChunkIndex}"
            });
        }
        var response = await _client.BulkAsync(bulkRequest);
        if (!response.IsValidResponse)
        {
            throw new Exception($"Bulk indexing failed: {response?.DebugInformation ?? "No debug information available"}");
        }
    }

    public async Task<List<PlagiarismMatch>> CheckSimilarityAsync(float[] queryEmbedding, string docId, int topK = 5)
    {
        // Check if index exists, create if it doesn't
        var indexExists = await _client.Indices.ExistsAsync(_indexKey);
        if (!indexExists.Exists)
        {
            var settings = new IndexSettings { NumberOfShards = 1, NumberOfReplicas = 1 };
            var mapping = new TypeMapping
            {
                Properties = new Properties
                {
                    { "DocId", new KeywordProperty() },
                    { "ChunkIndex", new IntegerNumberProperty() },
                    { "Content", new TextProperty() },
                    {
                        "Embedding", new DenseVectorProperty
                        {
                            Dims = 768,
                            Index = true 
                        }
                    }
                }
            };
            var createResponse = await _client.Indices.CreateAsync(_indexKey, c => c
                .Settings(s => s
                    .NumberOfShards(1)
                    .NumberOfReplicas(1)
                )
                .Mappings(m => m
                    .Properties(mapping.Properties)
                )
            );
            if (!createResponse.IsValidResponse)
            {
                throw new Exception($"Failed to create index: {createResponse.DebugInformation}");
            }
        }

        // Perform kNN search
        var searchResponse = await _client.SearchAsync<DocumentChunk>(s => s
            .Index(_indexKey)
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Knn(k => k
                            .Field(f => f.Embedding)
                            .QueryVector(queryEmbedding)
                            .NumCandidates(topK * 10)
                        )
                    )
                    .MustNot(mn => mn
                        .Term(t => t
                            .Field(f => f.DocId.Suffix("keyword"))
                            .Value(docId)
                        )
                    )
                )
            )
        );

        if (!searchResponse.IsValidResponse)
        {
            throw new Exception($"Search failed: {searchResponse.DebugInformation}");
        }

        // Aggregate results by DocId
        var docMatches = new Dictionary<string, (List<float> Scores, List<string> Texts)>();
        foreach (var hit in searchResponse.Hits)
        {
            var chunk = hit.Source;
            if (chunk == null) continue;

            var similarity = (float)hit.Score; // kNN score is the cosine similarity
            if (!docMatches.ContainsKey(chunk.DocId))
            {
                docMatches[chunk.DocId] = (new List<float>(), new List<string>());
            }
            docMatches[chunk.DocId].Scores.Add(similarity);
            docMatches[chunk.DocId].Texts.Add(chunk.Content);
        }

        // Compute aggregated similarity scores per document
        var plagiarismMatches = new List<PlagiarismMatch>();
        foreach (var kvp in docMatches)
        {
            var docIdMatch = kvp.Key;
            var scores = kvp.Value.Scores;
            var texts = kvp.Value.Texts;

            // Aggregate similarity: weighted average based on chunk count
            var avgSimilarity = scores.Average();

            // Combine text snippets (e.g., first chunk or concatenated preview)
            var combinedText = string.Join(" ", texts.Take(1)); // Take first chunk for preview

            plagiarismMatches.Add(new PlagiarismMatch(
                docIdMatch,
                combinedText,
                (float)Math.Round(avgSimilarity, 4)
            ));
        }

        // Sort by similarity and take topK
        return plagiarismMatches
            .OrderByDescending(m => m.Similarity)
            .Take(topK)
            .ToList();
    }
}
