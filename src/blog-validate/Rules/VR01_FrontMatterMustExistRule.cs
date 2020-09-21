namespace BlogValidator
{
    internal sealed class VR01_FrontMatterMustExistRule : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (context.FrontMatter == null)
                context.Error("VR01", context.Document, "Front matter is required, see /template/blank.md");
        }
    }
}
