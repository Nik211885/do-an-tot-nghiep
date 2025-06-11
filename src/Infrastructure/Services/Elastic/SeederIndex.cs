using Application.Models;
using CaseConverter;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;

namespace Infrastructure.Services.Elastic;

public class SeederIndex(ElasticsearchClient client)
{
    private readonly ElasticsearchClient _client = client;

    public async Task IndexMappingAsync()
    {
        var indexEmbedding = nameof(ChapterEmbeddingModel).ToKebabCase();
        var index = await  _client.Indices.ExistsAsync(indexEmbedding);
        if (!index.Exists)
        {
            await client.Indices.CreateAsync(indexEmbedding, c => 
                c.Settings(s=>s
                    .NumberOfReplicas(0)
                    .NumberOfShards(1))
                .Mappings(m => m
                    .Properties(new Properties
                    {
                        {nameof(ChapterEmbeddingModel.Id), new KeywordProperty()},
                        {nameof(ChapterEmbeddingModel.BookId), new KeywordProperty()},
                        {nameof(ChapterEmbeddingModel.ChapterId), new KeywordProperty()},
                        {nameof(ChapterEmbeddingModel.ChapterTitle), new TextProperty()},
                        {nameof(ChapterEmbeddingModel.AuthorId), new KeywordProperty()},
                        {nameof(ChapterEmbeddingModel.Content), new TextProperty()},
                        {nameof(ChapterEmbeddingModel.EmbeddingDim), new IntegerNumberProperty()},
                        {nameof(ChapterEmbeddingModel.WordCout), new IntegerNumberProperty()},
                        {nameof(ChapterEmbeddingModel.CharCout), new IntegerNumberProperty()},
                        {nameof(ChapterEmbeddingModel.ModelName), new KeywordProperty()},
                        {nameof(ChapterEmbeddingModel.Embeddings), new DenseVectorProperty
                        {
                            Dims = 1536
                        }},
                    })
                ));
        }
        var bookViewModelIndex = nameof(BookElasticModel).ToKebabCase();
            var bookViewModelIndexExits = await _client.Indices.ExistsAsync(bookViewModelIndex);
            if (!bookViewModelIndexExits.Exists)
            {
                await _client.Indices.CreateAsync(bookViewModelIndex, c=>
                    c.Settings(s=>
                        s.NumberOfReplicas(0)
                            .NumberOfShards(1))
                        .Mappings(m=>m.Properties(
                            new Properties
                            {
                                { nameof(BookElasticModel.AuthorId), new KeywordProperty() },
                                { nameof(BookElasticModel.AuthorName), new TextProperty() },
                                { nameof(BookElasticModel.Id), new KeywordProperty() },
                                { nameof(BookElasticModel.Title), new TextProperty() },
                                { nameof(BookElasticModel.AvatarUrl), new KeywordProperty() },
                                { nameof(BookElasticModel.Description), new TextProperty() },
                                { nameof(BookElasticModel.CreateDateTimeOffset), new DateProperty() },
                                { nameof(BookElasticModel.UpdateDateTimeOffset), new DateProperty() },
                                { nameof(BookElasticModel.IsCompleted), new BooleanProperty() },
                                { nameof(BookElasticModel.Visibility), new BooleanProperty() },
                                { nameof(BookElasticModel.Slug), new KeywordProperty() },
                                { nameof(BookElasticModel.Tags), new KeywordProperty { } },
                                { nameof(BookElasticModel.Price), new DoubleNumberProperty() },
                                { nameof(BookElasticModel.BookPolicy), new IntegerNumberProperty() },
                                { nameof(BookElasticModel.BookReleaseType), new IntegerNumberProperty() },
                                {
                                    nameof(BookElasticModel.Genres), new NestedProperty
                                    {
                                        Properties = new Properties
                                        {
                                            { "id", new KeywordProperty() },
                                            { "name", new TextProperty() },
                                            { "description", new TextProperty() },
                                            { "slug", new KeywordProperty() },
                                            { "avatarUrl", new KeywordProperty() },
                                            { "isActive", new KeywordProperty() },
                                            { "coutBook", new KeywordProperty() },
                                            { "createDateTime", new DateProperty() },
                                            { "lastUpdateTime", new DateProperty() },
                                        }
                                    }
                                }
                            })));
            }
    }
}
