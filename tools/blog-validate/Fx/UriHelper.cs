using System.Diagnostics.CodeAnalysis;

namespace Microsoft.DotNetBlog;

internal static class UriHelper
{
    public static bool TryGetRelativeOrAbsoluteUri(string text, [MaybeNullWhen(false)] out Uri uri)
    {
        try
        {
            uri = new Uri(text, UriKind.RelativeOrAbsolute);
            return true;
        }
        catch (Exception)
        {
            uri = null;
            return false;
        }
    }

    public static bool TryGetAbsoluteUri(string text, [MaybeNullWhen(false)] out Uri uri)
    {
        try
        {
            var u = new Uri(text, UriKind.RelativeOrAbsolute);
            if (u.IsAbsoluteUri)
            {
                uri = u;
                return true;
            }
        }
        catch (Exception)
        {
        }

        uri = null;
        return false;
    }

    public static bool TryGetRelativeUri(string text, [MaybeNullWhen(false)] out Uri uri)
    {
        try
        {
            var u = new Uri(text, UriKind.RelativeOrAbsolute);
            if (!u.IsAbsoluteUri && !IsAnchor(text))
            {
                uri = u;
                return true;
            }
        }
        catch (Exception)
        {
        }

        uri = null;
        return false;
    }

    private static bool IsAnchor(string text)
    {
        return text.Trim().StartsWith("#");
    }
}
