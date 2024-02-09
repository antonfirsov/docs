using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

internal sealed class VR06_FencedCodeBlocksMustHaveLanguage : ValidationRule
{
    private static readonly HashSet<string> _languages = new (
        """

        accesslog,actionscript,apache,apacheconf,arm,armasm,as,asm,atom,avrasm,bash,c,c++,cc,cjs,
        coffee,coffeescript,cpp,cs,csharp,cshtml,cshtml-razor,cson,css,cts,cxx,diff,django,docker,
        dockerfile,dsconfig,f,fs,fsharp,gemspec,gql,graphql,h,h++,haml,hh,hpp,html,http,https,
        hxx,iced,irb,java,javascript,jinja,js,json,jsp,jsx,latex,less,mak,make,makefile,markdown,
        md,mjs,mk,mkd,mkdown,mm,mts,nginx,nginxconf,obj-c,obj-c++,objc,objective-c++,objectivec,patch,
        php,plaintext,plist,podspec,powershell,proto,protobuf,ps,ps1,pwsh,razor,razor-cshtml,rb,rss,
        ruby,scala,scss,sh,sql,svg,swift,tex,text,thor,ts,tsx,txt,typescript,vb,vbnet,vbs,
        vbscript,vbscript-html,vim,wsf,x86asm,xhtml,xjb,xml,xsd,xsl,yaml,yml

        """.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
        StringComparer.OrdinalIgnoreCase);

    public override void Validate(ValidationContext context)
    {
        var blocks = context.Document.Descendants<FencedCodeBlock>().ToList();

        foreach (var block in blocks)
        {
            if (string.IsNullOrEmpty(block.Info))
            {
                context.Error(this.GetType().Name, block, "Fenced code blocks should specify a language");
            }
            else if (!_languages.Contains(block.Info))
            {
                var languageList = string.Join(", ", _languages);
                context.Warning(this.GetType().Name, block, $"Fenced code blocks should specify a language from the following list: {languageList}");
            }
        }
    }
}
