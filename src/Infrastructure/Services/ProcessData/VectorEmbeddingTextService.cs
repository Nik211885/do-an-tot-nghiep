using System.Text.Json;
using Application.Interfaces.ProcessData;
using Infrastructure.Helper;

namespace Infrastructure.Services.ProcessData;

public class VectorEmbeddingTextService(IHttpClientFactory factory) 
    : IVectorEmbeddingTextService
{
    private readonly IHttpClientFactory _clientFactory = factory;
    public async Task<List<DocSource>?> GetVectorEmbeddingFromTextAsync(string sentence)
    {
        var url = "jina";
        HttpClient client = _clientFactory.CreateClient(HttpClientKeyFactory.EmbeddingServer);
        var requestData = new { sentence };
        var content = requestData.GetStringContent();
        var response = await client.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DocSource>>(responseBody);
        }
        return null;
    }
}
