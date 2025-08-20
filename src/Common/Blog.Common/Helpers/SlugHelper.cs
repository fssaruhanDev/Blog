using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Common.Helpers;

public static class SlugHelper
{
    // Basic transliteration for Turkish characters + slug generation
    public static string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Normalize and lower
        var normalized = input.Trim().ToLowerInvariant();

        // Turkish specific replacements
        normalized = normalized.Replace('ç', 'c')
                               .Replace('ğ', 'g')
                               .Replace('ı', 'i')
                               .Replace('İ', 'i')
                               .Replace('ö', 'o')
                               .Replace('ş', 's')
                               .Replace('ü', 'u');

        // Remove diacritics for any remaining characters
        normalized = RemoveDiacritics(normalized);

        // replace spaces and control chars with hyphens
        normalized = Regex.Replace(normalized, "\\s+", "-");
        // remove invalid chars (keep a-z, 0-9, hyphen and underscore)
        normalized = Regex.Replace(normalized, "[^a-z0-9-_]", string.Empty);
        // collapse multiple hyphens
        normalized = Regex.Replace(normalized, "-+", "-");
        // trim hyphens
        normalized = normalized.Trim('-');

        return normalized;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in normalized)
        {
            var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
