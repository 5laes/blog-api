using blogg_api.Models.DTOs;
using FluentValidation;

namespace blogg_api.Models.Validators
{
    public class BlogPostUpdateValidator : AbstractValidator<BlogPostUpdateDTO>
    {
        public BlogPostUpdateValidator()
        {
            RuleFor(model => model.ContentId).NotEmpty();
            RuleFor(model => model.TagId).NotEmpty();
        }
    }
}
