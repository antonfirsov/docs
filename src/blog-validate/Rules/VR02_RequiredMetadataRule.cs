using System.Linq;

namespace BlogValidator
{
    internal sealed class VR02_RequiredMetadataRule : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (context.FrontMatter != null)
            {
                var block = context.Document.First();

                if (string.IsNullOrEmpty(context.FrontMatter.PostTitle))
                    context.Error("VR02", block, "Must specify post_title");

                if (string.IsNullOrEmpty(context.FrontMatter.Summary))
                    context.Error("VR02", block, "Must specify summary");

                if (string.IsNullOrEmpty(context.FrontMatter.Username))
                    context.Error("VR02", block, "Must specify username");
            }
        }
    }
}
