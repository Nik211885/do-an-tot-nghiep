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

    public async Task AddBulkDocumentChunkAsync(List<DocumentChunk> document)
    {
        if (document.Count == 0)
            return;
        var newChunkIds = document
            .Select(d => $"{d.DocId}_{d.ChunkIndex}")
            .ToList();
        List<string?> existingChunkIds;
        {
            var idsArray = newChunkIds.ToArray();

            var searchResponse = await _client.SearchAsync<DocumentChunk>(s => s
                .Index(_indexKey)                     
                .Size(idsArray.Length)               
                .Query(q => q
                    .Ids(i => i.Values(idsArray))
                )
            );

            if (!searchResponse.IsValidResponse)
                throw new Exception($"Failed to search existing chunks: {searchResponse.DebugInformation}");

            existingChunkIds = searchResponse.Hits
                .Select(h => h.Id)
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();
        }
        if (existingChunkIds.Count > 0)
        {
            var deleteOps = existingChunkIds
                .Select(id => new BulkDeleteOperation<DocumentChunk>(id)
                {
                    Index = _indexKey 
                })
                .Cast<IBulkOperation>()
                .ToList();

            var deleteBulkRequest = new BulkRequest
            {
                Operations = deleteOps
            };

            var deleteResponse = await _client.BulkAsync(deleteBulkRequest);
            if (!deleteResponse.IsValidResponse)
            {
                throw new Exception($"Bulk delete failed: {deleteResponse.DebugInformation}");
            }
        }
        var indexOps = document
            .Select(doc => new BulkIndexOperation<DocumentChunk>(doc)
            {
                Id = $"{doc.DocId}_{doc.ChunkIndex}",
                Index = _indexKey
            })
            .Cast<IBulkOperation>()
            .ToList();

        var bulkIndexRequest = new BulkRequest
        {
            Operations = indexOps
        };

        var indexResponse = await _client.BulkAsync(bulkIndexRequest);
        if (!indexResponse.IsValidResponse)
        {
            throw new Exception($"Bulk indexing failed: {indexResponse.DebugInformation}");
        }
    }


    public async Task<List<PlagiarismMatch>> CheckSimilarityAsync(float[] queryEmbedding, string docId, int topK = 5)
    {
        
        const float minSimilarityThreshold = 0.9f; 
        
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
                            Dims = 1536,
                            Index = true 
                        }
                    }
                }
            };
        var createResponse = await _client.Indices.CreateAsync(_indexKey, c => c
            .Settings(settings)
            .Mappings(mapping)
        );
        if (!createResponse.IsValidResponse)
        {
            throw new Exception($"Failed to create index: {createResponse.DebugInformation}");
        }
        }
        
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
        
        var docMatches = new Dictionary<string, (List<float> Scores, List<string> Texts)>();
        foreach (var hit in searchResponse.Hits)
        {
            var chunk = hit.Source;
            if (chunk == null) continue;

            var similarity = (float)hit.Score;
            
            // Filter out low similarity matches
            if (similarity < minSimilarityThreshold) continue;

            if (!docMatches.ContainsKey(chunk.DocId))
            {
                docMatches[chunk.DocId] = (new List<float>(), new List<string>());
            }
            docMatches[chunk.DocId].Scores.Add(similarity);
            docMatches[chunk.DocId].Texts.Add(chunk.Content);
        }
        
        var plagiarismMatches = new List<PlagiarismMatch>();
        foreach (var kvp in docMatches)
        {
            var docIdMatch = kvp.Key;
            var scores = kvp.Value.Scores;
            var texts = kvp.Value.Texts;
            
            var avgSimilarity = scores.Average();
            var combinedText = string.Join(" ", texts.Take(1));
            plagiarismMatches.Add(new PlagiarismMatch(
                docIdMatch,
                combinedText,
                (float)Math.Round(avgSimilarity, 4)
            ));
        }
        return plagiarismMatches
            .OrderByDescending(m => m.Similarity)
            .Take(topK)
            .ToList();
    }
}
