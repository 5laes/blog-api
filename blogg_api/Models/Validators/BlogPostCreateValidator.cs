using blogg_api.Models.DTOs;
using FluentValidation;

namespace blogg_api.Models.Validators
{
    public class BlogPostCreateValidator : AbstractValidator<BlogPostCreateDTO>
    {
        public BlogPostCreateValidator()
        {
            RuleFor(model => model.ContentId).NotEmpty();
            RuleFor(model => model.TagId).NotEmpty();
        }
    }
}
