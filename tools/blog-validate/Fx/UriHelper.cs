using System;

namespace Microsoft.DotNetBlog
{
    internal static class UriHelper
    {
        public static bool TryGetRelativeOrAbsoluteUri(string text, out Uri uri)
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
    }
}
