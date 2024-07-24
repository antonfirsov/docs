using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog.Rules
{
    internal class VR30_FilenamesCannotContainDot : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            ValidateFileName(context.FileName, context, context.Document);

            var images = context.Document.Descendants<LinkInline>()
                                         .Where(i => i.IsImage);

            foreach (var image in images)
            {
                ValidateFileName(image.Url!, context, image);
            }
        }

        private void ValidateFileName(string fileName, ValidationContext context, MarkdownObject markdownObject)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (!string.IsNullOrEmpty(fileNameWithoutExtension) && fileNameWithoutExtension.Contains("."))
            {
                var suggestedFileName = $"{fileNameWithoutExtension.Replace(".", "_")}{Path.GetExtension(fileName)}";
                context.Error(this.GetType().Name, markdownObject, $"The filename cannot contain a dot ({fileName}).", suggestedFileName);
            }
        }
    }
}
