using System.Linq;

using Markdig.Syntax;

namespace Microsoft.DotNetBlog
{
    internal sealed class VR02_MetadataRequired : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (context.FrontMatter != null)
            {
                if (string.IsNullOrEmpty(context.FrontMatter.PostTitle))
                    MustSpecifyField(context, BlogFrontMatterFields.PostTitle);

                if (string.IsNullOrEmpty(context.FrontMatter.FeaturedImage))
                    MustSpecifyField(context, BlogFrontMatterFields.FeaturedImage);

                if (string.IsNullOrEmpty(context.FrontMatter.Summary))
                    MustSpecifyField(context, BlogFrontMatterFields.Summary);

                if (string.IsNullOrEmpty(context.FrontMatter.Username))
                    MustSpecifyField(context, BlogFrontMatterFields.Username);

                if (string.IsNullOrEmpty(context.FrontMatter.Categories))
                    MustSpecifyField(context, BlogFrontMatterFields.Categories);

                if (string.IsNullOrEmpty(context.FrontMatter.MicrosoftAlias))
                    MustSpecifyField(context, BlogFrontMatterFields.MicrosoftAlias);

                if (context.FrontMatter.DesiredPublicationDate == null)
                    MustSpecifyField(context, BlogFrontMatterFields.DesiredPublicationDate);

                static void MustSpecifyField(ValidationContext context, string field)
                {
                    context.Error("VR02", context.Document.GetFrontMatterDiagnosticSpan(field), $"Must specify '{field}'");
                }
            }
        }
    }
}
