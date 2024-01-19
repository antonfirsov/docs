using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

internal sealed class VR03_FirstBlockShouldBeParagraph : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var firstBlock = context.Document.Skip(context.FrontMatter == null ? 0 : 1).FirstOrDefault();
        if (firstBlock != null)
        {
            if (!(firstBlock is ParagraphBlock))
            {
                context.Warning(this.GetType().Name, firstBlock, "First block should be a paragraph with the introduction");
            }
        }
    }
}
