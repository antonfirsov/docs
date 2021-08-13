namespace Microsoft.DotNetBlog
{
    internal sealed class VR24_AliasShouldBeValid : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            var alias = context.FrontMatter?.MicrosoftAlias;
            var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.MicrosoftAlias);

            // VR02 ensures there is an alias. We ignore missing alias to avoid double reporting.
            if (alias is null)
                return;

            if (alias.Contains("@"))
                context.Error("VR24", diagnosticSpan, $"The Microsoft alias should not be an email.");

            if (alias.Contains("\\"))
                context.Error("VR24", diagnosticSpan, $"The Microsoft alias should not contain the domain.");           
        }
    }
}
