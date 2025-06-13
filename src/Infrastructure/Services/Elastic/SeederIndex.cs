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
                        .NumberOfShards(1)
                        .Analysis(a => a
                            .CharFilters(cf => cf
                                // Custom char filter để xử lý dấu tiếng Việt
                                .Mapping("vietnamese_char_filter", mcf => mcf
                                    .Mappings([
                                        // Normalize các dấu thanh tiếng Việt
                                        "à=>a", "á=>a", "ả=>a", "ã=>a", "ạ=>a",
                                        "ă=>a", "ằ=>a", "ắ=>a", "ẳ=>a", "ẵ=>a", "ặ=>a",
                                        "â=>a", "ầ=>a", "ấ=>a", "ẩ=>a", "ẫ=>a", "ậ=>a",
                                        "è=>e", "é=>e", "ẻ=>e", "ẽ=>e", "ẹ=>e",
                                        "ê=>e", "ề=>e", "ế=>e", "ể=>e", "ễ=>e", "ệ=>e",
                                        "ì=>i", "í=>i", "ỉ=>i", "ĩ=>i", "ị=>i",
                                        "ò=>o", "ó=>o", "ỏ=>o", "õ=>o", "ọ=>o",
                                        "ô=>o", "ồ=>o", "ố=>o", "ổ=>o", "ỗ=>o", "ộ=>o",
                                        "ơ=>o", "ờ=>o", "ớ=>o", "ở=>o", "ỡ=>o", "ợ=>o",
                                        "ù=>u", "ú=>u", "ủ=>u", "ũ=>u", "ụ=>u",
                                        "ư=>u", "ừ=>u", "ứ=>u", "ử=>u", "ữ=>u", "ự=>u",
                                        "ỳ=>y", "ý=>y", "ỷ=>y", "ỹ=>y", "ỵ=>y",
                                        "đ=>d", "Đ=>D"
                                    ])
                                )
                            )
                            .Analyzers(an => an
                                .Custom("vi_advanced", ca => ca
                                    .CharFilter(["vietnamese_char_filter"])
                                    .Tokenizer("standard")
                                    .Filter(["lowercase", "asciifolding", "word_delimiter"])
                                )
                                .Custom("vi_simple", ca => ca
                                    .CharFilter(["vietnamese_char_filter"])
                                    .Tokenizer("standard")
                                    .Filter(["lowercase", "asciifolding"])
                                )
                                .Custom("vi_search", ca => ca
                                    .CharFilter(["vietnamese_char_filter"])
                                    .Tokenizer("keyword")
                                    .Filter(["lowercase", "asciifolding"])
                                )
                            )
                        ))
                    .Mappings(m=>m.Properties(
                        new Properties
                        {
                            { nameof(BookElasticModel.AuthorId), new KeywordProperty() },
                            { 
                                nameof(BookElasticModel.AuthorName), 
                                new TextProperty()
                                {
                                    Analyzer = "vi_advanced",
                                    SearchAnalyzer = "vi_advanced",
                                    Fields = new Properties
                                    {
                                        { "keyword", new KeywordProperty() },
                                        { "search", new TextProperty() { Analyzer = "vi_search" } }
                                    }
                                } 
                            },
                            { nameof(BookElasticModel.Id), new KeywordProperty() },
                            { 
                                nameof(BookElasticModel.Title), 
                                new TextProperty()
                                {
                                    Analyzer = "vi_advanced",
                                    SearchAnalyzer = "vi_advanced",
                                    Fields = new Properties
                                    {
                                        { "keyword", new KeywordProperty() },
                                        { "search", new TextProperty() { Analyzer = "vi_search" } }
                                    }
                                } 
                            },
                            { nameof(BookElasticModel.AvatarUrl), new KeywordProperty() },
                            { 
                                nameof(BookElasticModel.Description), 
                                new TextProperty()
                                {
                                    Analyzer = "vi_advanced",
                                    SearchAnalyzer = "vi_advanced"
                                } 
                            },
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
                                        { 
                                            "name", 
                                            new TextProperty() 
                                            {
                                                Analyzer = "vi_advanced",
                                                SearchAnalyzer = "vi_advanced",
                                                Fields = new Properties
                                                {
                                                    { "keyword", new KeywordProperty() },
                                                    { "search", new TextProperty() { Analyzer = "vi_search" } }
                                                }
                                            }
                                        },
                                        { "description", new TextProperty() { Analyzer = "vi_advanced" } },
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
