using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Helper;

public static class StringHelperExtension
{
    public static string CreateSlug(this string value)
    {
        int byteRandomSlug = 5;
        string normalized = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in 
                 normalized.Where(c => 
                     CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
        {
            sb.Append(c);
        }
        string slug = sb.ToString().Normalize(NormalizationForm.FormC);
        slug = slug.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); 
        slug = Regex.Replace(slug, @"\s+", "-").Trim('-');
        return string.Concat(slug,"-", RandomStringBase64ByBytes(byteRandomSlug));
    }

    public static string ToKebabCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return str;

        return string.Concat(str.Select((ch, i) =>
            i > 0 && char.IsUpper(ch)
                ? "-" + char.ToLower(ch)
                : char.ToLower(ch).ToString()
        ));
    }

    public static string RandomStringBase64ByBytes(int bytes)
    {
        byte[] byteArray = new byte[bytes];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray).ToLowerInvariant();
    }
    public static string NormalizeSearchString(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        return input.Trim().ToLowerInvariant();
    }

    
}
