using System.ComponentModel;

using YamlDotNet.Serialization;

namespace Microsoft.DotNetBlog;

public sealed class BlogFrontMatter
{
    public string? PostTitle { get; set; }
    public string? Author1 { get; set; }
    public string? Author2 { get; set; }
    public string? Author3 { get; set; }
    public string? Tags { get; set; }
    public string? Categories { get; set; }
    public string? FeaturedImage { get; set; }
    public string? Summary { get; set; }
    public string? MicrosoftAlias { get; set; }

    [YamlMember(Alias = "post_date")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string? PostDateText { get; set; }

    [YamlIgnore]
    public DateTime? PostDate
    {
        get
        {
            if (!DateTime.TryParse(PostDateText, out var date))
                return null;

            return date;
        }
    }
}
