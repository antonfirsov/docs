namespace Microsoft.DotNetBlog;

internal abstract class ValidationRule
{
    public virtual void Validate(ValidationContext context)
    {
    }

    public virtual Task ValidateAsync(ValidationContext context)
    {
        Validate(context);
        return Task.CompletedTask;
    }
}
