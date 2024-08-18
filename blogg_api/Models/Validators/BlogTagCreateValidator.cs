using blogg_api.Models.DTOs;
using FluentValidation;

namespace blogg_api.Models.Validators
{
    public class BlogTagCreateValidator : AbstractValidator<BlogTagCreateDTO>
    {
        public BlogTagCreateValidator()
        {
            RuleFor(model => model.TagName).NotEmpty();
        }
    }
}
