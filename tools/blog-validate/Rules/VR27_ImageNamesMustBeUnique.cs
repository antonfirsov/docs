using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR27_ImageNamesMustBeUnique : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var images = context.Document.Descendants<LinkInline>()
                                    .Where(i => i.IsImage);

        var postDirectory = Path.GetDirectoryName(context.FileName)!;
        var parentDirectory = Directory.GetParent(postDirectory) ?? new DirectoryInfo(postDirectory);
        var extensions = new HashSet<string> { ".png", ".gif", ".jpg", ".jpeg", ".webp", ".svg" };
        var allImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var files = Directory.EnumerateFiles(parentDirectory.FullName, "*.*", SearchOption.AllDirectories)
                             .Where(file => extensions.Contains(Path.GetExtension(file)) && Path.GetDirectoryName(file) != postDirectory);

        foreach (var file in files)
        {
            allImages.Add(Path.GetFileName(file));
        }

        foreach (var image in images)
        {
            var imageFileName = Path.GetFileName(image.Url)!;
            if(allImages.Contains(imageFileName))
            {
                context.Error(this.GetType().Name, image, $"The image name '{imageFileName}' is not unique. Image filenames must be unique within all posts in the month.");
            }
        }
    }
}
