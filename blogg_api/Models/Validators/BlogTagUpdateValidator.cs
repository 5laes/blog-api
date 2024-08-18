using blogg_api.Models.DTOs;
using FluentValidation;

namespace blogg_api.Models.Validators
{
    public class BlogTagUpdateValidator : AbstractValidator<BlogTagUpdateDTO>
    {
        public BlogTagUpdateValidator()
        {
            RuleFor(model => model.Id).NotEmpty();
            RuleFor(model => model.TagName).NotEmpty();
        }
    }
}
