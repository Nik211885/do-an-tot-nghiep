using System.Text.RegularExpressions;
using Application.Interfaces.ProcessData;
using HtmlAgilityPack;

namespace Infrastructure.Services.ProcessData;

public class CleanTextService : ICleanTextService
{
    public string RemoveHtmlTag(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var text = doc.DocumentNode.InnerText;
        return Regex.Replace(text, @"\s+", " ").Trim();
    }
}
