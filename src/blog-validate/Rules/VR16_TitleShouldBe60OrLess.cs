namespace BlogValidator
{
    internal sealed class VR16_TitleShouldBe60OrLess : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (context.FrontMatter != null)
            {
                var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan();

                if (context.FrontMatter.PostTitle?.Length > 60)
                    context.Warning("VR16", diagnosticSpan, "'post_title' should be 60 characters or less");
            }
        }
    }
}
