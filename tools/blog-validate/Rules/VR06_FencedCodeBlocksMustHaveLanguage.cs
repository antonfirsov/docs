using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

internal sealed class VR06_FencedCodeBlocksMustHaveLanguage : ValidationRule
{
    // Create a string with a pipe delimited list of languages
    private const string _languages = "accesslog,actionscript,apache,armasm,avrasm,bash,c,coffeescript,cpp,csharp,css,diff,django,dockerfile,dsconfig,fsharp,graphql,haml,html,http,java,javascript,json,latex,less,makefile,markdown,nginx,objectivec,php,plaintext,powershell,protobuf,ruby,scala,scss,sql,swift,typescript,vbnet,vbscript,vbscript-html,vim,x86asm,xml,yaml";

    public override void Validate(ValidationContext context)
    {
        var languages = new HashSet<string>(_languages.Split(','));
        var blocks = context.Document.Descendants<FencedCodeBlock>().ToList();

        foreach (var block in blocks)
        {
            if (string.IsNullOrEmpty(block.Info))
            {
                context.Error(this.GetType().Name, block, "Fenced code blocks should specify a language");
            }
            else if (!languages.Contains(block.Info))
            {
                context.Warning(this.GetType().Name, block, $"Fenced code blocks should specify a language from the following list: {_languages}");
            }
        }
    }
}
