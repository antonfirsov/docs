using System.ComponentModel;

using YamlDotNet.Serialization;

namespace Microsoft.DotNetBlog;

public sealed class BlogFrontMatter
{
    public string? PostTitle { get; set; }
    public string? Username { get; set; }
    public string? Categories { get; set; }
    public string? FeaturedImage { get; set; }
    public string? Summary { get; set; }
    public string? MicrosoftAlias { get; set; }

    [YamlMember(Alias = "desired_publication_date")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string? DesiredPublicationDateText { get; set; }

    [YamlIgnore]
    public DateTime? DesiredPublicationDate
    {
        get
        {
            if (!DateTime.TryParse(DesiredPublicationDateText, out var date))
                return null;

            return date;
        }
    }
}
