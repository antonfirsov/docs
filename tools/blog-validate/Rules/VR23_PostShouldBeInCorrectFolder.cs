namespace Microsoft.DotNetBlog;

internal sealed class VR23_PostShouldBeInCorrectFolder : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter?.PostDate is null)
            return;

        var root = context.RootDirectory;
        var date = context.FrontMatter.PostDate.Value;
        var year = date.Year.ToString();
        var month = date.ToString("MM-MMM");
        var name = Path.GetFileNameWithoutExtension(context.FileName);
        var expectedDirectory = Path.Join(root, year, month, name);
        var actualDirectory = Path.GetDirectoryName(context.FileName);

        if (actualDirectory != expectedDirectory)
            context.Error("VR23", context.Document, $"The post should be in directory '{expectedDirectory}'");
    }
}
